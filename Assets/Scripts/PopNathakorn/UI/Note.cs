using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace PopNathakorn.UI
{
    public class Note : MonoBehaviour
    {
        #region Components
        [SerializeField]
        private Image image;
        #endregion

        Vector3 start;
        Vector3 end;
        float startTime;
        float endTime;
        Action<Note> onDestroyed;
        IEnumerator launchingRoutine;

        float time => Time.time;

        /// <summary>
        /// Launch note
        /// </summary>
        /// <param name="color">Color for this note</param>
        /// <param name="start">Start position</param>
        /// <param name="end">Destination position</param>
        /// <param name="timeToReach">How long to reach destination</param>
        public void Launch(Color color, Vector3 start, Vector3 end, float timeToReach, Action<Note> onDestroyed = null)
        {
            if(launchingRoutine != null)
                return;

            image.color = color;
            this.start = start;
            this.end = end;
            this.onDestroyed = onDestroyed;

            image.rectTransform.position = start;
            startTime = time;
            endTime = startTime + timeToReach;

            launchingRoutine = Launching();
            StartCoroutine(launchingRoutine);
        }

        public void Destroy()
        {
            if(launchingRoutine == null)
                return;

            StopCoroutine(launchingRoutine);
            image.rectTransform.position = end;
            launchingRoutine = null;
            onDestroyed?.Invoke(this);
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
}
