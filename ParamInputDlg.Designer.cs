namespace Shenlong
{
	partial class ParamInputDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamInputDlg));
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.contextMenuToolStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuDeleteLatestParams = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonOK = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBoxInputControl = new System.Windows.Forms.GroupBox();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripShenValue = new System.Windows.Forms.ToolStripButton();
			this.toolStripReloadValue = new System.Windows.Forms.ToolStripButton();
			this.flowLayoutPanel.SuspendLayout();
			this.contextMenuToolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.groupBoxInputControl.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoScroll = true;
			this.flowLayoutPanel.Controls.Add(this.label1);
			this.flowLayoutPanel.Controls.Add(this.textBox1);
			this.flowLayoutPanel.Controls.Add(this.label2);
			this.flowLayoutPanel.Controls.Add(this.textBox2);
			this.flowLayoutPanel.Controls.Add(this.label3);
			this.flowLayoutPanel.Controls.Add(this.dateTimePicker1);
			this.flowLayoutPanel.Controls.Add(this.comboBox1);
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel.Location = new System.Drawing.Point(3, 15);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(322, 206);
			this.flowLayoutPanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.flowLayoutPanel.SetFlowBreak(this.textBox1, true);
			this.textBox1.Location = new System.Drawing.Point(44, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 19);
			this.textBox1.TabIndex = 1;
			this.textBox1.Validated += new System.EventHandler(this.textBox_Validated);
			this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "label2";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(44, 28);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(100, 19);
			this.textBox2.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(3, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(256, 2);
			this.label3.TabIndex = 4;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(3, 55);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(117, 19);
			this.dateTimePicker1.TabIndex = 5;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(126, 55);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 6;
			// 
			// contextMenuToolStrip
			// 
			this.contextMenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuDeleteLatestParams});
			this.contextMenuToolStrip.Name = "contextMenuReloadValue";
			this.contextMenuToolStrip.ShowImageMargin = false;
			this.contextMenuToolStrip.Size = new System.Drawing.Size(148, 26);
			this.contextMenuToolStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuToolStrip_Opening);
			// 
			// toolStripMenuDeleteLatestParams
			// 
			this.toolStripMenuDeleteLatestParams.Name = "toolStripMenuDeleteLatestParams";
			this.toolStripMenuDeleteLatestParams.Size = new System.Drawing.Size(147, 22);
			this.toolStripMenuDeleteLatestParams.Text = "前回の値を削除する";
			this.toolStripMenuDeleteLatestParams.Click += new System.EventHandler(this.toolStripMenuDeleteLatestParams_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(184, 256);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 24);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(264, 256);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 24);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// groupBoxInputControl
			// 
			this.groupBoxInputControl.Controls.Add(this.flowLayoutPanel);
			this.groupBoxInputControl.Location = new System.Drawing.Point(8, 24);
			this.groupBoxInputControl.Name = "groupBoxInputControl";
			this.groupBoxInputControl.Size = new System.Drawing.Size(328, 224);
			this.groupBoxInputControl.TabIndex = 0;
			this.groupBoxInputControl.TabStop = false;
			// 
			// toolStrip
			// 
			this.toolStrip.ContextMenuStrip = this.contextMenuToolStrip;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripShenValue,
            this.toolStripReloadValue});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip.Size = new System.Drawing.Size(345, 25);
			this.toolStrip.TabIndex = 5;
			this.toolStrip.Text = "toolStrip";
			// 
			// toolStripShenValue
			// 
			this.toolStripShenValue.Checked = true;
			this.toolStripShenValue.CheckOnClick = true;
			this.toolStripShenValue.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripShenValue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripShenValue.Image = ((System.Drawing.Image)(resources.GetObject("toolStripShenValue.Image")));
			this.toolStripShenValue.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripShenValue.Name = "toolStripShenValue";
			this.toolStripShenValue.Size = new System.Drawing.Size(23, 22);
			this.toolStripShenValue.Text = "条件入力無しの時 ON:クエリー項目の値 OFF:条件から除外";
			// 
			// toolStripReloadValue
			// 
			this.toolStripReloadValue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripReloadValue.Image = ((System.Drawing.Image)(resources.GetObject("toolStripReloadValue.Image")));
			this.toolStripReloadValue.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripReloadValue.Name = "toolStripReloadValue";
			this.toolStripReloadValue.Size = new System.Drawing.Size(23, 22);
			this.toolStripReloadValue.Text = "前回の値を読み込む [Ctl+R]";
			this.toolStripReloadValue.Click += new System.EventHandler(this.toolStripReloadValue_Click);
			// 
			// ParamInputDlg
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(345, 288);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.groupBoxInputControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(640, 480);
			this.MinimizeBox = false;
			this.Name = "ParamInputDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "抽出条件入力";
			this.Load += new System.EventHandler(this.ParamInputDlg_Load);
			this.Shown += new System.EventHandler(this.ParamInputDlg_Shown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ParamInputDlg_KeyUp);
			this.flowLayoutPanel.ResumeLayout(false);
			this.flowLayoutPanel.PerformLayout();
			this.contextMenuToolStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.groupBoxInputControl.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupBoxInputControl;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuToolStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuDeleteLatestParams;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton toolStripShenValue;
		private System.Windows.Forms.ToolStripButton toolStripReloadValue;
	}
}