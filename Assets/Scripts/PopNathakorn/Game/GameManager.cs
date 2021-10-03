using UnityEngine;
using UnityEngine.UI;
using PopNathakorn.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PopNathakorn.Game.Configuration;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;

namespace PopNathakorn.Game
{
    public class GameManager : MonoBehaviour
    {
        #region UI Components
        [Header("UI Components")]
        [SerializeField]
        private NoteGenerator noteGenerator;
        [SerializeField]
        private Text pressSpaceToStartText;
        [SerializeField]
        private Text scoreText;
        #endregion

        #region Audio Components
        [Header("Audio Components")]
        [SerializeField]
        private AudioSource audioSource;
        #endregion

        #region Prefabs
        [Header("Prefabs")]
        [SerializeField]
        private NoteLane originalNoteLane;
        #endregion

        #region Settings
        [Header("Settings")]
        [SerializeField]
        private RectTransform noteLaneGroup;
        [SerializeField]
        private LevelConfiguration levelConfiguration;
        #endregion

        private Dictionary<NoteLane, NoteSequence> noteLanePairSequence;
        private int TotalScore
        {
            get { return totalScore; }
            set
            {
                totalScore = value;
                scoreText.text = $"Score: {totalScore}";
            }
        }
        private int totalScore;

        float time => Time.time;

        private void Awake()
        {
            pressSpaceToStartText.enabled = false;
            Setup(levelConfiguration);

            StartCoroutine(GameRoutine());
        }

        void ResetGame()
        {
            Debug.Log("[GameManager] Reset Game!");
            foreach(var keyPairValue in noteLanePairSequence)
            {
                keyPairValue.Key.NoteSequence = keyPairValue.Value;
            }
            TotalScore = 0;
        }

        IEnumerator GameRoutine()
        {
            while(true)
            {
                pressSpaceToStartText.enabled = true;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

                ResetGame();
                pressSpaceToStartText.enabled = false;
                Debug.Log("[GameManager] Game Start!");

                bool isAllNoteRendered = false;
                LaunchNotes(() => isAllNoteRendered = true);
                yield return new WaitForSeconds(levelConfiguration.TimeToReachHitPosition - levelConfiguration.Music.TimeOffet);
                PlayAudioClip();
                yield return new WaitUntil(() => !audioSource.isPlaying && isAllNoteRendered);
                Debug.Log("[GameManager] Game End!");
            }
        }

        private void Setup(LevelConfiguration levelConfiguration)
        {
            // Read a MIDI file
            var midiFile = MidiFile.Read(levelConfiguration.Music.MidiFileAbsolutePath);
            var midiTracks = midiFile.GetTrackChunks().ToList();

            TempoMap tempoMap = midiFile.GetTempoMap();

            noteLanePairSequence = new Dictionary<NoteLane, NoteSequence>();
            foreach(var noteLaneConfig in levelConfiguration.NoteLanes)
            {
                // Get notes ordered by time where channel and midi value same as configuration
                using NotesManager notesManager = midiTracks[noteLaneConfig.Track].ManageNotes();
                var notes = notesManager.Notes.Where(note => note.Channel == noteLaneConfig.Channel && note.NoteNumber == noteLaneConfig.MidiValue);
                var noteSequence = new NotesCollectionAdapter(notes, tempoMap);
                noteSequence.TimeToReachHitPosition = levelConfiguration.TimeToReachHitPosition;

                var criteriaInSeconds = GetTimeInSeconds(noteLaneConfig.PrecisionCriteria, tempoMap);

                var noteLaneObject = Instantiate(originalNoteLane, noteLaneGroup, false);
                var noteLane = noteLaneObject.GetComponent<NoteLane>();
                noteLane.InputKey = noteLaneConfig.InputKey;
                noteLane.Color = noteLaneConfig.Color;
                noteLane.NoteGenerator = noteGenerator;
                noteLane.PrecisionCriteria = criteriaInSeconds;

                noteLane.NoteSequence = noteSequence;

                int scorePoint = noteLaneConfig.ScorePoint;
                noteLane.OnHit?.AddListener((note) =>
                {
                    OnNoteHit(scorePoint);
                });

                noteLanePairSequence.Add(noteLane, noteSequence);
            }
        }

        private void OnNoteHit(int scorePoint)
        {
            TotalScore += scorePoint;
        }

        private void PlayAudioClip()
        {
            audioSource.PlayOneShot(levelConfiguration.Music.AudioClip);
        }

        private void LaunchNotes(Action onCompleted = null)
        {
            int completedLane = 0;
            var noteLanes = noteLanePairSequence.Keys;

            foreach(var lane in noteLanePairSequence.Keys)
            {
                lane.Launch();
                lane.OnCompleted.AddListener(() =>
                {
                    completedLane++;
                    if(completedLane == noteLanes.Count)
                        onCompleted?.Invoke();
                });
            }
        }

        private MusicalTimeSpan GetMusicalTimeSpan(PrecisionCriteria precisionCriteria)
        {
            switch(precisionCriteria)
            {
                case PrecisionCriteria.QuarterNote:
                    return MusicalTimeSpan.Quarter;
                case PrecisionCriteria.EighthNote:
                    return MusicalTimeSpan.Eighth;
                case PrecisionCriteria.SixteenthNote:
                    return MusicalTimeSpan.Sixteenth;
                case PrecisionCriteria.ThirtySecondNote:
                    return MusicalTimeSpan.ThirtySecond;
                default:
                    return MusicalTimeSpan.SixtyFourth;
            }
        }

        private float GetTimeInSeconds(PrecisionCriteria precisionCriteria, TempoMap tempoMap)
        {
            var musicalTime = GetMusicalTimeSpan(precisionCriteria);
            var metricTimeOfNote = TimeConverter.ConvertTo<MetricTimeSpan>(musicalTime, tempoMap);
            long timeInMicroseconds = metricTimeOfNote.TotalMicroseconds;
            return timeInMicroseconds / 1000000F;
        }
    }
}
