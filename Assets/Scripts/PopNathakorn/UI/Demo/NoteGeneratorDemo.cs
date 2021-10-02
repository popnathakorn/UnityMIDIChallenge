using UnityEngine;

namespace PopNathakorn.UI.Demo
{
    public class NoteGeneratorDemo : MonoBehaviour
    {
        #region UI Components
        [SerializeField]
        private NoteGenerator noteGenerator;
        [SerializeField]
        private RectTransform lane1StartPosition;
        [SerializeField]
        private RectTransform lane1EndPosition;
        [SerializeField]
        private RectTransform lane1;
        [SerializeField]
        private RectTransform lane2StartPosition;
        [SerializeField]
        private RectTransform lane2EndPosition;
        [SerializeField]
        private RectTransform lane2;
        #endregion

        #region Settings
        [SerializeField]
        private Color color1;
        [SerializeField]
        private Color color2;
        #endregion

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                LaunchNoteOnLane1();
            if(Input.GetKeyDown(KeyCode.Alpha2))
                LaunchNoteOnLane2();
        }

        void LaunchNoteOnLane1()
        {
            var note = noteGenerator.Create(lane1);
            note.Launch(color1, lane1StartPosition.position, lane1EndPosition.position, 4F);
        }
        void LaunchNoteOnLane2()
        {
            var note = noteGenerator.Create(lane2);
            note.Launch(color2, lane2StartPosition.position, lane2EndPosition.position, 4F);
        }
    }
}
