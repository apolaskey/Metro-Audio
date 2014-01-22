using Experia.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio
{
    public class AudioPlayListItem
    {
        private AudioPlayer player;
        public AudioPlayListItem(AudioPlayer player)
        {
            this.player = player;
        }

        public String Name
        {
            get;
            private set;
        }

        private String m_Location;
        public String Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                Name = value.Split('\\').Last();
            }
        }
    }
}
