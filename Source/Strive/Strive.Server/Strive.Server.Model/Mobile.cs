using Strive.Common;


namespace Strive.Server.Model
{
    public class Mobile : PhysicalObject
    {
        public int Level;
        public int Cognition;
        public int Willpower;
        public int Dexterity;
        public int Strength;
        public int Constitution;
        public EnumRace Race;
        public EnumMobileSize MobileSize;
        public EnumMobileState MobileState;
        public bool Possessable;

        public Mobile() {}
        public Mobile(
            Schema.TemplateMobileRow mobile,
            Schema.TemplateObjectRow template,
            Schema.ObjectInstanceRow instance
        )
            : base(template, instance)
        {
            Level = mobile.Level;
            Cognition = mobile.Cognition;
            Willpower = mobile.Willpower;
            Dexterity = mobile.Dexterity;
            Strength = mobile.Strength;
            Constitution = mobile.Constitution;
            Race = (EnumRace) mobile.EnumRaceID;
            MobileSize = (EnumMobileSize) mobile.EnumMobileSizeID;
            MobileState = (EnumMobileState) mobile.EnumMobileStateID;
            MaxHitPoints = mobile.EnumMobileSizeID*100 + Level*Constitution/2;
            HitPoints = (float) instance.HitpointsCurrent;
            MaxEnergy = mobile.EnumMobileSizeID*100 + Level*Constitution/2;
            Energy = (float) instance.EnergyCurrent;
            Possessable = instance.GetMobilePossesableByPlayerRows().Length > 0;
        }
    }
}
