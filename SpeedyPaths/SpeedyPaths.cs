using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using AuthoritativeConfig;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

namespace SpeedyPaths
{
    [BepInPlugin("nex.SpeedyPaths", "Speedy Paths Mod", "1.0.7")]
    public class SpeedyPathsClientMod : BaseUnityPlugin { 
        public enum GroundType
        {
            Untamed,
            PathDirt,
            PathStone,
            Cultivated,
            StructureWood,
            StructureHardWood,
            StructureStone,
            StructureIron
        }
        private static Dictionary<GroundType, ConfigEntry<float>> _speedModifiers = new Dictionary<GroundType, ConfigEntry<float>>();
        private static Dictionary<GroundType, ConfigEntry<float>> _staminaModifiers = new Dictionary<GroundType, ConfigEntry<float>>();
        private static Dictionary<Heightmap.Biome, ConfigEntry<float>> _untamedSpeedModifiers = new Dictionary<Heightmap.Biome, ConfigEntry<float>>();
        private static Dictionary<Heightmap.Biome, ConfigEntry<float>> _untamedStaminaModifiers = new Dictionary<Heightmap.Biome, ConfigEntry<float>>();
        private static ConfigEntry<bool> _showHudStatus;
        private static ConfigEntry<string> _hudStatusText;
        private static ConfigEntry<bool> _hudDynamicStatusText;
        private static ConfigEntry<bool> _hudShowEffectPercent;
        private static ConfigEntry<bool> _applyModifierToEncumbered;
        private static ConfigEntry<bool> _applyModifierToSneak;
        private static List<ConfigEntry<float>> _hudPosIconThresholds = new List<ConfigEntry<float>>();
        private static List<ConfigEntry<float>> _hudNegIconThresholds = new List<ConfigEntry<float>>();
        private static ConfigEntry<float> _groundSensorUpdateInterval;
        private static ConfigEntry<int> _groundSensorRadius;
        private static ConfigEntry<bool> _printGroundSensorLogs;

        //Display Strings
        private static Dictionary<GroundType, ConfigEntry<string>> _groundTypeStrings = new Dictionary<GroundType, ConfigEntry<string>>();
        private static Dictionary<Heightmap.Biome, ConfigEntry<string>> _biomeTypeStrings = new Dictionary<Heightmap.Biome, ConfigEntry<string>>();

        private static int m_pieceLayer;
        private static object[] _worldToVertexArgs = new object[]{ Vector3.zero, null, null };
        private static List<Sprite> _speedSprites = new List<Sprite>();
        AssetBundle _speedyassets;

        private static float m_sensorTime;
        private static GroundType m_cachedGroundType;

        public new static BepInEx.Logging.ManualLogSource Logger;

        public new AuthoritativeConfig.Config Config
        {
            get { return AuthoritativeConfig.Config.Instance; }
            set {}
        }
        
        void Awake() {
            Logger = base.Logger;

            _speedyassets = AssetBundle.LoadFromMemory(Properties.Resources.speedyassets);
            if (!_speedyassets)
            {
                Logger.LogError($"Failed to read AssetBundle stream");
                return;
            }

            Config.init( this, true );

            _hudDynamicStatusText = Config.Bind("Hud", "EnableDynamicStatusText", true, "Show surface type text as status effect name", false);
            _hudShowEffectPercent = Config.Bind("Hud", "ShowStatusEffectPercent", true, "Show the status effect buff/debuff %", false);
            _showHudStatus = Config.Bind("Hud", "ShowStatusIcon", true, "Show the speedypaths status icon in the hud", false);
            _hudStatusText = Config.Bind("Hud", "StatusText", "Speedy Path", "Text shown above the status icon", false);
            _hudPosIconThresholds.Add( Config.Bind("Hud", "BuffIcon +1", 1.0f, "Speed buff threshold to show lvl 1 buff icon") );
            _hudPosIconThresholds.Add( Config.Bind("Hud", "BuffIcon +2", 1.39f, "Speed buff threshold to show lvl 2 buff icon") );
            _hudNegIconThresholds.Add( Config.Bind("Hud", "DebuffIcon -1", 1.0f, "Speed buff threshold to show lvl 1 debuff icon") );
            _hudNegIconThresholds.Add( Config.Bind("Hud", "DebuffIcon -2", 0.79f, "Speed buff threshold to show lvl 2 debuff icon") );

            _groundSensorUpdateInterval = Config.Bind("Performance", "Ground Sensor Interval", 1.0f, "Interval between in seconds between ground type checks. Lower number, more accute ground detection.", true);
            _groundSensorRadius = Config.Bind("Performance", "Ground Sensor Radius", 1, "Radius of ground pixels to sample when checking under the player", true);
            _printGroundSensorLogs = Config.Bind("Debug", "Ground Sensor Logs", false, "Prints the ground sensor debug info to logs. Very verbose.", false);

            _applyModifierToEncumbered = Config.Bind("Misc", "Apply Modifier to Encumbered", true, "Applies the current speed modifier to the player in an encumbered state", true);
            _applyModifierToSneak = Config.Bind("Misc", "Apply Modifier to Sneak", true, "Applies the current speed modifier to the player in an crouched/sneaking state", true);

            _speedModifiers[GroundType.PathDirt] = Config.Bind("SpeedModifiers", "DirtPathSpeed", 1.15f, "Modifier for speed while on dirt paths");
            _speedModifiers[GroundType.PathStone] = Config.Bind("SpeedModifiers", "StonePathSpeed", 1.4f, "Modifier for speed while on stone paths");
            _speedModifiers[GroundType.Cultivated] = Config.Bind("SpeedModifiers", "CultivatedSpeed", 1.0f, "Modifier for speed while on Cultivated land");
            _speedModifiers[GroundType.StructureWood] = Config.Bind("SpeedModifiers", "StructureWoodSpeed", 1.15f, "Modifier for speed while on wood structures");
            _speedModifiers[GroundType.StructureHardWood] = Config.Bind("SpeedModifiers", "StructureHardWoodSpeed", 1.15f, "Modifier for speed while on core wood structures");
            _speedModifiers[GroundType.StructureStone] = Config.Bind("SpeedModifiers", "StructureStoneSpeed", 1.4f, "Modifier for speed while on stone structures");
            _speedModifiers[GroundType.StructureIron] = Config.Bind("SpeedModifiers", "StructureIronSpeed", 1.4f, "Modifier for speed while on ironwood structures");

            _staminaModifiers[GroundType.PathDirt] = Config.Bind("StaminaModifiers", "DirtPathStamina", 0.8f, "Modifier for stamina while on dirt paths");
            _staminaModifiers[GroundType.PathStone] = Config.Bind("StaminaModifiers", "StonePathStamina", 0.7f, "Modifier for stamina while on stone paths");
            _staminaModifiers[GroundType.Cultivated] = Config.Bind("StaminaModifiers", "CultivatedStamina", 1.0f, "Modifier for stamina while on Cultivated land");
            _staminaModifiers[GroundType.StructureWood] = Config.Bind("StaminaModifiers", "StructureWoodStamina", 0.8f, "Modifier for stamina while on wood structures");
            _staminaModifiers[GroundType.StructureHardWood] = Config.Bind("StaminaModifiers", "StructureHardWoodStamina", 0.8f, "Modifier for stamina while on core wood structures");
            _staminaModifiers[GroundType.StructureStone] = Config.Bind("StaminaModifiers", "StructureStoneStamina", 0.7f, "Modifier for stamina while on stone structures");
            _staminaModifiers[GroundType.StructureIron] = Config.Bind("StaminaModifiers", "StructureIronStamina", 0.7f, "Modifier for stamina while on ironwood structures");
            
            //Should handle new biomes automagically
            foreach(Heightmap.Biome biome in Enum.GetValues(typeof(Heightmap.Biome)))
            {
                if(biome == Heightmap.Biome.BiomesMax)
                {
                    break;
                }
                _untamedSpeedModifiers[biome] = Config.Bind("SpeedModifiers_Biomes", "Untamed_" + biome.ToString() + "_Speed", 1.0f, "Speed modifier for uncleared ground in " + biome.ToString() + " Biomes");
                _untamedStaminaModifiers[biome] = Config.Bind("StaminaModifiers_Biomes", "Untamed_" + biome.ToString()+ "_Stamina", 1.0f, "Stamina modifier for uncleared ground in " + biome.ToString() + " Biomes");
                _biomeTypeStrings[biome] = Config.Bind("Strings", "Biome_"+biome.ToString(), "default", "Dynamic status mapping for "+ biome.ToString() +" groundcover. 'default' uses localized biome name.");
            }

            _groundTypeStrings[GroundType.PathDirt] = Config.Bind("Strings", "PathDirt", "Dirt Path", "Dynamic status mapping for dirt paths");
            _groundTypeStrings[GroundType.PathStone] = Config.Bind("Strings", "PathStone", "Stone Path", "Dynamic status mapping for  stone paths");
            _groundTypeStrings[GroundType.Cultivated] = Config.Bind("Strings", "Cultivated", "Cultivated", "Dynamic status mapping for Cultivated land");
            _groundTypeStrings[GroundType.StructureWood] = Config.Bind("Strings", "StructureWood", "Wood", "Dynamic status mapping for wood structures");
            _groundTypeStrings[GroundType.StructureHardWood] = Config.Bind("Strings", "StructureHardWood", "Hardwood", "Dynamic status mapping for core wood structures");
            _groundTypeStrings[GroundType.StructureStone] = Config.Bind("Strings", "StructureStone", "Stone", "MDynamic status mapping for stone structures");
            _groundTypeStrings[GroundType.StructureIron] = Config.Bind("Strings", "StructureIron", "Iron", "Dynamic status mapping for ironwood structures");

            if (m_pieceLayer == 0)
            {
                m_pieceLayer = LayerMask.NameToLayer("piece");
            }

            _speedSprites.Add( _speedyassets.LoadAsset<Sprite>("assets/nex.speedypaths/speedypaths_1.png") );
            _speedSprites.Add( _speedyassets.LoadAsset<Sprite>("assets/nex.speedypaths/speedypaths_2.png") );
            _speedSprites.Add( _speedyassets.LoadAsset<Sprite>("assets/nex.speedypaths/speedypaths_n1.png") );
            _speedSprites.Add( _speedyassets.LoadAsset<Sprite>("assets/nex.speedypaths/speedypaths_n2.png") );

            m_sensorTime = 0f;
            m_cachedGroundType = GroundType.Untamed;

            Harmony.CreateAndPatchAll(typeof(SpeedyPathsClientMod));
        }

        private static float _activeSpeedModifier = 1.0f;
        private static float _activeStaminaModifier = 1.0f;
        private static String _activeStatusText;
        private static Sprite _activeStatusSprite;

        //hooks
        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        [HarmonyPrefix]
        static void UpdateModifiers( Player __instance )
        {
            m_sensorTime -= Time.fixedDeltaTime;
            if( Player.m_localPlayer == __instance && !__instance.IsDead() )
            {
                UpdateGroundTypeCache( __instance );
                _activeSpeedModifier = GetSpeedyPathModifier( __instance );
                _activeStaminaModifier = GetSpeedyPathStaminaModifier(__instance);

                if( _activeSpeedModifier > 1.0f )
                {
                    for( int i = 0; i < _hudPosIconThresholds.Count; ++i )
                    {
                        if( _activeSpeedModifier > _hudPosIconThresholds[i].Value )
                        {
                            _activeStatusSprite = _speedSprites[i];
                        }
                    }
                }
                else
                {
                    for( int i = 0; i < _hudNegIconThresholds.Count; ++i )
                    {
                        if( _activeSpeedModifier < _hudNegIconThresholds[i].Value )
                        {
                            _activeStatusSprite = _speedSprites[_hudPosIconThresholds.Count + i];
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), "CheckRun")]
        [HarmonyPrefix]
        static void CheckRunPrefixStaminaMod( Player __instance, out float __state )
        {
            __state = __instance.m_runStaminaDrain;
            __instance.m_runStaminaDrain *= _activeStaminaModifier;
        }
        [HarmonyPatch(typeof(Player), "CheckRun")]
        [HarmonyPostfix]
        static void CheckRunPostfixStaminaMod( Player __instance, float __state )
        {
            __instance.m_runStaminaDrain = __state;
        }

        [HarmonyPatch(typeof(Character), "UpdateWalking")]
        [HarmonyPrefix]
        static void UpdateWalkingPrefixMod( Character __instance, out float __state )
        {
            __state = __instance.m_crouchSpeed;
            if((__instance.IsEncumbered() && _applyModifierToEncumbered.Value) 
                || (__instance.IsSneaking() && _applyModifierToSneak.Value))
            {
                __instance.m_crouchSpeed *= _activeSpeedModifier;
            }
        }
        [HarmonyPatch(typeof(Character), "UpdateWalking")]
        [HarmonyPostfix]
        static void UpdateWalkingPostfixMod( Character __instance, float __state )
        {
            __instance.m_crouchSpeed = __state;

        }

        [HarmonyPatch(typeof(Player), "GetJogSpeedFactor")]
        [HarmonyPostfix]
        static void JogSpeedPathFactor( Player __instance, ref float __result )
        {
            __result *= _activeSpeedModifier;
        }

        [HarmonyPatch(typeof(Player), "GetRunSpeedFactor")]
        [HarmonyPostfix]
        static void RunSpeedPathFactor( Player __instance, ref float __result )
        {
            __result *= _activeSpeedModifier;
        }

        private static StatusEffect _pathBuffSEDummy;
        private static float _prevModValue = 1.0f;
        [HarmonyPatch(typeof(Hud), "UpdateStatusEffects")]
        [HarmonyPrefix]
        static void UpdatePathIcon( Hud __instance, List<StatusEffect> statusEffects )
        {
            if( !Player.m_localPlayer.IsDead() && _showHudStatus.Value && _activeSpeedModifier != 1.0f )
            {
                if( _pathBuffSEDummy == null)
                {
                    _pathBuffSEDummy = ScriptableObject.CreateInstance(typeof(StatusEffect)) as StatusEffect;
                }
                _pathBuffSEDummy.m_name = _hudDynamicStatusText.Value ? _activeStatusText : _hudStatusText.Value;
                _pathBuffSEDummy.m_icon = _activeStatusSprite;

                if( _activeSpeedModifier != 1.0f )
                {
                    if( _activeSpeedModifier != _prevModValue )
                    {
                        _pathBuffSEDummy.m_isNew = true;
                    }
                    statusEffects.Add(_pathBuffSEDummy);
                }
                _prevModValue = _activeSpeedModifier;
            }
        }

        [HarmonyPatch(typeof(StatusEffect), "GetIconText")]
        [HarmonyPostfix]
        static void SpeedyPathsStatusEffect_GetIconText( StatusEffect __instance, ref string __result )
        {
            if( __instance == _pathBuffSEDummy && _hudShowEffectPercent.Value )
            {
                __result = (_activeSpeedModifier - 1.0f).ToString("P0");
            }
        }

        //util funcs
        static float GetSpeedyPathModifier( Player player )
        {
            if( !player.IsSwiming() && !player.InInterior() )
            {
                if( _speedModifiers.ContainsKey(m_cachedGroundType) )
                {
                    _activeStatusText = _groundTypeStrings[m_cachedGroundType].Value;
                    return _speedModifiers[m_cachedGroundType].Value;
                }
                //fallback to biome speed
                Heightmap.Biome playerBiome = player.GetCurrentBiome();
                //Handle new biomes "gracefully"
                if( !_untamedSpeedModifiers.ContainsKey(playerBiome) )
                {
                    Logger.LogWarning( $"New biome {playerBiome.ToString()}. Unsure how to Handle. Falling back to None." );
                    playerBiome = Heightmap.Biome.None;
                }
                _activeStatusText = "$biome_" + playerBiome.ToString().ToLower();
                if( _biomeTypeStrings.ContainsKey(playerBiome) && _biomeTypeStrings[playerBiome].Value != "default" )
                {
                    _activeStatusText = _biomeTypeStrings[playerBiome].Value;
                }
                return _untamedSpeedModifiers[playerBiome].Value;
            }
            return 1.0f;
        }

        static float GetSpeedyPathStaminaModifier( Player player )
        {
            if( !player.IsSwiming() && !player.InInterior() )
            {
                if( _staminaModifiers.ContainsKey(m_cachedGroundType) )
                {
                    return _staminaModifiers[m_cachedGroundType].Value;
                }
                //fallback to biome stamina
                Heightmap.Biome playerBiome = player.GetCurrentBiome();
                //Handle new biomes "gracefully"
                if( !_untamedStaminaModifiers.ContainsKey(playerBiome) )
                {
                    Logger.LogWarning( $"New biome {playerBiome.ToString()}. Unsure how to Handle. Falling back to None." );
                    playerBiome = Heightmap.Biome.None;
                }
                return _untamedStaminaModifiers[playerBiome].Value;
            }
            return 1.0f;
        }

        static void UpdateGroundTypeCache( Player player )
        {
            if(m_sensorTime > 0)
            {
                return;
            }
            m_sensorTime = _groundSensorUpdateInterval.Value;
            Collider lastGroundCollider = player.GetLastGroundCollider();
            if ((bool)lastGroundCollider)
            {
                //this is extremely generous, a kiln road will work...
                //todo: piece whitelist / blacklist?
                //Do this check first, so we can potentially break out from expensive terrain check
                if (lastGroundCollider.gameObject.layer == m_pieceLayer)
                {
                    WearNTear componentInParent = lastGroundCollider.GetComponentInParent<WearNTear>();
                    if ((bool)componentInParent)
                    {
                        switch (componentInParent.m_materialType)
                        {
                        case WearNTear.MaterialType.Wood:
                            m_cachedGroundType = GroundType.StructureWood;
                            return;
                        case WearNTear.MaterialType.Stone:
                            m_cachedGroundType = GroundType.StructureStone;
                            return;
                        case WearNTear.MaterialType.HardWood:
                            m_cachedGroundType = GroundType.StructureHardWood;
                            return;
                        case WearNTear.MaterialType.Iron:
                            m_cachedGroundType = GroundType.StructureIron;
                            return;
                        }
                    }
                }

                Heightmap hmap = lastGroundCollider.GetComponent<Heightmap>();
                if (hmap != null)
                {
                    Texture2D paintMask = Traverse.Create(hmap).Field("m_paintMask").GetValue() as Texture2D;
                    _worldToVertexArgs[0] = Traverse.Create(player).Field("m_lastGroundPoint").GetValue() as Vector3?;
                    AccessTools.Method(typeof(Heightmap), "WorldToVertex").Invoke(hmap, _worldToVertexArgs);
                    int sample_radius = _groundSensorRadius.Value;
                    Color hmcl_pixel = new Color();

                    //optimize ground sampling into a single call to getPixels
                    //These are stitched together, we only check the Heightmap the player is currently on. So we need to be mindful of the edges.
                    int x_range_min = Math.Max((int)_worldToVertexArgs[1] - sample_radius, 0);
                    int x_range_max = sample_radius * 2;
                    if(x_range_max + (int)_worldToVertexArgs[1] >= paintMask.width)
                    {
                        x_range_max = paintMask.width - x_range_min - 1;
                    }
                    int y_range_min = Math.Max((int)_worldToVertexArgs[2] - sample_radius, 0);
                    int y_range_max = sample_radius * 2;
                    if(y_range_max + (int)_worldToVertexArgs[2] >= paintMask.height)
                    {
                        y_range_max = paintMask.height - y_range_min - 1;
                    }

                    if(_printGroundSensorLogs.Value) 
                    { 
                        Logger.LogInfo($@"Fetching Pixel range x: {x_range_min.ToString()} + {x_range_max.ToString()}  
                                                            y: {y_range_min.ToString()} + {y_range_max.ToString()}"); 
                    }
                    var samples = paintMask.GetPixels(x_range_min, y_range_min, x_range_max, y_range_max, 0);
                    foreach(var pixel in samples)
                    {
                        hmcl_pixel += pixel;
                        if(_printGroundSensorLogs.Value) { Logger.LogInfo($"Sample: {pixel.ToString()}"); }
                    }

                    hmcl_pixel = new Color(hmcl_pixel.r / samples.Length, hmcl_pixel.g / samples.Length, hmcl_pixel.b / samples.Length);
                    if(_printGroundSensorLogs.Value) 
                    { 
                        Logger.LogInfo($"Avg ground pixel {hmcl_pixel.ToString()} from {samples.ToString()} Samples"); 
                    }

                    //Single Pixel
                    //Color hmcl_pixel = paintMask.GetPixel( (int)_worldToVertexArgs[1], (int)_worldToVertexArgs[2] );
                    if( hmcl_pixel.b > 0.4f )
                    {
                        m_cachedGroundType = GroundType.PathStone;
                        return;
                    }
                    else if( hmcl_pixel.r > 0.4f )
                    {
                        m_cachedGroundType = GroundType.PathDirt;
                        return;
                    }
                    else if( hmcl_pixel.g > 0.4f )
                    {
                        m_cachedGroundType = GroundType.Cultivated;
                        return;
                    }
                }
            }
            m_cachedGroundType = GroundType.Untamed;
        }
    }
}
