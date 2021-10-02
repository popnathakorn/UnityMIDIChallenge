using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace PopNathakorn.UI
{
    public class Note : MonoBehaviour
    {
        #region Components
        [SerializeField]
        private Image image;
        #endregion

        public NoteEvent OnDestroyed = new NoteEvent();

        Vector3 start;
        Vector3 end;
        float startTime;
        float endTime;
        IEnumerator launchingRoutine;

        float time => Time.time;

        /// <summary>
        /// Launch note
        /// </summary>
        /// <param name="color">Color for this note</param>
        /// <param name="start">Start position</param>
        /// <param name="end">Destination position</param>
        /// <param name="timeToReach">How long to reach destination</param>
        public void Launch(Color color, Vector3 start, Vector3 end, float timeToReach)
        {
            if(launchingRoutine != null)
                return;

            image.color = color;
            this.start = start;
            this.end = end;

            image.rectTransform.position = start;
            startTime = time;
            endTime = startTime + timeToReach;

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
            image.rectTransform.position = end;
            launchingRoutine = null;

            OnDestroyed?.Invoke(this);
            OnDestroyed?.RemoveAllListeners();
        }

        IEnumerator Launching()
        {
            while(time < endTime)
            {
                image.rectTransform.position = Vector3.Lerp(start, end, Mathf.InverseLerp(startTime, endTime, time));
                yield return null;
            }

            Destroy();
        }
    }

    public class NoteEvent : UnityEvent<Note> { }
}
