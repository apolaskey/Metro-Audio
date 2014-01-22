using NAudio.Wave;
using NAudio.Wave.SampleProviders;
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
        public SampleChannel VolumeStream;
        public AudioFileReader FallBackStream;
    }
}
