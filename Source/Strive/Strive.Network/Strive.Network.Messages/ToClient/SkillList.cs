using System;

namespace Strive.Network.Messages.ToClient
{
    public class SkillList
    {
        public Tuple<int, double>[] SkillCompetancy;

        public SkillList(Tuple<int, double>[] skillCompetancy)
        {
            SkillCompetancy = skillCompetancy;
        }
    }
}
