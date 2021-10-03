using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;

namespace PopNathakorn
{
    public class NotesCollectionAdapter : NoteSequence
    {
        public NotesCollectionAdapter(IEnumerable<Note> notesCollection, TempoMap tempoMap)
        {
            foreach(var note in notesCollection)
            {
                var metricTimeOfNote = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
                long timeInMicroseconds = metricTimeOfNote.TotalMicroseconds;
                float timeInSeconds = timeInMicroseconds / 1000000F;
                var noteData = new NoteData(timeInSeconds);
                Enqueue(noteData);
            }
        }
    }
}
