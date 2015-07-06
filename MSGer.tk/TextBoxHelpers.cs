using FastColoredTextBoxNS;
using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public static class TextBoxHelpers
    {
        /// <summary>
        /// This class is used as text renderer for smiles
        /// </summary>
        public class GifImageStyle : TextStyle
        {
            public Dictionary<string, Image> ImagesByText { get; private set; }
            public Dictionary<string, Image> SentImagesByText { get; private set; }
            FastColoredTextBox parent;
            System.Windows.Forms.Timer timer;

            public GifImageStyle(FastColoredTextBox parent)
                : base(null, null, FontStyle.Regular)
            {
                ImagesByText = new Dictionary<string, Image>();
                SentImagesByText = new Dictionary<string, Image>(); //2015.06.26.
                this.parent = parent;

                //create timer
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 100;
                timer.Tick += (EventHandler)delegate
                {
                    ImageAnimator.UpdateFrames();
                    parent.Invalidate();
                };
                timer.Start();
            }

            private int counter = 0;
            public string AddImage(Image image)
            {
                string str = "//!img" + counter++;
                //ImagesByText.Add(str, image);
                SentImagesByText.Add(str, image); //2015.04.07.
                return str;
            }

            public void StartAnimation()
            {
                foreach (var image in ImagesByText.Values)
                    if (ImageAnimator.CanAnimate(image))
                        ImageAnimator.Animate(image, new EventHandler(OnFrameChanged));
            }

            void OnFrameChanged(object sender, EventArgs args)
            {
            }

            public override void Draw(Graphics gr, Point position, Range range)
            {
                string text = range.Text;
                int iChar = range.Start.iChar;

                while (text != "")
                {
                    bool replaced = false;
                    foreach (var pair in ImagesByText)
                    {
                        if (text.StartsWith(pair.Key))
                        {
                            float k = (float)(pair.Key.Length * range.tb.CharWidth) / pair.Value.Width;
                            if (k > 1)
                                k = 1f;
                            //
                            text = text.Substring(pair.Key.Length);
                            RectangleF rect = new RectangleF(position.X + range.tb.CharWidth * pair.Key.Length / 2 - pair.Value.Width * k / 2, position.Y, pair.Value.Width * k, pair.Value.Height * k);
                            gr.DrawImage(pair.Value, rect);
                            position.Offset(range.tb.CharWidth * pair.Key.Length, 0);
                            replaced = true;
                            iChar += pair.Key.Length;
                            break;
                        }
                    }

                //Képküldés

                    foreach (var pair in SentImagesByText)
                    {
                        if (!pair.Key.StartsWith("//!img"))
                            throw new InvalidOperationException("An invalid text got set as image placeholder."); //2015.06.26.
                        if (text.StartsWith(pair.Key))
                        {
                            text = text.Substring(pair.Key.Length);
                            RectangleF rect = new RectangleF(0, position.Y, pair.Value.Width, pair.Value.Height);
                            gr.DrawImage(pair.Value, rect);
                            position.Offset(range.tb.CharWidth * pair.Key.Length, 0);
                            //position.Offset(-position.X, pair.Value.Height); //2015.07.04.
                            replaced = true;
                            iChar += pair.Key.Length;
                            break;
                        }
                    }
                    if (!replaced && text.Length > 0)
                    {
                        Range r = new Range(range.tb, iChar, range.Start.iLine, iChar + 1, range.Start.iLine);
                        base.Draw(gr, position, r);
                        position.Offset(range.tb.CharWidth, 0);
                        text = text.Substring(1);
                    }
                }
            }
        }
    }
}
