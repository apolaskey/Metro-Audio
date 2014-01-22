using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using Experia.Framework.Debug;

namespace Experia.Framework.Audio
{
    /// <summary>
    /// 
    /// </summary>
    public class AudioPlayer
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        protected IntPtr m_WindowHandle;

        /// <summary>
        /// Responsible for managing and playing sounds; if the Max streams is reached will dump the last file and play the new one.
        /// </summary>
        /// <param name="windowHandle">(IntPtr) Window Form / Control handle</param>
        public AudioPlayer(IntPtr windowHandle)
        {
            m_WindowHandle = windowHandle;
        }

        public PlaybackState Play(AudioFile file)
        {
            if (file.Stream.DeviceHandle.PlaybackState != PlaybackState.Playing)
            {
                file.Stream.DeviceHandle.Play();
            }
            return file.Stream.DeviceHandle.PlaybackState;
        }

        public PlaybackState Pause(AudioFile file)
        {
            if (file.Stream.DeviceHandle.PlaybackState != PlaybackState.Paused)
            {
                file.Stream.DeviceHandle.Pause();
            }
            return file.Stream.DeviceHandle.PlaybackState;
        }

        public PlaybackState Stop(AudioFile file)
        {
            if (file != null && file.Stream.DeviceHandle.PlaybackState != PlaybackState.Stopped)
            {
                file.Stream.DeviceHandle.Stop();
                // Is this not set internally? I am very confused...
                file.Stream.WaveStream.Position = 0;
                m_Logger.Info("Stopping playback of an AudioFile!");
                while (file.Stream.DeviceHandle.PlaybackState != PlaybackState.Stopped)
                {
                    file.Stream.DeviceHandle.Stop();
                }
                return file.Stream.DeviceHandle.PlaybackState;
            }
            else
            {
                m_Logger.Warn("Audio Stream was null when attempting to stop!");
            }
            return PlaybackState.Stopped;
        }

        public AudioFile LoadSound(String location)
        {
            AudioFile file = new AudioFile();
            file.Load(location);
            m_Logger.Info("Successfully loaded AudioFile - {0} into player.", location);

            return file;
        }

    }
}
