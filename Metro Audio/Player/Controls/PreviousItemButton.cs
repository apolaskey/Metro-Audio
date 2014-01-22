using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    public class PreviousItemButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroButtonBase button;
        MetroAudioForm form;
        public PreviousItemButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;

            this.button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            if (form.ListBoxPlayList.SelectedIndex > 0)
            {
                int previousItem = form.ListBoxPlayList.SelectedIndex - 1;
                m_Logger.Info("Clearing selected items before getting previous slot.");
                form.ListBoxPlayList.ClearSelected();
                form.ListBoxPlayList.SetSelected(previousItem, true);
                m_Logger.Info("Going to the previous playlist item {0}", form.ListBoxPlayList.SelectedIndex);
                form.ButtonPlay.PerformClick();
            }
        }
    }
}
