using UnityEngine;

namespace PopNathakorn.UI.Demo
{
    public class NoteDemo : MonoBehaviour
    {
        #region UI Components
        [SerializeField]
        private Note note;
        [SerializeField]
        private RectTransform noteGenerator;
        [SerializeField]
        private RectTransform noteDestroyer;
        #endregion

        #region Settings
        [SerializeField]
        private Color color1;
        [SerializeField]
        private Color color2;
        [SerializeField]
        private Color color3;
        #endregion

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                LaunchColor1With4Seconds();
            if(Input.GetKeyDown(KeyCode.Alpha2))
                LaunchColor2With3Seconds();
            if(Input.GetKeyDown(KeyCode.Alpha3))
                LaunchColor3With2Seconds();
            if(Input.GetKeyDown(KeyCode.Alpha4))
                note.Destroy();
        }

        #region Demo
        private void LaunchColor1With4Seconds()
        {
            Debug.Log("Launch note color 1 using 4 second to reach the end");
            note.Launch(color1, noteGenerator.position, noteDestroyer.position, 4F, (note)=>
            {
                Debug.Log("Note color 1 has been destroyed");
            });
        }

        private void LaunchColor2With3Seconds()
        {
            Debug.Log("Launch note color 2 using 3 second to reach the end");
            note.Launch(color2, noteGenerator.position, noteDestroyer.position, 3F, (note) =>
            {
                Debug.Log("Note color 2 has been destroyed");
            });
        }

        private void LaunchColor3With2Seconds()
        {
            Debug.Log("Launch note color 3 using 2 second to reach the end");
            note.Launch(color3, noteGenerator.position, noteDestroyer.position, 2F, (note) =>
            {
                Debug.Log("Note color 3 has been destroyed");
            });
        }
        #endregion
    }
}
