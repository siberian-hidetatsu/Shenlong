using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ListViewEx
{
	/// <summary>
	/// Event Handler for SubItem events
	/// </summary>
	public delegate void SubItemEventHandler(object sender, SubItemEventArgs e);
	/// <summary>
	/// Event Handler for SubItemEndEditing events
	/// </summary>
	public delegate void SubItemEndEditingEventHandler(object sender, SubItemEndEditingEventArgs e);

	/// <summary>
	/// Event Args for SubItemClicked event
	/// </summary>
	public class SubItemEventArgs : EventArgs
	{
		public SubItemEventArgs(ListViewItem item, int subItem)
		{
			_subItemIndex = subItem;
			_item = item;
		}
		private int _subItemIndex = -1;
		private ListViewItem _item = null;
		public int SubItem
		{
			get { return _subItemIndex; }
		}
		public ListViewItem Item
		{
			get { return _item; }
		}
	}
	
	/// <summary>
	/// Event Args for SubItemEndEditingClicked event
	/// </summary>
	public class SubItemEndEditingEventArgs : SubItemEventArgs
	{
		private string _text = string.Empty;
		private bool _cancel = true;

		public SubItemEndEditingEventArgs(ListViewItem item, int subItem, string display, bool cancel) :
			base(item, subItem)
		{
			_text = display;
			_cancel = cancel;
		}
		public string DisplayText
		{
			get { return _text; }
			set { _text = value; }
		}
		public bool Cancel
		{
			get { return _cancel; }
			set { _cancel = value; }
		}
	}

	///	<summary>
	///	Inherited ListView to allow in-place editing of subitems
	///	</summary>
	public class ListViewEx	: System.Windows.Forms.ListView
	{
		#region Interop structs, imports and constants
		/// <summary>
		/// MessageHeader for WM_NOTIFY
		/// </summary>
		private struct NMHDR
		{
			public IntPtr hwndFrom;
			public Int32  idFrom;
			public Int32  code;
		}

		[DllImport("user32.dll")]
		private	static extern IntPtr SendMessage(IntPtr hWnd, int msg,	IntPtr wPar, IntPtr	lPar);
		[DllImport("user32.dll", CharSet=CharSet.Ansi)]
		private	static extern IntPtr SendMessage(IntPtr	hWnd, int msg, int len,	ref	int	[] order);

		[DllImport("user32.dll", SetLastError = true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

		// ListView messages
		private const int LVM_FIRST					= 0x1000;
		private const int LVM_GETCOLUMNORDERARRAY	= (LVM_FIRST + 59);

		// Windows Messages that will abort editing
		private	const int WM_HSCROLL = 0x114;
		private	const int WM_VSCROLL = 0x115;
		private const int WM_SIZE	 = 0x05;
		private const int WM_NOTIFY	 = 0x4E;

		private const int HDN_FIRST = -300;
		private const int HDN_BEGINDRAG = (HDN_FIRST-10);
		private const int HDN_ITEMCHANGINGA = (HDN_FIRST-0);
		private const int HDN_ITEMCHANGINGW = (HDN_FIRST-20);

		private const int WM_KEYDOWN = 0x0100;
		private const int VK_NEXT = 0x22;
		private const int VK_END = 0x23;
		private const int WM_LBUTTONDOWN = 0x0201;
		private const int WM_RBUTTONUP = 0x0205;
		private const int WM_MOUSEWHEEL = 0x020A;

		private const int WM_NCCALCSIZE = 0x0083;

		private const int GWL_STYLE = -16;
		private const int WS_HSCROLL = 0x00100000;
		private const int WS_VSCROLL = 0x00200000;

		private int SB_HORZ = 0;
		private int SB_VERT = 1;
		private int SB_CTL = 2;
		private int SB_BOTH = 3;
		#endregion

		///	<summary>
		///	Required designer variable.
		///	</summary>
		private	System.ComponentModel.Container	components = null;

		public event SubItemEventHandler SubItemClicked;
		public event SubItemEventHandler SubItemBeginEditing;
		public event SubItemEndEditingEventHandler SubItemEndEditing;

		public ListViewEx()
		{
			// This	call is	required by	the	Windows.Forms Form Designer.
			InitializeComponent();

			base.FullRowSelect = true;
			base.View = View.Details;
			base.AllowColumnReorder = true;

			NMHDR h;					// �R���p�C�����̃��[�j���O��������邽�߁A�_�~�[�̏��������s��
			h.hwndFrom = IntPtr.Zero;
			h.idFrom = 0;
			h.code = 0;
		}

		///	<summary>
		///	Clean up any resources being used.
		///	</summary>
		protected override void	Dispose( bool disposing	)
		{
			if(	disposing )
			{
				if(	components != null )
					components.Dispose();
			}
			base.Dispose( disposing	);
		}

		#region Component	Designer generated code
		///	<summary>
		///	Required method	for	Designer support - do not modify 
		///	the	contents of	this method	with the code editor.
		///	</summary>
		private	void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		/*private bool _doubleClickActivation = false;
		/// <summary>
		/// Is a double click required to start editing a cell?
		/// </summary>
		public bool DoubleClickActivation
		{
			get {  return _doubleClickActivation; }
			set { _doubleClickActivation = value; }    
		}*/
		
		private int _validItemCount = 64;
		/// <summary>
		/// �L���ȃA�C�e����
		/// </summary>
		public int ValidItemCount
		{
			get {  return _validItemCount; }
			set { _validItemCount = value; }    
		}

		/// <summary>
		/// Retrieve the order in which columns appear
		/// </summary>
		/// <returns>Current display order of column indices</returns>
		public int[] GetColumnOrder()
		{
			IntPtr lPar	= Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * Columns.Count);

			IntPtr res = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, new IntPtr(Columns.Count), lPar);
			if (res.ToInt32() == 0)	// Something went wrong
			{
				Marshal.FreeHGlobal(lPar);
				return null;
			}

			int	[] order = new int[Columns.Count];
			Marshal.Copy(lPar, order, 0, Columns.Count);

			Marshal.FreeHGlobal(lPar);

			return order;
		}
		
		/// <summary>
		/// Find ListViewItem and SubItem Index at position (x,y)
		/// </summary>
		/// <param name="x">relative to ListView</param>
		/// <param name="y">relative to ListView</param>
		/// <param name="item">Item at position (x,y)</param>
		/// <returns>SubItem index</returns>
		public int GetSubItemAt(int x, int y, out ListViewItem item)
		{
			item = this.GetItemAt(x, y);
		
			if (item !=	null)
			{
				int[] order = GetColumnOrder();
				Rectangle lviBounds;
				int	subItemX;

				lviBounds =	item.GetBounds(ItemBoundsPortion.Entire);
				subItemX = lviBounds.Left;
				for (int i=0; i<order.Length; i++)
				{
					ColumnHeader h = this.Columns[order[i]];
					if (x <	subItemX+h.Width)
					{
						return h.Index;
					}
					subItemX += h.Width;
				}
			}
			
			return -1;
		}
		
		/// <summary>
		/// Get bounds for a SubItem
		/// </summary>
		/// <param name="Item">Target ListViewItem</param>
		/// <param name="SubItem">Target SubItem index</param>
		/// <returns>Bounds of SubItem (relative to ListView)</returns>
		public Rectangle GetSubItemBounds(ListViewItem Item, int SubItem)
		{
			int[] order = GetColumnOrder();

			Rectangle subItemRect = Rectangle.Empty;
			if (SubItem >= order.Length)
				throw new IndexOutOfRangeException("SubItem "+SubItem+" out of range");

			if (Item == null)
				throw new ArgumentNullException("Item");
			
			Rectangle lviBounds = Item.GetBounds(ItemBoundsPortion.Entire);
			int	subItemX = lviBounds.Left;

			ColumnHeader col;
			int i;
			for (i=0; i<order.Length; i++)
			{
				col = this.Columns[order[i]];
				if (col.Index == SubItem)
					break;
				subItemX += col.Width;
			} 
			subItemRect	= new Rectangle(subItemX, lviBounds.Top, this.Columns[order[i]].Width, lviBounds.Height);
			return subItemRect;
		}
		
		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="msg"></param>
		protected override void	WndProc(ref	Message	msg)
		{
			switch ( msg.Msg )
			{
				// Look	for	WM_VSCROLL,WM_HSCROLL or WM_SIZE messages.
				case WM_VSCROLL:
				case WM_HSCROLL:
				case WM_SIZE:
					EndEditing(false);
					break;
				case WM_NOTIFY:
					// Look for WM_NOTIFY of events that might also change the
					// editor's position/size: Column reordering or resizing
					NMHDR h = (NMHDR)Marshal.PtrToStructure(msg.LParam, typeof(NMHDR));
					if ( h.code == HDN_BEGINDRAG ||
						h.code == HDN_ITEMCHANGINGA ||
						h.code == HDN_ITEMCHANGINGW )
						EndEditing(false);
					break;
				case WM_KEYDOWN:
					if ( ((int)msg.WParam == VK_NEXT) || ((int)msg.WParam == VK_END) )
						return;
					break;
				case WM_LBUTTONDOWN:
					ushort x = (ushort)((uint)msg.LParam & 0xffff);	// LOWORD
					ushort y = (ushort)((uint)msg.LParam >> 16);	// HIWORD
					ListViewItem item = this.GetItemAt(x, y);
					System.Diagnostics.Debug.WriteLine("WM_LBUTTONDOWN: " + x + " " + y + " " + ((item == null) ? "item is null" : item.Index.ToString()));
					if ( (item != null) && (ValidItemCount <= item.Index) )
					{
						EndEditing(true);
						return;
					}
					break;
				case WM_MOUSEWHEEL:
					return;
				case WM_NCCALCSIZE:
					int ws = GetWindowLong(Handle, GWL_STYLE);
					ws &= WS_VSCROLL/* | WS_HSCROLL*/;
					if ( ws != 0 )
					{
						// �X�N���[���o�[���\��
						ShowScrollBar(Handle, SB_VERT/*SB_BOTH*/, false);
					}
					break;
			}

			base.WndProc(ref msg);
		}
		
		#region Initialize editing depending of DoubleClickActivation property
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);

			/*if (DoubleClickActivation)
			{
				return;
			} */

			EditSubitemAt(new Point(e.X, e.Y));
		}
		
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);

			/*if (!DoubleClickActivation)
			{
				return;
			} */

			Point pt = this.PointToClient(Cursor.Position);
		
			EditSubitemAt(pt);
		}

		///<summary>
		/// Fire SubItemClicked
		///</summary>
		///<param name="p">Point of click/doubleclick</param>
		private void EditSubitemAt(Point p)
		{
			ListViewItem item;
			int idx = GetSubItemAt(p.X, p.Y, out item);
			//if (idx >= 0)
			if ( (idx >= 0) && ((item != null) && (0 <= item.Index) && (item.Index < ValidItemCount)) )
			{
				OnSubItemClicked(new SubItemEventArgs(item, idx));
			}
		}
		#endregion

		#region In-place editing functions
		// The control performing the actual editing
		private Control _editingControl;
		// The LVI being edited
		private ListViewItem _editItem = null;
		// The SubItem being edited
		private int _editSubItem = -1;

		public int EditSubItem
		{
			get { return _editSubItem; }
		}

		protected void OnSubItemBeginEditing(SubItemEventArgs e)
		{
			if (SubItemBeginEditing != null)
				SubItemBeginEditing(this, e);
		}
		protected void OnSubItemEndEditing(SubItemEndEditingEventArgs e)
		{
			if (SubItemEndEditing != null)
				SubItemEndEditing(this, e);
		}
		protected void OnSubItemClicked(SubItemEventArgs e)
		{
			if (SubItemClicked != null)
				SubItemClicked(this, e);
		}
		
		/// <summary>
		/// Begin in-place editing of given cell
		/// </summary>
		/// <param name="c">Control used as cell editor</param>
		/// <param name="Item">ListViewItem to edit</param>
		/// <param name="SubItem">SubItem index to edit</param>
		public void StartEditing(Control c, ListViewItem Item, int SubItem)
		{
			try
			{
				OnSubItemBeginEditing(new SubItemEventArgs(Item, SubItem));

				Rectangle rcSubItem = GetSubItemBounds(Item, SubItem);

				if ( rcSubItem.X < 0 )
				{
					// Left edge of SubItem not visible - adjust rectangle position and width
					rcSubItem.Width += rcSubItem.X;
					rcSubItem.X = 0;
				}
				if ( rcSubItem.X + rcSubItem.Width > this.Width )
				{
					// Right edge of SubItem not visible - adjust rectangle width
					rcSubItem.Width = this.Width - rcSubItem.Left;
				}

				// Subitem bounds are relative to the location of the ListView!
				rcSubItem.Offset(Left, Top);

				// In case the editing control and the listview are on different parents,
				// account for different origins
				Point origin = new Point(0, 0);
				Point lvOrigin = this.Parent.PointToScreen(origin);
				Point ctlOrigin = c.Parent.PointToScreen(origin);

				// Position and show editor
				if ( c is CheckBox )
				{
					rcSubItem.Offset(lvOrigin.X - ctlOrigin.X + (rcSubItem.Width / 2) - (c.Width / 2) + 3, lvOrigin.Y - ctlOrigin.Y + (rcSubItem.Height / 2) - (c.Height / 2) + 3);
					((CheckBox)c).Checked = bool.Parse(Item.SubItems[SubItem].Text);
					_editSubItem = SubItem;	// �I�[�i�[�h���[�Ń`�F�b�N�{�b�N�X��\�������Ȃ��΍�
				}
				else
				{
					rcSubItem.Offset(lvOrigin.X - ctlOrigin.X + 2, lvOrigin.Y - ctlOrigin.Y - 1);
					c.Text = Item.SubItems[SubItem].Text;
				}
				c.Bounds = rcSubItem;
				c.Visible = true;
				c.BringToFront();
				c.Focus();

				_editingControl = c;
				_editingControl.Leave += new EventHandler(_editControl_Leave);
				_editingControl.KeyPress += new KeyPressEventHandler(_editControl_KeyPress);

				//System.Diagnostics.Debug.WriteLine("_editSubItem = SubItem:" + _editSubItem + " " + SubItem);
				_editItem = Item;
				_editSubItem = SubItem;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
		
		/// <summary>
		/// _editControl_Leave
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _editControl_Leave(object sender, EventArgs e)
		{
			// cell editor losing focus
			EndEditing(true);
		}
				
		/// <summary>
		/// _editControl_KeyPress
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _editControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
				case (char)(int)Keys.Escape:
				{
					EndEditing(false);
					break;
				}

				case (char)(int)Keys.Enter:
				{  
					EndEditing(true);
					break;
				}
			}
		}

		/// <summary>
		/// Accept or discard current value of cell editor control
		/// </summary>
		/// <param name="AcceptChanges">Use the _editingControl's Text as new SubItem text or discard changes?</param>
		public void EndEditing(bool AcceptChanges)
		{
			try
			{
				if ( _editingControl == null )
					return;

				SubItemEndEditingEventArgs e = new SubItemEndEditingEventArgs(
					_editItem,		// The item being edited
					_editSubItem,	// The subitem index being edited
					AcceptChanges ?
						_editingControl.Text :	// Use editControl text if changes are accepted
						_editItem.SubItems[_editSubItem].Text,	// or the original subitem's text, if changes are discarded
					!AcceptChanges	// Cancel?
				);

				if ( _editingControl is CheckBox )
				{
					e.DisplayText = ((CheckBox)_editingControl).Checked.ToString().ToLower();
				}

				OnSubItemEndEditing(e);

				_editItem.SubItems[_editSubItem].Text = e.DisplayText;

				_editingControl.Leave -= new EventHandler(_editControl_Leave);
				_editingControl.KeyPress -= new KeyPressEventHandler(_editControl_KeyPress);

				_editingControl.Visible = false;

				_editingControl = null;
				_editItem = null;
				_editSubItem = -1;

				this.Parent.Focus();
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
		#endregion
	}
}
