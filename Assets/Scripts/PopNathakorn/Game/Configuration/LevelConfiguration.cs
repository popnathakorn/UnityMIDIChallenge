using System;
using System.Collections.Generic;

namespace PopNathakorn.Game.Configuration
{
    [Serializable]
    public struct LevelConfiguration
    {
        public MusicConfiguration Music;
        public List<NoteLaneConfiguration> NoteLanes;
        public float TimeToReachHitPosition;
    }
}
