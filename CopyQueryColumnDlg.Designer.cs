namespace Shenlong
{
	partial class CopyQueryColumnDlg
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
			this.listViewQueryColumn = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.buttonOk = new System.Windows.Forms.Button();
			this.checkSelectAll = new System.Windows.Forms.CheckBox();
			this.checkWithTableJoin = new System.Windows.Forms.CheckBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioColUp = new System.Windows.Forms.RadioButton();
			this.radioColDown = new System.Windows.Forms.RadioButton();
			this.buttonColMove = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkSortByTname = new System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// listViewQueryColumn
			// 
			this.listViewQueryColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listViewQueryColumn.CheckBoxes = true;
			this.listViewQueryColumn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
			this.listViewQueryColumn.FullRowSelect = true;
			this.listViewQueryColumn.GridLines = true;
			this.listViewQueryColumn.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewQueryColumn.HideSelection = false;
			this.listViewQueryColumn.Location = new System.Drawing.Point(8, 8);
			this.listViewQueryColumn.MultiSelect = false;
			this.listViewQueryColumn.Name = "listViewQueryColumn";
			this.listViewQueryColumn.Size = new System.Drawing.Size(439, 240);
			this.listViewQueryColumn.TabIndex = 0;
			this.listViewQueryColumn.UseCompatibleStateImageBehavior = false;
			this.listViewQueryColumn.View = System.Windows.Forms.View.Details;
			this.listViewQueryColumn.SelectedIndexChanged += new System.EventHandler(this.listViewQueryColumn_SelectedIndexChanged);
			this.listViewQueryColumn.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewQueryColumn_ItemCheck);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "　　テーブル名";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "項目名";
			this.columnHeader2.Width = 105;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "表示";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 40;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "条件式";
			this.columnHeader4.Width = 55;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "値１";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "関数";
			this.columnHeader6.Width = 38;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(296, 272);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 5;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// checkSelectAll
			// 
			this.checkSelectAll.AutoSize = true;
			this.checkSelectAll.Location = new System.Drawing.Point(16, 14);
			this.checkSelectAll.Name = "checkSelectAll";
			this.checkSelectAll.Size = new System.Drawing.Size(15, 14);
			this.checkSelectAll.TabIndex = 1;
			this.checkSelectAll.UseVisualStyleBackColor = true;
			this.checkSelectAll.CheckedChanged += new System.EventHandler(this.checkSelectAll_CheckedChanged);
			// 
			// checkWithTableJoin
			// 
			this.checkWithTableJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkWithTableJoin.AutoSize = true;
			this.checkWithTableJoin.Checked = true;
			this.checkWithTableJoin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkWithTableJoin.Location = new System.Drawing.Point(8, 256);
			this.checkWithTableJoin.Name = "checkWithTableJoin";
			this.checkWithTableJoin.Size = new System.Drawing.Size(117, 16);
			this.checkWithTableJoin.TabIndex = 2;
			this.checkWithTableJoin.Text = "テーブル結合も含む";
			this.toolTip.SetToolTip(this.checkWithTableJoin, "テーブル結合の設定も含む");
			this.checkWithTableJoin.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(376, 272);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.radioColUp);
			this.groupBox2.Controls.Add(this.radioColDown);
			this.groupBox2.Controls.Add(this.buttonColMove);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox2.Location = new System.Drawing.Point(136, 256);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(152, 40);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "並び替え";
			// 
			// radioColUp
			// 
			this.radioColUp.Checked = true;
			this.radioColUp.Location = new System.Drawing.Point(8, 18);
			this.radioColUp.Name = "radioColUp";
			this.radioColUp.Size = new System.Drawing.Size(32, 16);
			this.radioColUp.TabIndex = 0;
			this.radioColUp.TabStop = true;
			this.radioColUp.Text = "↑";
			// 
			// radioColDown
			// 
			this.radioColDown.Location = new System.Drawing.Point(43, 18);
			this.radioColDown.Name = "radioColDown";
			this.radioColDown.Size = new System.Drawing.Size(32, 16);
			this.radioColDown.TabIndex = 1;
			this.radioColDown.Text = "↓";
			// 
			// buttonColMove
			// 
			this.buttonColMove.Location = new System.Drawing.Point(80, 12);
			this.buttonColMove.Name = "buttonColMove";
			this.buttonColMove.Size = new System.Drawing.Size(64, 22);
			this.buttonColMove.TabIndex = 2;
			this.buttonColMove.Text = "移動(&M)";
			this.buttonColMove.Click += new System.EventHandler(this.buttonColMove_Click);
			// 
			// checkSortByTname
			// 
			this.checkSortByTname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkSortByTname.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkSortByTname.AutoCheck = false;
			this.checkSortByTname.AutoSize = true;
			this.checkSortByTname.Location = new System.Drawing.Point(8, 273);
			this.checkSortByTname.Name = "checkSortByTname";
			this.checkSortByTname.Size = new System.Drawing.Size(117, 22);
			this.checkSortByTname.TabIndex = 3;
			this.checkSortByTname.Text = "テーブル名でソート(&S)";
			this.toolTip.SetToolTip(this.checkSortByTname, "テーブル名、項目名の順で並び替える");
			this.checkSortByTname.UseVisualStyleBackColor = true;
			this.checkSortByTname.Click += new System.EventHandler(this.checkSortByTname_Click);
			// 
			// CopyQueryColumnDlg
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(456, 305);
			this.Controls.Add(this.checkSortByTname);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.checkSelectAll);
			this.Controls.Add(this.checkWithTableJoin);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.listViewQueryColumn);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CopyQueryColumnDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[切り取り|コピー] クエリー項目を選択";
			this.Load += new System.EventHandler(this.CopyQueryColumnDlg_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listViewQueryColumn;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.CheckBox checkSelectAll;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.CheckBox checkWithTableJoin;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioColUp;
		private System.Windows.Forms.RadioButton radioColDown;
		private System.Windows.Forms.Button buttonColMove;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.CheckBox checkSortByTname;
	}
}