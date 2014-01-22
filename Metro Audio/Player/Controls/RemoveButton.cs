using Experia.Framework.Debug;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroAudio.Player.Controls
{
    public class RemoveButton
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();
        MetroAudioForm form;
        MetroButtonBase button;

        public RemoveButton(MetroButtonBase button)
        {
            this.button = button;
            this.form = button.FindForm() as MetroAudioForm;
        }
    }
}
