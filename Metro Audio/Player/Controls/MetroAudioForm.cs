using Experia.Framework.Audio;
using Experia.Framework.Debug;
using MetroAudio.Player;
using MetroAudio.Player.Controls;
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

        
        private PlayButton m_PlayButton;
        private PauseButton m_PauseButton;
        private StopButton m_StopButton;

        private OpenFileButton m_OpenFileButton;
        private RemoveButton m_RemoveButton;
        private SaveButton m_SaveButton;

        private NextItemButton m_NextButton;
        private PreviousItemButton m_PreviousButton;

        private VolumeControl m_VolumeControl;
        private MediaSeekBarControl m_SeekBarControl;

        private AudioPlayer audioPlayer;
        public AudioPlayer Player
        {
            get { return audioPlayer; }
        }

        private AudioFile currentMedia;
        public AudioFile LoadedMedia
        {
            get { return currentMedia; }
            set { currentMedia = value; }
        }
        private TimeSpan SeekUpdate;

        private BindingList<AudioPlayListItem> audioPlayListItems = new BindingList<AudioPlayListItem>();
        public BindingList<AudioPlayListItem> PlayList
        {
            get { return audioPlayListItems; }
        }

        private event EventHandler<MediaFileChanged> m_MediaChanged;

        private KeyboardHook m_KeyboardHook = new KeyboardHook();

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
            m_KeyboardHook.KeyPressed += m_KeyboardHook_KeyPressed;
            RadioButtonKeysBound.Checked = m_Register_Hotkeys();

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

            this.GotFocus += MetroAudioForm_GotFocus;

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

        void MetroAudioForm_GotFocus(object sender, EventArgs e)
        {
            RadioButtonKeysBound.Checked = m_Register_Hotkeys();
        }

        private bool m_Register_Hotkeys()
        {
            bool bound = false;
            try
            {
                m_KeyboardHook.RegisterHotKey(MetroAudio.ModifierKeys.None, Keys.MediaNextTrack);
                m_KeyboardHook.RegisterHotKey(MetroAudio.ModifierKeys.None, Keys.MediaPreviousTrack);
                m_KeyboardHook.RegisterHotKey(MetroAudio.ModifierKeys.None, Keys.MediaPlayPause);
                m_KeyboardHook.RegisterHotKey(MetroAudio.ModifierKeys.None, Keys.MediaStop);
                bound = true;
            }
            catch(Exception e) {
                m_Logger.Warn("Failed to register Media Hotkeys; another app has most likely claimed them.");
                bound = false;
            }

            return bound;
        }

        void m_KeyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            m_Logger.Info("Global Key: {0} pressed!", e.Key);
        }

        private void EventFormLoad(object sender, EventArgs e)
        {
            MetroAudioForm form = sender as MetroAudioForm;
            m_Logger.Info("Loading Application...");

            m_VolumeControl = new VolumeControl(this.TrackBarVolume);
            m_SeekBarControl = new MediaSeekBarControl(this.TrackBarProgress);

            m_PlayButton = new PlayButton(this.ButtonPlay);
            m_StopButton = new StopButton(this.ButtonStop);
            m_PauseButton = new PauseButton(this.ButtonPause);

            m_NextButton = new NextItemButton(this.ButtonNext);
            m_PreviousButton = new PreviousItemButton(this.ButtonPrevious);

            m_OpenFileButton = new OpenFileButton(this.ButtonOpen);
            m_RemoveButton = new RemoveButton(this.ButtonRemove);
            m_SaveButton = new SaveButton(this.ButtonSave);
        }

        private void EventUpdateForm(object sender, EventArgs e)
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
                TrackBarProgress.Enabled = false;

                if (currentMedia.Stream.WaveStream.CurrentTime >= currentMedia.Stream.WaveStream.TotalTime)
                {
                    if (ListBoxPlayList.SelectedIndex == ListBoxPlayList.Items.Count - 1)
                    {
                        if (ToggleOptionsRepeat.Checked)
                        {
                            ButtonNext.PerformClick();
                        }
                    }
                    else
                    {
                        ButtonNext.PerformClick();
                    }
                }
            }
        }

        void TrackBarProgress_MouseUp(object sender, MouseEventArgs e)
        {
            currentMedia.Stream.WaveStream.CurrentTime = currentMedia.Stream.WaveStream.CurrentTime + SeekUpdate;
            m_Logger.Info("Adjusting Media progress seek to: {0}", currentMedia.Stream.WaveStream.CurrentTime.ToString());
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

        private void ListBoxPlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = (ListBox)sender;
            m_Logger.Info("Playlist Selected Index Changed to {0}", listBox.SelectedIndex);
        }

        public AudioFile GetSelectedPlayListItem()
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

        private void ComboBoxOptionsTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentDropdown = (MetroComboBox)sender;
            MetroColorStyle selectedStyle = (MetroColorStyle)currentDropdown.SelectedItem;
            MetroStyleManager.Style = selectedStyle;
            m_Logger.Info("Theme changed to {0}", selectedStyle);
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
