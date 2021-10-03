using UnityEngine;
using System.Collections;

namespace PopNathakorn.UI.Demo
{
    public class NoteLaneDemo : MonoBehaviour
    {
        #region UI Components
        [Header("UI Components")]
        [SerializeField]
        private NoteLane noteLane1;
        [SerializeField]
        private NoteLane noteLane2;
        #endregion

        #region Settings
        [Header("UI Settings")]
        [SerializeField]
        private float precisionCriteria;
        #endregion

        float time => Time.time;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                LaunceSequenceOnLane1(4F);
            if(Input.GetKeyDown(KeyCode.Alpha2))
                LaunceSequenceOnLane2(4F);
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                LaunceSequenceOnLane1(4F);
                LaunceSequenceOnLane2(4F);
            }

            if(Input.GetKeyDown(KeyCode.Alpha4))
                LaunceSequenceOnLane1(2.5F);
            if(Input.GetKeyDown(KeyCode.Alpha5))
                LaunceSequenceOnLane2(2.5F);
            if(Input.GetKeyDown(KeyCode.Alpha6))
            {
                LaunceSequenceOnLane1(2.5F);
                LaunceSequenceOnLane2(2.5F);
            }
        }

        private void LaunceSequenceOnLane1(float timeToReachHitPosition)
        {
            var sequence = new NoteSequence(timeToReachHitPosition);
            sequence.Enqueue(new NoteData(0F));
            sequence.Enqueue(new NoteData(0.5F));
            sequence.Enqueue(new NoteData(1F));
            sequence.Enqueue(new NoteData(1.5F));
            sequence.Enqueue(new NoteData(2F));
            sequence.Enqueue(new NoteData(2.75F));
            sequence.Enqueue(new NoteData(3F));

            noteLane1.PrecisionCriteria = precisionCriteria;
            noteLane1.OnCompleted.AddListener(()=>
            {
                Debug.Log($"LaunceSequenceOnLane1 finish!, time({time})");
            });
            noteLane1.Launch(sequence);
            Debug.Log($"LaunceSequenceOnLane1 start!, time({time})");
            DelayInvokeAction(timeToReachHitPosition, () =>
            {
                Debug.Log($"First Note on lane1 should reach hit position!, time({time})");
            });
        }

        private void LaunceSequenceOnLane2(float timeToReachHitPosition)
        {
            var sequence = new NoteSequence(timeToReachHitPosition);
            sequence.Enqueue(new NoteData(0F));
            sequence.Enqueue(new NoteData(0.75F));
            sequence.Enqueue(new NoteData(1F));
            sequence.Enqueue(new NoteData(2F));
            sequence.Enqueue(new NoteData(2.75F));
            sequence.Enqueue(new NoteData(3F));

            noteLane2.PrecisionCriteria = precisionCriteria;
            noteLane2.OnCompleted.AddListener(() =>
            {
                Debug.Log($"LaunceSequenceOnLane2 finish!, time({time})");
            });
            noteLane2.Launch(sequence);
            Debug.Log($"LaunceSequenceOnLane2 start!, time({time})");
            DelayInvokeAction(timeToReachHitPosition, () =>
            {
                Debug.Log($"First Note on lane2 should reach hit position!, time({time})");
            });
        }

        private void DelayInvokeAction(float timeInSeconds, System.Action action)
        {
            StartCoroutine(DelayInvokeActionRoutine(timeInSeconds, action));
        }

        private IEnumerator DelayInvokeActionRoutine(float timeInSeconds, System.Action action)
        {
            yield return new WaitForSeconds(timeInSeconds);
            action?.Invoke();
        }
    }
}
