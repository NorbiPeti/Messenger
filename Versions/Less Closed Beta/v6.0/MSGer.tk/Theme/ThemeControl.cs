using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace MSGer.tk
{
    public class ThemeControl
    { //2015.04.11.
        public ThemeControlTypes ControlType { get; private set; } //2015.05.03.
        public Image Image { get; private set; } //2015.05.03.
        public string Script { get; private set; } //2015.05.03.
        //public List<Color> Colors { get; private set; } //2015.05.03.
        public Color Color { get; private set; } //2015.07.05.
        public Color ForeColor { get; private set; } //2015.05.16.

        public ThemeControl(BinaryReader br)
        {
            var type = (ThemeControlTypes)br.ReadInt32();
            ControlType = type; //2015.05.16.
            ForeColor = Color.FromArgb(br.ReadInt32()); //2015.05.16.
            switch (type)
            {
                case ThemeControlTypes.Dynamic:
                    { //2015.05.16.
                        Script = br.ReadString();
                        break;
                    }
                case ThemeControlTypes.Image:
                    { //2015.05.16.
                        int len = br.ReadInt32();
                        byte[] imgdata = br.ReadBytes(len);
                        MemoryStream ms = new MemoryStream(imgdata);
                        Image = Image.FromStream(ms);
                        break;
                    }
                case ThemeControlTypes.Colors:
                    { //2015.05.22.
                        ForeColor = Color.FromArgb(br.ReadInt32());
                        /*Colors = new List<Color>(br.ReadInt32());
                        for (int i = 0; i < Colors.Count; i++)
                        {
                            Colors[i] = Color.FromArgb(br.ReadInt32());
                        }*/
                        Color = Color.FromArgb(br.ReadInt32()); //2015.07.05.
                        break;
                    }
            }
        }

        public ThemeControl(Color forecolor, Image image)
        { //2015.07.04.
            ControlType = ThemeControlTypes.Image; //2015.07.05.
            ForeColor = forecolor;
            Image = image;
        }

        //public ThemeControl(Color forecolor, IEnumerable<Color> colors)
        public ThemeControl(Color forecolor, Color color)
        { //2015.07.04.
            ControlType = ThemeControlTypes.Colors; //2015.07.05.
            ForeColor = forecolor;
            //Colors = new List<Color>(colors);
            Color = color; //2015.07.05.
        }

        public ThemeControl(Color forecolor, string script)
        { //2015.07.04.
            ControlType = ThemeControlTypes.Dynamic; //2015.07.05.
            ForeColor = forecolor;
            Script = script;
        }

        public void Save(BinaryWriter bw)
        { //2015.05.16.
            bw.Write((int)ControlType);
            bw.Write(ForeColor.ToArgb());
            switch (ControlType)
            {
                case ThemeControlTypes.Dynamic:
                    {
                        bw.Write(Script);
                        break;
                    }
                case ThemeControlTypes.Image:
                    {
                        MemoryStream ms = new MemoryStream();
                        Image.Save(ms, ImageFormat.Tiff);
                        bw.Write(ms.Length);
                        bw.Write(ms.ToArray());
                        break;
                    }
                case ThemeControlTypes.Colors:
                    { //2015.05.22.
                        bw.Write(ForeColor.ToArgb());
                        //bw.Write(Colors.Count);
                        //Colors.ForEach(entry => bw.Write(entry.ToArgb()));
                        bw.Write(Color.ToArgb()); //2015.07.05.
                        break;
                    }
            }
        }
    }
    public enum ThemeControlTypes
    { //2015.04.11.
        Dynamic,
        Image,
        Colors
    }
}
