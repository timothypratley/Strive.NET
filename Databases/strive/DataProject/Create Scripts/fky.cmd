@echo off
REM: Command File Created by Microsoft Visual Database Tools
REM: Date Generated: 10/10/2002
REM: Authentication type: Windows NT
REM: Usage: CommandFilename [Server] [Database]

if '%1' == '' goto usage
if '%2' == '' goto usage

if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

 
 
 
osql -S %1 -d %2 -E -b -i "dbo.Area.fky"

osql -S %1 -d %2 -E -b -i "dbo.Clan.fky"

osql -S %1 -d %2 -E -b -i "dbo.ClanRank.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumSize.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.fky"

osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.fky"

osql -S %1 -d %2 -E -b -i "dbo.EquipableItem.fky"

osql -S %1 -d %2 -E -b -i "dbo.Inventory.fky"

osql -S %1 -d %2 -E -b -i "dbo.ItemPhysicalObject.fky"

osql -S %1 -d %2 -E -b -i "dbo.JunkItem.fky"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.fky"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.fky"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.fky"

osql -S %1 -d %2 -E -b -i "dbo.MobilePhysicalObject.fky"

osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.fky"

osql -S %1 -d %2 -E -b -i "dbo.PhysicalObject.fky"

osql -S %1 -d %2 -E -b -i "dbo.PhysicalObjectAffectedBySkill.fky"

osql -S %1 -d %2 -E -b -i "dbo.Player.fky"

osql -S %1 -d %2 -E -b -i "dbo.QuaffableItem.fky"

osql -S %1 -d %2 -E -b -i "dbo.Race.fky"

osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.fky"

osql -S %1 -d %2 -E -b -i "dbo.ReadableItem.fky"

osql -S %1 -d %2 -E -b -i "dbo.RespawnPoint.fky"

osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.fky"

osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.fky"

osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.fky"

osql -S %1 -d %2 -E -b -i "dbo.TerrainPhysicalObject.fky"

osql -S %1 -d %2 -E -b -i "dbo.WieldableItem.fky"

osql -S %1 -d %2 -E -b -i "dbo.World.fky"

osql -S %1 -d %2 -E -b -i "dbo.Area.ext"

osql -S %1 -d %2 -E -b -i "dbo.Clan.ext"

osql -S %1 -d %2 -E -b -i "dbo.ClanRank.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumSize.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.ext"

osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.ext"

osql -S %1 -d %2 -E -b -i "dbo.EquipableItem.ext"

osql -S %1 -d %2 -E -b -i "dbo.Inventory.ext"

osql -S %1 -d %2 -E -b -i "dbo.ItemPhysicalObject.ext"

osql -S %1 -d %2 -E -b -i "dbo.JunkItem.ext"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.ext"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.ext"

osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.ext"

osql -S %1 -d %2 -E -b -i "dbo.MobilePhysicalObject.ext"

osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.ext"

osql -S %1 -d %2 -E -b -i "dbo.PhysicalObject.ext"

osql -S %1 -d %2 -E -b -i "dbo.PhysicalObjectAffectedBySkill.ext"

osql -S %1 -d %2 -E -b -i "dbo.Player.ext"

osql -S %1 -d %2 -E -b -i "dbo.QuaffableItem.ext"

osql -S %1 -d %2 -E -b -i "dbo.Race.ext"

osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.ext"

osql -S %1 -d %2 -E -b -i "dbo.ReadableItem.ext"

osql -S %1 -d %2 -E -b -i "dbo.RespawnPoint.ext"

osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.ext"

osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.ext"

osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.ext"

osql -S %1 -d %2 -E -b -i "dbo.TerrainPhysicalObject.ext"

osql -S %1 -d %2 -E -b -i "dbo.WieldableItem.ext"

osql -S %1 -d %2 -E -b -i "dbo.World.ext"


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
