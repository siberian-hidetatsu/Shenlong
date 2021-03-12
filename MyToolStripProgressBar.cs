using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MyToolStripProgressBar
{
	public partial class MyToolStripProgressBar : System.Windows.Forms.ToolStripProgressBar
	{
		public MyToolStripProgressBar()
		{
			InitializeComponent();
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}
	}
}
