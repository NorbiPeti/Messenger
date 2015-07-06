using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public class SettingsPanel : UserControl
    {
        /// <summary>
        /// SaveSettings
        /// </summary>
        /// <returns>Reopen</returns>
        public virtual bool SaveSettings()
        {
            throw new InvalidOperationException("SaveSettings is not overriden."); //2015.05.23.
        }
    }
}
