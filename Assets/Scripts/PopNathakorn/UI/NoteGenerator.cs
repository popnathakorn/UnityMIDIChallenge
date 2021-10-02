using UnityEngine;
using System.Collections.Generic;

namespace PopNathakorn.UI
{
    /// <summary>
    /// Note pooling system
    /// </summary>
    public class NoteGenerator : MonoBehaviour
    {
        #region Prefabs
        [SerializeField]
        private Note note;
        #endregion

        private Queue<Note> notePool = new Queue<Note>();

        /// <summary>
        /// Serve note to requestor
        /// </summary>
        /// <returns>New note</returns>
        public Note Create(RectTransform parent)
        {
            Note newNote;
            if(notePool.Count == 0)
            {
                var noteObject = Instantiate(note, parent, false);
                newNote = noteObject.GetComponent<Note>();
            }
            else
            {
                newNote = notePool.Dequeue();
                newNote.transform.SetParent(parent);
            }

            newNote.OnDestroyed.AddListener((destroyedNote) =>
            {
                newNote.transform.SetParent(transform);
                notePool.Enqueue(destroyedNote);
            });

            return newNote;
        }
    }
}
