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
    public class Theme : IPackable, IPackWithDefaults, IPackWithSave
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
            /*for (int i = 0; i < Controls.Count; i++)
            {
                bw.Write(i);
                Controls[(ThemePart)i].Save(bw);
            }*/
            foreach (var pair in Controls) //2015.07.03.
            { //2015.07.03.
                //bw.Write(pair.Key.ToString());
                bw.Write(pair.Key.AssemblyQualifiedName); //2015.07.05.
                pair.Value.Save(bw);
            }
        }

        /*public enum ThemePart
        {
            MainBackgorund,
            MinimizeButton,
            MaximizeButton,
            CloseButton,
            Border,
            MenuBackground,
            NoImage
        }*/

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
        //public static event EventHandler ReloadEvent; //2014.12.24.
        public Dictionary<Type, List<Control>> ControlsByType = new Dictionary<Type, List<Control>>(); //2015.07.03.
        public static void ApplyTheme(Type onlythis = null)
        { //2015.07.03.
            var action = new Action<Type, List<Control>>((type, list) =>
            {
                foreach (Control control in list)
                    SkinControl(control);
            });
            if (onlythis == null)
            {
                foreach (var item in CurrentTheme.ControlsByType)
                    action(item.Key, item.Value);
            }
            else
                action(onlythis, CurrentTheme.ControlsByType[onlythis]);
        }

        //public static void SkinControl(ThemePart themepart, Control control)
        public static void SkinControl(Control control)
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
            //SkinThis(themepart, control.CreateGraphics()); //2015.05.22.
            using (var gr = control.CreateGraphics()) //2015.07.03.
                SkinThis(control.GetType(), gr); //2015.07.03.

        }

        //TODO: (2015.04.11.) Ne csak képeket tudjon kezelni, háttérszíneket, előtérszíneket, akár betűméretet/típust is
        //TODO: (2015.04.11.) Egyedi betűtípusokat is - Talán
        //private static Dictionary<ThemePart, Image> Images = new Dictionary<ThemePart, Image>();
        //private Dictionary<ThemePart, ThemeControl> Controls = new Dictionary<ThemePart, ThemeControl>();
        public Dictionary<Type, ThemeControl> Controls = new Dictionary<Type, ThemeControl>(); //2015.07.03.
        public static List<Theme> Themes = new List<Theme>();
        private static Theme currenttheme; //2015.05.22.
        public static Theme CurrentTheme
        {
            get
            { //2015.05.22.
                if (currenttheme == null)
                {
                    /*try
                    {
                        currenttheme = Themes.Single(entry => entry.Name == Storage.Settings[SettingType.Theme]);
                    }
                    catch { }*/
                    currenttheme = Themes.FirstOrDefault(entry => entry.Name == Storage.Settings[SettingType.Theme]); //Single-->FirstOrDefault: 2015.06.06.
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
            if (fs.Length < 5)
                return false; //2015.07.05.
            Name = br.ReadString(); //Név
            Description = br.ReadString(); //Leírás
            Creators = br.ReadString(); //Készítő(k)
            int count = br.ReadInt32(); //2015.05.03.
            for (int i = 0; i < count; i++)
            {
                //ThemePart id = (ThemePart)br.ReadInt32();
                string tmp = br.ReadString();
                //Type type = Type.GetType(br.ReadString()); //2015.07.03.
                Type type = Type.GetType(tmp);
                ThemeControl control = new ThemeControl(br);
                //Controls.Add(id, control);
                Controls.Add(type, control); //2015.07.03.
            }
            Themes.Add(this);
            if (CurrentTheme == null) //TODO: TMP
                CurrentTheme = this; //2015.07.05.
            return true;
        }

        public void UnloadFromPack()
        {
            //Ha törli az egységes listából, törli az egész rendszerből
        }

        //public static void SkinThis(ThemePart part, Graphics graphics)
        public static void SkinThis(Type controltype, Graphics graphics) //controltype: 2015.07.03.
        { //2015.05.22.
            if (CurrentTheme == null)
                return;
            //ThemeControl control = CurrentTheme.Controls[part];
            if(!CurrentTheme.Controls.ContainsKey(controltype))
            { //2015.07.03.
                //MessageBox.Show("Cannot theme this control type: " + controltype); //TODO: Több téma támogatása egyszerre (Minecraft resource pack)
                return;
            }
            ThemeControl control = CurrentTheme.Controls[controltype];
            switch (control.ControlType)
            {
                case ThemeControlTypes.Colors:
                    //graphics.FillRectangle(new LinearGradientBrush(new Point(), new Point(graphics.ClipBounds.Size.ToSize()), control.Colors[0], control.Colors[1]), graphics.ClipBounds);
                    graphics.FillRectangle(new SolidBrush(control.Color), graphics.ClipBounds); //2015.07.05.
                    break;
                case ThemeControlTypes.Dynamic: //2015.07.05.
                    IScriptTheme script = null; //TODO
                    script.SkinThis(controltype, control, graphics);
                    break;
                case ThemeControlTypes.Image: //2015.07.05.
                    graphics.DrawImage(control.Image, new Rectangle(new Point(), graphics.ClipBounds.Size.ToSize()));
                    break;
            }
            //ReloadEvent += delegate { SkinThis(part, graphics); };
            //ReloadEvent += delegate { SkinThis(controltype, graphics); };
        }
        public static void SetRenderer(ToolStrip toolstrip) //2015.07.05.
        { //TODO

        }
        public override string ToString()
        { //2015.07.03.
            return Name;
        }

        public void AddPack(string filename)
        {
            FileName = filename;
            Name = Path.GetFileNameWithoutExtension(filename); //2015.07.05.
            Description = ""; //TODO - 2015.07.05.
            Creators = ""; //2015.07.05.
        }

        public void SavePack(string filename)
        { //2015.07.03.
            Save(filename);
        }

        public string FileName
        { //2015.07.03.
            get;
            set;
        }
    }
}
