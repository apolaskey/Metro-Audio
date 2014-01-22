using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    /// <summary>
    /// Upgrade type for the MetroProgressBar to act instead as a Volume Control,
    /// instead of importing this into the designer I am going to be lazy and
    /// just have that component upgrade to this.
    /// </summary>
    public class VolumeControl
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroAudioForm form;
        MetroTrackBar bar;
        public VolumeControl(MetroTrackBar bar)
        {
            this.bar = bar;
            this.form = bar.FindForm() as MetroAudioForm;

            this.bar.MouseUp += bar_MouseUp;
        }

        void bar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (form.LoadedMedia != null)
            {
                float volume = (float)(bar.Value / 100.00f);
                form.LoadedMedia.Stream.VolumeStream.Volume = volume;
                m_Logger.Info("Volume settings changed to {0}", form.LoadedMedia.Stream.VolumeStream.Volume);
            }
        }


    }
}
