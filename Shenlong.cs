#define	TABLE_NAME_HAS_ALIAS		// テーブル名が別名を持つ事がある
#define	COLLECT_OUTER_JOIN			// 正しい外部結合のSQLを構築する
#define	EXCEL_LATE_BINDING			// Excel をレイト バインディングで操作する。静的な場合は "Microsoft Excel 11.0 Object Library" を参照設定に追加する
//#define	ENABLED_SUBQUERY			// サブクエリのロジックを有効にする（実際にはプロジェクト プロパティの[ビルド][条件付きコンパイル定数]で設定する）
//#define	WITHIN_SHENGLOBAL			// ShenGlobal クラスを内包する（実際にはプロジェクト プロパティの[ビルド][条件付きコンパイル定数]で設定する）
#define	UPDATE_20131204
#define	UPDATE_20140729
#define	UPDATE_20160316
#define	UPDATE_20191120
#define	UPDATE_20251202
using CommonFunctions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static CommonFunctions.api;
#if !EXCEL_LATE_BINDING
using Excel = Microsoft.Office.Interop.Excel;
#endif
#if WITHIN_SHENGLOBAL
using ShenCore = Shenlong.Shenlong;
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class Shenlong : Form
	{
		// shenlong.ini
		public const string SHENLONG_INI_FILE_NAME = "shenlong.ini";			// shenlong.ini ファイル名
		public const string SETTINGS_SECTION = "Settings";						// [Settings] セクション
		private const string RESUME_SECTION = "Resume";							// [Resume] セクション
		public const string KEY_WRITE_ACCESS_LOG = "WriteAccessLog";			// アクセス ログを保存する設定（隠し設定）
		public const string KEY_LOGON_PWD_TOOLTIP = "LogOnPwdToolTip";			// ログオン パスワードで tooltip を表示する設定（隠し設定）
		public const string KEY_RESUME_APPEND_LOGON_HIS = "ResumeAppendLogOnHis";	// ログオン履歴に追加する状態の復元設定（拡張設定）
		public const string KEY_AUTO_CHANGE_LOGON = "AutoChangeLogOn";				// ログオン先を自動で切り替える設定（拡張設定）
		public const string KEY_SELECTABLE_CLEAR_COLUMN_LOGON = "SelectableClearColumnLogOn";	// ログオン時にクエリ項目をクリアするか否かを選択できる設定（拡張設定）
		public const string KEY_MAX_LOGON_HISTORY_COUNT = "MaxLogOnHistoryCount";		// ログオン履歴の最大数（拡張設定）
		public const string KEY_MAX_QUERY_COLUMN_COUNT = "MaxQueryColumnCount";			// クエリー項目の最大数（拡張設定）
		public const string KEY_REVERSE_QUERY_COLUMN_TIME = "ReverseQueryColumnTime";	// クエリー項目を反転表示する時間(ms)（拡張設定）
		public const string KEY_INTELLI_TABLE_JOIN_MENU = "IntelliTableJoinMenu";		// テーブル結合メニューで、同じカラム名を別表示にする設定（拡張設定）
		public const string KEY_FORM_MAXIMUM_SIZE = "FormMaximumSize";					// フォームの最大サイズ（拡張設定）
		public const string KEY_ORACLE_SQLPLUS = "OracleSqlPlusPath";					// オラクルの SQL*PLUS のパス（拡張設定）
		public const string KEY_EXPERT_MODE = "ExpertMode";						// エキスパート用で起動するか否か（拡張設定）
		public const string KEY_PUT_DIFF_OWNER_TO_TABLE = "PutDiffOwnerToTable";// オーナーが違うテーブル名に "OWNER." を付けるか否か（拡張設定）
		public const string KEY_ENABLE_EXCEL_PASTE_NONE = "EnableExcelPasteNone";// "Excel へ貼り付けない" オプションを有効にする（拡張設定）
		public const string KEY_SHOW_QUERY_RECORD_COUNT = "ShowQueryRecordCount";// クエリー前にレコード件数を表示する（拡張設定）
		public const string KEY_SELECT_TABLE_NAME = "SelectTableName";			// TABLE, VIEW のテーブル名を取得する SELECT 文（拡張設定）
		public const string KEY_SELECT_SYNONYM_NAME = "SelectSynonymName";		// SYNONYM のテーブル名を取得する SELECT 文（拡張設定）
		public const string KEY_SELECT_COLUMNS = "SelectColumns";				// 選択されたテーブルのカラムを取得する SELECT 文（拡張設定）
		public const string KEY_COLUMN_LIST_BACK_COLOR_NAME = "ColumnListBackColorName";// カラム一覧の背景色名（拡張設定）
		public const string KEY_QUERY_COLUMN_HEADER_BACK_COLOR_NAME = "QueryColumnHeaderBackColorName"; // クエリー項目のヘッダの背景色名（拡張設定）
		public const string KEY_QUERY_COLUMN_COLOR_NAMES = "QueryColumnColorNames";	// クエリー項目のテーブル毎の識別色名（拡張設定）
		public const string KEY_FOR_SHENLONG_BOOK_NAME = "ForShenlongBookName";	// Shenlong 用のブック名（拡張設定）
		public const string KEY_INCREMENTAL_TABLENAME_FILTER = "IncrementalTableNameFilter";	// テーブル名のフィルタをインクリメンタル サーチするか否か（拡張設定）
		public const string KEY_RESTORE_CLIPBOARD_AFTER_EXCEL_PASTE = "RestoreClipboardAfterExcelPaste";	// エクセル貼付後にクリップボードの内容を復元する（拡張設定）
		public const string KEY_COMBO_SQL_DATE_FORMAT = "ComboSqlDateFormat";	// SQL 日付の条件書式の履歴
		private const string KEY_WINDOW_RECTANGLE = "WindowRectangle";			// ウィンドウの位置とサイズ
		private const string KEY_SPILITTER_DISTANCE = "SplitterDistance";		// テーブル項目の分割値
		private const string KEY_SELECTED_TAB_PAGE_TEXT = "SelectedTabPageText";// 選択されたタブページ
		private const string KEY_CUSTOM_TABLE_SELECT = "CustomTableSelect";		// カスタマイズされたテーブルのセレクト文を使うか否か
		private const string KEY_FILEDLG_INI_DIR = "FileDlgIniDir";				// ファイル ダイアログの初期ディレクトリ
		private const string KEY_FILEDLG_FILTER_INDEX = "FileDlgFilterIndex";	// ファイル ダイアログのフィルタ インデックス
		private const string KEY_RECENT_FILENAME = "RecentFileName";			// 最近使ったファイル名
		private const string KEY_LAST_HELP_FILE_CHECK = "LastHelpFileCheck";	// 前回のヘルプファイルの更新をチェックした日時
		private const string KEY_BASE_URI = "BaseURI";							// 最後に使用したクエリー項目ファイルの baseURI

		private const string KEY_WRITE_LOG_DSN_UID_PWD = "WriteLogDsnUidPwd";	// ログを書き込むテーブルの接続先
		private const string KEY_LAST_COMMON_SETTINGS_WRITE_TIME = "LastCommonSettingsWriteTime";	// 前回読み込んだ CommonSettings.ini ファイルの更新日時

		// shenlong.exe.config
		public static string latestProgramFolder;		// 最新のプログラム フォルダを置いているフォルダ名
		public static string urlMailToDeveloper;		// 問い合わせ先の url
		public static bool reloadLastColumnsOnStartup;	// 起動時に前回の状態を読み込む
		public static bool selectColumnByDragDrop;		// ドラッグ＆ドロップでカラムを選択する
		public static bool showSynonymOwner;			// シノニムの前にオーナーを表示する
		//public static bool clearColumnBySelTbl;			// テーブルを選択する度にクエリー項目をクリアする
		public static int tableSelectedAction;			// テーブルが選択された時の処理
		public static bool editableColumnName;			// 項目名の編集を許可する
#if WITHIN_SHENGLOBAL
		public static string sqlDateFormat;				// SQL 日付の条件書式
#endif
		public static bool pasteColumnComments;			// クエリーの出力結果に項目のコメントも貼り付ける
		public static bool saveQueryOutputFile;			// クエリーの出力結果をファイルに保存する
		public static string textQueryOutputFileName;	// クエリー出力結果のファイル名
		public static pasteExcel pasteQueryResultToExcel;// クエリーの出力結果を Excel に貼り付ける対象
		public static omw oraMiddleware;				// オラクルに接続する方法
		public static bool showParamInputDlg;			// クエリー前に抽出条件入力ダイアログを表示する

		// shenlong.ini
		public static string shenlongIniFileName = Application.StartupPath + "\\" + SHENLONG_INI_FILE_NAME;
		public static bool writeAccessLog = true;
		public static bool logOnPwdToolTip = false;
		public static bool? resumeAppendLogOnHis = null;
		public static bool autoChangeLogOn = false;
		public static bool selectableClearColumnLogOn = false;
		private int maxLogOnHistoryCount = 16;
		private int maxColumnCount = 256;
		public static int reverseQueryColumnTime = 1500;
		public static bool intelliTableJoinMenu = true;
		public static string oracleSqlPlusPath = null;
		public static bool putDiffOwnerToTable = false;
		public static bool enableExcelPasteNone = false;
		public static bool showQueryRecordCount = true;
		public static string columnListBackColorName = null;
		public static string queryColumnHeaderBackColorName = null;
		public static string queryColumnColorNames = null;
		public static bool incrementalTableNameFilter = true;
		public static bool restoreClipboardAfterExcelPaste = true;
		public static Rectangle windowRectangle = Rectangle.Empty;
		private static int splitterDistance = -1;
		private string selectedTabPageText;
		private string fileDlgIniDir;
		private int fileDlgFilterIndex;

		public static string[] writeLogDsnUidPwd = { null, null, null };

		public static bool clearQueryColumnWhenOraLogOn = true;	// ログオン時にクエリー項目をクリアする

		public const string appTitle = "shenlong";

		private enum oraon { success, none, cancel, exception };
		public enum tableSelAct { showColumns, clearSelectedColumns, appendAllColumns };
		public enum pasteExcel { none, newBookActSheet, actBookActSheet, actBookNewSheet, shenBookNewSheet };
		public enum omw { OracleClient, OleDb, oo4o };		// 兼 OraConnWare@shenlong.exe.config の value

		private Cursor noneCursor = null;
		private Cursor moveCursor = null;
		private Cursor copyCursor = null;
		private Cursor linkCursor = null;

		private OracleConnection oraConn = null;			// Oracle Connection

		private KeyEventArgs formKeyDownArgs = null;		// フォーム上で押されたキーの状態
		private Font listBoxFontForWin2000 = null;			// Windows2000 用の ListBox のフォント

		private XmlDocument xmlTableList = null;			// テーブル名の一覧（現在の状態）

		private const string tagTableList = "tableList";
		private const string tagTable = "table";
		private const string attrName = "name";				// TAB.TNAME
		private const string attrType = "type";				// TAB.TABTYPE
		private const string attrOwner = "owner";			// USER_SYNONYMS.TABLE_OWNER
		private const string attrDbLink = "dbLink";			// USER_SYNONYMS.DB_LINK
		private const string attrComments = "comments";		// [USER|ALL]_TAB_COMMENTS
#if ENABLED_SUBQUERY
		private const string attrDir = "dir";				// サブクエリでのクエリー項目ファイルのディレクトリ

		private const string SUBQUERY_TYPE = "SUBQUERY";	// サブクエリの時のテーブルのタイプ
		private const string SUBQUERY_OWNER = "SUBEGG";		// サブクエリ時のテーブルのオーナー
#endif

		private bool ascendingTableName;					// テーブル名の並び替え順（現在の状態）
		private bool hasTableComments;						// テーブルのコメントがあるか否か（現在の状態）
#if TABLE_NAME_HAS_ALIAS
		private int editingTableNameIndex = -1;				// 編集中のテーブル名のインデックス
		private enum selTbl { raw = 0x0000, withOwner = 0x0001, plainTblName = 0x0002 };
#endif

		private List<int> selTableHistory = null;			// 選択されたテーブルの履歴
		private int curSelTableHistory = -1;				// 現在選択中のテーブル履歴

		private enum co { name, type, length, comment, nullable };	// カラム情報の並び順
		public static Brush columnListBackColor = null;				// カラム一覧の背景色
		private Point columnListLastMouseDown = Point.Empty;		// listBoxColumnList でマウスの押された位置

		private const string propNullable = "y";					// NULLABLE
		public const string propNotNullable = "n";					// NOT NULLABLE
#if WITHIN_SHENGLOBAL
		public const string propNoComment = "n/c";					// NO COMMENT

		public enum prop { type, length, nullable, comment, alias, bubbles, count };// カラムのプロパティ（兼属性|タグ名）
#endif
		private const string sepProp = "\t";						// カラムのプロパティの区切り

#if WITHIN_SHENGLOBAL
		public enum bubbSet { control, input, setValue, dropDownList, hyperLink, classify };	// [bubbles] バブルスの設定値（兼属性|タグ名）
		public const Char sepBubbSet = '&';							// バブルスの設定値の区切り
		public enum bubbCtrl { textBox, label, noVisible };			// バブルスのコントロール設定（兼属性名）
		public enum bubbInput { noAppoint, necessary };				// バブルスの入力条件設定（兼属性名）
#endif

		private List<int> selColumnHistory = null;					// 選択されたカラムの履歴
		private int curSelColumnHistory = -1;						// 現在選択中のカラム履歴

#if WITHIN_SHENGLOBAL
		public enum qc {											// クエリー項目のアイテム（兼タグ名）
			fieldName, showField, expression, value1, value2, rColOp, orderBy, groupFunc, property };
#endif

		private const int defColumnWidth = 100;						// デフォルトのクエリー項目の横幅
		private const int narColumnWidth = 24;						// 狭いクエリー項目の横幅

		private Font queryColumnFont = null;						// クエリー項目のフォント
		private int qcFontHeight = 16;                              // クエリー項目のフォントの高さ
		private Brush queryColumnHeaderBackColor = null;			// クエリー項目の背景色
		//private Pen[] queryColumnPens = null;						// クエリー項目のペン
		public static Brush[] queryColumnBrushes = null;			// クエリー項目のブラシ
		private Control[] editors;									// 編集用コントロール配列
		private ToolStripMenuItem[] contextTableJoinColumns;		// 選択されたカラムの 相対[テーブル名.カラム名] | 結合候補[テーブル名] コンテキスト（[テーブル結合] のサブメニュー）
		private ToolStripMenuItem[][] contextTableJoinCandiColumns;	// 結合候補の [テーブル][カラム名] コンテキスト（結合候補[テーブル名] のサブメニュー）
		private Font contextTableJoinFont = null;					// テーブル結合コンテキストのフォント

		private MouseEventArgs queryColumnLastMouseArgs = null;		// クエリー項目でマウスがクリックされた時のイベント引数
		private List<string> queryTableNames = new List<string>();	// 選択済みのテーブル名（現在の状態）
		private int lastQueryColumn = -1;							// 前回のマウスポインタのあったカラム番号
		private bool modified = false;								// 編集されたか否か
		private int reverseQueryColumn = -1;						// 反転するカラム番号（並び順に対する）

#if WITHIN_SHENGLOBAL
		public enum tabJoin { leftTabCol, way, rightTabCol };		// [テーブル結合] のサブアイテム（兼タグ名）
#endif

		private string xmlShenlongColumnFileName = null;				// クエリー項目ファイル名
		private const int maxRecentFileName = 8;						// 最近使ったファイル名の最大数
		private List<string> recentFileNames = null;					// 最近使ったファイル名
		private ToolStripMenuItem[] toolStripMenuRecentFileNames = null;// 最近使ったファイル名のメニュー

		private XmlDocument xmlCopiedShenlongColumn = null;				// コピーされたクエリー項目

		private string fileComment;										// ファイルのコメント
		private string fileAuthor;										// ファイルの作成者
		private bool fileDistinct;										// 重複行を除いて抽出する
		private bool fileUseJoin;										// JOIN を使ってテーブルを結合する
		public static int fileHeaderOutput;								// ヘッダの出力フラグ
		private bool fileDownLoad;										// [bubbles] ダウンロードを許可するか否か
		private string fileEggPermission;								// [bubbles] タマゴへのアクセスを許可する端末
		private string fileMaxRowNum;									// [bubbles] 抽出する最大行数
		private bool fileSetValue;										// [bubbles] テキストボックスに値をセットするか否か
		private bool fileSqlSelect;										// [bubbles] SQLのSELECT文で抽出するか否か
#if ENABLED_SUBQUERY
		private List<string> fileSubQuery;								// サブクエリ用のクエリー項目ファイル
#endif

		private const string xmlTempQueryColumnFileName = "~tempQueryColumn.xml";
		private const string xmlLatestColBeforeExcelFileName = "~latestColBeforeExcel.xml";
		private const string xmlLatestQueryColumnFileName = "~latestQueryColumn.xml";

#if WITHIN_SHENGLOBAL
		public const string tagShenlong = "shenlong";
		public const string attrSID = "sid";
		public const string attrUserName = "userName";
		private const string attrVer = "ver";
		public const string tagColumn = "column";
		public const string attrTableName = "tableName";
		private const string attrWidth = "width";
		public const string tagTableJoin = "tableJoin";
		private const string tagSQL = "sql";
		private const string tagBuildedSQL = "buildedSql";
		public const string tagProperty = "property";
		private const string tagComment = "comment";
		private const string tagAuthor = "author";
		private const string tagDownload = "download";
		private const string tagMaxRowNum = "maxRowNum";
		public const string tagSetValue = "setValue";
#if ENABLED_SUBQUERY
		public const string tagSubQuery = "subQuery";

		public const char SUBQUERY_SEPARATOR = ';';
		public const string SUBQUERY_RELATIVE_PATH = ".";
#endif

		private enum authorize { permit, deny };						// 権限を許可するか否か（兼値）

		public const string withoutTableName = "::";
#endif

		private const string oraConnNone = "未接続";
		private const char oraConnStatusSplitter = '@'/*'/'*/;
		private enum logon { uid, sid }/*{ sid, uid }*/;

		private string forShenlongBookName = "~shenlong.xls";

		private Encoding sjisEnc = Encoding.GetEncoding("shift_jis");

#if WITHIN_SHENGLOBAL
		public const string sepOutput = "\t";							// クエリー出力の区切り
#endif

		private const uint WM_READ_SHENLONG_COLUMN_FILE = (api.WM_APP + 100);

		private Dictionary<string, string> latestSelectParams = null;

		private bool enableSameColumnAppend = false;					// 重複して項目を追加できるか否か

		private System.Threading.Timer timerReadCommonSettings = null;	// CommonSettings.ini ファイルを取り込むタイマ

		private common.platform osPlatform;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Shenlong()
		{
			InitializeComponent();

			try
			{
				osPlatform = common.GetOsPlatform();
				if ( Program.debMode )
				{
					StringBuilder _returnedString = new StringBuilder(1024);
					api.GetPrivateProfileString(SETTINGS_SECTION, "OsPlatform", ((int)osPlatform).ToString("X08"), _returnedString, (uint)_returnedString.Capacity, shenlongIniFileName);
					osPlatform = (common.platform)int.Parse(_returnedString.ToString(), System.Globalization.NumberStyles.HexNumber);
				}

				// 現在のコードを実行しているアセンブリを取得します
				Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
#if (DEBUG)
				string[] resNames = asm.GetManifestResourceNames();
				StringBuilder sb = new StringBuilder(resNames.GetUpperBound(0));
				foreach ( string resName in resNames )
				{
					sb.AppendFormat("{0}\r\n", resName);
				}
				Debug.Write(sb);
#endif
				noneCursor = new Cursor(asm.GetManifestResourceStream("Shenlong.Resources.none.cur"));
				moveCursor = new Cursor(asm.GetManifestResourceStream("Shenlong.Resources.move.cur"));
				copyCursor = new Cursor(asm.GetManifestResourceStream("Shenlong.Resources.copy.cur"));
				linkCursor = new Cursor(asm.GetManifestResourceStream("Shenlong.Resources.link.cur"));

				menuStrip.RenderMode = ToolStripRenderMode.System;	// WinXP でのメニューが青くなる対策（デザインがデフォルトの場合）

				if ( osPlatform == common.platform.win2000 )
				{
					menuStrip.RenderMode = ToolStripRenderMode.Professional;	// メニューが黒くなる対策
					listBoxFontForWin2000 = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
					this.listBoxTableList.Font = listBoxFontForWin2000;
					this.listBoxColumnList.Font = listBoxFontForWin2000;
				}

				recentFileNames = new List<string>();
				toolStripMenuRecentFileNames = new ToolStripMenuItem[] { toolStripMenuRecentFileName1, toolStripMenuRecentFileName2, toolStripMenuRecentFileName3, toolStripMenuRecentFileName4,
																		 toolStripMenuRecentFileName5, toolStripMenuRecentFileName6, toolStripMenuRecentFileName7, toolStripMenuRecentFileName8 };

				columnListBackColorName = "GhostWhite";

				string queryColumnFontName = "Helvetica"/*"ＭＳ Ｐゴシック"/*"MS Pゴシック"/*"system"/*"MS UI Gothic"*/;
				queryColumnFont = new Font(queryColumnFontName, 10, FontStyle.Regular);
				qcFontHeight = queryColumnFont.Height;
				queryColumnHeaderBackColorName = "LightGray";
				/*Color[] colColors = { Color.Black, Color.Blue, Color.Green, Color.Brown, Color.Cyan, Color.Lime, Color.Violet, Color.Gold };
				queryColumnPens = new Pen[colColors.Length];
				for ( int i = 0; i < colColors.Length; i++ )
				{
					queryColumnPens[i] = new Pen(colColors[i], 1);
				}*/
				/*queryColumnBrushes = new Brush[] { Brushes.Black, Brushes.Blue, Brushes.Tan, Brushes.Purple, Brushes.Gold, Brushes.DeepSkyBlue, Brushes.Violet, Brushes.Green,
												   Brushes.SandyBrown, Brushes.DarkKhaki, Brushes.DarkMagenta, Brushes.DarkOrange, Brushes.SteelBlue, Brushes.LawnGreen, Brushes.DeepPink, Brushes.DimGray };*/
				queryColumnColorNames = "Black,Blue,DarkGreen,Purple,SteelBlue,Chocolate,Indigo,DarkSlateGray,Maroon,Olive,DodgerBlue,PaleVioletRed,DarkOliveGreen,DarkGoldenrod,YellowGreen,DarkGray";
				queryColumnBrushes = new Brush[16];

				editors = new Control[] {
					/*カラム名	表示する/しない	条件式				値１		値２		右列連結演算子		ソート順	グループ関数*/
					textValue,	checkShowField,	comboExpression,	textValue,	textValue,	comboRightColOp,	textValue,	comboGroupFunc};

				toolStripMenuToExcel.ShortcutKeyDisplayString = Keys.F5.ToString();
				toolStripShowParamInputDlg.Checked = showParamInputDlg;

				// Immediately accept the new value once the value of the control has changed
				checkShowField.CheckedChanged += new EventHandler(control_SelectedValueChanged);
				comboExpression.SelectedIndexChanged += new EventHandler(control_SelectedValueChanged);
				comboRightColOp.SelectedIndexChanged += new EventHandler(control_SelectedValueChanged);
				comboGroupFunc.SelectedIndexChanged += new EventHandler(control_SelectedValueChanged);

				lveQueryColumn.ValidItemCount = editors.Length;
				lveQueryColumn.SubItemClicked += new ListViewEx.SubItemEventHandler(lveQueryColumn_SubItemClicked);
				lveQueryColumn.SubItemEndEditing += new ListViewEx.SubItemEndEditingEventHandler(lveQueryColumn_SubItemEndEditing);

				contextMenuQueryColumn.Tag = -1;

				contextTableJoinColumns = new ToolStripMenuItem[0];
				contextTableJoinCandiColumns = new ToolStripMenuItem[0][];
				contextTableJoinFont = new Font("Tahoma", 8.25F, FontStyle.Bold);

				checkStretchColumnWidth.ForeColor = Color.LightGray;

#if true
				textSQL.KeyDown += new KeyEventHandler(textSQL_KeyDown);
#endif

#if COLLECT_OUTER_JOIN
				toolStripMenuInnerJoin.ToolTipText = "INNER JOIN";
				toolStripMenuLeftJoin.Text = "左外部結合 (≧)";
				toolStripMenuLeftJoin.ToolTipText = "LEFT OUTER JOIN";
				toolStripMenuRightJoin.Text = "右外部結合 (≦)";
				toolStripMenuRightJoin.ToolTipText = "RIGHT OUTER JOIN";
				toolStripMenuFullOuterJoin.ToolTipText = "FULL OUTER JOIN";
#endif

#if ENABLED_SUBQUERY
				/*label6.AllowDrop = true;
				this.label6.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelTableList_DragEnter);
				this.label6.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelTableList_DragDrop);*/
				this.listBoxTableList.DoubleClick += new EventHandler(listBoxTableList_DoubleClick);
				fileSubQuery = new List<string>();
#endif

#if true
				this.listBoxColumnList.DrawMode = DrawMode.OwnerDrawFixed;
				this.listBoxColumnList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxColumnList_DrawItem);
#endif

#if !WITHIN_SHENGLOBAL
				ShenGlobal.app = ShenGlobal.apps.form;
#endif

				StringBuilder returnedString = new StringBuilder(1024);

				// エキスパート用で起動するか否か（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_EXPERT_MODE, Program.expertMode.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				Program.expertMode = bool.Parse(returnedString.ToString());

				if ( Program.expertMode )
				{
					if ( toolStripCustomTableSelect.Enabled = CustomTableSelectEnabled() )
					{
						api.GetPrivateProfileString(RESUME_SECTION, KEY_CUSTOM_TABLE_SELECT, false.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
						toolStripCustomTableSelect.Checked = bool.Parse(returnedString.ToString());
					}
				}
				else
				{
					toolStripCustomTableSelect.Visible = false;
				}

				// [Windows 7] リストビューの高さを伸ばしておく
                if ( (osPlatform & (common.platform.win7 | common.platform.win10)) != 0 )
				{
					int offset = 251/*239*/ - tabControl.Height;	// 251: VisualStudio2010@Windows7 でプロジェクトを読み込んだときの高さ
					splitContainerTable.Anchor -= AnchorStyles.Bottom;
					labelHorizon.Anchor = AnchorStyles.Top;
					tabControl.Anchor = AnchorStyles.Top;

					tabControl.Height += offset;
					lvTableJoin.Height += offset;
					this.Height += (offset + 4);					// 4: とりあえず微調整

					splitContainerTable.Anchor |= AnchorStyles.Bottom;
					labelHorizon.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
					tabControl.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

					for ( int i = 1; i <= 8; i++ )
					{
						Label label = (Label)tabQueryColumn.Controls["label" + i];
						label.Top += i * 3;
					}
				}

                if ( osPlatform == common.platform.win10 )
                {
                    lvTableJoin.OwnerDraw = true;
                    lvTableJoin.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(lvTableJoin_DrawColumnHeader);
                    lvTableJoin.DrawSubItem += new DrawListViewSubItemEventHandler(lvTableJoin_DrawSubItem);
                }
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				this.Close();
			}
		}

		/// <summary>
		/// WndProc
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			try
			{
				switch ( (uint)m.Msg )
				{
					case api.WM_COPYDATA:
						api.COPYDATASTRUCT cds = (api.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(api.COPYDATASTRUCT));
#if true
						byte[] fnameBuff = new byte[cds.cbData/* / sizeof(byte)*/];
						Marshal.Copy(cds.lpData, fnameBuff, 0, fnameBuff.Length);
						string cmdParamShenlongColumnFileName = Encoding.Unicode.GetString(fnameBuff);
						//cmdParamShenlongColumnFileName = cmdParamShenlongColumnFileName.TrimEnd('\0'); // 送り側が StringToHGlobalUni の時
#else
						string cmdParamShenlongColumnFileName = Marshal.PtrToStringUni(cds.lpData/*, cds.cbData*/);
#endif
						//ReadShenlongColumnFile(cmdParamShenlongColumnFileName, false, true);
						Program.cmdParamShenlongColumnFileName = cmdParamShenlongColumnFileName;
						api.PostMessage(this.Handle, WM_READ_SHENLONG_COLUMN_FILE, 0, 0);	// 呼び出し側の SendMessage を速やかに抜ける為、一旦ポストしてファイルを開く
						break;

					case WM_READ_SHENLONG_COLUMN_FILE:
						ReadShenlongColumnFile(Program.cmdParamShenlongColumnFileName, false, true);
						break;

					/*case 0x214:    //WM_SIZING
					case 0x216:    //WM_MOVING
						Rectangle rect = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
						Point location = rect.Location;
						Size size = new Size(rect.Width - rect.Left, rect.Height - rect.Top);
						if ( size.Width < minimumFormSize.Width || size.Height < minimumFormSize.Height )
						{
							//this.Size = minimumFormSize;
							Debug.Write(size.ToString() + "\r\n");
							return;
						}
						break;*/
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}

			base.WndProc(ref m);
		}

		/// <summary>
		/// Shenlong_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_Load(object sender, EventArgs e)
		{
			try
			{
				MyMessageBox._mainForm = this;

				this.MinimumSize = this.Size;	// WM_SIZING を捕まえて処理する代わり

				// shenlong.ini を読み込む
				GetPrivateProfile();
				//lveQueryColumn.DoubleClickActivation = false;
				lveQueryColumn.Size = new Size(lveQueryColumn.Size.Width, tabControl.Size.Height - 43);

				ToolStripMenuEnable(false);
				//ClearQueryColumn();

				RefreshRecentFileNameMenu();

				listBoxColumnList.SelectedIndexChanged += new EventHandler(listBoxColumnList_SelectedIndexChanged);
				listBoxColumnList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxColumnList_KeyDown);
				ChangeColumnSelection();

				if ( incrementalTableNameFilter )
				{
					this.textTableFilter.KeyUp -= this.textTableFilter_KeyUp;
					this.textTableFilter.KeyDown += new KeyEventHandler(this.textTableFilter_KeyDown);
					this.textTableFilter.TextChanged += new System.EventHandler(this.textTableFilter_TextChanged);
				}

				this.textColumnFilter.KeyDown += new KeyEventHandler(this.textColumnFilter_KeyDown);
				this.textColumnFilter.TextChanged += new System.EventHandler(this.textColumnFilter_TextChanged);

				Assembly myAssembly = Assembly.GetExecutingAssembly();	// 自分自身の Assembly を取得
				Version myVer = myAssembly.GetName().Version;			// バージョンの取得

				DateTime buildDateTime = new DateTime(2000, 1, 1);
				TimeSpan verSpan = new TimeSpan(myVer.Build * TimeSpan.TicksPerDay + myVer.Revision * 2 * TimeSpan.TicksPerSecond);
				buildDateTime += verSpan;

				//toolStripStatusVersion.Text = "Version " + myVer.Major + "." + myVer.Minor.ToString("D2") + (Program.expertMode ? "." + myVer.Build + " ex" : "");
				string version = "Version " + myVer.Major + "." + myVer.Minor.ToString("D2") + (Program.debMode ? "." + buildDateTime.ToString("yyMMdd.HHmm") : "") + (Program.expertMode ? " ex" : "");
				toolStripStatusVersion.Text = version + (Program.debMode ? "  " + "@Win" + Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor : "");

				if ( Program.cmdParamShenlongColumnFileName == null )
				{
					LogOnDlg.usages usage = (reloadLastColumnsOnStartup) ? LogOnDlg.usages.resume : LogOnDlg.usages.manual;
					if ( OraLogOn(usage, null, null) == oraon.cancel )
					{
						this.Close();
						return;
					}
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// SHENLONG_INI_FILE_NAME を読み込む
		/// </summary>
		private void GetPrivateProfile()
		{
			try
			{
				if ( !File.Exists(shenlongIniFileName) )
				{
					FileStream fs = File.Create(shenlongIniFileName);
					fs.Close();
				}

				StringBuilder returnedString = new StringBuilder(1024);

				// アクセス ログを保存する設定（隠し設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_WRITE_ACCESS_LOG, writeAccessLog.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				writeAccessLog = bool.Parse(returnedString.ToString());

				// ログオン パスワードで tooltip を表示する設定（隠し設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_LOGON_PWD_TOOLTIP, logOnPwdToolTip.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				logOnPwdToolTip = bool.Parse(returnedString.ToString());

				// ログオン履歴に追加する状態の復元設定（拡張設定）
				if ( api.GetPrivateProfileString(SETTINGS_SECTION, KEY_RESUME_APPEND_LOGON_HIS, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) != 0 )
				{
					resumeAppendLogOnHis = bool.Parse(returnedString.ToString());
				}

				// ログオン先を自動で切り替える設定（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_AUTO_CHANGE_LOGON, autoChangeLogOn.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				autoChangeLogOn = bool.Parse(returnedString.ToString());

				// ログオン時にクエリ項目をクリアするか否かを選択できる設定（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECTABLE_CLEAR_COLUMN_LOGON, selectableClearColumnLogOn.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				selectableClearColumnLogOn = bool.Parse(returnedString.ToString());

				// ログオン履歴の最大数（拡張設定）
				maxLogOnHistoryCount = (int)api.GetPrivateProfileInt(SETTINGS_SECTION, KEY_MAX_LOGON_HISTORY_COUNT, maxLogOnHistoryCount, shenlongIniFileName);

				// クエリー項目の最大数（拡張設定）
				maxColumnCount = (int)api.GetPrivateProfileInt(SETTINGS_SECTION, KEY_MAX_QUERY_COLUMN_COUNT, maxColumnCount, shenlongIniFileName);

				// クエリー項目を反転表示する時間(ms)（拡張設定）
				reverseQueryColumnTime = (int)api.GetPrivateProfileInt(SETTINGS_SECTION, KEY_REVERSE_QUERY_COLUMN_TIME, reverseQueryColumnTime, shenlongIniFileName);

				// テーブル結合メニューで、同じカラム名を別表示にする設定（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_INTELLI_TABLE_JOIN_MENU, intelliTableJoinMenu.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				intelliTableJoinMenu = bool.Parse(returnedString.ToString());

				// フォームの最大サイズ（拡張設定）
				if ( api.GetPrivateProfileString(SETTINGS_SECTION, KEY_FORM_MAXIMUM_SIZE, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) != 0 )
				{
					try
					{
						string[] size = returnedString.ToString().Split(',');
						Size maxSize = new Size(int.Parse(size[0]), int.Parse(size[1]));
						if ( (maxSize.Width == 0 && maxSize.Height == 0) || (this.Size.Width < maxSize.Width && this.Size.Height < maxSize.Height) )
						{
							this.MaximumSize = maxSize;
						}
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
				}

				// オラクルの SQL*Plus のパス（拡張設定）
				if ( api.GetPrivateProfileString(SETTINGS_SECTION, KEY_ORACLE_SQLPLUS, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) != 0 )
				{
					oracleSqlPlusPath = returnedString.ToString();
				}

				/*// エキスパート用で起動するか否か（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_EXPERT_MODE, Program.expertMode.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				Program.expertMode = bool.Parse(returnedString.ToString());*/

				// オーナーが違うテーブル名に "OWNER." を付けるか否か（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_PUT_DIFF_OWNER_TO_TABLE, putDiffOwnerToTable.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				putDiffOwnerToTable = bool.Parse(returnedString.ToString());

				// "Excel へ貼り付けない" オプションを有効にする（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_ENABLE_EXCEL_PASTE_NONE, enableExcelPasteNone.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				enableExcelPasteNone = bool.Parse(returnedString.ToString());

				// クエリー前にレコード件数を表示する（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SHOW_QUERY_RECORD_COUNT, showQueryRecordCount.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				showQueryRecordCount = bool.Parse(returnedString.ToString());

				// カラム一覧の背景色名（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_COLUMN_LIST_BACK_COLOR_NAME, columnListBackColorName, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				columnListBackColorName = returnedString.ToString();
				columnListBackColor = new SolidBrush(Color.FromName(columnListBackColorName));

				// クエリー項目のヘッダの背景色名（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_QUERY_COLUMN_HEADER_BACK_COLOR_NAME, queryColumnHeaderBackColorName, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				queryColumnHeaderBackColorName = returnedString.ToString();
				queryColumnHeaderBackColor = new SolidBrush(Color.FromName(queryColumnHeaderBackColorName));
				
				// クエリー項目のテーブル毎の識別色名（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_QUERY_COLUMN_COLOR_NAMES, queryColumnColorNames, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				queryColumnColorNames = returnedString.ToString();
				SetQueryColumnBrushes();

				// Shenlong 用のブック名（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_FOR_SHENLONG_BOOK_NAME, forShenlongBookName, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				forShenlongBookName = returnedString.ToString();

				// テーブル名のフィルタをインクリメンタル サーチするか否か（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_INCREMENTAL_TABLENAME_FILTER, incrementalTableNameFilter.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				incrementalTableNameFilter = bool.Parse(returnedString.ToString());

				// エクセル貼付後にクリップボードの内容を復元する（拡張設定）
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_RESTORE_CLIPBOARD_AFTER_EXCEL_PASTE, restoreClipboardAfterExcelPaste.ToString(), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				restoreClipboardAfterExcelPaste = bool.Parse(returnedString.ToString());

				// ログを書き込むテーブルの接続先
				string _writeLogDsnUidPwd = common.EncodePassword("dsn,uid,pwd");
				api.GetPrivateProfileString(SETTINGS_SECTION, KEY_WRITE_LOG_DSN_UID_PWD, _writeLogDsnUidPwd, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				_writeLogDsnUidPwd = common.DecodePassword(returnedString.ToString());
				writeLogDsnUidPwd = _writeLogDsnUidPwd.Split(',');

				// ウィンドウの位置とサイズ
				if ( !windowRectangle.IsEmpty )
				{
					if ( Program.isNewInstance )
						this.SetBounds(windowRectangle.X, windowRectangle.Y, windowRectangle.Width, windowRectangle.Height);
					else
						this.Size = new Size(windowRectangle.Width, windowRectangle.Height);
				}

				if ( splitterDistance != -1 )
				{
					splitContainer1.SplitterDistance = splitterDistance;
					splitContainerTable.SplitterDistance = splitterDistance;
				}

				// 選択されたタブ名
				api.GetPrivateProfileString(RESUME_SECTION, KEY_SELECTED_TAB_PAGE_TEXT, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				selectedTabPageText = returnedString.ToString();

				// ファイル ダイアログの初期ディレクトリ
				api.GetPrivateProfileString(RESUME_SECTION, KEY_FILEDLG_INI_DIR, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				fileDlgIniDir = returnedString.ToString();

				// ファイル ダイアログのフィルタ インデックス
				fileDlgFilterIndex = (int)api.GetPrivateProfileInt(RESUME_SECTION, KEY_FILEDLG_FILTER_INDEX, 1, shenlongIniFileName);

				// 最近使ったファイル名
				for ( int i = 1; i <= maxRecentFileName; i++ )
				{
					if ( api.GetPrivateProfileString(RESUME_SECTION, KEY_RECENT_FILENAME + i, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) == 0 )
						break;
					recentFileNames.Add(returnedString.ToString());
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(shenlongIniFileName + "\r\nini ファイルの読み込みが失敗しました.\r\n原因：" + exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// クエリー項目のテーブル毎のブラシ名をセットする
		/// </summary>
		public static void SetQueryColumnBrushes()
		{
			try
			{
				string[] _queryColumnColorNames = queryColumnColorNames.Split(',');

				int i = 0;
				for ( ; (i < queryColumnBrushes.Length) && (i < _queryColumnColorNames.Length); i++ )
				{
					SolidBrush brush = new SolidBrush(Color.FromName(_queryColumnColorNames[i]));
					if ( queryColumnBrushes[i] != null )
					{
						queryColumnBrushes[i].Dispose();
					}
					queryColumnBrushes[i] = brush;
				}

				for ( ; i < queryColumnBrushes.Length; i++ )
				{
					if ( queryColumnBrushes[i] != null )
					{
						queryColumnBrushes[i].Dispose();
					}
					queryColumnBrushes[i] = new SolidBrush(Color.White);
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// カスタマイズされたテーブルのセレクト文が有効か否か
		/// </summary>
		/// <returns></returns>
		private bool CustomTableSelectEnabled()
		{
			try
			{
				StringBuilder returnedString = new StringBuilder (1024);
				uint cSelectTableName = api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECT_TABLE_NAME, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				uint cSelectCulumns = api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECT_COLUMNS, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				return (cSelectTableName != 0 && cSelectCulumns != 0);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
				return false;
			}
		}

		/// <summary>
		/// ウィンドウの位置とサイズ
		/// </summary>
		public static void GetWindowRectangle()
		{
			StringBuilder returnedString = new StringBuilder(1024);
			Rectangle windowRectangle = Rectangle.Empty;

			api.GetPrivateProfileString(RESUME_SECTION, KEY_WINDOW_RECTANGLE, "\0", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);

			if ( returnedString.Length != 0 )
			{
				string[] rect = returnedString.ToString().Split(',');
				windowRectangle = new Rectangle(Int32.Parse(rect[0]), Int32.Parse(rect[1]), Int32.Parse(rect[2]), Int32.Parse(rect[3]));
			}

			Shenlong.windowRectangle = windowRectangle;

			Shenlong.splitterDistance = (int)api.GetPrivateProfileInt(RESUME_SECTION, KEY_SPILITTER_DISTANCE, -1, shenlongIniFileName);
		}

		/// <summary>
		/// カラムの選択方法を変更する
		/// </summary>
		private void ChangeColumnSelection()
		{
			if ( selectColumnByDragDrop )
			{
				//listBoxColumnList.SelectedIndexChanged -= listBoxColumnList_SelectedIndexChanged;
				listBoxColumnList.DoubleClick += new EventHandler(listBoxColumnList_DoubleClick);

				listBoxColumnList.SelectionMode = SelectionMode.MultiExtended;
				listBoxColumnList.MouseDown += new MouseEventHandler(listBoxColumnList_MouseDown);
				listBoxColumnList.MouseMove += new MouseEventHandler(listBoxColumnList_MouseMove);
				listBoxColumnList.MouseUp += new MouseEventHandler(listBoxColumnList_MouseUp);
				listBoxColumnList.QueryContinueDrag += new QueryContinueDragEventHandler(listBoxColumnList_QueryContinueDrag);
				listBoxColumnList.GiveFeedback += new GiveFeedbackEventHandler(listBoxColumnList_GiveFeedback);

				lveQueryColumn.AllowDrop = true;
				lveQueryColumn.DragOver += new DragEventHandler(lveQueryColumn_DragOver);
				lveQueryColumn.DragDrop += new DragEventHandler(lveQueryColumn_DragDrop);

				toolStripSelectColumnDD.Checked = true;
			}
			else
			{
				//listBoxColumnList.SelectedIndexChanged += new EventHandler(listBoxColumnList_SelectedIndexChanged);
				listBoxColumnList.DoubleClick -= listBoxColumnList_DoubleClick;

				listBoxColumnList.SelectionMode = SelectionMode.One;
				listBoxColumnList.MouseDown -= listBoxColumnList_MouseDown;
				listBoxColumnList.MouseMove -= listBoxColumnList_MouseMove;
				listBoxColumnList.MouseUp -= listBoxColumnList_MouseUp;
				listBoxColumnList.QueryContinueDrag -= listBoxColumnList_QueryContinueDrag;
				listBoxColumnList.GiveFeedback -= listBoxColumnList_GiveFeedback;

				lveQueryColumn.AllowDrop = false;
				lveQueryColumn.DragOver -= lveQueryColumn_DragOver;
				lveQueryColumn.DragDrop -= lveQueryColumn_DragDrop;

				toolStripSelectColumnDD.Checked = false;
			}
		}

		/// <summary>
		/// Shenlong_Shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_Shown(object sender, EventArgs e)
		{
			if ( oraConn != null )
			{
				SelectTableName();
			}

			try
			{
				if ( Program.cmdParamShenlongColumnFileName != null )
				{
					ReadShenlongColumnFile(Program.cmdParamShenlongColumnFileName, false, true);
					return;
				}

				InitFileProperty();

				if ( reloadLastColumnsOnStartup )
				{
					string fileName = Application.StartupPath + "\\" + xmlLatestQueryColumnFileName;
					if ( !toolStripStatusOraConn.Text.EndsWith/*.StartsWith*/(oraConnNone) && File.Exists(fileName) )
					{
						XmlDocument xmlShenlongColumn = new XmlDocument();
						xmlShenlongColumn.Load(fileName);
						if ( IsEqualCurrentOraConn(xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrSID].Value, xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrUserName].Value) )
						{
							ReadShenlongColumnFile(fileName, true);
						}
					}

					for ( int i = 0; i < tabControl.TabCount; i++ )
					{
						if ( tabControl.TabPages[i].Text == selectedTabPageText )
						{
							tabControl.SelectedIndex = i;
							tabSQL.Select();
							break;
						}
					}
				}

				// timerReadCommonSettings をセットする
				TimerCallback timerDelegate = new TimerCallback(OnTimerReadCommonSettings);
				timerReadCommonSettings = new System.Threading.Timer(timerDelegate, null, Timeout.Infinite, 0);
				timerReadCommonSettings.Change(10 * 1000, System.Threading.Timeout.Infinite);	// SetTimer（周期的なシグナル通知は無効）
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// リモート側の CommonSettings.ini を読み込んで、ローカル側の shenlong.ini に反映させる
		/// </summary>
		/// <param name="obj"></param>
		private void OnTimerReadCommonSettings(object obj)
		{
			try
			{
				string commonSettingsIniFileName = latestProgramFolder + "CommonSettings.ini";
				if ( !File.Exists(commonSettingsIniFileName) )
					return;
				DateTime commonSettingsIniWriteTime = File.GetLastWriteTime(commonSettingsIniFileName);

				StringBuilder returnedString = new StringBuilder(1024);
				// 前回読み込んだ CommonSettings.ini ファイルの更新日時
				api.GetPrivateProfileString(RESUME_SECTION, KEY_LAST_COMMON_SETTINGS_WRITE_TIME, "2000/01/01 00:00:00", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
				DateTime lastCommonSettingsWriteTime = DateTime.Parse(returnedString.ToString());

				if ( commonSettingsIniWriteTime <= lastCommonSettingsWriteTime )	// CommonSettings.ini は前回から更新されてない？
					return;

				byte[] returnedByte = new byte[0xffff];
				// リモート側の Settings@CommonSettings.ini 
				int count = (int)api.GetPrivateProfileSection(SETTINGS_SECTION, returnedByte, (uint)returnedByte.Length, commonSettingsIniFileName);

				string settings = Encoding.GetEncoding("Shift_JIS").GetString(returnedByte, 0, count - 1);
				string[] keys = settings.Split('\0');

				for ( int i = 0; i < keys.Length; i++ )
				{
					string[] keyValue = keys[i].Split('=');
					api.WritePrivateProfileString(SETTINGS_SECTION, keyValue[0], (string.IsNullOrEmpty(keyValue[1]) ? null : keyValue[1]), shenlongIniFileName);
					Debug.WriteLine(keys[i]);
				}

				api.WritePrivateProfileString(RESUME_SECTION, KEY_LAST_COMMON_SETTINGS_WRITE_TIME, commonSettingsIniWriteTime.ToString("yyyy/MM/dd HH:mm:ss"), shenlongIniFileName);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
			finally
			{
				try
				{
					//timerReadCommonSettings.Change(Timeout.Infinite, 0);	// KillTimer
					timerReadCommonSettings.Dispose();
					timerReadCommonSettings = null;
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
				}
			}
		}

		/// <summary>
		/// Shenlong_FormClosing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				XmlDocument xmlShenlongColumn;
				MakeQueryColumnXml(out xmlShenlongColumn, null);

				if ( (modified) && ((xmlShenlongColumnFileName != null) || !reloadLastColumnsOnStartup) )
				{
					DialogResult dialogResult = MyMessageBox.Show("編集中のクエリー項目をファイルに保存しますか？", appTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if ( dialogResult == DialogResult.Yes )
					{
						//xmlShenlongColumn.Save(xmlShenlongColumnFileName);
						SaveShenlongColumnFile(CheckShenlongColumnFileExtension(xmlShenlongColumnFileName), xmlShenlongColumn);
						ChangeModified(false);
					}
					else if ( dialogResult == DialogResult.Cancel )
					{
						e.Cancel = true;
						return;
					}
				}

				if ( Program.isNewInstance )
				{
					// 最後の状態をファイルに保存しておく
					//xmlShenlongColumn.Save(Application.StartupPath + "\\" + xmlLatestQueryColumnFileName);
					SaveShenlongColumnFile(Application.StartupPath + "\\" + xmlLatestQueryColumnFileName, xmlShenlongColumn);

					// アプリケーションの状態を保存する
					// ファイル ダイアログの初期ディレクトリ
					api.WritePrivateProfileString(RESUME_SECTION, KEY_FILEDLG_INI_DIR, fileDlgIniDir, shenlongIniFileName);

					// ファイル ダイアログのフィルター インデックス
					api.WritePrivateProfileString(RESUME_SECTION, KEY_FILEDLG_FILTER_INDEX, fileDlgFilterIndex.ToString(), shenlongIniFileName);

					// 最近使ったファイル名
					int i;
					for ( i = 1; i <= recentFileNames.Count; i++ )
					{
						api.WritePrivateProfileString(RESUME_SECTION, KEY_RECENT_FILENAME + i, recentFileNames[i - 1].ToString(), shenlongIniFileName);
					}
					for ( ; i <= maxRecentFileName; i++ )
					{
						api.WritePrivateProfileString(RESUME_SECTION, KEY_RECENT_FILENAME + i, null, shenlongIniFileName);
					}

					Point formPoint = this.Location;
					if ( this.WindowState == FormWindowState.Normal/*0 <= formPoint.X && 0 <= formPoint.Y*/ )
					{
						// KEY_WINDOW_RECTANGLE
						api.WritePrivateProfileString(RESUME_SECTION, KEY_WINDOW_RECTANGLE, formPoint.X + "," + formPoint.Y + "," + this.Size.Width + "," + this.Size.Height, shenlongIniFileName);
						// KEY_SPILITTER_DISTANCE
						api.WritePrivateProfileString(RESUME_SECTION, KEY_SPILITTER_DISTANCE, splitContainerTable.SplitterDistance.ToString(), shenlongIniFileName);
					}

					// KEY_SELECTED_TAB_PAGE_TEXT
					api.WritePrivateProfileString(RESUME_SECTION, KEY_SELECTED_TAB_PAGE_TEXT, tabControl.SelectedTab.Text, shenlongIniFileName);

					// KEY_CUSTOM_TABLE_SELECT
					api.WritePrivateProfileString(RESUME_SECTION, KEY_CUSTOM_TABLE_SELECT, toolStripCustomTableSelect.Checked.ToString().ToLower(), shenlongIniFileName);

					IntPtr hWnd;
					// ヘルプが開かれていれば閉じる
					if ( (hWnd = api.FindWindow(null, "shenlong document")) != IntPtr.Zero )
					{
						api.PostMessage(hWnd, api.WM_CLOSE, 0, 0);
					}
				}

				CloseOraConn();
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目ファイルのドラッグが開始された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if ( e.Data.GetDataPresent(DataFormats.FileDrop) )
				{
					string[] sourceFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
					string extension = Path.GetExtension(sourceFileNames[0]).ToLower();
					if ( (extension == ".xml") || (extension == ".sql") )
					{
						e.Effect = DragDropEffects.Copy;
						return;
					}
				}

				e.Effect = DragDropEffects.None;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目ファイルがドラッグされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				object obj = e.Data.GetData(DataFormats.FileDrop);
				string[] sourceFileNames = (string[])obj;

				if ( string.Compare(Path.GetExtension(sourceFileNames[0]), ".xml", true) == 0 )
				{
					ReadShenlongColumnFile(sourceFileNames[0], false);
				}
				else
				{
					ReadSqlFile(sourceFileNames[0], false);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Shenlong_KeyDown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_KeyDown(object sender, KeyEventArgs e)
		{
			// listBoxTableList のイベントで拾っていたが、別のコントロールにフォーカスがある状態で、
			// Shift キーを押しながら listBoxTableList をクリックするとイベントが拾えないので、フォームで拾うようにした。
			formKeyDownArgs = e;
		}

		/// <summary>
		/// Shenlong_KeyUp
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shenlong_KeyUp(object sender, KeyEventArgs e)
		{
			//Debug.WriteLine("KeyUp");
			formKeyDownArgs = null;

			if ( e.KeyCode == Keys.F5 )
			{
				StartQueryPasteExcel((e.Shift ? !showParamInputDlg : showParamInputDlg));
			}
		}

		#region メニュー関連のメソッド
		/// <summary>
		/// メニューがクリックされた
		/// </summary>
		private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				lveQueryColumn.EndEditing(false);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [新規作成(N)] メニュー
		/// </summary>
		private void toolStripMenuNew_Click(object sender, EventArgs e)
		{
			try
			{
				if ( (lveQueryColumn.Columns.Count == 0) && (textSQL.Text.Length == 0) )
					return;

				if ( modified )
				{
					if ( MyMessageBox.Show("編集中のクエリー項目を初期化しますか？", appTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes )
						return;
				}

#if ENABLED_SUBQUERY
				RemoveSubQueryFromTableList();
#endif

				ClearQueryColumn();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [開く(O)...] メニュー
		/// </summary>
		private void toolStripMenuOpen_Click(object sender, EventArgs e)
		{
			try
			{
				//openFileDialog.Reset();	// こうしないと前回選択したディレクトリが有効になってしまう？
				openFileDialog.RestoreDirectory = true;
				openFileDialog.InitialDirectory = fileDlgIniDir;
				openFileDialog.FilterIndex = fileDlgFilterIndex;
				if ( openFileDialog.ShowDialog(this) != DialogResult.OK )
					return;

				fileDlgIniDir = Path.GetDirectoryName(openFileDialog.FileName);
				fileDlgFilterIndex = openFileDialog.FilterIndex;

				if ( string.Compare(Path.GetExtension(openFileDialog.FileName), ".xml", true) == 0 )
				{
					ReadShenlongColumnFile(openFileDialog.FileName, true);
				}
				else
				{
					ReadSqlFile(openFileDialog.FileName, true);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [上書き保存(S)] メニュー
		/// </summary>
		private void toolStripMenuSave_Click(object sender, EventArgs e)
		{
			try
			{
				if ( (lveQueryColumn.Columns.Count == 0) && (textSQL.Text.Length == 0) )
					return;

				XmlDocument xmlShenlongColumn;
				if ( !MakeQueryColumnXml(out xmlShenlongColumn, null) )
					return;

				//xmlShenlongColumn.Save(Application.StartupPath + "\\" + xmlTempQueryColumnFileName : xmlShenlongColumnFileName);
				string saveFileName = (xmlShenlongColumnFileName == null) ? Application.StartupPath + "\\" + xmlTempQueryColumnFileName : xmlShenlongColumnFileName;
				saveFileName = CheckShenlongColumnFileExtension(saveFileName);
				SaveShenlongColumnFile(saveFileName, xmlShenlongColumn);

				ChangeModified(false);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// [名前を付けて保存(A)...] メニュー
		/// </summary>
		private void toolStripMenuSaveAs_Click(object sender, EventArgs e)
		{
			try
			{
				//saveFileDialog.Reset();	// こうしないと前回選択したディレクトリが有効になってしまう？
				saveFileDialog.RestoreDirectory = true;
				saveFileDialog.InitialDirectory = fileDlgIniDir;
				if ( saveFileDialog.ShowDialog(this) != DialogResult.OK )
					return;

				string saveFileName = CheckShenlongColumnFileExtension(saveFileDialog.FileName);

				fileDlgIniDir = Path.GetDirectoryName(saveFileName);
				fileDlgFilterIndex = 1;

				XmlDocument xmlShenlongColumn;
				if ( !MakeQueryColumnXml(out xmlShenlongColumn, null) )
					return;

				//xmlShenlongColumn.Save(saveFileName);
				SaveShenlongColumnFile(saveFileName, xmlShenlongColumn);

				ChangeModified(false);

				ShenlongColumnFileNameManager(saveFileName, true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [最近使ったファイル名] メニュー
		/// </summary>
		private void toolStripMenuRecentFileName_Click(object sender, EventArgs e)
		{
			try
			{
				string fileName = ((ToolStripMenuItem)sender).ToolTipText;
				if ( !File.Exists(fileName) )
				{
					MyMessageBox.Show("選択されたファイルは見つかりませんでした\r\n最近使ったファイル名の一覧から削除します", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

					recentFileNames.Remove(fileName);
					RefreshRecentFileNameMenu();
					return;
				}

				if ( string.Compare(Path.GetExtension(fileName), ".xml", true) == 0 )
				{
					ReadShenlongColumnFile(fileName, true);
				}
				else
				{
					ReadSqlFile(fileName, true);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [終了(X)] メニュー
		/// </summary>
		private void toolStripMenuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// [編集(E)] メニューが開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuEdit_DropDownOpening(object sender, EventArgs e)
		{
			toolStripMenuCutQueryColumn.Enabled = (lveQueryColumn.Items.Count != 0);
			toolStripMenuCopyQueryColumn.Enabled = (lveQueryColumn.Items.Count != 0);
			toolStripMenuPasteQueryColumn.Enabled = (xmlCopiedShenlongColumn != null);
		}

		/// <summary>
		/// [クエリー項目でSQLを構築(&S)] メニュー
		/// </summary>
		private void toolStripMenuBuildQueryColumnSQL_Click(object sender, EventArgs e)
		{
			try
			{
				ShenGlobal.InitLogMessage();

				Dictionary<string, string> _selectParams = null;
				string buildedSql, columnComments;
				List<string> fromTableNames = new List<string>();
				if ( !BuildQueryColumnSQL(_selectParams, out buildedSql, out columnComments, ref fromTableNames) )
					return;

				tabControl.SelectedTab = tabSQL;

				textSQL.Text = buildedSql;
				textSQL.SelectionStart = textSQL.Text.Length;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
			finally
			{
#if (DEBUG)
				string logFileName = Application.StartupPath + "\\" + "~shenlong.log";
				ShenGlobal.SaveLogMessage(logFileName);
#endif
			}
		}

		/// <summary>
		/// [クエリー項目を切り取り(T)...] メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuCutQueryColumn_Click(object sender, EventArgs e)
		{
			try
			{
				XmlDocument xmlShenlongColumn;
				if ( !MakeQueryColumnXml(out xmlShenlongColumn, null) )
					return;

				CopyQueryColumnDlg copyQueryColumnDlg = new CopyQueryColumnDlg(xmlShenlongColumn, CopyQueryColumnDlg.modes.cut);
				if ( copyQueryColumnDlg.ShowDialog(this) != DialogResult.OK )
					return;

				if ( copyQueryColumnDlg.xmlCopiedShenlongColumn == null )
					return;

				xmlCopiedShenlongColumn = copyQueryColumnDlg.xmlCopiedShenlongColumn;

				int[] colOrder = lveQueryColumn.GetColumnOrder();

				List<int> indexes = new List<int>();

				foreach ( XmlNode column in xmlCopiedShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn) )
				{
					int index = int.Parse(column.Attributes[CopyQueryColumnDlg.attrIndex].Value);
					indexes.Add(colOrder[index]);
				}

				indexes.Sort();

				for ( int i = indexes.Count - 1; 0 <= i; i-- )
				{
					RemoveQueryColumn(indexes[i]);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [クエリー項目をコピー(C)...] メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuCopyQueryColumn_Click(object sender, EventArgs e)
		{
			try
			{
				XmlDocument xmlShenlongColumn;
				if ( !MakeQueryColumnXml(out xmlShenlongColumn, null) )
					return;

				CopyQueryColumnDlg copyQueryColumnDlg = new CopyQueryColumnDlg(xmlShenlongColumn, CopyQueryColumnDlg.modes.copy);
				if ( copyQueryColumnDlg.ShowDialog(this) != DialogResult.OK )
					return;

				if ( copyQueryColumnDlg.xmlCopiedShenlongColumn == null )
					return;

				xmlCopiedShenlongColumn = copyQueryColumnDlg.xmlCopiedShenlongColumn;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [クエリー項目へ貼り付け(P)] メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuPasteQueryColumn_Click(object sender, EventArgs e)
		{
#if true
			PasteCopyShenlongColumn(-1);
#else
			try
			{
				// クエリー項目
				foreach ( XmlNode column in xmlCopyShenlongColumn.DocumentElement.SelectNodes(tagColumn) )
				{
					if ( HasQueryColumn(column.Attributes[attrTableName].Value, column[ShenCore.qc.fieldName.ToString()].InnerText, 0x0002) != -1 )	// 既に選択済み？
						continue;

					string[] subItemText = QueryColumnNodeToStrings(column);

					if ( AddQueryColumn(column.Attributes[attrTableName].Value, int.Parse(column.Attributes[attrWidth].Value), subItemText, -1/*true*/) != 1 )
						break;

					ChangeModified(true);
				}

				// テーブル結合
				foreach ( XmlNode tableJoin in xmlCopyShenlongColumn.DocumentElement.SelectNodes(tagTableJoin) )
				{
					if ( HasTableJoin(tableJoin.Attributes[ShenCore.tabJoin.leftTabCol.ToString()].Value, tableJoin.Attributes[ShenCore.tabJoin.rightTabCol.ToString()].Value, 2) != -1 )
						continue;

					ListViewItem lvi = new ListViewItem(tableJoin.Attributes[ShenCore.tabJoin.leftTabCol.ToString()].Value);
					lvi.SubItems.Add(tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value);
					lvi.SubItems.Add(tableJoin.Attributes[ShenCore.tabJoin.rightTabCol.ToString()].Value);
					lvTableJoin.Items.Add(lvi);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
#endif
		}

		/// <summary>
		/// コピーされたクエリー項目を貼り付ける
		/// </summary>
		/// <param name="index"></param>
		private void PasteCopyShenlongColumn(int index)
		{
			try
			{
				// クエリー項目
				foreach ( XmlNode column in xmlCopiedShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn) )
				{
					if ( !enableSameColumnAppend && (HasQueryColumn(column.Attributes[ShenGlobal.attrTableName].Value, column[ShenGlobal.qc.fieldName.ToString()].InnerText, 0x0002) != -1) )	// 既に選択済み？
						continue;

					string[] subItemText = QueryColumnNodeToStrings(column);

					if ( AddQueryColumn(column.Attributes[ShenGlobal.attrTableName].Value, int.Parse(column.Attributes[ShenGlobal.attrWidth].Value), subItemText, index/*true*/) != 1 )
						break;

					ChangeModified(true);
				}

				// テーブル結合
				foreach ( XmlNode tableJoin in xmlCopiedShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagTableJoin) )
				{
					if ( HasTableJoin(tableJoin.Attributes[ShenGlobal.tabJoin.leftTabCol.ToString()].Value, tableJoin.Attributes[ShenGlobal.tabJoin.rightTabCol.ToString()].Value, 2) != -1 )
						continue;

					ListViewItem lvi = new ListViewItem(tableJoin.Attributes[ShenGlobal.tabJoin.leftTabCol.ToString()].Value);
					lvi.SubItems.Add(tableJoin.Attributes[ShenGlobal.tabJoin.way.ToString()].Value);
					lvi.SubItems.Add(tableJoin.Attributes[ShenGlobal.tabJoin.rightTabCol.ToString()].Value);
					lvTableJoin.Items.Add(lvi);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [ファイルのプロパティ...(R)] メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuFileProperty_Click(object sender, EventArgs e)
		{
			try
			{
				FilePropertyDlg filePropertyDlg = new FilePropertyDlg();
				filePropertyDlg.comment = fileComment;
				filePropertyDlg.author = fileAuthor;
				filePropertyDlg.distinct = fileDistinct;
				filePropertyDlg.useJoin = fileUseJoin;
				filePropertyDlg.headerOutput = fileHeaderOutput;
				filePropertyDlg.download = fileDownLoad;
				filePropertyDlg.eggPermission = fileEggPermission;
				filePropertyDlg.maxRowNum = fileMaxRowNum;
				filePropertyDlg.setValue = fileSetValue;
				filePropertyDlg.sqlSelect = fileSqlSelect;
#if ENABLED_SUBQUERY
				filePropertyDlg.subQueries = new List<string>(fileSubQuery)/*fileSubQuery*/;
				filePropertyDlg.xmlShenlongColumnFileName = xmlShenlongColumnFileName;
#endif

				if ( filePropertyDlg.ShowDialog(this) != DialogResult.OK )
					return;

				fileComment = filePropertyDlg.comment;
				fileAuthor = filePropertyDlg.author;
				fileDistinct = filePropertyDlg.distinct;
				fileUseJoin = filePropertyDlg.useJoin;
				fileHeaderOutput = filePropertyDlg.headerOutput;
				fileDownLoad = filePropertyDlg.download;
				fileEggPermission = filePropertyDlg.eggPermission;
				fileMaxRowNum = filePropertyDlg.maxRowNum;
				fileSetValue = filePropertyDlg.setValue;
				fileSqlSelect = filePropertyDlg.sqlSelect;
#if ENABLED_SUBQUERY
				fileSubQuery = filePropertyDlg.subQueries;

				RemoveSubQueryFromTableList();

				foreach ( string subQuery in fileSubQuery )
				{
					AppendSubQueryToTableList(subQuery);
				}
#endif

				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [ログオン(L)...] メニュー
		/// </summary>
		private void toolStripMenuLogOn_Click(object sender, EventArgs e)
		{
			if ( lveQueryColumn.Columns.Count != 0 )
			{
				lveQueryColumn.EndEditing(false);
			}

			if ( OraLogOn(LogOnDlg.usages.manual, null, null) != oraon.success )
				return;

			SelectTableName();
		}

		/// <summary>
		/// [Excel へ貼付(E)] メニュー
		/// </summary>
		private void toolStripMenuToExcel_Click(object sender, EventArgs e)
		{
			StartQueryPasteExcel(showParamInputDlg);
		}

		/// <summary>
		/// [オプション(O)...] メニュー
		/// </summary>
		private void toolStripMenuOption_Click(object sender, EventArgs e)
		{
			OptionDlg optionDlg = new OptionDlg();
			if ( optionDlg.ShowDialog(this) != DialogResult.OK )
				return;

			if ( selectColumnByDragDrop != optionDlg.checkSelectColumnByDragDrop.Checked )
			{
				selectColumnByDragDrop = optionDlg.checkSelectColumnByDragDrop.Checked;
				ChangeColumnSelection();

				//showParamInputDlg = optionDlg.checkShowParamInputDlg.Checked;
				//toolStripShowParamInputDlg.Checked = showParamInputDlg;
			}

			if ( Program.expertMode )
			{
				if ( !(toolStripCustomTableSelect.Enabled = CustomTableSelectEnabled()) )
				{
					toolStripCustomTableSelect.Checked = false;
				}

				listBoxColumnList.Refresh();
				lveQueryColumn.Refresh();
			}
		}

		/// <summary>
		/// [目次] メニュー
		/// </summary>
		private void toolStripMenuContents_Click(object sender, EventArgs e)
		{
			try
			{
				string localShenlongChmFile = Application.StartupPath + @"\shenlong.chm";
#if true
				System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
				string now = DateTime.Now.ToString("yyyy/MM/dd tt", cultureInfo);
				StringBuilder returnedString = new StringBuilder(1024);

				try
				{
					api.GetPrivateProfileString(RESUME_SECTION, KEY_LAST_HELP_FILE_CHECK, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
					if ( (returnedString.Length == 0) || (returnedString.ToString() != now) )
					{
						string remoteShenlongChmFile = latestProgramFolder + "shenlong.chm";
						DateTime remoteShenlongChmWriteTime = File.GetLastWriteTime(remoteShenlongChmFile);
						DateTime localShenlongChmWriteTime = (File.Exists(localShenlongChmFile)) ? File.GetLastWriteTime(localShenlongChmFile) : DateTime.Parse("2007/11/01");
						if ( localShenlongChmWriteTime < remoteShenlongChmWriteTime )
						{
							File.Copy(remoteShenlongChmFile, localShenlongChmFile, true);
						}
						api.WritePrivateProfileString(RESUME_SECTION, KEY_LAST_HELP_FILE_CHECK, now, shenlongIniFileName);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
#if (DEBUG)
					MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
				}
#endif

#if (DEBUG)
				localShenlongChmFile = @"C:\Users\Hidetatsu\Documents\RoboHTML\" + Application.ProductName + "\\" + Application.ProductName + ".chm";
#endif
				Process.Start(localShenlongChmFile);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [バージョン情報(A)...] メニュー
		/// </summary>
		private void toolStripMenuAbout_Click(object sender, EventArgs e)
		{
			AboutDlg aboutDlg = new AboutDlg();
			aboutDlg.ShowDialog(this);
		}

		/// <summary>
		/// [抽出条件を入力] がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripShowParamInputDlg_Click(object sender, EventArgs e)
		{
			try
			{
				showParamInputDlg = !showParamInputDlg;
				toolStripShowParamInputDlg.Checked = showParamInputDlg;

				string appName = Process.GetCurrentProcess().ProcessName;
#if (DEBUG)
				appName = Shenlong.appTitle;	// デバッグでは shenlong.vshost.exe.config となっているので書き換える
#endif
				AppConfig appConfig = new AppConfig(appName);
				appConfig.SetValue(Program.CONSET_SHOW_PARAM_INPUT_DLG, showParamInputDlg.ToString().ToLower());
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [ドラッグ＆ドロップで項目を選択] がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripSelectColumnDD_Click(object sender, EventArgs e)
		{
			try
			{
				selectColumnByDragDrop = !selectColumnByDragDrop;

				ChangeColumnSelection();

				string appName = Process.GetCurrentProcess().ProcessName;
#if (DEBUG)
				appName = Shenlong.appTitle;	// デバッグでは shenlong.vshost.exe.config となっているので書き換える
#endif
				AppConfig appConfig = new AppConfig(appName);
				appConfig.SetValue(Program.CONSET_SELECT_COLUMN_BY_DRAG_DROP, selectColumnByDragDrop.ToString().ToLower());
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [右端の項目を削除] がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripRemoveEndColumn_Click(object sender, EventArgs e)
		{
			try
			{
				int columnCount = lveQueryColumn.Columns.Count;
				if ( columnCount == 0 )
					return;

				lveQueryColumn.EndEditing(false);

				int[] colOrder = lveQueryColumn.GetColumnOrder();
				RemoveQueryColumn(colOrder[columnCount - 1]);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [重複項目の追加を許可] がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripEnableSameColumnAppend_Click(object sender, EventArgs e)
		{
			SetEnableSameColumnAppend(!enableSameColumnAppend);
		}

		/// <summary>
		/// 重複して項目を追加できるか否かを設定する
		/// </summary>
		/// <param name="status"></param>
		private void SetEnableSameColumnAppend(bool enable)
		{
			try
			{
				enableSameColumnAppend = enable;
				toolStripEnableSameColumnAppend.Checked = enableSameColumnAppend;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// contextMenuTableList が開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuTableList_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				toolStripMenuSortTable.Enabled = (textTableFilter.Text.Length == 0);
				toolStripMenuSortTable.Text = "並び替え（現在:" + (ascendingTableName ? "昇順" : "降順") + "）";
				toolStripMenuSortTableComment.Enabled = hasTableComments;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [並べ替え] - [テーブル名]|[コメント] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolStripMenuSortTableName_Click(object sender, EventArgs e)
		{
			try
			{
				ToolStripMenuItem menuItemSortTable = (ToolStripMenuItem)sender;
				int sortColumn = 0;
				if ( menuItemSortTable.Name == "toolStripMenuSortTableName" )			// テーブル名で並び替え
				{
					sortColumn = 1;
					toolStripMenuSortTableComment.Checked = false;
				}
				else if ( menuItemSortTable.Name == "toolStripMenuSortTableComment" )	// コメントで並び替え
				{
					sortColumn = 2;
					toolStripMenuSortTableName.Checked = false;
				}

				if ( !menuItemSortTable.Checked )
				{
					menuItemSortTable.Checked = true;
				}
				else
				{
					ascendingTableName = !ascendingTableName;
				}

				Cursor.Current = Cursors.WaitCursor;

				List<string> tables;
				int maxTableName;
				if ( SortTableName(sortColumn, out tables, out maxTableName) )
				{
					listBoxTableList.Items.Clear();
					listBoxColumnList.Items.Clear();

					// リストボックスにテーブル名を追加する
					SetTableName(tables, maxTableName);
				}

				Cursor.Current = Cursors.Default;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [最新の情報に更新] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuRefreshTableList_Click(object sender, EventArgs e)
		{
			try
			{
				string selectedItem = (string)listBoxTableList.SelectedItem;
				int topIndex = listBoxTableList.TopIndex;

				selTableHistory = new List<int>();
				curSelTableHistory = -1;

				textColumnFilter.Text = string.Empty;
				textTableFilter.Text = string.Empty;
				listBoxColumnList.Items.Clear();
				listBoxColumnList.Update();
				listBoxTableList.Items.Clear();
				listBoxTableList.Update();
				//listBoxTableList.BeginUpdate();

				SelectTableName();

				foreach ( string subQuery in fileSubQuery )
				{
					AppendSubQueryToTableList(subQuery);
				}

				if ( selectedItem != null )
				{
					listBoxTableList.SelectedItem = selectedItem;
					listBoxTableList.TopIndex = topIndex;
				}

				//listBoxTableList.EndUpdate();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// contextMenuColumnList が開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuColumnList_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				bool enabled = listBoxColumnList.Items.Count != 0;
				toolStripMenuSelectAll.Enabled = enabled;
				toolStripMenuShowIndex.Enabled = enabled;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [全て選択] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuSelectAll_Click(object sender, EventArgs e)
		{
			try
			{
				listBoxColumnList.ClearSelected();
				for ( int i = 0; i < listBoxColumnList.Items.Count - 1; i++ )
				{
					listBoxColumnList.SelectedIndex = i;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [インデックス] のサブメニューが開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuShowIndex_DropDownOpening(object sender, EventArgs e)
		{
			OracleCommand oraCmd = null;
			OracleDataReader oraReader = null;

			try
			{
				if ( listBoxTableList.Text.Length == 0 )
					return;

				Cursor.Current = Cursors.WaitCursor;

#if UPDATE_20140729
				string tableOwner = GetListBoxTableOwner();
				string tableName = GetListBoxTableName(selTbl.plainTblName), sql;

				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "']";
				if ( tableOwner != null )
				{
					xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "'" + " and @" + attrOwner + "='" + tableOwner + "']";
				}
#else
				string tableName = GetListBoxTableName(selTbl.plainTblName), sql;

				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "']";
#endif
				XmlNode table = xmlTableList.SelectSingleNode(xpath);

				bool dbLink = (table.Attributes[attrDbLink] != null) && !string.IsNullOrEmpty(table.Attributes[attrDbLink].Value);
				bool synonym = (string.Compare(table.Attributes[attrType].Value, "SYNONYM", true) == 0) && !dbLink;

#if UPDATE_20160316
				if ( synonym )
				{
					// サブクエリで取得する
					string sqlUSER_SYNONYMS =
						"(select * from user_synonyms where synonym_name = '" + tableName + "') user_synonyms";
					sql =
						"select\r\n" +
						" all_ind_columns.*,\r\n" +
						" all_constraints.constraint_type,\r\n" +
						" all_constraints.status\r\n" +
						"from\r\n" +
						" all_ind_columns\r\n" +
						" inner join " + sqlUSER_SYNONYMS + "\r\n" +
						" on (all_ind_columns.table_owner = user_synonyms.table_owner and all_ind_columns.table_name = user_synonyms.table_name)\r\n" +
						" left outer join all_constraints all_constraints\r\n" +
						"  on (all_ind_columns.table_name = all_constraints.table_name and all_ind_columns.index_name = all_constraints.constraint_name)\r\n" +
						"order by\r\n" +
						" all_ind_columns.index_name,\r\n" +
						" all_ind_columns.column_position";
				}
				else
				{
					string _dbLink = (dbLink) ? ("@" + table.Attributes[attrDbLink].Value) : "";

					sql = "select\r\n" +
						  " user_ind_columns.*,\r\n" +
						  " user_constraints.constraint_type,\r\n" +
						  " user_constraints.status\r\n" +
						  "from\r\n" +
						  " user_ind_columns" + _dbLink + " user_ind_columns\r\n" +
						  " left outer join user_constraints" + _dbLink + " user_constraints\r\n" +
						  "  on (user_ind_columns.table_name = user_constraints.table_name and user_ind_columns.index_name = user_constraints.constraint_name)\r\n" +
						  "where\r\n" +
						  " user_ind_columns.table_name = '" + tableName + "'\r\n" +
						  "order by\r\n" +
						  " user_ind_columns.index_name,\r\n" +
						  " user_ind_columns.column_position";

					if ( Program.expertMode && toolStripCustomTableSelect.Checked )
					{
						if ( tableOwner != null )
						{
							sql = "select\r\n" +
								  " all_ind_columns.*,\r\n" +
								  " all_constraints.constraint_type,\r\n" +
								  " all_constraints.status\r\n" +
								  "from\r\n" +
								  " all_ind_columns" + _dbLink + " all_ind_columns\r\n" +
								  " left outer join all_constraints" + _dbLink + " all_constraints\r\n" +
								  "  on (all_ind_columns.table_name = all_constraints.table_name and all_ind_columns.index_name = all_constraints.constraint_name)\r\n" +
								  "where\r\n" +
								  " all_ind_columns.table_name = '" + tableName + "'\r\n" +
								  " and all_ind_columns.table_owner='" + tableOwner + "' " +
								  "order by\r\n" +
								  " all_ind_columns.index_name,\r\n" +
								  " all_ind_columns.column_position";
						}
					}
				}
#else
				if ( synonym )
				{
					// サブクエリで取得する
					string sqlUSER_SYNONYMS =
						"(SELECT * FROM USER_SYNONYMS WHERE SYNONYM_NAME = '" + tableName + "') USER_SYNONYMS ";
					sql =
						"SELECT" +
						" ALL_IND_COLUMNS.* " +
						"FROM" +
						" ALL_IND_COLUMNS," + sqlUSER_SYNONYMS +
						"WHERE" +
						" ALL_IND_COLUMNS.TABLE_OWNER=USER_SYNONYMS.TABLE_OWNER AND" +
						" ALL_IND_COLUMNS.TABLE_NAME=USER_SYNONYMS.TABLE_NAME " +
						"ORDER BY" +
						" ALL_IND_COLUMNS.INDEX_NAME,ALL_IND_COLUMNS.COLUMN_POSITION";
				}
				else
				{
					string _dbLink = (dbLink) ? ("@" + table.Attributes[attrDbLink].Value) : "";

					sql = "SELECT * " +
						  "FROM USER_IND_COLUMNS" + _dbLink + " " +
						  "WHERE TABLE_NAME='" + tableName + "' " +
						  "ORDER BY INDEX_NAME,COLUMN_POSITION";
#if UPDATE_20140729
					if ( Program.expertMode && toolStripCustomTableSelect.Checked )
					{
						if ( tableOwner != null )
						{
							sql = "SELECT * " +
								  "FROM ALL_IND_COLUMNS" + _dbLink + " " +
								  "WHERE TABLE_NAME='" + tableName + "' " +
								  " AND TABLE_OWNER='" + tableOwner + "' " +
								  "ORDER BY INDEX_NAME,COLUMN_POSITION";
						}
					}
#endif
				}
#endif

				oraCmd = new OracleCommand(sql, oraConn);
				oraReader = oraCmd.ExecuteReader();

				StringBuilder indexes = new StringBuilder();
				string lastIndexName = "";
				if ( oraReader.HasRows )
				{
					StringBuilder secondaryKey = new StringBuilder();
					while ( oraReader.Read() )
					{
						string indexName = oraReader["INDEX_NAME"].ToString();
						string columnName = oraReader["COLUMN_NAME"].ToString();
#if UPDATE_20160316
						string constraint_type = oraReader["constraint_type"].ToString();
						string status = oraReader["status"].ToString();

						if ( lastIndexName != indexName )
						{
							string index = "+ " + indexName;
							if ( constraint_type == "P")
							{
								index += " (*" + (status == "ENABLED" ? "" : "d") + ")";
								indexes.Append(index + "\r\n");
							}
							else
							{
								secondaryKey.Append(index + "\r\n");
							}

							lastIndexName = indexName;
						}

						string column = "　- " + columnName + "\r\n";
						if ( constraint_type == "P" )
						{
							indexes.Append(column);
						}
						else
						{
							secondaryKey.Append(column);
						}
#else
						if ( lastIndexName != indexName )
						{
							string index = "+ " + indexName + "\r\n";
							if ( indexName.StartsWith("PK_") ) indexes.Append(index); else secondaryKey.Append(index);
							lastIndexName = indexName;
						}
						string column = "　- " + columnName + "\r\n";
						if ( indexName.StartsWith("PK_") ) indexes.Append(column); else secondaryKey.Append(column);
#endif
					}

					indexes.Append(secondaryKey);
					indexes.Length -= 2;
				}
				else
				{
					indexes.Append("登録無し");
				}

				((ToolStripDropDownMenu)toolStripMenuShowIndex.DropDown).ShowImageMargin = false;
				toolStripMenuIndex.Text = indexes.ToString();

#if (DEBUG)
				try
				{
					string indexKeyFileName = Application.StartupPath + "\\" + "~indexkey.txt";
					using ( StreamWriter swIndexKeyFile = new StreamWriter(indexKeyFileName, false, Encoding.Default) )
					{
						swIndexKeyFile.WriteLine(tableName);
						swIndexKeyFile.WriteLine(indexes.ToString());
						swIndexKeyFile.Close();
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
				}
#endif
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			finally
			{
				if ( oraReader != null )
				{
					oraReader.Close();
					oraReader.Dispose();
					oraReader = null;
				}

				if ( oraCmd != null )
				{
					oraCmd.Dispose();
					oraCmd = null;
				}

				Cursor.Current = Cursors.Default;
			}
		}
#endregion

		/// <summary>
		/// オラクルにログオンする
		/// </summary>
		/// <param name="usage"></param>
		/// <param name="sid"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		private oraon OraLogOn(LogOnDlg.usages usage, string sid, string userName)
		{
			try
			{
				if ( (usage == LogOnDlg.usages.require) && (sid == oraConnNone) )
					return oraon.none;

				LogOnDlg logOnDlg = new LogOnDlg(usage, sid, userName, maxLogOnHistoryCount);
				if ( logOnDlg.ShowDialog(this) != DialogResult.OK )
					return oraon.cancel;

				CloseOraConn();
				toolStripStatusOraConn.Text = oraConnStatusSplitter + oraConnNone/*oraConnNone + oraConnStatusSplitter*/;
				toolStripStatusOraConn.Tag = null;
				toolStripStatusOraConn.ToolTipText = "";

				textTableFilter.Text = string.Empty;
				textTableFilter.Update();
				listBoxTableList.Items.Clear();
				textColumnFilter.Text = string.Empty;
				textColumnFilter.Update();
				listBoxColumnList.Items.Clear();
				if ( clearQueryColumnWhenOraLogOn )
				{
					ClearQueryColumn();
				}

				oraConn = logOnDlg.oraConn;
				//toolStripToExcel.Enabled = true;	// 入力された SQL を実行できるようにするため
				//toolStripStatusOraConn.Text = logOnDlg.textSID.Text + " " + oraConnStatusSplitter + " " + logOnDlg.comboUserName.Text;
				toolStripStatusOraConn.Text = logOnDlg.comboUserName.Text + oraConnStatusSplitter + logOnDlg.textSID.Text;
				toolStripStatusOraConn.Tag = logOnDlg.textPassword.Text;
				toolStripStatusOraConn.ToolTipText = oraConn.ServerVersion.Split('\n')[0];

				selTableHistory = new List<int>();
				curSelTableHistory = -1;

				labelTableList.Focus();

				return oraon.success;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return oraon.exception;
			}
		}

		/// <summary>
		/// CloseOraConn
		/// </summary>
		private void CloseOraConn()
		{
			if ( oraConn == null )
				return;

			if ( oraConn.State == ConnectionState.Open )
			{
				oraConn.Close();
			}
			oraConn.Dispose();
			oraConn = null;
		}

		/// <summary>
		/// クエリー項目を初期化する
		/// </summary>
		private void ClearQueryColumn()
		{
			try
			{
				ClearContextTableJoinColumns();

				textSQL.Text = "";

				if ( (lveQueryColumn.Columns.Count == 0) && (textSQL.Text.Length == 0) )
					return;

				ToolStripMenuEnable(false);

				xmlShenlongColumnFileName = null;
				InitFileProperty();
				toolStripStatusFileName.Text = "--";
				toolStripStatusFileName.ToolTipText = "";
				toolStripStatusColumnCount.Text = "0";

				//listBoxTableList.SelectedIndex = -1;

				lveQueryColumn.Columns.Clear();
				lveQueryColumn.Items.Clear();
				queryTableNames = new List<string>();
				lastQueryColumn = -1;
				ChangeModified(false);

				lvTableJoin.Items.Clear();

				lveQueryColumn.Select();

				latestSelectParams = null;

				SaveBaseURI(null);

				SetEnableSameColumnAppend(false);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// ToolStripMenuEnable
		/// </summary>
		/// <param name="enabled"></param>
		private void ToolStripMenuEnable(bool enable)
		{
			//bool enabled = (lveQueryColumn.Columns.Count != 0);
			toolStripMenuNew.Enabled = enable;
			toolStripMenuSave.Enabled = enable;
			toolStripMenuSaveAs.Enabled = enable;

			toolStripMenuBuildQueryColumnSQL.Enabled = enable;
			toolStripMenuFileProperty.Enabled = enable;

			//ToolStripMenuToExcel.Enabled = (enable || (textSQL.Text.Length != 0));
			toolStripMenuToExcel.Enabled = enable;

			toolStripNew.Enabled = enable;
			toolStripSave.Enabled = enable;

			toolStripToExcel.Enabled = enable;
			toolStripRemoveEndColumn.Enabled = enable;
		}

		/// <summary>
		/// テーブル名の一覧を取得する
		/// </summary>
		private void SelectTableName()
		{
			OracleCommand oraCmd = null;
			OracleDataReader oraReader = null;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				ascendingTableName = true;
				hasTableComments = false;
				toolStripMenuSortTableName.Checked = true;
				toolStripMenuSortTableComment.Checked = false;

				xmlTableList = new XmlDocument();
				XmlDeclaration decl = xmlTableList.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlTableList.AppendChild(decl);

				XmlNode tableList = xmlTableList.CreateNode(XmlNodeType.Element, tagTableList, null);	// <tableList>
				xmlTableList.AppendChild(tableList);

				List<string> tables = new List<string>();
				int maxTableName = 0;

#if true
				// TABLE, VIEW のテーブル名を取得する
				string sqlTableName =
					"(SELECT TAB.TNAME,TAB.TABTYPE" +
					" FROM TAB,USER_TABLES" +
					" WHERE (TAB.TNAME = USER_TABLES.TABLE_NAME(+))" +
					"  AND (TAB.TABTYPE IN ('VIEW','TABLE'))" +
					"  AND (INSTR(TAB.TNAME,'$') = 0)) SUB ";
				string sql =
					"SELECT SUB.TNAME,SUB.TABTYPE,USER_TAB_COMMENTS.COMMENTS " +
					"FROM " + sqlTableName + ",USER_TAB_COMMENTS " +
					"WHERE SUB.TNAME = USER_TAB_COMMENTS.TABLE_NAME(+)";

				if ( Program.expertMode && toolStripCustomTableSelect.Checked )
				{
					StringBuilder returnedString = new StringBuilder(1024);
					api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECT_TABLE_NAME, sql, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
					sql = returnedString.ToString();
				}
#else
				// USER_TAB_COMMENTS にリンクがあるテーブル名を取得する (TABLE, VIEW)
#if false
				//string sql = "SELECT TNAME FROM TAB ORDER BY TNAME";
				string sql = "SELECT TAB.TNAME,USER_TAB_COMMENTS.COMMENTS " +
							 "FROM TAB INNER JOIN USER_TAB_COMMENTS " +
							 "ON TAB.TNAME = USER_TAB_COMMENTS.TABLE_NAME";
#else
				string sql = "SELECT TAB.TNAME,TAB.TABTYPE,USER_TAB_COMMENTS.COMMENTS " +
							 "FROM TAB,USER_TAB_COMMENTS " +
							 "WHERE (TAB.TNAME (+) = USER_TAB_COMMENTS.TABLE_NAME) AND" +
							 //" (SUBSTR(TAB.TNAME,1,4) <> 'BIN$')";
							 " (INSTR(TAB.TNAME,'$') = 0)";
#endif
#endif
				//StringBuilder sqlNotEqualTable = new StringBuilder ("");

				oraCmd = new OracleCommand(sql, oraConn);
				oraReader = oraCmd.ExecuteReader();

				bool withOwner = false;
				if ( Program.expertMode )
				{
					try
					{
						oraReader.GetOrdinal("OWNER");
						withOwner = true;
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
				}

				while ( oraReader.Read() )
				{
					XmlElement elem = xmlTableList.CreateElement(tagTable);

					string tname = oraReader["TNAME"].ToString();
					XmlAttribute attr = xmlTableList.CreateAttribute(attrName);		// @name
					attr.Value = tname;
					elem.Attributes.Append(attr);

					maxTableName = Math.Max(maxTableName, GetByteCount(tname));
					//sqlNotEqualTable.Append(" TAB.TNAME<>'" + tname + "' AND");

					string tabtype = oraReader["TABTYPE"].ToString();
					attr = xmlTableList.CreateAttribute(attrType);					// @type
					attr.Value = tabtype;
					elem.Attributes.Append(attr);

					string owner = "";
					if ( Program.expertMode )
					{
						if ( withOwner )
						{
							owner = oraReader["OWNER"].ToString();
						}
					}
					attr = xmlTableList.CreateAttribute(attrOwner);					// @owner
					attr.Value = owner;
					elem.Attributes.Append(attr);

					string comments = (oraReader["COMMENTS"] != DBNull.Value) ? oraReader["COMMENTS"].ToString() : "";
					attr = xmlTableList.CreateAttribute(attrComments);				// @comments
					attr.Value = comments;
					elem.Attributes.Append(attr);
					if ( comments.Length != 0 )
					{
						hasTableComments = true;
					}

					tableList.AppendChild(elem);

					tables.Add(tname + "\t" + comments);
				}

				oraReader.Close();
				oraReader.Dispose();
				oraReader = null;
				oraCmd.Dispose();
				oraCmd = null;

#if false
				// USER_TAB_COMMENTS にリンクが無いテーブル名を取得する (SYNONYM)
#if true
				sql = "SELECT TAB.TNAME,TAB.TABTYPE,USER_SYNONYMS.TABLE_OWNER,USER_SYNONYMS.TABLE_NAME " +
					  "FROM TAB,USER_SYNONYMS " +
					  "WHERE TAB.TABTYPE = 'SYNONYM' AND " +
					  " TAB.TNAME (+) = USER_SYNONYMS.SYNONYM_NAME " +
					  "ORDER BY USER_SYNONYMS.TABLE_OWNER,TAB.TNAME";
#else
				sql = "SELECT TNAME,TABTYPE " +
					  "FROM TAB " +
					  ((tables.Count != 0) ? "WHERE " + sqlNotEqualTable.ToString().Substring(0, sqlNotEqualTable.Length - 4) : "") + " " +
					  "ORDER BY TNAME";
#endif
				oraCmd = new OracleCommand(sql, oraConn);
				oraReader = oraCmd.ExecuteReader();

				while ( oraReader.Read() )
				{
					XmlElement elem = xmlTableList.CreateElement(tagTable);

					string tname = oraReader["TNAME"].ToString();
					XmlAttribute attr = xmlTableList.CreateAttribute(attrName);		// @name
					attr.Value = tname;
					elem.Attributes.Append(attr);

					string tabtype = oraReader["TABTYPE"].ToString();
					attr = xmlTableList.CreateAttribute(attrType);					// @type
					attr.Value = tabtype;
					elem.Attributes.Append(attr);

					string owner = oraReader["TABLE_OWNER"].ToString();
					attr = xmlTableList.CreateAttribute(attrOwner);					// @owner
					attr.Value = owner;
					elem.Attributes.Append(attr);

					tname = owner + "." + tname;
					maxTableName = Math.Max(maxTableName, GetByteCount(tname));

					string tableName = oraReader["TABLE_NAME"].ToString();
					string comments = "";
					string sql2 = "SELECT ALL_TAB_COMMENTS.COMMENTS " +
								  "FROM ALL_TAB_COMMENTS " +
								  "WHERE ALL_TAB_COMMENTS.OWNER = '" + owner + "' AND ALL_TAB_COMMENTS.TABLE_NAME = '" + tableName + "'";
					OracleCommand oraCmd2 = new OracleCommand(sql2, oraConn);
					OracleDataReader oraReader2 = oraCmd2.ExecuteReader();
					if ( oraReader2.HasRows )
					{
						oraReader2.Read();
						comments = oraReader2["COMMENTS"].ToString();
						if ( comments.Length != 0 )
						{
							hasTableComments = true;
						}
					}
					else
					{
						comments = "no table ?";
					}
					oraReader2.Close();
					oraReader2.Dispose();
					oraCmd2.Dispose();
					attr = xmlTableList.CreateAttribute(attrComments);				// @comments
					attr.Value = comments;
					elem.Attributes.Append(attr);
					Debug.WriteLine("tname:" + tname + " owner:" + owner + " tableName:" + tableName + " comments:" + comments);

					tableList.AppendChild(elem);

					tables.Add(tname + "\t" + comments);
				}
#else
				// SYNONYM のテーブル名を取得する
#if true
				sqlTableName =
					"(SELECT TAB.TNAME,TAB.TABTYPE,USER_SYNONYMS.TABLE_OWNER,USER_SYNONYMS.TABLE_NAME,USER_SYNONYMS.DB_LINK" +
					" FROM TAB,USER_SYNONYMS" +
					" WHERE (TAB.TNAME = USER_SYNONYMS.SYNONYM_NAME(+))" +
					"  AND (TAB.TABTYPE = 'SYNONYM')" +
					" ORDER BY USER_SYNONYMS.TABLE_OWNER,TAB.TNAME) SUB ";
				sql =
					"SELECT SUB.TNAME,SUB.TABTYPE,SUB.TABLE_OWNER,SUB.DB_LINK,ALL_TAB_COMMENTS.COMMENTS " +
					"FROM " + sqlTableName + ",ALL_TAB_COMMENTS " +
					"WHERE (SUB.TABLE_OWNER = ALL_TAB_COMMENTS.OWNER(+))" +
					" AND (SUB.TABLE_NAME = ALL_TAB_COMMENTS.TABLE_NAME(+))";

				if ( Program.expertMode )
				{
					StringBuilder returnedString = new StringBuilder(1024);
					api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECT_SYNONYM_NAME, sql, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
					sql = returnedString.ToString();
				}
#else
				string subQuery =
					"(SELECT TAB.TNAME,TAB.TABTYPE,USER_SYNONYMS.TABLE_OWNER,USER_SYNONYMS.TABLE_NAME " +
					"FROM TAB,USER_SYNONYMS " +
					"WHERE TAB.TABTYPE = 'SYNONYM' AND TAB.TNAME (+) = USER_SYNONYMS.SYNONYM_NAME ORDER BY USER_SYNONYMS.TABLE_OWNER,TAB.TNAME) SUB ";
				sql =
					"SELECT SUB.TNAME,SUB.TABTYPE,SUB.TABLE_OWNER,ALL_TAB_COMMENTS.COMMENTS " +
					"FROM ALL_TAB_COMMENTS," + subQuery +
					"WHERE ALL_TAB_COMMENTS.OWNER = SUB.TABLE_OWNER AND ALL_TAB_COMMENTS.TABLE_NAME = SUB.TABLE_NAME";
#endif
				oraCmd = new OracleCommand(sql, oraConn);
				oraReader = oraCmd.ExecuteReader();

				while ( oraReader.Read() )
				{
					XmlElement elem = xmlTableList.CreateElement(tagTable);

					string tname = oraReader["TNAME"].ToString();
					XmlAttribute attr = xmlTableList.CreateAttribute(attrName);		// @name
					attr.Value = tname;
					elem.Attributes.Append(attr);

					string tabtype = oraReader["TABTYPE"].ToString();
					attr = xmlTableList.CreateAttribute(attrType);					// @type
					attr.Value = tabtype;
					elem.Attributes.Append(attr);

					string owner = oraReader["TABLE_OWNER"].ToString();
					attr = xmlTableList.CreateAttribute(attrOwner);					// @owner
					attr.Value = owner;
					elem.Attributes.Append(attr);

					string dbLink = oraReader["DB_LINK"].ToString();
					attr = xmlTableList.CreateAttribute(attrDbLink);				// @dbLink
					attr.Value = dbLink;
					elem.Attributes.Append(attr);

					if ( showSynonymOwner )
					{
						string _owner = owner;
						if ( !string.IsNullOrEmpty(dbLink) )
						{
							_owner = dbLink.Split('.')[0];
						}
						tname = owner + "." + tname;
					}
					maxTableName = Math.Max(maxTableName, GetByteCount(tname));

					string comments = oraReader["COMMENTS"].ToString();
					attr = xmlTableList.CreateAttribute(attrComments);				// @comments
					attr.Value = comments;
					elem.Attributes.Append(attr);
					if ( comments.Length != 0 )
					{
						hasTableComments = true;
					}

					tableList.AppendChild(elem);

					tables.Add(tname + "\t" + comments);
				}
#endif

				if ( Program.debMode )
				{
					xmlTableList.Save(Application.StartupPath + "\\" + "~tableList.xml");
				}

				//if ( !showSynonymOwner )
				//{
					SortTableName(1, out tables, out maxTableName);
				//}

				// リストボックスにテーブル名を追加する
				SetTableName(tables, maxTableName);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				if ( oraReader != null )
				{
					oraReader.Close();
					oraReader.Dispose();
					oraReader = null;
				}

				if ( oraCmd != null )
				{
					oraCmd.Dispose();
					oraCmd = null;
				}

				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// GetByteCount
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private int GetByteCount(string str)
		{
			return sjisEnc.GetByteCount(str);
			//return (int)api.lstrlenA(str);
		}

		/// <summary>
		/// リストボックスにテーブル名を追加する
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="maxTableName"></param>
		private void SetTableName(List<string> tables, int maxTableName)
		{
			int maxTabTableName = (maxTableName / 8) + 1;

			foreach ( string table in tables )
			{
				string[] values = table.Split('\t');
				int tabTableName = maxTabTableName - (GetByteCount(values[0]) / 8);
				listBoxTableList.Items.Add(values[0] + ((values[1].Length != 0) ? new string('\t', tabTableName) + values[1] : ""));
			}

#if TABLE_NAME_HAS_ALIAS
			listBoxTableList.Tag = maxTableName;
#endif
		}

		/// <summary>
		/// テーブル一覧をソートする
		/// </summary>
		/// <param name="sortColumn">1:テーブル名 2:コメント</param>
		/// <param name="tables"></param>
		/// <param name="maxTableName"></param>
		private bool SortTableName(int sortColumn, out List<string> tables, out int maxTableName)
		{
			tables = new List<string>();
			maxTableName = 0;

			try
			{
				// ソート後のテーブル名の一覧
				XmlDocument xmlSortedTableList = new XmlDocument();
				XmlDeclaration decl = xmlSortedTableList.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlSortedTableList.AppendChild(decl);
				xmlSortedTableList.AppendChild(xmlSortedTableList.CreateNode(XmlNodeType.Element, tagTableList, null));	// <tableList>

				List<string> alTables = new List<string>();
				XmlNodeList tableList = xmlTableList.DocumentElement.ChildNodes;
				int sortKeyLen = 0, zenHanJudge = 0;

				if ( sortColumn == 1 )		// テーブル名でソートする
				{
					const int ownerLen = 32, tableNameLen = 64;
					for ( int i = 0; i < tableList.Count; i++ )
					{
						string owner = (showSynonymOwner) ? tableList[i].Attributes[attrOwner].Value : "";
						string tname = tableList[i].Attributes[attrName].Value;
						alTables.Add(GetStringWithSpace(owner, ownerLen) + GetStringWithSpace(tname, tableNameLen) + i);
					}
					sortKeyLen = ownerLen + tableNameLen;
					zenHanJudge = ownerLen;
				}
				else if ( sortColumn == 2 )	// コメントでソートする
				{
					const int commentsLen = 128;
					for ( int i = 0; i < tableList.Count; i++ )
					{
						string comments = tableList[i].Attributes[attrComments].Value;
						if ( comments.Length == 0 )
						{
							comments = GetStringWithSpace("_" + ((showSynonymOwner) ? tableList[i].Attributes[attrOwner].Value : ""), 32) + tableList[i].Attributes[attrName].Value;
						}
						alTables.Add(GetStringWithSpace(comments, commentsLen) + i);
					}
					sortKeyLen = commentsLen;
					zenHanJudge = 0;
				}

				TableNameComparer tableNameComparer = new TableNameComparer(sortKeyLen, zenHanJudge, ascendingTableName);
				alTables.Sort(tableNameComparer);

				// tables にソートした順番で格納する
				for ( int i = 0; i < alTables.Count; i++ )
				{
					int idx = int.Parse(alTables[i].ToString().Substring(sortKeyLen));
					XmlNode tableNode = tableList[idx];

					string owner = tableNode.Attributes[attrOwner].Value;
					if ( tableNode.Attributes[attrDbLink] != null )
					{
						string dbLink = tableNode.Attributes[attrDbLink].Value;
						if ( !string.IsNullOrEmpty(dbLink) )
						{
							owner = dbLink.Split('.')[0];
						}
					}
					string tname = tableNode.Attributes[attrName].Value;
					if ( showSynonymOwner && (owner.Length != 0) )
					{
						tname = owner + "." + tname;
					}
					maxTableName = Math.Max(maxTableName, GetByteCount(tname));

					string comments = tableNode.Attributes[attrComments].Value;

					tables.Add(tname + "\t" + comments);

					xmlSortedTableList.DocumentElement.AppendChild(xmlSortedTableList.ImportNode(tableNode, true));
				}

				xmlTableList = xmlSortedTableList;

				if ( Program.debMode )
				{
					xmlTableList.Save(Application.StartupPath + "\\" + "~tableList.xml");
				}

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}

		/// <summary>
		/// テーブル名をソートするための比較用関数
		/// </summary>
		class TableNameComparer : IComparer<string>
		{
			//private bool ascending;
			private int sortKeyLen;
			private int zenHanJudge;
			//private System.Globalization.CultureInfo cultureInfo;
			private Encoding sjisEnc = Encoding.GetEncoding("shift_jis");
			private int order;

			public TableNameComparer(int sortKeyLen, int zenHanJudge, bool ascending)
			{
				this.sortKeyLen = sortKeyLen;
				//this.ascending = ascending;
				this.zenHanJudge = zenHanJudge;
				//cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
				//cultureInfo = new System.Globalization.CultureInfo("en-US");
				this.order = ascending ? 1 : -1;
			}

			public int Compare(string x, string y)
			{
				try
				{
					string ox = ((string)x).Substring(0, sortKeyLen);
					string oy = ((string)y).Substring(0, sortKeyLen);
#if true
					//if ( ascending )
					//	//return cultureInfo.CompareInfo.Compare(ox, oy, System.Globalization.CompareOptions.IgnoreKanaType/*System.Globalization.CompareOptions.OrdinalIgnoreCase*/);
					//	return string.CompareOrdinal(ox, oy);
					//else
					//	//return -cultureInfo.CompareInfo.Compare(ox, oy, System.Globalization.CompareOptions.OrdinalIgnoreCase);
					//	return -string.CompareOrdinal(ox, oy);
					int oxCount = sjisEnc.GetByteCount(ox[zenHanJudge].ToString());
					int oyCount = sjisEnc.GetByteCount(oy[zenHanJudge].ToString());
					// (コメントの並び替え || (オーナーが無い)) && (半角全角の比較)？
					if ( (zenHanJudge == 0 || (ox[0] == ' ' && oy[0] == ' ')) && (oxCount != oyCount) )
					{
						return (oxCount < oyCount ? -1 : 1) * order;
					}
					return String.Compare(ox, oy) * order;
#else
					if ( ascending )
						return String.Compare(ox, oy);
					else
						return -String.Compare(ox, oy);
#endif
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
					return 0;
				}
			}
		}

		/// <summary>
		/// GetStringWithSpace
		/// </summary>
		public static string GetStringWithSpace(string strSource, int cbString)
		{
			string strReply = null;

			try
			{
				//strReply = strSource + ((strSource.Length < cbString) ? new string(' ', cbString - strSource.Length) : "");
				strReply = string.Format("{0,-" + cbString + "}", strSource);
			}
			catch ( Exception )
			{
				strReply = new string(' ', cbString);
			}

			return strReply.Substring(0, cbString);
		}

		/// <summary>
		/// 選択されたテーブル名のカラムを取得する
		/// </summary>
		private bool SelectColumns()
		{
			OracleCommand oraCmd = null;
			OracleDataReader oraReader = null;

			try
			{
				textColumnFilter.Text = string.Empty;
				textColumnFilter.Update();
				listBoxColumnList.Items.Clear();

#if !TABLE_NAME_HAS_ALIAS
				if ( listBoxTableList.Text.Length == 0 )
					return false;
#endif

				Cursor.Current = Cursors.WaitCursor;

#if UPDATE_20140729
				string tableOwner = GetListBoxTableOwner();
				string tableName = GetListBoxTableName(selTbl.plainTblName), sql;

				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "']";
				if ( tableOwner != null )
				{
					xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "'" + " and @" + attrOwner + "='" + tableOwner + "']";
				}
#else
				string tableName = GetListBoxTableName(selTbl.plainTblName), sql;

				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + tableName + "']";
#endif
				XmlNode table = xmlTableList.SelectSingleNode(xpath);

				List<string> columns = new List<string>();
				int maxColumnName = 0, maxDataType = 0, maxComments = 0;

				bool dbLink = (table.Attributes[attrDbLink] != null) && !string.IsNullOrEmpty(table.Attributes[attrDbLink].Value);
				bool synonym = (string.Compare(table.Attributes[attrType].Value, "SYNONYM", true) == 0) && !dbLink;

				if ( synonym )
				{
#if true
					// サブクエリで取得する
					string sqlUSER_SYNONYMS =
						"(SELECT * FROM USER_SYNONYMS WHERE SYNONYM_NAME = '" + tableName + "') USER_SYNONYMS ";
					string sqlALL_TAB_COLUMNS =
						"(SELECT" +
						" ALL_TAB_COLUMNS.OWNER," +
						" ALL_TAB_COLUMNS.TABLE_NAME," +
						" ALL_TAB_COLUMNS.COLUMN_NAME," +
						" ALL_TAB_COLUMNS.COLUMN_ID," +
						" ALL_TAB_COLUMNS.DATA_TYPE," +
						" ALL_TAB_COLUMNS.NULLABLE, " +
						" NVL(ALL_TAB_COLUMNS.DATA_PRECISION,ALL_TAB_COLUMNS.DATA_LENGTH) AS LENGTH," +
						" ALL_TAB_COLUMNS.DATA_SCALE " +
						"FROM" +
						" ALL_TAB_COLUMNS," + sqlUSER_SYNONYMS +
						"WHERE" +
						" ALL_TAB_COLUMNS.OWNER=USER_SYNONYMS.TABLE_OWNER AND" +
						" ALL_TAB_COLUMNS.TABLE_NAME=USER_SYNONYMS.TABLE_NAME " +
						"ORDER BY" +
						" ALL_TAB_COLUMNS.COLUMN_ID) ALL_TAB_COLUMNS ";
					sql =
						"SELECT" +
						" ALL_TAB_COLUMNS.*," +
						" ALL_COL_COMMENTS.COMMENTS " +
						"FROM" +
						" ALL_COL_COMMENTS," + sqlALL_TAB_COLUMNS +
						"WHERE" +
						" ALL_TAB_COLUMNS.OWNER=ALL_COL_COMMENTS.OWNER AND" +
						" ALL_TAB_COLUMNS.TABLE_NAME=ALL_COL_COMMENTS.TABLE_NAME AND" +
						" ALL_TAB_COLUMNS.COLUMN_NAME=ALL_COL_COMMENTS.COLUMN_NAME";
#else
					sql = "SELECT * FROM USER_SYNONYMS WHERE SYNONYM_NAME = '" + tableName + "'";
					oraCmd = new OracleCommand(sql, oraConn);
					oraReader = oraCmd.ExecuteReader();
					oraReader.Read();
					string tableOwner = oraReader["TABLE_OWNER"].ToString();
					tableName = oraReader["TABLE_NAME"].ToString();
					oraReader.Close();
					oraReader.Dispose();
					oraReader = null;
					oraCmd.Dispose();
					oraCmd = null;

#if true
					sql = "SELECT ALL_TAB_COLUMNS.COLUMN_NAME," +
						  " ALL_TAB_COLUMNS.DATA_TYPE," +
						  " ALL_TAB_COLUMNS.NULLABLE," +
						  " NVL(ALL_TAB_COLUMNS.DATA_PRECISION,ALL_TAB_COLUMNS.DATA_LENGTH) AS LENGTH," +
						  " ALL_TAB_COLUMNS.DATA_SCALE, " +
						  " ALL_COL_COMMENTS.COMMENTS " +
						  "FROM ALL_TAB_COLUMNS,ALL_COL_COMMENTS " +
						  "WHERE (ALL_TAB_COLUMNS.OWNER = ALL_COL_COMMENTS.OWNER) AND" +
						  " (ALL_TAB_COLUMNS.TABLE_NAME=ALL_COL_COMMENTS.TABLE_NAME AND ALL_TAB_COLUMNS.COLUMN_NAME=ALL_COL_COMMENTS.COLUMN_NAME) AND" +
						  " (ALL_TAB_COLUMNS.OWNER='" + tableOwner + "' AND ALL_TAB_COLUMNS.TABLE_NAME='" + tableName + "') " +
						  "ORDER BY ALL_TAB_COLUMNS.COLUMN_ID";
#else
					// こっちだと ALL_COL_COMMENTS.OWNER に別名で同名の TABLE_NAME が存在すると、カラムがダブって SELECT される
					sql = "SELECT ALL_TAB_COLUMNS.COLUMN_NAME," +
						  " ALL_TAB_COLUMNS.DATA_TYPE," +
						  " ALL_TAB_COLUMNS.NULLABLE," +
						  " NVL(ALL_TAB_COLUMNS.DATA_PRECISION,ALL_TAB_COLUMNS.DATA_LENGTH) AS LENGTH," +
						  " ALL_TAB_COLUMNS.DATA_SCALE, " +
						  " ALL_COL_COMMENTS.COMMENTS " +
						  "FROM ALL_TAB_COLUMNS,ALL_COL_COMMENTS " +
						  "WHERE ALL_TAB_COLUMNS.OWNER = '" + tableOwner + "' AND ALL_TAB_COLUMNS.TABLE_NAME = '" + tableName + "' AND " +
						  //"((ALL_TAB_COLUMNS.COLUMN_NAME (+) = ALL_COL_COMMENTS.COLUMN_NAME) AND (ALL_TAB_COLUMNS.TABLE_NAME (+) = ALL_COL_COMMENTS.TABLE_NAME)) " +
						  "((ALL_TAB_COLUMNS.COLUMN_NAME = ALL_COL_COMMENTS.COLUMN_NAME) AND (ALL_TAB_COLUMNS.TABLE_NAME = ALL_COL_COMMENTS.TABLE_NAME)) " +
						  "ORDER BY ALL_TAB_COLUMNS.COLUMN_ID";
#endif
#endif
				}
#if ENABLED_SUBQUERY
				else if ( string.Compare(table.Attributes[attrType].Value, SUBQUERY_TYPE, true) == 0 )
				{
					sql = null;
					string subQuery = table.Attributes[attrDir].Value + "\\" + table.Attributes[attrName].Value + ".xml";
					XmlDocument _xmlShenlongColumn = ShenGlobal.ReadSubQueryFile(subQuery, GetSubQueryBaseURI(subQuery, xmlShenlongColumnFileName));

					foreach ( XmlNode columnNode in _xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn) )
					{
						if ( !bool.Parse(columnNode[ShenGlobal.qc.showField.ToString()].InnerText) )
						//if ( columnNode[ShenCore.qc.value1.ToString()].InnerText.StartsWith(withoutTableName) )
							continue;

						StringBuilder column = new StringBuilder();

						XmlNode fieldName = columnNode[ShenGlobal.qc.fieldName.ToString()];
						string columnName = fieldName.InnerText;
#if true
						//string alias = null;
						//int fieldAsIndex;
						string fieldAliasName;
						string plainFieldName = ShenGlobal.GetPlainTableFieldName(columnName, /*out fieldAsIndex, */out fieldAliasName);
						if ( fieldAliasName != null )/*( fieldAsIndex != -1 )*/				// 直接の別名指定がある？
						{
							//alias = columnName.Substring(fieldAsIndex + 4).Trim();
						}
						else
						{
							XmlNode property = columnNode[ShenGlobal.qc.property.ToString()];
							if ( property[ShenGlobal.prop.alias.ToString()] != null )	// プロパティでの別名指定がある？
							{
								fieldAliasName/*alias*/ = property[ShenGlobal.prop.alias.ToString()].InnerText;
							}
						}
						columnName = fieldAliasName/*alias*/ ?? columnName;
#endif
						maxColumnName = Math.Max(maxColumnName, GetByteCount(columnName));
						column.Append(columnName + "\t");

						string dataType = fieldName.Attributes[ShenGlobal.prop.type.ToString()].Value + "(" + fieldName.Attributes[ShenGlobal.prop.length.ToString()].Value + ")";
						maxDataType = Math.Max(maxDataType, dataType.Length);
						column.Append(dataType + "\t");

						string comments = columnNode[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.comment.ToString()].InnerText;
						maxComments = Math.Max(maxComments, GetByteCount(comments));
						column.Append(comments + "\t");

						string nullable = fieldName.Attributes[ShenGlobal.prop.nullable.ToString()].Value;
						column.Append((nullable == propNotNullable) ? "NOT NULL" : "");

						columns.Add(column.ToString());
					}
				}
#endif
				else
				{
#if false
					sql = "SELECT USER_TAB_COLUMNS.COLUMN_NAME," +
						  " USER_TAB_COLUMNS.DATA_TYPE," +
						  " USER_TAB_COLUMNS.NULLABLE," +
						  " NVL(USER_TAB_COLUMNS.DATA_PRECISION,USER_TAB_COLUMNS.DATA_LENGTH) AS LENGTH," +
						  " USER_TAB_COLUMNS.DATA_SCALE, " +
						  " USER_COL_COMMENTS.COMMENTS " +
						  "FROM USER_TAB_COLUMNS " +
						  "INNER JOIN USER_COL_COMMENTS " +
						  "ON (USER_TAB_COLUMNS.COLUMN_NAME = USER_COL_COMMENTS.COLUMN_NAME) AND (USER_TAB_COLUMNS.TABLE_NAME = USER_COL_COMMENTS.TABLE_NAME) " +
						  "WHERE USER_TAB_COLUMNS.TABLE_NAME = '" + tableName + "' " +
						  "ORDER BY USER_TAB_COLUMNS.COLUMN_ID";
#else
					string _dbLink = (dbLink) ? ("@" + table.Attributes[attrDbLink].Value) : "";

					sql = "SELECT USER_TAB_COLUMNS.COLUMN_NAME," +
						  " USER_TAB_COLUMNS.DATA_TYPE," +
						  " USER_TAB_COLUMNS.NULLABLE," +
						  " NVL(USER_TAB_COLUMNS.DATA_PRECISION,USER_TAB_COLUMNS.DATA_LENGTH) AS LENGTH," +
						  " USER_TAB_COLUMNS.DATA_SCALE," +
						  " USER_COL_COMMENTS.COMMENTS " +
						  "FROM USER_TAB_COLUMNS" + _dbLink + ",USER_COL_COMMENTS" + _dbLink + " " +
						  "WHERE USER_TAB_COLUMNS.TABLE_NAME = '" + tableName + "' AND " +
						//"((USER_TAB_COLUMNS.COLUMN_NAME (+) = USER_COL_COMMENTS.COLUMN_NAME) AND (USER_TAB_COLUMNS.TABLE_NAME (+) = USER_COL_COMMENTS.TABLE_NAME)) " +
						  "((USER_TAB_COLUMNS.COLUMN_NAME = USER_COL_COMMENTS.COLUMN_NAME(+)) AND (USER_TAB_COLUMNS.TABLE_NAME = USER_COL_COMMENTS.TABLE_NAME(+))) " +
						  "ORDER BY USER_TAB_COLUMNS.COLUMN_ID";

					if ( Program.expertMode && toolStripCustomTableSelect.Checked )
					{
						StringBuilder returnedString = new StringBuilder(1024);
						/*if ( api.GetPrivateProfileString(SETTINGS_SECTION, "SelectColumnsReplace", "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) != 0 )
						{
							string[] replaces = returnedString.ToString().Split(';');
							for ( int i = 0; i < replaces.Length; i++ )
							{
								string[] value = replaces[i].Split(',');
								sql = sql.Replace(value[0], value[1]);
							}
						}*/
						api.GetPrivateProfileString(SETTINGS_SECTION, KEY_SELECT_COLUMNS, sql, returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
						sql = returnedString.ToString().Replace("%dblink%", _dbLink);
						sql = sql.Replace("%tablename%", tableName);
#if UPDATE_20140729
						if ( tableOwner != null )
						{
							sql = sql.Replace("%tableowner%", tableOwner);
						}
#endif
					}
#endif
				}

#if ENABLED_SUBQUERY
				if ( sql != null )
				{
#endif
					oraCmd = new OracleCommand(sql, oraConn);
					oraReader = oraCmd.ExecuteReader();

					//List<string> columns = new List<string>();
					//int maxColumnName = 0, maxDataType = 0, maxComments = 0;

					while ( oraReader.Read() )
					{
						StringBuilder column = new StringBuilder();

						string columnName = oraReader["COLUMN_NAME"].ToString();
						maxColumnName = Math.Max(maxColumnName, GetByteCount(columnName));
						column.Append(columnName + "\t");

						StringBuilder dataType = new StringBuilder();
						dataType.Append(oraReader["DATA_TYPE"].ToString());
						dataType.Append("(" + oraReader["LENGTH"].ToString());
						if ( oraReader["DATA_SCALE"] != DBNull.Value )
						{
							string dataScale = oraReader["DATA_SCALE"].ToString();
							if ( dataScale != "0" )
							{
								dataType.Append("," + dataScale);
							}
						}
						dataType.Append(")");
						maxDataType = Math.Max(maxDataType, dataType.Length);
						column.Append(dataType + "\t");

						string comments = (oraReader["COMMENTS"] != DBNull.Value) ? oraReader["COMMENTS"].ToString() : ShenGlobal.propNoComment;
						maxComments = Math.Max(maxComments, GetByteCount(comments));
						column.Append(comments + "\t");

						string nullable = oraReader["NULLABLE"].ToString();
						column.Append((nullable == "N") ? "NOT NULL" : "");

						columns.Add(column.ToString());
					}

					oraReader.Close();
					oraReader.Dispose();
					oraReader = null;
					oraCmd.Dispose();
					oraCmd = null;
#if ENABLED_SUBQUERY
				}
#endif

#if true
				SetColumnName(columns, maxColumnName, maxDataType, maxComments);
#else
				//StreamWriter swDebugLog = new StreamWriter(Application.StartupPath + @"\~debug.log", false, Encoding.Default);
				//swDebugLog.WriteLine("maxColumnName:" + maxColumnName + " maxDataType:" + maxDataType + " maxComments:" + maxComments);
				int maxTabColumnName = (maxColumnName / 8) + 1;
				int maxTabDataType = (maxDataType / 8) + 1;
				int maxTabComments = (maxComments / 8) + 1;
				//swDebugLog.WriteLine("maxTabColumnName:" + maxTabColumnName + " maxTabDataType:" + maxTabDataType + " maxTabComments:" + maxTabComments);
				foreach ( string column in columns )
				{
					string[] values = column.Split('\t');
					int tabColumnName = maxTabColumnName - (GetByteCount(values[0]) / 8);
					int tabDataType = maxTabDataType - (values[1].Length / 8);
					int tabComments = maxTabComments - (GetByteCount(values[2]) / 8);
					listBoxColumnList.Items.Add(values[0] + new string('\t', tabColumnName) + values[1] + new string('\t', tabDataType) + values[2] + new string('\t', tabComments) + values[3]);
					//swDebugLog.WriteLine(values[0] + new string('\t', tabColumnName) + values[1] + new string('\t', tabDataType) + values[2] + new string('\t', tabComments) + values[3]);
				}
				listBoxColumnList.Items.Add("*" + new string('\t', maxTabColumnName) + "" + new string('\t', maxTabDataType) + "" + new string('\t', maxTabComments) + "");
				//swDebugLog.Close();
#endif

				XDocument xmlColumnList = new XDocument(new XDeclaration("1.0", "utf-8", "true"));
				XElement xeColumnList = new XElement("columnList",
										new XAttribute("maxColumnName", maxColumnName),
										new XAttribute("maxDataType", maxDataType),
										new XAttribute("maxComments", maxComments));
				xmlColumnList.Add(xeColumnList);
				//columns.Add("*" + "\t" + "" + "\t" + "" + "\t" + "");
				foreach ( string column in columns )
				{
					string[] values = column.Split('\t');
					XElement xeColumn = new XElement("column",
										new XAttribute("name", values[0]),
										new XAttribute("type", values[1]),
										new XAttribute("comment", values[2]),
										new XAttribute("nullable", values[3]));
					xeColumnList.Add(xeColumn);
				}
				xmlColumnList.Save(Application.StartupPath + "\\" + "~columnList.xml");

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
			finally
			{
				if ( oraReader != null )
				{
					oraReader.Close();
					oraReader.Dispose();
					oraReader = null;
				}

				if ( oraCmd != null )
				{
					oraCmd.Dispose();
					oraCmd = null;
				}

				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// リストボックスにカラム名を追加する
		/// </summary>
		/// <param name="columns"></param>
		private void SetColumnName(List<string> columns, int maxColumnName, int maxDataType, int maxComments)
		{
			//StreamWriter swDebugLog = new StreamWriter(Application.StartupPath + @"\~debug.log", false, Encoding.Default);
			//swDebugLog.WriteLine("maxColumnName:" + maxColumnName + " maxDataType:" + maxDataType + " maxComments:" + maxComments);
			int maxTabColumnName = (maxColumnName / 8) + 1;
			int maxTabDataType = (maxDataType / 8) + 1;
			int maxTabComments = (maxComments / 8) + 1;
			//swDebugLog.WriteLine("maxTabColumnName:" + maxTabColumnName + " maxTabDataType:" + maxTabDataType + " maxTabComments:" + maxTabComments);
			foreach ( string column in columns )
			{
				string[] values = column.Split('\t');
				int tabColumnName = maxTabColumnName - (GetByteCount(values[0]) / 8);
				int tabDataType = maxTabDataType - (values[1].Length / 8);
				int tabComments = maxTabComments - (GetByteCount(values[2]) / 8);
				listBoxColumnList.Items.Add(values[0] + new string('\t', tabColumnName) + values[1] + new string('\t', tabDataType) + values[2] + new string('\t', tabComments) + values[3]);
				//swDebugLog.WriteLine(values[0] + new string('\t', tabColumnName) + values[1] + new string('\t', tabDataType) + values[2] + new string('\t', tabComments) + values[3]);
			}
			listBoxColumnList.Items.Add("*" + new string('\t', maxTabColumnName) + "" + new string('\t', maxTabDataType) + "" + new string('\t', maxTabComments) + "");
			//swDebugLog.Close();
#if UPDATE_20251202
			UpdateHorizontalExtent(listBoxColumnList);
#endif
		}

#if TABLE_NAME_HAS_ALIAS
        /// <summary>
        /// リストボックスのテーブル名を取得する
        /// </summary>
        /// <returns></returns>
        private string GetListBoxTableName(selTbl seltbl)
		{
			return GetListBoxTableName(listBoxTableList.SelectedIndex, seltbl);
		}

		private string GetListBoxTableName(int index, selTbl seltbl)
		{
			string tableName = (string)listBoxTableList.Items[index]/*listBoxTableList.Text*/;

			int comment = tableName.IndexOf('\t');
			if ( comment != -1 )
			{
				tableName = tableName.Substring(0, comment);	// コメントを削除する
			}

			int owner = tableName.IndexOf('.');
			if ( ((uint)(seltbl & selTbl.withOwner) == 0) && (owner != -1) )
			{
				tableName = tableName.Substring(owner + 1);		// オーナーを削除する
			}

			int alias = tableName.IndexOf(' ');
			if ( ((uint)(seltbl & selTbl.plainTblName) != 0) && (alias != -1) )
			{
				tableName = tableName.Substring(0, alias);		// 別名を削除する
			}

			return tableName;
		}
#else
		/// <summary>
		/// 選択されたテーブル名を取得する
		/// </summary>
		/// <returns></returns>
		private string GetSelectedTableName()
		{
			string[] values = listBoxTableList.Text.Split('\t');
			int index = values[0].IndexOf('.');
			if ( index != -1 )
			{
				return values[0].Substring(index + 1);
			}

			return values[0];
		}
#endif

#if UPDATE_20140729
		/// <summary>
		/// リストボックスのテーブル オーナーを取得する
		/// </summary>
		/// <returns></returns>
		private string GetListBoxTableOwner()
		{
			string tableOwner = null;

			string tableName = listBoxTableList.Text;

#if UPDATE_20191120
			int comment = tableName.IndexOf('\t');
			if ( comment != -1 )
			{
				tableName = tableName.Substring(0, comment);    // コメントを削除する
			}
#endif

			int owner = tableName.IndexOf('.');
			if ( owner != -1 )
			{
				tableOwner = tableName.Substring(0, owner);
			}

			return tableOwner;
		}
#endif

		/// <summary>
		/// カラムが選択された時の処理
		/// </summary>
		/// <param name="columnItem"></param>
		private void ColumnItemSelected(string columnItem)
		{
			try
			{
				if ( tabControl.SelectedIndex != 0 )
				{
					tabControl.SelectedIndex = 0;
				}

				if ( columnItem[0] == '*' )
				{
					//lveQueryColumn.BeginUpdate();
					for ( int i = 0; i < listBoxColumnList.Items.Count - 1; i++ )
					{
						if ( !AppendSelectedColumnItem(listBoxColumnList.Items[i].ToString()) )
							break;
					}
					//lveQueryColumn.EndUpdate();
					api.PostMessage(lveQueryColumn.Handle, api.WM_HSCROLL, api.SB_RIGHT, 0);
				}
				else
				{
					int lastColCount = lveQueryColumn.Columns.Count;
					AppendSelectedColumnItem(columnItem);

					if ( lastColCount != lveQueryColumn.Columns.Count )
					{
						api.PostMessage(lveQueryColumn.Handle, api.WM_HSCROLL, api.SB_PAGERIGHT, 0);
					}
					else
					{
#if UPDATE_20140729
						string tableOwner = string.Empty;
						string tableName = GetListBoxTableName(selTbl.raw);
						if ( Program.expertMode && toolStripCustomTableSelect.Checked )
						{
							if ( putDiffOwnerToTable )	// オーナーが違うテーブル名に "OWNER." を付ける？
							{
								string _owner = GetListBoxTableOwner();
								if ( !string.IsNullOrEmpty(_owner) )
								{
									string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
									if ( string.Compare(_owner, logOn[(int)logon.uid].Trim(), true) != 0 )
									{
										tableOwner = _owner + ".";
									}
								}
							}
						}

						ReverseQueryColumn(tableOwner + tableName, columnItem.Split('\t')[(int)co.name], false);
#else
						ReverseQueryColumn(GetListBoxTableName(selTbl.raw), columnItem.Split('\t')[(int)co.name], false);
#endif
					}
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				//lveQueryColumn.EndUpdate();
			}
		}

		/// <summary>
		/// 選択されたカラムをクエリー項目に追加する
		/// </summary>
		/// <param name="columnItem"></param>
		private bool AppendSelectedColumnItem(string columnItem, int index)
		{
			try
			{
				string tableName = GetListBoxTableName(selTbl.raw);
#if true
				if ( Program.expertMode && toolStripCustomTableSelect.Checked )
				{
					if ( putDiffOwnerToTable )	// オーナーが違うテーブル名に "OWNER." を付ける？
					{
						string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + ShenGlobal.GetTableName(tableName, true) + "']";
#if UPDATE_20140729
						string tableOwner = GetListBoxTableOwner();
						if ( tableOwner != null )
						{
							xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrName + "='" + ShenGlobal.GetTableName(tableName, true) + "'" + " and @" + attrOwner + "='" + tableOwner + "']";
						}
#endif
						XmlNode table = xmlTableList.SelectSingleNode(xpath);
						if ( table != null )
						{
							string _owner = table.Attributes[attrOwner].Value;
							if ( !string.IsNullOrEmpty(_owner) )
							{
								string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
								if ( string.Compare(_owner, logOn[(int)logon.uid].Trim(), true) != 0 )
								{
									tableName = _owner + "." + tableName;
								}
							}
						}
					}
				}
#endif

				StringBuilder column = new StringBuilder();
				char c = '\0';
				for ( int i = 0, count = 0; i < columnItem.Length; i++ )
				{
					if ( c == '\t' && columnItem[i] == '\t' )
						continue;
					if ( (c = columnItem[i]) == '\t' )
						count++;
					if ( count == (int)co.type )
					{
						if ( c == '(' )
							c = '\t';				// ここで擬似的にデータ タイプとレングスを分離しておく
						else if ( c == ')' )
							continue;
					}
					column.Append(c);
				}
				string[] values = column.ToString().Split('\t');
				values[(int)co.nullable] = (values[(int)co.nullable].Length == 0) ? propNullable : propNotNullable;

				if ( !enableSameColumnAppend && (HasQueryColumn(tableName, values[(int)co.name], 0x0002) != -1) )	// 既に選択済み？
					return true;

				string[] property = new string[(int)ShenGlobal.prop.count];
				property[(int)ShenGlobal.prop.type] = values[(int)co.type];
				property[(int)ShenGlobal.prop.length] = values[(int)co.length];
				property[(int)ShenGlobal.prop.nullable] = values[(int)co.nullable];
				property[(int)ShenGlobal.prop.comment] = values[(int)co.comment];

				string[] queryColumn = { values[(int)co.name], true.ToString().ToLower(), "", "", "", "", "", "", string.Join(sepProp, property) };

				if ( AddQueryColumn(tableName, (checkStretchColumnWidth.Checked ? narColumnWidth : defColumnWidth), queryColumn, index/*-1/*true*/) == 1 )
				{
					ChangeModified(true);
					return true;
				}

				return false;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}
		private bool AppendSelectedColumnItem(string columnItem)
		{
			return AppendSelectedColumnItem(columnItem, -1);
		}

		/// <summary>
		/// 既に選択済みの[テーブル].[カラム]があるかチェックする
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		/// <param name="hasPattern">
		/// 0x0001:テーブル名のみをチェックする
		/// 0x0002:既に編集されているカラムは同一と見なさない
		/// </param>
		/// <returns></returns>
		private int HasQueryColumn(string tableName, string columnName, uint hasPattern)
		{
			for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
			{
				if ( tableName != lveQueryColumn.Columns[i].Text )
					continue;

				if ( (hasPattern & 0x0001) != 0 )
					return i;

				if ( columnName != lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[i].Text )
					continue;

				if ( (hasPattern & 0x0002) != 0 )
				{
					if ( (lveQueryColumn.Items[(int)ShenGlobal.qc.expression].SubItems[i].Text.Length != 0) ||
						 (lveQueryColumn.Items[(int)ShenGlobal.qc.groupFunc].SubItems[i].Text.Length != 0) )
						continue;
				}

				return i;
			}

			return -1;
		}

#if true
		/// <summary>
		/// クエリー項目にカラムを追加する
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnWidth"></param>
		/// <param name="items"></param>
		/// <param name="index"></param>
		/// <returns>0:カラムは追加されなかった 1:カラムは１つ追加された</returns>
		private int AddQueryColumn(string tableName, int columnWidth, string[] items, int index)
		{
			int columnCount = int.Parse(toolStripStatusColumnCount.Text);
			if ( maxColumnCount <= columnCount )
			{
				MessageBox.Show("これ以上の項目の追加は出来ません.（最大" + maxColumnCount + "個まで）", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}

			if ( lveQueryColumn.Items.Count == 0 )	// 最初の項目？
			{
				lveQueryColumn.Columns.Add(tableName, columnWidth, HorizontalAlignment.Left);

				foreach ( string text in items )
				{
					ListViewItem lvi = new ListViewItem(text);
					lveQueryColumn.Items.Add(lvi);
				}

				queryTableNames.Add(tableName);

				ToolStripMenuEnable(true);

				api.EnableScrollBar(lveQueryColumn.Handle, (uint)api.SBFlags.SB_VERT, (uint)api.SBArrows.ESB_DISABLE_BOTH);
			}
			else
			{
				if ( index == -1 )
				{
					index = lveQueryColumn.Columns.Count;
				}

				if ( HasQueryColumn(tableName, null, 0x0001) == -1 )
				{
					queryTableNames.Add(tableName);
				}

				//int latestWidth = lveQueryColumn.Items[0].SubItems[0].Bounds.Width;

				lveQueryColumn.Columns.Insert(index, tableName, columnWidth, HorizontalAlignment.Left);

				for ( int i = 0; i < items.Length; i++ )
				{
					ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lveQueryColumn.Items[i], items[i]);
					lveQueryColumn.Items[i].SubItems.Insert(index, subItem);
				}

				// [Windows 7] 水平スクロールバーが表示されない対策
				if ( (osPlatform == common.platform.win7) || (osPlatform == common.platform.win10) )
				{
					if ( Program.debMode )
					{
						string toolTip = "QueryColumnClientWidth:" + lveQueryColumn.ClientRectangle.Width + "　ItemsBoundsWidth:" + lveQueryColumn.Items[0].SubItems[0].Bounds.Width;
						toolStripStatusColumnCount.ToolTipText = toolTip;
					}

					// アイテムの横幅合計がクライアントの横幅を/*初めて*/超えた？
					if ( /*(latestWidth <= lveQueryColumn.ClientRectangle.Width) &&*/
						 (lveQueryColumn.ClientRectangle.Width < lveQueryColumn.Items[0].SubItems[0].Bounds.Width) )
					{
						lveQueryColumn.BeginUpdate();
						/*columnCount = lveQueryColumn.Columns.Count;
						lveQueryColumn.Columns[columnCount - 1].Width++;
						lveQueryColumn.Columns[columnCount - 1].Width--;*/
						lveQueryColumn.EndUpdate();
					}
				}
			}

			toolStripStatusColumnCount.Text = (int.Parse(toolStripStatusColumnCount.Text) + 1).ToString();
			return 1;
		}
#else
		/// <summary>
		/// クエリー項目にカラムを追加する
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnWidth"></param>
		/// <param name="items"></param>
		/// <param name="checkQueryColumnItemsCount"></param>
		/// <returns>0:カラムは追加されなかった 1:カラムは１つ追加された</returns>
		private int AddQueryColumn(string tableName, int columnWidth, string[] items, bool checkQueryColumnItemsCount)
		{
			int columnCount = int.Parse(toolStripStatusColumnCount.Text);
			if ( maxColumnCount <= columnCount )
			{
				MessageBox.Show("これ以上の項目の追加は出来ません.（最大" + maxColumnCount + "個まで）", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}

			if ( checkQueryColumnItemsCount && (lveQueryColumn.Items.Count == 0) )	// 最初の項目？
			{
				lveQueryColumn.Columns.Add(tableName, columnWidth, HorizontalAlignment.Left);

				foreach ( string text in items )
				{
					ListViewItem lvi = new ListViewItem(text);
					lveQueryColumn.Items.Add(lvi);
				}

				queryTableNames.Add(tableName);

				ToolStripMenuEnable(true);

				api.EnableScrollBar(lveQueryColumn.Handle, (uint)api.SBFlags.SB_VERT, (uint)api.SBArrows.ESB_DISABLE_BOTH);
			}
			else
			{
				if ( HasQueryColumn(tableName, null, 0x0001) == -1 )
				{
					queryTableNames.Add(tableName);
				}

				lveQueryColumn.Columns.Add(tableName, columnWidth, HorizontalAlignment.Left);

				for ( int i = 0; i < items.Length; i++ )
				{
					lveQueryColumn.Items[i].SubItems.Add(items[i]);
				}
			}

			toolStripStatusColumnCount.Text = (int.Parse(toolStripStatusColumnCount.Text) + 1).ToString();
			return 1;
		}
#endif

		/// <summary>
		/// クエリー項目を削除する
		/// </summary>
		/// <param name="column"></param>
		private void RemoveQueryColumn(int column)
		{
			try
			{
				if ( lveQueryColumn.Columns.Count == 1 )
				{
					//ToolStripMenuEnable(false);

					lveQueryColumn.Columns.Clear();
					lveQueryColumn.Items.Clear();
					queryTableNames = new List<string>();

					lvTableJoin.Items.Clear();
				}
				else
				{
					string tableName = lveQueryColumn.Columns[column].Text;
					string fieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[column].Text;

					foreach ( ListViewItem lvi in lveQueryColumn.Items )
					{
						lvi.SubItems.RemoveAt(column);
					}

					lveQueryColumn.Columns.RemoveAt(column);

					if ( HasQueryColumn(tableName, null, 0x0001) == -1 )
					{
						queryTableNames.Remove(tableName);
					}

					// テーブル結合があれば解除する
					if ( lvTableJoin.Items.Count != 0 )
					{
						if ( HasQueryColumn(tableName, fieldName, 0) == -1 )
						{
							int i = HasTableJoin(tableName + "." + fieldName, null, 1);
							if ( i != -1 )
							{
								lvTableJoin.Items.RemoveAt(i);
							}
						}
					}
				}

				lastQueryColumn = -1;
				//ChangeModified(!(lveQueryColumn.Columns.Count == 0));
				if ( lveQueryColumn.Columns.Count != 0 )
				{
					ChangeModified(true);
				}
				else
				{
					bool empty = (textSQL.Text.Length == 0);
					ToolStripMenuEnable(!empty);
					ChangeModified(!empty);
				}

				toolStripStatusColumnCount.Text = (int.Parse(toolStripStatusColumnCount.Text) - 1).ToString();
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// 既に選択済みのテーブル結合があるかチェックする
		/// </summary>
		/// <param name="leftTableColumn"></param>
		/// <param name="rightTableColumn"></param>
		/// <param name="hasMode">
		/// 1:[テーブル].[カラム]が既に左右どちらかに存在済みかチェックする
		/// 2:[テーブル].[カラム]のペアが既に存在済みかチェックする
		/// </param>
		/// <returns></returns>
		private int HasTableJoin(string leftTableColumn, string rightTableColumn, int hasMode)
		{
			if ( hasMode == 1 )
			{
				for ( int i = 0; i < lvTableJoin.Items.Count; i++ )
				{
					ListViewItem lvi = lvTableJoin.Items[i];
					if ( (leftTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text) || (leftTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text) )
						return i;
				}
			}
			else if ( hasMode == 2 )
			{
				for ( int i = 0; i < lvTableJoin.Items.Count; i++ )
				{
					ListViewItem lvi = lvTableJoin.Items[i];
					if ( ((leftTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text) && (rightTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text)) ||
						 ((leftTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text) && (rightTableColumn == lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text)) )
						return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// カラムの表示領域を無効にする
		/// </summary>
		/// <param name="column"></param>
		private void InvalidateQueryColumn(int column)
		{
			try
			{
				int[] colOrder = lveQueryColumn.GetColumnOrder();
				Rectangle rect = lveQueryColumn.Items[0].SubItems[colOrder[column]].Bounds;
				rect.Height *= lveQueryColumn.ValidItemCount;
				lveQueryColumn.Invalidate(rect);
				Debug.WriteLine("InvalidateQueryColumn(" + column + ") " + rect);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// 編集状態を変更する
		/// </summary>
		/// <param name="status"></param>
		private void ChangeModified(bool status)
		{
			try
			{
				if ( modified = status )
				{
					if ( toolStripStatusFileName.Text[toolStripStatusFileName.Text.Length - 1] != '*' )
					{
						toolStripStatusFileName.Text += " *";
					}
				}
				else
				{
					if ( toolStripStatusFileName.Text[toolStripStatusFileName.Text.Length - 1] == '*' )
					{
						toolStripStatusFileName.Text = toolStripStatusFileName.Text.Substring(0, toolStripStatusFileName.Text.Length - 2);
					}
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目ファイルを読み込む
		/// <summary>
		/// <param name="fileName"></param>
		/// <param name="manageRecentFileName"></param>
		/// <returns></returns>
		private bool ReadShenlongColumnFile(string fileName, bool manageRecentFileName)
		{
			try
			{
				if ( modified )
				{
					if ( MyMessageBox.Show("編集中のクエリー項目を破棄しますか？", appTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes )
						return false;
				}

				XmlDocument xmlShenlongColumn = new XmlDocument();
				xmlShenlongColumn.Load(fileName);

				if ( xmlShenlongColumn[ShenGlobal.tagShenlong] == null )
				{
					throw new Exception("shenlong のクエリー項目ファイルではありません");
				}

				if ( !ChangeLogOn(xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrSID].Value, xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrUserName].Value) )
					return false;

				Cursor.Current = Cursors.WaitCursor;

				textColumnFilter.Text = string.Empty;
				//listBoxTableList.SelectedIndex = -1;
				listBoxTableList.SelectedItem = null;
				listBoxColumnList.Items.Clear();
				toolStripStatusColumnCount.Text = "0";

#if ENABLED_SUBQUERY
				RemoveSubQueryFromTableList();
#endif

				// クエリー項目
				lveQueryColumn.Columns.Clear();
				lveQueryColumn.Items.Clear();
				queryTableNames = new List<string>();
				lastQueryColumn = -1;
				ChangeModified(false);

				Version verShenColumn = new Version(0, 0);
				if ( xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrVer] != null )
				{
					verShenColumn = new Version(xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrVer].Value);
				}

				bool hasColumn = (xmlShenlongColumn.DocumentElement[ShenGlobal.tagColumn] != null);
				if ( hasColumn )
				{
					//lveQueryColumn.BeginUpdate();

					foreach ( XmlNode column in xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn) )
					{
						string[] subItemText = QueryColumnNodeToStrings(column);
						if ( AddQueryColumn(column.Attributes[ShenGlobal.attrTableName].Value, int.Parse(column.Attributes[ShenGlobal.attrWidth].Value), subItemText, -1/*true*/) != 1 )
							break;
					}

					//lveQueryColumn.EndUpdate();
				}

				// テーブル結合
				lvTableJoin.Items.Clear();

				foreach ( XmlNode tableJoin in xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagTableJoin) )
				{
					ListViewItem lvi = new ListViewItem(tableJoin.Attributes[ShenGlobal.tabJoin.leftTabCol.ToString()].Value);
#if COLLECT_OUTER_JOIN
					string way = tableJoin.Attributes[ShenGlobal.tabJoin.way.ToString()].Value;
					if ( verShenColumn <= new Version(1, 13) )	// Version 1.13 以前の外部結合は逆向きにする
					{
						way = (way == "<=") ? ">=" : ((way == ">=") ? "<=" : way);
					}
					lvi.SubItems.Add(way);
#else
					lvi.SubItems.Add(tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value);
#endif
					lvi.SubItems.Add(tableJoin.Attributes[ShenGlobal.tabJoin.rightTabCol.ToString()].Value);
					lvTableJoin.Items.Add(lvi);
				}

				// SQL
				XmlNode sql = xmlShenlongColumn.DocumentElement[ShenGlobal.tagSQL];
				if ( sql != null )
				{
					textSQL.Text = sql.InnerText.Replace("<br>", "\r\n");
					modified = false;
				}

				// ファイルのプロパティ
				GetFileProperty(xmlShenlongColumn);

				ShenlongColumnFileNameManager(fileName, manageRecentFileName);

				//fileDlgIniDir = Path.GetDirectoryName(xmlShenlongColumnFileName);

				ToolStripMenuEnable(hasColumn || (textSQL.Text.Length != 0));

				//tabControl.SelectedIndex = 0;
				tabControl.SelectedTab = (hasColumn || textSQL.Text.Length == 0) ? tabQueryColumn : tabSQL;

				latestSelectParams = null;

				SaveBaseURI(fileName);

				SetEnableSameColumnAppend(false);

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
			finally
			{
				//lveQueryColumn.EndUpdate();
			}
		}

		private bool ReadShenlongColumnFile(string fileName, bool manageRecentFileName, bool tempAutoChangeLogOn)
		{
			bool _autoChangeLogOn = autoChangeLogOn;
			autoChangeLogOn = tempAutoChangeLogOn;

			bool result = ReadShenlongColumnFile(fileName, manageRecentFileName);

			autoChangeLogOn = _autoChangeLogOn;

			return result;
		}

		/// <summary>
		/// SQL ファイルを読み込む
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="manageRecentFileName"></param>
		/// <returns></returns>
		private bool ReadSqlFile(string fileName, bool manageRecentFileName)
		{
			try
			{
				if ( modified )
				{
					if ( MyMessageBox.Show("編集中のクエリー項目を破棄しますか？", appTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes )
						return false;
				}

				ClearQueryColumn();

				byte[] sql;

				using ( FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read) )
				{
					sql = new byte[fs.Length];
					//byte配列に読み込む
					fs.Read(sql, 0, sql.Length);
					fs.Close();
				}

				//文字コードを取得する
				Encoding encoding = GetCode(sql);

				tabControl.SelectedTab = tabSQL;
				//textSQL.Text = File.ReadAllText(fileName, sjisEnc);
				textSQL.Text = encoding.GetString(sql);	// デコードして表示する
				modified = false;

				ShenlongColumnFileNameManager(fileName, manageRecentFileName);

				textSQL.Select(0, 0);
				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}

		/// <summary>
		/// 文字コードを判別する
		/// </summary>
		/// <remarks>
		/// Jcode.pmのgetcodeメソッドを移植したものです。
		/// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
		/// </remarks>
		/// <param name="byts">文字コードを調べるデータ</param>
		/// <returns>適当と思われるEncodingオブジェクト。
		/// 判断できなかった時はnull。</returns>
		public static Encoding GetCode(byte[] byts)
		{
			try
			{
				const byte bESC = 0x1B;
				const byte bAT = 0x40;
				const byte bDollar = 0x24;
				const byte bAnd = 0x26;
				const byte bOP = 0x28;    //(
				const byte bB = 0x42;
				const byte bD = 0x44;
				const byte bJ = 0x4A;
				const byte bI = 0x49;

				int len = byts.Length;
				int binary = 0;
				int ucs2 = 0;
				int sjis = 0;
				int euc = 0;
				int utf8 = 0;
				byte b1, b2;

				for ( int i = 0; i < len; i++ )
				{
					if ( byts[i] <= 0x06 || byts[i] == 0x7F || byts[i] == 0xFF )
					{
						//'binary'
						binary++;
						if ( len - 1 > i && byts[i] == 0x00
							&& i > 0 && byts[i - 1] <= 0x7F )
						{
							//smells like raw unicode
							ucs2++;
						}
					}
				}

				if ( binary > 0 )
				{
					if ( ucs2 > 0 )
						//JIS
						//ucs2(Unicode)
						return Encoding.Unicode;
					else
						//binary
						return null;
				}

				for ( int i = 0; i < len - 1; i++ )
				{
					b1 = byts[i];
					b2 = byts[i + 1];

					if ( b1 == bESC )
					{
						if ( b2 >= 0x80 )
							//not Japanese
							//ASCII
							return System.Text.Encoding.ASCII;
						else if ( len - 2 > i &&
							b2 == bDollar && byts[i + 2] == bAT )
							//JIS_0208 1978
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						else if ( len - 2 > i &&
							b2 == bDollar && byts[i + 2] == bB )
							//JIS_0208 1983
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						else if ( len - 5 > i &&
							b2 == bAnd && byts[i + 2] == bAT && byts[i + 3] == bESC &&
							byts[i + 4] == bDollar && byts[i + 5] == bB )
							//JIS_0208 1990
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						else if ( len - 3 > i &&
							b2 == bDollar && byts[i + 2] == bOP && byts[i + 3] == bD )
							//JIS_0212
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						else if ( len - 2 > i &&
							b2 == bOP && (byts[i + 2] == bB || byts[i + 2] == bJ) )
							//JIS_ASC
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
						else if ( len - 2 > i &&
							b2 == bOP && byts[i + 2] == bI )
							//JIS_KANA
							//JIS
							return System.Text.Encoding.GetEncoding(50220);
					}
				}

				for ( int i = 0; i < len - 1; i++ )
				{
					b1 = byts[i];
					b2 = byts[i + 1];
					if ( ((b1 >= 0x81 && b1 <= 0x9F) || (b1 >= 0xE0 && b1 <= 0xFC)) &&
						((b2 >= 0x40 && b2 <= 0x7E) || (b2 >= 0x80 && b2 <= 0xFC)) )
					{
						sjis += 2;
						i++;
					}
				}
				for ( int i = 0; i < len - 1; i++ )
				{
					b1 = byts[i];
					b2 = byts[i + 1];
					if ( ((b1 >= 0xA1 && b1 <= 0xFE) && (b2 >= 0xA1 && b2 <= 0xFE)) ||
						(b1 == 0x8E && (b2 >= 0xA1 && b2 <= 0xDF)) )
					{
						euc += 2;
						i++;
					}
					else if ( len - 2 > i &&
						b1 == 0x8F && (b2 >= 0xA1 && b2 <= 0xFE) &&
						(byts[i + 2] >= 0xA1 && byts[i + 2] <= 0xFE) )
					{
						euc += 3;
						i += 2;
					}
				}
				for ( int i = 0; i < len - 1; i++ )
				{
					b1 = byts[i];
					b2 = byts[i + 1];
					if ( (b1 >= 0xC0 && b1 <= 0xDF) && (b2 >= 0x80 && b2 <= 0xBF) )
					{
						utf8 += 2;
						i++;
					}
					else if ( len - 2 > i &&
						(b1 >= 0xE0 && b1 <= 0xEF) && (b2 >= 0x80 && b2 <= 0xBF) &&
						(byts[i + 2] >= 0x80 && byts[i + 2] <= 0xBF) )
					{
						utf8 += 3;
						i += 2;
					}
				}

				if ( euc > sjis && euc > utf8 )
					//EUC
					return Encoding.GetEncoding(51932);
				else if ( sjis > euc && sjis > utf8 )
					//SJIS
					return Encoding.GetEncoding(932);
				else if ( utf8 > euc && utf8 > sjis )
					//UTF8
					return Encoding.UTF8;

				return Encoding.GetEncoding("shift_jis")/*null*/;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
				return Encoding.GetEncoding("shift_jis");
			}
		}

		/// <summary>
		/// ファイルのプロパティを初期化する
		/// </summary>
		private void InitFileProperty()
		{
			fileComment = string.Empty;
			fileAuthor = string.Empty;
			fileDistinct = false;
			fileUseJoin = false;
			fileHeaderOutput = ((int)ShenGlobal.header.columnName | (int)ShenGlobal.header.comment);
			fileDownLoad = false;
			fileEggPermission = string.Empty;
			fileMaxRowNum = string.Empty;
			fileSetValue = false;
			fileSqlSelect = false;
#if ENABLED_SUBQUERY
			fileSubQuery = new List<string>();
#endif
		}
	
		/// <summary>
		/// ファイルのプロパティを取得する
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		private void GetFileProperty(XmlDocument xmlShenlongColumn)
		{
			InitFileProperty();

			XmlNode fileProperty = xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty];
			if ( fileProperty == null )
				return;

			if ( fileProperty[ShenGlobal.tagComment] != null )
			{
				fileComment = fileProperty[ShenGlobal.tagComment].InnerText;
			}

			if ( fileProperty[ShenGlobal.tagAuthor] != null )
			{
				fileAuthor = fileProperty[ShenGlobal.tagAuthor].InnerText;
			}

			if ( fileProperty[ShenGlobal.tagDistinct] != null )
			{
				fileDistinct = bool.Parse(fileProperty[ShenGlobal.tagDistinct].InnerText);
			}

			if ( fileProperty[ShenGlobal.tagUseJoin] != null )
			{
				fileUseJoin = bool.Parse(fileProperty[ShenGlobal.tagUseJoin].InnerText);
			}

			if ( fileProperty[ShenGlobal.tagHeaderOutput] != null )
			{
				fileHeaderOutput = int.Parse(fileProperty[ShenGlobal.tagHeaderOutput].InnerText);
			}

			if ( fileProperty[ShenGlobal.tagDownload] != null )
			{
				fileDownLoad = (fileProperty[ShenGlobal.tagDownload].InnerText == ShenGlobal.authority.permit.ToString());
			}

			if ( fileProperty[ShenGlobal.tagEggPermission] != null )
			{
				fileEggPermission = fileProperty[ShenGlobal.tagEggPermission].InnerText;
			}

			if ( fileProperty[ShenGlobal.tagMaxRowNum] != null )
			{
				fileMaxRowNum = fileProperty[ShenGlobal.tagMaxRowNum].InnerText;
			}

			if ( fileProperty[ShenGlobal.tagSetValue] != null )
			{
				fileSetValue = bool.Parse(fileProperty[ShenGlobal.tagSetValue].InnerText);
			}

			if ( fileProperty[ShenGlobal.tagSqlSelect] != null )
			{
				fileSqlSelect = bool.Parse(fileProperty[ShenGlobal.tagSqlSelect].InnerText);
			}

#if ENABLED_SUBQUERY
			if ( (fileProperty[ShenGlobal.tagSubQuery] != null) && (fileProperty[ShenGlobal.tagSubQuery].InnerText.Length != 0) )
			{
				string shenColumnBaseURI = (Path.GetFileName(xmlShenlongColumn.BaseURI)[0] != '~') ? xmlShenlongColumn.BaseURI : GetLatestBaseURI();
				if ( shenColumnBaseURI.StartsWith("file:") )
				{
					shenColumnBaseURI = shenColumnBaseURI.Substring(5);	// 5:file:
					if ( shenColumnBaseURI.StartsWith("///") )
					{
						shenColumnBaseURI = shenColumnBaseURI.Substring(3);
					}
				}
				shenColumnBaseURI = System.Web.HttpUtility.UrlDecode(shenColumnBaseURI);

				foreach ( string subQuery in fileProperty[ShenGlobal.tagSubQuery].InnerText.Split(ShenGlobal.SUBQUERY_SEPARATOR) )
				{
					if ( fileSubQuery.IndexOf(subQuery) == -1 )
					{
						fileSubQuery.Add(subQuery);
					}

					AppendSubQueryToTableList(subQuery, shenColumnBaseURI);
				}
			}
#endif
		}

		/// <summary>
		/// CheckShenlongColumnFileExtension
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private string CheckShenlongColumnFileExtension(string fileName)
		{
			if ( string.Compare(Path.GetExtension(fileName), ".xml", true) != 0 )
			{
				fileName = fileName + ".xml";
			}

			return fileName;
		}

		/// <summary>
		/// クエリー項目ファイルを保存する
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="xmlShenlongColumnFileName"></param>
		private void SaveShenlongColumnFile(string _xmlShenlongColumnFileName, XmlDocument xmlShenlongColumn)
		{
#if ENABLED_SUBQUERY
			XmlNode fileProperty = xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty];
			if ( fileProperty != null )
			{
				if ( (fileProperty[ShenGlobal.tagSubQuery] != null) && (fileProperty[ShenGlobal.tagSubQuery].InnerText.Length != 0) )
				{
					string[] subQueries = fileProperty[ShenGlobal.tagSubQuery].InnerText.Split(ShenGlobal.SUBQUERY_SEPARATOR);
					for ( int i = 0; i < subQueries.Length; i++ )
					{
						if ( !subQueries[i].StartsWith(ShenGlobal.SUBQUERY_RELATIVE_PATH) )
						{
							// 絶対パスを相対パスに変換する
							subQueries[i] = subQueries[i].Replace(Path.GetDirectoryName(_xmlShenlongColumnFileName), ShenGlobal.SUBQUERY_RELATIVE_PATH);
						}
					}

					fileProperty[ShenGlobal.tagSubQuery].InnerText = string.Join(ShenGlobal.SUBQUERY_SEPARATOR.ToString(), subQueries);
				}
			}
#endif

			xmlShenlongColumn.Save(_xmlShenlongColumnFileName);

			SaveBaseURI(_xmlShenlongColumnFileName);
		}

		/// <summary>
		/// INI ファイルに保存された前回の baseURI を取得する
		/// </summary>
		/// <returns></returns>
		private string GetLatestBaseURI()
		{
			StringBuilder returnedString = new StringBuilder(1024);
			api.GetPrivateProfileString(RESUME_SECTION, KEY_BASE_URI, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName);
			return returnedString.ToString();
		}

		/// <summary>
		/// baseURI を INI ファイルに保存しておく
		/// </summary>
		/// <param name="baseURI"></param>
		private void SaveBaseURI(string baseURI)
		{
#if ENABLED_SUBQUERY
			if ( !Program.isNewInstance )					// 最初のインスタンスではない？
				return;
			if ( baseURI != null )
			{
				string fileName = Path.GetFileName(baseURI);
				if ( fileName[0] == '~' )					// テンポラリ ファイル？
				{
					if ( fileName != xmlLatestQueryColumnFileName )
						return;

					if ( GetLatestBaseURI().Length != 0 )	// 以前にクエリー項目ファイル名の読み書きがあった？
						return;
				}
			}
#else
			baseURI = null;
#endif

			api.WritePrivateProfileString(RESUME_SECTION, KEY_BASE_URI, baseURI, shenlongIniFileName);
		}

		/// <summary>
		/// クエリー項目の column ノードを文字列に変換する
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		private string[] QueryColumnNodeToStrings(XmlNode column)
		{
			XmlNode fieldName = column[ShenGlobal.qc.fieldName.ToString()];

			string[] property = new string[(int)ShenGlobal.prop.count];
			// <comments> から <comment> に変更した為の対策 /* 2008/03/10 */
			string comment = (column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.comment.ToString()] != null) ? column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.comment.ToString()].InnerText : column[ShenGlobal.qc.property.ToString()]["comments"].InnerText;

			property[(int)ShenGlobal.prop.type] = fieldName.Attributes[ShenGlobal.prop.type.ToString()].Value;
			property[(int)ShenGlobal.prop.length] = fieldName.Attributes[ShenGlobal.prop.length.ToString()].Value;
			property[(int)ShenGlobal.prop.nullable] = fieldName.Attributes[ShenGlobal.prop.nullable.ToString()].Value;
			property[(int)ShenGlobal.prop.comment] = comment/*column[ShenCore.qc.property.ToString()][ShenCore.prop.comments.ToString()].InnerText*/;
			XmlNode alias = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.alias.ToString()];
			if ( alias != null )
			{
				property[(int)ShenGlobal.prop.alias] = alias.InnerText;
			}
			XmlNode dateFormat = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.dateFormat.ToString()];
			if ( dateFormat != null )
			{
				property[(int)ShenGlobal.prop.dateFormat] = dateFormat.InnerText;
			}
			XmlNode bubbles = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.bubbles.ToString()];
			if ( bubbles != null )
			{
				property[(int)ShenGlobal.prop.bubbles] = ShenGlobal.BubblesSettingToString(bubbles);
			}

			//// <rightJoin> から <rColOp> に変更した為の対策 /* 2007/10/18 */
			//string rColOp = (column[ShenCore.qc.rColOp.ToString()] != null) ? column[ShenCore.qc.rColOp.ToString()].InnerText : column["rightJoin"].InnerText;
			//// <showField>する|しない から true|false に変更した為の対策 /* 2007/10/30 */
			//string showField = (column[ShenCore.qc.showField.ToString()].InnerText == "する") ? true.ToString().ToLower() : ((column[ShenCore.qc.showField.ToString()].InnerText == "しない") ? false.ToString().ToLower() : column[ShenCore.qc.showField.ToString()].InnerText);

			string[] subItemText = {
				fieldName.InnerText, column[ShenGlobal.qc.showField.ToString()].InnerText/*showField*/,
				column[ShenGlobal.qc.expression.ToString()].InnerText, column[ShenGlobal.qc.value1.ToString ()].InnerText,
				column[ShenGlobal.qc.value2.ToString()].InnerText, column[ShenGlobal.qc.rColOp.ToString()].InnerText/*rColOp*/,
				column[ShenGlobal.qc.orderBy.ToString()].InnerText, column[ShenGlobal.qc.groupFunc.ToString()].InnerText,
				string.Join(sepProp, property)};

			return subItemText;
		}

#if WITHIN_SHENGLOBAL
		/// <summary>
		/// バブルス設定を文字列に変換する
		/// </summary>
		/// <param name="bubbles"></param>
		/// <returns></returns>
		private static string BubblesSettingToString(XmlNode bubbles)
		{
			StringBuilder setting = new StringBuilder();			// enum bubbSet の順に取り出して格納する

			setting.Append(bubbles.Attributes[bubbSet.control.ToString()].Value);
			setting.Append(sepBubbSet);

			setting.Append((bubbles.Attributes[bubbSet.input.ToString()] != null) ? bubbles.Attributes[bubbSet.input.ToString()].Value : bubbInput.noAppoint.ToString());
			setting.Append(sepBubbSet);

			setting.Append((bubbles.Attributes[bubbSet.setValue.ToString()] != null) ? bubbles.Attributes[bubbSet.setValue.ToString()].Value : false.ToString());
			setting.Append(sepBubbSet);

			setting.Append((bubbles[bubbSet.dropDownList.ToString()] != null) ? bubbles[bubbSet.dropDownList.ToString()].InnerText : /*(bubbles["dropDownSql"] != null ? bubbles["dropDownSql"].InnerText : */string.Empty/*)*/);
			setting.Append(sepBubbSet);

			setting.Append((bubbles[bubbSet.hyperLink.ToString()] != null) ? bubbles[bubbSet.hyperLink.ToString()].InnerText : string.Empty);
			setting.Append(sepBubbSet);

			setting.Append((bubbles[bubbSet.classify.ToString()] != null) ? bubbles[bubbSet.classify.ToString()].InnerText : string.Empty);

			return setting.ToString();
		}
#endif

		/// <summary>
		/// ログオン先を変更する
		/// </summary>
		/// <returns></returns>
		private bool ChangeLogOn(string sid, string userName)
		{
			try
			{
				if ( IsEqualCurrentOraConn(sid, userName) )
					return true;

				LogOnDlg.usages usage = (autoChangeLogOn) ? LogOnDlg.usages.auto : LogOnDlg.usages.require;
				if ( OraLogOn(usage, sid, userName) != oraon.success )
					return false;

				SelectTableName();
				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
		}

		/// <summary>
		/// 現在の接続先と同じか否か比較する
		/// </summary>
		/// <param name="sid"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		private bool IsEqualCurrentOraConn(string sid, string userName)
		{
			try
			{
				string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
				return ((string.Compare(sid, logOn[(int)logon.sid].Trim(), true) == 0) && (string.Compare(userName, logOn[(int)logon.uid].Trim(), true) == 0));
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				return false;
			}
		}

		/// <summary>
		/// xmlShenlongColumnFileName を管理する
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="manageRecentFileName"></param>
		private void ShenlongColumnFileNameManager(string fileName, bool manageRecentFileName)
		{
			try
			{
				if ( Path.GetFileName(fileName)[0] == '~' )
				{
					xmlShenlongColumnFileName = null;
					toolStripStatusFileName.Text = "--";
					toolStripStatusFileName.ToolTipText = "";
					return;
				}

				xmlShenlongColumnFileName = fileName;
				toolStripStatusFileName.Text = Path.GetFileNameWithoutExtension(xmlShenlongColumnFileName);
				toolStripStatusFileName.ToolTipText = Path.GetDirectoryName(xmlShenlongColumnFileName);
				if ( !manageRecentFileName )
					return;

				int index;
				if ( (index = recentFileNames.IndexOf(fileName)) != -1 )
				{
					recentFileNames.RemoveAt(index);
				}
				else
				{
					if ( recentFileNames.Count == maxRecentFileName )
					{
						recentFileNames.RemoveAt(maxRecentFileName - 1);
					}
				}

				recentFileNames.Insert(0, fileName);

				RefreshRecentFileNameMenu();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// toolStripMenuRecentFileName メニューを再構築する
		/// </summary>
		private void RefreshRecentFileNameMenu()
		{
			try
			{
				toolStripSeparatorRecentFileName.Visible = (recentFileNames.Count != 0);

				int i;
				for ( i = 0; i < recentFileNames.Count; i++ )
				{
					string recentFileName = recentFileNames[i].ToString();
					toolStripMenuRecentFileNames[i].Tag = Path.GetDirectoryName(recentFileName);
					toolStripMenuRecentFileNames[i].Text = "&" + (i + 1) + " " + Path.GetFileName(recentFileName);
					toolStripMenuRecentFileNames[i].ToolTipText = recentFileName;
					toolStripMenuRecentFileNames[i].Visible = true;
				}
				for ( ; i < maxRecentFileName; i++ )
				{
					toolStripMenuRecentFileNames[i].Visible = false;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

#if true
		/// <summary>
		/// クエリーを実行して、結果を Excel に貼り付ける
		/// </summary>
		/// <param name="_showParamInputDlg"></param>
		private void StartQueryPasteExcel(bool _showParamInputDlg)
		{
			try
			{
				ShenGlobal.InitLogMessage();

				// Excel へ貼り付ける直前の状態をファイルに保存しておく
				XmlDocument xmlShenlongColumn;
				if ( !MakeQueryColumnXml(out xmlShenlongColumn, null) )
					return;

				string buildedSql, columnComments;
				List<string> logTableNames = new List<string>();	// アクセスログに保存するテーブル名

				if ( (tabControl.SelectedTab != tabSQL) || (textSQL.Text.Length == 0) )	// クエリー項目を使う？
				{
					if ( lveQueryColumn.Columns.Count == 0 )
						return;

					lveQueryColumn.EndEditing(false);

					if ( _showParamInputDlg )	// 抽出条件ダイアログを表示する？
					{
						ParamInputDlg paramInputDlg = new ParamInputDlg(xmlShenlongColumn, xmlShenlongColumnFileName ?? GetLatestBaseURI(), latestSelectParams, (string)toolStripStatusOraConn.Tag/*null*/, false);
						if ( !paramInputDlg.IsDisposed )
						{
							if ( paramInputDlg.ShowDialog(this) != DialogResult.OK )
							{
								if ( paramInputDlg.selectParams == null )
									latestSelectParams = null;
								return;
							}
							latestSelectParams = paramInputDlg.selectParams;
							paramInputDlg.Dispose();
						}
					}

					List<string> fromTableNames = new List<string>();
					if ( !BuildQueryColumnSQL((showParamInputDlg ? latestSelectParams : null), out buildedSql, out columnComments, ref fromTableNames) )
						return;

#if TABLE_NAME_HAS_ALIAS
					foreach ( string table in fromTableNames )
					{
						string tableName = ShenGlobal.GetTableName(table, true);
						if ( logTableNames.IndexOf(tableName) == -1 )
						{
							logTableNames.Add(tableName);
						}
					}
#else
					logTableNames = fromTableNames;
#endif
				}
				else
				{
					buildedSql = textSQL.Text.Trim();
					columnComments = null;
					if ( string.Compare(buildedSql, 0, "SELECT", 0, 6, true) != 0 && string.Compare(buildedSql, 0, "WITH", 0, 4, true) != 0 )
					{
						MyMessageBox.Show("SELECT,WITH 以外は指定できません", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						return;
					}

					//#if !ENABLED_SUBQUERY
					// SQL からテーブル名を抜き出す
					/*string[] tables = GetTableNameFromSQL(buildedSql);
					foreach ( string table in tables )
					{
						logTableNames.Add(table.Trim());
					}*/
					logTableNames = ShenGlobal.GetTableNameInSQL(buildedSql, true, true);
					//#endif
				}

				if ( !string.IsNullOrEmpty(buildedSql) )
				{
					XmlElement elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagBuildedSQL);	// <buildedSql>
					elem.InnerText = buildedSql.Replace("\r\n", "<br>");
					xmlShenlongColumn.DocumentElement.AppendChild(elem);
				}

				// クエリー項目ファイルを保存する
				//xmlShenlongColumn.Save(Application.StartupPath + "\\" + xmlLatestColBeforeExcelFileName);
				SaveShenlongColumnFile(Application.StartupPath + "\\" + xmlLatestColBeforeExcelFileName, xmlShenlongColumn);

				// クエリーを実行する
				buildedSql = buildedSql.Replace("\r\n", " ");
				ExecuteQueryDlg executeQueryDlg = new ExecuteQueryDlg(oraConn, buildedSql, pasteColumnComments ? columnComments : null, writeAccessLog ? logTableNames : null, (string)toolStripStatusOraConn.Tag);
				DialogResult result = executeQueryDlg.ShowDialog(this);
				if ( result != DialogResult.OK )
				{
					if ( result == DialogResult.No )
					{
						MyMessageBox.Show(executeQueryDlg.queryOutput.ToString(), "query error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
					return;
				}
				string queryOutput = executeQueryDlg.queryOutput.ToString();
				string[] dataTypeName = executeQueryDlg.dataTypeName;

				// クエリーの出力結果をファイルに保存する
				if ( saveQueryOutputFile )
				{
					string queryOutputFileName = (textQueryOutputFileName.StartsWith(@".\")) ? Application.StartupPath + @"\" + Path.GetFileName(textQueryOutputFileName) : textQueryOutputFileName;
					using ( StreamWriter swQueryOutput = new StreamWriter(queryOutputFileName, false, sjisEnc) )
					{
						swQueryOutput.Write(queryOutput);
					}
				}

				if ( pasteQueryResultToExcel != pasteExcel.none )
				{
					// クエリーの出力結果を Excel に貼り付ける
					QueryOutputToExcel(queryOutput, dataTypeName, executeQueryDlg.fileHeaderOutputed);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
#if (DEBUG)
				string logFileName = Application.StartupPath + "\\" + "~shenlong.log";
				ShenGlobal.SaveLogMessage(logFileName);
#endif
			}
		}
#else
		/// <summary>
		/// [Excel へ貼付(E)] メニュー
		/// </summary>
		private void toolStripMenuToExcel_Click(object sender, EventArgs e)
		{
			try
			{
				string buildedSql, columnComments;

				if ( (tabControl.SelectedTab != tabSQL) || (textSQL.Text.Length == 0) )	// [SQL] タブ以外が選択されている？
				{
					if ( lveQueryColumn.Columns.Count == 0 )
						return;

					lveQueryColumn.EndEditing(false);

					if ( !BuildQueryColumnSQL(out buildedSql, out columnComments) )
						return;
				}
				else
				{
					buildedSql = textSQL.Text.Trim();
					columnComments = null;
					if ( string.Compare(buildedSql, 0, "SELECT", 0, 6, true) != 0 )
					{
						MyMessageBox.Show("SELECT 以外は指定できません", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						return;
					}
				}

				// Excel へ貼り付ける直前の状態をファイルに保存しておく
				XmlDocument xmlShenlongColumn;
				if ( MakeQueryColumnXml(out xmlShenlongColumn, buildedSql) )
				{
					xmlShenlongColumn.Save(Application.StartupPath + "\\" + xmlLatestColBeforeExcelFileName);
				}

				// アクセスログに保存するテーブル名
				ArrayList logTableNames = null;
				if ( writeAccessLog )
				{
					logTableNames = new ArrayList();		// TABLE_NAME
					try
					{
						if ( tabControl.SelectedTab != tabSQL )
						{
#if TABLE_NAME_HAS_ALIAS
							foreach ( string table in queryTableNames )
							{
								string tableName = GetTableName(table, true);
								if ( logTableNames.IndexOf(tableName) == -1 )
								{
									logTableNames.Add(GetTableName(table, true));
								}
							}
#else
							logTableNames = queryTableNames;
#endif
						}
						else
						{
							string[] tables = GetTableNameFromSQL(textSQL.Text.Trim());	// SQL からテーブル名を抜き出す
							foreach ( string table in tables )
							{
								logTableNames.Add(table.Trim());
							}
						}
					}
					catch ( Exception exp )
					{
						logTableNames.Add(exp.Message);
					}
				}

				// クエリーを実行する
				ExecuteQueryDlg executeQueryDlg = new ExecuteQueryDlg(oraConn, buildedSql.Replace("\r\n", " "), pasteColumnComments ? columnComments : null, logTableNames);
				DialogResult result = executeQueryDlg.ShowDialog(this);
				if ( result != DialogResult.OK )
				{
					if ( result == DialogResult.No )
					{
						MyMessageBox.Show(executeQueryDlg.queryOutput.ToString(), "query error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
					return;
				}
				string queryOutput = executeQueryDlg.queryOutput.ToString();
				string[] dataTypeName = executeQueryDlg.dataTypeName;

				// クエリーの出力結果をファイルに保存する
				if ( saveQueryOutputFile )
				{
					string queryOutputFileName = (textQueryOutputFileName.StartsWith(@".\")) ? Application.StartupPath + @"\" + Path.GetFileName(textQueryOutputFileName) : textQueryOutputFileName;
					using ( StreamWriter swQueryOutput = new StreamWriter(queryOutputFileName, false, sjisEnc) )
					{
						swQueryOutput.Write(queryOutput);
					}
				}

				if ( pasteQueryResultToExcel != pasteExcel.none )
				{
					// クエリーの出力結果を Excel に貼り付ける
					QueryOutputToExcel(queryOutput, dataTypeName);
				}

#if false
				// アクセス ログをテーブルに保存する
				WriteAccessLog();
#endif
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}
#endif

		/// <summary>
		/// 選択されたクエリー項目を xml 化する
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		/// <param name="buildedSql"></param>
		/// <returns></returns>
		private bool MakeQueryColumnXml(out XmlDocument xmlShenlongColumn, string buildedSql)
		{
			xmlShenlongColumn = new XmlDocument();

			try
			{
				XmlDeclaration decl = xmlShenlongColumn.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlShenlongColumn.AppendChild(decl);

				XmlNode root = xmlShenlongColumn.CreateNode(XmlNodeType.Element, ShenGlobal.tagShenlong, null);	// <shenlong>
				xmlShenlongColumn.AppendChild(root);

				string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
				XmlAttribute attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.attrSID);						// @sid
				attr.Value = logOn[(int)logon.sid].Trim();
				root.Attributes.Append(attr);
				attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.attrUserName);								// @userName
				attr.Value = logOn[(int)logon.uid].Trim();
				root.Attributes.Append(attr);
				attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.attrVer);									// @ver
				Version ver = Assembly.GetExecutingAssembly().GetName().Version;
				attr.Value = ver.Major + "." + ver.Minor;
				root.Attributes.Append(attr);

				XmlElement elem, child;
				int[] colOrder = lveQueryColumn.GetColumnOrder();

				for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
				{
					int j = colOrder[i];

					string[] property = lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[j].Text.Split(sepProp[0]);

					XmlNode column = xmlShenlongColumn.CreateNode(XmlNodeType.Element, ShenGlobal.tagColumn, null);// <column>
					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.attrTableName);							// @tableName
					attr.Value = lveQueryColumn.Columns[j].Text;
					column.Attributes.Append(attr);
					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.attrWidth);								// @width
					attr.Value = lveQueryColumn.Columns[j].Width.ToString();
					column.Attributes.Append(attr);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.fieldName.ToString());					// <fieldName>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text;
					column.AppendChild(elem);
					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.prop.type.ToString());					// @type
					attr.Value = property[(int)ShenGlobal.prop.type];
					elem.Attributes.Append(attr);
					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.prop.length.ToString());				// @length
					attr.Value = property[(int)ShenGlobal.prop.length];
					elem.Attributes.Append(attr);
					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.prop.nullable.ToString());				// @nullable
					attr.Value = property[(int)ShenGlobal.prop.nullable];
					elem.Attributes.Append(attr);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.showField.ToString());					// <showField>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.showField].SubItems[j].Text;
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.expression.ToString());				// <expression>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.expression].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.value1.ToString());					// <value1>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.value1].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.value2.ToString());					// <value2>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.value2].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.rColOp.ToString());					// <rColOp>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.rColOp].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.orderBy.ToString());					// <orderBy>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.orderBy].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.groupFunc.ToString());					// <groupFunc>
					elem.InnerText = lveQueryColumn.Items[(int)ShenGlobal.qc.groupFunc].SubItems[j].Text;
					elem.IsEmpty = (elem.InnerText.Length == 0);
					column.AppendChild(elem);

					elem = xmlShenlongColumn.CreateElement(ShenGlobal.qc.property.ToString());					// <property>
					column.AppendChild(elem);

					child = xmlShenlongColumn.CreateElement(ShenGlobal.prop.comment.ToString());				//  <comment>
					child.InnerText = property[(int)ShenGlobal.prop.comment];
					elem.AppendChild(child);

					if ( property[(int)ShenGlobal.prop.alias].Length != 0 )
					{
						child = xmlShenlongColumn.CreateElement(ShenGlobal.prop.alias.ToString());				//  <alias>
						child.InnerText = property[(int)ShenGlobal.prop.alias];
						elem.AppendChild(child);
					}

					if ( property[(int)ShenGlobal.prop.dateFormat].Length != 0 )
					{
						child = xmlShenlongColumn.CreateElement(ShenGlobal.prop.dateFormat.ToString());			//  <dateFormat>
						child.InnerText = property[(int)ShenGlobal.prop.dateFormat];
						elem.AppendChild(child);
					}

					if ( property[(int)ShenGlobal.prop.bubbles].Length != 0 )
					{
						child = ShenGlobal.BubblesSettingToXml(property[(int)ShenGlobal.prop.bubbles], xmlShenlongColumn);	//  <bubbles>
						elem.AppendChild(child);
					}

					root.AppendChild(column);
				}

				foreach ( ListViewItem lvi in lvTableJoin.Items )
				{
					elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagTableJoin);					// <tableJoin>

					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.tabJoin.leftTabCol.ToString());	// @leftTabCol
					attr.Value = lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text;
					elem.Attributes.Append(attr);

					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.tabJoin.way.ToString());		// @way
					attr.Value = lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text;
					elem.Attributes.Append(attr);

					attr = xmlShenlongColumn.CreateAttribute(ShenGlobal.tabJoin.rightTabCol.ToString());// @rightTabCol
					attr.Value = lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text;
					elem.Attributes.Append(attr);

					root.AppendChild(elem);
				}

				elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagSQL);					// <sql>
				elem.InnerText = textSQL.Text.Replace("\r\n", "<br>");
				elem.IsEmpty = (elem.InnerText.Length == 0);
				root.AppendChild(elem);

				if ( buildedSql != null )
				{
					elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagBuildedSQL);		// <buildedSql>
					elem.InnerText = buildedSql.Replace("\r\n", "<br>");
					root.AppendChild(elem);
				}

				elem = FilePropertyToXml(xmlShenlongColumn);								// <property>
				root.AppendChild(elem);

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}

#if WITHIN_SHENGLOBAL
		/// <summary>
		/// バブルス設定を文字列を XmlElement に変換する
		/// </summary>
		/// <param name="setting"></param>
		/// <param name="xmlShenlongColumn"></param>
		/// <returns></returns>
		private static XmlElement BubblesSettingToXml(string setting, XmlDocument xmlShenlongColumn)
		{
			string[] settings = setting.Split(sepBubbSet);

			XmlElement bubbles = xmlShenlongColumn.CreateElement(ShenCore.prop.bubbles.ToString());	// <bubbles>

			XmlAttribute attr = xmlShenlongColumn.CreateAttribute(bubbSet.control.ToString());		// @control
			attr.Value = settings[(int)bubbSet.control];
			bubbles.Attributes.Append(attr);

			attr = xmlShenlongColumn.CreateAttribute(bubbSet.input.ToString());						// @input
			attr.Value = settings[(int)bubbSet.input];
			bubbles.Attributes.Append(attr);

			attr = xmlShenlongColumn.CreateAttribute(bubbSet.setValue.ToString());					// @setValue
			attr.Value = settings[(int)bubbSet.setValue];
			bubbles.Attributes.Append(attr);

			XmlElement elem = xmlShenlongColumn.CreateElement(bubbSet.dropDownList.ToString());		//   <dropDownList>
			elem.InnerText = settings[(int)bubbSet.dropDownList];
			elem.IsEmpty = (elem.InnerText.Length == 0);
			bubbles.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(bubbSet.hyperLink.ToString());					//   <hyperLink>
			elem.InnerText = settings[(int)bubbSet.hyperLink];
			elem.IsEmpty = (elem.InnerText.Length == 0);
			bubbles.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(bubbSet.classify.ToString());					//   <classify>
			elem.InnerText = settings[(int)bubbSet.classify];
			elem.IsEmpty = (elem.InnerText.Length == 0);
			bubbles.AppendChild(elem);

			return bubbles;
		}
#endif

		/// <summary>
		/// ファイルのプロパティを XmlNode に変換する
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		/// <returns></returns>
		private XmlElement FilePropertyToXml(XmlDocument xmlShenlongColumn)
		{
			XmlElement fileProperty = xmlShenlongColumn.CreateElement(ShenGlobal.tagProperty);		// <property>

			XmlElement elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagComment);				//   <comment>
			elem.InnerText = fileComment;
			elem.IsEmpty = (elem.InnerText.Length == 0);
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagAuthor);							//   <author>
			elem.InnerText = fileAuthor;
			elem.IsEmpty = (elem.InnerText.Length == 0);
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagDistinct);							//   <distinct>
			elem.InnerText = fileDistinct.ToString().ToLower();
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagUseJoin);							//   <useJoin>
			elem.InnerText = fileUseJoin.ToString().ToLower();
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagHeaderOutput);						//   <headerOutput>
			elem.InnerText = fileHeaderOutput.ToString();
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagDownload);							//   <download>
			elem.InnerText = ((fileDownLoad) ? ShenGlobal.authority.permit : ShenGlobal.authority.deny).ToString();
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagEggPermission);					//   <eggPermission>
			elem.InnerText = fileEggPermission;
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagMaxRowNum);						//   <maxRowNum>
			elem.InnerText = fileMaxRowNum;
			elem.IsEmpty = (elem.InnerText.Length == 0);
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagSetValue);							//   <setValue>
			elem.InnerText = fileSetValue.ToString().ToLower();
			fileProperty.AppendChild(elem);

			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagSqlSelect);						//   <sqlSelect>
			elem.InnerText = ((textSQL.Text.Length != 0) ? fileSqlSelect : false).ToString().ToLower();
			fileProperty.AppendChild(elem);

#if ENABLED_SUBQUERY
			elem = xmlShenlongColumn.CreateElement(ShenGlobal.tagSubQuery);							//   <subQuery>
			StringBuilder _fileSubQuery = new StringBuilder();
			foreach ( string subQuery in fileSubQuery )
			{
				if ( _fileSubQuery.Length != 0 )
				{
					_fileSubQuery.Append(ShenGlobal.SUBQUERY_SEPARATOR);
				}
				_fileSubQuery.Append(subQuery);
			}
			elem.InnerText = _fileSubQuery.ToString();
			elem.IsEmpty = (elem.InnerText.Length == 0);
			fileProperty.AppendChild(elem);
#endif

			return fileProperty;
		}

#if true
		/// <summary>
		/// 選択されたクエリー項目から SQL を構築する
		/// </summary>
		/// <param name="selectParams"></param>
		/// <param name="buildedSql"></param>
		/// <param name="columnComments"></param>
		/// <param name="fromTableNames"></param>
		/// <param name="indentCnt"></param>
		/// <returns></returns>
		private bool BuildQueryColumnSQL(Dictionary<string, string> selectParams, out string buildedSql, out string columnComments, ref List<string> fromTableNames/*, int indentCnt*/)
		{
			buildedSql = null;
			columnComments = null;

			int indentCnt = 0;

			try
			{
				string indent = new string(' ', indentCnt);
				StringBuilder select = new StringBuilder("SELECT\r\n");
				StringBuilder from = new StringBuilder("\r\n" + indent + "FROM\r\n");
				StringBuilder where = new StringBuilder("\r\n" + indent + "WHERE\r\n");
				StringBuilder groupBy = new StringBuilder("\r\n" + indent + "GROUP BY\r\n");
				StringBuilder orderBy = new StringBuilder("\r\n" + indent + "ORDER BY\r\n");
				int defSelect = select.Length;
				int defWhereLen = where.Length;
				int defGroupByLen = groupBy.Length;
				int defOrderByLen = orderBy.Length;

				Dictionary<string, int> paramNames = new Dictionary<string, int>();
				List<string> orders = new List<string>();
				StringBuilder colComments = new StringBuilder();
				int colCommentsCount = 0;
				int usersRndBktCount = 0;
				int groupFuncCount = 0;
				bool cameOR = false;
				indent += " ";

				int[] colOrder = lveQueryColumn.GetColumnOrder();

				for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
				{
					int j = colOrder[i];

#if TABLE_NAME_HAS_ALIAS
					string tableName = ShenGlobal.GetTableName(lveQueryColumn.Columns[j].Text, false);			// テーブル名
#else
					string tableName = lveQueryColumn.Columns[j].Text;								// テーブル名
#endif
					string fieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text;	// フィールド名
					string[] property = lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[j].Text.Split(sepProp[0]);// プロパティ
					string tableFieldName = (!fieldName.StartsWith(ShenGlobal.withoutTableName)) ? (tableName + "." + fieldName) : fieldName.Substring(ShenGlobal.withoutTableName.Length);

					int fieldAsIndex;
					string fieldAliasName;
					string plainTableFieldName = ShenGlobal.GetPlainTableFieldName(tableFieldName, out fieldAsIndex, out fieldAliasName);

					if ( fieldAsIndex == -1 )	// 直接の別名指定は無い？
					{
						if ( property[(int)ShenGlobal.prop.alias].Length != 0 )	// プロパティでの別名がある？
						{
							property[(int)ShenGlobal.prop.alias] = "\"" + property[(int)ShenGlobal.prop.alias] + "\"";

							fieldAliasName = property[(int)ShenGlobal.prop.alias];
							tableFieldName += " AS " + fieldAliasName;	// 直接指定の書式に変換しておく
							plainTableFieldName = ShenGlobal.GetPlainTableFieldName(tableFieldName, out fieldAsIndex);
						}
					}

					if ( bool.Parse(lveQueryColumn.Items[(int)ShenGlobal.qc.showField].SubItems[j].Text) )
					{
						string groupFunc = lveQueryColumn.Items[(int)ShenGlobal.qc.groupFunc].SubItems[j].Text;
						if ( !string.IsNullOrEmpty(groupFunc) )
						{
							tableFieldName = groupFunc + "(" + plainTableFieldName + ")" + ((fieldAsIndex != -1) ? tableFieldName.Substring(fieldAsIndex) : "");
							groupFuncCount++;
						}

#if false
						if ( property[(int)ShenCore.prop.type] == "DATE" )
						{
							select.Append(" " + "to_char(" + tableFieldName + ",'YYYY/MM/DD HH24:MI:SS') " + fieldName + ",\r\n");
						}
						else
						{
							select.Append(" " + tableFieldName + ",\r\n");
						}
#else
						select.Append(" " + tableFieldName + ",\r\n");
#endif

						colComments.Append(property[(int)ShenGlobal.prop.comment] + ShenGlobal.sepOutput);
						if ( property[(int)ShenGlobal.prop.comment] != ShenGlobal.propNoComment )
						{
							colCommentsCount++;
						}
					}

					tableFieldName = plainTableFieldName;

					// 条件式
					string expression = lveQueryColumn.Items[(int)ShenGlobal.qc.expression].SubItems[j].Text;
					string value1 = lveQueryColumn.Items[(int)ShenGlobal.qc.value1].SubItems[j].Text.Trim();
					string value2 = lveQueryColumn.Items[(int)ShenGlobal.qc.value2].SubItems[j].Text.Trim();
					string rColOp = lveQueryColumn.Items[(int)ShenGlobal.qc.rColOp].SubItems[j].Text;
					string leftRndBkt = "(", rightRndBkt = ")";
					string usersRoundBlanket = ShenGlobal.GetUsersRoundBlanket(ref value2);

					if ( (expression.Length != 0)/**/ && !string.IsNullOrEmpty(value1)/**/ )
					{
						string plainFieldName = ShenGlobal.GetPlainTableFieldName(fieldName);
						ShenGlobal.SetShenlongParam(selectParams, "", property[(int)ShenGlobal.prop.bubbles], tableName + "." + plainFieldName/*plainTableFieldName*/, ref paramNames, ref expression, ref value1, ref value2);

						// 値１の指定が無くなった時、rColOp を無効にして、余計なロジックを通らないようした。(2011/01/12)
						rColOp = (value1.Length == 0) ? null : rColOp;

						// ユーザー定義の開き括弧があれば設定する
						ShenGlobal.SetUsersRoundBlanket(usersRoundBlanket, indent, ref where, ref usersRndBktCount);
					}

					//string quotation = (property[(int)ShenCore.prop.type].StartsWith("VARCHAR")) ? "'" : "";
					string quotation = ShenGlobal.IsCharColumn(property[(int)ShenGlobal.prop.type]) ? "'" : "";

					if ( !string.IsNullOrEmpty(value1) && (property[(int)ShenGlobal.prop.type] == "DATE") )	// 日付の条件指定あり？
					{
						/*string toChar = (value1[0] == '(') ? "to_char" : "";
						string dateQuote = (Char.IsDigit(value1[0])) ? "'" : "";
						string _sqlDateFormat = ShenGlobal.sqlDateFormat;
						if ( (value1.IndexOf('/') != -1) && (_sqlDateFormat.IndexOf('/') == -1) )
						{
							_sqlDateFormat = "yyyy/mm/dd hh24:mi";
						}
						value1 = "to_date(" + toChar + dateQuote + value1 + dateQuote + ",'" + _sqlDateFormat + "')";*/
						value1 = ShenGlobal.ValueToDateFormat(value1, property[(int)ShenGlobal.prop.dateFormat]);
					}

					if ( rColOp != null )	// 有効な条件式？
					{
						if ( rColOp.Length == 0 )
						{
							rColOp = "AND";
						}

						// 連続した OR 条件の開き|閉じ括弧をセットする
						ShenGlobal.SetOrRoundBlanket(rColOp, expression, ref leftRndBkt, ref rightRndBkt, ref cameOR, ref where);
					}

					// =, NOT =, >=, <=, >, <
					if ( (expression.IndexOf('=') != -1 || expression == "<" || expression == ">") && !string.IsNullOrEmpty(value1) )
					{
						expression = (expression == "NOT =") ? "<>" : expression;
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " " + quotation + value1 + quotation + rightRndBkt + " " + rColOp + "\r\n");
					}
					// BETWEEN, NOT BETWEEN
					else if ( (expression.IndexOf("BETWEEN") != -1) && (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2)) )
					{
						if ( !string.IsNullOrEmpty(value2) && (property[(int)ShenGlobal.prop.type] == "DATE") )	// 日付の条件指定あり？
						{
							/*string toChar = (value2[0] == '(') ? "to_char" : "";
							string dateQuote = (Char.IsDigit(value2[0])) ? "'" : "";
							string _sqlDateFormat = ShenGlobal.sqlDateFormat;
							if ( (value1.IndexOf('/') != -1) && (_sqlDateFormat.IndexOf('/') == -1) )
							{
								_sqlDateFormat = "yyyy/mm/dd hh24:mi";
							}
							value2 = "to_date(" + toChar + dateQuote + value2 + dateQuote + ",'" + _sqlDateFormat + "')";*/
							value2 = ShenGlobal.ValueToDateFormat(value2, property[(int)ShenGlobal.prop.dateFormat]);
						}
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " " + quotation + value1 + quotation + " AND " + quotation + value2 + quotation + rightRndBkt + " " + rColOp + "\r\n");
					}
					// IN, NOT IN
					else if ( (expression.IndexOf("IN") != -1) && !string.IsNullOrEmpty(value1) )
					{
						string[] values = value1.Split(',');
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " (");
						for ( int k = 0; k < values.Length; k++ )
						{
							where.Append(quotation + values[k] + quotation + ((k != values.Length - 1) ? "," : ""));
						}
						where.Append(")" + rightRndBkt + " " + rColOp + "\r\n");
					}
					// LIKE, NOT LIKE
					else if ( (expression.IndexOf("LIKE") != -1) && !string.IsNullOrEmpty(value1) )
					{
						string wildCard = (value1.IndexOfAny(new char[] { '%', '_' }) == -1) ? "%" : "";
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " '" + value1 + wildCard + "'" + rightRndBkt + " " + rColOp + "\r\n");
					}
					// IS NULL, IS NOT NULL
					else if ( (expression.IndexOf("NULL") != -1) && string.IsNullOrEmpty(value1) )
					{
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + rightRndBkt + " " + rColOp + "\r\n");
					}

					// ユーザー定義の閉じ括弧があれば設定する
					ShenGlobal.SetUsersRoundBlanket(usersRoundBlanket, null, ref where, ref usersRndBktCount);

					// OR の途中でユーザー定義の閉じ括弧が設定された？
					if ( cameOR && ((usersRoundBlanket != null) && (where[where.Length - (1 + 1 + rColOp.Length + 2)] == '」')) )
					{
						// OR 条件を閉じ括弧でターミネートする
						ShenGlobal.TerminateOrRoundBlanket(ref cameOR, where.Length - (1 + 1 + rColOp.Length + 2), ref where);
					}

					// ソート順
					string order = lveQueryColumn.Items[(int)ShenGlobal.qc.orderBy].SubItems[j].Text.Trim();
					if ( !string.IsNullOrEmpty(order) )
					{
						int k, number;
						for ( k = 0; k < order.Length && Char.IsDigit(order[k]); k++ ) ;
						number = (Char.IsDigit(order[0])) ? int.Parse(order.Substring(0, k)) : 999;
#if UPDATE_20140729
						string option = order.Substring(k).Trim().ToUpper();	// desc 変数は実際には他のオプションも入る
						string desc = (option.Length != 0 ? " " : "") + option;
#else
						string desc = (order.IndexOf("DESC", k, StringComparison.CurrentCultureIgnoreCase) != -1) ? " DESC" : "";
#endif
#if true
						//string orderTableFieldName = (property[(int)ShenGlobal.prop.alias].Length == 0) ? tableFieldName : property[(int)ShenGlobal.prop.alias];
						string orderTableFieldName = fieldAliasName ?? tableFieldName;
						orders.Add(number.ToString("D3") + "\t" + orderTableFieldName + desc);
#else
						orders.Add(number.ToString("D3") + "\t" + tableFieldName + desc);
#endif
					}
				}

				if ( select.Length == defSelect )
				{
					MyMessageBox.Show("表示する項目が１つ以上必要です", appTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}

				if ( fileDistinct )
				{
					select.Insert(6, " DISTINCT");	// 6:SELECT
				}

#if !ENABLED_SUBQUERY
				// FROM テーブル名
				foreach ( string tableName in queryTableNames )
				{
					from.Append(" " + tableName + ",\r\n");
					fromTableNames.Add(tableName);
				}
#else
				List<ShenGlobal.fromJoin> fromJoins = null;

				if ( fileUseJoin )
				{
					// JOIN でテーブルを結合する
					fromJoins = new List<ShenGlobal.fromJoin>();

					foreach ( ListViewItem lvi in lvTableJoin.Items )
					{
						string leftTableName, leftColumnName, leftTableColumn;
						ShenGlobal.SplitTableFieldName(lvi.Text, out leftTableName, out leftColumnName, null/*false*/);
						leftTableColumn = ShenGlobal.GetPlainTableFieldName(!leftColumnName.StartsWith(ShenGlobal.withoutTableName) ? (ShenGlobal.GetTableName(leftTableName, false)/*leftTableName*/ + "." + leftColumnName) : leftColumnName.Substring(ShenGlobal.withoutTableName.Length));

						string way = lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text;

						string rightTableName, rightColumnName, rightTableColumn;
						ShenGlobal.SplitTableFieldName(lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text, out rightTableName, out rightColumnName, null/*false*/);
						rightTableColumn = ShenGlobal.GetPlainTableFieldName(!rightColumnName.StartsWith(ShenGlobal.withoutTableName) ? (ShenGlobal.GetTableName(rightTableName, false)/*rightTableName*/ + "." + rightColumnName) : rightColumnName.Substring(ShenGlobal.withoutTableName.Length));

						int j;
						// 新規の核となるテーブル？
						if ( (j = ShenGlobal.GetIndexOfJoinTableName(fromJoins, leftTableName, null)) == fromJoins.Count )
						{
							ShenGlobal.fromJoin fromJoin = new ShenGlobal.fromJoin("", "", leftTableName);
							fromJoins.Add(fromJoin);
						}

						// 新規の結合するテーブル？
						if ( (j = ShenGlobal.GetIndexOfJoinTableName(fromJoins, rightTableName, way)) == fromJoins.Count )
						{
							// 結合するテーブルを追加する
							string join = (way == "=" ? "INNER" : (way == "<=" ? "RIGHT OUTER" : (way == ">=" ? "LEFT OUTER" : (way == ">=<" ? "FULL OUTER" : "")))) + " JOIN ";
							string subQuery;
							Dictionary<string, string> _subQueryAlias = new Dictionary<string, string>();
							if ( (subQuery = ShenGlobal.IsTableNameSubQuery(rightTableName, fileSubQuery, ref _subQueryAlias)) != null )
							{
								XmlDocument _xmlShenlongColumn = ShenGlobal.ReadSubQueryFile(subQuery, GetSubQueryBaseURI(subQuery, xmlShenlongColumnFileName ?? GetLatestBaseURI()));
								string _buildedSql, _columnComments;
								if ( !ShenGlobal.BuildQueryColumnSQL(_xmlShenlongColumn, selectParams, out _buildedSql, out _columnComments, ref fromTableNames, indentCnt + 2) )
									return false;
								subQuery = "(" + _buildedSql + indent + ") " + ShenGlobal.GetSubQueryName(subQuery, _subQueryAlias);
							}
							ShenGlobal.fromJoin fromJoin = new ShenGlobal.fromJoin(join, way, rightTableName);
							fromJoin.subQuery = subQuery;
							fromJoins.Add(fromJoin);
						}

						// 結合するカラムを追加する
						fromJoins[j].equalColumn.Add(leftTableColumn + " = " + rightTableColumn);
					}

					// JOIN 句を使った SQL を構築する
					StringBuilder fromJoinSql = ShenGlobal.BuildFromJoinSql(fromJoins, indent, ref fromTableNames);

					if ( fromJoinSql.Length != 0 )	// JOIN するテーブルがある？
					{
						fromJoinSql.Insert(fromJoinSql.Length - 2, ",");	// 2:"\r\n"
						from.Append(fromJoinSql);
					}
				}

				// FROM テーブル名
				Dictionary<string, string> subQueryAlias = new Dictionary<string, string>();
				foreach ( string tableName in queryTableNames )
				{
					if ( fromJoins != null )
					{
						int j;
						for ( j = 0; (j < fromJoins.Count) && (tableName != fromJoins[j].tableName); j++ ) ;
						if ( j != fromJoins.Count )						// JOIN されたテーブル名？
							continue;
					}

					/*if ( fileSubQuery.Find(delegate(string s) { return s.IndexOf(tableName) != -1; }) != null )
						continue;*/
					/* サブクエリの別名対応 */
					if ( ShenGlobal.IsTableNameSubQuery(tableName, fileSubQuery, ref subQueryAlias) != null )
						continue;
					from.Append(indent + tableName + ",\r\n");
					fromTableNames.Add(tableName);
				}

				// サブクエリ
				foreach ( string subQuery in fileSubQuery )
				{
					if ( fromJoins != null )
					{
						string _subQuery = Path.GetFileNameWithoutExtension(subQuery);
						int j;
						for ( j = 0; (j < fromJoins.Count) && (_subQuery != ShenGlobal.GetTableName(fromJoins[j].tableName, true)); j++ ) ;
						if ( j != fromJoins.Count )						// JOIN されたサブクエリ？
							continue;
					}

					XmlDocument _xmlShenlongColumn = ShenGlobal.ReadSubQueryFile(subQuery, GetSubQueryBaseURI(subQuery, xmlShenlongColumnFileName ?? GetLatestBaseURI()));
					string _buildedSql, _columnComments;
					if ( !ShenGlobal.BuildQueryColumnSQL(_xmlShenlongColumn, selectParams, out _buildedSql, out _columnComments, ref fromTableNames, indentCnt + 2) )
						return false;
					/*from.Append(indent + "(" + _buildedSql + indent + ") " + Path.GetFileNameWithoutExtension(subQuery) + ",\r\n");*/
					/* サブクエリの別名対応 */
					from.Append(indent + "(" + _buildedSql + indent + ") ");
					from.Append(ShenGlobal.GetSubQueryName(subQuery, subQueryAlias));
					from.Append(",\r\n");
				}
#endif

				if ( groupFuncCount != 0 )	// グループ関数の指定あり？
				{
					// GROUP BY
					List<string> groupFields = new List<string>();
					for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
					{
						int j = colOrder[i];
						if ( !bool.Parse(lveQueryColumn.Items[(int)ShenGlobal.qc.showField].SubItems[j].Text) )
							continue;
						if ( !string.IsNullOrEmpty(lveQueryColumn.Items[(int)ShenGlobal.qc.groupFunc].SubItems[j].Text) )
							continue;

#if TABLE_NAME_HAS_ALIAS
#if UPDATE_20131204
						string tableName = ShenGlobal.GetTableName(lveQueryColumn.Columns[j].Text, false);			// テーブル名
						string fieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text;		// フィールド名
						string tableFieldName = (!fieldName.StartsWith(ShenGlobal.withoutTableName)) ? (tableName + "." + fieldName) : fieldName.Substring(ShenGlobal.withoutTableName.Length);
#else
						string tableFieldName = ShenGlobal.GetPlainTableFieldName(ShenGlobal.GetTableName(lveQueryColumn.Columns[j].Text, false) + "." + lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text);
#endif
#else
						string tableFieldName = GetPlainTableFieldName(lveQueryColumn.Columns[j].Text + "." + lveQueryColumn.Items[(int)ShenCore.qc.fieldName].SubItems[j].Text);
#endif
						if ( groupFields.IndexOf(tableFieldName) != -1 )
							continue;
						groupFields.Add(tableFieldName);
					}

					if ( groupFields.Count != 0 )
					{
						foreach ( string groupField in groupFields )
						{
							groupBy.Append(indent + groupField + ",\r\n");
						}
						groupBy.Length -= (1 + 2);	// (1 + 2):",\r\n"
					}
				}

				if ( orders.Count != 0 )	// ソートの指定あり？
				{
					// ORDER BY
					orders.Sort();
					foreach ( string order in orders )
					{
						orderBy.Append(indent + order.Substring(3 + 1) + ",\r\n");	// (3 + 1):ソート順\t
					}
					orderBy.Length -= (1 + 2);	// (1 + 2):",\r\n"
				}

				// WHERE
				if ( defWhereLen < where.Length )
				{
					if ( !fileUseJoin )
					{
						where.Insert(defWhereLen, indent + "(");
						where.Remove(defWhereLen + indent.Length + 1, indent.Length);
					}
					int lastSpace;
					for ( lastSpace = where.Length - 1; where[lastSpace] != ' '; lastSpace-- ) ;
					where.Remove(lastSpace + 1, where.Length - lastSpace - 1);		// "AND|OR\r\n" を削除する
					if ( cameOR )
					{
						// OR 条件を閉じ括弧でターミネートする
						ShenGlobal.TerminateOrRoundBlanket(ref cameOR, lastSpace++, ref where);
					}
					if ( !fileUseJoin )
					{
						where.Insert(lastSpace, ")");
					}

					// エンコードされたユーザー定義の括弧をデコードする
					ShenGlobal.DecodeUsersRoundBlanket(usersRndBktCount, ref where);

					/*if ( groupFuncCount != 0 )
					{
						// HAVING
						groupBy.Append("\r\nHAVING\r\n" + where.ToString().Substring(defWhereLen));
						where = new StringBuilder("\r\nWHERE\r\n");
					}*/
				}

				if ( !fileUseJoin )
				{
					// テーブル結合
					foreach ( ListViewItem lvi in lvTableJoin.Items )
					{
						if ( (lvi.Index == 0) && (defWhereLen != where.Length) )
						{
							where.Append("AND\r\n");
						}

#if TABLE_NAME_HAS_ALIAS
						string leftTableName, leftColumnName, leftTableColumn;
						ShenGlobal.SplitTableFieldName(lvi.Text, out leftTableName, out leftColumnName, false);
						//leftTableColumn = GetPlainTableFieldName(leftTableName + "." + leftColumnName);
						leftTableColumn = ShenGlobal.GetPlainTableFieldName(!leftColumnName.StartsWith(ShenGlobal.withoutTableName) ? (leftTableName + "." + leftColumnName) : leftColumnName.Substring(ShenGlobal.withoutTableName.Length));

						string rightTableName, rightColumnName, rightTableColumn;
						ShenGlobal.SplitTableFieldName(lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text, out rightTableName, out rightColumnName, false);
						//rightTableColumn = GetPlainTableFieldName(rightTableName + "." + rightColumnName);
						rightTableColumn = ShenGlobal.GetPlainTableFieldName(!rightColumnName.StartsWith(ShenGlobal.withoutTableName) ? (rightTableName + "." + rightColumnName) : rightColumnName.Substring(ShenGlobal.withoutTableName.Length));

						where.Append(" (");
#if COLLECT_OUTER_JOIN
						where.Append(leftTableColumn + ((lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text == "<=") ? "(+)" : ""));	// 右外部結合(RIGHT [OUTER] JOIN)
						where.Append(" = ");
						where.Append(rightTableColumn + ((lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text == ">=") ? "(+)" : ""));	// 左外部結合(LEFT [OUTER] JOIN)
#else
						where.Append(leftTableColumn + ((lvi.SubItems[(int)ShenCore.tabJoin.way].Text == ">=") ? " (+)" : ""));
						where.Append(" = ");
						where.Append(rightTableColumn + ((lvi.SubItems[(int)ShenCore.tabJoin.way].Text == "<=") ? " (+)" : ""));
#endif
						where.Append(") ");
#else
						where.Append(" (" + GetPlainTableFieldName(lvi.Text) + (lvi.SubItems[(int)ShenCore.tabJoin.way].Text == ">=" ? " (+)" : ""));
						where.Append(" = ");
						where.Append(GetPlainTableFieldName(lvi.SubItems[(int)ShenCore.tabJoin.rightTabCol].Text) + (lvi.SubItems[(int)ShenCore.tabJoin.way].Text == "<=" ? " (+)" : ""));
						where.Append(") ");
#endif

						if ( lvi.Index != lvTableJoin.Items.Count - 1 )
						{
							where.Append("AND\r\n");
						}
					}
				}

				buildedSql = select.ToString(0, select.Length - (1 + 2)) + " " +	// (1 + 2):",\r\n"
							 from.ToString(0, from.Length - (1 + 2)) + " " +		// (1 + 2):",\r\n"
							 ((where.Length == defWhereLen) ? "" : where.ToString()) +
							 ((groupBy.Length == defGroupByLen) ? "" : groupBy.ToString()) +
							 ((orderBy.Length == defOrderByLen) ? "" : orderBy.ToString()) +
							 "\r\n";

				if ( colCommentsCount != 0 )
				{
					colComments.Length--;
					columnComments = colComments.ToString();
				}

#if (DEBUG)
				ShenGlobal.LogMessage("[" + MethodBase.GetCurrentMethod().Name + "]", ShenGlobal.mout.strb);
				foreach ( string tableName in fromTableNames )
				{
					ShenGlobal.LogMessage(tableName, ShenGlobal.mout.strb);
				}
				ShenGlobal.LogMessage("", ShenGlobal.mout.strb);
#endif
				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}
#else
		/// <summary>
		/// 選択されたクエリー項目から SQL を構築する
		/// </summary>
		/// <param name="buldedSql"></param>
		/// <param name="columnComments"></param>
		/// <param name="dataTypeName"></param>
		/// <returns></returns>
		private bool BuildQueryColumnSQL(out string buldedSql, out string columnComments/*, out string [] dataTypeName*/)
		{
			buldedSql = null;
			columnComments = null;
			//dataTypeName = null;

			try
			{
				StringBuilder select = new StringBuilder("SELECT\r\n");
				StringBuilder from = new StringBuilder("\r\nFROM\r\n");
				StringBuilder where = new StringBuilder("\r\nWHERE\r\n");
				StringBuilder orderBy = new StringBuilder("\r\nORDER BY\r\n");
				int defWhereLen = where.Length;
				int defOrderByLen = orderBy.Length;

				ArrayList orders = new ArrayList();
				//dataTypeName = new string[lveQueryColumn.Columns.Count];
				StringBuilder colComments = new StringBuilder();
				int colCommentsCount = 0;

				int[] colOrder = lveQueryColumn.GetColumnOrder();

				for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
				{
					int j = colOrder[i];

					// テーブル名
					string tableName = lveQueryColumn.Columns[j].Text;

					// プロパティ
					string[] property = lveQueryColumn.Items[(int)ShenCore.qc.property].SubItems[j].Text.Split('\t');

					// フィールド名
					string fieldName = lveQueryColumn.Items[(int)ShenCore.qc.fieldName].SubItems[j].Text;

					if ( lveQueryColumn.Items[(int)ShenCore.qc.showField].SubItems[j].Text == "する" )
					{
						string groupFunc = lveQueryColumn.Items[(int)ShenCore.qc.groupFunc].SubItems[j].Text;
						if ( string.IsNullOrEmpty(groupFunc) )
						{
							select.Append(" " + tableName + "." + fieldName + ",\r\n");
						}
						else
						{
							select.Append(" " + groupFunc + "(" + tableName + "." + fieldName + "),\r\n");
						}

						//dataTypeName[j] = property[(int)ShenCore.prop.type];
						colComments.Append(property[(int)ShenCore.prop.comments] + "\t");
						if ( property[(int)ShenCore.prop.comments] != propNoComment )
						{
							colCommentsCount++;
						}
					}
					else
					{
						//dataTypeName[j] = null;
					}

					// 条件式
					string expression = lveQueryColumn.Items[(int)ShenCore.qc.expression].SubItems[j].Text;
					string value1 = lveQueryColumn.Items[(int)ShenCore.qc.value1].SubItems[j].Text;
					string value2 = lveQueryColumn.Items[(int)ShenCore.qc.value2].SubItems[j].Text;
					string rColOp = lveQueryColumn.Items[(int)ShenCore.qc.rColOp].SubItems[j].Text;

					if ( rColOp.Length == 0 )
					{
						rColOp = "AND";
					}

					if ( (expression.IndexOf('=') != -1 || expression == "<" || expression == ">" || expression.IndexOf("LIKE") != -1) &&
						 !string.IsNullOrEmpty(value1) )
					{
						expression = (expression == "NOT =") ? "<>" : expression;
						where.Append(" (" + tableName + "." + fieldName + " " + expression + "'" + value1 + "') " + rColOp + "\r\n");
					}
					else if ( (expression.IndexOf("BETWEEN") != -1) && (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2)) )
					{
						where.Append(" (" + tableName + "." + fieldName + " " + expression + "'" + value1 + "' AND '" + value2 + "') " + rColOp + "\r\n");
					}
					else if ( (expression.IndexOf("NULL") != -1) && string.IsNullOrEmpty(value1) )
					{
						where.Append(" (" + tableName + "." + fieldName + " " + expression + ") " + rColOp + "\r\n");
					}

					// ソート順
					string order = lveQueryColumn.Items[(int)ShenCore.qc.orderBy].SubItems[j].Text;
					if ( !string.IsNullOrEmpty(order) )
					{
						int k, number;
						for ( k = 0; k < order.Length && Char.IsDigit(order[k]); k++ ) ;
						number = (Char.IsDigit(order[0])) ? int.Parse(order.Substring(0, k)) : 999;
						string desc = (order.IndexOf("DESC", k, StringComparison.CurrentCultureIgnoreCase) != -1) ? " DESC" : "";
						orders.Add(number.ToString("D3") + "\t" + tableName + "." + fieldName + desc);
					}
				}

				// FROM テーブル名
				foreach ( string tableName in queryTableNames )
				{
					from.Append(" " + tableName + ",\r\n");
				}

				if ( orders.Count != 0 )
				{
					// ORDER BY
					orders.Sort();
					foreach ( string order in orders )
					{
						orderBy.Append(" " + order.Substring(3 + 1) + ",\r\n");	// (3 + 1):ソート順\t
					}
				}

				// WHERE
				if ( defWhereLen < where.Length )
				{
					where.Insert(defWhereLen + 1, '(');
					int lastSpace;
					for ( lastSpace = where.Length - 1; where[lastSpace] != ' '; lastSpace-- ) ;
					where.Remove(lastSpace + 1, where.Length - lastSpace - 1);		// "AND|OR\r\n" を削除する
					where.Insert(lastSpace, ')');
				}

				// テーブル結合
				foreach ( ListViewItem lvi in lvTableJoin.Items )
				{
					if ( (lvi.Index == 0) && (defWhereLen != where.Length) )
					{
						where.Append("AND\r\n");
					}
					where.Append(" (" + lvi.Text + (lvi.SubItems[(int)ShenCore.tabJoin.way].Text == ">=" ? " (+)" : ""));
					where.Append(" = ");
					where.Append(lvi.SubItems[(int)ShenCore.tabJoin.rightTabCol].Text + (lvi.SubItems[(int)ShenCore.tabJoin.way].Text == "<=" ? " (+)" : ""));
					where.Append(") ");
					if ( lvi.Index != lvTableJoin.Items.Count - 1 )
					{
						where.Append("AND\r\n");
					}
				}

				buldedSql = select.ToString(0, select.Length - (1 + 2)) + " " +	// (1 + 2):",\r\n"
							from.ToString(0, from.Length - (1 + 2)) + " " +		// (1 + 2):",\r\n"
							((defWhereLen == where.Length) ? "" : where.ToString()) +
							((defOrderByLen == orderBy.Length) ? "" : orderBy.ToString(0, orderBy.Length - (1 + 2))) +	// (1 + 2):",\r\n"
							"\r\n";

				if ( colCommentsCount != 0 )
				{
					columnComments = colComments.ToString();
				}

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}
#endif

#if WITHIN_SHENGLOBAL
		/// <summary>
		/// 入力された抽出条件があればクエリー項目にセットする
		/// </summary>
		/// <param name="selectParams"></param>
		/// <param name="bubbles"></param>
		/// <param name="plainTableFieldName"></param>
		/// <param name="paramNames"></param>
		/// <param name="expression"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		private static void SetShenlongParam(Dictionary<string, string> selectParams, string baseURI, string bubbles, string plainTableFieldName, ref Dictionary<string, int> paramNames, string expression, ref string value1, ref string value2)
		{
			if ( (selectParams == null) || (expression.Length == 0) )
				return;

			if ( bubbles.Length != 0 )
			{
				string[] setting = bubbles.Split(sepBubbSet);
				if ( setting[(int)bubbSet.control] == bubbCtrl.noVisible.ToString() )
					return;
			}

			int sameParamNo = 0;
			if ( !paramNames.TryGetValue(plainTableFieldName, out sameParamNo) )
			{
				paramNames[plainTableFieldName] = sameParamNo;
			}
			else
			{
				sameParamNo = ++paramNames[plainTableFieldName];
			}

			string _baseURI = Path.GetFileNameWithoutExtension(baseURI);
			string paramName = ParamInputDlg.pmShenlongTextID + _baseURI + ParamInputDlg.pmShenlongTextIdJoin + plainTableFieldName + ParamInputDlg.pmShenlongTextIdNo + sameParamNo;
			string _value;
			if ( !selectParams.TryGetValue(paramName, out _value) )
				return;

			value1 = _value;

			if ( expression == "BETWEEN" )
			{
				paramName += "HI";
				if ( selectParams.TryGetValue(paramName, out _value) )
				{
					value2 = _value;
				}
				else
				{
					int index = value1.IndexOf(" AND ", StringComparison.OrdinalIgnoreCase);
					if ( index != -1 )
					{
						value2 = value1.Substring(index + 5).TrimStart();
						value1 = value1.Substring(0, index).TrimEnd();
					}
				}
			}
		}

		/// <summary>
		/// カラムがCHAR型か否か
		/// </summary>
		/// <param name="colType"></param>
		/// <returns></returns>
		private static bool IsCharColumn(string colType)
		{
			return (colType.StartsWith("VARCHAR") || colType.StartsWith("CHAR"));
		}

		/// <summary>
		/// 別名を除いたテーブル名.カラム名を抽出する
		/// </summary>
		/// <param name="tableFieldName"></param>
		/// <param name="asFieldName"></param>
		/// <returns></returns>
		public static string GetPlainTableFieldName(/*ref */string tableFieldName, out int asFieldName)
		{
			asFieldName = -1;
			string rawTableFieldName = tableFieldName;

			try
			{
				if ( (asFieldName = tableFieldName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase)) != -1 )
				{
					rawTableFieldName = tableFieldName.Substring(0, asFieldName).TrimEnd();
					//tableFieldName = tableFieldName.Replace('(', '（').Replace(')', '）');
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}

			return rawTableFieldName;
		}

		public static string GetPlainTableFieldName(string tableFieldName)
		{
			int asFieldName;
			return GetPlainTableFieldName(tableFieldName, out asFieldName);
		}
#endif

#if TABLE_NAME_HAS_ALIAS
#if WITHIN_SHENGLOBAL
		/// <summary>
		/// テーブル名（またはその別名）を取得する
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="plainTblName"></param>
		/// <returns></returns>
		public static string GetTableName(string tableName, bool plainTblName)
		{
			string _tableName;
			string _alias;

			int index = tableName.IndexOf(' ');
			if ( index == -1 )
			{
				_tableName = tableName;
				_alias = null;
			}
			else
			{
				_tableName = tableName.Substring(0, index);
				_alias = tableName.Substring(index).Trim();
			}

			return (plainTblName || (_alias == null)) ? _tableName : _alias;
		}

		/// <summary>
		/// テーブル名.カラム名を分割する
		/// </summary>
		/// <param name="tableFieldName"></param>
		/// <param name="tableName"></param>
		/// <param name="fieldName"></param>
		private static bool SplitTableFieldName(string tableFieldName, out string tableName, out string fieldName, bool? plainTblName)
		{
			int dot = tableFieldName.IndexOf('.');
			if ( dot == -1 )
			{
				tableName = fieldName = string.Empty;
				return false;
			}

			if ( plainTblName == null )
			{
				tableName = tableFieldName.Substring(0, dot);
			}
			else
			{
				tableName = GetTableName(tableFieldName.Substring(0, dot), (bool)plainTblName);
			}

			fieldName = tableFieldName.Substring(dot + 1);

			return true;
		}
#endif
#endif

#if ENABLED_SUBQUERY
#if WITHIN_SHENGLOBAL
		/// <summary>
		/// サブクエリ ファイルを読み込む
		/// </summary>
		/// <param name="subQuery"></param>
		/// <returns></returns>
		public static XmlDocument ReadSubQueryFile(string subQuery, string shenColumnBaseURI)
		{
			string _xmlShenlongColumnFileName = subQuery;

			if ( subQuery.StartsWith(SUBQUERY_RELATIVE_PATH) )
			{
				// 相対パスを絶対パスに変換する
				_xmlShenlongColumnFileName = Path.GetDirectoryName(shenColumnBaseURI) + subQuery.Substring(SUBQUERY_RELATIVE_PATH.Length);
			}

			_xmlShenlongColumnFileName = Path.GetDirectoryName(_xmlShenlongColumnFileName) + "\\" + Path.GetFileName(_xmlShenlongColumnFileName).Replace('□', ' ');

			XmlDocument _xmlShenlongColumn = new XmlDocument();
			_xmlShenlongColumn.Load(_xmlShenlongColumnFileName);

			return _xmlShenlongColumn;
		}
#endif

		/// <summary>
		/// サブクエリの baseURI を取得する
		/// </summary>
		/// <param name="subQuery"></param>
		/// <param name="shenColumnBaseURI"></param>
		/// <returns></returns>
		private string GetSubQueryBaseURI(string subQuery, string shenColumnBaseURI)
		{
			if ( shenColumnBaseURI == null )
			{
				string tname = Path.GetFileNameWithoutExtension(subQuery);
				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrType + "='" + SUBQUERY_TYPE + "' and @" + attrName + "='" + tname + "']";
				XmlNode table = xmlTableList.SelectSingleNode(xpath);
				if ( table != null )
				{
					shenColumnBaseURI = table.Attributes[attrDir].Value + "\\" + tname + ".xml";
				}
			}

			return shenColumnBaseURI;
		}

		/// <summary>
		/// サブクエリをテーブル一覧に追加する
		/// </summary>
		/// <param name="subQuery"></param>
		private void AppendSubQueryToTableList(string subQuery, string shenColumnBaseURI)
		{
			try
			{
				string tname = Path.GetFileNameWithoutExtension(subQuery);
				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrType + "='" + SUBQUERY_TYPE + "' and @" + attrName + "='" + tname + "']";
				if ( xmlTableList.SelectSingleNode(xpath) != null )	// 既に存在する？
					return;

				XmlDocument _xmlShenlongColumn = ShenGlobal.ReadSubQueryFile(subQuery, shenColumnBaseURI);

				List<string> tables = new List<string>();
				int maxTableName = (int)listBoxTableList.Tag;

				XmlElement elem = xmlTableList.CreateElement(tagTable);

				XmlAttribute attr = xmlTableList.CreateAttribute(attrName);	// @name
				attr.Value = tname;
				elem.Attributes.Append(attr);

				attr = xmlTableList.CreateAttribute(attrDir);				// @dir
				if ( subQuery[0] == ShenGlobal.SUBQUERY_RELATIVE_PATH[0] )
					attr.Value = Path.GetDirectoryName(shenColumnBaseURI) + Path.GetDirectoryName(subQuery).Substring(ShenGlobal.SUBQUERY_RELATIVE_PATH.Length);
				else
					attr.Value = Path.GetDirectoryName(subQuery);
				elem.Attributes.Append(attr);

				attr = xmlTableList.CreateAttribute(attrType);				// @type
				attr.Value = SUBQUERY_TYPE;
				elem.Attributes.Append(attr);

				string owner = SUBQUERY_OWNER;
				attr = xmlTableList.CreateAttribute(attrOwner);				// @owner
				attr.Value = owner;
				elem.Attributes.Append(attr);

				string comments = _xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty][ShenGlobal.tagComment].InnerText;
				attr = xmlTableList.CreateAttribute(attrComments);			// @comments
				attr.Value = comments;
				elem.Attributes.Append(attr);

				xmlTableList.DocumentElement.AppendChild(elem);

				tables.Add(owner + "." + tname + "\t" + comments);

				if ( Program.debMode )
				{
					xmlTableList.Save(Application.StartupPath + "\\" + "~tableList.xml");
				}

				SetTableName(tables, maxTableName);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AppendSubQueryToTableList(string subQuery)
		{
			AppendSubQueryToTableList(subQuery, xmlShenlongColumnFileName ?? GetLatestBaseURI());
		}

		/// <summary>
		/// サブクエリをテーブル一覧から削除する
		/// </summary>
		private void RemoveSubQueryFromTableList()
		{
			try
			{
				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrType + "='" + SUBQUERY_TYPE + "']";
				XmlNodeList subQueries = xmlTableList.SelectNodes(xpath);
				if ( (subQueries == null) || (subQueries.Count == 0) )
					return;

				// テーブル一覧からサブクエリを削除する
				for ( int i = subQueries.Count - 1; 0 <= i; i-- )
				{
					//string subQuery = subQueries[i].Attributes[attrOwner].Value + "." + subQueries[i].Attributes[attrName].Value;
					//listBoxTableList.Items.Remove(subQuery);
					xmlTableList.DocumentElement.RemoveChild(subQueries[i]);
				}

				object _selectedItem = listBoxTableList.SelectedItem;

				for ( int i = listBoxTableList.Items.Count - 1; 0 <= i; i-- )
				{
					if ( listBoxTableList.Items[i].ToString().StartsWith(SUBQUERY_OWNER + ".") )
					{
						listBoxTableList.Items.RemoveAt(i);
					}
				}

				if ( _selectedItem != null )	// テーブルは選択されていなかった？
				{
					if ( listBoxTableList.Items.IndexOf(_selectedItem) != -1 )
					{
						listBoxTableList.SelectedItem = _selectedItem;	// テーブルの選択を復元する
					}
					else
					{
						listBoxTableList.SelectedItem = null;
						listBoxColumnList.Items.Clear();
					}
				}

				if ( Program.debMode )
				{
					xmlTableList.Save(Application.StartupPath + "\\" + "~tableList.xml");
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/*/// <summary>
		/// サブクエリ用のクエリー項目ファイルのドラッグが開始された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelTableList_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if ( e.Data.GetDataPresent(DataFormats.FileDrop) )
				{
					string[] sourceFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
					if ( Path.GetExtension(sourceFileNames[0]) == ".xml" )
					{
						e.Effect = DragDropEffects.Copy;
						return;
					}
				}

				e.Effect = DragDropEffects.None;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// サブクエリ用のクエリー項目ファイルがドラッグされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void labelTableList_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				object obj = e.Data.GetData(DataFormats.FileDrop);
				string shenFileName = ((string[])obj)[0];

				AppendSubQueryToTableList(shenFileName);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}*/

#if WITHIN_SHENGLOBAL
		/// <summary>
		/// クエリー項目(xml)から SQL を構築する
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		/// <param name="buildedSql"></param>
		/// <param name="columnComments"></param>
		/// <param name="indentCnt"></param>
		/// <returns></returns>
		private static bool BuildQueryColumnSQL(XmlDocument xmlShenlongColumn, Dictionary<string, string> selectParams, out string buildedSql, out string columnComments, ref List<string> fromTableNames, int indentCnt)
		{
			buildedSql = null;
			columnComments = null;

			try
			{
				//shencore.LogMessage(MethodBase.GetCurrentMethod().Name, shencore.lmo.strb);

				string indent = new string(' ', indentCnt);
				StringBuilder select = new StringBuilder("SELECT\r\n");
				StringBuilder from = new StringBuilder("\r\n" + indent + "FROM\r\n");
				StringBuilder where = new StringBuilder("\r\n" + indent + "WHERE\r\n");
				StringBuilder groupBy = new StringBuilder("\r\n" + indent + "GROUP BY\r\n");
				StringBuilder orderBy = new StringBuilder("\r\n" + indent + "ORDER BY\r\n");
				int defSelect = select.Length;
				int defWhereLen = where.Length;
				int defGroupByLen = groupBy.Length;
				int defOrderByLen = orderBy.Length;

				string[] _sqlDateFormat = { "yyyymmdd hh24mi", "yyyy/mm/dd hh24:mi" };
				List<string> _queryTableNames = new List<string>();		// 選択済みのテーブル名（現在の状態）
				List<string> _fileSubQuery = new List<string>();

				XmlNode fileProperty = xmlShenlongColumn.DocumentElement[tagProperty];
				if ( fileProperty != null )
				{
					if ( (fileProperty[tagSubQuery] != null) && (fileProperty[tagSubQuery].InnerText.Length != 0) )
					{
						foreach ( string subQuery in fileProperty[tagSubQuery].InnerText.Split(SUBQUERY_SEPARATOR) )
						{
							if ( _fileSubQuery.IndexOf(subQuery) == -1 )
							{
								_fileSubQuery.Add(subQuery);
							}
						}
					}
				}

				Dictionary<string, int> paramNames = new Dictionary<string, int>();
				List<string> orders = new List<string>();
				StringBuilder colComments = new StringBuilder();
				int colCommentsCount = 0;
				int groupFuncCount = 0;
				bool cameOR = false;
				indent += " ";

				foreach ( XmlNode column in xmlShenlongColumn.DocumentElement.SelectNodes(tagColumn) )
				{
					string _tableName = column.Attributes[attrTableName].Value;
					if ( _queryTableNames.IndexOf(_tableName) == -1 )
					{
						_queryTableNames.Add(_tableName);
					}

#if TABLE_NAME_HAS_ALIAS
					string tableName = GetTableName(column.Attributes[attrTableName].Value, false);			// テーブル名
#else
					string tableName = column.Attributes[attrTableName].Value;		// テーブル名
#endif
					string fieldName = column[ShenCore.qc.fieldName.ToString()].InnerText;	// フィールド名
					string[] property = new string[(int)ShenCore.prop.count];				// プロパティ
					property[(int)ShenCore.prop.type] = column[ShenCore.qc.fieldName.ToString()].Attributes[ShenCore.prop.type.ToString()].Value;
					property[(int)ShenCore.prop.length] = column[ShenCore.qc.fieldName.ToString()].Attributes[ShenCore.prop.length.ToString()].Value;
					property[(int)ShenCore.prop.nullable] = column[ShenCore.qc.fieldName.ToString()].Attributes[ShenCore.prop.nullable.ToString()].Value;
					property[(int)ShenCore.prop.comment] = column[ShenCore.qc.property.ToString()][ShenCore.prop.comment.ToString()].InnerText;
					string tableFieldName = (!fieldName.StartsWith(withoutTableName)) ? (tableName + "." + fieldName) : fieldName.Substring(withoutTableName.Length);

					int asFieldName;
					string plainTableFieldName = GetPlainTableFieldName(tableFieldName, out asFieldName);

#if true
					XmlNode alias = column[ShenCore.qc.property.ToString()][ShenCore.prop.alias.ToString()];
					property[(int)ShenCore.prop.alias] = (alias == null) ? string.Empty : "\"" + alias.InnerText + "\"";
					if ( (property[(int)ShenCore.prop.alias].Length != 0) && (asFieldName == -1) )	// プロパティでの別名があり、直接の別名指定は無い？
					{
						tableFieldName += " AS " + property[(int)ShenCore.prop.alias];
						plainTableFieldName = GetPlainTableFieldName(tableFieldName, out asFieldName);
					}
#endif

					if ( bool.Parse(column[ShenCore.qc.showField.ToString()].InnerText) )
					{
						string groupFunc = column[ShenCore.qc.groupFunc.ToString()].InnerText;
						if ( !string.IsNullOrEmpty(groupFunc) )
						{
							tableFieldName = groupFunc + "(" + tableFieldName + ")";
							groupFuncCount++;
						}

#if false
						if ( (property[(int)ShenCore.prop.type] == "DATE") && !tableFieldName.StartsWith("to_char(", StringComparison.OrdinalIgnoreCase) )
						{
							//select.Append(" " + "to_char(" + tableFieldName + ",'YYYY/MM/DD HH24:MI:SS') " + fieldName + ",\r\n");
							select.Append(" " + "to_char(" + plainTableFieldName + ",'YYYY/MM/DD HH24:MI:SS') ");
							select.Append((asFieldName != -1) ? tableFieldName.Substring(asFieldName/* + 4*/).Trim() : fieldName);
							select.Append(",\r\n");
						}
						else
						{
							select.Append(" " + tableFieldName + ",\r\n");
						}
#else
						select.Append(indent + tableFieldName + ",\r\n");
#endif

						colComments.Append(property[(int)ShenCore.prop.comment] + sepOutput);
						if ( property[(int)ShenCore.prop.comment] != propNoComment )
						{
							colCommentsCount++;
						}
					}

					tableFieldName = plainTableFieldName;

					// 条件式
					string expression = column[ShenCore.qc.expression.ToString()].InnerText;
					string value1 = column[ShenCore.qc.value1.ToString()].InnerText.Trim();
					string value2 = column[ShenCore.qc.value2.ToString()].InnerText.Trim();
					string rColOp = column[ShenCore.qc.rColOp.ToString()].InnerText;
					string leftRndBkt = "(", rightRndBkt = ")";

					string bubbles = string.Empty;
					XmlNode bubblesNode = column[ShenCore.qc.property.ToString()][ShenCore.prop.bubbles.ToString()];
					if ( bubblesNode != null )
					{
						bubbles = BubblesSettingToString(bubblesNode);
					}
					SetShenlongParam(selectParams, xmlShenlongColumn.BaseURI, bubbles, plainTableFieldName, ref paramNames, expression, ref value1, ref value2);

					//string quotation = (property[(int)ShenCore.prop.type].StartsWith("VARCHAR")) ? "'" : "";
					string quotation = IsCharColumn(property[(int)ShenCore.prop.type]) ? "'" : "";

					if ( !string.IsNullOrEmpty(value1) && (property[(int)ShenCore.prop.type] == "DATE") )	// 日付の条件指定あり？
					{
						int dtfmt = value1.IndexOf('/') == -1 ? 0 : 1;
						string toChar = (value1[0] == '(') ? "to_char" : "";
						string dateQuote = (Char.IsDigit(value1[0])) ? "'" : "";
						value1 = "to_date(" + toChar + dateQuote + value1 + dateQuote + ",'" + _sqlDateFormat[dtfmt] + "')";
					}

					if ( rColOp.Length == 0 )
					{
						rColOp = "AND";
					}

					if ( rColOp == "OR" )
					{
						leftRndBkt += (!cameOR) ? "(" : "";
						cameOR = true;
					}
					else/* if ( rColOp == "AND" )*/
					{
						//rightRndBkt += (cameOR) ? ")" : "";
						if ( cameOR )
						{
							if ( expression.Length != 0 )
							{
								rightRndBkt += ")";
							}
							else
							{
								where.Insert(where.Length - 5, ')');	// OR の括弧が閉じられていないので、強制的に右括弧で閉じる 5:" OR\r\n"
							}
						}
						cameOR = false;
					}

					// =, NOT =, >=, <=, >, <
					if ( (expression.IndexOf('=') != -1 || expression == "<" || expression == ">") && !string.IsNullOrEmpty(value1) )
					{
						expression = (expression == "NOT =") ? "<>" : expression;
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " " + quotation + value1 + quotation + rightRndBkt + " " + rColOp + "\r\n");
					}
					// BETWEEN, NOT BETWEEN
					else if ( (expression.IndexOf("BETWEEN") != -1) && (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2)) )
					{
						if ( !string.IsNullOrEmpty(value2) && (property[(int)ShenCore.prop.type] == "DATE") )	// 日付の条件指定あり？
						{
							int dtfmt = value2.IndexOf('/') == -1 ? 0 : 1;
							string toChar = (value2[0] == '(') ? "to_char" : "";
							string dateQuote = (Char.IsDigit(value2[0])) ? "'" : "";
							value2 = "to_date(" + toChar + dateQuote + value2 + dateQuote + ",'" + _sqlDateFormat[dtfmt] + "')";
						}
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " " + quotation + value1 + quotation + " AND " + quotation + value2 + quotation + rightRndBkt + " " + rColOp + "\r\n");
					}
					// IN, NOT IN
					else if ( (expression.IndexOf("IN") != -1) && !string.IsNullOrEmpty(value1) )
					{
						string[] values = value1.Split(',');
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " (");
						for ( int k = 0; k < values.Length; k++ )
						{
							where.Append(quotation + values[k] + quotation + ((k != values.Length - 1) ? "," : ""));
						}
						where.Append(")" + rightRndBkt + " " + rColOp + "\r\n");
					}
					// LIKE, NOT LIKE
					else if ( (expression.IndexOf("LIKE") != -1) && !string.IsNullOrEmpty(value1) )
					{
						string wildCard = (value1.IndexOfAny(new char[] { '%', '_' }) == -1) ? "%" : "";
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + " '" + value1 + wildCard + "'" + rightRndBkt + " " + rColOp + "\r\n");
					}
					// IS NULL, IS NOT NULL
					else if ( (expression.IndexOf("NULL") != -1) && string.IsNullOrEmpty(value1) )
					{
						where.Append(indent + leftRndBkt + tableFieldName + " " + expression + rightRndBkt + " " + rColOp + "\r\n");
					}

					// ソート順
					string order = column[ShenCore.qc.orderBy.ToString()].InnerText.Trim();
					if ( !string.IsNullOrEmpty(order) )
					{
						int k, number;
						for ( k = 0; k < order.Length && Char.IsDigit(order[k]); k++ ) ;
						number = (Char.IsDigit(order[0])) ? int.Parse(order.Substring(0, k)) : 999;
						string desc = (order.IndexOf("DESC", k, StringComparison.CurrentCultureIgnoreCase) != -1) ? " DESC" : "";
#if true
						string orderTableFieldName = (property[(int)ShenCore.prop.alias].Length == 0) ? tableFieldName : property[(int)ShenCore.prop.alias];
						orders.Add(number.ToString("D3") + "\t" + orderTableFieldName + desc);
#else
					orders.Add(number.ToString("D3") + "\t" + tableFieldName + desc);
#endif
					}
				}

				if ( select.Length == defSelect )
				{
					columnComments = "表示する項目が１つ以上必要です";
					return false;
				}

				// FROM テーブル名
				foreach ( string tableName in _queryTableNames )
				{
					if ( _fileSubQuery.Find(delegate(string s) { return s.IndexOf(tableName) != -1; }) != null )
						continue;
					from.Append(indent + tableName + ",\r\n");
					fromTableNames.Add(tableName);
				}
				// サブクエリ
				foreach ( string subQuery in _fileSubQuery )
				{
					XmlDocument _xmlShenlongColumn = ReadSubQueryFile(subQuery, xmlShenlongColumn.BaseURI/*GetSubQueryBaseURI(subQuery, xmlShenlongColumn.BaseURI)*/);
					string _buildedSql, _columnComments;
					if ( !BuildQueryColumnSQL(_xmlShenlongColumn, selectParams, out _buildedSql, out _columnComments, ref fromTableNames, indentCnt + 2) )
						return false;
					from.Append(indent + "(" + _buildedSql + indent + ") " + Path.GetFileNameWithoutExtension(subQuery) + ",\r\n");
				}

				if ( groupFuncCount != 0 )	// グループ関数の指定あり？
				{
					// GROUP BY
					List<string> groupFields = new List<string>();
					foreach ( XmlNode column in xmlShenlongColumn.DocumentElement.SelectNodes(tagColumn) )
					{
						if ( !bool.Parse(column[ShenCore.qc.showField.ToString()].InnerText) )
							continue;
						if ( !string.IsNullOrEmpty(column[ShenCore.qc.groupFunc.ToString()].InnerText) )
							continue;

#if TABLE_NAME_HAS_ALIAS
						string tableFieldName = GetPlainTableFieldName(GetTableName(column.Attributes[attrTableName].Value, false) + "." + column[ShenCore.qc.fieldName.ToString()].InnerText);
#else
						string tableFieldName = GetPlainTableFieldName(column.Attributes[attrTableName].Value + "." + column[ShenCore.qc.fieldName.ToString()].InnerText);
#endif
						if ( groupFields.IndexOf(tableFieldName) != -1 )
							continue;
						groupFields.Add(tableFieldName);
					}

					if ( groupFields.Count != 0 )
					{
						foreach ( string groupField in groupFields )
						{
							groupBy.Append(indent + groupField + ",\r\n");
						}
						groupBy.Length -= (1 + 2);	// (1 + 2):",\r\n"
					}
				}

				if ( orders.Count != 0 )	// ソートの指定あり？
				{
					// ORDER BY
					orders.Sort();
					foreach ( string order in orders )
					{
						orderBy.Append(indent + order.Substring(3 + 1) + ",\r\n");	// (3 + 1):ソート順\t
					}
					orderBy.Length -= (1 + 2);	// (1 + 2):",\r\n"
				}

				// WHERE
#if false
				if ( groupFuncCount == 0 )
				{
					/*// ROWNUM の最大指定あり？
					if ( HasMaxRowNum(xmlShenlongColumn) )
					{
						maxRowNum = int.Parse(xmlShenlongColumn.DocumentElement[tagProperty][tagMaxRowNum].InnerText);
					}*/
					//if ( 0 < maxRowNum )
					{
						where.Append(" (ROWNUM <= " + maxRowNum + ") AND\r\n");
					}
				}
#endif
				if ( defWhereLen < where.Length )
				{
					//where.Insert(defWhereLen + 1, '(');
					where.Insert(defWhereLen, indent + "(");
					where.Remove(defWhereLen + indent.Length + 1, indent.Length);
					int lastSpace;
					for ( lastSpace = where.Length - 1; where[lastSpace] != ' '; lastSpace-- ) ;
					where.Remove(lastSpace + 1, where.Length - lastSpace - 1);		// "AND|OR\r\n" を削除する
					if ( cameOR )
					{
						where.Insert(lastSpace++, ')');
						cameOR = false;
					}
					where.Insert(lastSpace, ')');

					/*if ( groupFuncCount != 0 )
					{
						// HAVING
						groupBy.Append("\r\nHAVING\r\n" + where.ToString().Substring(defWhereLen));
						where = new StringBuilder("\r\nWHERE\r\n");
					}*/
				}

				// テーブル結合
				XmlNodeList tableJoins = xmlShenlongColumn.DocumentElement.SelectNodes(tagTableJoin);
				for ( int i = 0; i < tableJoins.Count; i++ )
				{
					XmlNode tableJoin = tableJoins[i];
					if ( (i == 0) && (defWhereLen != where.Length) )
					{
						where.Append("AND\r\n");
					}

#if TABLE_NAME_HAS_ALIAS
					string leftTableName, leftColumnName, leftTableColumn;
					SplitTableFieldName(tableJoin.Attributes[ShenCore.tabJoin.leftTabCol.ToString()].Value, out leftTableName, out leftColumnName, false);
					//leftTableColumn = GetPlainTableFieldName(leftTableName + "." + leftColumnName);
					leftTableColumn = GetPlainTableFieldName(!leftColumnName.StartsWith(withoutTableName) ? (leftTableName + "." + leftColumnName) : leftColumnName.Substring(withoutTableName.Length));

					string rightTableName, rightColumnName, rightTableColumn;
					SplitTableFieldName(tableJoin.Attributes[ShenCore.tabJoin.rightTabCol.ToString()].Value, out rightTableName, out rightColumnName, false);
					//rightTableColumn = GetPlainTableFieldName(rightTableName + "." + rightColumnName);
					rightTableColumn = GetPlainTableFieldName(!rightColumnName.StartsWith(withoutTableName) ? (rightTableName + "." + rightColumnName) : rightColumnName.Substring(withoutTableName.Length));

					where.Append(" (");
#if COLLECT_OUTER_JOIN
					where.Append(leftTableColumn + ((tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == "<=") ? "(+)" : ""));	// 右外部結合(RIGHT [OUTER] JOIN)
					where.Append(" = ");
					where.Append(rightTableColumn + ((tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == ">=") ? "(+)" : ""));	// 左外部結合(LEFT [OUTER] JOIN)
#else
					where.Append(leftTableColumn + ((tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == ">=") ? " (+)" : ""));
					where.Append(" = ");
					where.Append(rightTableColumn + ((tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == "<=") ? " (+)" : ""));
#endif
					where.Append(") ");
#else
					where.Append(" (" + GetPlainTableFieldName(tableJoin.Attributes[ShenCore.tabJoin.leftTabCol.ToString()].Value) + (tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == ">=" ? " (+)" : ""));
					where.Append(" = ");
					where.Append(GetPlainTableFieldName(tableJoin.Attributes[ShenCore.tabJoin.rightTabCol.ToString()].Value) + (tableJoin.Attributes[ShenCore.tabJoin.way.ToString()].Value == "<=" ? " (+)" : ""));
					where.Append(") ");
#endif

					if ( i != tableJoins.Count - 1 )
					{
						where.Append("AND\r\n");
					}
				}

				buildedSql = select.ToString(0, select.Length - (1 + 2)) + " " +	// (1 + 2):",\r\n"
							 from.ToString(0, from.Length - (1 + 2)) + " " +		// (1 + 2):",\r\n"
							 ((where.Length == defWhereLen) ? "" : where.ToString()) +
							 ((groupBy.Length == defGroupByLen) ? "" : groupBy.ToString()) +
							 ((orderBy.Length == defOrderByLen) ? "" : orderBy.ToString()) +
							 "\r\n";

				if ( colCommentsCount != 0 )
				{
					columnComments = colComments.ToString();
				}

				return true;
			}
			catch ( Exception exp )
			{
				//shencore.LogMessage(exp.Message, shencore.lmo.all);
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}
#endif
#endif

#if EXCEL_LATE_BINDING
		/// <summary>
		/// クエリーの出力結果を Excel に貼り付ける
		/// </summary>
		/// <param name="queryOutput"></param>
		/// <param name="dataTypeName"></param>
		private void QueryOutputToExcel(string queryOutput, string[] dataTypeName, int fileHeaderOutputed)
		{
			IDataObject clipboardData = null;
			string clipboardText = null;
			// Excel object references.
			object m_objExcel = null;
			object m_objBooks = null;
			object m_objBook = null;
			object m_objSheets = null;
			object m_objSheet = null;
			object m_objCells = null;
			object m_objRange = null;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				// 現在のクリップボードの内容を退避しておく
				if ( restoreClipboardAfterExcelPaste )
				{
					if ( (clipboardData = Clipboard.GetDataObject()) != null )
					{
#if (DEBUG)
						foreach ( string fmt in clipboardData.GetFormats() )
						{
							Console.WriteLine(fmt);
						}
#endif
						if ( clipboardData.GetDataPresent(DataFormats.Text) )
						{
							clipboardText = (string)clipboardData.GetData(DataFormats.Text);
						}
					}
				}

				// Copy a string to the Windows clipboard.
				Clipboard.SetDataObject(queryOutput);

				// Frequenty-used variable for optional arguments.
				object m_objOpt = System.Reflection.Missing.Value;

				try
				{
					IntPtr hWndExcel = api.FindWindow("XLMAIN", null);
					Debug.WriteLine("hWndExcel:" + hWndExcel);
					if ( hWndExcel != IntPtr.Zero )
					{
						string pID = "Excel.Application";
						/*m_objExcel = (Excel.Application)Marshal.GetActiveObject(pID);*/
						m_objExcel = Marshal.GetActiveObject(pID);
						// 編集中のセルがあればキャンセルしておく
						api.PostMessage(hWndExcel, api.WM_KEYDOWN, api.VK_ESCAPE, 0);
						api.PostMessage(hWndExcel, api.WM_KEYUP, api.VK_ESCAPE, 0);
					}
				}
				catch ( COMException exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}
				if ( m_objExcel == null )
				{
					// Start a new workbook in Excel.
					/*m_objExcel = new Excel.Application();*/
					m_objExcel = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                }

                m_objExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, m_objExcel, new object[] { true });
                //throw new Exception("test");

                // Book
                /*m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;*/
                m_objBooks = m_objExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, m_objExcel, null);

				if ( (pasteQueryResultToExcel == pasteExcel.actBookActSheet) || (pasteQueryResultToExcel == pasteExcel.actBookNewSheet) )
				{
					/*m_objBook = m_objExcel.ActiveWorkbook;*/
					m_objBook = m_objExcel.GetType().InvokeMember("ActiveWorkbook", BindingFlags.GetProperty, null, m_objExcel, null);
				}
				else if ( pasteQueryResultToExcel == pasteExcel.shenBookNewSheet )
				{
					/*foreach ( Excel._Workbook objBook in m_objBooks )*/
					// forShenlongBookName が既に開かれているか確認する
					int booksCount = (int)m_objBooks.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, m_objBooks, null);
					for ( int i = 1; i <= booksCount; i++ )
					{
						/*if ( objBook.Name == forShenlongBookName )*/
						object objBook = m_objBooks.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, m_objBooks, new object[] { i });
						string bookName = objBook.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, objBook, null).ToString();
						if ( bookName == forShenlongBookName )
						{
							m_objBook = objBook;
							break;
						}
						Marshal.ReleaseComObject(objBook);
					}
					if ( m_objBook == null )							// まだ開かれていなかった？
					{
						// ワークブックを新規に作成する
						/*m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));*/
						m_objBook = m_objBooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, m_objBooks, null);
						try
						{
							string shenExcelFileName = Application.StartupPath + "\\" + forShenlongBookName;
							if ( File.Exists(shenExcelFileName) )
							{
								File.Delete(shenExcelFileName);
							}
							// 新規のブック名を forShenlongBookName に変更する為に一旦保存する
							/*m_objBook.SaveAs(shenExcelFileName, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);*/
							m_objBook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, m_objBook, new object[] { shenExcelFileName, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, XlSaveAsAccessMode.xlNoChange });
						}
						catch ( Exception exp )
						{
							MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						}
					}
					else
					{
						/*m_objBook.Activate();*/
						m_objBook.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, m_objBook, null);
					}
				}

				if ( m_objBook == null )
				{
					/*m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));*/
					m_objBook = m_objBooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, m_objBooks, null);
				}

				/*m_objExcel.Visible = true;*/
				m_objExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, m_objExcel, new object[] { true });

                //m_objBooks.OpenText(@"C:\Documents and Settings\Hidetatsu\My Documents\Visual Studio 2005\Projects\Visual C#\Shenlong\bin\Debug\~QueryOutput.txt", Excel.XlPlatform.xlWindows, 1, Excel.XlTextParsingType.xlDelimited, Excel.XlTextQualifier.xlTextQualifierDoubleQuote, false, false, false, true, false, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // Sheet
                /*m_objSheets = (Excel.Sheets)m_objBook.Worksheets;*/
                m_objSheets = m_objBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, m_objBook, null);

				if ( (pasteQueryResultToExcel == pasteExcel.newBookActSheet) || (pasteQueryResultToExcel == pasteExcel.actBookActSheet) )
				{
					//m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
					/*m_objSheet = (Excel._Worksheet)(m_objBook.ActiveSheet);*/
					m_objSheet = m_objBook.GetType().InvokeMember("ActiveSheet", BindingFlags.GetProperty, null, m_objBook, null);
				}

				if ( m_objSheet == null )	// アクティブシート以外に貼り付ける？
				{
					/*Excel._Worksheet objSheet = (Excel._Worksheet)m_objSheets[m_objSheets.Count];*/
					int sheetsCount = (int)m_objSheets.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, m_objSheets, null);
					object objSheet = m_objSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, m_objSheets, new object[] { sheetsCount });
					/*m_objSheet = (Excel._Worksheet)m_objSheets.Add(m_objOpt, objSheet, m_objOpt, m_objOpt);*/
					// ワークシートを新規に追加する
					m_objSheet = m_objSheets.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, m_objSheets, new object[] { m_objOpt, objSheet });
					Marshal.ReleaseComObject(objSheet);
#if true
					try
					{
						string sheetName = (xmlShenlongColumnFileName != null) ? Path.GetFileNameWithoutExtension(xmlShenlongColumnFileName) : lveQueryColumn.Columns[0].Text;
						int seqNo = 1;
						/*foreach ( Excel._Worksheet _objSheet in m_objSheets )*/
						for ( int i = 1; i <= sheetsCount; i++ )
						{
							/*string _sheetName = _objSheet.Name;*/
							object _objSheet = m_objSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, m_objSheets, new object[] { i });
							string _sheetName = _objSheet.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, _objSheet, null).ToString();
							Marshal.ReleaseComObject(_objSheet);
							if ( _sheetName.StartsWith(sheetName + "#") )
							{
								seqNo = Math.Max(seqNo, int.Parse(_sheetName.Substring(sheetName.Length + 1)) + 1);
							}
						}
						/*m_objSheet.Name = sheetName + "#" + seqNo;*/
						m_objSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, m_objSheet, new object[] { sheetName + "#" + seqNo });
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
#endif
				}

				if ( dataTypeName != null )
				{
					for ( int i = 0, j = 0; i < dataTypeName.Length; i++ )
					{
						if ( dataTypeName[i] == null )
							continue;
						if ( ShenGlobal.IsCharColumn(dataTypeName[i]) )
						{
							int c1 = j / 26;
							int c2 = j % 26;
							char cc1 = (c1 == 0) ? ' ' : (char)('A' + (c1 - 1));
							char cc2 = (char)('A' + c2);
							string column = cc1.ToString().TrimStart() + cc2.ToString();
							//m_objRange = m_objSheet.Columns.get_Range(column + ":" + column, m_objOpt);
							//m_objRange.NumberFormatLocal = "@";
							/*Excel.Range objColumns = m_objSheet.Columns;*/
							object objColumns = m_objSheet.GetType().InvokeMember("Columns", BindingFlags.GetProperty, null, m_objSheet, null);
							/*Excel.Range objRange = objColumns.get_Range(column + ":" + column, m_objOpt);*/
							object objRange = objColumns.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, objColumns, new object[] { column + ":" + column });
							/*objRange.NumberFormatLocal = "@";*/
							objRange.GetType().InvokeMember("NumberFormatLocal", BindingFlags.SetProperty, null, objRange, new object[] { "@" });
							Marshal.ReleaseComObject(objRange);
							Marshal.ReleaseComObject(objColumns);
						}
						j++;
					}
				}

				// Paste the data starting at cell A1.
				/*m_objRange = m_objSheet.get_Range("A1", m_objOpt);*/
				m_objCells = m_objSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, m_objSheet, null);
				m_objRange = m_objCells.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, m_objCells, new object[] { 1, 1 });
				/*m_objSheet.Paste(m_objRange, false);*/
				m_objSheet.GetType().InvokeMember("Paste", BindingFlags.InvokeMethod, null, m_objSheet, new object[] { m_objRange, false });

				/*// Save the workbook and quit Excel.
				m_objBook.SaveAs(@".\" + "Book5.xls", m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);
				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();*/

				try
				{
					if ( fileHeaderOutputed != 0 )
					{
						int headerLineCount = 0;
						if ( (fileHeaderOutputed & (int)ShenGlobal.header.columnName) != 0 ) headerLineCount++;
						if ( (fileHeaderOutputed & (int)ShenGlobal.header.comment) != 0 ) headerLineCount++;

						object objCells = null;
						object objRange = null;

#if true
						// 先頭行を固定
						objCells = m_objSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, m_objSheet, null);
						objRange = m_objSheet.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, objCells, new object[] { headerLineCount + 1, 1 });
						objRange.GetType().InvokeMember("Select", BindingFlags.InvokeMethod, null, objRange, null);

						object _objActiveWindow = m_objExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, m_objExcel, null);
						_objActiveWindow.GetType().InvokeMember("FreezePanes", BindingFlags.SetProperty, null, _objActiveWindow, new object[] { true });
						Marshal.ReleaseComObject(_objActiveWindow);
						Marshal.ReleaseComObject(objRange);

						objRange = m_objSheet.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, objCells, new object[] { 1, 1 });
						objRange.GetType().InvokeMember("Select", BindingFlags.InvokeMethod, null, objRange, null);
						Marshal.ReleaseComObject(objRange);
						Marshal.ReleaseComObject(objCells);
#else
						// 先頭行を固定
						object _objActiveWindow = m_objExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty, null, m_objExcel, null);
						_objActiveWindow.GetType().InvokeMember("SplitRow", BindingFlags.SetProperty, null, _objActiveWindow, new object[] { headerLineCount });
						_objActiveWindow.GetType().InvokeMember("FreezePanes", BindingFlags.SetProperty, null, _objActiveWindow, new object[] { true });
						Marshal.ReleaseComObject(_objActiveWindow);
#endif

#if true
						// オートフィルタ
						int c1 = (dataTypeName.Count() - 1) / 26;
						int c2 = (dataTypeName.Count() - 1) % 26;
						char cc1 = (c1 == 0) ? ' ' : (char)('A' + (c1 - 1));
						char cc2 = (char)('A' + c2);
						string endColumn = cc1.ToString().TrimStart() + cc2.ToString();
						objCells = m_objSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, m_objSheet, null);
						objRange = m_objSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, objCells, new object[] { "A" + headerLineCount, endColumn + headerLineCount });
						objRange.GetType().InvokeMember("AutoFilter", BindingFlags.InvokeMethod, null, objRange, null);
						Marshal.ReleaseComObject(objRange);
						Marshal.ReleaseComObject(objCells);
#else
						// オートフィルタ
						/*object */objCells = m_objSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty, null, m_objSheet, null);
						/*object */objRange = m_objSheet.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, objCells, new object[] { 1, 1 });
						objRange.GetType().InvokeMember("AutoFilter", BindingFlags.InvokeMethod, null, objRange, new object[] { 1 });
						Marshal.ReleaseComObject(objRange);
						Marshal.ReleaseComObject(objCells);
#endif

						if ( fileHeaderOutputed == (int)ShenGlobal.header.columnName )
						{
							// オートフィット
							object objEntireColumn = m_objCells.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, m_objCells, null);
							objEntireColumn.GetType().InvokeMember("AutoFit", BindingFlags.InvokeMethod, null, objEntireColumn, null);
							Marshal.ReleaseComObject(objEntireColumn);
						}
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
				}

				try
				{
					IntPtr hWndExcel = api.FindWindow("XLMAIN", null);
					if ( hWndExcel != IntPtr.Zero )
					{
						if ( api.IsIconic(hWndExcel) )
							api.ShowWindow(hWndExcel, api.SW_SHOWNOACTIVATE);
						api.SetForegroundWindow(hWndExcel);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show("Excel への貼付が失敗しました．\r\n" + exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				if ( m_objRange != null )
				{
					Marshal.ReleaseComObject(m_objRange);
					m_objRange = null;
				}
				if ( m_objSheet != null )
				{
					Marshal.ReleaseComObject(m_objSheet);
					m_objSheet = null;
				}
				if ( m_objSheets != null )
				{
					Marshal.ReleaseComObject(m_objSheets);
					m_objSheets = null;
				}
				if ( m_objBook != null )
				{
					Marshal.ReleaseComObject(m_objBook);
					m_objBook = null;
				}
				if ( m_objBooks != null )
				{
					Marshal.ReleaseComObject(m_objBooks);
					m_objBooks = null;
				}
				if ( m_objExcel != null )
				{
					Marshal.ReleaseComObject(m_objExcel);
					m_objExcel = null;
				}

				if ( clipboardData == null )
				{
					Clipboard.Clear();
				}
				else
				{
					try
					{
						if ( clipboardText != null )
						{
							Clipboard.SetData(DataFormats.Text, clipboardText);
						}
						else
						{
							if ( clipboardData != null )
							{
								Clipboard.SetDataObject(clipboardData);
							}
						}
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
				}

				Cursor.Current = Cursors.Default;
			}
		}

		public enum XlSaveAsAccessMode
		{
			xlNoChange = 1,
			xlShared = 2,
			xlExclusive = 3,
		}
#else
		/// <summary>
		/// クエリーの出力結果を Excel に貼り付ける
		/// </summary>
		/// <param name="queryOutput"></param>
		/// <param name="dataTypeName"></param>
		private void QueryOutputToExcel(string queryOutput, string[] dataTypeName)
		{
			// Excel object references.
			Excel.Application m_objExcel = null;
			Excel.Workbooks m_objBooks = null;
			Excel._Workbook m_objBook = null;
			Excel.Sheets m_objSheets = null;
			Excel._Worksheet m_objSheet = null;
			Excel.Range m_objRange = null;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				// Copy a string to the Windows clipboard.
				Clipboard.SetDataObject(queryOutput);

				// Frequenty-used variable for optional arguments.
				object m_objOpt = System.Reflection.Missing.Value;

				try
				{
					IntPtr hWndExcel = api.FindWindow("XLMAIN", null);
					Debug.WriteLine("hWndExcel:" + hWndExcel);
					if ( hWndExcel != IntPtr.Zero )
					{
						string pID = "Excel.Application";
						m_objExcel = (Excel.Application)Marshal.GetActiveObject(pID);
						// 編集中のセルがあればキャンセルしておく
						api.PostMessage(hWndExcel, api.WM_KEYDOWN, api.VK_ESCAPE, 0);
						api.PostMessage(hWndExcel, api.WM_KEYUP, api.VK_ESCAPE, 0);
					}
				}
				catch ( COMException exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}
				if ( m_objExcel == null )
				{
					// Start a new workbook in Excel.
					m_objExcel = new Excel.Application();
				}

				// Book
				m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;

				if ( (pasteQueryResultToExcel == pasteExcel.actBookActSheet) || (pasteQueryResultToExcel == pasteExcel.actBookNewSheet) )
				{
					m_objBook = m_objExcel.ActiveWorkbook;
				}
				else if ( pasteQueryResultToExcel == pasteExcel.shenBookNewSheet )
				{
					foreach ( Excel._Workbook objBook in m_objBooks )	// forShenlongBookName が既に開かれているか確認する
					{
						if ( objBook.Name == forShenlongBookName )
						{
							m_objBook = objBook;
							break;
						}
						Marshal.ReleaseComObject(objBook);
					}
					if ( m_objBook == null )							// まだ開かれていなかった？
					{
						m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));
						try
						{
							string shenExcelFileName = Application.StartupPath + "\\" + forShenlongBookName;
							if ( File.Exists(shenExcelFileName) )
							{
								File.Delete(shenExcelFileName);
							}
							// 新規のブック名を forShenlongBookName に変更する為に一旦保存する
							m_objBook.SaveAs(shenExcelFileName, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);
						}
						catch ( Exception exp )
						{
							MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						}
					}
					else
					{
						m_objBook.Activate();
					}
				}

				if ( m_objBook == null )
				{
					m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));
				}

				m_objExcel.Visible = true;

				//m_objBooks.OpenText(@"C:\Documents and Settings\Hidetatsu\My Documents\Visual Studio 2005\Projects\Visual C#\Shenlong\bin\Debug\~QueryOutput.txt", Excel.XlPlatform.xlWindows, 1, Excel.XlTextParsingType.xlDelimited, Excel.XlTextQualifier.xlTextQualifierDoubleQuote, false, false, false, true, false, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

				// Sheet
				m_objSheets = (Excel.Sheets)m_objBook.Worksheets;

				if ( (pasteQueryResultToExcel == pasteExcel.newBookActSheet) || (pasteQueryResultToExcel == pasteExcel.actBookActSheet) )
				{
					//m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
					m_objSheet = (Excel._Worksheet)(m_objBook.ActiveSheet);
				}

				if ( m_objSheet == null )	// アクティブシート以外に貼り付ける？
				{
					Excel._Worksheet objSheet = (Excel._Worksheet)m_objSheets[m_objSheets.Count];
					m_objSheet = (Excel._Worksheet)m_objSheets.Add(m_objOpt, objSheet, m_objOpt, m_objOpt);	// ワークシートを新規に追加する
					Marshal.ReleaseComObject(objSheet);
#if true
					try
					{
						string sheetName = (xmlShenlongColumnFileName != null) ? Path.GetFileNameWithoutExtension(xmlShenlongColumnFileName) : lveQueryColumn.Columns[0].Text;
						int seqNo = 1;
						foreach ( Excel._Worksheet _objSheet in m_objSheets )
						{
							string _sheetName = _objSheet.Name;
							Marshal.ReleaseComObject(_objSheet);
							if ( _sheetName.StartsWith(sheetName + "#") )
							{
								seqNo = Math.Max(seqNo, int.Parse(_sheetName.Substring(sheetName.Length + 1)) + 1);
							}
						}
						m_objSheet.Name = sheetName + "#" + seqNo;
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
#endif
				}

				if ( dataTypeName != null )
				{
					for ( int i = 0, j = 0; i < dataTypeName.Length; i++ )
					{
						if ( dataTypeName[i] == null )
							continue;
						if ( ShenGlobal.IsCharColumn(dataTypeName[i]) )
						{
							int c1 = j / 26;
							int c2 = j % 26;
							char cc1 = (c1 == 0) ? ' ' : (char)('A' + (c1 - 1));
							char cc2 = (char)('A' + c2);
							string column = cc1.ToString().TrimStart() + cc2.ToString();
							//m_objRange = m_objSheet.Columns.get_Range(column + ":" + column, m_objOpt);
							//m_objRange.NumberFormatLocal = "@";
							Excel.Range objColumns = m_objSheet.Columns;
							Excel.Range objRange = objColumns.get_Range(column + ":" + column, m_objOpt);
							objRange.NumberFormatLocal = "@";
							Marshal.ReleaseComObject(objRange);
							Marshal.ReleaseComObject(objColumns);
						}
						j++;
					}
				}

				// Paste the data starting at cell A1.
				m_objRange = m_objSheet.get_Range("A1", m_objOpt);
				m_objSheet.Paste(m_objRange, false);

				/*// Save the workbook and quit Excel.
				m_objBook.SaveAs(@".\" + "Book5.xls", m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt);
				m_objBook.Close(false, m_objOpt, m_objOpt);
				m_objExcel.Quit();*/

				try
				{
					IntPtr hWndExcel = api.FindWindow("XLMAIN", null);
					if ( hWndExcel != IntPtr.Zero )
					{
						if ( api.IsIconic(hWndExcel) )
							api.ShowWindow(hWndExcel, api.SW_SHOWNOACTIVATE);
						api.SetForegroundWindow(hWndExcel);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show("Excel への貼付が失敗しました．\r\n" + exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				if ( m_objRange != null )
				{
					Marshal.ReleaseComObject(m_objRange);
					m_objRange = null;
				}
				if ( m_objSheet != null )
				{
					Marshal.ReleaseComObject(m_objSheet);
					m_objSheet = null;
				}
				if ( m_objSheets != null )
				{
					Marshal.ReleaseComObject(m_objSheets);
					m_objSheets = null;
				}
				if ( m_objBook != null )
				{
					Marshal.ReleaseComObject(m_objBook);
					m_objBook = null;
				}
				if ( m_objBooks != null )
				{
					Marshal.ReleaseComObject(m_objBooks);
					m_objBooks = null;
				}
				if ( m_objExcel != null )
				{
					Marshal.ReleaseComObject(m_objExcel);
					m_objExcel = null;
				}

				Clipboard.Clear();
				Cursor.Current = Cursors.Default;
			}
		}
#endif

#if false
		/// <summary>
		/// アクセス ログをテーブルに保存する
		/// </summary>
		private void WriteAccessLog()
		{
			OracleConnection oraInfoPub = null;
			OracleCommand oraCmd = null;

			try
			{
				if ( !writeAccessLog )
					return;

				Cursor.Current = Cursors.WaitCursor;

				ArrayList tableNames = new ArrayList();		// TABLE_NAME

				try
				{
					if ( tabControl.SelectedTab != tabSQL )
					{
						tableNames = queryTableNames;
					}
					else
					{
						string[] tables = GetTableNameFromSQL(textSQL.Text.Trim());	// SQL からテーブル名を抜き出す
						foreach ( string table in tables )
						{
							tableNames.Add(table.Trim());
						}
					}
				}
				catch ( Exception exp )
				{
					tableNames.Add(exp.Message);
				}

				string infoPubSID = "dbsv01", infoPubUser = "shenlong", infoPubPwd = "amkj1shen";

				try
				{
					string xmlLogOnFileName = Application.StartupPath + LogOnDlg.LOGON_FILE_NAME;
					XmlDocument xmlLogOn = new XmlDocument();
					xmlLogOn.Load(xmlLogOnFileName);
					string xpath = "/" + LogOnDlg.tagRoot + "/" + LogOnDlg.tagLogOn + "[@" + LogOnDlg.attrSID + "='" + infoPubSID + "']" + "[" + LogOnDlg.tagUserName + "='" + infoPubUser + "']";
					XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
					if ( logOnNode != null )
					{
						// LogOn.xml に登録されているパスワードを優先する
						infoPubSID = logOnNode.Attributes[LogOnDlg.attrSID].Value;
						infoPubUser = logOnNode[LogOnDlg.tagUserName].InnerText;
						infoPubPwd = common.DecodePassword(logOnNode[LogOnDlg.tagPassword].InnerText);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}

				string conStr = "Data Source=" + infoPubSID + ";User Id=" + infoPubUser + ";Password=" + infoPubPwd;
				oraInfoPub = new OracleConnection(conStr);
				oraInfoPub.Open();							// 情報公開サーバに接続する

				string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");	// ACCESS_DATE
				string serviceName, userName, pcName;
				string[] oraConnName = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);

				try
				{
					serviceName = oraConnName[0].Trim().ToLower();	// SERVICE_NAME
				}
				catch ( Exception exp )
				{
					serviceName = exp.Message;
				}

				try
				{
					userName = oraConnName[1].Trim().ToLower();		// USER_NAME
				}
				catch ( Exception exp )
				{
					userName = exp.Message;
				}

				try
				{
					pcName = System.Net.Dns.GetHostName().ToLower();// PC_NAME
				}
				catch ( Exception exp )
				{
					pcName = exp.Message;
				}

				foreach ( string tableName in tableNames )
				{
					string sql = "INSERT INTO T_LOG_SHENLONG (ACCESS_DATE,SERVICE_NAME,USER_NAME,TABLE_NAME,PC_NAME) " +
								 "VALUES(" + "TO_DATE('" + now + "','yyyy/mm/dd hh24:mi:ss')" + ",'" + serviceName + "','" + userName + "','" + tableName + "','" + pcName + "')";
					oraCmd = new OracleCommand(sql, oraInfoPub);
					oraCmd.ExecuteNonQuery();
					oraCmd.Dispose();
					oraCmd = null;
				}
#if (DEBUG)
#if false
				{
					string sql = "DELETE T_LOG_SHENLONG " +
								 "WHERE USER_NAME='" + userName + "' AND PC_NAME='" + pcName + "'";
					oraCmd = new OracleCommand(sql, oraInfoPub);
					int rows = oraCmd.ExecuteNonQuery();
					oraCmd.Dispose();
					oraCmd = null;
				}
#endif
#endif
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
			finally
			{
				if ( oraCmd != null )
				{
					oraCmd.Dispose();
					oraCmd = null;
				}

				if ( oraInfoPub != null )
				{
					oraInfoPub.Close();
					oraInfoPub.Dispose();
					oraInfoPub = null;
				}

				Cursor.Current = Cursors.Default;
			}
		}
#endif

#if false
		/// <summary>
		/// SQL から FROM 以降のテーブル名を抜き出す
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		string[] GetTableNameFromSQL(string sql)
		{
			int from = sql.IndexOf("FROM", StringComparison.CurrentCultureIgnoreCase);
			if ( from == -1 )
				return null;

			int startTableName, lenTableName = 0;
			for ( startTableName = from + 4; !Char.IsLetter(sql[startTableName]); startTableName++ ) ;
			bool comma = false;
			for ( int i = startTableName; i < sql.Length; i++ )
			{
				if ( sql[i] == ',' )
				{
					comma = true;
				}
				else if ( sql[i] == '\r' )
				{
					if ( !comma )
						break;
				}
				else if ( sql[i] == ' ' )
				{
					if ( !comma )
						break;
				}
				else
				{
					if ( comma && Char.IsLetter(sql[i]) )
					{
						comma = false;
					}
				}

				lenTableName++;
			}

			return sql.Substring(startTableName, lenTableName).Split(',');
		}
#endif

		#region コントロールのイベント
		/// <summary>
		/// splitContainer1 のスプリッタが移動した
		/// </summary>
		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			splitContainerTable.SplitterDistance = splitContainer1.SplitterDistance;

			textTableFilter.Width = (int)(splitContainer1.Panel1.Width * .4F);

			textColumnFilter.Width = (int)(splitContainer1.Panel2.Width * .4F);
		}

		/// <summary>
		/// splitContainerTable のスプリッタが移動した
		/// </summary>
		private void splitContainerTable_SplitterMoved(object sender, SplitterEventArgs e)
		{
			splitContainer1.SplitterDistance = splitContainerTable.SplitterDistance;
		}

		/// <summary>
		/// テーブル名が選択された
		/// </summary>
		private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
		{
#if TABLE_NAME_HAS_ALIAS
			if ( listBoxTableList.SelectedIndex == editingTableNameIndex )
				return;

			if ( listBoxTableList.Text.Length == 0 )
				return;
#endif

			if ( !SelectColumns() )
				return;

			if ( tableSelectedAction == (int)tableSelAct.clearSelectedColumns )
			{
				Debug.WriteLine((formKeyDownArgs == null) ? "formKeyDownArgs is null" : ("Shift:" + formKeyDownArgs.Shift));
				if ( (formKeyDownArgs == null) || !formKeyDownArgs.Shift )
				{
					ClearQueryColumn();
				}
			}
			else if ( tableSelectedAction == (int)tableSelAct.appendAllColumns )
			{
				ClearQueryColumn();
				ColumnItemSelected("*");
			}

			try
			{
				selColumnHistory = new List<int>();
				curSelColumnHistory = -1;

				if ( (formKeyDownArgs == null) || !formKeyDownArgs.Alt )	// 戻る/進む以外で選択された？
				{
					int index = selTableHistory.IndexOf(listBoxTableList.SelectedIndex);
					if ( index != -1 )
					{
						selTableHistory.Remove(listBoxTableList.SelectedIndex);
					}
					selTableHistory.Add(listBoxTableList.SelectedIndex);
					curSelTableHistory = selTableHistory.Count - 1;		// 現在のテーブルを最終履歴とする
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// listBoxTableList でキーが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxTableList_KeyDown(object sender, KeyEventArgs e)
		{
#if TABLE_NAME_HAS_ALIAS
			if ( e.KeyData == Keys.F2 )
			{
				if ( (listBoxTableList.SelectedIndex != -1) && editableColumnName )
				{
					StartTableNameEditing();
				}
				return;
			}
#endif

			try
			{
				if ( e.Alt )	// Alt キーが押されている？
				{
					if ( (selTableHistory.Count != 0)/* && (listBoxTableList.Focused)*/ )	// 選択されたテーブルの履歴があり、テーブル一覧にフォーカスがある？
					{
						if ( e.KeyCode == Keys.Left )		// 戻る(Alt + ←)？
						{
							if ( curSelTableHistory != 0 )
							{
								curSelTableHistory--;
								listBoxTableList.SelectedIndex = selTableHistory[curSelTableHistory];	// 履歴のテーブルで一覧を選択状態にする
							}
						}
						else if ( e.KeyCode == Keys.Right )	// 進む(Alt + →)？
						{
							if ( curSelTableHistory != (selTableHistory.Count - 1) )
							{
								curSelTableHistory++;
								listBoxTableList.SelectedIndex = selTableHistory[curSelTableHistory];	// 履歴のテーブルで一覧を選択状態にする
							}
						}
					}
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
#if (DEBUG)
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
#if ENABLED_SUBQUERY
				try { selTableHistory.RemoveAt(curSelTableHistory); }
				catch { }
#endif
			}
		}

#if TABLE_NAME_HAS_ALIAS
		/// <summary>
		/// テーブル名の編集が始まった
		/// </summary>
		private void StartTableNameEditing()
		{
			try
			{
				editingTableNameIndex = listBoxTableList.SelectedIndex;
				Rectangle r = listBoxTableList.GetItemRectangle(editingTableNameIndex);
				//string itemText = (string)listBoxTableList.Items[editingTableNameIndex];
				//int tableComment = itemText.IndexOf('\t');

				int delta = 0;
				textTableName.Location = new System.Drawing.Point(r.X + delta, r.Y + delta);
				textTableName.Size = new System.Drawing.Size(r.Width/* - 10*/, r.Height - delta);
				textTableName.Show();
				listBoxTableList.Controls.AddRange(new System.Windows.Forms.Control[] { this.textTableName });
				textTableName.Text = GetListBoxTableName(selTbl.withOwner | selTbl.plainTblName);
				textTableName.Tag = textTableName.Text.Length.ToString("D2")/* + ((tableComment == -1) ? "" : itemText.Substring(tableComment))*/;	// D2(別名を除いたテーブル名の長さ) + コメント
				textTableName.Select();
				textTableName.Select(textTableName.Text.Length, 0);
				textTableName.KeyPress += new KeyPressEventHandler(this._textTableName_KeyPress);
				textTableName.Leave += new EventHandler(this._textTableName_Leave);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// テーブル名の編集が終わった
		/// </summary>
		private void EndTableNameEditing()
		{
			try
			{
				if ( listBoxTableList.Controls[textTableName.Name] == null )
					return;

				textTableName.Leave -= new EventHandler(this._textTableName_Leave);
				textTableName.KeyPress -= new KeyPressEventHandler(this._textTableName_KeyPress);
				textTableName.Hide();
				listBoxTableList.Controls.Remove(textTableName);
				editingTableNameIndex = -1;

				listBoxTableList.Select();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// textTableName がフォーカスを失った
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _textTableName_Leave(object sender, System.EventArgs e)
		{
			EndTableNameEditing();
		}

		/// <summary>
		/// textTableName でキーが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _textTableName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if ( e.KeyChar == (char)Keys.Enter )
				{
					int plainTblNameLen = int.Parse(((string)textTableName.Tag).Substring(0, 2));	// 2:D2(別名を除いたテーブル名の長さ)
					if ( plainTblNameLen <= textTableName.Text.Length )	// 別名が追加|削除された？
					{
						string alias = textTableName.Text.Substring(plainTblNameLen).Trim().ToUpper();
						string rawTableName = textTableName.Text.Substring(0, plainTblNameLen) + ((alias.Length == 0) ? "" : " " + alias);
						EditListBoxTableName(editingTableNameIndex, rawTableName);
					}

					EndTableNameEditing();
				}
				else if ( e.KeyChar == (char)Keys.Escape )
				{
					EndTableNameEditing();
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// リストボックスのテーブル名を編集する
		/// </summary>
		/// <param name="index"></param>
		/// <param name="rawTableName"></param>
		private void EditListBoxTableName(int index, string rawTableName)
		{
#if true
			string itemText = (string)listBoxTableList.Items[index];
			int indexTab = itemText.IndexOf('\t');
			string comment = string.Empty;

			if ( indexTab != -1 )
			{
				int maxTableName = (int)listBoxTableList.Tag;
				int maxTabTableName = (maxTableName / 8) + 1;

				int tabTableName = Math.Max(maxTabTableName - (GetByteCount(rawTableName) / 8), 1);
				comment = new string('\t', tabTableName) + itemText.Substring(indexTab).TrimStart(new char[] { '\t' });
			}

			listBoxTableList.Items[index] = rawTableName + comment;
#else
			string itemText = (string)listBoxTableList.Items[index];
			int indexTab = itemText.IndexOf('\t');
			string comment = (indexTab == -1) ? "" : itemText.Substring(indexTab);

			if ( indexTab != -1 )
			{
				int difLen = rawTableName.Length - indexTab;
				if ( 0 < difLen )	// 増えた？
				{
					for ( int len = (indexTab % 8) + difLen; 0 < len; len -= 8 )
					{
						if ( comment[1] != '\t' )
							break;
						comment = comment.Substring(1);
					}
				}
				else if ( difLen < 0 )	// 減った？
				{
					for ( int len = Math.Abs(difLen); 8 <= len; len -= 8 )
					{
						comment = "\t" + comment;
					}
				}
			}

			listBoxTableList.Items[index] = rawTableName + comment;
#endif
		}
#endif

#if ENABLED_SUBQUERY
		/// <summary>
		/// テーブル名がダブルクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxTableList_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				string tableName = GetListBoxTableName(selTbl.plainTblName);
				string xpath = "/" + tagTableList + "/" + tagTable + "[@" + attrType + "='" + SUBQUERY_TYPE + "' and @" + attrName + "='" + tableName + "']";
				XmlNode table = xmlTableList.SelectSingleNode(xpath);
				if ( table == null )	// サブクエリではない？
					return;

				string _xmlShenlongColumnFileName = table.Attributes[attrDir].Value + "\\" + tableName + ".xml";
				if ( !File.Exists(_xmlShenlongColumnFileName) )
					throw new Exception(_xmlShenlongColumnFileName + " がありません");

				string arguments = "\"" + _xmlShenlongColumnFileName + "\"" + " " +
								   Program.CMDPARAM_NEW_INSTANCE + " " +
								   (Program.expertMode ? Program.CMDPARAM_EXPERT_MODE : "") + " " +
								   (Program.debMode ? Program.CMDPARAM_DEBMODE : "");
				ProcessStartInfo startInfo = new ProcessStartInfo(Application.StartupPath + "\\" + Application.ProductName + ".exe", arguments);

				Process.Start(startInfo);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
#endif

		/// <summary>
		/// 入力された文字列でテーブル一覧を絞り込む
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textTableFilter_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if ( e.KeyCode != Keys.Enter )
					return;
				if ( xmlTableList == null )
					return;

				Cursor.Current = Cursors.WaitCursor;
				listBoxTableList.BeginUpdate();

				string tableFilter = textTableFilter.Text.Trim();

				selTableHistory = new List<int>();
				curSelColumnHistory = -1;

				textColumnFilter.Text = string.Empty;
				listBoxTableList.SelectedIndex = -1;
				listBoxColumnList.Items.Clear();

				// フィルタ無し？（ログイン直後の状態にする）
				if ( tableFilter.Length == 0 )
				{
					int sortColumn = 1;
					toolStripMenuSortTableName.Checked = true;
					toolStripMenuSortTableComment.Checked = false;
					ascendingTableName = true;

					List<string> tables;
					int maxTableName;
					if ( SortTableName(sortColumn, out tables, out maxTableName) )
					{
						listBoxTableList.Items.Clear();

						SetTableName(tables, maxTableName);
					}

					return;
				}

				bool matchFilter = true;			// 一致条件で絞り込む
				if ( tableFilter[0] == '!' )
				{
					matchFilter = false;			// 不一致条件で絞り込む
					tableFilter = tableFilter.Substring(1).TrimStart();
				}
				
				tableFilter = System.Text.RegularExpressions.Regex.Replace(tableFilter, "\\s+", " ");
				tableFilter = tableFilter.ToUpper();
				string[] tableFilters = tableFilter.Split(' ');

				// フィルタ条件以外のテーブル名はリストボックスから削除する
				for ( int i = listBoxTableList.Items.Count - 1; 0 <= i; i-- )
				{
					string tableName = listBoxTableList.Items[i].ToString().Split('\t')[0];
					int j;
					for ( j = 0; (j < tableFilters.Length) && (tableName.IndexOf(tableFilters[j]) == -1); j++ ) ;
					if ( matchFilter && (j < tableFilters.Length) )		// 一致した？
						continue;
					if ( !matchFilter && (j == tableFilters.Length) )	// 不一致？
						continue;
					listBoxTableList.Items.RemoveAt(i);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				listBoxTableList.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// textTableFilter_KeyDown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textTableFilter_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ( listBoxTableList.Items.Count == 0 )
					return;

				if ( e.KeyCode == Keys.Down )
				{
					listBoxTableList.TopIndex++;
					e.SuppressKeyPress = true;
				}
				else if ( e.KeyCode == Keys.Up )
				{
					listBoxTableList.TopIndex--;
					e.SuppressKeyPress = true;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// 入力された文字列でテーブル一覧をリアルタイムで絞り込む
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textTableFilter_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if ( xmlTableList == null )
					return;

				Cursor.Current = Cursors.WaitCursor;
				listBoxTableList.BeginUpdate();

				selTableHistory = new List<int>();
				curSelColumnHistory = -1;

				textColumnFilter.Text = string.Empty;
				listBoxTableList.SelectedIndex = -1;
				listBoxColumnList.Items.Clear();

				List<string> tables;
				int maxTableName;

				// フィルタ無し？（ログイン直後の状態にする）
				if ( textTableFilter.Text.Length == 0 )
				{
					int sortColumn = 1;
					toolStripMenuSortTableName.Checked = true;
					toolStripMenuSortTableComment.Checked = false;
					ascendingTableName = true;

					if ( SortTableName(sortColumn, out tables, out maxTableName) )
					{
						listBoxTableList.Items.Clear();

						SetTableName(tables, maxTableName);
					}

					return;
				}

				tables = new List<string>();
				maxTableName = 0;

				string _owner = null, _tableName = null;
				string tableFilter = textTableFilter.Text;

				if ( (1 < tableFilter.Length) && (tableFilter.IndexOf('.') != -1) )
				{
					if ( tableFilter.EndsWith(".") )		// owner.
					{
						_owner = tableFilter.Substring(0, tableFilter.Length - 1);
					}
					else if ( tableFilter.StartsWith(".") )	// .tableName
					{
						_tableName = tableFilter.Substring(1);
					}
					else if ( (tableFilter[0] != '.') && (tableFilter[tableFilter.Length - 1] != '.') )	// owner.tableName
					{
						_owner = tableFilter.Split('.')[0];
						_tableName = tableFilter.Split('.')[1];
					}
				}

				XDocument _xmlTableList = XDocument.Parse(xmlTableList.OuterXml);

				var query = from n in _xmlTableList.Root.Elements()
							where ((_owner == null && _tableName == null) && (n.Attribute(attrOwner).Value.Contains(tableFilter) || n.Attribute(attrName).Value.Contains(tableFilter))) ||
								  ((tableFilter == ".") && (n.Attribute(attrOwner).Value.Length != 0)) ||
								  ((_owner != null && _tableName == null) && n.Attribute(attrOwner).Value.EndsWith(_owner)) ||
								  ((_owner == null && _tableName != null) && (n.Attribute(attrOwner).Value.Length != 0) && (n.Attribute(attrName).Value.StartsWith(_tableName))) ||
								  ((_owner != null && _tableName != null) && (n.Attribute(attrOwner).Value.EndsWith(_owner)) && (n.Attribute(attrName).Value.StartsWith(_tableName)))
							select n;

				foreach ( XElement xeTable in query )
				{
					string owner = xeTable.Attribute(attrOwner).Value;
					string tableName = xeTable.Attribute(attrName).Value;
					if ( showSynonymOwner && (owner.Length != 0) )
					{
						tableName = owner + "." + tableName;
					}
					tables.Add(tableName + "\t" + xeTable.Attribute(attrComments).Value);
					maxTableName = Math.Max(maxTableName, GetByteCount(tableName));
				}

				listBoxTableList.Items.Clear();

				SetTableName(tables, maxTableName);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				listBoxTableList.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// textTableFilter のサイズが変化した
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textTableFilter_SizeChanged(object sender, EventArgs e)
		{
			MovePictureBoxClearAnyFilterControl(pictureBoxClearTableFilter, splitContainer1.Panel1);
		}

		/// <summary>
		/// フィルターを初期化するコントロールをテキストボックスの右端に合わせる
		/// </summary>
		/// <param name="pictureBoxClearAnyFilter"></param>
		/// <param name="splitterPanel"></param>
		private void MovePictureBoxClearAnyFilterControl(Control pictureBoxClearAnyFilter, SplitterPanel splitterPanel)
		{
			try
			{
				/*Point point = labelTableList.PointToScreen(labelTableList.Location);
				toolStripStatusVersion.Text = point.ToString();
				point.X = point.X + labelTableList.Width - pictureBoxClearTableFilter.Width;

				point = labelTableList.PointToClient(point);
				toolStripStatusVersion.Text += " " + textTableFilter.Location + " " + point.ToString();

				pictureBoxClearTableFilter.Top = point.Y + 1;
				pictureBoxClearTableFilter.Left = point.X;
				toolStripStatusVersion.Text = textTableFilter.Location + " " + pictureBoxClearTableFilter.Location;*/

				pictureBoxClearAnyFilter.Top = splitterPanel.Top + 1;
				pictureBoxClearAnyFilter.Left = /*splitterPanel.Left + */splitterPanel.Width - pictureBoxClearAnyFilter.Width - 6;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// pictureBoxClearTableFilter がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxClearTableFilter_Click(object sender, EventArgs e)
		{
			textTableFilter.Text = "";
		}

		/// <summary>
		/// textColumnFilter_KeyDown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textColumnFilter_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ( listBoxColumnList.Items.Count == 0 )
					return;

				if ( e.KeyCode == Keys.Down )
				{
					listBoxColumnList.TopIndex++;
					e.SuppressKeyPress = true;
				}
				else if ( e.KeyCode == Keys.Up )
				{
					listBoxColumnList.TopIndex--;
					e.SuppressKeyPress = true;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// 入力された文字列でカラム一覧をリアルタイムで絞り込む
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textColumnFilter_TextChanged(object sender, EventArgs e)
		{
			try
			{
				string columnListFileName = Application.StartupPath + "\\" + "~columnList.xml";
				if ( !File.Exists(columnListFileName) )
					return;

				Cursor.Current = Cursors.WaitCursor;
				listBoxColumnList.BeginUpdate();

				selColumnHistory = new List<int>();
				curSelColumnHistory = -1;

				listBoxColumnList.SelectedIndex = -1;

				List<string> columns = new List<string>();
				int maxColumnName = 0;
				int maxDataType = 0;
				int maxComments = 0;

				XDocument xmlColumnList = XDocument.Load(columnListFileName);
				XElement xeColumnList = xmlColumnList.Root;

				// フィルタ無し？（テーブル選択直後の状態にする）
				if ( textColumnFilter.Text.Length == 0 )
				{
					maxColumnName = int.Parse(xeColumnList.Attribute("maxColumnName").Value);
					maxDataType = int.Parse(xeColumnList.Attribute("maxDataType").Value);
					maxComments = int.Parse(xeColumnList.Attribute("maxComments").Value);

					foreach ( XElement xeColumn in xeColumnList.Elements() )
					{
						string column = xeColumn.Attribute("name").Value + "\t" + xeColumn.Attribute("type").Value + "\t" + xeColumn.Attribute("comment").Value + "\t" + xeColumn.Attribute("nullable").Value;
						columns.Add(column);
					}

					listBoxColumnList.Items.Clear();

					SetColumnName(columns, maxColumnName, maxDataType, maxComments);

					return;
				}

				var query = from n in xeColumnList.Elements()
							where n.Attribute(attrName).Value.Contains(textColumnFilter.Text)
							select n;

				foreach ( XElement xeColumn in query )
				{
					string columnName = xeColumn.Attribute("name").Value;
					string dataType = xeColumn.Attribute("type").Value;
					string comment = xeColumn.Attribute("comment").Value;
					string nullable = xeColumn.Attribute("nullable").Value;

					columns.Add(columnName + "\t" + dataType + "\t" + comment + "\t" + nullable);

					maxColumnName = Math.Max(maxColumnName, GetByteCount(columnName));
					maxDataType = Math.Max(maxDataType, GetByteCount(dataType));
					maxComments = Math.Max(maxComments, GetByteCount(comment));
				}

				listBoxColumnList.Items.Clear();

				SetColumnName(columns, maxColumnName, maxDataType, maxComments);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				listBoxColumnList.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// textColumnFilter のサイズが変わった
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textColumnFilter_SizeChanged(object sender, EventArgs e)
		{
			MovePictureBoxClearAnyFilterControl(pictureBoxClearColumnFilter, splitContainer1.Panel2);
		}

		/// <summary>
		/// pictureBoxClearColumnFilter がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxClearColumnFilter_Click(object sender, EventArgs e)
		{
			textColumnFilter.Text = "";
		}

		/// <summary>
		/// カラムが選択された
		/// </summary>
		private void listBoxColumnList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ( !selectColumnByDragDrop )
			{
				ListBox listBox = (ListBox)sender;
				if ( listBox.SelectedIndex == -1 )
					return;

				ColumnItemSelected(listBox.Text);
			}

			try
			{
				if ( (formKeyDownArgs == null) || !formKeyDownArgs.Alt )	// 戻る/進む以外で選択された？
				{
					int index = selColumnHistory.IndexOf(listBoxColumnList.SelectedIndex);
					if ( index != -1 )
					{
						selColumnHistory.Remove(listBoxColumnList.SelectedIndex);
					}
					selColumnHistory.Add(listBoxColumnList.SelectedIndex);
					curSelColumnHistory = selColumnHistory.Count - 1;		// 現在のカラムを最終履歴とする
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// カラムがダブルクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if ( columnListLastMouseDown == Point.Empty )
					return;

				ListBox listBox = (ListBox)sender;

				// アイテムのインデックスを取得する
				int itemIndex = listBox.IndexFromPoint(columnListLastMouseDown);
				string itemText = (string)listBox.Items[itemIndex];
				if ( string.IsNullOrEmpty(itemText) )
					return;

				listBox.SelectedIndex = itemIndex;

				ColumnItemSelected(itemText);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// listBoxColumnList でキーが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ( listBoxColumnList.Items.Count == 0 )
					return;

				if ( e.Alt )	// Alt キーが押されている？
				{
					if ( (selColumnHistory.Count != 0)/* && (listBoxColumnList.Focused)*/ )	// 選択されたカラムの履歴があり、カラム一覧にフォーカスがある？
					{
						int move = 0;

						if ( (e.KeyCode == Keys.Left) && (curSelColumnHistory != 0) )		// 戻る(Alt + ←)？
						{
							move = -1;
						}
						else if ( (e.KeyCode == Keys.Right) && (curSelColumnHistory != (selColumnHistory.Count - 1)) )	// 進む(Alt + →)？
						{
							move = 1;
						}

						if ( move != 0 )
						{
							listBoxColumnList.SelectedIndices.Clear();
							curSelColumnHistory += move;
							listBoxColumnList.SelectedIndex = selColumnHistory[curSelColumnHistory];	// 履歴のカラムで一覧を選択状態にする
						}
					}
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
#if (DEBUG)
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
			}
		}

		/// <summary>
		/// listBoxColumnList でマウスボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				// マウスの左ボタンだけが押されている時のみドラッグできるようにする
				if ( e.Button == MouseButtons.Left )
				{
					// ドラッグの準備
					ListBox lbx = (ListBox)sender;

					// マウスの押された位置を記憶する
					if ( lbx.IndexFromPoint(e.X, e.Y) >= 0 )
						columnListLastMouseDown = new Point(e.X, e.Y);
				}
				else
				{
					columnListLastMouseDown = Point.Empty;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// listBoxColumnList でマウスボタンが離れた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_MouseUp(object sender, MouseEventArgs e)
		{
			columnListLastMouseDown = Point.Empty;
		}

		/// <summary>
		/// listBoxColumnList でマウスボタンが移動した
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if ( columnListLastMouseDown == Point.Empty )
					return;

				// ドラッグとしないマウスの移動範囲を取得する
				Rectangle moveRect = new Rectangle(columnListLastMouseDown.X - SystemInformation.DragSize.Width / 2,
												   columnListLastMouseDown.Y - SystemInformation.DragSize.Height / 2,
												   SystemInformation.DragSize.Width,
												   SystemInformation.DragSize.Height);

				// ドラッグとする移動範囲を超えたか調べる
				if ( moveRect.Contains(e.X, e.Y) )
					return;

				// ドラッグの準備
				ListBox lbx = (ListBox)sender;

				// ドラッグするアイテムのインデックスを取得する
				int itemIndex = lbx.IndexFromPoint(columnListLastMouseDown);
				//if ( itemIndex < 0 )
				//	return;
				lbx.SelectedIndex = itemIndex;
				//if ( lbx.SelectedItems.Count == 0 )
				//	return;

				// ドラッグするアイテムの内容を取得する
				string itemText/* = (string)lbx.Items[itemIndex]*/;
				StringBuilder itemTexts = new StringBuilder();
				for ( int i = 0; i < lbx.SelectedItems.Count; i++ )
				{
					itemTexts.Append((string)lbx.SelectedItems[i] + "\0");
				}
				itemText = itemTexts.ToString().Substring(0, itemTexts.Length - 1);

				// ドラッグ&ドロップ処理を開始する
				DragDropEffects dde = lbx.DoDragDrop(itemText, DragDropEffects.Move/*DragDropEffects.All | DragDropEffects.Link*/);

				/*// ドロップ効果がMoveの時はもとのアイテムを削除する
				if ( dde == DragDropEffects.Move )
					lbx.Items.RemoveAt(itemIndex);*/

				columnListLastMouseDown = Point.Empty;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// マウス カーソルを指定する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			// 既定のカーソルを使用しない
			e.UseDefaultCursors = false;

			// ドロップ効果にあわせてカーソルを指定する
			if ( (e.Effect & DragDropEffects.Move) == DragDropEffects.Move )
				Cursor.Current = copyCursor/*moveCursor*/;
			/*else if ( (e.Effect & DragDropEffects.Copy) == DragDropEffects.Copy )
				Cursor.Current = copyCursor;
			else if ( (e.Effect & DragDropEffects.Link) == DragDropEffects.Link )
				Cursor.Current = linkCursor;*/
			else
				Cursor.Current = noneCursor;
		}

		/// <summary>
		/// ドラッグをキャンセルする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			// マウスの右ボタン(2)が押されていればドラッグをキャンセル
			if ( (e.KeyState & 2) == 2 )
			{
				e.Action = DragAction.Cancel;
			}
		}

		/// <summary>
		/// listBoxColumnList のオーナードロー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxColumnList_DrawItem(object sender, DrawItemEventArgs e)
		{
			try
			{
				if ( e.Index == -1 )
					return;

				e.DrawBackground();

				RectangleF rect = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

				// 奇数項目の背景を設定する
				if ( e.Index % 2 == 1 )
				{
					// 項目が選択されている場合は変更しない
					if ( (e.State & DrawItemState.Selected) == 0 )
					{
						e.Graphics.FillRectangle(columnListBackColor, rect);
					}
				}

#if UPDATE_20251202
				var flags = TextFormatFlags.NoPadding | TextFormatFlags.ExpandTabs | TextFormatFlags.NoPrefix;
				var bounds = e.Bounds;
				bounds.X += 2;
				bounds.Width -= 2;

                TextRenderer.DrawText(e.Graphics, (string)listBoxColumnList.Items[e.Index], e.Font, bounds, listBoxColumnList.ForeColor, flags);

				e.DrawFocusRectangle();
#else
				Brush brush = new SolidBrush(listBoxColumnList.ForeColor);

				e.Graphics.DrawString((string)listBoxColumnList.Items[e.Index], e.Font, brush, rect);

				brush.Dispose();
#endif
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

#if UPDATE_20251202
        /// <summary>
		/// 最大幅を計測して HorizontalExtent を更新
		/// </summary>
		/// <param name="lb"></param>
		void UpdateHorizontalExtent(ListBox lb)
        {
            var flags = TextFormatFlags.NoPadding | TextFormatFlags.ExpandTabs | TextFormatFlags.NoPrefix;
            int maxWidth = 0;

            foreach (var item in lb.Items)
            {
                var text = item?.ToString() ?? "";
                // ExpandTabs を計測側にも適用する
                var size = TextRenderer.MeasureText(text, lb.Font, new Size(int.MaxValue, int.MaxValue), flags);
                if (size.Width > maxWidth) maxWidth = size.Width;
            }

            lb.HorizontalExtent = maxWidth + 4; // 余白ぶん
        }
#endif

        /// <summary>
        /// タブ コントロールの選択が変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				bool enabled = false;

				if ( tabControl.SelectedIndex == 0 )		// クエリー項目？
				{
					enabled = (lveQueryColumn.Columns.Count != 0);
				}
				else if ( tabControl.SelectedIndex == 2 )	// SQL ?
				{
					textSQL.Select();
					textSQL.Select(0, 0);
				}

				toolStripRemoveEndColumn.Enabled = enabled;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// タブ コントロールがダブルクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabControl_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if ( tabControl.SelectedTab == tabSQL )
				{
					if ( !string.IsNullOrEmpty(oracleSqlPlusPath) )
					{
						string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
						ProcessStartInfo startInfo = new ProcessStartInfo();
						startInfo.Arguments = logOn[(int)logon.uid] + "/" + toolStripStatusOraConn.Tag + "@" + logOn[(int)logon.sid];	// UID/PWD@SID
						startInfo.FileName = oracleSqlPlusPath;
						Process.Start(startInfo);
					}
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// contextMenuTableJoin が開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuTableJoin_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				contextMenuTableJoin.Enabled = (lvTableJoin.SelectedIndices.Count != 0);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [結合解除] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuReleaseJoin_Click(object sender, EventArgs e)
		{
			try
			{
				lvTableJoin.Items.RemoveAt(lvTableJoin.SelectedIndices[0]);

				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [左右の項目を入替] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuSwapColumn_Click(object sender, EventArgs e)
		{
			try
			{
				ListViewItem selectedItem = lvTableJoin.SelectedItems[0];
				string leftColumn = selectedItem.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text;
				selectedItem.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text = selectedItem.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text;
				selectedItem.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text = leftColumn;

				toolTipQueryColumn.SetToolTip(lveQueryColumn, null);
				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [結合方法] のサブメニューが開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuJoinWay_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				ListViewItem lvi = lvTableJoin.SelectedItems[0];
				string way = lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text;
				toolStripMenuInnerJoin.Checked = (way == "=");
				toolStripMenuLeftJoin.Checked = (way == ">=");
				toolStripMenuRightJoin.Checked = (way == "<=");
				toolStripMenuFullOuterJoin.Checked = (way == ">=<");
				toolStripMenuFullOuterJoin.Enabled = fileUseJoin;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [等価結合 (＝)]|[左外部結合 (≧)]|[右外部結合 (≦)]|[左右外部結合 (≧≦)] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuChangeJoinWay_Click(object sender, EventArgs e)
		{
			try
			{
				ToolStripMenuItem toolStripMenuAnyJoin = (ToolStripMenuItem)sender;
				string way = "=";
				if ( toolStripMenuAnyJoin.Name == toolStripMenuInnerJoin.Name )
					way = "=";
				else if ( toolStripMenuAnyJoin.Name == toolStripMenuLeftJoin.Name )
					way = ">=";
				else if ( toolStripMenuAnyJoin.Name == toolStripMenuRightJoin.Name )
					way = "<=";
				else if ( toolStripMenuAnyJoin.Name == toolStripMenuFullOuterJoin.Name )
					way = ">=<";
				ListViewItem lvi = lvTableJoin.SelectedItems[0];
				lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text = way;

				toolTipQueryColumn.SetToolTip(lveQueryColumn, null);
				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// テーブル結合条件がダブルクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvTableJoin_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				// マウスポインタのあるアイテムを取得
				ListViewItem lvi = lvTableJoin.GetItemAt(e.X, e.Y);
				if ( lvi == null )
					return;

				int n;
				for ( n = lvTableJoin.Items[0].SubItems.Count - 1; (0 <= n) && (e.X < lvTableJoin.Items[0].SubItems[n].Bounds.X); n-- ) ;
#if TABLE_NAME_HAS_ALIAS
				string tableName, columnName;
				if ( !ShenGlobal.SplitTableFieldName(lvi.SubItems[n].Text, out tableName, out columnName, null) )
					return;

				ReverseQueryColumn(tableName, columnName, true);
#else
				string[] tableColumn = lvi.SubItems[n].Text.Split('.');
				if ( tableColumn.Length != 2 )
					return;

				ReverseQueryColumn(tableColumn[0], tableColumn[1]);
#endif
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// lvTableJoin_KeyUp
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvTableJoin_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				/// 結合条件の順番を入れ替える（MultiSelect を True にしておかないとダメ）
				if ( e.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) )
				{
					if ( lvTableJoin.SelectedIndices.Count != 0 )
					{
						int selected = lvTableJoin.SelectedIndices[0];
						int insert = -1;

						if ( e.KeyCode == Keys.Up )
						{
							if ( selected != 0 )
							{
								insert = selected - 1;
							}
						}
						if ( e.KeyCode == Keys.Down )
						{
							if ( selected != lvTableJoin.Items.Count - 1 )
							{
								insert = selected + 1;
							}
						}
						if ( insert == -1 )
							return;

						ListViewItem lvi = lvTableJoin.Items[selected];
						lvTableJoin.Items.Remove(lvi);

						lvTableJoin.Items.Insert(insert, lvi);
						lvTableJoin.Items[insert].Selected = true;
						lvTableJoin.Items[insert].Focused = true;

						ChangeModified(true);
					}
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// ヘッダ用のオーナードロー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvTableJoin_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
#if false
            e.DrawDefault = true;
#else
			e.DrawBackground();

			e.Graphics.FillRectangle(queryColumnHeaderBackColor, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);

			using ( StringFormat sf = new StringFormat() )
			{
				// Draw the header text.
				Rectangle rect = e.Bounds;
				rect.Height = qcFontHeight;

				rect.Y += 3;

				sf.Alignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				e.Graphics.DrawString(e.Header.Text, queryColumnFont, Brushes.DarkBlue, rect/*e.Bounds*/, sf);
			}
#endif
		}

		/// <summary>
		/// サブアイテム用のオーナードロー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvTableJoin_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			e.DrawDefault = true;
		}

		/// <summary>
		/// textSQL_TextChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textSQL_TextChanged(object sender, EventArgs e)
		{
			try
			{
				bool? enabled = null;

				if ( textSQL.Text.Length == 0 )
				{
					if ( lveQueryColumn.Columns.Count == 0 )
					{
						enabled = false;
						ChangeModified(false);
					}
				}
				else
				{
					if ( !toolStripMenuNew.Enabled )
					{
						enabled = true;
					}
					ChangeModified(true);
				}

				if ( enabled != null )
				{
					toolStripMenuNew.Enabled = (bool)enabled;
					toolStripMenuSave.Enabled = (bool)enabled;
					toolStripMenuSaveAs.Enabled = (bool)enabled;
					toolStripMenuToExcel.Enabled = (bool)enabled;
					toolStripNew.Enabled = (bool)enabled;
					toolStripSave.Enabled = (bool)enabled;
					toolStripToExcel.Enabled = (bool)enabled;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// textSQL_KeyDown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textSQL_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//textSqlSelectedAll = false;

			try
			{
				if ( e.Control && (e.KeyCode == Keys.A) )
				{
					textSQL.SelectAll();
					e.Handled = true;	// 本当は、ここで textSqlSelectedAll フラグをセットして、KeyPress イベント内で Handled = true にした方がいい？
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// クエリ項目の[テーブル].[カラム]を反転させる
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="columnName"></param>
		private void ReverseQueryColumn(string tableName, string columnName, bool moveCursor)
		{
			try
			{
				api.SendMessage(lveQueryColumn.Handle, api.WM_HSCROLL, api.SB_LEFT, 0);

				int[] colOrder = lveQueryColumn.GetColumnOrder();
				int width = 0;
				for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
				{
					int j = colOrder[i];
					if ( (lveQueryColumn.Columns[j].Text == tableName) &&
						 (lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text == columnName) )
					{
						tabControl.SelectedIndex = 0;
						api.SendMessage(lveQueryColumn.Handle, api.LVM_SCROLL, (uint)width, 0);

						reverseQueryColumn = i;
						InvalidateQueryColumn(reverseQueryColumn);

						if ( moveCursor )
						{
							Rectangle rect = lveQueryColumn.Items[0].SubItems[colOrder[i]].Bounds;
							Cursor.Position = lveQueryColumn.PointToScreen(new Point(rect.X + (int)(lveQueryColumn.Columns[j].Width * 0.8F)/*(lveQueryColumn.Columns[j].Width / 2)*/, rect.Y + (rect.Height / 2)));
						}

						timerReverseQueryColumn.Interval = reverseQueryColumnTime;
						timerReverseQueryColumn.Start();
						break;
					}

					width += lveQueryColumn.Columns[j].Width;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// テーブル結合元のカラムの反転表示を解除する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerReverseQueryColumn_Tick(object sender, EventArgs e)
		{
			try
			{
				timerReverseQueryColumn.Stop();

				if ( reverseQueryColumn != -1 )
				{
					InvalidateQueryColumn(reverseQueryColumn);
					reverseQueryColumn = -1;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}

		}
#endregion

		#region クエリー項目のメソッド
		/// <summary>
		/// クエリー項目の DragOver
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lveQueryColumn_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				// ドラッグされているデータがstring型か調べる
				if ( e.Data.GetDataPresent(typeof(string)) )
				{
					// Ctrlキー(8)が押されていれば Copy
					if ( (e.KeyState & 8) == 8 && (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy )
						e.Effect = DragDropEffects.Copy;
					// Altキー(32)が押されていれば Link
					else if ( (e.KeyState & 32) == 32 && (e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link )
						e.Effect = DragDropEffects.Link;
					// 何も押されていなければ Move
					else if ( (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move )
						e.Effect = DragDropEffects.Move;
					else
						e.Effect = DragDropEffects.None;
				}
				else
				{
					// string 型でなければ受け入れない
					e.Effect = DragDropEffects.None;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目にドロップされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lveQueryColumn_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				// ドロップされたデータが string 型か調べる
				if ( !e.Data.GetDataPresent(typeof(string)) )
					return;

				int index = -1;
#if true
				// ドロップされた項目上に挿入する
				Point pointScreen = /*new Point(e.X, e.Y)*/Cursor.Position;
				Point pointClient = lveQueryColumn.PointToClient(pointScreen);
				ListViewItem lvi = lveQueryColumn.GetItemAt(pointClient.X, Math.Max(pointClient.Y, 6));	// Y が 5 以下だと lvi が null になる？
				Debug.WriteLine(lvi + " " + pointScreen + " " + pointClient);
				if ( lvi != null )
				{
					// マウスポインタのあるアイテムを取得
					int[] colOrder = lveQueryColumn.GetColumnOrder();
					for ( index = lvi.SubItems.Count - 1; (0 <= index) && (pointClient.X < lvi.SubItems[colOrder[index]].Bounds.X); index-- ) ;
				}
#endif

				string[] columns = ((string)e.Data.GetData(typeof(string))).Split('\0');
				int lastColCount = lveQueryColumn.Columns.Count;

				for ( int i = 0; i < columns.Length; i++ )
				{
					if ( columns[i][0] == '*' )
					{
						for ( int j = 0; j < listBoxColumnList.Items.Count - 1; j++ )
						{
							if ( !AppendSelectedColumnItem(listBoxColumnList.Items[j].ToString(), index) )
								break;
						}
						break;
					}

					if ( !AppendSelectedColumnItem(columns[i], index) )
						break;
				}

				if ( (lastColCount != lveQueryColumn.Columns.Count) && (index == -1) )
				{
					api.PostMessage(lveQueryColumn.Handle, api.WM_HSCROLL, api.SB_RIGHT, 0);
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// contextMenuQueryColumn が開かれようとしている
		/// </summary>
		private void contextMenuQueryColumn_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				ListViewItem lvi = null;

				lveQueryColumn.EndEditing(false);

				toolStripMenuDelete.Text = "項目削除(&D)";

				Point pointScreen = Cursor.Position/*new Point(contextMenuQueryColumn.Bounds.X, contextMenuQueryColumn.Bounds.Y)*/;
				Point pointClient = lveQueryColumn.PointToClient(pointScreen);
				lvi = lveQueryColumn.GetItemAt(pointClient.X, pointClient.Y);

				bool enableMenu = !((lveQueryColumn.Columns.Count == 0) || (lvi == null) || (lveQueryColumn.ValidItemCount <= lvi.Index));
				toolStripMenuDelete.Enabled = enableMenu;
				toolStripMenuPasteHere.Enabled = enableMenu;
				toolStripMenuTableJoin.Enabled = enableMenu;
				toolStripMenuColumnProperty.Enabled = enableMenu;
				if ( !enableMenu )
					return;

				// マウスポインタのあるアイテムを取得
				int[] colOrder = lveQueryColumn.GetColumnOrder();
				int n;
				for ( n = lvi.SubItems.Count - 1; (0 <= n) && (pointClient.X < lvi.SubItems[colOrder[n]].Bounds.X); n-- ) ;

				contextMenuQueryColumn.Tag = n;	// 選択されたカラム番号（並び順に対する）
				reverseQueryColumn = n;

				string selTableName = lveQueryColumn.Columns[colOrder[n]].Text;
				string selFieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[colOrder[n]].Text;

				// [項目削除] メニュー
				toolStripMenuDelete.Text = "[" + selTableName + "." + selFieldName + "] を削除(&D)";
				if ( (1 < lveQueryColumn.Columns.Count) && (colOrder[n] == 0) )
				{
					toolStripMenuDelete.Enabled = false;	// カラムの順番が変更されている時、０列目は削除できないようにしておく（OwnerDraw の関係で）
				}
				toolStripMenuDelete.Tag = colOrder[n];	// 削除するカラム番号

				// [ここに貼り付け] メニュー
				toolStripMenuPasteHere.Visible = (xmlCopiedShenlongColumn != null);
				toolStripMenuPasteHere.Tag = n/*colOrder[n]*/;

				// [テーブル結合] メニュー
				if ( toolStripMenuTableJoin.Enabled = (1 < queryTableNames.Count) )
				{
					if ( toolStripMenuTableJoin.DropDownItems.Count == 0 )
					{
						// ドロップダウンの▲マークを出す為に、ダミー メニューを追加しておく
						contextTableJoinColumns = new ToolStripMenuItem[1];
						contextTableJoinColumns[0] = new ToolStripMenuItem();
						toolStripMenuTableJoin.DropDownItems.Add(contextTableJoinColumns[0]);
					}
				}
				else
				{
					ClearContextTableJoinColumns();
				}

				// [プロパティ] メニュー
				toolStripMenuColumnProperty.Tag = colOrder[n];

				// 選択されたカラムを反転させる
				InvalidateQueryColumn(n);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// contextMenuQueryColumn が閉じられようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuQueryColumn_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			try
			{
				// 選択されたカラムの反転表示を解除する
				InvalidateQueryColumn(reverseQueryColumn);

				contextMenuQueryColumn.Tag = -1;
				reverseQueryColumn = -1;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// [項目削除] コンテキスト メニュー
		/// </summary>
		private void toolStripMenuDelete_Click(object sender, EventArgs e)
		{
			try
			{
#if (DEBUG)
				Point pointScreen = Cursor.Position/*new Point(contextMenuQueryColumn.Bounds.X, contextMenuQueryColumn.Bounds.Y)*/;
				Point pointClient = lveQueryColumn.PointToClient(pointScreen);
				// マウスポインタのあるアイテムを取得
				ListViewItem lvi = lveQueryColumn.GetItemAt(pointClient.X, pointClient.Y);
				ListViewHitTestInfo hitTestInfo = lveQueryColumn.HitTest(pointClient);
#endif

				//int[] colOrder = lveQueryColumn.GetColumnOrder();
				//int n;
				//for ( n = lveQueryColumn.Items[0].SubItems.Count - 1; (0 <= n) && (pointClient.X < lveQueryColumn.Items[0].SubItems[colOrder[n]].Bounds.X); n-- ) ;
				RemoveQueryColumn((int)toolStripMenuDelete.Tag/*colOrder[n]*/);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [ここに貼り付け] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuPasteHere_Click(object sender, EventArgs e)
		{
			try
			{
				int index = (int)toolStripMenuPasteHere.Tag;

				PasteCopyShenlongColumn(index);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// [テーブル結合] のサブメニューをクリアする
		/// </summary>
		private void ClearContextTableJoinColumns()
		{
			if ( contextTableJoinColumns.Length == 0 )
				return;

			// 結合候補の [テーブル][カラム名] コンテキストを解放する
			for ( int i = 0; i < contextTableJoinCandiColumns.Length; i++ )
			{
				foreach ( ToolStripMenuItem tableJoinCadiColumns in contextTableJoinCandiColumns[i] )
				{
					tableJoinCadiColumns.Dispose();
				}
			}
			contextTableJoinCandiColumns = new ToolStripMenuItem[0][];

			// 選択されたカラムの 相対[テーブル名.カラム名] | 結合候補[テーブル名] コンテキストを解放する
			foreach ( ToolStripMenuItem tableJoinColumns in contextTableJoinColumns )
			{
				tableJoinColumns.DropDownItems.Clear();
				tableJoinColumns.Dispose();
			}
			contextTableJoinColumns = new ToolStripMenuItem[0];

			toolStripMenuTableJoin.DropDownItems.Clear();
		}

		/// <summary>
		/// [テーブル結合] のサブメニューが開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuTableJoin_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				TimeSpan timeStart = new TimeSpan(DateTime.Now.Ticks);

				ClearContextTableJoinColumns();

				int[] colOrder = lveQueryColumn.GetColumnOrder();
				int n = (int)contextMenuQueryColumn.Tag;

				string selTableName = lveQueryColumn.Columns[colOrder[n]].Text;
				string selFieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[colOrder[n]].Text;

				const string tagCandidate = "candidate";	// 結合候補のテーブル毎にカラムを取得する
				const string tagTable = "table";
				const string attrName = "name";
				const string tagField = "field";
				const string attrProperty = "property";
				XmlDocument xmlCandidateTable = new XmlDocument();
				XmlDeclaration decl = xmlCandidateTable.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlCandidateTable.AppendChild(decl);
				xmlCandidateTable.AppendChild(xmlCandidateTable.CreateElement(tagCandidate));	// <candidate>
				int relativeFieldCount = 0;

				string selTableFieldName = selTableName + "." + selFieldName;
				toolStripMenuTableJoin.Tag = selTableFieldName;

				for ( int i = 0; i < lveQueryColumn.Columns.Count; i++ )
				{
					int j = colOrder[i];
					if ( lveQueryColumn.Columns[j].Text == selTableName )	// 選択されたカラムと同じテーブルはスキップする
						continue;

					if ( intelliTableJoinMenu )
					{
						// 選択されたカラムと同じカラム名？
						if ( lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text == selFieldName )
						{
							string tableFieldName = lveQueryColumn.Columns[j].Text + "." + lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text;

							int m;
							for ( m = 0; (m < contextTableJoinColumns.Length) && (tableFieldName == contextTableJoinColumns[m].Text); m++ ) ;
							if ( (m != 0) && (m == contextTableJoinColumns.Length) )	// 既に追加済みの項目？
								continue;

							Array.Resize(ref contextTableJoinColumns, contextTableJoinColumns.Length + 1);
							m = contextTableJoinColumns.Length - 1;

							// 相対[テーブル名.カラム名] をコンテキストに追加する
							string[] property = lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[j].Text.Split(sepProp[0]);
							contextTableJoinColumns[m] = new ToolStripMenuItem();
							contextTableJoinColumns[m].Name = "toolStripMenuTableJoinColumn" + m;
							contextTableJoinColumns[m].Text = tableFieldName;
							//contextTableJoinColumns[m].Enabled = (HasTableJoin(selTableFieldName, tableFieldName, 2) == -1);
							contextTableJoinColumns[m].ToolTipText = (property[(int)ShenGlobal.prop.comment] == ShenGlobal.propNoComment ? "" : (property[(int)ShenGlobal.prop.comment] + "\r\n")) + property[(int)ShenGlobal.prop.type] + "(" + property[(int)ShenGlobal.prop.length] + ")";
							contextTableJoinColumns[m].Click += new System.EventHandler(this.toolStripMenuTableJoinColumn_Click);
							if ( HasTableJoin(selTableFieldName, tableFieldName, 2) != -1 )
								contextTableJoinColumns[m].Font = contextTableJoinFont;
							relativeFieldCount++;
							continue;
						}
					}

					// xmlCandidateTable に 結合候補の[テーブル名]と[カラム名]（プロパティも）を追加する
					string candidateTableName = lveQueryColumn.Columns[j].Text;
					string xpath = "/" + tagCandidate + "/" + tagTable + "[@" + attrName + "='" + candidateTableName + "']";
					XmlNode tableNode = xmlCandidateTable.SelectSingleNode(xpath);
					if ( tableNode == null )
					{
						tableNode = xmlCandidateTable.CreateNode(XmlNodeType.Element, tagTable, null);	// <table>
						XmlAttribute attr = xmlCandidateTable.CreateAttribute(attrName);				// @name
						attr.Value = candidateTableName;
						tableNode.Attributes.Append(attr);
						xmlCandidateTable.DocumentElement.AppendChild(tableNode);
					}

					string candidateFieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[j].Text.Replace("'", "’");
					xpath = tagField + "[@" + attrName + "='" + candidateFieldName + "']";
					//XmlNode fieldNode = tableNode.SelectSingleNode(xpath);
					XmlNode fieldNode = null;	// 例外が発生して検索できない（フィールド名に'(Apostrophe:&apos;)がある）場合、とりあえず追加しておく。直接書かれた関数などが、ダブる事はないので…。と思ったが xml は .Replace("'", "’") で変換して、メニューでは .Replace("’", "'") で復元するようにした。
					try
					{
						fieldNode = tableNode.SelectSingleNode(xpath);
					}
					catch ( Exception exp )
					{
						Debug.WriteLine(exp.Message);
					}
					if ( fieldNode == null )
					{
						fieldNode = xmlCandidateTable.CreateElement(tagField);							// <field>
						XmlAttribute attr = xmlCandidateTable.CreateAttribute(attrName);				// @name
						attr.Value = candidateFieldName;
						fieldNode.Attributes.Append(attr);
						attr = xmlCandidateTable.CreateAttribute(attrProperty);							// @property
						attr.Value = lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[j].Text;
						fieldNode.Attributes.Append(attr);
						tableNode.AppendChild(fieldNode);
					}
				}

				if ( Program.debMode )
				{
					xmlCandidateTable.Save(Application.StartupPath + "\\" + "~candidateTable.xml");
				}

				XmlNodeList candidateTableList = xmlCandidateTable.DocumentElement.ChildNodes;
				// 結合候補[テーブル名] の数だけ領域を確保する
				contextTableJoinCandiColumns = new ToolStripMenuItem[candidateTableList.Count][];
				Array.Resize(ref contextTableJoinColumns, contextTableJoinColumns.Length + contextTableJoinCandiColumns.Length);

				for ( int i = 0; i < candidateTableList.Count; i++ )
				{
					if ( (Cursor.Current != Cursors.WaitCursor) && (100 < new TimeSpan(DateTime.Now.Ticks - timeStart.Ticks).Milliseconds) )
					{
						Cursor.Current = Cursors.WaitCursor;	// カーソルのちらつき防止
					}

					XmlNode candidateTableNode = candidateTableList[i];
					XmlNodeList fieldList = candidateTableNode.ChildNodes;
					// 結合候補[カラム名] の数だけ領域を確保する
					contextTableJoinCandiColumns[i] = new ToolStripMenuItem[fieldList.Count];

					for ( int mm = 0; mm < fieldList.Count; mm++ )
					{
						XmlNode fieldNode = fieldList[mm];
						// 結合候補[カラム名] をコンテキストに追加する
						string[] property = fieldNode.Attributes[attrProperty].Value.Split(sepProp[0]);
						contextTableJoinCandiColumns[i][mm] = new ToolStripMenuItem();
						contextTableJoinCandiColumns[i][mm].Tag = candidateTableNode.Attributes[attrName].Value;
						contextTableJoinCandiColumns[i][mm].Name = "toolStripMenuOtherTableJoinColumn" + mm;
						contextTableJoinCandiColumns[i][mm].Text = fieldNode.Attributes[attrName].Value.Replace("’", "'");
						//contextTableJoinCandiColumns[i][mm].Enabled = (HasTableJoin(selTableFieldName, candidateTableNode.Attributes[attrName].Value + "." + fieldNode.Attributes[attrName].Value, 2) == -1);
						contextTableJoinCandiColumns[i][mm].ToolTipText = (property[(int)ShenGlobal.prop.comment] == ShenGlobal.propNoComment ? "" : (property[(int)ShenGlobal.prop.comment] + "\r\n")) + property[(int)ShenGlobal.prop.type] + "(" + property[(int)ShenGlobal.prop.length] + ")";
						contextTableJoinCandiColumns[i][mm].Click += new System.EventHandler(this.toolStripMenuTableJoinColumn_Click);
						if ( HasTableJoin(selTableFieldName, candidateTableNode.Attributes[attrName].Value + "." + contextTableJoinCandiColumns[i][mm].Text/*fieldNode.Attributes[attrName].Value*/, 2) != -1 )
						{
							contextTableJoinCandiColumns[i][mm].Font = contextTableJoinFont;
						}
					}

					// 結合候補[テーブル名]
					int m = relativeFieldCount + i;
					contextTableJoinColumns[m] = new ToolStripMenuItem();
					contextTableJoinColumns[m].Name = "toolStripMenuTableJoinColumn" + m;
					contextTableJoinColumns[m].Text = candidateTableNode.Attributes[attrName].Value;
					contextTableJoinColumns[m].Image = global::Shenlong.Properties.Resources.queryColumn;
					// 結合候補[テーブル名] のサブメニューに [カラム名] コンテキストを追加する
					contextTableJoinColumns[m].DropDownItems.AddRange(contextTableJoinCandiColumns[i]);
				}

				if ( !(toolStripMenuTableJoin.Enabled = (contextTableJoinColumns.Length != 0)) )
					return;

				// [テーブル結合] のサブメニューに、相対[テーブル名.カラム名] | 結合候補[テーブル名] コンテキストを追加する
				for ( int i = 0; i < contextTableJoinColumns.Length; i++ )
				{
					toolStripMenuTableJoin.DropDownItems.Add(contextTableJoinColumns[i]);
					if ( (relativeFieldCount != 0) && (i == relativeFieldCount - 1) && (i != contextTableJoinColumns.Length - 1) )
					{
						toolStripMenuTableJoin.DropDownItems.Add(new ToolStripSeparator());	// セパレータ
					}
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
		
		/// <summary>
		/// [テーブル結合] - [テーブル名.カラム名] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuTableJoinColumn_Click(object sender, EventArgs e)
		{
			try
			{
				string leftTableColumn = toolStripMenuTableJoin.Tag.ToString();
				string rightTableName, rightColumnName, rightTableColumn;

				ToolStripMenuItem menuItemTableJoinColumn = (ToolStripMenuItem)sender;
				if ( menuItemTableJoinColumn.Tag != null )
				{
					rightTableName = menuItemTableJoinColumn.Tag.ToString();
					rightColumnName = menuItemTableJoinColumn.Text;
				}
				else
				{
#if TABLE_NAME_HAS_ALIAS
					ShenGlobal.SplitTableFieldName(menuItemTableJoinColumn.Text, out rightTableName, out rightColumnName, null);
#else
					int dot = menuItemTableJoinColumn.Text.IndexOf('.');
					rightTableName = menuItemTableJoinColumn.Text.Substring(0, dot);
					rightColumnName = menuItemTableJoinColumn.Text.Substring(dot + 1);
#endif
				}
				rightTableColumn = rightTableName + "." + rightColumnName;

				if ( HasTableJoin(leftTableColumn, rightTableColumn, 2) != -1 )
				{
					ReverseQueryColumn(rightTableName, rightColumnName, true);
					return;
				}

				ListViewItem lvi = new ListViewItem(leftTableColumn);	// ShenCore.tabJoin.leftTabCol
				lvi.SubItems.Add("=");									// ShenCore.tabJoin.way
				lvi.SubItems.Add(rightTableColumn);						// ShenCore.tabJoin.rightTabCol
				lvTableJoin.Items.Add(lvi);

				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// [プロパティ] コンテキスト メニュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuColumnProperty_Click(object sender, EventArgs e)
		{
			try
			{
				int column = (int)toolStripMenuColumnProperty.Tag;
				string tableName = lveQueryColumn.Columns[column].Text;
				string fieldName = lveQueryColumn.Items[(int)ShenGlobal.qc.fieldName].SubItems[column].Text;
				string[] property = lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[column].Text.Split(sepProp[0]);

				//bool outputOnly = ((lveQueryColumn.Items[(int)ShenCore.qc.expression].SubItems[column].Text.Length == 0) || (lveQueryColumn.Items[(int)ShenCore.qc.value1].SubItems[column].Text.Length == 0));
				bool[] bubbPagesEnable = new bool[2];//{ ((lveQueryColumn.Items[(int)ShenCore.qc.expression].SubItems[column].Text.Length == 0) || (lveQueryColumn.Items[(int)ShenCore.qc.value1].SubItems[column].Text.Length == 0)), bool.Parse(lveQueryColumn.Items[(int)ShenCore.qc.showField].SubItems[column].Text) };
				bubbPagesEnable[0] = ((lveQueryColumn.Items[(int)ShenGlobal.qc.expression].SubItems[column].Text.Length != 0) && (lveQueryColumn.Items[(int)ShenGlobal.qc.value1].SubItems[column].Text.Length != 0));
				bubbPagesEnable[1] = bool.Parse(lveQueryColumn.Items[(int)ShenGlobal.qc.showField].SubItems[column].Text);

				ColumnPropertyDlg columnPropertyDlg = new ColumnPropertyDlg(tableName + "." + fieldName, property, bubbPagesEnable, oraConn);
				if ( columnPropertyDlg.ShowDialog(this) != DialogResult.OK )
					return;

				property = columnPropertyDlg.property;
				lveQueryColumn.Items[(int)ShenGlobal.qc.property].SubItems[column].Text = string.Join(sepProp, property);

				toolTipQueryColumn.SetToolTip(lveQueryColumn, null);
				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// カラムの順番が変更された
		/// </summary>
		private void lveQueryColumn_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			try
			{
#if false
				if ( e.NewDisplayIndex == 0 || e.OldDisplayIndex == 0 )
				{
					// OwnerDraw が true の時、移動後の０列目の _DrawSubItem での e.Bounds.X が 0 になったままなので、０列目は移動できないようにしておく
					e.Cancel = true;
				}
#else
				if ( /*e.NewDisplayIndex == 0 || */e.OldDisplayIndex == 0 )
				{
					// OwnerDraw が true の時、移動後の０列目の _DrawSubItem での e.Bounds.X が 0 になったままなので、０列目は移動できないようにしておく
					e.Cancel = true;
					return;
				}
				else if ( e.NewDisplayIndex == 0 )
				{
					int[] colOrder = lveQueryColumn.GetColumnOrder();
					int column = colOrder[e.OldDisplayIndex];
					int columnWidth = lveQueryColumn.Columns[column].Width;
					string tableName = lveQueryColumn.Columns[column].Text;
					string[] queryColumn = new string[lveQueryColumn.Items.Count];

					// 移動元のクエリー項目を削除する
					for ( int i = 0; i < lveQueryColumn.Items.Count; i++ )
					{
						ListViewItem lvi = lveQueryColumn.Items[i];
						queryColumn[i] = lvi.SubItems[column].Text;
						lvi.SubItems.RemoveAt(column);
					}
					lveQueryColumn.Columns.RemoveAt(column);

					// 移動先に挿入する
					column = colOrder[e.NewDisplayIndex];
					lveQueryColumn.Columns.Insert(column, tableName, columnWidth, HorizontalAlignment.Left);
					for ( int i = 0; i < queryColumn.Length; i++ )
					{
						ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lveQueryColumn.Items[i], queryColumn[i]);
						lveQueryColumn.Items[i].SubItems.Insert(column, subItem);
					}

					e.Cancel = true;
#if (DEBUG)
					// 移動元が最左端の場合、０，１，２列とあった時、（１）２を１に移動。（２）０を１と２の間に挿入すると表示が乱れる
					Debug.Write("new:" + e.NewDisplayIndex + " " + "old:" + e.OldDisplayIndex + " ");
					Debug.Write("order:");
					for ( int i = 0; i < colOrder.Length; i++ )
					{
						Debug.Write(colOrder[i] + ",");
					}
					Debug.Write("\r\n");
#endif
				}
#endif

				ChangeModified(true);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
			finally
			{
				toolTipQueryColumn.SetToolTip(lveQueryColumn, null);
				lastQueryColumn = -1;
			}
		}

		/// <summary>
		/// クエリー項目でマウスが移動した
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lveQueryColumn_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				ListViewEx.ListViewEx listView = (ListViewEx.ListViewEx)sender;
				if ( (listView.Columns.Count == 0) || (lveQueryColumn.GetItemAt(e.X, e.Y) == null) || (lveQueryColumn.Items[0].Bounds.Bottom < e.Y) )
				{
					toolTipQueryColumn.Active = false;
					return;
				}

				toolTipQueryColumn.Active = true;

				int[] colOrder = listView.GetColumnOrder();
				int n;
				for ( n = listView.Items[0].SubItems.Count - 1; (0 <= n) && (e.X < listView.Items[0].SubItems[colOrder[n]].Bounds.X); n-- ) ;
				if ( n == lastQueryColumn )
					return;

				string tableFieldName = listView.Columns[colOrder[n]].Text + "." + listView.Items[(int)ShenGlobal.qc.fieldName].SubItems[colOrder[n]].Text;
				string[] property = listView.Items[(int)ShenGlobal.qc.property].SubItems[colOrder[n]].Text.Split(sepProp[0]);

				StringBuilder toolTip = new StringBuilder(tableFieldName);
				toolTip.Append("\r\n" + property[(int)ShenGlobal.prop.type] + "(" + property[(int)ShenGlobal.prop.length] + ")");

				if ( property[(int)ShenGlobal.prop.comment] != ShenGlobal.propNoComment )
				{
					toolTip.Append("\r\n" + property[(int)ShenGlobal.prop.comment]);
				}

				if ( property[(int)ShenGlobal.prop.nullable] == propNotNullable )
				{
					toolTip.Append("\r\n" + "NOT NULL");
				}

				if ( property[(int)ShenGlobal.prop.alias].Length != 0 )
				{
					toolTip.Append("\r\n" + property[(int)ShenGlobal.prop.alias]);
				}

				if ( property[(int)ShenGlobal.prop.bubbles].Length != 0 )
				{
					toolTip.Append("\r\n" + "using bubbles property");
				}

				foreach ( ListViewItem lvi in lvTableJoin.Items )
				{
					if ( lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text == tableFieldName )
					{
						toolTip.Append("\r\n" + lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text + " " + lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text);
					}
					else if ( lvi.SubItems[(int)ShenGlobal.tabJoin.rightTabCol].Text == tableFieldName )
					{
						toolTip.Append("\r\n" + lvi.SubItems[(int)ShenGlobal.tabJoin.leftTabCol].Text + " " + lvi.SubItems[(int)ShenGlobal.tabJoin.way].Text);
					}
				}

				toolTipQueryColumn.SetToolTip(listView, toolTip.ToString());

				lastQueryColumn = n;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目のヘッダがクリックされた
		/// </summary>
		private void lveQueryColumn_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			try
			{
#if UPDATE_20140729
				string selTableOwner = string.Empty;
#endif
#if TABLE_NAME_HAS_ALIAS
				string selTableName = ShenGlobal.GetTableName(lveQueryColumn.Columns[e.Column].Text, true);
#else
				string selTableName = lveQueryColumn.Columns[e.Column].Text;
#endif
#if true
				if ( Program.expertMode && toolStripCustomTableSelect.Checked )
				{
					if ( putDiffOwnerToTable )	// オーナーが違うテーブル名に "OWNER." を付ける？
					{
						int index = selTableName.IndexOf('.');
						if ( index != -1 )
						{
#if UPDATE_20140729
							selTableOwner = selTableName.Substring(0, index);
#endif
							selTableName = selTableName.Substring(index + 1);
						}
#if UPDATE_20140729
						else
						{
							string[] logOn = toolStripStatusOraConn.Text.Split(oraConnStatusSplitter);
							selTableOwner = logOn[(int)logon.uid].Trim().ToUpper();
						}

						selTableOwner += ".";
#endif
					}
				}
#endif

#if false
				XmlNodeList tableList = xmlTableList.DocumentElement.ChildNodes;
				for ( int i = 0; i < tableList.Count; i++ )
				{
					if ( tableList[i].Attributes[attrName].Value != selTableName )
						continue;

					toolTipQueryColumn.Active = false;

#if TABLE_NAME_HAS_ALIAS
					// 一覧とクエリー項目のテーブルが違う（どちらかが別名を持っている）？
					//if ( GetListBoxTableName(i, selTbl.raw) != lveQueryColumn.Columns[e.Column].Text )
					string _listTableName = GetListBoxTableName(i, selTbl.raw);
					string _colTableName = lveQueryColumn.Columns[e.Column].Text;
					if ( (_listTableName.IndexOf(' ') != -1 || _colTableName.IndexOf(' ') != -1) && (_listTableName != _colTableName) )
					{
						//string owner = (tableList[i].Attributes[attrOwner].Value.Length == 0) ? "" : tableList[i].Attributes[attrOwner].Value + ".";
						string owner = string.Empty;
						if ( _colTableName.IndexOf('.') == -1 )
						{
							owner = (tableList[i].Attributes[attrOwner].Value.Length == 0) ? "" : tableList[i].Attributes[attrOwner].Value + ".";
						}
						/*string itemText = (string)listBoxTableList.Items[i];
						int indexTab = itemText.IndexOf('\t');
						string comment = (indexTab == -1) ? "" : itemText.Substring(indexTab);
						listBoxTableList.Items[i] = owner + lveQueryColumn.Columns[e.Column].Text + comment;*/
						EditListBoxTableName(i, owner + _colTableName/*lveQueryColumn.Columns[e.Column].Text*/);
					}
#endif

					if ( i != listBoxTableList.SelectedIndex )
					{
						listBoxTableList.SelectedIndex = i;	// 選択されたクエリ項目のテーブル名で一覧を選択状態にする
					}

					return;
				}
#else
				// テーブル名の絞り込みに対応するため、リストボックスから検索するようにした。(2011/08/08)
				for ( int i = 0; i < listBoxTableList.Items.Count; i++ )
				{
					string tableOwner = string.Empty;
					string tableName = listBoxTableList.Items[i].ToString().Split('\t')[0];
					int period, alias;
					if ( (period = tableName.IndexOf('.')) != -1 )	// オーナー付き？
					{
						tableOwner = tableName.Substring(0, period + 1);
						tableName = tableName.Substring(period + 1);
					}
					if ( (alias = tableName.IndexOf(' ')) != -1 )	// 別名付き？
					{
						tableName = tableName.Substring(0, alias);
					}

					if ( tableName != selTableName )
						continue;
#if UPDATE_20140729
					if ( !string.IsNullOrEmpty(selTableOwner) && (tableOwner != selTableOwner) )
						continue;
#endif

					toolTipQueryColumn.Active = false;

#if TABLE_NAME_HAS_ALIAS
					// 一覧とクエリー項目のテーブルが違う（どちらかが別名を持っている）？
					string _listTableName = GetListBoxTableName(i, selTbl.raw);
					string _colTableName = lveQueryColumn.Columns[e.Column].Text;
					if ( (_listTableName.IndexOf(' ') != -1 || _colTableName.IndexOf(' ') != -1) && (_listTableName != _colTableName) )
					{
						EditListBoxTableName(i, tableOwner + _colTableName);
					}
#endif

					if ( i != listBoxTableList.SelectedIndex )
					{
						listBoxTableList.SelectedIndex = i;	// 選択されたクエリ項目のテーブル名で一覧を選択状態にする
					}

					return;
				}
#endif
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目でマウスがクリックされた
		/// </summary>
		private void lveQueryColumn_MouseClick(object sender, MouseEventArgs e)
		{
			queryColumnLastMouseArgs = e;

			if ( reverseQueryColumn != -1 )
			{
				InvalidateQueryColumn(reverseQueryColumn);
				reverseQueryColumn = -1;
			}
		}

		/// <summary>
		/// control_SelectedValueChanged
		/// </summary>
		private void control_SelectedValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				lveQueryColumn.EndEditing(true);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目でサブアイテムがクリックされた
		/// </summary>
		private void lveQueryColumn_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
		{
			try
			{
				int minEditableItemIndex = editableColumnName ? -1 : 0;
				if ( (queryColumnLastMouseArgs.Button != MouseButtons.Left) || (e.Item.Index <= minEditableItemIndex)/* || (editors.Length <= e.Item.Index)*/ )
					return;

				/*if ( e.SubItem == 3 ) // Password field
				{
					// the current value (text) of the subitem is ****, so we have to provide
					// the control with the actual text (that's been saved in the item's Tag property)
					e.Item.SubItems[e.SubItem].Text = e.Item.Tag.ToString();
				}*/

				lveQueryColumn.StartEditing(editors[e.Item.Index], e.Item, e.SubItem);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目でサブアイテムの編集が終了した
		/// </summary>
		private void lveQueryColumn_SubItemEndEditing(object sender, ListViewEx.SubItemEndEditingEventArgs e)
		{
			try
			{
				if ( !e.Cancel )
				{
					if ( e.Item.SubItems[e.SubItem].Text != e.DisplayText )
					{
						ChangeModified(true);
					}

					if ( e.Item.Index == (int)ShenGlobal.qc.expression )
					{
						if ( e.DisplayText.Length != 0 )
						{
							if ( lveQueryColumn.Items[(int)ShenGlobal.qc.rColOp].SubItems[e.SubItem].Text.Length == 0 )
							{
								lveQueryColumn.Items[(int)ShenGlobal.qc.rColOp].SubItems[e.SubItem].Text = "AND";
							}
						}
						else
						{
							lveQueryColumn.Items[(int)ShenGlobal.qc.rColOp].SubItems[e.SubItem].Text = "";
						}
					}
				}

				/*if ( e.SubItem == 3 ) // Password field
				{
					if ( e.Cancel )
					{
						e.DisplayText = new string(textBoxPassword.PasswordChar, e.Item.Tag.ToString().Length);
					}
					else
					{
						// in order to display a series of asterisks instead of the plain password text
						// (textBox.Text _gives_ plain text, after all), we have to modify what'll get
						// displayed and save the plain value somewhere else.
						string plain = e.DisplayText;
						e.DisplayText = new string(textBoxPassword.PasswordChar, plain.Length);
						e.Item.Tag = plain;
					}
				}*/
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// ヘッダ用のオーナードロー
		/// </summary>
		private void lveQueryColumn_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			try
			{
				//Debug.WriteLine("head " + e.ColumnIndex + " " + e.Bounds);

				using ( StringFormat sf = new StringFormat() )
				{
					// Draw the standard header background.
					e.DrawBackground();

					if ( osPlatform == common.platform.win10 )
					{
						e.Graphics.FillRectangle(queryColumnHeaderBackColor, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
						//e.Graphics.DrawLine(new Pen(Color.Red, 1), new Point(e.Bounds.X + e.Bounds.Width - 1, e.Bounds.Y), new Point(e.Bounds.X + e.Bounds.Width - 1, e.Bounds.Y + e.Bounds.Height));
					}

#if true
					// Draw the header text.
					Rectangle rect = e.Bounds;
					//rect.Width = 30;
					rect.Height = qcFontHeight;

					if ( osPlatform == common.platform.win10 )
					{
						//sf.LineAlignment = StringAlignment.Center;
						rect.Y += 3;
					}

					sf.Alignment = StringAlignment.Center;
					sf.Trimming = StringTrimming.EllipsisCharacter;

					e.Graphics.DrawString(e.Header.Text, queryColumnFont, Brushes.DarkBlue, rect/*e.Bounds*/, sf);
#else
					e.DrawText();
#endif
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// サブアイテム用のオーナードロー
		/// </summary>
		private void lveQueryColumn_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			try
			{
				//Debug.WriteLine("sub " + e.ColumnIndex + " " + e.Bounds);
#if true
				string text = e.SubItem.Text;

				/*if ( (int)contextMenuQueryColumn.Tag != -1 )	// コンテキストメニューが表示されている？
				{
					int[] colOrder = lveQueryColumn.GetColumnOrder();
					if ( colOrder[(int)contextMenuQueryColumn.Tag] == e.ColumnIndex )
					{
						e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
						//Debug.WriteLine(text + " FillRectangle:" + e.Bounds);
					}
				}*/
				if ( reverseQueryColumn != -1 )	// 反転指定のサブアイテムがある？
				{
					int[] colOrder = lveQueryColumn.GetColumnOrder();
					if ( (colOrder[reverseQueryColumn] == e.ColumnIndex) && (e.ItemIndex < lveQueryColumn.ValidItemCount) )
					{
						e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
						//Debug.WriteLine(text + " FillRectangle:" + e.Bounds);
					}
				}

				/*if ( e.ItemIndex == (int)ShenCore.qc.fieldName )
				{
					Rectangle rect = e.Bounds;
					rect.X++;
					rect.Y++;
					rect.Width -= 2;
					rect.Height -= 3;
					e.Graphics.FillRectangle(Brushes.LightGray, rect);
				}*/
				if ( e.ItemIndex == (int)ShenGlobal.qc.showField )
				{
					/*Debug.WriteLine("checkShowField.Visible:" + checkShowField.Visible);
					Debug.WriteLine("EditSubItem:" + lveQueryColumn.EditSubItem);
					Debug.WriteLine("e.ColumnIndex:" + e.ColumnIndex);*/
					if ( (lveQueryColumn.EditSubItem == e.ColumnIndex)/* && checkShowField.Visible*/ )
						return;
					int image = bool.Parse(text) ? 1 : 0;
					e.Graphics.DrawImage(imageCheckBox.Images[image], e.Bounds.X + (e.Bounds.Width / 2) - (13 / 2), e.Bounds.Y + (e.Bounds.Height / 2) - (13 / 2));
					return;
				}
				else if ( e.ItemIndex == (int)ShenGlobal.qc.property )
				{
#if false
					//e.Graphics.DrawLine(queryColumnPens[0], e.Bounds.X + e.Bounds.Width, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
					//e.Graphics.DrawLine(queryColumnPens[0], e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
					Pen pen;
					try
					{
						int tableIndex = queryTableNames.IndexOf(e.Header.Text);
						pen = queryColumnPens[tableIndex % queryColumnPens.Length];	// テーブル毎に下線の色を変える
					}
					catch ( Exception exp )
					{
						pen = queryColumnPens[0];
						Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
					}
					e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
#else
					Brush brush;
					try
					{
						int tableIndex = queryTableNames.IndexOf(e.Header.Text);
						brush = queryColumnBrushes[tableIndex % queryColumnBrushes.Length];
					}
					catch ( Exception exp )
					{
						brush = queryColumnBrushes[0];
						Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
					}
					// テーブル毎に最終アイテムの背景色を変える
					//e.Graphics.FillRectangle(brush, e.Bounds);
					e.Graphics.FillRectangle(brush, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, 2);
#endif
					return;
				}

				Rectangle rect = e.Bounds;
				rect.Height = qcFontHeight;

				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				//StringFormat sf = StringFormat.GenericDefault;

				e.Graphics.DrawString(text/*e.SubItem.Text*/, queryColumnFont, ((e.ItemIndex == (int)ShenGlobal.qc.fieldName) ? Brushes.DarkBlue : Brushes.Black), rect/*e.Bounds*/, sf);

				sf.Dispose();
#else
				e.DrawText();
#endif
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}

		/// <summary>
		/// クエリー項目の横幅を伸縮する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkStretchColumnWidth_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				int width = checkStretchColumnWidth.Checked ? narColumnWidth : defColumnWidth;

				foreach ( ColumnHeader column in lveQueryColumn.Columns )
				{
					column.Width = width;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
		#endregion

		#region MyMessageBox クラス
		/// <summary>
		/// MyMessageBox
		/// </summary>
		public class MyMessageBox
		{
			static public Form _mainForm = null;

			static private System.Threading.Timer timerShowMessageBox = null;
			static private TimerCallback timerDelegateShowMessageBox = new TimerCallback(OnTimerShowMessageBox);

			/// <summary>
			/// timerState
			/// </summary>
			private struct timerState
			{
				public IntPtr hWndParent;
				public string caption;
			}

			/// <summary>
			/// MessageBox を親ウィンドウの中央に表示する
			/// </summary>
			/// <param name="text"></param>
			/// <param name="caption"></param>
			/// <param name="buttons"></param>
			/// <param name="icon"></param>
			/// <returns></returns>
			static public DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
			{
				try
				{
					if ( _mainForm != null )
					{
						timerState state;
						state.hWndParent = _mainForm.Handle;
						state.caption = caption;

						timerShowMessageBox = new System.Threading.Timer(timerDelegateShowMessageBox, state, Timeout.Infinite, 0);
						timerShowMessageBox.Change(100, Timeout.Infinite);	// SetTimer（周期的なシグナル通知は無効）

						bool b = api.LockWindowUpdate(api.GetDesktopWindow());
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}

				return MessageBox.Show(text, caption, buttons, icon);
			}

			delegate void CallLockWindowUpdateDelegate(IntPtr hwnd);

			/// <summary>
			/// CallLockWinodwUpdate
			/// </summary>
			static private void CallLockWindowUpdate(IntPtr hwnd)
			{
				if ( !api.LockWindowUpdate(hwnd) )
				{
					int errorNo = Marshal.GetLastWin32Error();
				}
			}

			/// <summary>
			/// OnTimerShowMessageBox
			/// </summary>
			/// <param name="state"></param>
			static private void OnTimerShowMessageBox(Object state)
			{
				try
				{
					IntPtr hMessageBox = api.FindWindow(null, ((timerState)state).caption);
					if ( hMessageBox != null )
					{
						api.RECT rectMessageBox, rectParent;
						api.GetWindowRect(hMessageBox, out rectMessageBox);
						api.GetWindowRect(((timerState)state).hWndParent, out rectParent);

						int x = rectParent.Left + ((rectParent.Right - rectParent.Left) - (rectMessageBox.Right - rectMessageBox.Left)) / 2;
						int y = rectParent.Top + ((rectParent.Bottom - rectParent.Top) - (rectMessageBox.Bottom - rectMessageBox.Top)) / 2;

						api.SetWindowPos(hMessageBox, (IntPtr)api.HWND_TOP, x, y, 0, 0, api.SWP_NOZORDER | api.SWP_NOSIZE);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}
				finally
				{
					_mainForm.Invoke(new CallLockWindowUpdateDelegate(CallLockWindowUpdate), new Object[] { IntPtr.Zero });
					timerShowMessageBox.Dispose();
				}
			}
		}
		#endregion
    }
}