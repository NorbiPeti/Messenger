using Khendys.Controls;
using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HdSystemLibrary.IO;

namespace MSGer.tk
{
    public partial class ChatForm : ThemedForms
    {
        public ChatForm()
        {
            InitializeComponent();
            //Amint létrehozom, ez a kód lefut - Nem számit, hogy megjelenik-e

            this.Text = Language.Translate(Language.StringID.Chat_Title, this);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
