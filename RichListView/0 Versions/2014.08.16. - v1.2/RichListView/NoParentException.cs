using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SzNPProjects
{
    class NoParentException : Exception
    {
        /*public override System.Collections.IDictionary Data
        {
            get
            {
                var ret = new Dictionary<string, string>();
                ret["details"] = "Try to reference ListViewItem which is not assigned to a ListView control.";
                return ret;
            }
        }
        public override string Message
        {
            get
            {
                return "Try to reference ListViewItem which is not assigned to a ListView control.";
            }
        }*/
        public NoParentException(string message) : base(message)
        {
        }
    }
}
