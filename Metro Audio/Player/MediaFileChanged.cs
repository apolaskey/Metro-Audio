using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player
{
    public class MediaFileChanged: EventArgs
    {
        public AudioPlayListItem OldItem;
        public AudioPlayListItem NewItem;
    }
}
