using UnityEngine;
using UnityEditor;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace PopNathakorn.Editor
{
    public class Tools
    {
        [MenuItem("PopNathakorn/Tools/Test Read MIDI File")]
        private static void Test()
        {
            Debug.Log("Test Read MIDI File");

            string path = EditorUtility.OpenFilePanel("Select Midi File", string.Empty, "mid");
            if(string.IsNullOrEmpty(path))
            {
                Debug.Log("No file selected!");
                return;
            }

            // Read a MIDI file
            var midiFile = MidiFile.Read(path);

            using NotesManager notesManager = midiFile.GetTrackChunks().First().ManageNotes();

            // Get notes ordered by time
            NotesCollection notes = notesManager.Notes;

            // Tempo map is needed in order to perform time span conversions
            TempoMap tempoMap = midiFile.GetTempoMap();

            // PopNathakorn
            var startTime = TimeConverter.ConvertTo<BarBeatTicksTimeSpan>(0, tempoMap);
            var startTempo = tempoMap.GetTempoAtTime(startTime);
            var tempoChanges = tempoMap.GetTempoChanges();

            string message = "Overview :";
            message += " " + $"bpmAtStart({startTempo.BeatsPerMinute})";
            message += " " + $"TempoChangesCount({tempoChanges.Count()})";
            message += " " + $"TimeSignatureAtStart({tempoMap.GetTimeSignatureAtTime(startTime)})";
            message += " " + $"TimeSignatureChanges({tempoMap.GetTimeSignatureChanges().Count()})";
            message += "\n";

            foreach(var note in notes)
            {
                var metricTimeOfNote = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
                var timeInMicroseconds = metricTimeOfNote.TotalMicroseconds;

                message += "Note :";
                message += "\t" + $"Name({note.NoteName})";
                message += "\t" + $"Number({note.NoteNumber})";
                message += "\t" + $"Channel({note.Channel})";
                message += "\t" + $"Length({note.Length})";
                message += "\t" + $"Time({note.Time})";
                message += "\t" + $"Octave({note.Octave})";
                message += "\t" + $"Length({TimeConverter.ConvertTo<MusicalTimeSpan>(note.Length, tempoMap)})";
                message += "\t" + $"Time({TimeConverter.ConvertTo<BarBeatTicksTimeSpan>(note.Time, tempoMap)})";
                message += "\t" + $"timeInMicroseconds({timeInMicroseconds})";
                message += "\n";
            }
            Debug.Log(message);
        }

        [MenuItem("PopNathakorn/Tools/Test File Path")]
        private static void TestFilePath()
        {
            string referencePath = "/AssetData/Midi/DrumTrack1.mid";

            // Read a MIDI file
            var midiFile = MidiFile.Read(Application.dataPath + referencePath);
        }
    }
}
