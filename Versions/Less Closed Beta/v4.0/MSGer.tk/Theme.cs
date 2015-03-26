using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public class Theme
    {
        public void Save(string themepath)
        {
            List<byte> bytes = new List<byte>();
            foreach (var item in Images)
            {
                bytes.AddRange(BitConverter.GetBytes((int)item.Key));
                var ms = new MemoryStream();
                item.Value.Save(ms, ImageFormat.Tiff);
                byte[] img = ms.ToArray();
                ms.Dispose();
                bytes.AddRange(BitConverter.GetBytes(img.Length));
                bytes.AddRange(img);
            }
            File.WriteAllBytes(themepath, bytes.ToArray());
        }

        public enum ThemePart
        {
            MainBackgorund,
            MinimizeButton,
            MaximizeButton,
            CloseButton,
            Border
        }
        
        public Theme(string themepath)
        {
            //FileStream fs = new FileStream(themepath, FileMode.Open);
            Images.Clear();
            byte[] bytes = File.ReadAllBytes(themepath);
            int i = 0;
            while (i < bytes.Length)
            {
                int part = BitConverter.ToInt32(bytes, i); //4 byte ThemePart
                i += sizeof(int);
                int len = BitConverter.ToInt32(bytes, i); //4 byte imglen
                i += sizeof(int);
                Image img = Image.FromStream(new MemoryStream(bytes, i, len)); //len byte image
                new Theme((ThemePart)part, img);
            }
            ReloadEvent(null, null);
        }
        private Theme(ThemePart themepart, Image image)
        {
            Images.Add(themepart, image);
            //Frissítse az összes helyen a képeket (lásd Language osztály) - Csak ne itt
        }
        public static event EventHandler ReloadEvent; //2014.12.24.

        public static void SkinControl(ThemePart themepart, Control control)
        {
            if (control != null && !control.IsDisposed && Images.ContainsKey(themepart))
            {
                control.BackgroundImage = Images[themepart];
                if (themepart == ThemePart.MainBackgorund)
                {
                    foreach(Control c in control.GetAll())
                    {
                        Bitmap bmp = new Bitmap(Images[themepart]).Clone(new Rectangle(c.Location.X, c.Location.Y, c.Width, c.Height), PixelFormat.Format32bppRgb);
                        c.BackgroundImage = bmp;
                    }
                }
            }
            ReloadEvent += delegate
            { //Ugyanazt az indexű képet fogja használni, csak a kép változik meg
                if (control != null && !control.IsDisposed && Images.ContainsKey(themepart))
                {
                    control.BackgroundImage = Images[themepart];
                }
            };
        }

        private static Dictionary<ThemePart, Image> Images = new Dictionary<ThemePart, Image>();
    }
}
