using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class SkillList : IMessage {
		public int [] skills;
		public float [] competancy;
		public SkillList(){}
		public SkillList( int [] skills, float [] competancy ) {
			this.skills = skills;
			this.competancy = competancy;
		}
	}
}
