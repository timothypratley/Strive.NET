using System;

using Strive.Multiverse;

namespace Strive.Server
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Mobile : MobileBase {
		public PhysicalObject target = null;
		public bool hasStruck = false;

		public Mobile(
			Schema.TemplateMobileRow mobile,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( mobile, template, instance ) {
		}

		public void CombatUpdate() {
			if (  target != null ) {
				if ( Global.random.Next(1) == 0 ) {
					// this mob strikes first
					PhysicalAttack( target );
				} else {
					// opponent strikes first
					return;
				}

				// dexterity challenge
			}
		}

		public void PeaceUpdate() {

		}

		public void PhysicalAttack( PhysicalObject po ) {

		}
	}
}
