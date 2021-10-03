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

        float time => Time.time;

        float startTime;
        NoteSequence noteSequence;
        IEnumerator launchingRoutine;
        float timeToReachEndPosition;
        bool isLatestNoteReachEndPosition;

        private void Awake()
        {
            keyButton.targetGraphic.color = color;
            keyButtonText.text = inputKeyCode.ToString();
        }

        public void Launch(NoteSequence noteSequence)
        {
            if(launchingRoutine != null || noteSequence == null)
                return;

            this.noteSequence = NoteSequence.OrderByTime(noteSequence);
            timeToReachEndPosition = CalculateTimeToReachEndPosition(noteSequence.TimeToReachHitPosition);

            startTime = time;

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
        public float Time;

        public NoteData(float time)
        {
            Time = time;
        }
    }
}
