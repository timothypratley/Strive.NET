using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Mobile : PhysicalObject
	{
		public int Level;
		public int Cognition;
		public int Willpower;
		public int Dexterity;
		public int Strength;
		public int Constitution;
		public EnumRace Race;
		public int HitPoints;
		public int Size;

		public Mobile (
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
			Level = mobile.Level;
			Cognition = mobile.Cognition;
			Willpower = mobile.Willpower;
			Dexterity = mobile.Dexterity;
			Strength = mobile.Strength;
			Constitution = mobile.Constitution;
			Race = (EnumRace)mobile.EnumRaceID;
			Size = 4; // EEERRR add to db omg
			HitPoints = Size*100 + Level * Constitution / 2;
		}
	}
}
