using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzNPProjects
{
    public class ItemClickEventArgs : EventArgs
    { //2015.08.23.
        public int ItemIndex { get; private set; }
        public ItemClickEventArgs(int itemindex)
        {
            ItemIndex = itemindex;
        }
    }
}
