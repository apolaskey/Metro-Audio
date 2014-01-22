using Experia.Framework.Debug;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetroAudio.Player.Controls
{
    public class OpenFileButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroAudioForm form;
        MetroButtonBase button;
        OpenFileDialog fileDialog;
        String[] files;
        private BackgroundWorker m_Worker;

        public OpenFileButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;

            this.button.Click += button_Click;

            m_Worker = new BackgroundWorker();
            m_Worker.DoWork += m_Worker_DoWork;
            m_Worker.RunWorkerCompleted += m_Worker_RunWorkerCompleted;
            m_Worker.ProgressChanged += m_Worker_ProgressChanged;
            m_Worker.WorkerReportsProgress = true;

            fileDialog = new OpenFileDialog();
        }

        void m_Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            form.SpinnerListBox.Value = e.ProgressPercentage;
        }

        void m_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form.ListBoxPlayList.DrawMode = DrawMode.Normal;
            form.PlayList.ResetBindings();
            form.SpinnerListBox.Hide();
            form.ListBoxPlayList.Enabled = true;
            form.ButtonOpen.Enabled = true;
        }

        void m_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int workLoad = files.Length;
            int currentWork = 0;
            float fProgress, fProgressReal = 0.0f;
            foreach (var file in files)
            {
                if (form.PlayList.Where(p => p.Location == file).Count() == 0)
                {
                    m_Logger.Info("Adding media: {0}", file);
                    form.PlayList.Add(new AudioPlayListItem(form.Player) { Location = file });
                }
                else
                {
                    m_Logger.Info("Skipping media: {0}, already exists in playlist.", file);
                }
                currentWork = currentWork + 1;
                fProgress = (float)currentWork / workLoad;
                fProgressReal = (int)(fProgress * 100);
                m_Worker.ReportProgress((int)fProgressReal);
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            m_Logger.Info("Open button clicked!");
            var currentButton = (MetroButton)sender;

            /*OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP3 File (.mp3)|*.mp3|Wave File (.wav)|*.wav|Free Lossless Audio (.flac)|*.flac|Audio Interchange File Format (.aiff)|*.aiff";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var files = fileDialog.FileNames;
                foreach (var file in files)
                {
                    if (form.PlayList.Where(p => p.Location == file).Count() == 0)
                    {
                        m_Logger.Info("Adding media: {0}", file);
                        form.PlayList.Add(new AudioPlayListItem(form.Player) { Location = file });
                    }
                    else
                    {
                        m_Logger.Info("Skipping media: {0}, already exists in playlist.", file);
                    }
                }
                form.PlayList.ResetBindings();
            }*/

            fileDialog.Filter = "Media Files|*.mp3;*.wav;*.flac;*.aiff;*.ogg|MP3 File|*.mp3|Wave File|*.wav|Free Lossless Audio|*.flac|Audio Interchange File Format|*.aiff|Ogg Vorbis File Format|*.ogg";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                files = fileDialog.FileNames;
                form.ListBoxPlayList.Enabled = false;
                form.ListBoxPlayList.DrawMode = DrawMode.OwnerDrawFixed;
                form.SpinnerListBox.Show();
                form.ButtonOpen.Enabled = false;
                m_Worker.RunWorkerAsync();
            }   
        }

    }
}
