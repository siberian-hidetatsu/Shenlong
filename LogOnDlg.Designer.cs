namespace Shenlong
{
	partial class LogOnDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogOnDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.comboUserName = new System.Windows.Forms.ComboBox();
			this.textSID = new System.Windows.Forms.TextBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkAppendLogOnHis = new System.Windows.Forms.CheckBox();
			this.checkSavePassword = new System.Windows.Forms.CheckBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "ユーザー名(&U)：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(23, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "パスワード(&P)：";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 62);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "ホスト文字列(&H)：";
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(190, 114);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(58, 23);
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(104, 33);
			this.textPassword.Margin = new System.Windows.Forms.Padding(2);
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(144, 20);
			this.textPassword.TabIndex = 3;
			this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
			// 
			// statusStrip
			// 
			this.statusStrip.BackColor = System.Drawing.SystemColors.ControlLight;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 145);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip.Size = new System.Drawing.Size(259, 22);
			this.statusStrip.SizingGrip = false;
			this.statusStrip.TabIndex = 10;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(109, 17);
			this.toolStripStatusLabel.Text = "toolStripStatusLabel1";
			// 
			// comboUserName
			// 
			this.comboUserName.FormattingEnabled = true;
			this.comboUserName.Location = new System.Drawing.Point(104, 7);
			this.comboUserName.Name = "comboUserName";
			this.comboUserName.Size = new System.Drawing.Size(144, 21);
			this.comboUserName.TabIndex = 1;
			this.comboUserName.SelectedIndexChanged += new System.EventHandler(this.comboUserName_SelectedIndexChanged);
			// 
			// textSID
			// 
			this.textSID.Location = new System.Drawing.Point(104, 59);
			this.textSID.Name = "textSID";
			this.textSID.Size = new System.Drawing.Size(144, 20);
			this.textSID.TabIndex = 5;
			// 
			// checkAppendLogOnHis
			// 
			this.checkAppendLogOnHis.AutoSize = true;
			this.checkAppendLogOnHis.Location = new System.Drawing.Point(8, 88);
			this.checkAppendLogOnHis.Name = "checkAppendLogOnHis";
			this.checkAppendLogOnHis.Size = new System.Drawing.Size(164, 17);
			this.checkAppendLogOnHis.TabIndex = 6;
			this.checkAppendLogOnHis.Text = "ログオン履歴に追加する(&A)";
			this.toolTip.SetToolTip(this.checkAppendLogOnHis, "接続情報を履歴を保存する ([Ctrl]+[D]で履歴削除)");
			this.checkAppendLogOnHis.UseVisualStyleBackColor = true;
			this.checkAppendLogOnHis.CheckedChanged += new System.EventHandler(this.checkAppendLogOnHis_CheckedChanged);
			// 
			// checkSavePassword
			// 
			this.checkSavePassword.AutoSize = true;
			this.checkSavePassword.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkSavePassword.Location = new System.Drawing.Point(8, 108);
			this.checkSavePassword.Name = "checkSavePassword";
			this.checkSavePassword.Size = new System.Drawing.Size(140, 16);
			this.checkSavePassword.TabIndex = 7;
			this.checkSavePassword.Text = "パスワードも保存する(&W)";
			this.toolTip.SetToolTip(this.checkSavePassword, "パスワードも履歴に保存するか否か");
			this.checkSavePassword.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(189, 88);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(58, 23);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// LogOnDlg
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(259, 167);
			this.Controls.Add(this.checkSavePassword);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.checkAppendLogOnHis);
			this.Controls.Add(this.textSID);
			this.Controls.Add(this.comboUserName);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LogOnDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ログオン";
			this.Load += new System.EventHandler(this.LogOn_Load);
			this.Shown += new System.EventHandler(this.LogOn_Shown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LogOnDlg_KeyUp);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.ToolTip toolTip;
		public System.Windows.Forms.ComboBox comboUserName;
		public System.Windows.Forms.TextBox textSID;
		private System.Windows.Forms.CheckBox checkAppendLogOnHis;
		private System.Windows.Forms.Button buttonCancel;
		public System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.CheckBox checkSavePassword;
	}
}