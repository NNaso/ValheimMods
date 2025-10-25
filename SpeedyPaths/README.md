**Gives more purpose to paths beyond cosmetics.**  
![demogif](https://i.imgur.com/yNv1TS9.gif)  
Paths and constructions (floors, etc) apply a modifier to player jog and sprint speed while traveling on them.  
Sprinting also has it's stamina consumption reduced while on paths or floors.  
  
Works on most constructions, want a path made of Kilns? enjoy your speed bonus.  
****Works with carts.**  
  
Supports speed/stamina modifiers on uncleared ground tuned per biome! Make swamps even more painful to traverse!  
By default no modifiers are applied to uncleared terrain.  
  
Displays a status indicator when boosts are being applied. Currently has 2 buff and 2 debuff icons. The thresholds for each icon can be tuned in the config.  
**Default config does not add any debuffs.**

![speedy icons](https://i.imgur.com/1DN5URT.png)

## **NEW IN 1.0.4 => Server Config Sync**

Clients connecting to servers running Speedy Paths will now sync their configs with the server automatically.  
Forcibly Syncs Buff Values, Status Effect Icon Thresholds, String Configs.  
The synced server values are not easily accessible via clientside config editors.  

Can be disabled in the server config.

## Configuration and Installation
Speed and stamina modifier for each surface are fully tune-able in ***/BepInEx/config/nex.speedypaths.cfg***  
  
Install to **/BepInEx/plugins** folder.

## Config

    [Hud]

    ## Show surface type text as status effect name
    # Setting type: Boolean
    # Default value: true
    EnableDynamicStatusText = true

    ## Show the status effect buff/debuff %
    # Setting type: Boolean
    # Default value: true
    ShowStatusEffectPercent = true

    ## Show the speedypaths status icon in the hud
    # Setting type: Boolean
    # Default value: true
    ShowStatusIcon = true

    ## Text shown above the status icon
    # Setting type: String
    # Default value: Speedy Path
    StatusText = Speedy Path

    ## Speed buff threshold to show lvl 1 buff icon
    # Setting type: Single
    # Default value: 1
    BuffIcon +1 = 1

    ## Speed buff threshold to show lvl 2 buff icon
    # Setting type: Single
    # Default value: 1.39
    BuffIcon +2 = 1.39

    ## Speed buff threshold to show lvl 1 debuff icon
    # Setting type: Single
    # Default value: 1
    DebuffIcon -1 = 1

    ## Speed buff threshold to show lvl 2 debuff icon
    # Setting type: Single
    # Default value: 0.79
    DebuffIcon -2 = 0.79

    [ServerAuthoritativeConfig]

    ## <Server Only> Forces Clients to use Server defined configs.
    # Setting type: Boolean
    # Default value: true
    ServerIsAuthoritative = true

    [SpeedModifiers]

    ## Modifier for speed while on dirt paths
    # Setting type: Single
    # Default value: 1.15
    DirtPathSpeed = 1.15

    ## Modifier for speed while on stone paths
    # Setting type: Single
    # Default value: 1.4
    StonePathSpeed = 1.4

    ## Modifier for speed while on Cultivated land
    # Setting type: Single
    # Default value: 1
    CultivatedSpeed = 1

    ## Modifier for speed while on wood structures
    # Setting type: Single
    # Default value: 1.15
    StructureWoodSpeed = 1.15

    ## Modifier for speed while on core wood structures
    # Setting type: Single
    # Default value: 1.15
    StructureHardWoodSpeed = 1.15

    ## Modifier for speed while on stone structures
    # Setting type: Single
    # Default value: 1.4
    StructureStoneSpeed = 1.4

    ## Modifier for speed while on ironwood structures
    # Setting type: Single
    # Default value: 1.4
    StructureIronSpeed = 1.4

    [SpeedModifiers_Biomes]

    ## Speed modifier for uncleared ground in None Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_None_Speed = 1

    ## Speed modifier for uncleared ground in Meadows Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Meadows_Speed = 1

    ## Speed modifier for uncleared ground in Swamp Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Swamp_Speed = 1

    ## Speed modifier for uncleared ground in Mountain Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Mountain_Speed = 1

    ## Speed modifier for uncleared ground in BlackForest Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_BlackForest_Speed = 1

    ## Speed modifier for uncleared ground in Plains Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Plains_Speed = 1

    ## Speed modifier for uncleared ground in AshLands Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_AshLands_Speed = 1

    ## Speed modifier for uncleared ground in DeepNorth Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_DeepNorth_Speed = 1

    ## Speed modifier for uncleared ground in Ocean Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Ocean_Speed = 1

    ## Speed modifier for uncleared ground in Mistlands Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Mistlands_Speed = 1

    [StaminaModifiers]

    ## Modifier for stamina while on dirt paths
    # Setting type: Single
    # Default value: 0.8
    DirtPathStamina = 0.8

    ## Modifier for stamina while on stone paths
    # Setting type: Single
    # Default value: 0.7
    StonePathStamina = 0.7

    ## Modifier for stamina while on Cultivated land
    # Setting type: Single
    # Default value: 1
    CultivatedStamina = 1

    ## Modifier for stamina while on wood structures
    # Setting type: Single
    # Default value: 0.8
    StructureWoodStamina = 0.8

    ## Modifier for stamina while on core wood structures
    # Setting type: Single
    # Default value: 0.8
    StructureHardWoodStamina = 0.8

    ## Modifier for stamina while on stone structures
    # Setting type: Single
    # Default value: 0.7
    StructureStoneStamina = 0.7

    ## Modifier for stamina while on ironwood structures
    # Setting type: Single
    # Default value: 0.7
    StructureIronStamina = 0.7

    [StaminaModifiers_Biomes]

    ## Stamina modifier for uncleared ground in None Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_None_Stamina = 1

    ## Stamina modifier for uncleared ground in Meadows Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Meadows_Stamina = 1

    ## Stamina modifier for uncleared ground in Swamp Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Swamp_Stamina = 1

    ## Stamina modifier for uncleared ground in Mountain Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Mountain_Stamina = 1

    ## Stamina modifier for uncleared ground in BlackForest Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_BlackForest_Stamina = 1

    ## Stamina modifier for uncleared ground in Plains Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Plains_Stamina = 1

    ## Stamina modifier for uncleared ground in AshLands Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_AshLands_Stamina = 1

    ## Stamina modifier for uncleared ground in DeepNorth Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_DeepNorth_Stamina = 1

    ## Stamina modifier for uncleared ground in Ocean Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Ocean_Stamina = 1

    ## Stamina modifier for uncleared ground in Mistlands Biomes
    # Setting type: Single
    # Default value: 1
    Untamed_Mistlands_Stamina = 1

    [Strings]

    ## Dynamic status mapping for None groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_None = default

    ## Dynamic status mapping for Meadows groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Meadows = default

    ## Dynamic status mapping for Swamp groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Swamp = default

    ## Dynamic status mapping for Mountain groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Mountain = default

    ## Dynamic status mapping for BlackForest groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_BlackForest = default

    ## Dynamic status mapping for Plains groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Plains = default

    ## Dynamic status mapping for AshLands groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_AshLands = default

    ## Dynamic status mapping for DeepNorth groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_DeepNorth = default

    ## Dynamic status mapping for Ocean groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Ocean = default

    ## Dynamic status mapping for Mistlands groundcover. 'default' uses localized biome name.
    # Setting type: String
    # Default value: default
    Biome_Mistlands = default

    ## Dynamic status mapping for dirt paths
    # Setting type: String
    # Default value: Dirt Path
    PathDirt = Dirt Path

    ## Dynamic status mapping for  stone paths
    # Setting type: String
    # Default value: Stone Path
    PathStone = Stone Path

    ## Dynamic status mapping for Cultivated land
    # Setting type: String
    # Default value: Cultivated
    Cultivated = Cultivated

    ## Dynamic status mapping for wood structures
    # Setting type: String
    # Default value: Wood
    StructureWood = Wood

    ## Dynamic status mapping for core wood structures
    # Setting type: String
    # Default value: Hardwood
    StructureHardWood = Hardwood

    ## MDynamic status mapping for stone structures
    # Setting type: String
    # Default value: Stone
    StructureStone = Stone

    ## Dynamic status mapping for ironwood structures
    # Setting type: String
    # Default value: Iron
    StructureIron = Iron

## Download
https://www.nexusmods.com/valheim/mods/452

https://valheim.thunderstore.io/package/Nextek/SpeedyPaths/

## Source
https://github.com/NNaso/ValheimMods/tree/main/SpeedyPaths

## Changelog
**Version 1.0.9**  
- Added support for Grausten structures

**Version 1.0.8**  
- Updated for Valheim Patch 0.216.9

**Version 1.0.7**  
- Added Support for black marble structures

**Version 1.0.6**  
- Fix for SpeedyPaths not working on linux clients.
- Performance fixes and 2 new configs for performance.

**Version 1.0.5**  
- Support for Game Update 0.150.3

**Version 1.0.4**  
- Speedy Paths Server Config Sync. Client will Sync to server config if server is running Speedy Paths.
- Smoother road detection sampling an average. This should make patchy roads more uniform (preferring stone)
- Config options for setting default Biome groundcover string in status effect icon. (ie. Change Meadows to Grass, Swamp to Mud, etc)
- Status effect icon now shows buff/debuff % (can hide in config)

**Version 1.0.3**  
- Fix Cultivated stamina modifier listed under wrong header.
- Added 2 debuff status icons.
- Expose status effect icon thresholds in config file.
- Add Dynamic status effect name (default on). Replaces static "Speedy Path" effect with ground type.
- Prioritize stone pathways over blended dirt pathways.
- Fuzzier path detection.
    
**Version 1.0.2**  
- Add config option for status effect name. Change the text "Speed Path" or hide it entirely!
- Add Support for Cultivated Land.
- Add Support for Stamina/Speed buffs/debuffs on uncleared ground, per biome!
- Fix status effect showing up while swimming.
    
**Version 1.0.1**  
- Refactor status effect icon injection. Should now work with mods that modify status effect icons.
    
**Version 1.0.0**  
- Initial Release
