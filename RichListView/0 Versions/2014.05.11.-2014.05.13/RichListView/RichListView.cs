using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Khendys.Controls;

namespace SzNPProjects
{
    public partial class RichListView : UserControl
    {
        public RichListView()
        {
            InitializeComponent();
            Items.ListChanged += Items_ListChanged;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //Columns = new BindingList<RichListViewColumn>();

            //var item = new ExRichTextBox();
            //var listitem = new RichListViewItem(item);
            //Items.Add(listitem);
        }

        #region Events
        void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch(e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Items[e.NewIndex].Parent = this;
                    Items[e.NewIndex]._CreateInstance();
                    break;
            }
            _RefreshList();
        }
        public event EventHandler<int> ItemClicked;
        public event EventHandler<int> ItemDoubleClicked;
        #endregion

        #region Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingList<RichListViewItem> items;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingList<RichListViewItem> Items
        {
            get
            {
                if (items == null)
                    items = new BindingList<RichListViewItem>();
                return items;
            }
        }
        /*[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingList<RichListViewColumn> Columns { get; set; }*/
        public RichListViewColumn[] columns;
        public RichListViewColumn[] Columns
        {
            get
            {
                if (columns == null)
                    return new RichListViewColumn[0];
                return columns;
            }
            set
            {
                columns = value;
                _RefreshList();
            }
        }
        public int HeaderHeight { get; set; }
        public int ItemHeight { get; set; }
        /// <summary>
        /// Determines whenether to set last column width to fit the remaining space.
        /// </summary>
        public bool ColumnAutoFill { get; set; }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                _SetItemsBackground(value);
                base.BackColor = value;
            }
        }
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                _SetItemsForeground(value);
                base.ForeColor = value;
            }
        }
        #endregion

        #region Public Methods
        public new void Show()
        {
            _RefreshList();
            for (int i = 0; i < Items.Count; i++)
                Items[i].Show();
            base.Show();
        }
        public new void Hide()
        {

        }
        public void SetItems(List<Control[]> items)
        {
            for (int i = 0; i < items.Count; i++)
                Items.Add(new RichListViewItem(items[i]));
            _RefreshList();
        }
        #endregion

        #region Internal Methods
        private void _SetItemsBackground(Color value)
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].BackColor = value;
        }
        private void _SetItemsForeground(Color value)
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].ForeColor = value;
        }
        private void _RefreshList()
        {
            //Console.WriteLine("RefreshList");
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Hide();
                //Console.WriteLine("Item found: " + i);
                Items[i].SetBounds(HeaderHeight + ItemHeight * i, ItemHeight);
                //for (int j = 0; j < Columns.Count; i++)
                //Items[i].Control.SetBounds(this.Location.X, this.Location.Y + HeaderHeight + ItemHeight * i, Columns[j].Width, ItemHeight);
                /*Items[i].Control.Show();
                Items[i].Control.Parent = this;*/
                Items[i].Show();
            }
            Refresh();
        }
        internal void _ItemClicked(object sender)
        {
            ItemClicked(sender, Items.IndexOf((RichListViewItem)sender));
        }
        internal void _ItemDoubleClicked(object sender)
        {
            var index=Items.IndexOf((RichListViewItem)sender);
            ItemDoubleClicked(sender, index);
            for (int i = 0; i < Items[index].SubItems.Length; i++)
                if (Items[index].SubItems[i].GetType().IsSubclassOf(typeof(TextBoxBase)))
                    ((TextBoxBase)Items[index].SubItems[i]).SelectionLength = 0;
        }
        private void RichListView_Resize(object sender, EventArgs e)
        {
            _RefreshList();
        }
        #endregion
    }
}
