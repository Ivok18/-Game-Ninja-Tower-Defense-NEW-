using System;

namespace TD.WaveSystem
{
    [Serializable]
    public class WaveData
    {
        public string waveName;
        public WaveState waveState;
        public EnemySpawnerData spawner;     
    }
}

