using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace PopNathakorn.UI
{
    /// <summary>
    /// Control how notes launch
    /// </summary>
    public class NoteLane : MonoBehaviour
    {
        #region UI Components
        [Header("UI Components")]
        [SerializeField]
        private NoteGenerator noteGenerator;
        [SerializeField]
        private Button keyButton;
        [SerializeField]
        private Text keyButtonText;
        #endregion

        #region Settings
        [Header("Settings")]
        [SerializeField]
        private RectTransform startPosition;
        [SerializeField]
        private RectTransform hitPosition;
        [SerializeField]
        private RectTransform endPosition;
        [SerializeField]
        private RectTransform noteLaneArea;
        [SerializeField]
        private Color color;
        [SerializeField]
        private KeyCode inputKeyCode;
        #endregion

        public UnityEvent OnCompleted = new UnityEvent();

        public Color Color
        {
            set
            {
                color = value;
                keyButton.targetGraphic.color = color;
            }
        }

        public KeyCode InputKey
        {
            set
            {
                inputKeyCode = value;
                keyButtonText.text = inputKeyCode.ToString();
                name = $"NoteLane:{inputKeyCode}";
            }
        }

        public NoteGenerator NoteGenerator { set { noteGenerator = value; } }
        public NoteSequence NoteSequence { set { noteSequence = NoteSequence.OrderByTime(value); } }

        float time => Time.time;

        float startTime;
        NoteSequence noteSequence;
        IEnumerator launchingRoutine;
        float timeToReachEndPosition;
        bool isLatestNoteReachEndPosition;

        private void Awake()
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            keyButton.targetGraphic.color = color;
            keyButtonText.text = inputKeyCode.ToString();
        }

        public void Launch(NoteSequence noteSequence)
        {
            NoteSequence = noteSequence;
            Launch();
        }

        public void Launch()
        {
            if(launchingRoutine != null || noteSequence == null)
                return;

            startTime = time;
            timeToReachEndPosition = CalculateTimeToReachEndPosition(noteSequence.TimeToReachHitPosition);

            launchingRoutine = Launching();
            StartCoroutine(launchingRoutine);
        }

        IEnumerator Launching()
        {
            isLatestNoteReachEndPosition = false;

            while(noteSequence.Count > 0)
            {
                var noteData = noteSequence.Dequeue();
                var nextTimeToLaunch = noteData.Time;
                yield return new WaitUntil(() => time - startTime >= nextTimeToLaunch);

                var note = noteGenerator.Create(noteLaneArea);
                note.Launch(color, startPosition.position, endPosition.position, timeToReachEndPosition);

                // if this is the latest note, then watch for destroy
                if(noteSequence.Count == 0)
                    note.OnDestroyed.AddListener((destroyedNote) => isLatestNoteReachEndPosition = true);
            }

            yield return new WaitUntil(() => isLatestNoteReachEndPosition);

            launchingRoutine = null;
            OnCompleted?.Invoke();
            OnCompleted?.RemoveAllListeners();
        }

        public float CalculateTimeToReachEndPosition(float timeToReachHitPosition)
        {
            var hitDistance = Vector3.Distance(startPosition.position, hitPosition.position);
            var endDistance = Vector3.Distance(startPosition.position, endPosition.position);
            float hitDistanceRatio = Mathf.InverseLerp(0, endDistance, hitDistance);

            return timeToReachHitPosition / hitDistanceRatio;
        }
    }

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

    public struct NoteData
    {
        /// <summary>
        /// Time in seconds
        /// </summary>
        public float Time;

        public NoteData(float time)
        {
            Time = time;
        }
    }
}
