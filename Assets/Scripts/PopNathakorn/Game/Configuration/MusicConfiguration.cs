using System;
using UnityEngine;

namespace PopNathakorn.Game.Configuration
{
    [Serializable]
    public struct MusicConfiguration
    {
        /// <summary>
        /// Relative path for midi file
        /// </summary>
        public string MidiFileRelativePath;

        /// <summary>
        /// Audio clip
        /// </summary>
        public AudioClip AudioClip;

        /// <summary>
        /// How long of midi delay from music in seconds
        /// </summary>
        public float TimeOffet;

        /// <summary>
        /// Absolute path for midi file
        /// </summary>
        public string MidiFileAbsolutePath { get => Application.dataPath + MidiFileRelativePath; }
    }
}
