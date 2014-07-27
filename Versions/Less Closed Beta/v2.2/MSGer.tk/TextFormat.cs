using Khendys.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSGer.tk
{
    class TextFormat
    {
        public TextFormat()
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
        public TextFormat(string text, Image image)
        {
            var tmp = new ImgReplaceStrs();
            tmp.Text = text;
            tmp.Image = image;
            ImgReplaceStrings.Add(tmp);
        }
        public static List<ImgReplaceStrs> ImgReplaceStrings = new List<ImgReplaceStrs>();
        public static ExRichTextBox Parse(ExRichTextBox textbox)
        {
            int j;
            do
            {
                j = 0;
                for (int i = 0; i < ImgReplaceStrings.Count; i++)
                {
                    int index = textbox.Text.IndexOf(ImgReplaceStrings[i].Text);
                    if (index >= 0)
                    { //Feldolgozza a hangualtjeleket
                        textbox.Select(index, ImgReplaceStrings[i].Text.Length);
                        textbox.InsertImage(ImgReplaceStrings[i].Image);
                        //set = true;
                    }
                    else j++;
                }
            }
            while (j != ImgReplaceStrings.Count);
            return textbox;
        }
        public static ExExRichTextBox Parse(ExExRichTextBox textbox)
        {
            int pos = textbox.SelectionStart;
            int len = textbox.SelectionLength;
            ((ChatForm)textbox.Parent).InternalMessageChange = true;
            textbox.Rtf = TextFormat.removeRtfObjects(textbox.Rtf);
            ((ChatForm)textbox.Parent).InternalMessageChange = false;
                for (int i = 0; i < ImgReplaceStrings.Count; i++)
                {
                    for (int j = 0; textbox.Text.IndexOf(ImgReplaceStrings[i].Text, j + 1) != -1; j = textbox.Text.IndexOf(ImgReplaceStrings[i].Text, j + 1) + 1)
                    {
                        int index = textbox.Text.IndexOf(ImgReplaceStrings[i].Text, j) - 1;
                        if (index >= 0)
                        { //Feldolgozza a hangualtjeleket
                            ((ChatForm)textbox.Parent).InternalMessageChange = true;
                            textbox.Text = textbox.Text.Remove(index, ImgReplaceStrings[i].Text.Length);
                            textbox.Select(index, 0);
                            TextFormat.InsertHiddenText(textbox, ImgReplaceStrings[i].Text);
                            textbox.Select(index + ImgReplaceStrings[i].Text.Length, 0);
                            textbox.InsertImage(ImgReplaceStrings[i].Image);
                            ((ChatForm)textbox.Parent).InternalMessageChange = false;
                        }
                    }
                }
            textbox.Select(pos, len);
            return textbox;
        }
        public static ExExRichTextBox InsertHiddenText(ExExRichTextBox textbox, string text)
        {
            textbox.SelectedRtf = @"{\rtf1\ansi{\v " + text + "}}";
            return textbox;
        }
        //http://stackoverflow.com/questions/14321385/how-to-make-richtextbox-text-only
        public static string removeRtfObjects(string rtf)
        {
            //removing {\pict or {\object groups
            string pattern = "\\{\\\\pict|\\{\\\\object";
            Match m = Regex.Match(rtf, pattern);
            while (m.Success)
            {
                int count = 1;
                for (int i = m.Index + 2; i <= rtf.Length; i++)
                {
                    //start group
                    if (rtf[i] == '{')
                    {
                        count += 1;
                        //end group
                    }
                    else if (rtf[i] == '}')
                    {
                        count -= 1;
                    }
                    //found end of pict/object group
                    if (count == 0)
                    {
                        rtf = rtf.Remove(m.Index, i - m.Index + 1);
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                m = Regex.Match(rtf, pattern);
                //go again
            }
            return rtf;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
    class ImgReplaceStrs
    {
        public string Text;
        public Image Image;
    }
    class ExExRichTextBox : ExRichTextBox
    {
        /*private string originaltext = "";
        public string OriginalText
        {
            get
            {
                //http://www.pcreview.co.uk/forums/re-adding-hidden-text-rich-text-box-control-t1348949.html
                for(int i=0; i<TextFormat.ImgReplaceStrings.Count; i++)
                TextFormat.ImgReplaceStrings[i].Text
                return originaltext;
            }
            set
            {
                originaltext = value;
            }
        }*/
        //public Dictionary<int, string> UsedIcons = new Dictionary<int,string>();
        /*public new void InsertImage(Image _image)
        {
            int sel = SelectionStart;
            int len = SelectionLength;
            string tmp=Text.Substring(sel, len);
            //UsedIcons.Add(sel, tmp);
            base.InsertImage(_image);
            //OriginalText = Text;
            //OriginalText.Remove(sel, len); - Magától eltávolítja
            *foreach(var entry in UsedIcons)
            {
                OriginalText = OriginalText.Insert(entry.Key, entry.Value);
            }*
            OriginalText = OriginalText.Insert(sel, tmp);
        }*/
    }
}
