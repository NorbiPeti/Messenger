﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    class ErrorHandling
    {
        public static void FormError(Form fname, Exception e)
        {
            MessageBox.Show(e.GetType().ToString());
            if (fname == Program.MainF)
            {
                switch (e.GetType().ToString())
                {
                    /*case "System.NullReferenceException":
                        MessageBox.Show("lol");
                        break;*/
                    default:
                        MessageBox.Show("Ismeretlen hiba történt (" + e.GetType().ToString() + ")!\n\nForrás: " + e.Source + "\nA hibaüzenet: \n" + e.Message + "\nEnnél a műveletnél: " + e.TargetSite);
                        break;
                }
            }
            else if(fname==MainForm.LoginDialog)
            {
                switch (e.GetType().ToString())
                {
                    /*case "System.NullReferenceException":
                        MessageBox.Show("lol");
                        break;*/
                    default:
                        MessageBox.Show("Ismeretlen hiba történt (" + e.GetType().ToString() + ")!\n\nForrás: " + e.Source + "\nA hibaüzenet: \n" + e.Message + "\nEnnél a műveletnél: " + e.TargetSite);
                        break;
                }
            }
        }
    }
}