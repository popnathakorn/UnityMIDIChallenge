using System.Linq;
using System.Collections.Generic;

namespace PopNathakorn
{
    public class NoteSequence : Queue<NoteData>
    {
        public float TimeToReachHitPosition;

        public NoteSequence() { }

        public NoteSequence(float timeToReachHitPosition)
        {
            TimeToReachHitPosition = timeToReachHitPosition;
        }

        public static NoteSequence OrderByTime(NoteSequence noteSequence)
        {
            var newSequence = new NoteSequence(noteSequence.TimeToReachHitPosition);

            var sortedQueue = noteSequence.OrderBy(noteData => noteData.Time);
            foreach(var noteData in sortedQueue)
                newSequence.Enqueue(noteData);

            return newSequence;
        }
    }
}
