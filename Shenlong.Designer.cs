namespace Shenlong
{
	partial class Shenlong
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
			noneCursor.Dispose();
			moveCursor.Dispose();
			copyCursor.Dispose();
			linkCursor.Dispose();

			if ( listBoxFontForWin2000 != null )
			{
				listBoxFontForWin2000.Dispose();
			}

			columnListBackColor.Dispose();

			queryColumnFont.Dispose();

			/*if ( queryColumnPens != null )
			{
				foreach ( System.Drawing.Pen queryColumnPen in queryColumnPens )
				{
					queryColumnPen.Dispose();
				}
			}*/

			if ( queryColumnBrushes != null )
			{
				foreach ( System.Drawing.Brush queryColumnBrush in queryColumnBrushes )
				{
					queryColumnBrush.Dispose();
				}
			}

			contextTableJoinFont.Dispose();

			if ( timerReadCommonSettings != null )
			{
				timerReadCommonSettings.Dispose();
				timerReadCommonSettings = null;
			}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shenlong));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusOraConn = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusFileName = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusColumnCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusVersion = new System.Windows.Forms.ToolStripStatusLabel();
			this.contextMenuTableList = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuSortTable = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuSortTableName = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuSortTableComment = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuRefreshTableList = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuColumnList = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuShowIndex = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuIndex = new System.Windows.Forms.ToolStripMenuItem();
			this.tabTableJoin = new System.Windows.Forms.TabPage();
			this.lvTableJoin = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuTableJoin = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuReleaseJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuJoinWay = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuInnerJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuLeftJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRightJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuFullOuterJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuSwapColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.tabQueryColumn = new System.Windows.Forms.TabPage();
			this.checkStretchColumnWidth = new System.Windows.Forms.CheckBox();
			this.checkShowField = new System.Windows.Forms.CheckBox();
			this.comboGroupFunc = new System.Windows.Forms.ComboBox();
			this.comboRightColOp = new System.Windows.Forms.ComboBox();
			this.comboExpression = new System.Windows.Forms.ComboBox();
			this.textValue = new System.Windows.Forms.TextBox();
			this.contextMenuQueryColumn = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuPasteHere = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuTableJoin = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuColumnProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSQL = new System.Windows.Forms.TabPage();
			this.textSQL = new System.Windows.Forms.TextBox();
			this.imageListTabPage = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripNew = new System.Windows.Forms.ToolStripButton();
			this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLogOn = new System.Windows.Forms.ToolStripButton();
			this.toolStripCustomTableSelect = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripToExcel = new System.Windows.Forms.ToolStripButton();
			this.toolStripShowParamInputDlg = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripOption = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSelectColumnDD = new System.Windows.Forms.ToolStripButton();
			this.toolStripEnableSameColumnAppend = new System.Windows.Forms.ToolStripButton();
			this.toolStripRemoveEndColumn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripContents = new System.Windows.Forms.ToolStripButton();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.ToolStripMenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorRecentFileName = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuRecentFileName1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName6 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName7 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuRecentFileName8 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuBuildQueryColumnSQL = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuCutQueryColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuCopyQueryColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuPasteQueryColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuFileProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuTool = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuLogOn = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuToExcel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuOption = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuContents = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolTipQueryColumn = new System.Windows.Forms.ToolTip(this.components);
			this.imageCheckBox = new System.Windows.Forms.ImageList(this.components);
			this.timerReverseQueryColumn = new System.Windows.Forms.Timer(this.components);
			this.labelHorizon = new System.Windows.Forms.Label();
			this.lveQueryColumn = new ListViewEx.ListViewEx();
			this.splitContainerTable = new MySplitContainer.MySplitContainer(this.components);
			this.textTableName = new System.Windows.Forms.TextBox();
			this.listBoxTableList = new System.Windows.Forms.ListBox();
			this.listBoxColumnList = new System.Windows.Forms.ListBox();
			this.splitContainer1 = new MySplitContainer.MySplitContainer(this.components);
			this.pictureBoxClearTableFilter = new System.Windows.Forms.PictureBox();
			this.textTableFilter = new System.Windows.Forms.TextBox();
			this.labelTableList = new System.Windows.Forms.Label();
			this.pictureBoxClearColumnFilter = new System.Windows.Forms.PictureBox();
			this.textColumnFilter = new System.Windows.Forms.TextBox();
			this.labelColumnList = new System.Windows.Forms.Label();
			this.statusStrip.SuspendLayout();
			this.contextMenuTableList.SuspendLayout();
			this.contextMenuColumnList.SuspendLayout();
			this.tabTableJoin.SuspendLayout();
			this.contextMenuTableJoin.SuspendLayout();
			this.tabQueryColumn.SuspendLayout();
			this.contextMenuQueryColumn.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabSQL.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.splitContainerTable.Panel1.SuspendLayout();
			this.splitContainerTable.Panel2.SuspendLayout();
			this.splitContainerTable.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearTableFilter)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearColumnFilter)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.BackColor = System.Drawing.SystemColors.ControlLight;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusOraConn,
            this.toolStripStatusFileName,
            this.toolStripStatusColumnCount,
            this.toolStripStatusVersion});
			this.statusStrip.Location = new System.Drawing.Point(0, 435);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip.ShowItemToolTips = true;
			this.statusStrip.Size = new System.Drawing.Size(604, 27);
			this.statusStrip.TabIndex = 6;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusOraConn
			// 
			this.toolStripStatusOraConn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.toolStripStatusOraConn.Name = "toolStripStatusOraConn";
			this.toolStripStatusOraConn.Size = new System.Drawing.Size(60, 22);
			this.toolStripStatusOraConn.Text = "@未接続";
			// 
			// toolStripStatusFileName
			// 
			this.toolStripStatusFileName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.toolStripStatusFileName.Name = "toolStripStatusFileName";
			this.toolStripStatusFileName.Size = new System.Drawing.Size(22, 22);
			this.toolStripStatusFileName.Text = "--";
			// 
			// toolStripStatusColumnCount
			// 
			this.toolStripStatusColumnCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.toolStripStatusColumnCount.Name = "toolStripStatusColumnCount";
			this.toolStripStatusColumnCount.Size = new System.Drawing.Size(19, 22);
			this.toolStripStatusColumnCount.Text = "0";
			// 
			// toolStripStatusVersion
			// 
			this.toolStripStatusVersion.Name = "toolStripStatusVersion";
			this.toolStripStatusVersion.Size = new System.Drawing.Size(486, 22);
			this.toolStripStatusVersion.Spring = true;
			this.toolStripStatusVersion.Text = "Version";
			this.toolStripStatusVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// contextMenuTableList
			// 
			this.contextMenuTableList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSortTable,
            this.toolStripSeparator12,
            this.toolStripMenuRefreshTableList});
			this.contextMenuTableList.Name = "contextMenuTableList";
			this.contextMenuTableList.Size = new System.Drawing.Size(173, 54);
			this.contextMenuTableList.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTableList_Opening);
			// 
			// toolStripMenuSortTable
			// 
			this.toolStripMenuSortTable.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSortTableName,
            this.toolStripMenuSortTableComment});
			this.toolStripMenuSortTable.Image = global::Shenlong.Properties.Resources.sort;
			this.toolStripMenuSortTable.Name = "toolStripMenuSortTable";
			this.toolStripMenuSortTable.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuSortTable.Text = "並べ替え";
			// 
			// toolStripMenuSortTableName
			// 
			this.toolStripMenuSortTableName.Name = "toolStripMenuSortTableName";
			this.toolStripMenuSortTableName.Size = new System.Drawing.Size(136, 22);
			this.toolStripMenuSortTableName.Text = "テーブル名";
			this.toolStripMenuSortTableName.Click += new System.EventHandler(this.ToolStripMenuSortTableName_Click);
			// 
			// toolStripMenuSortTableComment
			// 
			this.toolStripMenuSortTableComment.Name = "toolStripMenuSortTableComment";
			this.toolStripMenuSortTableComment.Size = new System.Drawing.Size(136, 22);
			this.toolStripMenuSortTableComment.Text = "コメント";
			this.toolStripMenuSortTableComment.Click += new System.EventHandler(this.ToolStripMenuSortTableName_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(169, 6);
			// 
			// toolStripMenuRefreshTableList
			// 
			this.toolStripMenuRefreshTableList.Name = "toolStripMenuRefreshTableList";
			this.toolStripMenuRefreshTableList.Size = new System.Drawing.Size(172, 22);
			this.toolStripMenuRefreshTableList.Text = "最新の情報に更新";
			this.toolStripMenuRefreshTableList.Click += new System.EventHandler(this.toolStripMenuRefreshTableList_Click);
			// 
			// contextMenuColumnList
			// 
			this.contextMenuColumnList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSelectAll,
            this.toolStripSeparator7,
            this.toolStripMenuShowIndex});
			this.contextMenuColumnList.Name = "contextMenuColumnList";
			this.contextMenuColumnList.ShowImageMargin = false;
			this.contextMenuColumnList.Size = new System.Drawing.Size(148, 54);
			this.contextMenuColumnList.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuColumnList_Opening);
			// 
			// toolStripMenuSelectAll
			// 
			this.toolStripMenuSelectAll.Name = "toolStripMenuSelectAll";
			this.toolStripMenuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.toolStripMenuSelectAll.Size = new System.Drawing.Size(147, 22);
			this.toolStripMenuSelectAll.Text = "全て選択";
			this.toolStripMenuSelectAll.Click += new System.EventHandler(this.toolStripMenuSelectAll_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(144, 6);
			// 
			// toolStripMenuShowIndex
			// 
			this.toolStripMenuShowIndex.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuIndex});
			this.toolStripMenuShowIndex.Name = "toolStripMenuShowIndex";
			this.toolStripMenuShowIndex.Size = new System.Drawing.Size(147, 22);
			this.toolStripMenuShowIndex.Text = "インデックス一覧";
			this.toolStripMenuShowIndex.DropDownOpening += new System.EventHandler(this.toolStripMenuShowIndex_DropDownOpening);
			// 
			// toolStripMenuIndex
			// 
			this.toolStripMenuIndex.Name = "toolStripMenuIndex";
			this.toolStripMenuIndex.Size = new System.Drawing.Size(148, 22);
			this.toolStripMenuIndex.Text = "インデックス";
			// 
			// tabTableJoin
			// 
			this.tabTableJoin.Controls.Add(this.lvTableJoin);
			this.tabTableJoin.ImageIndex = 1;
			this.tabTableJoin.Location = new System.Drawing.Point(4, 23);
			this.tabTableJoin.Name = "tabTableJoin";
			this.tabTableJoin.Padding = new System.Windows.Forms.Padding(3);
			this.tabTableJoin.Size = new System.Drawing.Size(580, 197);
			this.tabTableJoin.TabIndex = 1;
			this.tabTableJoin.Text = "テーブル結合";
			this.tabTableJoin.UseVisualStyleBackColor = true;
			// 
			// lvTableJoin
			// 
			this.lvTableJoin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvTableJoin.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.lvTableJoin.ContextMenuStrip = this.contextMenuTableJoin;
			this.lvTableJoin.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lvTableJoin.FullRowSelect = true;
			this.lvTableJoin.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvTableJoin.Location = new System.Drawing.Point(8, 9);
			this.lvTableJoin.Name = "lvTableJoin";
			this.lvTableJoin.Size = new System.Drawing.Size(560, 181);
			this.lvTableJoin.TabIndex = 0;
			this.lvTableJoin.UseCompatibleStateImageBehavior = false;
			this.lvTableJoin.View = System.Windows.Forms.View.Details;
			this.lvTableJoin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvTableJoin_KeyUp);
			this.lvTableJoin.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvTableJoin_MouseDoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "左のテーブル項目";
			this.columnHeader1.Width = 250;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "結合";
			this.columnHeader2.Width = 50;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "右のテーブル項目";
			this.columnHeader3.Width = 250;
			// 
			// contextMenuTableJoin
			// 
			this.contextMenuTableJoin.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuReleaseJoin,
            this.toolStripMenuJoinWay,
            this.toolStripMenuSwapColumn});
			this.contextMenuTableJoin.Name = "contextMenuTableJoin";
			this.contextMenuTableJoin.Size = new System.Drawing.Size(191, 70);
			this.contextMenuTableJoin.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTableJoin_Opening);
			// 
			// toolStripMenuReleaseJoin
			// 
			this.toolStripMenuReleaseJoin.Image = global::Shenlong.Properties.Resources.remove;
			this.toolStripMenuReleaseJoin.Name = "toolStripMenuReleaseJoin";
			this.toolStripMenuReleaseJoin.Size = new System.Drawing.Size(190, 22);
			this.toolStripMenuReleaseJoin.Text = "結合解除(&D)";
			this.toolStripMenuReleaseJoin.Click += new System.EventHandler(this.toolStripMenuReleaseJoin_Click);
			// 
			// toolStripMenuJoinWay
			// 
			this.toolStripMenuJoinWay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuInnerJoin,
            this.toolStripMenuLeftJoin,
            this.toolStripMenuRightJoin,
            this.toolStripMenuFullOuterJoin});
			this.toolStripMenuJoinWay.Image = global::Shenlong.Properties.Resources.way;
			this.toolStripMenuJoinWay.Name = "toolStripMenuJoinWay";
			this.toolStripMenuJoinWay.Size = new System.Drawing.Size(190, 22);
			this.toolStripMenuJoinWay.Text = "結合方法";
			this.toolStripMenuJoinWay.DropDownOpening += new System.EventHandler(this.toolStripMenuJoinWay_DropDownOpening);
			// 
			// toolStripMenuInnerJoin
			// 
			this.toolStripMenuInnerJoin.Name = "toolStripMenuInnerJoin";
			this.toolStripMenuInnerJoin.Size = new System.Drawing.Size(158, 22);
			this.toolStripMenuInnerJoin.Text = "等価結合 (＝)";
			this.toolStripMenuInnerJoin.Click += new System.EventHandler(this.toolStripMenuChangeJoinWay_Click);
			// 
			// toolStripMenuLeftJoin
			// 
			this.toolStripMenuLeftJoin.Name = "toolStripMenuLeftJoin";
			this.toolStripMenuLeftJoin.Size = new System.Drawing.Size(158, 22);
			this.toolStripMenuLeftJoin.Text = "左結合 (≧)";
			this.toolStripMenuLeftJoin.Click += new System.EventHandler(this.toolStripMenuChangeJoinWay_Click);
			// 
			// toolStripMenuRightJoin
			// 
			this.toolStripMenuRightJoin.Name = "toolStripMenuRightJoin";
			this.toolStripMenuRightJoin.Size = new System.Drawing.Size(158, 22);
			this.toolStripMenuRightJoin.Text = "右結合 (≦)";
			this.toolStripMenuRightJoin.Click += new System.EventHandler(this.toolStripMenuChangeJoinWay_Click);
			// 
			// toolStripMenuFullOuterJoin
			// 
			this.toolStripMenuFullOuterJoin.Name = "toolStripMenuFullOuterJoin";
			this.toolStripMenuFullOuterJoin.Size = new System.Drawing.Size(158, 22);
			this.toolStripMenuFullOuterJoin.Text = "左右結合 (≧≦)";
			this.toolStripMenuFullOuterJoin.Click += new System.EventHandler(this.toolStripMenuChangeJoinWay_Click);
			// 
			// toolStripMenuSwapColumn
			// 
			this.toolStripMenuSwapColumn.Name = "toolStripMenuSwapColumn";
			this.toolStripMenuSwapColumn.Size = new System.Drawing.Size(190, 22);
			this.toolStripMenuSwapColumn.Text = "左右の項目を入替(&C)";
			this.toolStripMenuSwapColumn.Click += new System.EventHandler(this.toolStripMenuSwapColumn_Click);
			// 
			// tabQueryColumn
			// 
			this.tabQueryColumn.Controls.Add(this.checkStretchColumnWidth);
			this.tabQueryColumn.Controls.Add(this.checkShowField);
			this.tabQueryColumn.Controls.Add(this.comboGroupFunc);
			this.tabQueryColumn.Controls.Add(this.comboRightColOp);
			this.tabQueryColumn.Controls.Add(this.comboExpression);
			this.tabQueryColumn.Controls.Add(this.textValue);
			this.tabQueryColumn.Controls.Add(this.lveQueryColumn);
			this.tabQueryColumn.Controls.Add(this.label8);
			this.tabQueryColumn.Controls.Add(this.label7);
			this.tabQueryColumn.Controls.Add(this.label6);
			this.tabQueryColumn.Controls.Add(this.label5);
			this.tabQueryColumn.Controls.Add(this.label4);
			this.tabQueryColumn.Controls.Add(this.label3);
			this.tabQueryColumn.Controls.Add(this.label2);
			this.tabQueryColumn.Controls.Add(this.label1);
			this.tabQueryColumn.ImageIndex = 0;
			this.tabQueryColumn.Location = new System.Drawing.Point(4, 23);
			this.tabQueryColumn.Name = "tabQueryColumn";
			this.tabQueryColumn.Padding = new System.Windows.Forms.Padding(3);
			this.tabQueryColumn.Size = new System.Drawing.Size(580, 197);
			this.tabQueryColumn.TabIndex = 0;
			this.tabQueryColumn.Text = "クエリー項目";
			this.tabQueryColumn.UseVisualStyleBackColor = true;
			// 
			// checkStretchColumnWidth
			// 
			this.checkStretchColumnWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkStretchColumnWidth.AutoSize = true;
			this.checkStretchColumnWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkStretchColumnWidth.Location = new System.Drawing.Point(569, 0);
			this.checkStretchColumnWidth.Name = "checkStretchColumnWidth";
			this.checkStretchColumnWidth.Size = new System.Drawing.Size(12, 11);
			this.checkStretchColumnWidth.TabIndex = 16;
			this.checkStretchColumnWidth.TabStop = false;
			this.toolTipQueryColumn.SetToolTip(this.checkStretchColumnWidth, "項目の横幅を伸縮する");
			this.checkStretchColumnWidth.UseVisualStyleBackColor = true;
			this.checkStretchColumnWidth.CheckedChanged += new System.EventHandler(this.checkStretchColumnWidth_CheckedChanged);
			// 
			// checkShowField
			// 
			this.checkShowField.AutoSize = true;
			this.checkShowField.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.checkShowField.Location = new System.Drawing.Point(114, 17);
			this.checkShowField.Name = "checkShowField";
			this.checkShowField.Size = new System.Drawing.Size(15, 14);
			this.checkShowField.TabIndex = 14;
			this.checkShowField.UseVisualStyleBackColor = true;
			this.checkShowField.Visible = false;
			// 
			// comboGroupFunc
			// 
			this.comboGroupFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroupFunc.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboGroupFunc.FormattingEnabled = true;
			this.comboGroupFunc.Items.AddRange(new object[] {
            "",
            "SUM",
            "AVG",
            "MIN",
            "MAX",
            "COUNT"});
			this.comboGroupFunc.Location = new System.Drawing.Point(114, 127);
			this.comboGroupFunc.Name = "comboGroupFunc";
			this.comboGroupFunc.Size = new System.Drawing.Size(100, 23);
			this.comboGroupFunc.TabIndex = 13;
			this.comboGroupFunc.Visible = false;
			// 
			// comboRightColOp
			// 
			this.comboRightColOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRightColOp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboRightColOp.FormattingEnabled = true;
			this.comboRightColOp.Items.AddRange(new object[] {
            "",
            "AND",
            "OR"});
			this.comboRightColOp.Location = new System.Drawing.Point(114, 97);
			this.comboRightColOp.Name = "comboRightColOp";
			this.comboRightColOp.Size = new System.Drawing.Size(100, 23);
			this.comboRightColOp.TabIndex = 12;
			this.comboRightColOp.Visible = false;
			// 
			// comboExpression
			// 
			this.comboExpression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboExpression.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.comboExpression.FormattingEnabled = true;
			this.comboExpression.Items.AddRange(new object[] {
            "",
            "=",
            "NOT =",
            ">=",
            "<=",
            ">",
            "<",
            "BETWEEN",
            "NOT BETWEEN",
            "IN",
            "NOT IN",
            "LIKE",
            "NOT LIKE",
            "IS NULL",
            "IS NOT NULL"});
			this.comboExpression.Location = new System.Drawing.Point(114, 67);
			this.comboExpression.Name = "comboExpression";
			this.comboExpression.Size = new System.Drawing.Size(100, 23);
			this.comboExpression.TabIndex = 11;
			this.comboExpression.Visible = false;
			// 
			// textValue
			// 
			this.textValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textValue.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textValue.Location = new System.Drawing.Point(114, 38);
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(100, 22);
			this.textValue.TabIndex = 10;
			this.textValue.Visible = false;
			// 
			// contextMenuQueryColumn
			// 
			this.contextMenuQueryColumn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuDelete,
            this.toolStripMenuPasteHere,
            this.toolStripMenuTableJoin,
            this.toolStripSeparator10,
            this.toolStripMenuColumnProperty});
			this.contextMenuQueryColumn.Name = "contextMenuQueryColumn";
			this.contextMenuQueryColumn.Size = new System.Drawing.Size(178, 98);
			this.contextMenuQueryColumn.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuQueryColumn_Closing);
			this.contextMenuQueryColumn.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuQueryColumn_Opening);
			// 
			// toolStripMenuDelete
			// 
			this.toolStripMenuDelete.Image = global::Shenlong.Properties.Resources.remove;
			this.toolStripMenuDelete.Name = "toolStripMenuDelete";
			this.toolStripMenuDelete.Size = new System.Drawing.Size(177, 22);
			this.toolStripMenuDelete.Text = "項目削除(&D)";
			this.toolStripMenuDelete.Click += new System.EventHandler(this.toolStripMenuDelete_Click);
			// 
			// toolStripMenuPasteHere
			// 
			this.toolStripMenuPasteHere.Name = "toolStripMenuPasteHere";
			this.toolStripMenuPasteHere.Size = new System.Drawing.Size(177, 22);
			this.toolStripMenuPasteHere.Text = "ここに貼り付け(&P)";
			this.toolStripMenuPasteHere.Click += new System.EventHandler(this.toolStripMenuPasteHere_Click);
			// 
			// toolStripMenuTableJoin
			// 
			this.toolStripMenuTableJoin.Image = global::Shenlong.Properties.Resources.tableJoin;
			this.toolStripMenuTableJoin.Name = "toolStripMenuTableJoin";
			this.toolStripMenuTableJoin.Size = new System.Drawing.Size(177, 22);
			this.toolStripMenuTableJoin.Text = "テーブル結合";
			this.toolStripMenuTableJoin.DropDownOpening += new System.EventHandler(this.toolStripMenuTableJoin_DropDownOpening);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(174, 6);
			// 
			// toolStripMenuColumnProperty
			// 
			this.toolStripMenuColumnProperty.Image = global::Shenlong.Properties.Resources.qcProperty;
			this.toolStripMenuColumnProperty.Name = "toolStripMenuColumnProperty";
			this.toolStripMenuColumnProperty.Size = new System.Drawing.Size(177, 22);
			this.toolStripMenuColumnProperty.Text = "プロパティ(&R)";
			this.toolStripMenuColumnProperty.Click += new System.EventHandler(this.toolStripMenuColumnProperty_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(4, 153);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(62, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "集計関数:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(19, 136);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(47, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "並び順:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(4, 119);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(62, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "右列連結:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(34, 102);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "値２:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(34, 85);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "値１:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "条件式:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "表示:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "項目名:";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabQueryColumn);
			this.tabControl.Controls.Add(this.tabTableJoin);
			this.tabControl.Controls.Add(this.tabSQL);
			this.tabControl.ImageList = this.imageListTabPage;
			this.tabControl.Location = new System.Drawing.Point(8, 208);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(588, 224);
			this.tabControl.TabIndex = 5;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			this.tabControl.DoubleClick += new System.EventHandler(this.tabControl_DoubleClick);
			// 
			// tabSQL
			// 
			this.tabSQL.Controls.Add(this.textSQL);
			this.tabSQL.ImageIndex = 2;
			this.tabSQL.Location = new System.Drawing.Point(4, 23);
			this.tabSQL.Name = "tabSQL";
			this.tabSQL.Padding = new System.Windows.Forms.Padding(3);
			this.tabSQL.Size = new System.Drawing.Size(580, 197);
			this.tabSQL.TabIndex = 2;
			this.tabSQL.Text = "SQL";
			this.tabSQL.UseVisualStyleBackColor = true;
			// 
			// textSQL
			// 
			this.textSQL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textSQL.Location = new System.Drawing.Point(3, 3);
			this.textSQL.Multiline = true;
			this.textSQL.Name = "textSQL";
			this.textSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textSQL.Size = new System.Drawing.Size(574, 191);
			this.textSQL.TabIndex = 0;
			this.textSQL.TextChanged += new System.EventHandler(this.textSQL_TextChanged);
			// 
			// imageListTabPage
			// 
			this.imageListTabPage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabPage.ImageStream")));
			this.imageListTabPage.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTabPage.Images.SetKeyName(0, "queryColumn.ico");
			this.imageListTabPage.Images.SetKeyName(1, "tableJoin.ico");
			this.imageListTabPage.Images.SetKeyName(2, "SQLPLUSICON.ico");
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNew,
            this.toolStripOpen,
            this.toolStripSave,
            this.toolStripSeparator2,
            this.toolStripLogOn,
            this.toolStripCustomTableSelect,
            this.toolStripSeparator11,
            this.toolStripToExcel,
            this.toolStripShowParamInputDlg,
            this.toolStripSeparator5,
            this.toolStripOption,
            this.toolStripSeparator4,
            this.toolStripSelectColumnDD,
            this.toolStripEnableSameColumnAppend,
            this.toolStripRemoveEndColumn,
            this.toolStripSeparator3,
            this.toolStripContents});
			this.toolStrip1.Location = new System.Drawing.Point(0, 26);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(604, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripNew
			// 
			this.toolStripNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripNew.Image")));
			this.toolStripNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripNew.Name = "toolStripNew";
			this.toolStripNew.Size = new System.Drawing.Size(23, 22);
			this.toolStripNew.Text = "新規作成";
			this.toolStripNew.ToolTipText = "新規作成";
			this.toolStripNew.Click += new System.EventHandler(this.toolStripMenuNew_Click);
			// 
			// toolStripOpen
			// 
			this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOpen.Image")));
			this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripOpen.Name = "toolStripOpen";
			this.toolStripOpen.Size = new System.Drawing.Size(23, 22);
			this.toolStripOpen.Text = "開く";
			this.toolStripOpen.ToolTipText = "開く";
			this.toolStripOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
			// 
			// toolStripSave
			// 
			this.toolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSave.Image")));
			this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSave.Name = "toolStripSave";
			this.toolStripSave.Size = new System.Drawing.Size(23, 22);
			this.toolStripSave.Text = "上書き保存";
			this.toolStripSave.ToolTipText = "上書き保存";
			this.toolStripSave.Click += new System.EventHandler(this.toolStripMenuSave_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLogOn
			// 
			this.toolStripLogOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLogOn.Image = global::Shenlong.Properties.Resources.netca_01;
			this.toolStripLogOn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripLogOn.Name = "toolStripLogOn";
			this.toolStripLogOn.Size = new System.Drawing.Size(23, 22);
			this.toolStripLogOn.Text = "ログオン";
			this.toolStripLogOn.Click += new System.EventHandler(this.toolStripMenuLogOn_Click);
			// 
			// toolStripCustomTableSelect
			// 
			this.toolStripCustomTableSelect.CheckOnClick = true;
			this.toolStripCustomTableSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripCustomTableSelect.Image = global::Shenlong.Properties.Resources.customTableSelect;
			this.toolStripCustomTableSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripCustomTableSelect.Name = "toolStripCustomTableSelect";
			this.toolStripCustomTableSelect.Size = new System.Drawing.Size(23, 22);
			this.toolStripCustomTableSelect.Text = "Customized Table Select";
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripToExcel
			// 
			this.toolStripToExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripToExcel.Image = global::Shenlong.Properties.Resources.excel;
			this.toolStripToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripToExcel.Name = "toolStripToExcel";
			this.toolStripToExcel.Size = new System.Drawing.Size(23, 22);
			this.toolStripToExcel.Text = "Excel へ貼付";
			this.toolStripToExcel.Click += new System.EventHandler(this.toolStripMenuToExcel_Click);
			// 
			// toolStripShowParamInputDlg
			// 
			this.toolStripShowParamInputDlg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripShowParamInputDlg.Image = global::Shenlong.Properties.Resources.paramInput;
			this.toolStripShowParamInputDlg.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripShowParamInputDlg.Name = "toolStripShowParamInputDlg";
			this.toolStripShowParamInputDlg.Size = new System.Drawing.Size(23, 22);
			this.toolStripShowParamInputDlg.Text = "抽出条件を入力";
			this.toolStripShowParamInputDlg.Click += new System.EventHandler(this.toolStripShowParamInputDlg_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripOption
			// 
			this.toolStripOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripOption.Image = global::Shenlong.Properties.Resources.option;
			this.toolStripOption.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripOption.Name = "toolStripOption";
			this.toolStripOption.Size = new System.Drawing.Size(23, 22);
			this.toolStripOption.Text = "オプション";
			this.toolStripOption.Click += new System.EventHandler(this.toolStripMenuOption_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSelectColumnDD
			// 
			this.toolStripSelectColumnDD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSelectColumnDD.Image = global::Shenlong.Properties.Resources.selectColumnDD;
			this.toolStripSelectColumnDD.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSelectColumnDD.Name = "toolStripSelectColumnDD";
			this.toolStripSelectColumnDD.Size = new System.Drawing.Size(23, 22);
			this.toolStripSelectColumnDD.Text = "ドラッグ＆ドロップで項目を選択";
			this.toolStripSelectColumnDD.Click += new System.EventHandler(this.toolStripSelectColumnDD_Click);
			// 
			// toolStripEnableSameColumnAppend
			// 
			this.toolStripEnableSameColumnAppend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripEnableSameColumnAppend.Image = global::Shenlong.Properties.Resources.noCheckSameColumn;
			this.toolStripEnableSameColumnAppend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripEnableSameColumnAppend.Name = "toolStripEnableSameColumnAppend";
			this.toolStripEnableSameColumnAppend.Size = new System.Drawing.Size(23, 22);
			this.toolStripEnableSameColumnAppend.Text = "重複項目の追加を許可";
			this.toolStripEnableSameColumnAppend.Click += new System.EventHandler(this.toolStripEnableSameColumnAppend_Click);
			// 
			// toolStripRemoveEndColumn
			// 
			this.toolStripRemoveEndColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripRemoveEndColumn.Image = global::Shenlong.Properties.Resources.removeEndColumn;
			this.toolStripRemoveEndColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripRemoveEndColumn.Name = "toolStripRemoveEndColumn";
			this.toolStripRemoveEndColumn.Size = new System.Drawing.Size(23, 22);
			this.toolStripRemoveEndColumn.Text = "右端の項目を削除";
			this.toolStripRemoveEndColumn.Click += new System.EventHandler(this.toolStripRemoveEndColumn_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripContents
			// 
			this.toolStripContents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripContents.Image = global::Shenlong.Properties.Resources.helpContents;
			this.toolStripContents.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripContents.Name = "toolStripContents";
			this.toolStripContents.Size = new System.Drawing.Size(23, 22);
			this.toolStripContents.Text = "目次";
			this.toolStripContents.Click += new System.EventHandler(this.toolStripMenuContents_Click);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuFile,
            this.ToolStripMenuEdit,
            this.ToolStripMenuTool,
            this.ToolStripMenuHelp});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(604, 26);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip";
			this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
			// 
			// ToolStripMenuFile
			// 
			this.ToolStripMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuNew,
            this.toolStripMenuOpen,
            this.toolStripSeparator,
            this.toolStripMenuSave,
            this.toolStripMenuSaveAs,
            this.toolStripSeparatorRecentFileName,
            this.toolStripMenuRecentFileName1,
            this.toolStripMenuRecentFileName2,
            this.toolStripMenuRecentFileName3,
            this.toolStripMenuRecentFileName4,
            this.toolStripMenuRecentFileName5,
            this.toolStripMenuRecentFileName6,
            this.toolStripMenuRecentFileName7,
            this.toolStripMenuRecentFileName8,
            this.toolStripSeparator1,
            this.toolStripMenuClose});
			this.ToolStripMenuFile.Name = "ToolStripMenuFile";
			this.ToolStripMenuFile.Size = new System.Drawing.Size(85, 22);
			this.ToolStripMenuFile.Text = "ファイル(&F)";
			// 
			// toolStripMenuNew
			// 
			this.toolStripMenuNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuNew.Image")));
			this.toolStripMenuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuNew.Name = "toolStripMenuNew";
			this.toolStripMenuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.toolStripMenuNew.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuNew.Text = "新規作成(&N)";
			this.toolStripMenuNew.Click += new System.EventHandler(this.toolStripMenuNew_Click);
			// 
			// toolStripMenuOpen
			// 
			this.toolStripMenuOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuOpen.Image")));
			this.toolStripMenuOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuOpen.Name = "toolStripMenuOpen";
			this.toolStripMenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.toolStripMenuOpen.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuOpen.Text = "開く(&O)...";
			this.toolStripMenuOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(200, 6);
			// 
			// toolStripMenuSave
			// 
			this.toolStripMenuSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuSave.Image")));
			this.toolStripMenuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuSave.Name = "toolStripMenuSave";
			this.toolStripMenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.toolStripMenuSave.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuSave.Text = "上書き保存(&S)";
			this.toolStripMenuSave.Click += new System.EventHandler(this.toolStripMenuSave_Click);
			// 
			// toolStripMenuSaveAs
			// 
			this.toolStripMenuSaveAs.Name = "toolStripMenuSaveAs";
			this.toolStripMenuSaveAs.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuSaveAs.Text = "名前を付けて保存(&A)...";
			this.toolStripMenuSaveAs.Click += new System.EventHandler(this.toolStripMenuSaveAs_Click);
			// 
			// toolStripSeparatorRecentFileName
			// 
			this.toolStripSeparatorRecentFileName.Name = "toolStripSeparatorRecentFileName";
			this.toolStripSeparatorRecentFileName.Size = new System.Drawing.Size(200, 6);
			// 
			// toolStripMenuRecentFileName1
			// 
			this.toolStripMenuRecentFileName1.Name = "toolStripMenuRecentFileName1";
			this.toolStripMenuRecentFileName1.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName1.Text = "最近使ったファイル名1";
			this.toolStripMenuRecentFileName1.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName2
			// 
			this.toolStripMenuRecentFileName2.Name = "toolStripMenuRecentFileName2";
			this.toolStripMenuRecentFileName2.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName2.Text = "最近使ったファイル名2";
			this.toolStripMenuRecentFileName2.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName3
			// 
			this.toolStripMenuRecentFileName3.Name = "toolStripMenuRecentFileName3";
			this.toolStripMenuRecentFileName3.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName3.Text = "最近使ったファイル名3";
			this.toolStripMenuRecentFileName3.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName4
			// 
			this.toolStripMenuRecentFileName4.Name = "toolStripMenuRecentFileName4";
			this.toolStripMenuRecentFileName4.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName4.Text = "最近使ったファイル名4";
			this.toolStripMenuRecentFileName4.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName5
			// 
			this.toolStripMenuRecentFileName5.Name = "toolStripMenuRecentFileName5";
			this.toolStripMenuRecentFileName5.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName5.Text = "最近使ったファイル名5";
			this.toolStripMenuRecentFileName5.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName6
			// 
			this.toolStripMenuRecentFileName6.Name = "toolStripMenuRecentFileName6";
			this.toolStripMenuRecentFileName6.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName6.Text = "最近使ったファイル名6";
			this.toolStripMenuRecentFileName6.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName7
			// 
			this.toolStripMenuRecentFileName7.Name = "toolStripMenuRecentFileName7";
			this.toolStripMenuRecentFileName7.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName7.Text = "最近使ったファイル名7";
			this.toolStripMenuRecentFileName7.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripMenuRecentFileName8
			// 
			this.toolStripMenuRecentFileName8.Name = "toolStripMenuRecentFileName8";
			this.toolStripMenuRecentFileName8.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuRecentFileName8.Text = "最近使ったファイル名8";
			this.toolStripMenuRecentFileName8.Click += new System.EventHandler(this.toolStripMenuRecentFileName_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
			// 
			// toolStripMenuClose
			// 
			this.toolStripMenuClose.Image = global::Shenlong.Properties.Resources.exit;
			this.toolStripMenuClose.Name = "toolStripMenuClose";
			this.toolStripMenuClose.Size = new System.Drawing.Size(203, 22);
			this.toolStripMenuClose.Text = "終了(&X)";
			this.toolStripMenuClose.Click += new System.EventHandler(this.toolStripMenuClose_Click);
			// 
			// ToolStripMenuEdit
			// 
			this.ToolStripMenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuBuildQueryColumnSQL,
            this.toolStripSeparator8,
            this.toolStripMenuCutQueryColumn,
            this.toolStripMenuCopyQueryColumn,
            this.toolStripMenuPasteQueryColumn,
            this.toolStripSeparator9,
            this.toolStripMenuFileProperty});
			this.ToolStripMenuEdit.Name = "ToolStripMenuEdit";
			this.ToolStripMenuEdit.Size = new System.Drawing.Size(61, 22);
			this.ToolStripMenuEdit.Text = "編集(&E)";
			this.ToolStripMenuEdit.DropDownOpening += new System.EventHandler(this.ToolStripMenuEdit_DropDownOpening);
			// 
			// toolStripMenuBuildQueryColumnSQL
			// 
			this.toolStripMenuBuildQueryColumnSQL.Image = global::Shenlong.Properties.Resources.SQLPLUSICON;
			this.toolStripMenuBuildQueryColumnSQL.Name = "toolStripMenuBuildQueryColumnSQL";
			this.toolStripMenuBuildQueryColumnSQL.Size = new System.Drawing.Size(238, 22);
			this.toolStripMenuBuildQueryColumnSQL.Text = "クエリー項目でSQLを構築(&S)";
			this.toolStripMenuBuildQueryColumnSQL.Click += new System.EventHandler(this.toolStripMenuBuildQueryColumnSQL_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(235, 6);
			// 
			// toolStripMenuCutQueryColumn
			// 
			this.toolStripMenuCutQueryColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuCutQueryColumn.Image")));
			this.toolStripMenuCutQueryColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuCutQueryColumn.Name = "toolStripMenuCutQueryColumn";
			this.toolStripMenuCutQueryColumn.Size = new System.Drawing.Size(238, 22);
			this.toolStripMenuCutQueryColumn.Text = "クエリー項目を切り取り(&T)...";
			this.toolStripMenuCutQueryColumn.Click += new System.EventHandler(this.toolStripMenuCutQueryColumn_Click);
			// 
			// toolStripMenuCopyQueryColumn
			// 
			this.toolStripMenuCopyQueryColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuCopyQueryColumn.Image")));
			this.toolStripMenuCopyQueryColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuCopyQueryColumn.Name = "toolStripMenuCopyQueryColumn";
			this.toolStripMenuCopyQueryColumn.Size = new System.Drawing.Size(238, 22);
			this.toolStripMenuCopyQueryColumn.Text = "クエリー項目をコピー(&C)...";
			this.toolStripMenuCopyQueryColumn.Click += new System.EventHandler(this.toolStripMenuCopyQueryColumn_Click);
			// 
			// toolStripMenuPasteQueryColumn
			// 
			this.toolStripMenuPasteQueryColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuPasteQueryColumn.Image")));
			this.toolStripMenuPasteQueryColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripMenuPasteQueryColumn.Name = "toolStripMenuPasteQueryColumn";
			this.toolStripMenuPasteQueryColumn.Size = new System.Drawing.Size(238, 22);
			this.toolStripMenuPasteQueryColumn.Text = "クエリー項目へ貼り付け(&P)";
			this.toolStripMenuPasteQueryColumn.Click += new System.EventHandler(this.toolStripMenuPasteQueryColumn_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(235, 6);
			// 
			// toolStripMenuFileProperty
			// 
			this.toolStripMenuFileProperty.Image = global::Shenlong.Properties.Resources.property;
			this.toolStripMenuFileProperty.Name = "toolStripMenuFileProperty";
			this.toolStripMenuFileProperty.Size = new System.Drawing.Size(238, 22);
			this.toolStripMenuFileProperty.Text = "ファイルのプロパティ(&R)...";
			this.toolStripMenuFileProperty.Click += new System.EventHandler(this.toolStripMenuFileProperty_Click);
			// 
			// ToolStripMenuTool
			// 
			this.ToolStripMenuTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuLogOn,
            this.toolStripMenuToExcel,
            this.toolStripSeparator6,
            this.toolStripMenuOption});
			this.ToolStripMenuTool.Name = "ToolStripMenuTool";
			this.ToolStripMenuTool.Size = new System.Drawing.Size(74, 22);
			this.ToolStripMenuTool.Text = "ツール(&T)";
			// 
			// toolStripMenuLogOn
			// 
			this.toolStripMenuLogOn.Image = global::Shenlong.Properties.Resources.netca_01;
			this.toolStripMenuLogOn.Name = "toolStripMenuLogOn";
			this.toolStripMenuLogOn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.toolStripMenuLogOn.Size = new System.Drawing.Size(199, 22);
			this.toolStripMenuLogOn.Text = "ログオン(&L)...";
			this.toolStripMenuLogOn.Click += new System.EventHandler(this.toolStripMenuLogOn_Click);
			// 
			// toolStripMenuToExcel
			// 
			this.toolStripMenuToExcel.Image = global::Shenlong.Properties.Resources.excel;
			this.toolStripMenuToExcel.Name = "toolStripMenuToExcel";
			this.toolStripMenuToExcel.ShortcutKeyDisplayString = "";
			this.toolStripMenuToExcel.Size = new System.Drawing.Size(199, 22);
			this.toolStripMenuToExcel.Text = "Excel へ貼り付け(&E)";
			this.toolStripMenuToExcel.Click += new System.EventHandler(this.toolStripMenuToExcel_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(196, 6);
			// 
			// toolStripMenuOption
			// 
			this.toolStripMenuOption.Image = global::Shenlong.Properties.Resources.option;
			this.toolStripMenuOption.Name = "toolStripMenuOption";
			this.toolStripMenuOption.Size = new System.Drawing.Size(199, 22);
			this.toolStripMenuOption.Text = "オプション(&O)...";
			this.toolStripMenuOption.Click += new System.EventHandler(this.toolStripMenuOption_Click);
			// 
			// ToolStripMenuHelp
			// 
			this.ToolStripMenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuContents,
            this.toolStripMenuAbout});
			this.ToolStripMenuHelp.Name = "ToolStripMenuHelp";
			this.ToolStripMenuHelp.Size = new System.Drawing.Size(75, 22);
			this.ToolStripMenuHelp.Text = "ヘルプ(&H)";
			// 
			// toolStripMenuContents
			// 
			this.toolStripMenuContents.Image = global::Shenlong.Properties.Resources.helpContents;
			this.toolStripMenuContents.Name = "toolStripMenuContents";
			this.toolStripMenuContents.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.toolStripMenuContents.Size = new System.Drawing.Size(190, 22);
			this.toolStripMenuContents.Text = "目次(&C)";
			this.toolStripMenuContents.Click += new System.EventHandler(this.toolStripMenuContents_Click);
			// 
			// toolStripMenuAbout
			// 
			this.toolStripMenuAbout.Name = "toolStripMenuAbout";
			this.toolStripMenuAbout.Size = new System.Drawing.Size(190, 22);
			this.toolStripMenuAbout.Text = "バージョン情報(&A)...";
			this.toolStripMenuAbout.Click += new System.EventHandler(this.toolStripMenuAbout_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "クエリー項目ファイル (*.xml)|*.xml";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "クエリー項目ファイル (*.xml)|*.xml|SQLファイル (*.sql)|*.sql";
			// 
			// imageCheckBox
			// 
			this.imageCheckBox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageCheckBox.ImageStream")));
			this.imageCheckBox.TransparentColor = System.Drawing.Color.Transparent;
			this.imageCheckBox.Images.SetKeyName(0, "checkOff.bmp");
			this.imageCheckBox.Images.SetKeyName(1, "checkOn.bmp");
			// 
			// timerReverseQueryColumn
			// 
			this.timerReverseQueryColumn.Tick += new System.EventHandler(this.timerReverseQueryColumn_Tick);
			// 
			// labelHorizon
			// 
			this.labelHorizon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelHorizon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelHorizon.Location = new System.Drawing.Point(8, 200);
			this.labelHorizon.Name = "labelHorizon";
			this.labelHorizon.Size = new System.Drawing.Size(584, 2);
			this.labelHorizon.TabIndex = 4;
			// 
			// lveQueryColumn
			// 
			this.lveQueryColumn.AllowColumnReorder = true;
			this.lveQueryColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lveQueryColumn.ContextMenuStrip = this.contextMenuQueryColumn;
			this.lveQueryColumn.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lveQueryColumn.FullRowSelect = true;
			this.lveQueryColumn.GridLines = true;
			this.lveQueryColumn.Location = new System.Drawing.Point(63, 9);
			this.lveQueryColumn.MultiSelect = false;
			this.lveQueryColumn.Name = "lveQueryColumn";
			this.lveQueryColumn.OwnerDraw = true;
			this.lveQueryColumn.Size = new System.Drawing.Size(505, 163);
			this.lveQueryColumn.TabIndex = 8;
			this.lveQueryColumn.UseCompatibleStateImageBehavior = false;
			this.lveQueryColumn.ValidItemCount = 64;
			this.lveQueryColumn.View = System.Windows.Forms.View.Details;
			this.lveQueryColumn.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lveQueryColumn_ColumnClick);
			this.lveQueryColumn.ColumnReordered += new System.Windows.Forms.ColumnReorderedEventHandler(this.lveQueryColumn_ColumnReordered);
			this.lveQueryColumn.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lveQueryColumn_DrawColumnHeader);
			this.lveQueryColumn.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lveQueryColumn_DrawSubItem);
			this.lveQueryColumn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lveQueryColumn_MouseClick);
			this.lveQueryColumn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lveQueryColumn_MouseMove);
			// 
			// splitContainerTable
			// 
			this.splitContainerTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainerTable.Location = new System.Drawing.Point(9, 72);
			this.splitContainerTable.Name = "splitContainerTable";
			// 
			// splitContainerTable.Panel1
			// 
			this.splitContainerTable.Panel1.Controls.Add(this.textTableName);
			this.splitContainerTable.Panel1.Controls.Add(this.listBoxTableList);
			this.splitContainerTable.Panel1MinSize = 100;
			// 
			// splitContainerTable.Panel2
			// 
			this.splitContainerTable.Panel2.Controls.Add(this.listBoxColumnList);
			this.splitContainerTable.Panel2MinSize = 100;
			this.splitContainerTable.Size = new System.Drawing.Size(584, 120);
			this.splitContainerTable.SplitterDistance = 255;
			this.splitContainerTable.SplitterWidth = 6;
			this.splitContainerTable.TabIndex = 3;
			this.splitContainerTable.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerTable_SplitterMoved);
			// 
			// textTableName
			// 
			this.textTableName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textTableName.Location = new System.Drawing.Point(8, 24);
			this.textTableName.Name = "textTableName";
			this.textTableName.Size = new System.Drawing.Size(100, 20);
			this.textTableName.TabIndex = 17;
			this.textTableName.Visible = false;
			// 
			// listBoxTableList
			// 
			this.listBoxTableList.ContextMenuStrip = this.contextMenuTableList;
			this.listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxTableList.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listBoxTableList.FormattingEnabled = true;
			this.listBoxTableList.HorizontalScrollbar = true;
			this.listBoxTableList.IntegralHeight = false;
			this.listBoxTableList.Location = new System.Drawing.Point(0, 0);
			this.listBoxTableList.Name = "listBoxTableList";
			this.listBoxTableList.Size = new System.Drawing.Size(251, 116);
			this.listBoxTableList.TabIndex = 0;
			this.listBoxTableList.SelectedIndexChanged += new System.EventHandler(this.listBoxTableList_SelectedIndexChanged);
			this.listBoxTableList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxTableList_KeyDown);
			// 
			// listBoxColumnList
			// 
			this.listBoxColumnList.ContextMenuStrip = this.contextMenuColumnList;
			this.listBoxColumnList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxColumnList.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.listBoxColumnList.FormattingEnabled = true;
			this.listBoxColumnList.HorizontalScrollbar = true;
			this.listBoxColumnList.IntegralHeight = false;
			this.listBoxColumnList.Location = new System.Drawing.Point(0, 0);
			this.listBoxColumnList.Name = "listBoxColumnList";
			this.listBoxColumnList.Size = new System.Drawing.Size(319, 116);
			this.listBoxColumnList.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Location = new System.Drawing.Point(9, 56);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxClearTableFilter);
			this.splitContainer1.Panel1.Controls.Add(this.textTableFilter);
			this.splitContainer1.Panel1.Controls.Add(this.labelTableList);
			this.splitContainer1.Panel1MinSize = 50;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pictureBoxClearColumnFilter);
			this.splitContainer1.Panel2.Controls.Add(this.textColumnFilter);
			this.splitContainer1.Panel2.Controls.Add(this.labelColumnList);
			this.splitContainer1.Panel2MinSize = 50;
			this.splitContainer1.Size = new System.Drawing.Size(584, 17);
			this.splitContainer1.SplitterDistance = 255;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.TabStop = false;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// pictureBoxClearTableFilter
			// 
			this.pictureBoxClearTableFilter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxClearTableFilter.Image")));
			this.pictureBoxClearTableFilter.Location = new System.Drawing.Point(133, 0);
			this.pictureBoxClearTableFilter.Name = "pictureBoxClearTableFilter";
			this.pictureBoxClearTableFilter.Size = new System.Drawing.Size(12, 12);
			this.pictureBoxClearTableFilter.TabIndex = 17;
			this.pictureBoxClearTableFilter.TabStop = false;
			this.pictureBoxClearTableFilter.Click += new System.EventHandler(this.pictureBoxClearTableFilter_Click);
			// 
			// textTableFilter
			// 
			this.textTableFilter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.textTableFilter.Dock = System.Windows.Forms.DockStyle.Right;
			this.textTableFilter.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textTableFilter.Location = new System.Drawing.Point(151, 0);
			this.textTableFilter.MaxLength = 256;
			this.textTableFilter.Name = "textTableFilter";
			this.textTableFilter.Size = new System.Drawing.Size(100, 18);
			this.textTableFilter.TabIndex = 1;
			this.toolTipQueryColumn.SetToolTip(this.textTableFilter, "絞り込む（↑↓でスクロール）");
			this.textTableFilter.WordWrap = false;
			this.textTableFilter.SizeChanged += new System.EventHandler(this.textTableFilter_SizeChanged);
			this.textTableFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textTableFilter_KeyUp);
			// 
			// labelTableList
			// 
			this.labelTableList.AutoSize = true;
			this.labelTableList.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelTableList.ForeColor = System.Drawing.Color.Navy;
			this.labelTableList.Location = new System.Drawing.Point(0, 0);
			this.labelTableList.Name = "labelTableList";
			this.labelTableList.Size = new System.Drawing.Size(75, 13);
			this.labelTableList.TabIndex = 0;
			this.labelTableList.Text = "テーブル一覧";
			// 
			// pictureBoxClearColumnFilter
			// 
			this.pictureBoxClearColumnFilter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxClearColumnFilter.Image")));
			this.pictureBoxClearColumnFilter.Location = new System.Drawing.Point(201, 0);
			this.pictureBoxClearColumnFilter.Name = "pictureBoxClearColumnFilter";
			this.pictureBoxClearColumnFilter.Size = new System.Drawing.Size(12, 12);
			this.pictureBoxClearColumnFilter.TabIndex = 17;
			this.pictureBoxClearColumnFilter.TabStop = false;
			this.pictureBoxClearColumnFilter.Click += new System.EventHandler(this.pictureBoxClearColumnFilter_Click);
			// 
			// textColumnFilter
			// 
			this.textColumnFilter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.textColumnFilter.Dock = System.Windows.Forms.DockStyle.Right;
			this.textColumnFilter.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textColumnFilter.Location = new System.Drawing.Point(219, 0);
			this.textColumnFilter.MaxLength = 256;
			this.textColumnFilter.Name = "textColumnFilter";
			this.textColumnFilter.Size = new System.Drawing.Size(100, 18);
			this.textColumnFilter.TabIndex = 1;
			this.toolTipQueryColumn.SetToolTip(this.textColumnFilter, "絞り込む（↑↓でスクロール）");
			this.textColumnFilter.WordWrap = false;
			this.textColumnFilter.SizeChanged += new System.EventHandler(this.textColumnFilter_SizeChanged);
			// 
			// labelColumnList
			// 
			this.labelColumnList.AutoSize = true;
			this.labelColumnList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelColumnList.ForeColor = System.Drawing.Color.Navy;
			this.labelColumnList.Location = new System.Drawing.Point(0, 0);
			this.labelColumnList.Name = "labelColumnList";
			this.labelColumnList.Size = new System.Drawing.Size(59, 13);
			this.labelColumnList.TabIndex = 0;
			this.labelColumnList.Text = "項目一覧";
			// 
			// Shenlong
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(604, 462);
			this.Controls.Add(this.labelHorizon);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.splitContainerTable);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip);
			this.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximumSize = new System.Drawing.Size(1024, 768);
			this.Name = "Shenlong";
			this.Text = "shenlong";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Shenlong_FormClosing);
			this.Load += new System.EventHandler(this.Shenlong_Load);
			this.Shown += new System.EventHandler(this.Shenlong_Shown);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Shenlong_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Shenlong_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Shenlong_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Shenlong_KeyUp);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.contextMenuTableList.ResumeLayout(false);
			this.contextMenuColumnList.ResumeLayout(false);
			this.tabTableJoin.ResumeLayout(false);
			this.contextMenuTableJoin.ResumeLayout(false);
			this.tabQueryColumn.ResumeLayout(false);
			this.tabQueryColumn.PerformLayout();
			this.contextMenuQueryColumn.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabSQL.ResumeLayout(false);
			this.tabSQL.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.splitContainerTable.Panel1.ResumeLayout(false);
			this.splitContainerTable.Panel1.PerformLayout();
			this.splitContainerTable.Panel2.ResumeLayout(false);
			this.splitContainerTable.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearTableFilter)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearColumnFilter)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.Label labelTableList;
		private System.Windows.Forms.Label labelColumnList;
		private System.Windows.Forms.TabPage tabTableJoin;
		private System.Windows.Forms.TabPage tabQueryColumn;
		private ListViewEx.ListViewEx lveQueryColumn;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusOraConn;
		private System.Windows.Forms.ListBox listBoxTableList;
		private System.Windows.Forms.ListBox listBoxColumnList;
		private System.Windows.Forms.TabPage tabSQL;
		private System.Windows.Forms.ContextMenuStrip contextMenuQueryColumn;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuDelete;
		private System.Windows.Forms.TextBox textValue;
		private System.Windows.Forms.ComboBox comboExpression;
		private System.Windows.Forms.ComboBox comboRightColOp;
		private System.Windows.Forms.ComboBox comboGroupFunc;
		private System.Windows.Forms.TextBox textSQL;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripNew;
		private System.Windows.Forms.ToolStripButton toolStripOpen;
		private System.Windows.Forms.ToolStripButton toolStripSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton toolStripContents;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuFile;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuNew;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSave;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuClose;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuEdit;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuTool;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuLogOn;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuToExcel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuOption;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuHelp;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAbout;
		private System.Windows.Forms.ToolStripButton toolStripToExcel;
		private System.Windows.Forms.ToolStripButton toolStripLogOn;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusFileName;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuBuildQueryColumnSQL;
		private System.Windows.Forms.ImageList imageListTabPage;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRecentFileName;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName4;
		private System.Windows.Forms.ToolStripButton toolStripSelectColumnDD;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolTip toolTipQueryColumn;
		private System.Windows.Forms.ToolStripButton toolStripRemoveEndColumn;
		private System.Windows.Forms.ContextMenuStrip contextMenuTableList;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSortTable;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSortTableName;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSortTableComment;
		private System.Windows.Forms.ToolStripButton toolStripOption;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName5;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName7;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRecentFileName8;
		private System.Windows.Forms.ContextMenuStrip contextMenuColumnList;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSelectAll;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuTableJoin;
		private System.Windows.Forms.ListView lvTableJoin;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ContextMenuStrip contextMenuTableJoin;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuReleaseJoin;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuJoinWay;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuInnerJoin;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuLeftJoin;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRightJoin;
		private System.Windows.Forms.CheckBox checkShowField;
		private System.Windows.Forms.ImageList imageCheckBox;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuContents;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuShowIndex;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuIndex;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.Timer timerReverseQueryColumn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuCopyQueryColumn;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuPasteQueryColumn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuFileProperty;
		private System.Windows.Forms.CheckBox checkStretchColumnWidth;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusColumnCount;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuColumnProperty;
		private System.Windows.Forms.TextBox textTableName;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuCutQueryColumn;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuPasteHere;
		private MySplitContainer.MySplitContainer splitContainer1;
		private MySplitContainer.MySplitContainer splitContainerTable;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusVersion;
		private System.Windows.Forms.ToolStripButton toolStripShowParamInputDlg;
		private System.Windows.Forms.ToolStripButton toolStripEnableSameColumnAppend;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuSwapColumn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripButton toolStripCustomTableSelect;
		private System.Windows.Forms.Label labelHorizon;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuRefreshTableList;
		private System.Windows.Forms.TextBox textTableFilter;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuFullOuterJoin;
		private System.Windows.Forms.TextBox textColumnFilter;
		private System.Windows.Forms.PictureBox pictureBoxClearTableFilter;
		private System.Windows.Forms.PictureBox pictureBoxClearColumnFilter;
	}
}

