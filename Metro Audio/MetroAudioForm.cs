using Experia.Framework.Audio;
using Experia.Framework.Debug;
using MetroFramework;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetroAudio
{
    public partial class MetroAudioForm : MetroFramework.Forms.MetroForm
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();

        private AudioPlayer audioPlayer;
        private AudioFile currentMedia;
        private BindingList<AudioPlayListItem> audioPlayListItems = new BindingList<AudioPlayListItem>();
        private TimeSpan SeekUpdate;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MetroAudioForm(args));
        }

        public MetroAudioForm(String[] args)
        {
            InitializeComponent();
            BindAccessibilityEvents();
            audioPlayer = new AudioPlayer(this.Handle);
            ListBoxPlayList.DataSource = audioPlayListItems;
            ListBoxPlayList.DisplayMember = "Name";
            ListBoxPlayList.ValueMember = "Location";


            // Support Drag & Drop
            this.AllowDrop = true;
            this.DragEnter += MetroAudioForm_DragEnter;
            this.DragDrop += MetroAudioForm_DragDrop;
            
            // Fill our options
            this.ComboBoxOptionsTheme.DataSource = Enum.GetValues(typeof(MetroFramework.MetroColorStyle)).Cast<MetroFramework.MetroColorStyle>().ToList();
            this.ComboBoxOptionsTheme.SelectedItem = MetroColorStyle.Red;
            
            this.ListBoxPlayList.MouseDoubleClick += MetroAudioForm_MouseDoubleClickListBox;

            this.TrackBarProgress.MouseUp += TrackBarProgress_MouseUp;

            // Support the Open With Context menu
            if (args != null && args.Length > 0)
            {
                m_Logger.Info("Loading {0} items into player...", args.Length);
                for (int i = 0; i < args.Length; i++)
                {
                    m_Logger.Info("Adding media: {0}", args[i]);
                    audioPlayListItems.Add(new AudioPlayListItem(audioPlayer) { Location = args[i] });
                }
            }
        }

        void TrackBarProgress_MouseUp(object sender, MouseEventArgs e)
        {
            currentMedia.Stream.WaveStream.CurrentTime = currentMedia.Stream.WaveStream.CurrentTime + SeekUpdate;
            m_Logger.Info("Adjusting Media progress seek to: {0}", currentMedia.Stream.WaveStream.CurrentTime.ToString());
        }

        void BindAccessibilityEvents()
        {

        }

        void MetroAudioForm_DragDrop(object sender, DragEventArgs e)
        {
            m_Logger.Info("Item dropped into application");
        }

        void MetroAudioForm_DragEnter(object sender, DragEventArgs e)
        {
            m_Logger.Info("Item dragged into application; awaiting drop.");
        }

        void MetroAudioForm_MouseDoubleClickListBox(object sender, MouseEventArgs e)
        {
            if (ListBoxPlayList.Items.Count > 0 && ListBoxPlayList.SelectedIndex >= 0)
            {
                Rectangle selectedItemRectangle = ListBoxPlayList.GetItemRectangle(ListBoxPlayList.SelectedIndex);
                if (selectedItemRectangle.Contains(e.Location))
                {
                    ButtonPlay.PerformClick();
                }
            }
        }

        private void MetroAudioForm_Load(object sender, EventArgs e)
        {
            m_Logger.Info("Loading Application...");
        }

        private void ListBoxPlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = (ListBox)sender;
            m_Logger.Info("Playlist Selected Index Changed to {0}", listBox.SelectedIndex);
        }

        private AudioFile GetSelectedPlayListItem()
        {
            if (ListBoxPlayList.Items.Count > 0 && ListBoxPlayList.SelectedIndex >= 0)
            {
                var selectedAudio = ListBoxPlayList.Items[ListBoxPlayList.SelectedIndex] as AudioPlayListItem;
                if (selectedAudio != null)
                {
                    m_Logger.Info("Retrieving Playlist item: {0}", selectedAudio);
                    return audioPlayer.LoadSound(selectedAudio.Location);
                }
                else
                {
                    m_Logger.Warn("Playlist Item {0} returned back with null audio stream!", ListBoxPlayList.SelectedIndex);
                }
            }
            return null;
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            m_Logger.Info("Play button clicked!");
            if (currentMedia != null)
            {
                m_Logger.Info("Playing media {0} at playlist index {1}", currentMedia.ToString(), ListBoxPlayList.SelectedIndex);
                audioPlayer.Stop(currentMedia);
                currentMedia = GetSelectedPlayListItem();
                audioPlayer.Play(currentMedia);
                TrackBarProgress.Enabled = true;
            }
            else
            {
                currentMedia = GetSelectedPlayListItem();
                if (currentMedia != null)
                {
                    audioPlayer.Play(currentMedia);
                    TrackBarProgress.Enabled = true;
                }
                else
                {
                    m_Logger.Warn("Media was unable to load successfully.");
                }
            }
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            var currentButton = (MetroButton)sender;
            m_Logger.Info("Pause button clicked!");

            if (currentMedia != null)
            {
                audioPlayer.Pause(currentMedia);
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            var currentButton = (MetroButton)sender;
            m_Logger.Info("Stop button clicked!");
            if (currentMedia != null)
            {
                audioPlayer.Stop(currentMedia);
                TrackBarProgress.Enabled = false;
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            m_Logger.Info("Open button clicked!");
            var currentButton = (MetroButton)sender;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP3 File (.mp3)|*.mp3|Wave File (.wav)|*.wav|Free Lossless Audio (.flac)|*.flac|Audio Interchange File Format (.aiff)|*.aiff";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var files = fileDialog.FileNames;
                foreach (var file in files)
                {
                    if (audioPlayListItems.Where(p => p.Location == file).Count() == 0)
                    {
                        m_Logger.Info("Adding media: {0}", file);
                        audioPlayListItems.Add(new AudioPlayListItem(audioPlayer) { Location = file });
                    }
                    else
                    {
                        m_Logger.Info("Skipping media: {0}, already exists in playlist.", file);
                    }
                }
                audioPlayListItems.ResetBindings();
            }
        }

        private void ComboBoxOptionsTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentDropdown = (MetroComboBox)sender;
            MetroColorStyle selectedStyle = (MetroColorStyle)currentDropdown.SelectedItem;
            MetroStyleManager.Style = selectedStyle;
            m_Logger.Info("Theme changed to {0}", selectedStyle);
        }

        private void ButtonPrevious_Click(object sender, EventArgs e)
        {
            if (ListBoxPlayList.SelectedIndex > 0)
            {
                int previousItem = ListBoxPlayList.SelectedIndex - 1;
                m_Logger.Info("Clearing selected items before getting previous slot.");
                ListBoxPlayList.ClearSelected();
                ListBoxPlayList.SetSelected(previousItem, true);
                m_Logger.Info("Going to the previous playlist item {0}", ListBoxPlayList.SelectedIndex);
                ButtonPlay.PerformClick();
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            int itemIndex = ListBoxPlayList.SelectedIndex;
            int playListCount = ListBoxPlayList.Items.Count - 1;
            if (itemIndex >= 0 && itemIndex <= playListCount)
            {
                int nextItem = itemIndex + 1;
                nextItem = nextItem <= playListCount ? (nextItem) : (playListCount);
                m_Logger.Info("Clearing selected items before getting next slot.");
                ListBoxPlayList.ClearSelected();
                ListBoxPlayList.SetSelected(nextItem, true);
                m_Logger.Info("Going to the next playlist item {0}", ListBoxPlayList.SelectedIndex);
                ButtonPlay.PerformClick();
            }
        }

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            if (currentMedia != null)
            {
                double progress = currentMedia.Stream.WaveStream.CurrentTime.TotalSeconds / currentMedia.Stream.WaveStream.TotalTime.TotalSeconds;
                progress *= 100;
                TrackBarProgress.Value = (int)progress;
                LabelCurrentMedia.Text = currentMedia.Stream.WaveStream.CurrentTime.ToString(@"hh\:mm\:ss")
                    + " / " +
                    currentMedia.Stream.WaveStream.TotalTime.ToString(@"hh\:mm\:ss")
                    + " | " + currentMedia.ToString();
            }
        }

        private void TrackBarVolume_Scroll(object sender, ScrollEventArgs e)
        {
            var currentTrackBar = (MetroTrackBar)sender;
            if (currentMedia != null)
            {
                currentMedia.Volume = (float)(currentTrackBar.Value / 100.00f);
                currentMedia.Stream.DeviceHandle.Volume = currentMedia.Volume;
                m_Logger.Info("Volume settings changed to {0}", currentMedia.Volume);
            }
        }

        private void TrackBarProgress_Scroll(object sender, ScrollEventArgs e)
        {
            if (currentMedia != null)
            {
                int percentTrackBar = e.NewValue;
                var differenceTime = currentMedia.Stream.WaveStream.TotalTime.TotalSeconds * (percentTrackBar / 100.0);

                if (e.NewValue > e.OldValue)
                {
                    SeekUpdate = new TimeSpan(0, 0, (int)differenceTime);

                    m_Logger.Info("Seeking ahead: {0}", differenceTime);
                }
                else if (e.NewValue < e.OldValue)
                {
                    SeekUpdate = new TimeSpan(0, 0, -(int)differenceTime);

                    m_Logger.Info("Seeking behind: {0}", -differenceTime);
                }
                else
                {
                    SeekUpdate = TimeSpan.Zero;
                }
            }
        }

        
    }
}
