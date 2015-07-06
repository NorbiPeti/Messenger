using Khendys.Controls;
using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
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

        public static List<TextFormat> TextFormats = new List<TextFormat>(); //2015.06.06.
        private static IEnumerable<Emoticon> GetEmoticons()
        { //2015.06.06.
            return TextFormats.SelectMany(entry => entry.Emoticons);
        }

        public List<Emoticon> Emoticons = new List<Emoticon>(); //2015.06.06.
        public bool LoadFromPack(string filename) //2015.05.03.
        { //2015.05.24.
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
                int len = br.ReadInt32(); //2015.06.06.
                byte[] buf = new byte[len]; //2015.06.06.
                fs.Read(buf, 0, len); //2015.06.06.
                var ms = new MemoryStream(buf); //2015.06.06.
                ms.Seek(0, SeekOrigin.Begin); //2015.06.06.
                var bitmap = new Bitmap(ms);
                ms.Dispose(); //2015.06.06.
                Emoticon emoticon = new Emoticon(id); //2015.06.06.
                bitmap.MakeTransparent(Color.White); //2015.06.06.
                emoticon.Image = (Bitmap)bitmap.Clone(); //2015.07.05.
                bitmap.Dispose(); //2015.07.05.
                Emoticons.Add(emoticon); //2015.06.06.
            }
            Name = Path.GetFileNameWithoutExtension(filename); //2015.06.06.
            TextFormats.Add(this); //2015.06.06.
            fs.Dispose(); //2015.06.06.
            br.Dispose(); //2015.06.06.
            return true;
        }

        public void UnloadFromPack() //2015.05.03.
        { //2015.05.24.
            foreach (var entry in Emoticons) //<-- 2015.06.06.
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
    public class Emoticon
    { //2015.06.06.
        public Bitmap Image; //2015.07.05.
        public string Value;
        public Emoticon(string value)
        {
            Value = value;
        }
    }
}
