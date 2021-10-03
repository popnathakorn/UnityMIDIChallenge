using UnityEngine;

namespace PopNathakorn.UI.Demo
{
    public class NoteLaneDemo : MonoBehaviour
    {
        #region UI Components
        [SerializeField]
        private NoteLane noteLane1;
        [SerializeField]
        private NoteLane noteLane2;
        #endregion

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

            noteLane1.Launch(sequence);
            Debug.Log("LaunceSequenceOnLane1 start!");
            noteLane1.OnCompleted.AddListener(()=>
            {
                Debug.Log("LaunceSequenceOnLane1 finish!");
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
            noteLane2.Launch(sequence);
            Debug.Log("LaunceSequenceOnLane2 start!");
            noteLane2.OnCompleted.AddListener(() =>
            {
                Debug.Log("LaunceSequenceOnLane2 finish!");
            });
        }
    }
}
