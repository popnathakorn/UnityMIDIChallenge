using System;
using UnityEngine;

namespace PopNathakorn.Game.Configuration
{
    [Serializable]
    public struct MusicConfiguration
    {
        /// <summary>
        /// Absolute path for midi file
        /// </summary>
        public string MidiFilePath;

        /// <summary>
        /// Audio clip
        /// </summary>
        public AudioClip AudioClip;

        /// <summary>
        /// How long of midi delay from music in seconds
        /// </summary>
        public float TimeOffet;
    }
}
