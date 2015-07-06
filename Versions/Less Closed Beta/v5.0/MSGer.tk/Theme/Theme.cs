using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using MSGer.tk.Theme;

namespace MSGer.tk
{
    public class Theme : IPackable
    {
        public void Save(string themepath)
        {
            /*List<byte> bytes = new List<byte>();
            //foreach (var item in Images)
            foreach(var item in Controls)
            {
                bytes.AddRange(BitConverter.GetBytes((int)item.Key));
                var ms = new MemoryStream();
                switch(item.Value.ControlType)
                {
                    case ThemeControlTypes.Image:
                        break;
                }
                item.Value.Image.Save(ms, ImageFormat.Tiff);
                byte[] img = ms.ToArray();
                ms.Dispose();
                bytes.AddRange(BitConverter.GetBytes(img.Length));
                bytes.AddRange(img);
            }
            File.WriteAllBytes(themepath, bytes.ToArray());*/
            //2015.05.03.
            var fs = File.Open(themepath, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(Name); //Név
            bw.Write(Description); //Leírás
            bw.Write(Creators); //Készítő(k)
            bw.Write(Controls.Count);
            for (int i = 0; i < Controls.Count; i++)
            {
                bw.Write(i);
                Controls[(ThemePart)i].Save(bw);
            }
        }

        public enum ThemePart
        {
            MainBackgorund,
            MinimizeButton,
            MaximizeButton,
            CloseButton,
            Border,
            MenuBackground
        }
        
        /*public Theme(string themepath)
        {
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
        }*/
        private Theme()
        { //2015.04.11.

        }
        public static event EventHandler ReloadEvent; //2014.12.24.

        public static void SkinControl(ThemePart themepart, Control control)
        {
            /*if (control != null && !control.IsDisposed && Images.ContainsKey(themepart))
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
            };*/
            SkinThis(themepart, control.CreateGraphics()); //2015.05.22.
        }

        //TODO: (2015.04.11.) Ne csak képeket tudjon kezelni, háttérszíneket, előtérszíneket, akár betűméretet/típust is
        //TODO: (2015.04.11.) Egyedi betűtípusokat is - Talán
        //private static Dictionary<ThemePart, Image> Images = new Dictionary<ThemePart, Image>();
        private Dictionary<ThemePart, ThemeControl> Controls = new Dictionary<ThemePart, ThemeControl>();
        private static List<Theme> Themes = new List<Theme>();
        private static Theme currenttheme; //2015.05.22.
        public static Theme CurrentTheme
        {
            get
            { //2015.05.22.
                if (currenttheme == null)
                {
                    try
                    {
                        currenttheme = Themes.Single(entry => entry.Name == Storage.Settings[SettingType.Theme]);
                    }
                    catch { }
                }
                return currenttheme;
            }
            set
            { //2015.05.22.
                currenttheme = value;
                Storage.Settings[SettingType.Theme] = currenttheme.Name;
            }
        }

        public string Name;
        public string Description;
        public string Creators;

        public bool LoadFromPack(string filename)
        {
            var fs = File.Open(filename, FileMode.Open);
            var br = new BinaryReader(fs);
            Name = br.ReadString(); //Név
            Description = br.ReadString(); //Leírás
            Creators = br.ReadString(); //Készítő(k)
            int count = br.ReadInt32(); //2015.05.03.
            for (int i = 0; i < count; i++)
            {
                ThemePart id = (ThemePart)br.ReadInt32();
                ThemeControl control = new ThemeControl(br);
                Controls.Add(id, control);
            }
            Themes.Add(this);
            return true;
        }

        public void UnloadFromPack()
        {
            //Ha törli az egységes listából, törli az egész rendszerből
        }

        public static void SkinThis(ThemePart part, Graphics graphics)
        { //2015.05.22.
            if (CurrentTheme == null)
                return;
            ThemeControl control = CurrentTheme.Controls[part];
            switch (control.ControlType)
            {
                case ThemeControlTypes.Colors:
                    graphics.FillRectangle(new LinearGradientBrush(new Point(), new Point(graphics.ClipBounds.Size.ToSize()), control.Colors[0], control.Colors[1]), graphics.ClipBounds);
                    break;
                case ThemeControlTypes.Dynamic:
                    break;
                case ThemeControlTypes.Image:
                    break;
            }
        }
    }
}
