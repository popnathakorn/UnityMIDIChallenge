using System;
using UnityEngine;

namespace PopNathakorn.Game.Configuration
{
    [Serializable]
    public struct NoteLaneConfiguration
    {
        public KeyCode InputKey;
        public byte Track;
        public byte Channel;
        public byte MidiValue;
        public Color Color;
        public int ScorePoint;
        public PrecisionCriteria PrecisionCriteria;
    }

    public enum PrecisionCriteria : byte
    {
        WholeNote = 1,
        HalfNote = 2,
        QuarterNote = 4,
        EighthNote = 8,
        SixteenthNote = 16,
        ThirtySecondNote = 32,
        SixtyFourthNote = 64
    }
}
