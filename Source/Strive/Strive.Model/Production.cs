using System;
using System.Diagnostics.Contracts;
using Microsoft.FSharp.Collections;


namespace Strive.Model
{
    public class Production
    {
        public Production(FSharpList<int> queue, bool repeat, float progress, float span, float rate)
        {
            Contract.Requires<ArgumentException>(rate >= 0);
            Contract.Requires<ArgumentException>(queue.IsEmpty || progress < span);

            Queue = queue;
            Repeat = repeat;
            Progress = progress;
            Span = span;
            Rate = rate;
        }

        public float Rate { get; private set; }
        public FSharpList<int> Queue { get; private set; }
        public bool Repeat { get; private set; }
        public float Span { get; private set; }
        public float Progress { get; private set; }
        public DateTime LastUpdated { get; private set; }

        private static Production _empty = new Production(ListModule.Empty<int>(), true, 0, 0, 0);

        public static Production Empty { get { return _empty; } }

        public Production WithProduction(int id, float span, DateTime when)
        {
            var r = (Production)this.MemberwiseClone();
            if (Queue.IsEmpty)
                r.Progress = 0;
            r.Queue = ListModule.Append(r.Queue, ListModule.OfArray(new[] { id }));
            r.Span = span;
            // TODO: use a specific rate
            r.Rate = 1;
            r.LastUpdated = when;
            return r;
        }

        public Production WithProductionComplete(DateTime when)
        {
            var r = (Production)this.MemberwiseClone();
            if (r.Repeat)
                r.Queue = ListModule.Reverse(
                    new FSharpList<int>(r.Queue.Head, ListModule.Reverse(r.Queue.Tail)));
            else
                r.Queue = r.Queue.Tail;

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
