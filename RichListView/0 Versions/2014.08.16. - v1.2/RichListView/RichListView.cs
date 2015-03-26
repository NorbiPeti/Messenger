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
        /// <summary>
        /// Create new instance of RichListView.
        /// </summary>
        public RichListView()
        {
            InitializeComponent();
            Items.ListChanged += Items_ListChanged;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            ResizeRedraw = false; //Redraw is done using RefreshList()
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
                case ListChangedType.ItemDeleted:
                    items2[e.OldIndex].Remove();
                    break;
                case ListChangedType.Reset:
                    for (int i = 0; i < items2.Count; i++)
                        items2[i].Remove();
                    break;
            }
            RefreshList();
        }
        public event EventHandler<int> ItemClicked;
        public event EventHandler<int> ItemDoubleClicked;
        public event EventHandler<int> ItemRightClicked;
        #endregion

        #region Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BindingList<RichListViewItem> items;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal BindingList<RichListViewItem> items2; //Stores the last item list
        /// <summary>
        /// A list of items shown in the RichListView.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BindingList<RichListViewItem> Items
        {
            get
            {
                if (items == null)
                    items = new BindingList<RichListViewItem>();
                if (items.Count != 0)
                {
                    if (items2 == null)
                        items2 = new BindingList<RichListViewItem>();
                    items2.Clear();
                    for (int i = 0; i < items.Count; i++)
                        items2.Add(items[i]);
                }
                //items2 = new BindingList<RichListViewItem>(items);
                return items;
            }
        }
        public RichListViewColumn[] columns;
        /// <summary>
        /// Array of columns.
        /// </summary>
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
                RefreshList();
            }
        }
        /// <summary>
        /// Get/set the header height.
        /// </summary>
        public int HeaderHeight { get; set; }
        /// <summary>
        /// Get/set items height.
        /// </summary>
        public int ItemHeight { get; set; }
        /// <summary>
        /// Determines whenether to set last column width to fit the remaining space.
        /// </summary>
        public bool ColumnAutoFill { get; set; }
        private bool autoupdate = true;
        /// <summary>
        /// When false, will disable any update event.
        /// </summary>
        public bool AutoUpdate
        {
            get
            {
                return autoupdate;
            }
            set
            {
                autoupdate = value;
                if (autoupdate)
                    RefreshList();
            }
        }
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
        private Color selectioncolor = Color.Aqua;
        public Color SelectionColor
        {
            get
            {
                return selectioncolor;
            }
            set
            {
                selectioncolor = value;
            }
        }
        private int selectedindex = -1;
        public int SelectedIndex
        {
            get
            {
                return selectedindex;
            }
            internal set
            {
                if (selectedindex != -1)
                    Items[selectedindex].Selected = false;
                if (value != -1)
                    Items[value].Selected = true;
                selectedindex = value;
            }
        }
        #endregion

        #region Public Methods
        public new void Show()
        {
            RefreshList();
            /*for (int i = 0; i < Items.Count; i++)
                Items[i].Show();*/
            base.Show();
        }
        public new void Hide()
        {
            RefreshList();
            /*for (int i = 0; i < Items.Count; i++)
                Items[i].Hide();*/
            base.Hide();
        }
        /// <summary>
        /// Provide a list of Control arrays to set the items.
        /// </summary>
        /// <param name="items">The items to set.</param>
        public void SetItems(List<Control[]> items)
        {
            Action<List<Control[]>> func = SetItems2;
            func(items);
        }
        public void SetItems2(List<Control[]> items)
        {
            for (int i = 0; i < items.Count; i++)
                Items.Add(new RichListViewItem(items[i]));
            RefreshList();
        }
        /// <summary>
        /// Refresh the list manually, if built-in refresh is not enough.
        /// </summary>
        public void RefreshList()
        {
            if(!AutoUpdate)
            {
                AutoValidate = AutoValidate.Disable;
                return;
            }
            AutoValidate = AutoValidate.EnablePreventFocusChange; //- Actually it will raise the update, no need for automation
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Hide();
                Items[i].SetBounds(HeaderHeight + ItemHeight * i, ItemHeight);
                if (Items[i].Selected)
                    Items[i].BackColor = SelectionColor;
                else
                    Items[i].BackColor = this.BackColor;
                Items[i].Show();
            }
            Refresh();
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
        //internal void _ItemClicked(object sender)
        internal void _ItemClicked(RichListViewItem sender)
        {
            try
            {
                //ItemClicked(sender, Items.IndexOf((RichListViewItem)sender));
                /*for (int i = 0; i < Items.Count; i++)
                {
                    if (!Items[i].Equals(sender))
                        Items[i].Selected = false;
                }*/
                sender.Selected = true;
                ItemClicked(sender, Items.IndexOf(sender));
            }
            catch(NullReferenceException)
            {
            }
        }
        //internal void _ItemDoubleClicked(object sender)
        internal void _ItemDoubleClicked(RichListViewItem sender)
        {
            try
            {
                //var index = Items.IndexOf((RichListViewItem)sender);
                var index = Items.IndexOf(sender);
                ItemDoubleClicked(sender, index);
                for (int i = 0; i < Items[index].SubItems.Length; i++)
                    if (Items[index].SubItems[i].GetType().IsSubclassOf(typeof(TextBoxBase)))
                        ((TextBoxBase)Items[index].SubItems[i]).SelectionLength = 0;
            }
            catch (NullReferenceException)
            {
            }
        }
        internal void _ItemRightClicked(RichListViewItem sender)
        {
            try
            {
                ItemRightClicked(sender, Items.IndexOf(sender));
            }
            catch (NullReferenceException)
            {
            }
        }
        private void RichListView_Resize(object sender, EventArgs e)
        {
            RefreshList();
        }
        private void RichListView_Click(object sender, EventArgs e)
        {
            this.SelectedIndex = -1;
        }
        #endregion
    }
}
