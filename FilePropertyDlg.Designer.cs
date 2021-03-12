namespace Shenlong
{
	partial class FilePropertyDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilePropertyDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.textComment = new System.Windows.Forms.TextBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textAuthor = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.radioDlPermit = new System.Windows.Forms.RadioButton();
			this.radioDlDeny = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.textMaxRowNum = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textEggPermission = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.checkSqlSelect = new System.Windows.Forms.CheckBox();
			this.checkSetValue = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.listBoxSubQuery = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkDistinct = new System.Windows.Forms.CheckBox();
			this.checkColumnName = new System.Windows.Forms.CheckBox();
			this.checkComment = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkUseJoin = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "コメント";
			this.toolTip.SetToolTip(this.label1, "バブルスでの一覧に表示される");
			// 
			// textComment
			// 
			this.textComment.Location = new System.Drawing.Point(52, 12);
			this.textComment.Name = "textComment";
			this.textComment.Size = new System.Drawing.Size(322, 19);
			this.textComment.TabIndex = 1;
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(224, 264);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 9;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "作成者";
			this.toolTip.SetToolTip(this.label2, "作成者の名前");
			// 
			// textAuthor
			// 
			this.textAuthor.Location = new System.Drawing.Point(52, 40);
			this.textAuthor.Name = "textAuthor";
			this.textAuthor.Size = new System.Drawing.Size(126, 19);
			this.textAuthor.TabIndex = 3;
			// 
			// buttonCancel
			// 
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(304, 264);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 10;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "ダウンロード";
			this.toolTip.SetToolTip(this.label3, "シェンロンの卵をダウンロードする設定");
			// 
			// radioDlPermit
			// 
			this.radioDlPermit.AutoSize = true;
			this.radioDlPermit.Location = new System.Drawing.Point(75, 16);
			this.radioDlPermit.Name = "radioDlPermit";
			this.radioDlPermit.Size = new System.Drawing.Size(66, 16);
			this.radioDlPermit.TabIndex = 1;
			this.radioDlPermit.TabStop = true;
			this.radioDlPermit.Text = "許可する";
			this.radioDlPermit.UseVisualStyleBackColor = true;
			// 
			// radioDlDeny
			// 
			this.radioDlDeny.AutoSize = true;
			this.radioDlDeny.Location = new System.Drawing.Point(142, 16);
			this.radioDlDeny.Name = "radioDlDeny";
			this.radioDlDeny.Size = new System.Drawing.Size(76, 16);
			this.radioDlDeny.TabIndex = 2;
			this.radioDlDeny.TabStop = true;
			this.radioDlDeny.Text = "許可しない";
			this.radioDlDeny.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "最大抽出行";
			this.toolTip.SetToolTip(this.label4, "指定無し：バブルス側の設定値　0：無制限");
			// 
			// textMaxRowNum
			// 
			this.textMaxRowNum.Location = new System.Drawing.Point(75, 57);
			this.textMaxRowNum.MaxLength = 6;
			this.textMaxRowNum.Name = "textMaxRowNum";
			this.textMaxRowNum.Size = new System.Drawing.Size(56, 19);
			this.textMaxRowNum.TabIndex = 6;
			this.textMaxRowNum.Tag = "半角数字のみ";
			this.textMaxRowNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textMaxRowNum.Validated += new System.EventHandler(this.textMaxRowNum_Validated);
			this.textMaxRowNum.Validating += new System.ComponentModel.CancelEventHandler(this.textMaxRowNum_Validating);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textEggPermission);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.checkSqlSelect);
			this.groupBox1.Controls.Add(this.checkSetValue);
			this.groupBox1.Controls.Add(this.radioDlPermit);
			this.groupBox1.Controls.Add(this.textMaxRowNum);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.radioDlDeny);
			this.groupBox1.Location = new System.Drawing.Point(14, 173);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 82);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "for Bubbles";
			// 
			// textEggPermission
			// 
			this.textEggPermission.Location = new System.Drawing.Point(75, 34);
			this.textEggPermission.Name = "textEggPermission";
			this.textEggPermission.Size = new System.Drawing.Size(151, 19);
			this.textEggPermission.TabIndex = 4;
			this.textEggPermission.Validated += new System.EventHandler(this.textEggPermission_Validated);
			this.textEggPermission.Validating += new System.ComponentModel.CancelEventHandler(this.textEggPermission_Validating);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 38);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(65, 12);
			this.label7.TabIndex = 3;
			this.label7.Text = "アクセス許可";
			this.toolTip.SetToolTip(this.label7, "タマゴへのアクセスを許可する端末");
			// 
			// checkSqlSelect
			// 
			this.checkSqlSelect.AutoSize = true;
			this.checkSqlSelect.Location = new System.Drawing.Point(248, 48);
			this.checkSqlSelect.Name = "checkSqlSelect";
			this.checkSqlSelect.Size = new System.Drawing.Size(98, 16);
			this.checkSqlSelect.TabIndex = 9;
			this.checkSqlSelect.Text = "SQLで抽出する";
			this.toolTip.SetToolTip(this.checkSqlSelect, "SQLタブのSELECT文で抽出する");
			this.checkSqlSelect.UseVisualStyleBackColor = true;
			// 
			// checkSetValue
			// 
			this.checkSetValue.AutoSize = true;
			this.checkSetValue.Location = new System.Drawing.Point(248, 24);
			this.checkSetValue.Name = "checkSetValue";
			this.checkSetValue.Size = new System.Drawing.Size(89, 16);
			this.checkSetValue.TabIndex = 8;
			this.checkSetValue.Text = "値をセットする";
			this.toolTip.SetToolTip(this.checkSetValue, "全てのテキストボックスに値をセットする");
			this.checkSetValue.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label6.Location = new System.Drawing.Point(232, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(2, 58);
			this.label6.TabIndex = 7;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// listBoxSubQuery
			// 
			this.listBoxSubQuery.FormattingEnabled = true;
			this.listBoxSubQuery.ItemHeight = 12;
			this.listBoxSubQuery.Location = new System.Drawing.Point(113, 80);
			this.listBoxSubQuery.Name = "listBoxSubQuery";
			this.listBoxSubQuery.Size = new System.Drawing.Size(261, 88);
			this.listBoxSubQuery.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(111, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 12);
			this.label5.TabIndex = 6;
			this.label5.Text = "サブクエリ";
			this.toolTip.SetToolTip(this.label5, "サブクエリ（副問い合わせ）をD&Dで登録する");
			// 
			// checkDistinct
			// 
			this.checkDistinct.AutoSize = true;
			this.checkDistinct.Location = new System.Drawing.Point(10, 14);
			this.checkDistinct.Name = "checkDistinct";
			this.checkDistinct.Size = new System.Drawing.Size(75, 16);
			this.checkDistinct.TabIndex = 0;
			this.checkDistinct.Text = "DISTINCT";
			this.toolTip.SetToolTip(this.checkDistinct, "重複行を除いて抽出する");
			this.checkDistinct.UseVisualStyleBackColor = true;
			// 
			// checkColumnName
			// 
			this.checkColumnName.AutoSize = true;
			this.checkColumnName.Location = new System.Drawing.Point(10, 14);
			this.checkColumnName.Name = "checkColumnName";
			this.checkColumnName.Size = new System.Drawing.Size(63, 16);
			this.checkColumnName.TabIndex = 0;
			this.checkColumnName.Text = "カラム名";
			this.toolTip.SetToolTip(this.checkColumnName, "列のカラム名を出力する");
			this.checkColumnName.UseVisualStyleBackColor = true;
			// 
			// checkComment
			// 
			this.checkComment.AutoSize = true;
			this.checkComment.Location = new System.Drawing.Point(10, 31);
			this.checkComment.Name = "checkComment";
			this.checkComment.Size = new System.Drawing.Size(57, 16);
			this.checkComment.TabIndex = 1;
			this.checkComment.Text = "コメント";
			this.toolTip.SetToolTip(this.checkComment, "コメントを出力する");
			this.checkComment.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkUseJoin);
			this.groupBox2.Controls.Add(this.checkDistinct);
			this.groupBox2.Location = new System.Drawing.Point(14, 65);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(91, 50);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "SQL";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkComment);
			this.groupBox3.Controls.Add(this.checkColumnName);
			this.groupBox3.Location = new System.Drawing.Point(14, 117);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(91, 50);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "ヘッダ出力";
			// 
			// checkUseJoin
			// 
			this.checkUseJoin.AutoSize = true;
			this.checkUseJoin.Location = new System.Drawing.Point(10, 31);
			this.checkUseJoin.Name = "checkUseJoin";
			this.checkUseJoin.Size = new System.Drawing.Size(50, 16);
			this.checkUseJoin.TabIndex = 1;
			this.checkUseJoin.Text = "JOIN";
			this.toolTip.SetToolTip(this.checkUseJoin, "JOIN を使う");
			this.checkUseJoin.UseVisualStyleBackColor = true;
			// 
			// FilePropertyDlg
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(386, 297);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.listBoxSubQuery);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.textAuthor);
			this.Controls.Add(this.textComment);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FilePropertyDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ファイルのプロパティ";
			this.Load += new System.EventHandler(this.FilePropertyDlg_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textComment;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textAuthor;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioDlPermit;
		private System.Windows.Forms.RadioButton radioDlDeny;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textMaxRowNum;
		private System.Windows.Forms.GroupBox groupBox1;
		protected System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.ListBox listBoxSubQuery;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkSetValue;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox checkSqlSelect;
		private System.Windows.Forms.CheckBox checkDistinct;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox checkComment;
		private System.Windows.Forms.CheckBox checkColumnName;
		private System.Windows.Forms.TextBox textEggPermission;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkUseJoin;
	}
}