using Experia.Framework.Audio;
using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    public class PlayButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroButtonBase button;
        MetroAudioForm form;
        public PlayButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;

            this.button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            m_Logger.Info("Play button clicked!");
            if (form.LoadedMedia != null)
            {
                m_Logger.Info("Playing media {0} at playlist index {1}", form.LoadedMedia.ToString(), form.ListBoxPlayList.SelectedIndex);
                form.Player.Stop(form.LoadedMedia);
                form.LoadedMedia = form.GetSelectedPlayListItem();
                form.Player.Play(form.LoadedMedia);
                float volume = (float)(form.TrackBarVolume.Value / 100.00f);
                form.LoadedMedia.Stream.VolumeStream.Volume = volume;
                form.TrackBarProgress.Enabled = true;
                form.LabelSampleRateValue.Text = ((float)(form.LoadedMedia.Stream.WaveStream.WaveFormat.SampleRate / 1000.0f)).ToString() + "Khz";
                form.LabelBitRateValue.Text = form.LoadedMedia.Stream.WaveStream.WaveFormat.BitsPerSample.ToString();
                form.LabelEncodingValue.Text = form.LoadedMedia.Stream.WaveStream.WaveFormat.Encoding.ToString();
            }
            else
            {
                form.LoadedMedia = form.GetSelectedPlayListItem();
                if (form.LoadedMedia != null)
                {
                    form.Player.Play(form.LoadedMedia);
                    form.TrackBarProgress.Enabled = true;
                    form.LabelSampleRateValue.Text = ((float)(form.LoadedMedia.Stream.WaveStream.WaveFormat.SampleRate / 1000.0f)).ToString() + "Khz";
                    form.LabelBitRateValue.Text = form.LoadedMedia.Stream.WaveStream.WaveFormat.BitsPerSample.ToString();
                    form.LabelEncodingValue.Text = form.LoadedMedia.Stream.WaveStream.WaveFormat.Encoding.ToString();
                }
                else
                {
                    m_Logger.Warn("Media was unable to load successfully.");
                }
            }
        }
    }
}
