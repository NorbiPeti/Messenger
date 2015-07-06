using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public interface IScript
    {
        void Load();
        void Unload();
    }

    public interface IScriptTheme
    { //2015.07.05.
        void SkinThis(Type controltype, ThemeControl control, Graphics graphics);
    }
}
