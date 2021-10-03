using UnityEngine;

namespace PopNathakorn.UI.Demo
{
    public class NoteDemo : MonoBehaviour
    {
        #region UI Components
        [Header("UI Components")]
        [SerializeField]
        private Note note;
        [SerializeField]
        private RectTransform noteGenerator;
        [SerializeField]
        private RectTransform noteDestroyer;
        [SerializeField]
        private RectTransform hitPosition;
        #endregion

        #region Settings
        [Header("UI Settings")]
        [SerializeField]
        private Color color1;
        [SerializeField]
        private Color color2;
        [SerializeField]
        private Color color3;
        [SerializeField]
        private KeyCode inputKey;
        [SerializeField]
        private float precisionCriteria;
        #endregion

        float time => Time.time;

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
            Debug.Log($"Launch note color 1 using 4 second to reach hit position, time({time})");
            note.Launch(color1, noteGenerator.position, noteDestroyer.position, hitPosition.position, 4F, precisionCriteria, inputKey);
            note.OnDestroyed.AddListener((note) =>
            {
                Debug.Log($"Note color 1 has been destroyed, time({time})");
            });
        }

        private void LaunchColor2With3Seconds()
        {
            Debug.Log($"Launch note color 2 using 3 second to reach hit position, time({time})");
            note.Launch(color2, noteGenerator.position, noteDestroyer.position, hitPosition.position, 3F, precisionCriteria, inputKey);
            note.OnDestroyed.AddListener((note) =>
            {
                Debug.Log($"Note color 2 has been destroyed, time({time})");

            });
        }

        private void LaunchColor3With2Seconds()
        {
            Debug.Log($"Launch note color 3 using 2 second to reach hit position, time({time})");
            note.Launch(color3, noteGenerator.position, noteDestroyer.position, hitPosition.position, 2F, precisionCriteria, inputKey);
            note.OnDestroyed.AddListener((note) =>
            {
                Debug.Log($"Note color 3 has been destroyed, time({time})");
            });
        }
        #endregion
    }
}
