using UnityEngine;
using UnityEngine.UI;

namespace PopNathakorn.UI
{
    public class KeyVisualizer : Selectable
    {
        #region Settings
        [Header("Settings")]
        public KeyCode InputKey;
        #endregion

        private void Update()
        {
            if(Input.GetKeyDown(InputKey))
            {
                DoStateTransition(SelectionState.Pressed, false);
            }
            else if(Input.GetKeyUp(InputKey))
            {
                DoStateTransition(SelectionState.Normal, false);
            }
        }
    }
}
