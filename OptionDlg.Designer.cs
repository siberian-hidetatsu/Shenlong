namespace Shenlong
{
	partial class OptionDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionDlg));
			this.checkReloadLastColumnsOnStartup = new System.Windows.Forms.CheckBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.checkSelectColumnByDragDrop = new System.Windows.Forms.CheckBox();
			this.checkSaveQueryOutputFile = new System.Windows.Forms.CheckBox();
			this.textQueryOutputFileName = new System.Windows.Forms.TextBox();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioExcelPasteActBookNewSheet = new System.Windows.Forms.RadioButton();
			this.radioExcelPasteActBookActSheet = new System.Windows.Forms.RadioButton();
			this.radioExcelPasteShenBookNewSheet = new System.Windows.Forms.RadioButton();
			this.radioExcelPasteNewBookActSheet = new System.Windows.Forms.RadioButton();
			this.radioExcelPasteNone = new System.Windows.Forms.RadioButton();
			this.buttonSelectQueryOutputFile = new System.Windows.Forms.Button();
			this.checkPasteColumnComments = new System.Windows.Forms.CheckBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkShowSynonymOwner = new System.Windows.Forms.CheckBox();
			this.radioShowColumns = new System.Windows.Forms.RadioButton();
			this.radioClearSelectedColumns = new System.Windows.Forms.RadioButton();
			this.radioAppendAllColumns = new System.Windows.Forms.RadioButton();
			this.comboSqlDateFormat = new System.Windows.Forms.ComboBox();
			this.comboOraMiddleware = new System.Windows.Forms.ComboBox();
			this.checkEditableColumnName = new System.Windows.Forms.CheckBox();
			this.checkMultiInstanceEnabled = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textMaxLogOnHistoryCount = new System.Windows.Forms.TextBox();
			this.textMaxQueryColumnCount = new System.Windows.Forms.TextBox();
			this.textReverseQueryColumnTime = new System.Windows.Forms.TextBox();
			this.textFormMaximumSize = new System.Windows.Forms.TextBox();
			this.textSelectTableName = new System.Windows.Forms.TextBox();
			this.textSelectSynonymName = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textSelectColumns = new System.Windows.Forms.TextBox();
			this.checkShowParamInputDlg = new System.Windows.Forms.CheckBox();
			this.textOracleSqlPlusPath = new System.Windows.Forms.TextBox();
			this.checkPutDiffOwnerToTable = new System.Windows.Forms.CheckBox();
			this.checkResumeAppendLogOnHis = new System.Windows.Forms.CheckBox();
			this.checkAutoChangeLogOn = new System.Windows.Forms.CheckBox();
			this.checkSelectableClearColumnLogOn = new System.Windows.Forms.CheckBox();
			this.checkIntelliTableJoinMenu = new System.Windows.Forms.CheckBox();
			this.checkExpertMode = new System.Windows.Forms.CheckBox();
			this.checkWriteAccessLog = new System.Windows.Forms.CheckBox();
			this.checkLogOnPwdToolTip = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textMaxInputParamHistoryCount = new System.Windows.Forms.TextBox();
			this.checkClearQueryColumnWhenOraLogOn = new System.Windows.Forms.CheckBox();
			this.checkEnableExcelPasteNone = new System.Windows.Forms.CheckBox();
			this.textColumnListBackColorName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkShowQueryRecordCount = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textQueryColumnColorNames = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSettings = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tabQueryOutput = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.tabPageExpertSettings = new System.Windows.Forms.TabPage();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabSettings.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabQueryOutput.SuspendLayout();
			this.tabPageExpertSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkReloadLastColumnsOnStartup
			// 
			this.checkReloadLastColumnsOnStartup.AutoSize = true;
			this.checkReloadLastColumnsOnStartup.Location = new System.Drawing.Point(8, 8);
			this.checkReloadLastColumnsOnStartup.Name = "checkReloadLastColumnsOnStartup";
			this.checkReloadLastColumnsOnStartup.Size = new System.Drawing.Size(181, 16);
			this.checkReloadLastColumnsOnStartup.TabIndex = 0;
			this.checkReloadLastColumnsOnStartup.Text = "起動時に前回の状態を読み込む";
			this.toolTip.SetToolTip(this.checkReloadLastColumnsOnStartup, "前回終了時の状態を復元してプログラムを起動する");
			this.checkReloadLastColumnsOnStartup.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(208, 272);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 24);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(288, 272);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "キャンセル";
			// 
			// checkSelectColumnByDragDrop
			// 
			this.checkSelectColumnByDragDrop.AutoSize = true;
			this.checkSelectColumnByDragDrop.Location = new System.Drawing.Point(8, 32);
			this.checkSelectColumnByDragDrop.Name = "checkSelectColumnByDragDrop";
			this.checkSelectColumnByDragDrop.Size = new System.Drawing.Size(287, 16);
			this.checkSelectColumnByDragDrop.TabIndex = 1;
			this.checkSelectColumnByDragDrop.Text = "ドラッグ＆ドロップ（またはダブルクリック）で項目を選択する";
			this.toolTip.SetToolTip(this.checkSelectColumnByDragDrop, "ドラッグ＆ドロップとシングル クリックでの選択方法を切り替える");
			this.checkSelectColumnByDragDrop.UseVisualStyleBackColor = true;
			// 
			// checkSaveQueryOutputFile
			// 
			this.checkSaveQueryOutputFile.AutoSize = true;
			this.checkSaveQueryOutputFile.Location = new System.Drawing.Point(8, 32);
			this.checkSaveQueryOutputFile.Name = "checkSaveQueryOutputFile";
			this.checkSaveQueryOutputFile.Size = new System.Drawing.Size(177, 16);
			this.checkSaveQueryOutputFile.TabIndex = 1;
			this.checkSaveQueryOutputFile.Text = "クエリー結果をファイルに保存する";
			this.toolTip.SetToolTip(this.checkSaveQueryOutputFile, "クエリーの出力結果を指定されたファイルに保存する");
			this.checkSaveQueryOutputFile.UseVisualStyleBackColor = true;
			// 
			// textQueryOutputFileName
			// 
			this.textQueryOutputFileName.Location = new System.Drawing.Point(24, 56);
			this.textQueryOutputFileName.Name = "textQueryOutputFileName";
			this.textQueryOutputFileName.Size = new System.Drawing.Size(256, 19);
			this.textQueryOutputFileName.TabIndex = 2;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "クエリー出力ファイル (*.txt)|*.txt|すべてのファイル(*.*)|*.*";
			this.saveFileDialog.Title = "クエリー出力ファイル";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioExcelPasteActBookNewSheet);
			this.groupBox1.Controls.Add(this.radioExcelPasteActBookActSheet);
			this.groupBox1.Controls.Add(this.radioExcelPasteShenBookNewSheet);
			this.groupBox1.Controls.Add(this.radioExcelPasteNewBookActSheet);
			this.groupBox1.Controls.Add(this.radioExcelPasteNone);
			this.groupBox1.Location = new System.Drawing.Point(8, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 72);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Excel シートへの貼り付け先";
			// 
			// radioExcelPasteActBookNewSheet
			// 
			this.radioExcelPasteActBookNewSheet.AutoSize = true;
			this.radioExcelPasteActBookNewSheet.Location = new System.Drawing.Point(144, 16);
			this.radioExcelPasteActBookNewSheet.Name = "radioExcelPasteActBookNewSheet";
			this.radioExcelPasteActBookNewSheet.Size = new System.Drawing.Size(133, 16);
			this.radioExcelPasteActBookNewSheet.TabIndex = 3;
			this.radioExcelPasteActBookNewSheet.TabStop = true;
			this.radioExcelPasteActBookNewSheet.Text = "既存ブックの新規シート";
			this.toolTip.SetToolTip(this.radioExcelPasteActBookNewSheet, "現在のブックに新たなシートを追加して、そこに貼り付ける");
			this.radioExcelPasteActBookNewSheet.UseVisualStyleBackColor = true;
			// 
			// radioExcelPasteActBookActSheet
			// 
			this.radioExcelPasteActBookActSheet.AutoSize = true;
			this.radioExcelPasteActBookActSheet.Location = new System.Drawing.Point(8, 48);
			this.radioExcelPasteActBookActSheet.Name = "radioExcelPasteActBookActSheet";
			this.radioExcelPasteActBookActSheet.Size = new System.Drawing.Size(133, 16);
			this.radioExcelPasteActBookActSheet.TabIndex = 2;
			this.radioExcelPasteActBookActSheet.TabStop = true;
			this.radioExcelPasteActBookActSheet.Text = "既存ブックの既存シート";
			this.toolTip.SetToolTip(this.radioExcelPasteActBookActSheet, "現在開かれているブックのシートに上書きする");
			this.radioExcelPasteActBookActSheet.UseVisualStyleBackColor = true;
			// 
			// radioExcelPasteShenBookNewSheet
			// 
			this.radioExcelPasteShenBookNewSheet.AutoSize = true;
			this.radioExcelPasteShenBookNewSheet.Location = new System.Drawing.Point(144, 32);
			this.radioExcelPasteShenBookNewSheet.Name = "radioExcelPasteShenBookNewSheet";
			this.radioExcelPasteShenBookNewSheet.Size = new System.Drawing.Size(133, 16);
			this.radioExcelPasteShenBookNewSheet.TabIndex = 4;
			this.radioExcelPasteShenBookNewSheet.TabStop = true;
			this.radioExcelPasteShenBookNewSheet.Text = "専用ブックの新規シート";
			this.toolTip.SetToolTip(this.radioExcelPasteShenBookNewSheet, "専用のブックに新たなシートを追加して、そこに貼り付ける");
			this.radioExcelPasteShenBookNewSheet.UseVisualStyleBackColor = true;
			// 
			// radioExcelPasteNewBookActSheet
			// 
			this.radioExcelPasteNewBookActSheet.AutoSize = true;
			this.radioExcelPasteNewBookActSheet.Location = new System.Drawing.Point(8, 32);
			this.radioExcelPasteNewBookActSheet.Name = "radioExcelPasteNewBookActSheet";
			this.radioExcelPasteNewBookActSheet.Size = new System.Drawing.Size(109, 16);
			this.radioExcelPasteNewBookActSheet.TabIndex = 1;
			this.radioExcelPasteNewBookActSheet.TabStop = true;
			this.radioExcelPasteNewBookActSheet.Text = "新規ブックのシート";
			this.toolTip.SetToolTip(this.radioExcelPasteNewBookActSheet, "新しいブックを作成して、そこのシートに貼り付ける");
			this.radioExcelPasteNewBookActSheet.UseVisualStyleBackColor = true;
			// 
			// radioExcelPasteNone
			// 
			this.radioExcelPasteNone.AutoSize = true;
			this.radioExcelPasteNone.Location = new System.Drawing.Point(8, 16);
			this.radioExcelPasteNone.Name = "radioExcelPasteNone";
			this.radioExcelPasteNone.Size = new System.Drawing.Size(85, 16);
			this.radioExcelPasteNone.TabIndex = 0;
			this.radioExcelPasteNone.TabStop = true;
			this.radioExcelPasteNone.Text = "貼り付けない";
			this.toolTip.SetToolTip(this.radioExcelPasteNone, "Excel には貼り付けない");
			this.radioExcelPasteNone.UseVisualStyleBackColor = true;
			// 
			// buttonSelectQueryOutputFile
			// 
			this.buttonSelectQueryOutputFile.Image = global::Shenlong.Properties.Resources.folder;
			this.buttonSelectQueryOutputFile.Location = new System.Drawing.Point(288, 54);
			this.buttonSelectQueryOutputFile.Name = "buttonSelectQueryOutputFile";
			this.buttonSelectQueryOutputFile.Size = new System.Drawing.Size(24, 22);
			this.buttonSelectQueryOutputFile.TabIndex = 3;
			this.toolTip.SetToolTip(this.buttonSelectQueryOutputFile, "保存先のファイルを選択するダイアログを表示する");
			this.buttonSelectQueryOutputFile.UseVisualStyleBackColor = true;
			this.buttonSelectQueryOutputFile.Click += new System.EventHandler(this.buttonSelectQueryOutputFile_Click);
			// 
			// checkPasteColumnComments
			// 
			this.checkPasteColumnComments.AutoSize = true;
			this.checkPasteColumnComments.Location = new System.Drawing.Point(8, 8);
			this.checkPasteColumnComments.Name = "checkPasteColumnComments";
			this.checkPasteColumnComments.Size = new System.Drawing.Size(231, 16);
			this.checkPasteColumnComments.TabIndex = 0;
			this.checkPasteColumnComments.Text = "クエリー結果に各項目のコメントも付け加える";
			this.toolTip.SetToolTip(this.checkPasteColumnComments, "出力された項目名の下に、コメントを貼り付ける");
			this.checkPasteColumnComments.UseVisualStyleBackColor = true;
			// 
			// checkShowSynonymOwner
			// 
			this.checkShowSynonymOwner.AutoSize = true;
			this.checkShowSynonymOwner.Location = new System.Drawing.Point(8, 56);
			this.checkShowSynonymOwner.Name = "checkShowSynonymOwner";
			this.checkShowSynonymOwner.Size = new System.Drawing.Size(183, 16);
			this.checkShowSynonymOwner.TabIndex = 2;
			this.checkShowSynonymOwner.Text = "シノニムの前にオーナーを表示する";
			this.toolTip.SetToolTip(this.checkShowSynonymOwner, "[オーナー].[テーブル名] の形式で表示する");
			this.checkShowSynonymOwner.UseVisualStyleBackColor = true;
			// 
			// radioShowColumns
			// 
			this.radioShowColumns.AutoSize = true;
			this.radioShowColumns.Location = new System.Drawing.Point(8, 16);
			this.radioShowColumns.Name = "radioShowColumns";
			this.radioShowColumns.Size = new System.Drawing.Size(166, 16);
			this.radioShowColumns.TabIndex = 0;
			this.radioShowColumns.TabStop = true;
			this.radioShowColumns.Text = "項目名の一覧を表示するのみ";
			this.toolTip.SetToolTip(this.radioShowColumns, "項目名の一覧を表示する");
			this.radioShowColumns.UseVisualStyleBackColor = true;
			// 
			// radioClearSelectedColumns
			// 
			this.radioClearSelectedColumns.AutoSize = true;
			this.radioClearSelectedColumns.Location = new System.Drawing.Point(8, 32);
			this.radioClearSelectedColumns.Name = "radioClearSelectedColumns";
			this.radioClearSelectedColumns.Size = new System.Drawing.Size(204, 16);
			this.radioClearSelectedColumns.TabIndex = 0;
			this.radioClearSelectedColumns.TabStop = true;
			this.radioClearSelectedColumns.Text = "追加されているクエリー項目をクリアする";
			this.toolTip.SetToolTip(this.radioClearSelectedColumns, "クエリー項目を初期化する");
			this.radioClearSelectedColumns.UseVisualStyleBackColor = true;
			// 
			// radioAppendAllColumns
			// 
			this.radioAppendAllColumns.AutoSize = true;
			this.radioAppendAllColumns.Location = new System.Drawing.Point(8, 48);
			this.radioAppendAllColumns.Name = "radioAppendAllColumns";
			this.radioAppendAllColumns.Size = new System.Drawing.Size(249, 16);
			this.radioAppendAllColumns.TabIndex = 0;
			this.radioAppendAllColumns.TabStop = true;
			this.radioAppendAllColumns.Text = "表示された全ての項目をクエリー項目に追加する";
			this.toolTip.SetToolTip(this.radioAppendAllColumns, "全ての項目を自動的にクエリー項目に追加する");
			this.radioAppendAllColumns.UseVisualStyleBackColor = true;
			// 
			// comboSqlDateFormat
			// 
			this.comboSqlDateFormat.FormattingEnabled = true;
			this.comboSqlDateFormat.Location = new System.Drawing.Point(128, 179);
			this.comboSqlDateFormat.Name = "comboSqlDateFormat";
			this.comboSqlDateFormat.Size = new System.Drawing.Size(160, 20);
			this.comboSqlDateFormat.TabIndex = 5;
			this.toolTip.SetToolTip(this.comboSqlDateFormat, "クエリー項目の日付型の条件書式");
			// 
			// comboOraMiddleware
			// 
			this.comboOraMiddleware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOraMiddleware.FormattingEnabled = true;
			this.comboOraMiddleware.Location = new System.Drawing.Point(120, 176);
			this.comboOraMiddleware.Name = "comboOraMiddleware";
			this.comboOraMiddleware.Size = new System.Drawing.Size(104, 20);
			this.comboOraMiddleware.TabIndex = 6;
			this.toolTip.SetToolTip(this.comboOraMiddleware, "System.Data.OracleClient | System.Data.OleDb");
			// 
			// checkEditableColumnName
			// 
			this.checkEditableColumnName.AutoSize = true;
			this.checkEditableColumnName.Location = new System.Drawing.Point(8, 160);
			this.checkEditableColumnName.Name = "checkEditableColumnName";
			this.checkEditableColumnName.Size = new System.Drawing.Size(146, 16);
			this.checkEditableColumnName.TabIndex = 7;
			this.checkEditableColumnName.Text = "項目名の編集を許可する";
			this.toolTip.SetToolTip(this.checkEditableColumnName, "項目名に関数を指定した時は :: で始める");
			this.checkEditableColumnName.UseVisualStyleBackColor = true;
			// 
			// checkMultiInstanceEnabled
			// 
			this.checkMultiInstanceEnabled.AutoSize = true;
			this.checkMultiInstanceEnabled.Location = new System.Drawing.Point(8, 208);
			this.checkMultiInstanceEnabled.Name = "checkMultiInstanceEnabled";
			this.checkMultiInstanceEnabled.Size = new System.Drawing.Size(124, 16);
			this.checkMultiInstanceEnabled.TabIndex = 6;
			this.checkMultiInstanceEnabled.Text = "多重起動を許可する";
			this.toolTip.SetToolTip(this.checkMultiInstanceEnabled, "shenlong を多重起動できるようにする");
			this.checkMultiInstanceEnabled.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 65);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(101, 12);
			this.label6.TabIndex = 3;
			this.label6.Text = "MaxLogOnHistory...";
			this.toolTip.SetToolTip(this.label6, "MaxLogOnHistoryCount");
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 86);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 12);
			this.label7.TabIndex = 5;
			this.label7.Text = "MaxQueryColumn...";
			this.toolTip.SetToolTip(this.label7, "MaxQueryColumnCount");
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 107);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 12);
			this.label8.TabIndex = 7;
			this.label8.Text = "ReverseQuery...";
			this.toolTip.SetToolTip(this.label8, "ReverseQueryColumnTime");
			// 
			// textMaxLogOnHistoryCount
			// 
			this.textMaxLogOnHistoryCount.Location = new System.Drawing.Point(112, 62);
			this.textMaxLogOnHistoryCount.Name = "textMaxLogOnHistoryCount";
			this.textMaxLogOnHistoryCount.Size = new System.Drawing.Size(48, 19);
			this.textMaxLogOnHistoryCount.TabIndex = 4;
			this.textMaxLogOnHistoryCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.textMaxLogOnHistoryCount, "ログオン履歴の最大数");
			this.textMaxLogOnHistoryCount.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textMaxLogOnHistoryCount.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// textMaxQueryColumnCount
			// 
			this.textMaxQueryColumnCount.Location = new System.Drawing.Point(112, 83);
			this.textMaxQueryColumnCount.Name = "textMaxQueryColumnCount";
			this.textMaxQueryColumnCount.Size = new System.Drawing.Size(48, 19);
			this.textMaxQueryColumnCount.TabIndex = 6;
			this.textMaxQueryColumnCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.textMaxQueryColumnCount, "クエリー項目の最大数");
			this.textMaxQueryColumnCount.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textMaxQueryColumnCount.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// textReverseQueryColumnTime
			// 
			this.textReverseQueryColumnTime.Location = new System.Drawing.Point(112, 104);
			this.textReverseQueryColumnTime.Name = "textReverseQueryColumnTime";
			this.textReverseQueryColumnTime.Size = new System.Drawing.Size(48, 19);
			this.textReverseQueryColumnTime.TabIndex = 8;
			this.textReverseQueryColumnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.textReverseQueryColumnTime, "クエリー項目を反転表示する時間(ms)!");
			this.textReverseQueryColumnTime.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textReverseQueryColumnTime.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// textFormMaximumSize
			// 
			this.textFormMaximumSize.Location = new System.Drawing.Point(112, 143);
			this.textFormMaximumSize.Name = "textFormMaximumSize";
			this.textFormMaximumSize.Size = new System.Drawing.Size(48, 19);
			this.textFormMaximumSize.TabIndex = 11;
			this.toolTip.SetToolTip(this.textFormMaximumSize, "フォームの最大サイズ");
			this.textFormMaximumSize.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textFormMaximumSize.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// textSelectTableName
			// 
			this.textSelectTableName.Location = new System.Drawing.Point(288, 46);
			this.textSelectTableName.Name = "textSelectTableName";
			this.textSelectTableName.Size = new System.Drawing.Size(48, 19);
			this.textSelectTableName.TabIndex = 20;
			this.toolTip.SetToolTip(this.textSelectTableName, "TABLE, VIEW のテーブル名を取得する SELECT 文!");
			this.textSelectTableName.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textSelectTableName.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// textSelectSynonymName
			// 
			this.textSelectSynonymName.Location = new System.Drawing.Point(288, 68);
			this.textSelectSynonymName.Name = "textSelectSynonymName";
			this.textSelectSynonymName.Size = new System.Drawing.Size(48, 19);
			this.textSelectSynonymName.TabIndex = 22;
			this.toolTip.SetToolTip(this.textSelectSynonymName, "SYNONYM のテーブル名を取得する SELECT 文!");
			this.textSelectSynonymName.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textSelectSynonymName.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(190, 71);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(89, 12);
			this.label12.TabIndex = 21;
			this.label12.Text = "SelectSynonym...";
			this.toolTip.SetToolTip(this.label12, "SelectSynonymName");
			// 
			// textSelectColumns
			// 
			this.textSelectColumns.Location = new System.Drawing.Point(288, 90);
			this.textSelectColumns.Name = "textSelectColumns";
			this.textSelectColumns.Size = new System.Drawing.Size(48, 19);
			this.textSelectColumns.TabIndex = 24;
			this.toolTip.SetToolTip(this.textSelectColumns, "選択されたテーブルのカラムを取得する SELECT 文!");
			this.textSelectColumns.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textSelectColumns.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// checkShowParamInputDlg
			// 
			this.checkShowParamInputDlg.AutoSize = true;
			this.checkShowParamInputDlg.Location = new System.Drawing.Point(152, 160);
			this.checkShowParamInputDlg.Name = "checkShowParamInputDlg";
			this.checkShowParamInputDlg.Size = new System.Drawing.Size(193, 16);
			this.checkShowParamInputDlg.TabIndex = 8;
			this.checkShowParamInputDlg.Text = "抽出条件入力ダイアログを表示する";
			this.toolTip.SetToolTip(this.checkShowParamInputDlg, "クエリー前に抽出条件を入力する");
			this.checkShowParamInputDlg.UseVisualStyleBackColor = true;
			this.checkShowParamInputDlg.Visible = false;
			// 
			// textOracleSqlPlusPath
			// 
			this.textOracleSqlPlusPath.Location = new System.Drawing.Point(112, 164);
			this.textOracleSqlPlusPath.Name = "textOracleSqlPlusPath";
			this.textOracleSqlPlusPath.Size = new System.Drawing.Size(48, 19);
			this.textOracleSqlPlusPath.TabIndex = 13;
			this.toolTip.SetToolTip(this.textOracleSqlPlusPath, "オラクルの SQL*Plus のパス!");
			this.textOracleSqlPlusPath.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textOracleSqlPlusPath.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// checkPutDiffOwnerToTable
			// 
			this.checkPutDiffOwnerToTable.AutoSize = true;
			this.checkPutDiffOwnerToTable.Location = new System.Drawing.Point(8, 203);
			this.checkPutDiffOwnerToTable.Name = "checkPutDiffOwnerToTable";
			this.checkPutDiffOwnerToTable.Size = new System.Drawing.Size(133, 16);
			this.checkPutDiffOwnerToTable.TabIndex = 15;
			this.checkPutDiffOwnerToTable.Text = "PutDiffOwnerToTable";
			this.checkPutDiffOwnerToTable.ThreeState = true;
			this.toolTip.SetToolTip(this.checkPutDiffOwnerToTable, "オーナーが違うテーブル名に \"OWNER.\" を付ける!");
			this.checkPutDiffOwnerToTable.UseVisualStyleBackColor = true;
			// 
			// checkResumeAppendLogOnHis
			// 
			this.checkResumeAppendLogOnHis.AutoSize = true;
			this.checkResumeAppendLogOnHis.Location = new System.Drawing.Point(8, 8);
			this.checkResumeAppendLogOnHis.Name = "checkResumeAppendLogOnHis";
			this.checkResumeAppendLogOnHis.Size = new System.Drawing.Size(152, 16);
			this.checkResumeAppendLogOnHis.TabIndex = 0;
			this.checkResumeAppendLogOnHis.Text = "ResumeAppendLogOnHis";
			this.checkResumeAppendLogOnHis.ThreeState = true;
			this.toolTip.SetToolTip(this.checkResumeAppendLogOnHis, "ログオン履歴に追加する状態の復元設定!");
			this.checkResumeAppendLogOnHis.UseVisualStyleBackColor = true;
			// 
			// checkAutoChangeLogOn
			// 
			this.checkAutoChangeLogOn.AutoSize = true;
			this.checkAutoChangeLogOn.Location = new System.Drawing.Point(8, 26);
			this.checkAutoChangeLogOn.Name = "checkAutoChangeLogOn";
			this.checkAutoChangeLogOn.Size = new System.Drawing.Size(118, 16);
			this.checkAutoChangeLogOn.TabIndex = 1;
			this.checkAutoChangeLogOn.Text = "AutoChangeLogOn";
			this.checkAutoChangeLogOn.ThreeState = true;
			this.toolTip.SetToolTip(this.checkAutoChangeLogOn, "ログオン先を自動で切り替える設定!");
			this.checkAutoChangeLogOn.UseVisualStyleBackColor = true;
			// 
			// checkSelectableClearColumnLogOn
			// 
			this.checkSelectableClearColumnLogOn.AutoSize = true;
			this.checkSelectableClearColumnLogOn.Location = new System.Drawing.Point(8, 44);
			this.checkSelectableClearColumnLogOn.Name = "checkSelectableClearColumnLogOn";
			this.checkSelectableClearColumnLogOn.Size = new System.Drawing.Size(148, 16);
			this.checkSelectableClearColumnLogOn.TabIndex = 2;
			this.checkSelectableClearColumnLogOn.Text = "SelectableClearColumn...";
			this.checkSelectableClearColumnLogOn.ThreeState = true;
			this.toolTip.SetToolTip(this.checkSelectableClearColumnLogOn, "ログオン時にクエリ項目をクリアするか否かを選択できる設定! (SelectableClearColumnLogOn)");
			this.checkSelectableClearColumnLogOn.UseVisualStyleBackColor = true;
			// 
			// checkIntelliTableJoinMenu
			// 
			this.checkIntelliTableJoinMenu.AutoSize = true;
			this.checkIntelliTableJoinMenu.Location = new System.Drawing.Point(8, 125);
			this.checkIntelliTableJoinMenu.Name = "checkIntelliTableJoinMenu";
			this.checkIntelliTableJoinMenu.Size = new System.Drawing.Size(129, 16);
			this.checkIntelliTableJoinMenu.TabIndex = 9;
			this.checkIntelliTableJoinMenu.Text = "IntelliTableJoinMenu";
			this.checkIntelliTableJoinMenu.ThreeState = true;
			this.toolTip.SetToolTip(this.checkIntelliTableJoinMenu, "テーブル結合メニューで、同じカラム名を別表示にする設定!");
			this.checkIntelliTableJoinMenu.UseVisualStyleBackColor = true;
			// 
			// checkExpertMode
			// 
			this.checkExpertMode.AutoSize = true;
			this.checkExpertMode.Location = new System.Drawing.Point(8, 185);
			this.checkExpertMode.Name = "checkExpertMode";
			this.checkExpertMode.Size = new System.Drawing.Size(84, 16);
			this.checkExpertMode.TabIndex = 14;
			this.checkExpertMode.Text = "ExpertMode";
			this.checkExpertMode.ThreeState = true;
			this.toolTip.SetToolTip(this.checkExpertMode, "エキスパート用で起動するか否か");
			this.checkExpertMode.UseVisualStyleBackColor = true;
			// 
			// checkWriteAccessLog
			// 
			this.checkWriteAccessLog.AutoSize = true;
			this.checkWriteAccessLog.Location = new System.Drawing.Point(192, 178);
			this.checkWriteAccessLog.Name = "checkWriteAccessLog";
			this.checkWriteAccessLog.Size = new System.Drawing.Size(106, 16);
			this.checkWriteAccessLog.TabIndex = 31;
			this.checkWriteAccessLog.Text = "WriteAccessLog";
			this.checkWriteAccessLog.ThreeState = true;
			this.toolTip.SetToolTip(this.checkWriteAccessLog, "アクセス ログを保存する設定!");
			this.checkWriteAccessLog.UseVisualStyleBackColor = true;
			// 
			// checkLogOnPwdToolTip
			// 
			this.checkLogOnPwdToolTip.AutoSize = true;
			this.checkLogOnPwdToolTip.Location = new System.Drawing.Point(192, 197);
			this.checkLogOnPwdToolTip.Name = "checkLogOnPwdToolTip";
			this.checkLogOnPwdToolTip.Size = new System.Drawing.Size(115, 16);
			this.checkLogOnPwdToolTip.TabIndex = 32;
			this.checkLogOnPwdToolTip.Text = "LogOnPwdToolTip";
			this.checkLogOnPwdToolTip.ThreeState = true;
			this.toolTip.SetToolTip(this.checkLogOnPwdToolTip, "ログオン パスワードで tooltip を表示する設定!");
			this.checkLogOnPwdToolTip.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(190, 159);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(89, 12);
			this.label3.TabIndex = 29;
			this.label3.Text = "MaxInputParam...";
			this.toolTip.SetToolTip(this.label3, "MaxInputParamHistoryCount");
			// 
			// textMaxInputParamHistoryCount
			// 
			this.textMaxInputParamHistoryCount.Location = new System.Drawing.Point(288, 156);
			this.textMaxInputParamHistoryCount.Name = "textMaxInputParamHistoryCount";
			this.textMaxInputParamHistoryCount.Size = new System.Drawing.Size(48, 19);
			this.textMaxInputParamHistoryCount.TabIndex = 30;
			this.textMaxInputParamHistoryCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip.SetToolTip(this.textMaxInputParamHistoryCount, "抽出条件ダイアログの入力履歴の最大数!");
			this.textMaxInputParamHistoryCount.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textMaxInputParamHistoryCount.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// checkClearQueryColumnWhenOraLogOn
			// 
			this.checkClearQueryColumnWhenOraLogOn.AutoSize = true;
			this.checkClearQueryColumnWhenOraLogOn.Location = new System.Drawing.Point(144, 208);
			this.checkClearQueryColumnWhenOraLogOn.Name = "checkClearQueryColumnWhenOraLogOn";
			this.checkClearQueryColumnWhenOraLogOn.Size = new System.Drawing.Size(181, 16);
			this.checkClearQueryColumnWhenOraLogOn.TabIndex = 7;
			this.checkClearQueryColumnWhenOraLogOn.Text = "ログオン時にクエリ項目をクリアする";
			this.toolTip.SetToolTip(this.checkClearQueryColumnWhenOraLogOn, "起動中のみ適用される");
			this.checkClearQueryColumnWhenOraLogOn.UseVisualStyleBackColor = true;
			this.checkClearQueryColumnWhenOraLogOn.Visible = false;
			// 
			// checkEnableExcelPasteNone
			// 
			this.checkEnableExcelPasteNone.AutoSize = true;
			this.checkEnableExcelPasteNone.Location = new System.Drawing.Point(192, 8);
			this.checkEnableExcelPasteNone.Name = "checkEnableExcelPasteNone";
			this.checkEnableExcelPasteNone.Size = new System.Drawing.Size(141, 16);
			this.checkEnableExcelPasteNone.TabIndex = 17;
			this.checkEnableExcelPasteNone.Text = "EnableExcelPasteNone";
			this.checkEnableExcelPasteNone.ThreeState = true;
			this.toolTip.SetToolTip(this.checkEnableExcelPasteNone, "\"Excel へ貼り付けない\" オプションを有効にする!");
			this.checkEnableExcelPasteNone.UseVisualStyleBackColor = true;
			// 
			// textColumnListBackColorName
			// 
			this.textColumnListBackColorName.Location = new System.Drawing.Point(288, 112);
			this.textColumnListBackColorName.Name = "textColumnListBackColorName";
			this.textColumnListBackColorName.Size = new System.Drawing.Size(48, 19);
			this.textColumnListBackColorName.TabIndex = 26;
			this.toolTip.SetToolTip(this.textColumnListBackColorName, "カラム一覧の背景名色!");
			this.textColumnListBackColorName.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textColumnListBackColorName.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(190, 115);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(94, 12);
			this.label4.TabIndex = 25;
			this.label4.Text = "ColumnListBack...";
			this.toolTip.SetToolTip(this.label4, "ColumnListBackColorName");
			// 
			// checkShowQueryRecordCount
			// 
			this.checkShowQueryRecordCount.AutoSize = true;
			this.checkShowQueryRecordCount.Location = new System.Drawing.Point(192, 27);
			this.checkShowQueryRecordCount.Name = "checkShowQueryRecordCount";
			this.checkShowQueryRecordCount.Size = new System.Drawing.Size(147, 16);
			this.checkShowQueryRecordCount.TabIndex = 18;
			this.checkShowQueryRecordCount.Text = "ShowQueryRecordCount";
			this.checkShowQueryRecordCount.ThreeState = true;
			this.toolTip.SetToolTip(this.checkShowQueryRecordCount, "クエリー前にレコード件数を表示する!");
			this.checkShowQueryRecordCount.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(190, 137);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 12);
			this.label5.TabIndex = 27;
			this.label5.Text = "QueryColumn...";
			this.toolTip.SetToolTip(this.label5, "QueryColumnColorNames");
			// 
			// textQueryColumnColorNames
			// 
			this.textQueryColumnColorNames.Location = new System.Drawing.Point(288, 134);
			this.textQueryColumnColorNames.Name = "textQueryColumnColorNames";
			this.textQueryColumnColorNames.Size = new System.Drawing.Size(48, 19);
			this.textQueryColumnColorNames.TabIndex = 28;
			this.toolTip.SetToolTip(this.textQueryColumnColorNames, "クエリー項目のテーブル毎の識別色名!");
			this.textQueryColumnColorNames.TextChanged += new System.EventHandler(this.textExpSet_TextChanged);
			this.textQueryColumnColorNames.Leave += new System.EventHandler(this.textExpSet_Leave);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabSettings);
			this.tabControl.Controls.Add(this.tabQueryOutput);
			this.tabControl.Controls.Add(this.tabPageExpertSettings);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.ShowToolTips = true;
			this.tabControl.Size = new System.Drawing.Size(352, 256);
			this.tabControl.TabIndex = 0;
			// 
			// tabSettings
			// 
			this.tabSettings.Controls.Add(this.checkShowParamInputDlg);
			this.tabSettings.Controls.Add(this.checkEditableColumnName);
			this.tabSettings.Controls.Add(this.comboSqlDateFormat);
			this.tabSettings.Controls.Add(this.label1);
			this.tabSettings.Controls.Add(this.checkMultiInstanceEnabled);
			this.tabSettings.Controls.Add(this.checkClearQueryColumnWhenOraLogOn);
			this.tabSettings.Controls.Add(this.groupBox2);
			this.tabSettings.Controls.Add(this.checkReloadLastColumnsOnStartup);
			this.tabSettings.Controls.Add(this.checkShowSynonymOwner);
			this.tabSettings.Controls.Add(this.checkSelectColumnByDragDrop);
			this.tabSettings.Location = new System.Drawing.Point(4, 22);
			this.tabSettings.Name = "tabSettings";
			this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabSettings.Size = new System.Drawing.Size(344, 230);
			this.tabSettings.TabIndex = 0;
			this.tabSettings.Text = "動作設定";
			this.tabSettings.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 184);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "SQL 日付の条件書式";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioAppendAllColumns);
			this.groupBox2.Controls.Add(this.radioClearSelectedColumns);
			this.groupBox2.Controls.Add(this.radioShowColumns);
			this.groupBox2.Location = new System.Drawing.Point(8, 80);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(264, 72);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "テーブルを選択した時の処理";
			// 
			// tabQueryOutput
			// 
			this.tabQueryOutput.Controls.Add(this.comboOraMiddleware);
			this.tabQueryOutput.Controls.Add(this.label2);
			this.tabQueryOutput.Controls.Add(this.checkPasteColumnComments);
			this.tabQueryOutput.Controls.Add(this.checkSaveQueryOutputFile);
			this.tabQueryOutput.Controls.Add(this.groupBox1);
			this.tabQueryOutput.Controls.Add(this.textQueryOutputFileName);
			this.tabQueryOutput.Controls.Add(this.buttonSelectQueryOutputFile);
			this.tabQueryOutput.Location = new System.Drawing.Point(4, 22);
			this.tabQueryOutput.Name = "tabQueryOutput";
			this.tabQueryOutput.Padding = new System.Windows.Forms.Padding(3);
			this.tabQueryOutput.Size = new System.Drawing.Size(344, 230);
			this.tabQueryOutput.TabIndex = 1;
			this.tabQueryOutput.Text = "クエリー出力";
			this.tabQueryOutput.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 180);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(110, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "Oracle への接続方法";
			// 
			// tabPageExpertSettings
			// 
			this.tabPageExpertSettings.AutoScroll = true;
			this.tabPageExpertSettings.Controls.Add(this.textQueryColumnColorNames);
			this.tabPageExpertSettings.Controls.Add(this.textColumnListBackColorName);
			this.tabPageExpertSettings.Controls.Add(this.checkShowQueryRecordCount);
			this.tabPageExpertSettings.Controls.Add(this.checkEnableExcelPasteNone);
			this.tabPageExpertSettings.Controls.Add(this.checkExpertMode);
			this.tabPageExpertSettings.Controls.Add(this.checkIntelliTableJoinMenu);
			this.tabPageExpertSettings.Controls.Add(this.checkSelectableClearColumnLogOn);
			this.tabPageExpertSettings.Controls.Add(this.checkAutoChangeLogOn);
			this.tabPageExpertSettings.Controls.Add(this.checkResumeAppendLogOnHis);
			this.tabPageExpertSettings.Controls.Add(this.checkLogOnPwdToolTip);
			this.tabPageExpertSettings.Controls.Add(this.checkWriteAccessLog);
			this.tabPageExpertSettings.Controls.Add(this.checkPutDiffOwnerToTable);
			this.tabPageExpertSettings.Controls.Add(this.groupBox9);
			this.tabPageExpertSettings.Controls.Add(this.textOracleSqlPlusPath);
			this.tabPageExpertSettings.Controls.Add(this.textFormMaximumSize);
			this.tabPageExpertSettings.Controls.Add(this.textReverseQueryColumnTime);
			this.tabPageExpertSettings.Controls.Add(this.textMaxQueryColumnCount);
			this.tabPageExpertSettings.Controls.Add(this.textMaxInputParamHistoryCount);
			this.tabPageExpertSettings.Controls.Add(this.textMaxLogOnHistoryCount);
			this.tabPageExpertSettings.Controls.Add(this.textSelectColumns);
			this.tabPageExpertSettings.Controls.Add(this.textSelectSynonymName);
			this.tabPageExpertSettings.Controls.Add(this.textSelectTableName);
			this.tabPageExpertSettings.Controls.Add(this.label15);
			this.tabPageExpertSettings.Controls.Add(this.label10);
			this.tabPageExpertSettings.Controls.Add(this.label8);
			this.tabPageExpertSettings.Controls.Add(this.label3);
			this.tabPageExpertSettings.Controls.Add(this.label7);
			this.tabPageExpertSettings.Controls.Add(this.label6);
			this.tabPageExpertSettings.Controls.Add(this.label14);
			this.tabPageExpertSettings.Controls.Add(this.label5);
			this.tabPageExpertSettings.Controls.Add(this.label4);
			this.tabPageExpertSettings.Controls.Add(this.label12);
			this.tabPageExpertSettings.Controls.Add(this.label11);
			this.tabPageExpertSettings.Location = new System.Drawing.Point(4, 22);
			this.tabPageExpertSettings.Name = "tabPageExpertSettings";
			this.tabPageExpertSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageExpertSettings.Size = new System.Drawing.Size(344, 230);
			this.tabPageExpertSettings.TabIndex = 2;
			this.tabPageExpertSettings.Text = "拡張設定";
			this.tabPageExpertSettings.UseVisualStyleBackColor = true;
			// 
			// groupBox9
			// 
			this.groupBox9.Location = new System.Drawing.Point(176, 3);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(2, 217);
			this.groupBox9.TabIndex = 16;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "groupBox9";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(6, 167);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(99, 12);
			this.label15.TabIndex = 12;
			this.label15.Text = "OracleSqlPlusPath";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 146);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 12);
			this.label10.TabIndex = 10;
			this.label10.Text = "FormMaximumSize";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(190, 93);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(81, 12);
			this.label14.TabIndex = 23;
			this.label14.Text = "SelectColumns";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(190, 49);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(94, 12);
			this.label11.TabIndex = 19;
			this.label11.Text = "SelectTableName";
			// 
			// OptionDlg
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(371, 306);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "オプション";
			this.Load += new System.EventHandler(this.OptionDlg_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabSettings.ResumeLayout(false);
			this.tabSettings.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabQueryOutput.ResumeLayout(false);
			this.tabQueryOutput.PerformLayout();
			this.tabPageExpertSettings.ResumeLayout(false);
			this.tabPageExpertSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox checkReloadLastColumnsOnStartup;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		public System.Windows.Forms.CheckBox checkSelectColumnByDragDrop;
		private System.Windows.Forms.CheckBox checkSaveQueryOutputFile;
		private System.Windows.Forms.TextBox textQueryOutputFileName;
		private System.Windows.Forms.Button buttonSelectQueryOutputFile;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioExcelPasteNone;
		private System.Windows.Forms.RadioButton radioExcelPasteActBookNewSheet;
		private System.Windows.Forms.RadioButton radioExcelPasteActBookActSheet;
		private System.Windows.Forms.RadioButton radioExcelPasteNewBookActSheet;
		private System.Windows.Forms.CheckBox checkPasteColumnComments;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox checkShowSynonymOwner;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabSettings;
		private System.Windows.Forms.TabPage tabQueryOutput;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioAppendAllColumns;
		private System.Windows.Forms.RadioButton radioClearSelectedColumns;
		private System.Windows.Forms.RadioButton radioShowColumns;
		private System.Windows.Forms.CheckBox checkClearQueryColumnWhenOraLogOn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboSqlDateFormat;
		private System.Windows.Forms.ComboBox comboOraMiddleware;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkEditableColumnName;
		private System.Windows.Forms.RadioButton radioExcelPasteShenBookNewSheet;
		private System.Windows.Forms.CheckBox checkMultiInstanceEnabled;
		private System.Windows.Forms.TabPage tabPageExpertSettings;
		private System.Windows.Forms.TextBox textFormMaximumSize;
		private System.Windows.Forms.TextBox textReverseQueryColumnTime;
		private System.Windows.Forms.TextBox textMaxQueryColumnCount;
		private System.Windows.Forms.TextBox textMaxLogOnHistoryCount;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.TextBox textSelectTableName;
		private System.Windows.Forms.TextBox textSelectSynonymName;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textSelectColumns;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.CheckBox checkShowParamInputDlg;
		private System.Windows.Forms.TextBox textOracleSqlPlusPath;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox checkPutDiffOwnerToTable;
		private System.Windows.Forms.CheckBox checkResumeAppendLogOnHis;
		private System.Windows.Forms.CheckBox checkAutoChangeLogOn;
		private System.Windows.Forms.CheckBox checkSelectableClearColumnLogOn;
		private System.Windows.Forms.CheckBox checkIntelliTableJoinMenu;
		private System.Windows.Forms.CheckBox checkExpertMode;
		private System.Windows.Forms.CheckBox checkWriteAccessLog;
		private System.Windows.Forms.CheckBox checkLogOnPwdToolTip;
		private System.Windows.Forms.TextBox textMaxInputParamHistoryCount;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkEnableExcelPasteNone;
		private System.Windows.Forms.TextBox textColumnListBackColorName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkShowQueryRecordCount;
		private System.Windows.Forms.TextBox textQueryColumnColorNames;
		private System.Windows.Forms.Label label5;

	}
}