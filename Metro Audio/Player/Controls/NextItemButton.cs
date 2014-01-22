using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    public class NextItemButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroButtonBase button;
        MetroAudioForm form;
        public NextItemButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;

            this.button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            if (form.ListBoxPlayList.SelectedIndex < form.ListBoxPlayList.Items.Count - 1)
            {
                int newIndex = form.ListBoxPlayList.SelectedIndex + 1;

                form.ListBoxPlayList.SelectedItems.Clear();
                form.ListBoxPlayList.ClearSelected();
                while (form.ListBoxPlayList.SelectedIndex != -1)
                {

                }
                m_Logger.Info("Clear stuff");
                form.ListBoxPlayList.SetSelected(newIndex, true);
                form.ButtonPlay.PerformClick();
            }
            else if (form.ListBoxPlayList.SelectedIndex == form.ListBoxPlayList.Items.Count - 1)
            {
                form.ListBoxPlayList.SelectedItems.Clear();
                form.ListBoxPlayList.SetSelected(0, true);
                form.ButtonPlay.PerformClick();
            }
        }

    }
}
