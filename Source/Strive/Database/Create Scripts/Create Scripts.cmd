@echo off
REM: Command File Created by Microsoft Visual Database Tools
REM: Date Generated: 30/05/2004
REM: Authentication type: Windows NT
REM: Usage: CommandFilename [Server] [Database]

if '%1' == '' goto usage
if '%2' == '' goto usage

if '%1' == '/?' goto usage
if '%1' == '-?' goto usage
if '%1' == '?' goto usage
if '%1' == '/help' goto usage

osql -S %1 -d %2 -E -b -i "dbo.Area.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Clan.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ClanRank.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumItemDurability.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileSize.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileState.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumRace.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumResourceType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSex.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWeaponSize.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearSlot.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Inventory.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ObjectInstance.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Player.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Announcements.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Contacts.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Discussion.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Documents.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Events.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_HtmlText.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Links.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleDefinitions.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Modules.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleSettings.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Portals.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Roles.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Tabs.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UserRoles.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Users.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Quote.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Resource.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateAffectedBySkill.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItem.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemEquipable.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemJunk.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemQuaffable.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemReadable.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemWieldable.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateMobile.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateObject.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateTerrain.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.World.tab"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Area.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Clan.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ClanRank.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumItemDurability.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileSize.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileState.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumRace.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumResourceType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSex.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWeaponSize.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearSlot.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Inventory.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ObjectInstance.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Player.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Announcements.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Contacts.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Discussion.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Documents.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Events.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_HtmlText.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Links.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleDefinitions.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Modules.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleSettings.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Portals.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Roles.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Tabs.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UserRoles.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Users.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Quote.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Resource.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateAffectedBySkill.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItem.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemEquipable.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemJunk.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemQuaffable.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemReadable.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemWieldable.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateMobile.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateObject.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateTerrain.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.World.kci"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Area.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Clan.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ClanRank.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumItemDurability.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileSize.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileState.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumRace.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumResourceType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSex.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWeaponSize.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearSlot.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Inventory.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ObjectInstance.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Player.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Announcements.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Contacts.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Discussion.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Documents.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Events.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_HtmlText.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Links.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleDefinitions.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Modules.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleSettings.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Portals.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Roles.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Tabs.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UserRoles.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Users.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Quote.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Resource.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateAffectedBySkill.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItem.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemEquipable.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemJunk.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemQuaffable.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemReadable.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemWieldable.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateMobile.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateObject.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateTerrain.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.World.fky"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Area.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Clan.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ClanRank.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumActivationType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumCommand.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumDamageType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumEmote.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumItemDurability.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumLiquidType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileSize.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumMobileState.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumRace.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumResourceType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSex.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTargetType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainType.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWeaponSize.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearLocation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumWearSlot.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Inventory.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasClanRank.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobileHasSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.MobilePossesableByPlayer.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ObjectInstance.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Player.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Announcements.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Contacts.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Discussion.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Documents.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Events.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_HtmlText.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Links.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleDefinitions.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Modules.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_ModuleSettings.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Portals.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Roles.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Tabs.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UserRoles.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_Users.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Quote.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RaceHasEmote.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.Resource.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SkillEnablesSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SpecialisationEnablesSpecialisation.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateAffectedBySkill.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItem.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemEquipable.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemJunk.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemQuaffable.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemReadable.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateItemWieldable.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateMobile.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateObject.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TemplateTerrain.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.World.ext"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.ActivatePlayer.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.AreaSquareDetails.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateCharacter.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateEquipable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateGroupsTable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateJunk.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateMirror.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateMobile.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateModel.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreatePlayer.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateReadable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateResource.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.CreateWieldable.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.EnumTerrainInSquare.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.GetAreas.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.GetTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.LogonPlayer.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.LowerTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddAnnouncement.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddContact.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddEvent.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddLink.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddMessage.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddModule.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddModuleDefinition.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddTab.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddUser.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_AddUserRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteAnnouncement.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteContact.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteDocument.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteEvent.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteLink.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteModule.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteModuleDefinition.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteTab.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteUser.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_DeleteUserRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetAnnouncements.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetAuthRoles.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetContacts.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetDocumentContent.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetDocuments.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetEvents.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetHtmlText.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetLinks.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetModuleDefinitions.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetModuleSettings.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetNextMessageID.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetPortalRoles.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetPortalSettings.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetPrevMessageID.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetRoleMembership.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetRolesByUser.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleAnnouncement.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleContact.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleDocument.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleEvent.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleLink.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleMessage.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleModuleDefinition.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetSingleUser.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetThreadMessages.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetTopLevelMessages.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_GetUsers.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateAnnouncement.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateContact.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateDocument.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateEvent.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateHtmlText.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateLink.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateModule.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateModuleDefinition.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateModuleOrder.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateModuleSetting.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdatePortalInfo.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateRole.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateTab.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateTabOrder.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UpdateUser.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.PO_UserLogin.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RaiseTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.RotateTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SelectPlayer.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.SelectPlayerByPlayerKey.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.TerrainSquareDetails.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.UpdateResource.prc"
if %ERRORLEVEL% NEQ 0 goto errors
osql -S %1 -d %2 -E -b -i "dbo.UpdateTerrain.prc"
if %ERRORLEVEL% NEQ 0 goto errors

goto finish

REM: How to use screen
:usage
echo.
echo Usage: MyScript Server Database
echo Server: the name of the target SQL Server
echo Database: the name of the target database
echo.
echo Example: MyScript.cmd MainServer MainDatabase
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
