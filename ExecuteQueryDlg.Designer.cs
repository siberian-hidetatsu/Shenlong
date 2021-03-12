namespace Shenlong
{
	partial class ExecuteQueryDlg
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteQueryDlg));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar = new MyToolStripProgressBar.MyToolStripProgressBar();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelMessage = new System.Windows.Forms.Label();
			this.buttonYes = new System.Windows.Forms.Button();
			this.buttonNo = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.bgWorkerExecuteScalar = new System.ComponentModel.BackgroundWorker();
			this.bgWorkerExecuteQuery = new System.ComponentModel.BackgroundWorker();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.BackColor = System.Drawing.SystemColors.ControlLight;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
			this.statusStrip.Location = new System.Drawing.Point(0, 87);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip.ShowItemToolTips = true;
			this.statusStrip.Size = new System.Drawing.Size(258, 22);
			this.statusStrip.SizingGrip = false;
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(121, 17);
			this.toolStripStatusLabel.Spring = true;
			this.toolStripStatusLabel.Text = "toolStripStatusLabel";
			this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripProgressBar
			// 
			this.toolStripProgressBar.AutoToolTip = true;
			this.toolStripProgressBar.Name = "toolStripProgressBar";
			this.toolStripProgressBar.Size = new System.Drawing.Size(120, 16);
			this.toolStripProgressBar.Step = 5;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(92, 56);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelMessage
			// 
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new System.Drawing.Point(63, 17);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(74, 12);
			this.labelMessage.TabIndex = 3;
			this.labelMessage.Text = "labelMessage";
			// 
			// buttonYes
			// 
			this.buttonYes.Location = new System.Drawing.Point(52, 56);
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.Size = new System.Drawing.Size(75, 23);
			this.buttonYes.TabIndex = 5;
			this.buttonYes.Text = "はい(&Y)";
			this.buttonYes.UseVisualStyleBackColor = true;
			this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
			// 
			// buttonNo
			// 
			this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonNo.Location = new System.Drawing.Point(132, 56);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(75, 23);
			this.buttonNo.TabIndex = 5;
			this.buttonNo.Text = "いいえ(&N)";
			this.buttonNo.UseVisualStyleBackColor = true;
			this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "102_06.ico");
			this.imageList.Images.SetKeyName(1, "104_07.ico");
			// 
			// bgWorkerExecuteScalar
			// 
			this.bgWorkerExecuteScalar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerExecuteScalar_DoWork);
			this.bgWorkerExecuteScalar.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerExecuteScalar_RunWorkerCompleted);
			// 
			// bgWorkerExecuteQuery
			// 
			this.bgWorkerExecuteQuery.WorkerReportsProgress = true;
			this.bgWorkerExecuteQuery.WorkerSupportsCancellation = true;
			this.bgWorkerExecuteQuery.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerExecuteQuery_DoWork);
			this.bgWorkerExecuteQuery.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerExecuteQuery_RunWorkerCompleted);
			this.bgWorkerExecuteQuery.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerExecuteQuery_ProgressChanged);
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(13, 11);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(32, 32);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox.TabIndex = 4;
			this.pictureBox.TabStop = false;
			// 
			// ExecuteQueryDlg
			// 
			this.AcceptButton = this.buttonYes;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonNo;
			this.ClientSize = new System.Drawing.Size(258, 109);
			this.ControlBox = false;
			this.Controls.Add(this.buttonNo);
			this.Controls.Add(this.buttonYes);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.labelMessage);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.statusStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ExecuteQueryDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "shenlong query";
			this.Load += new System.EventHandler(this.ExecuteQueryDlg_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private MyToolStripProgressBar.MyToolStripProgressBar toolStripProgressBar;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button buttonYes;
		private System.Windows.Forms.Button buttonNo;
		private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.BackgroundWorker bgWorkerExecuteScalar;
		private System.ComponentModel.BackgroundWorker bgWorkerExecuteQuery;
	}
}