@echo off
REM: Command File Created by Microsoft Visual Database Tools
REM: Date Generated: 21/10/2002
REM: Authentication type: SQL Server
REM: Usage: CommandFilename [Server] [Database] [Login] [Password]

if '%1' == '' goto usage
if '%2' == '' goto usage
if '%3' == '' goto usage

if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Area.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.Area" in "dbo.Area.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Clan.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.Clan" in "dbo.Clan.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ClanRank.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.ClanRank" in "dbo.ClanRank.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumActivationType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumActivationType" in "dbo.EnumActivationType.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumCommand.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumCommand" in "dbo.EnumCommand.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumDamageType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumDamageType" in "dbo.EnumDamageType.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumEmote.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumEmote" in "dbo.EnumEmote.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumLiquidType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumLiquidType" in "dbo.EnumLiquidType.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSize.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumSize" in "dbo.EnumSize.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumSkill" in "dbo.EnumSkill.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumSpecialisation" in "dbo.EnumSpecialisation.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTargetType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumTargetType" in "dbo.EnumTargetType.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTerrainType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumTerrainType" in "dbo.EnumTerrainType.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumWearLocation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EnumWearLocation" in "dbo.EnumWearLocation.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EquipableItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.EquipableItem" in "dbo.EquipableItem.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Inventory.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.Inventory" in "dbo.Inventory.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ItemPhysicalObject.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.ItemPhysicalObject" in "dbo.ItemPhysicalObject.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.JunkItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.JunkItem" in "dbo.JunkItem.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasClanRank.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.MobileHasClanRank" in "dbo.MobileHasClanRank.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.MobileHasSkill" in "dbo.MobileHasSkill.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.MobileHasSpecialisation" in "dbo.MobileHasSpecialisation.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePhysicalObject.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.MobilePhysicalObject" in "dbo.MobilePhysicalObject.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePossesableByPlayer.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.MobilePossesableByPlayer" in "dbo.MobilePossesableByPlayer.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObject.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.PhysicalObject" in "dbo.PhysicalObject.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObjectAffectedBySkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.PhysicalObjectAffectedBySkill" in "dbo.PhysicalObjectAffectedBySkill.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Player.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.Player" in "dbo.Player.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.QuaffableItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.QuaffableItem" in "dbo.QuaffableItem.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Race.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.Race" in "dbo.Race.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RaceHasEmote.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.RaceHasEmote" in "dbo.RaceHasEmote.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ReadableItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.ReadableItem" in "dbo.ReadableItem.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RespawnPoint.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.RespawnPoint" in "dbo.RespawnPoint.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SkillEnablesSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.SkillEnablesSkill" in "dbo.SkillEnablesSkill.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.SpecialisationEnablesSkill" in "dbo.SpecialisationEnablesSkill.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.SpecialisationEnablesSpecialisation" in "dbo.SpecialisationEnablesSpecialisation.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.TerrainPhysicalObject.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.TerrainPhysicalObject" in "dbo.TerrainPhysicalObject.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.WieldableItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.WieldableItem" in "dbo.WieldableItem.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.World.tab"
if %ERRORLEVEL% NEQ 0 goto errors
bcp "%2.dbo.World" in "dbo.World.dat" -S %1 -U %3 -P %4 -k -n -q
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Area.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Clan.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ClanRank.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumActivationType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumCommand.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumDamageType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumEmote.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumLiquidType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSize.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTargetType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTerrainType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumWearLocation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EquipableItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Inventory.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ItemPhysicalObject.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.JunkItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasClanRank.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePhysicalObject.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePossesableByPlayer.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObject.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObjectAffectedBySkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Player.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.QuaffableItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Race.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RaceHasEmote.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ReadableItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RespawnPoint.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SkillEnablesSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.TerrainPhysicalObject.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.WieldableItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.World.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Area.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Clan.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ClanRank.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumActivationType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumCommand.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumDamageType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumEmote.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumLiquidType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSize.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTargetType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTerrainType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumWearLocation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EquipableItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Inventory.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ItemPhysicalObject.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.JunkItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasClanRank.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePhysicalObject.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePossesableByPlayer.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObject.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObjectAffectedBySkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Player.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.QuaffableItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Race.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RaceHasEmote.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ReadableItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RespawnPoint.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SkillEnablesSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.TerrainPhysicalObject.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.WieldableItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.World.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Area.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Clan.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ClanRank.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumActivationType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumCommand.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumDamageType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumEmote.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumLiquidType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSize.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTargetType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumTerrainType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EnumWearLocation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.EquipableItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Inventory.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ItemPhysicalObject.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.JunkItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasClanRank.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobileHasSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePhysicalObject.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.MobilePossesableByPlayer.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObject.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObjectAffectedBySkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Player.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.QuaffableItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Race.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RaceHasEmote.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.ReadableItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.RespawnPoint.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SkillEnablesSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.SpecialisationEnablesSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.TerrainPhysicalObject.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.WieldableItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.World.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Equipable.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Junk.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Mobile.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.PhysicalObjectInstance.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Quaffable.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.QuaffableItems.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Terrain.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.Wieldable.viw"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateEquipable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateJunk.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateMirror.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateMobile.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateMobileMirror.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateQuaffable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateReadable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -U %3 -P %4 -b -i "dbo.CreateWieldable.prc"
if %ERRORLEVEL% NEQ 0 goto errors

goto finish

REM: How to use screen
:usage
echo.
echo Usage: MyScript Server Database User [Password]
echo Server: the name of the target SQL Server
echo Database: the name of the target database
echo User: the login name on the target server
echo Password: the password for the login on the target server (optional)
echo.
echo Example: MyScript.cmd MainServer MainDatabase MyName MyPassword
echo.
echo.
goto done

REM: error handler
:errors
echo.
echo WARNING! Error(s) were detected!
echo --------------------------------
echo Please evaluate the situation and, if needed,
echo restart this command file. You may need to
echo supply command parameters when executing
echo this command file.
echo.
pause
goto done

REM: finished execution
:finish
echo.
echo Script execution is complete!
:done
@echo on
