using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    /// <summary>
    /// Same idea as the VolumeControl just upgraded it's type.
    /// </summary>
    public class MediaSeekBarControl
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroAudioForm form;
        MetroTrackBar bar;

        public MediaSeekBarControl(MetroTrackBar bar)
        {
            this.bar = bar;
            this.form = bar.FindForm() as MetroAudioForm;
        }
    }
}
