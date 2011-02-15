using System;

namespace Strive.Network.Messages.ToClient
{
    public class SkillList : IMessage
    {
        public Tuple<int, double>[] SkillCompetancy;

        public SkillList(Tuple<int, double>[] skillCompetancy)
        {
            SkillCompetancy = skillCompetancy;
        }
    }
}
