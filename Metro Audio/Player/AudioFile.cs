using BigMansStuff.NAudio.Ogg;
using Experia.Framework.Content;
using Experia.Framework.Debug;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.WindowsMediaFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experia.Framework.Audio
{
    /// <summary>
    /// A loaded in-memory audio file.
    /// </summary>
    public class AudioFile
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();

        protected AudioStream m_Stream;

        protected String m_Name;

        public AudioStream Stream
        {
            get { return m_Stream; }
        }

        public String Location
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return m_Name;
        }

        public bool Load(String location)
        {
            Location = location;
            m_Name = location.Split('\\').Last();
            String a = ContentManager.GetFileExtension(location);

            switch(a.ToLower())
            {
                case "wav":
                    m_Stream = CreateWavInputStream(location);
                    break;
                case "mp3":
                    m_Stream = CreateMp3InputStream(location);
                    break;
                case "flac":
                    m_Stream = CreateFlacInputStream(location);
                    break;
                case "ogg":
                    m_Stream = CreateOggInputStream(location);
                    break;
                case "aiff":
                    m_Stream = CreateAiffInputStream(location);
                    break;
                case "wma":
                    m_Stream = CreateWmaInputStream(location);
                    break;
                default:
                    m_Logger.Warn("Failed to find strong type conversion for audio format [{0}] attempting a deep file read!", a.ToLower());
                    m_Stream = DefaultInputStream(location);
                    if (m_Stream == null)
                    {
                        m_Logger.Error("Unable to load audio file {0}!", location);
                    }
                    break;
            }

            return false;
        }

        protected static AudioStream DefaultInputStream(String location)
        {
            AudioStream stream = null;
            try
            {
                AudioFileReader reader = new AudioFileReader(location);
                if (reader.TotalTime > TimeSpan.Zero)
                {
                    stream.FallBackStream = reader;
                }
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load audio file - {0}", location, e);
            }
            return stream;
        }

        protected static AudioStream CreateWavInputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new WaveFileReader(location);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
            }
            catch(Exception e)
            {
                m_Logger.Error("Failed to load *.wav Audio Asset - {0}{1}{2}", location,Environment.NewLine, e);
            }

            return stream;
        }

        protected static AudioStream CreateWmaInputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new WMAFileReader(location);
                stream.DeviceHandle = new WaveOutEvent();
                stream.DeviceHandle.Init(stream.WaveStream);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load *.wma Audio Asset - {0}{1}{2}", location, Environment.NewLine, e);
            }

            return stream;
        }

        protected static AudioStream CreateMp3InputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new Mp3FileReader(location); 
                stream.DeviceHandle = new WaveOutEvent();
                stream.DeviceHandle.Init(stream.WaveStream);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
                
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load *.mp3 Audio Asset - {0}{1}{2}", location, Environment.NewLine, e);
            }

            return stream;
        }

        protected static AudioStream CreateAiffInputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new AiffFileReader(location);
                stream.DeviceHandle = new WaveOutEvent();
                stream.DeviceHandle.Init(stream.WaveStream);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load *.aiff Audio Asset - {0}{1}{2}", location, Environment.NewLine, e);
            }

            return stream;
        }

        protected static AudioStream CreateFlacInputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new FLACFileReader(location);
                stream.DeviceHandle = new WaveOutEvent();
                stream.DeviceHandle.Init(stream.WaveStream);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load *.flac Audio Asset - {0}{1}{2}", location, Environment.NewLine, e);
            }
            return stream;
        }

        protected static AudioStream CreateOggInputStream(String location)
        {
            AudioStream stream = new AudioStream();
            try
            {
                stream.WaveStream = new OggFileReader(location);
                stream.DeviceHandle = new WaveOutEvent();
                stream.DeviceHandle.Init(stream.WaveStream);
                stream.VolumeStream = new SampleChannel(stream.WaveStream, true);
            }
            catch (Exception e)
            {
                m_Logger.Error("Failed to load *.ogg Audio Asset - {0}{1}{2}", location, Environment.NewLine, e);
            }
            return stream;
        }

    }
}
