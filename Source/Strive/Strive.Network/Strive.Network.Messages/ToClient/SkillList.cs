using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class SkillList : IMessage {
        public Tuple<int, double>[] SkillCompetancy;
		public SkillList(){}
        public SkillList(Tuple<int, double>[] skillCompetancy)
        {
			SkillCompetancy = skillCompetancy;
		}
	}
}
