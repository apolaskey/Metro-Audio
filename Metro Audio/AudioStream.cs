using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experia.Framework.Audio
{
    /// <summary>
    /// Data Container for everything relating to raw audio
    /// </summary>
    public class AudioStream
    {
        public IWavePlayer DeviceHandle;
        public WaveStream WaveStream;
        public WaveChannel32 VolumeStream;
        public AudioFileReader FallBackStream;
    }
}
