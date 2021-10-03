using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace PopNathakorn.UI
{
    public class Note : MonoBehaviour
    {
        #region Components
        [Header("Conponents")]
        [SerializeField]
        private Image image;
        #endregion

        #region Settings
        [Header("Settings")]
        [SerializeField]
        private float precisionCriteria;
        [SerializeField]
        private KeyCode inputKey;
        #endregion

        public NoteEvent OnDestroyed = new NoteEvent();
        public NoteEvent OnHit = new NoteEvent();
        public KeyCode InputKey { get { return inputKey; } }

        Vector3 startPosition;
        Vector3 hitPosition;
        Vector3 endPosition;
        float startTime;
        float endTime;
        float hitTime;
        IEnumerator launchingRoutine;

        float time => Time.time;

        /// <summary>
        /// Launch note
        /// </summary>
        /// <param name="color">Color for this note</param>
        /// <param name="startPosition">Start position</param>
        /// <param name="endPosition">Destination position</param>
        /// <param name="timeToReachEndPosition">How long to reach hit position</param>
        public void Launch(Color color, Vector3 startPosition, Vector3 endPosition, Vector3 hitPosition, float timeToReachHitPosition, float precisionCriteria, KeyCode inputKey = KeyCode.None)
        {
            if(launchingRoutine != null)
                return;

            image.color = color;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.hitPosition = hitPosition;
            this.inputKey = inputKey;
            this.precisionCriteria = precisionCriteria;

            image.rectTransform.position = startPosition;
            startTime = time;
            hitTime = time + timeToReachHitPosition;
            float timeToReachEndPosition = CalculateTimeToReachEndPosition(timeToReachHitPosition);
            endTime = startTime + timeToReachEndPosition;

            launchingRoutine = Launching();
            StartCoroutine(launchingRoutine);
        }

        /// <summary>
        /// Visually destroy this note
        /// </summary>
        public void Destroy()
        {
            if(launchingRoutine == null)
                return;

            StopCoroutine(launchingRoutine);
            image.rectTransform.position = endPosition;
            launchingRoutine = null;

            OnDestroyed?.Invoke(this);
            OnDestroyed?.RemoveAllListeners();
            OnHit?.RemoveAllListeners();
        }

        IEnumerator Launching()
        {
            while(time < endTime)
            {
                image.rectTransform.position = Vector3.Lerp(startPosition, endPosition, Mathf.InverseLerp(startTime, endTime, time));
                yield return null;
            }

            Destroy();
        }

        private void Update()
        {
            if(launchingRoutine == null)
                return;

            if(Input.GetKeyDown(inputKey))
            {
                float precision = Mathf.Abs(hitTime - time);
                if(precision <= precisionCriteria)
                {
                    OnHit?.Invoke(this);
                    Destroy();
                }
            }
        }

        public float CalculateTimeToReachEndPosition(float timeToReachHitPosition)
        {
            var hitDistance = Vector3.Distance(startPosition, hitPosition);
            var endDistance = Vector3.Distance(startPosition, endPosition);
            float hitDistanceRatio = Mathf.InverseLerp(0, endDistance, hitDistance);

            return timeToReachHitPosition / hitDistanceRatio;
        }
    }

    public class NoteEvent : UnityEvent<Note> { }
}
