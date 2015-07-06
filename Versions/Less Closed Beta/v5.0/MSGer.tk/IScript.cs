using System;
using System.Collections.Generic;
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
}
