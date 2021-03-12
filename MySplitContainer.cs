using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace MySplitContainer
{
	public partial class MySplitContainer : System.Windows.Forms.SplitContainer
	{
		public MySplitContainer()
		{
			InitializeComponent();
		}

		public MySplitContainer(IContainer container)
		{
			container.Add(this);

			InitializeComponent();

			//this.SetStyle(ControlStyles.Selectable, false);
		}

		/*private bool _DisplayFocusCues = false;

		protected override bool ShowFocusCues
		{
			get { return _DisplayFocusCues; }
		}*/
		
		/*public bool DisplayFocusCues
		{
			get
			{
				return _DisplayFocusCues;
			}
			set
			{
				_DisplayFocusCues = value;
			}
		}*/

		protected override void WndProc(ref Message m)
		{
			try
			{
				switch ( (uint)m.Msg )
				{
					case 0x0007:	// WM_SETFOCUS
						return;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}

			base.WndProc(ref m);
		}

		/*protected override void OnChangeUICues(System.Windows.Forms.UICuesEventArgs e)
		{
			//base.OnChangeUICues(e);
		}*/

		/*protected override void OnEnter(EventArgs e)
		{
			//base.OnEnter(e);
		}*/

		/*protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			//base.OnPaint(e);

			//e.Graphics.FillRectangle(System.Drawing.Brushes.Black, e.ClipRectangle);
		}*/
	}
}
