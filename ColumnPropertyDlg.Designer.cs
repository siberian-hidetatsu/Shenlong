namespace Shenlong
{
	partial class ColumnPropertyDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnPropertyDlg));
			this.label1 = new System.Windows.Forms.Label();
			this.textType = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textLength = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textNULLABLE = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textComment = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textAlias = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabGeneral = new System.Windows.Forms.TabPage();
			this.tabBubInput = new System.Windows.Forms.TabPage();
			this.checkSetValue = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioTextBox = new System.Windows.Forms.RadioButton();
			this.radioLabel = new System.Windows.Forms.RadioButton();
			this.radioNoVisible = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioNecessary = new System.Windows.Forms.RadioButton();
			this.radioNoAppoint = new System.Windows.Forms.RadioButton();
			this.buttonTest = new System.Windows.Forms.Button();
			this.labelDropDownList = new System.Windows.Forms.Label();
			this.textDropDownList = new System.Windows.Forms.TextBox();
			this.tabBubOutput = new System.Windows.Forms.TabPage();
			this.textClassify = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textHyperLink = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.textDateFormat = new System.Windows.Forms.TextBox();
			this.labelDateFormat = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabGeneral.SuspendLayout();
			this.tabBubInput.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabBubOutput.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(38, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "データ型";
			this.toolTip.SetToolTip(this.label1, "項目のデータ型");
			// 
			// textType
			// 
			this.textType.BackColor = System.Drawing.SystemColors.Window;
			this.textType.Location = new System.Drawing.Point(88, 8);
			this.textType.Name = "textType";
			this.textType.ReadOnly = true;
			this.textType.Size = new System.Drawing.Size(104, 19);
			this.textType.TabIndex = 1;
			this.textType.TextChanged += new System.EventHandler(this.textType_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(58, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "長さ";
			this.toolTip.SetToolTip(this.label2, "項目の長さ");
			// 
			// textLength
			// 
			this.textLength.BackColor = System.Drawing.SystemColors.Window;
			this.textLength.Location = new System.Drawing.Point(88, 40);
			this.textLength.Name = "textLength";
			this.textLength.ReadOnly = true;
			this.textLength.Size = new System.Drawing.Size(104, 19);
			this.textLength.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(21, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "NULLABLE";
			this.toolTip.SetToolTip(this.label3, "項目がNULL可能か否か");
			// 
			// textNULLABLE
			// 
			this.textNULLABLE.BackColor = System.Drawing.SystemColors.Window;
			this.textNULLABLE.Location = new System.Drawing.Point(88, 72);
			this.textNULLABLE.Name = "textNULLABLE";
			this.textNULLABLE.ReadOnly = true;
			this.textNULLABLE.Size = new System.Drawing.Size(104, 19);
			this.textNULLABLE.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(45, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 12);
			this.label4.TabIndex = 6;
			this.label4.Text = "コメント";
			this.toolTip.SetToolTip(this.label4, "コメント");
			// 
			// textComment
			// 
			this.textComment.Location = new System.Drawing.Point(88, 104);
			this.textComment.Name = "textComment";
			this.textComment.Size = new System.Drawing.Size(224, 19);
			this.textComment.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 140);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75, 12);
			this.label5.TabIndex = 8;
			this.label5.Text = "出力時の別名";
			this.toolTip.SetToolTip(this.label5, "クエリー出力時に表示される項目名");
			// 
			// textAlias
			// 
			this.textAlias.Location = new System.Drawing.Point(88, 136);
			this.textAlias.Name = "textAlias";
			this.textAlias.Size = new System.Drawing.Size(224, 19);
			this.textAlias.TabIndex = 9;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(272, 264);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(192, 264);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabGeneral);
			this.tabControl.Controls.Add(this.tabBubInput);
			this.tabControl.Controls.Add(this.tabBubOutput);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(336, 248);
			this.tabControl.TabIndex = 0;
			// 
			// tabGeneral
			// 
			this.tabGeneral.Controls.Add(this.textType);
			this.tabGeneral.Controls.Add(this.label1);
			this.tabGeneral.Controls.Add(this.label2);
			this.tabGeneral.Controls.Add(this.textDateFormat);
			this.tabGeneral.Controls.Add(this.textAlias);
			this.tabGeneral.Controls.Add(this.label3);
			this.tabGeneral.Controls.Add(this.textComment);
			this.tabGeneral.Controls.Add(this.label4);
			this.tabGeneral.Controls.Add(this.textNULLABLE);
			this.tabGeneral.Controls.Add(this.labelDateFormat);
			this.tabGeneral.Controls.Add(this.label5);
			this.tabGeneral.Controls.Add(this.textLength);
			this.tabGeneral.Location = new System.Drawing.Point(4, 21);
			this.tabGeneral.Name = "tabGeneral";
			this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tabGeneral.Size = new System.Drawing.Size(328, 223);
			this.tabGeneral.TabIndex = 0;
			this.tabGeneral.Text = "全般";
			this.tabGeneral.UseVisualStyleBackColor = true;
			// 
			// tabBubInput
			// 
			this.tabBubInput.Controls.Add(this.checkSetValue);
			this.tabBubInput.Controls.Add(this.groupBox1);
			this.tabBubInput.Controls.Add(this.groupBox2);
			this.tabBubInput.Controls.Add(this.buttonTest);
			this.tabBubInput.Controls.Add(this.labelDropDownList);
			this.tabBubInput.Controls.Add(this.textDropDownList);
			this.tabBubInput.Location = new System.Drawing.Point(4, 21);
			this.tabBubInput.Name = "tabBubInput";
			this.tabBubInput.Size = new System.Drawing.Size(328, 223);
			this.tabBubInput.TabIndex = 1;
			this.tabBubInput.Text = "バブ入力設定";
			this.tabBubInput.UseVisualStyleBackColor = true;
			// 
			// checkSetValue
			// 
			this.checkSetValue.AutoSize = true;
			this.checkSetValue.Location = new System.Drawing.Point(176, 64);
			this.checkSetValue.Name = "checkSetValue";
			this.checkSetValue.Size = new System.Drawing.Size(89, 16);
			this.checkSetValue.TabIndex = 2;
			this.checkSetValue.Text = "値をセットする";
			this.toolTip.SetToolTip(this.checkSetValue, "テキストボックスに値をセットする");
			this.checkSetValue.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioTextBox);
			this.groupBox1.Controls.Add(this.radioLabel);
			this.groupBox1.Controls.Add(this.radioNoVisible);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "表示形式";
			// 
			// radioTextBox
			// 
			this.radioTextBox.AutoSize = true;
			this.radioTextBox.Checked = true;
			this.radioTextBox.Location = new System.Drawing.Point(16, 16);
			this.radioTextBox.Name = "radioTextBox";
			this.radioTextBox.Size = new System.Drawing.Size(93, 16);
			this.radioTextBox.TabIndex = 0;
			this.radioTextBox.TabStop = true;
			this.radioTextBox.Text = "テキストボックス";
			this.toolTip.SetToolTip(this.radioTextBox, "ユーザーが変更可能");
			this.radioTextBox.UseVisualStyleBackColor = true;
			this.radioTextBox.CheckedChanged += new System.EventHandler(this.radioControl_CheckedChanged);
			// 
			// radioLabel
			// 
			this.radioLabel.AutoSize = true;
			this.radioLabel.Location = new System.Drawing.Point(120, 16);
			this.radioLabel.Name = "radioLabel";
			this.radioLabel.Size = new System.Drawing.Size(47, 16);
			this.radioLabel.TabIndex = 1;
			this.radioLabel.Text = "固定";
			this.toolTip.SetToolTip(this.radioLabel, "ラベルとして表示する");
			this.radioLabel.UseVisualStyleBackColor = true;
			this.radioLabel.CheckedChanged += new System.EventHandler(this.radioControl_CheckedChanged);
			// 
			// radioNoVisible
			// 
			this.radioNoVisible.AutoSize = true;
			this.radioNoVisible.Location = new System.Drawing.Point(176, 16);
			this.radioNoVisible.Name = "radioNoVisible";
			this.radioNoVisible.Size = new System.Drawing.Size(59, 16);
			this.radioNoVisible.TabIndex = 2;
			this.radioNoVisible.Text = "非表示";
			this.toolTip.SetToolTip(this.radioNoVisible, "表示しない");
			this.radioNoVisible.UseVisualStyleBackColor = true;
			this.radioNoVisible.CheckedChanged += new System.EventHandler(this.radioControl_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioNecessary);
			this.groupBox2.Controls.Add(this.radioNoAppoint);
			this.groupBox2.Location = new System.Drawing.Point(8, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(152, 40);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "条件入力";
			// 
			// radioNecessary
			// 
			this.radioNecessary.AutoSize = true;
			this.radioNecessary.Location = new System.Drawing.Point(96, 16);
			this.radioNecessary.Name = "radioNecessary";
			this.radioNecessary.Size = new System.Drawing.Size(47, 16);
			this.radioNecessary.TabIndex = 1;
			this.radioNecessary.Text = "必須";
			this.toolTip.SetToolTip(this.radioNecessary, "条件の入力は必須");
			this.radioNecessary.UseVisualStyleBackColor = true;
			// 
			// radioNoAppoint
			// 
			this.radioNoAppoint.AutoSize = true;
			this.radioNoAppoint.Checked = true;
			this.radioNoAppoint.Location = new System.Drawing.Point(16, 16);
			this.radioNoAppoint.Name = "radioNoAppoint";
			this.radioNoAppoint.Size = new System.Drawing.Size(68, 16);
			this.radioNoAppoint.TabIndex = 0;
			this.radioNoAppoint.TabStop = true;
			this.radioNoAppoint.Text = "指定無し";
			this.toolTip.SetToolTip(this.radioNoAppoint, "入力時の指定は無い");
			this.radioNoAppoint.UseVisualStyleBackColor = true;
			// 
			// buttonTest
			// 
			this.buttonTest.Location = new System.Drawing.Point(104, 104);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(40, 20);
			this.buttonTest.TabIndex = 5;
			this.buttonTest.Text = "テスト";
			this.toolTip.SetToolTip(this.buttonTest, "ドロップダウンリストのテスト表示");
			this.buttonTest.UseVisualStyleBackColor = true;
			this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
			// 
			// labelDropDownList
			// 
			this.labelDropDownList.AutoSize = true;
			this.labelDropDownList.Location = new System.Drawing.Point(8, 112);
			this.labelDropDownList.Name = "labelDropDownList";
			this.labelDropDownList.Size = new System.Drawing.Size(98, 12);
			this.labelDropDownList.TabIndex = 3;
			this.labelDropDownList.Text = "ドロップ ダウン リスト";
			// 
			// textDropDownList
			// 
			this.textDropDownList.AcceptsReturn = true;
			this.textDropDownList.Location = new System.Drawing.Point(8, 128);
			this.textDropDownList.Multiline = true;
			this.textDropDownList.Name = "textDropDownList";
			this.textDropDownList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textDropDownList.Size = new System.Drawing.Size(312, 88);
			this.textDropDownList.TabIndex = 4;
			this.textDropDownList.WordWrap = false;
			// 
			// tabBubOutput
			// 
			this.tabBubOutput.Controls.Add(this.textClassify);
			this.tabBubOutput.Controls.Add(this.label6);
			this.tabBubOutput.Controls.Add(this.textHyperLink);
			this.tabBubOutput.Controls.Add(this.label7);
			this.tabBubOutput.Location = new System.Drawing.Point(4, 21);
			this.tabBubOutput.Name = "tabBubOutput";
			this.tabBubOutput.Size = new System.Drawing.Size(328, 223);
			this.tabBubOutput.TabIndex = 2;
			this.tabBubOutput.Text = "バブ出力設定";
			this.tabBubOutput.UseVisualStyleBackColor = true;
			// 
			// textClassify
			// 
			this.textClassify.Location = new System.Drawing.Point(80, 32);
			this.textClassify.Name = "textClassify";
			this.textClassify.Size = new System.Drawing.Size(240, 19);
			this.textClassify.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(37, 36);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 12);
			this.label6.TabIndex = 2;
			this.label6.Text = "色分け";
			this.toolTip.SetToolTip(this.label6, "比較演算子 + 比較値 + \":\" + 背景色 + (\"/\" + 文字色 + (\"/\" + オプション))");
			// 
			// textHyperLink
			// 
			this.textHyperLink.Location = new System.Drawing.Point(80, 8);
			this.textHyperLink.Name = "textHyperLink";
			this.textHyperLink.Size = new System.Drawing.Size(240, 19);
			this.textHyperLink.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 12);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(68, 12);
			this.label7.TabIndex = 0;
			this.label7.Text = "ハイパーリンク";
			this.toolTip.SetToolTip(this.label7, "シェンロンの卵 + \":\" + 転送元カラム名 + \">\" + 転送先テーブル + \".\" + カラム名");
			// 
			// textDateFormat
			// 
			this.textDateFormat.Location = new System.Drawing.Point(88, 168);
			this.textDateFormat.Name = "textDateFormat";
			this.textDateFormat.Size = new System.Drawing.Size(224, 19);
			this.textDateFormat.TabIndex = 11;
			// 
			// labelDateFormat
			// 
			this.labelDateFormat.AutoSize = true;
			this.labelDateFormat.Location = new System.Drawing.Point(20, 171);
			this.labelDateFormat.Name = "labelDateFormat";
			this.labelDateFormat.Size = new System.Drawing.Size(63, 12);
			this.labelDateFormat.TabIndex = 10;
			this.labelDateFormat.Text = "日付の書式";
			this.toolTip.SetToolTip(this.labelDateFormat, "SQL 日付の条件書式");
			// 
			// ColumnPropertyDlg
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(353, 297);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColumnPropertyDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TableName.FieldName のプロパティ";
			this.Load += new System.EventHandler(this.ColumnPropertyDlg_Load);
			this.Shown += new System.EventHandler(this.ColumnPropertyDlg_Shown);
			this.tabControl.ResumeLayout(false);
			this.tabGeneral.ResumeLayout(false);
			this.tabGeneral.PerformLayout();
			this.tabBubInput.ResumeLayout(false);
			this.tabBubInput.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabBubOutput.ResumeLayout(false);
			this.tabBubOutput.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textLength;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textNULLABLE;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textComment;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textAlias;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TabPage tabBubInput;
		private System.Windows.Forms.TabPage tabBubOutput;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioTextBox;
		private System.Windows.Forms.RadioButton radioLabel;
		private System.Windows.Forms.RadioButton radioNoVisible;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioNecessary;
		private System.Windows.Forms.RadioButton radioNoAppoint;
		private System.Windows.Forms.Button buttonTest;
		private System.Windows.Forms.Label labelDropDownList;
		private System.Windows.Forms.TextBox textDropDownList;
		private System.Windows.Forms.TextBox textClassify;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textHyperLink;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkSetValue;
		private System.Windows.Forms.TextBox textDateFormat;
		private System.Windows.Forms.Label labelDateFormat;
	}
}