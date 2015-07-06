using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public interface IPackable
    {
        bool LoadFromPack(string filename);
        void UnloadFromPack();
        //void SaveToPack(string filename);
    }
}
