using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    public class StopButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroButtonBase button;
        MetroAudioForm form;
        public StopButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;

            this.button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            var currentButton = (MetroButton)sender;
            m_Logger.Info("Stop button clicked!");
            if (form.LoadedMedia != null)
            {
                form.Player.Stop(form.LoadedMedia);
                form.TrackBarProgress.Enabled = false;
            }
        }
    }
}
