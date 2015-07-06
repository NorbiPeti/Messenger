using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    class MSGerToolStripRenderer : ToolStripRenderer
    { //TODO
        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);
            toolStrip.ForeColor = Color.Blue; //2015.05.16.
        }
        protected override void InitializeContentPanel(ToolStripContentPanel contentPanel)
        {
            base.InitializeContentPanel(contentPanel);
        }
        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);
            item.ForeColor = Color.Blue; //2015.05.16.
        }
        protected override void InitializePanel(ToolStripPanel toolStripPanel)
        {
            base.InitializePanel(toolStripPanel);
        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            base.OnRenderArrow(e);
        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderButtonBackground(e);
        }
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderDropDownButtonBackground(e);
        }
        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            base.OnRenderGrip(e);
        }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            base.OnRenderImageMargin(e);
            //
        }
        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            //base.OnRenderItemBackground(e);
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle(0, 0, e.Item.Width, e.Item.Height));
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), e.Item.Bounds);
        }
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);
        }
        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemImage(e);
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            base.OnRenderItemText(e);
            //
        }
        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderLabelBackground(e);
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle(0, 0, e.Item.Width, e.Item.Height));
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), e.Item.Bounds);
        }
        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderOverflowButtonBackground(e);
        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
            //
        }
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderSplitButtonBackground(e);
        }
        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
        {
            base.OnRenderStatusStripSizingGrip(e);
        }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
            //e.Graphics.DrawRectangle(new Pen(Brushes.Blue), e.AffectedBounds);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, e.AffectedBounds); //2015.05.16.
            //e.Graphics.DrawImage(Properties.Resources.Blue_Wallpaper_HD_2, new Point()); //2015.05.16.
            Theme.SkinThis(Theme.ThemePart.MenuBackground, e.Graphics); //2015.05.22.
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightBlue)), new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightBlue)), e.AffectedBounds);
            //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightBlue)), e.AffectedBounds); //2015.05.16.
        }
        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            base.OnRenderToolStripContentPanelBackground(e);
        }
        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            base.OnRenderToolStripPanelBackground(e);
        }
        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderToolStripStatusLabelBackground(e);
        }
    }
}
