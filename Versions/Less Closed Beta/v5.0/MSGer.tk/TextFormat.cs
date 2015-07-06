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
    class TextFormat : IPackable
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
        public static ExRichTextBox Parse(ExRichTextBox textbox)
        {
            /*for (int i = 0; i < ImgReplaceStrings.Count; i++)
            { //2014.10.12.
                int index = 0;
                while ((index = textbox.Text.IndexOf(ImgReplaceStrings[i].Text)) != -1)
                {
                    textbox.Select(index, ImgReplaceStrings[i].Text.Length);
                    textbox.InsertImage(ImgReplaceStrings[i].Image);
                }
            }*/
            foreach (var item in TextFormats)
            { //2015.05.24.
                int index = 0;
                while ((index = textbox.Text.IndexOf(item.Key)) != -1)
                {
                    textbox.Select(index, item.Key.Length);
                    textbox.InsertImage(item.Value.Frames[0]); //TODO: Animált hangulatjelek - Akár saját TextBox
                }
            }
            return textbox;
        }

        private static Dictionary<string, TextFormat> TextFormats = new Dictionary<string, TextFormat>(); //2015.05.24.

        private List<Bitmap> Frames = new List<Bitmap>(); //2015.05.24.
        public bool LoadFromPack(string filename) //2015.05.03.
        { //2015.05.24.
            switch (Path.GetExtension(filename))
            {
                case ".txt":
                    return false; //Ne tartsa betöltve
                default:
                    break;
            }
            var img = Image.FromFile(filename);
            var bitmap = new Bitmap(img);
            for (int x = 0; x < bitmap.Width / bitmap.Height; x += bitmap.Height)
                Frames.Add(bitmap.Clone(new Rectangle(x, 0, bitmap.Height, bitmap.Height), bitmap.PixelFormat));
            TextFormats.Add(File.ReadAllText(Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filename) + ".txt"), this);
            return true;
        }

        public void UnloadFromPack() //2015.05.03.
        { //2015.05.24.
            foreach (var item in Frames)
                item.Dispose();
        }
    }
    /*class ImgReplaceStrs
    {
        public string Text;
        public Image Image;
    }*/
}
