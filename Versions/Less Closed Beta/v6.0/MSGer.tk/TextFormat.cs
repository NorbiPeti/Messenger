using Khendys.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSGer.tk
{
    class TextFormat : IPackable, IPackWithSave
    {
        private TextFormat()
        {
        }
        /*public TextFormat()
        {
            var img = Image.FromFile("emoticons\\iconbase.png");
            var bitmap = new Bitmap(img);
            int x = 0;
            Bitmap bitmap1 = bitmap.Clone(new Rectangle(x, 0, 19, 19), bitmap.PixelFormat);
            x += 19;
            new TextFormat(":)", bitmap1);
            new TextFormat(":-)", bitmap1);
            bitmap1 = bitmap.Clone(new Rectangle(x, 0, 19, 19), bitmap.PixelFormat);
            x += 19;
            new TextFormat(":D", bitmap1);
            new TextFormat(":-D", bitmap1);
            new TextFormat(":d", bitmap1);
            new TextFormat(":-d", bitmap1);
            bitmap1 = bitmap.Clone(new Rectangle(x, 0, 19, 19), bitmap.PixelFormat);
            x += 19;
            new TextFormat(";)", bitmap1);
            new TextFormat(";-)", bitmap1);
            bitmap1 = bitmap.Clone(new Rectangle(x, 0, 19, 19), bitmap.PixelFormat);
            x += 19;
            new TextFormat(":O", bitmap1);
            new TextFormat(":o", bitmap1);
            new TextFormat(":-O", bitmap1);
            new TextFormat(":-o", bitmap1);
            bitmap1 = bitmap.Clone(new Rectangle(x, 0, 19, 19), bitmap.PixelFormat);
            x += 19;
            new TextFormat(":P", bitmap1);
            new TextFormat(":-P", bitmap1);
            new TextFormat(":p", bitmap1);
            new TextFormat(":-p", bitmap1);

            bitmap.Dispose();
        }
        private TextFormat(string text, Image image)
        {
            var tmp = new ImgReplaceStrs();
            tmp.Text = text;
            tmp.Image = image;
            ImgReplaceStrings.Add(tmp);
        }*/
        //public static List<ImgReplaceStrs> ImgReplaceStrings = new List<ImgReplaceStrs>();
        /*public static ExRichTextBox Parse(ExRichTextBox textbox)
        {
            *for (int i = 0; i < ImgReplaceStrings.Count; i++)
            { //2014.10.12.
                int index = 0;
                while ((index = textbox.Text.IndexOf(ImgReplaceStrings[i].Text)) != -1)
                {
                    textbox.Select(index, ImgReplaceStrings[i].Text.Length);
                    textbox.InsertImage(ImgReplaceStrings[i].Image);
                }
            }*
            //foreach (var item in TextFormats)
            foreach (var item in GetEmoticons()) //2015.06.06.
            { //2015.05.24.
                int index = 0;
                while ((index = textbox.Text.IndexOf(item.Value)) != -1)
                {
                    textbox.Select(index, item.Value.Length);
                    textbox.InsertImage(item.Frames[0]); //TO!DO: Animált hangulatjelek - Akár saját TextBox
                }
            }
            return textbox;
        }*/

        //private static Dictionary<string, TextFormat> TextFormats = new Dictionary<string, TextFormat>(); //2015.05.24.

        public static List<TextFormat> TextFormats = new List<TextFormat>(); //2015.06.06.
        private static IEnumerable<Emoticon> GetEmoticons()
        { //2015.06.06.
            return TextFormats.SelectMany(entry => entry.Emoticons);
        }

        //private List<Bitmap> Frames = new List<Bitmap>(); //2015.05.24.
        public List<Emoticon> Emoticons = new List<Emoticon>(); //2015.06.06.
        public bool LoadFromPack(string filename) //2015.05.03.
        { //2015.05.24.
            /*switch (Path.GetExtension(filename))
            {
                case ".txt":
                    return false; //Ne tartsa betöltve
                default:
                    break;
            }*/
            //var img = Image.FromFile(filename);
            if (Path.GetExtension(filename) != ".npack")
                return false; //2015.06.06.
            var fs = File.Open(filename, FileMode.Open); //2015.06.06.
            if (fs.Length <= 10)
            {
                fs.Dispose();
                return false; //2015.06.06. - TODO: Hasonló ellenőrzéseket a többi packhoz is
            }
            var br = new BinaryReader(fs); //2015.06.06.
            int count = br.ReadInt32(); //2015.06.06.
            for (int i = 0; i < count; i++)
            {
                string id = br.ReadString(); //2015.06.06.
                //var ms = new MemoryStream(); //2015.06.06.
                //fs.CopyTo(ms); //2015.06.06.
                int len = br.ReadInt32(); //2015.06.06.
                byte[] buf = new byte[len]; //2015.06.06.
                fs.Read(buf, 0, len); //2015.06.06.
                //fs.Dispose(); //2015.06.06.
                //br.Dispose(); //2015.06.06.
                var ms = new MemoryStream(buf); //2015.06.06.
                ms.Seek(0, SeekOrigin.Begin); //2015.06.06.
                //var img = Image.FromStream(ms); //2015.06.06.
                //var bitmap = new Bitmap(img);
                var bitmap = new Bitmap(ms);
                ms.Dispose(); //2015.06.06.
                Emoticon emoticon = new Emoticon(id); //2015.06.06.
                /*for (int x = 0; x < bitmap.Width / bitmap.Height; x += bitmap.Height)
                    Frames.Add(bitmap.Clone(new Rectangle(x, 0, bitmap.Height, bitmap.Height), bitmap.PixelFormat));*/
                bitmap.MakeTransparent(Color.White); //2015.06.06.
                //for (int x = 0; x < bitmap.Width / bitmap.Height; x += bitmap.Height)
                    //emoticon.Frames.Add(bitmap.Clone(new Rectangle(x, 0, bitmap.Height, bitmap.Height), bitmap.PixelFormat)); //2015.06.06.
                emoticon.Image = (Bitmap)bitmap.Clone(); //2015.07.05.
                bitmap.Dispose(); //2015.07.05.
                Emoticons.Add(emoticon); //2015.06.06.
            }
            Name = Path.GetFileNameWithoutExtension(filename); //2015.06.06.
            //TextFormats.Add(File.ReadAllText(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filename) + ".txt"), this);
            TextFormats.Add(this); //2015.06.06.
            fs.Dispose(); //2015.06.06.
            br.Dispose(); //2015.06.06.
            return true;
        }

        public void UnloadFromPack() //2015.05.03.
        { //2015.05.24.
            foreach (var entry in Emoticons) //<-- 2015.06.06.
                /*foreach (var item in entry.Frames)
                    item.Dispose();*/
                entry.Image.Dispose(); //2015.07.05.
        }

        private string Name;

        public override string ToString()
        { //2015.06.06.
            return Name;
        }

        public void AddPack(string filename)
        {
            FileName = filename;
        }

        public void SavePack(string filename)
        { //2015.06.06.
            var fs = File.Open(filename, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(Emoticons.Count);
            for (int i = 0; i < Emoticons.Count; i++)
            {
                bw.Write(Emoticons[i].Value);
                var ms = new MemoryStream();
                /*int width = 0;
                Emoticons[i].Frames.ForEach(entry => width += entry.Width);*/
                //Bitmap finalimg = new Bitmap(Emoticons[i].Frames[0].Height * Emoticons[i].Frames.Count, Emoticons[i].Frames[0].Height);
                /*Graphics gr = Graphics.FromImage(finalimg);
                for (int x = 0; x < finalimg.Width / finalimg.Height; x += finalimg.Height)
                    gr.DrawImage(Emoticons[i].Frames[x], x, 0);
                finalimg.Save(ms, ImageFormat.Tiff);*/
                Emoticons[i].Image.Save(ms, ImageFormat.Gif); //2015.07.05.
                bw.Write((int)ms.Length);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fs);
                ms.Dispose();
            }
            fs.Dispose();
        }

        public string FileName
        {
            get;
            set;
        }
    }
    /*class ImgReplaceStrs
    {
        public string Text;
        public Image Image;
    }*/
    public class Emoticon
    { //2015.06.06.
        //public List<Bitmap> Frames = new List<Bitmap>(); //TODO: Ehelyett GIF
        public Bitmap Image; //2015.07.05.
        public string Value;
        public Emoticon(string value)
        {
            Value = value;
        }
    }
}
