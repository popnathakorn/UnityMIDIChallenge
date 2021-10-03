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
        private KeyCode inputKey;
        [SerializeField]
        private float precisionCriteria;
        #endregion

        public NoteEvent OnHit = new NoteEvent();
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
                inputKey = value;
                keyButtonText.text = inputKey.ToString();
                name = $"NoteLane:{inputKey}";
            }
        }

        public NoteGenerator NoteGenerator { set { noteGenerator = value; } }
        public NoteSequence NoteSequence { set { noteSequence = NoteSequence.OrderByTime(value); } }
        public float PrecisionCriteria { set { precisionCriteria = value; } }

        float time => Time.time;

        float startTime;
        NoteSequence noteSequence;
        IEnumerator launchingRoutine;
        bool isLatestNoteDestroyed;

        private void Awake()
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            keyButton.targetGraphic.color = color;
            keyButtonText.text = inputKey.ToString();
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

            launchingRoutine = Launching();
            StartCoroutine(launchingRoutine);
        }

        IEnumerator Launching()
        {
            isLatestNoteDestroyed = false;

            while(noteSequence.Count > 0)
            {
                var noteData = noteSequence.Dequeue();
                var nextTimeToLaunch = noteData.Time;
                yield return new WaitUntil(() => time - startTime >= nextTimeToLaunch);

                var note = noteGenerator.Create(noteLaneArea);
                note.Launch(color, startPosition.position, endPosition.position, hitPosition.position, noteSequence.TimeToReachHitPosition, precisionCriteria, inputKey);
                note.OnHit.AddListener((n) => { OnHit?.Invoke(n); });

                // if this is the latest note, then watch for destroy
                if(noteSequence.Count == 0)
                    note.OnDestroyed.AddListener((destroyedNote) => isLatestNoteDestroyed = true);
            }

            yield return new WaitUntil(() => isLatestNoteDestroyed);

            launchingRoutine = null;
            OnCompleted?.Invoke();
            OnCompleted?.RemoveAllListeners();
        }
    }
}
