using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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
    }
    public interface IPackWithSave
    {
        void AddPack(string filename);
        void SavePack(string filename);
        string FileName { get; set; }
    }
    public interface IPackWithDefaults
    { //2015.06.14.
    }
}
