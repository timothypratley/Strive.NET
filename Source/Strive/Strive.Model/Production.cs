using System;
using System.Diagnostics.Contracts;
using Microsoft.FSharp.Collections;


namespace Strive.Model
{
    public class Production
    {
        public Production(float rate, FSharpList<int> queue, float target, float progress)
        {
            Contract.Requires<ArgumentException>(rate >= 0);
            Contract.Requires<ArgumentException>(progress < target);

            Rate = rate;
            Queue = queue;
            Target = target;
            Progress = progress;
        }

        public float Rate { get; private set; }
        public FSharpList<int> Queue { get; private set; }
        public float Target { get; private set; }
        public float Progress { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public Production WithProduction(int id, DateTime when)
        {
            var r = (Production)this.MemberwiseClone();
            if (Queue.IsEmpty)
                r.Progress = 0;
            r.Queue = ListModule.Append(r.Queue, ListModule.OfArray(new[] { id }));
            r.LastUpdated = when;
            return r;
        }

        public Production WithProductionComplete(DateTime when)
        {
            var r = (Production)this.MemberwiseClone();
            r.Queue = ListModule.OfSeq(SeqModule.Take(r.Queue.Length - 1, r.Queue));
            r.LastUpdated = when;
            return r;
        }

        public Production WithProgressChange(float progressChange, DateTime when)
        {
            var r = (Production)this.MemberwiseClone();
            r.Progress += progressChange;
            r.LastUpdated = when;
            return r;
        }
    }
}
