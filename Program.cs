using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using CommonFunctions;
using ProgUpdateClass;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	static class Program
	{
		//private const string CMDPARAM_ENABLE_MULTIPLE_INSTANCES = "/EnableMultipleInstances";
		private const string CMDPARAM_ORA_MIDDLEWARE = "/OraMiddleware:";
		public const string CMDPARAM_DEBMODE = "/DebMode";
		public const string CMDPARAM_EXPERT_MODE = "/ExpertMode";			// エキスパート モードで起動する
		public const string CMDPARAM_NEW_INSTANCE = "/NewInstance";			// 既存のインスタンスがある場合、新しいインスタンスで起動する

		private static bool showProgUpdateMessage = false;
		//private static bool enableMultipleInstances = false;
		private static bool cmdParamOraMiddleware = false;
		public static bool debMode = false;
		public static bool expertMode = false;
		private static bool newInstance = false;
		public static string cmdParamShenlongColumnFileName = null;

		// <App.config>
		private const string CONSET_LATEST_PROGRAM_FOLDER = "LatestProgramFolder";
		private const string CONSET_URL_MAIL_TO_DEVELOPER = "UrlMailToDeveloper";
		public const string CONSET_RELOAD_LAST_COLUMNS_ON_STARTUP = "ReloadLastColumnsOnStartup";
		public const string CONSET_SELECT_COLUMN_BY_DRAG_DROP = "SelectColumnByDragDrop";
		public const string CONSET_SHOW_SYNONYM_OWNER = "ShowSynonymOwner";
		public const string CONSET_TABLE_SELECTED_ACTION = "TableSelectedAction";
		public const string CONSET_EDITABLE_COLUMN_NAME = "EditableColumnName";
		public const string CONSET_SQL_DATE_FORMAT = "SqlDateFormat";
		public const string CONSET_MULTI_INSTANCE_ENABLED = "MultiInstanceEnabled";
		public const string CONSET_PASTE_COLUMN_COMMENTS = "PasteColumnComments";
		public const string CONSET_SAVE_QUERY_OUTPUT_FILE = "SaveQueryOutputFile";
		public const string CONSET_TEXT_QUERY_OUTPUT_FILE_NAME = "TextQueryOutputFileName";
		public const string CONSET_PASTE_QUERY_RESULT_TO_EXCEL = "PasteQueryResultToExcel";
		public const string CONSET_ORA_MIDDLEWARE = "OraMiddleware";
		public const string CONSET_SHOW_PARAM_INPUT_DLG = "ShowParamInputDlg";
		public const string CONSET_RELOAD_PREV_FIELD_ON_STARTUP = "ReloadPrevFieldOnStartup";	/* 未使用 */
		public const string CONSET_SELECT_FIELD_BY_DRAG_DROP = "SelectFieldByDragDrop";			/* 未使用 */
		public const string CONSET_CLEAR_COLUMN_BY_SELTBL = "ClearColumnBySelTbl";				/* 未使用 */
		// </App.config>

		public static bool multiInstanceEnabled = false;

		//public const string CONFIG_YES = "yes";
		//public const string CONFIG_NO = "no";

		private static string messageApplicationError = null;
		public static bool isNewInstance = true;							// 最初に起動されたインスタンスか否か

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Shenlong());

			try
			{
				// コマンド ライン パラメータ
				string[] cmdParam = Environment.GetCommandLineArgs();
				for ( int i = 0; i < cmdParam.Length; i++ )
				{
					if ( cmdParam[i] == update.CMDPARAM_SHOW_PROG_UPDATE_MESSAGE )
					{
						showProgUpdateMessage = true;
					}
					/*else if ( cmdParam[i] == CMDPARAM_ENABLE_MULTIPLE_INSTANCES )
					{
						enableMultipleInstances = true;
					}*/
					else if ( cmdParam[i].StartsWith(CMDPARAM_ORA_MIDDLEWARE) )
					{
						cmdParamOraMiddleware = true;
						string oraMiddleware = cmdParam[i].Substring(CMDPARAM_ORA_MIDDLEWARE.Length);
						if ( string.Compare(oraMiddleware, Shenlong.omw.OracleClient.ToString(), true) == 0 )
							Shenlong.oraMiddleware = Shenlong.omw.OracleClient;
						else if ( string.Compare(oraMiddleware, Shenlong.omw.OleDb.ToString(), true) == 0 )
							Shenlong.oraMiddleware = Shenlong.omw.OleDb;
						else if ( string.Compare(oraMiddleware, Shenlong.omw.oo4o.ToString(), true) == 0 )
							Shenlong.oraMiddleware = Shenlong.omw.oo4o;
						else
							cmdParamOraMiddleware = false;
					}
					else if ( cmdParam[i] == CMDPARAM_DEBMODE )
					{
						debMode = true;
					}
					else if ( cmdParam[i] == CMDPARAM_EXPERT_MODE )
					{
						expertMode = true;
					}
					else if ( cmdParam[i] == CMDPARAM_NEW_INSTANCE )
					{
						newInstance = true;
					}
					else if ( cmdParam[i].EndsWith(".xml") )
					{
						if ( System.IO.File.Exists(cmdParam[i]) )
						{
							cmdParamShenlongColumnFileName = cmdParam[i];
						}
					}
				}

				if ( !GetAppConfig() )
				{
					MessageBox.Show(messageApplicationError, "APPLICATION CONFIG ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// ウィンドウの位置とサイズ
				Shenlong.GetWindowRectangle();

				if ( multiInstanceEnabled/*enableMultipleInstances*/ || newInstance )	// 多重起動が許可されている？
				{
					if ( api.FindWindow(null, Shenlong.appTitle) == IntPtr.Zero )	// 最初の起動？
					{
						if ( ProgramUpdateRelaunch() )
							return;
					}
					else
					{
						isNewInstance = false;
					}

					Application.Run(new Shenlong());
				}
				else
				{
					// ミューテックスを確認して、アプリケーションを起動する
					Mutex m = new Mutex(false, Shenlong.appTitle);

					if ( m.WaitOne(0, false) )
					{
						if ( ProgramUpdateRelaunch() )
						{
							m.ReleaseMutex();
							m.Close();
							return;
						}

						Application.Run(new Shenlong());
						m.ReleaseMutex();
					}
					else
					{
						// bring old instance to the front
						IntPtr hwnd = api.FindWindow(null, Shenlong.appTitle);
						if ( hwnd != IntPtr.Zero )
						{
							if ( api.IsIconic(hwnd) )
								api.ShowWindow(hwnd, api.SW_SHOWNOACTIVATE);
							api.SetForegroundWindow(hwnd);
						}

						if ( cmdParamShenlongColumnFileName != null )
						{
							api.COPYDATASTRUCT cds = new api.COPYDATASTRUCT();
							cds.dwData = IntPtr.Zero;
#if true
							byte[] fnameBuff = System.Text.Encoding.Unicode.GetBytes(cmdParamShenlongColumnFileName);
							//Array.Resize(ref fnameBuff, fnameBuff.Length + 2); // 受け側が PtrToStringUni の時
							cds.lpData = Marshal.AllocHGlobal(fnameBuff.Length);
							cds.cbData = fnameBuff.Length;
							Marshal.Copy(fnameBuff, 0, cds.lpData, fnameBuff.Length);
							IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cds));
							Marshal.StructureToPtr(cds, lParam, true);
							api.SendMessage(hwnd, api.WM_COPYDATA, (uint)IntPtr.Zero, (uint)lParam);
							Marshal.FreeCoTaskMem(lParam);
#else
							cds.lpData = Marshal.StringToHGlobalUni(cmdParamShenlongColumnFileName);
							cds.cbData = (cmdParamShenlongColumnFileName.Length + 1) * 2;
							//cds.cbData = System.Text.Encoding.Unicode.GetByteCount(cmdParamShenlongColumnFileName) + 2;
							api.SendMessageCds(hwnd, api.WM_COPYDATA, IntPtr.Zero, ref cds);
#endif
							Marshal.FreeHGlobal(cds.lpData);
						}
					}

					// アプリケーションが終わるまでmへの参照を維持するようにする
					GC.KeepAlive(m);
					m.Close();
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// アプリケーション設定ファイルを読み込む
		/// </summary>
		private static bool GetAppConfig()
		{
			try
			{
				//string[] returnedStrings;
				string returnedString;

				// 最新のプログラム フォルダを置いているフォルダ名
				Shenlong.latestProgramFolder = ConfigurationManager.AppSettings[CONSET_LATEST_PROGRAM_FOLDER];
				if ( Shenlong.latestProgramFolder.Length != 0 && !Shenlong.latestProgramFolder.EndsWith("\\") )
					Shenlong.latestProgramFolder += "\\";

				// 問い合わせ先の url
				Shenlong.urlMailToDeveloper = ConfigurationManager.AppSettings[CONSET_URL_MAIL_TO_DEVELOPER];

				// 起動時に前回の状態を読み込む
				//Shenlong.reloadLastColumnsOnStartup = (ConfigurationManager.AppSettings[CONSET_RELOAD_LAST_COLUMNS_ON_STARTUP] == CONFIG_YES);
				Shenlong.reloadLastColumnsOnStartup = GetAppSettings(CONSET_RELOAD_LAST_COLUMNS_ON_STARTUP);

				// ドラッグ＆ドロップでカラムを選択する
				//Shenlong.selectColumnByDragDrop = (ConfigurationManager.AppSettings[CONSET_SELECT_COLUMN_BY_DRAG_DROP] == CONFIG_YES);
				Shenlong.selectColumnByDragDrop = GetAppSettings(CONSET_SELECT_COLUMN_BY_DRAG_DROP);

				// シノニムの前にオーナーを表示する
				//Shenlong.showSynonymOwner = (ConfigurationManager.AppSettings[CONSET_SHOW_SYNONYM_OWNER] == CONFIG_YES);
				Shenlong.showSynonymOwner = GetAppSettings(CONSET_SHOW_SYNONYM_OWNER);

				//// テーブルを選択する度にクエリー項目をクリアする
				//Shenlong.clearColumnBySelTbl = (ConfigurationManager.AppSettings[CONSET_CLEAR_COLUMN_BY_SELTBL] == CONFIG_YES);
				// テーブルが選択された時の処理
				Shenlong.tableSelectedAction = int.Parse(ConfigurationManager.AppSettings[CONSET_TABLE_SELECTED_ACTION]);

				// 項目名の編集を許可する
				//Shenlong.editableColumnName = (ConfigurationManager.AppSettings[CONSET_EDITABLE_COLUMN_NAME] == CONFIG_YES);
				Shenlong.editableColumnName = GetAppSettings(CONSET_EDITABLE_COLUMN_NAME);

				// SQL 日付の条件書式
				ShenGlobal.sqlDateFormat = ConfigurationManager.AppSettings[CONSET_SQL_DATE_FORMAT];

				// 多重起動を許可する
				multiInstanceEnabled = bool.Parse(ConfigurationManager.AppSettings[CONSET_MULTI_INSTANCE_ENABLED]);

				// クエリーの出力結果に項目のコメントも貼り付ける
				//Shenlong.pasteColumnComments = (ConfigurationManager.AppSettings[CONSET_PASTE_COLUMN_COMMENTS] == CONFIG_YES);
				Shenlong.pasteColumnComments = GetAppSettings(CONSET_PASTE_COLUMN_COMMENTS);

				// クエリーの出力結果をファイルに保存する
				//Shenlong.saveQueryOutputFile = (ConfigurationManager.AppSettings[CONSET_SAVE_QUERY_OUTPUT_FILE] == CONFIG_YES);
				Shenlong.saveQueryOutputFile = GetAppSettings(CONSET_SAVE_QUERY_OUTPUT_FILE);

				// クエリー出力結果のファイル名
				Shenlong.textQueryOutputFileName = ConfigurationManager.AppSettings[CONSET_TEXT_QUERY_OUTPUT_FILE_NAME];

				// クエリーの出力結果を Excel に貼り付ける対象
				Shenlong.pasteQueryResultToExcel = (Shenlong.pasteExcel)int.Parse(ConfigurationManager.AppSettings[CONSET_PASTE_QUERY_RESULT_TO_EXCEL]);

				// オラクルに接続する方法
				if ( !cmdParamOraMiddleware )
				{
					returnedString = ConfigurationManager.AppSettings[CONSET_ORA_MIDDLEWARE];
					if ( string.Compare(returnedString, Shenlong.omw.OracleClient.ToString()) == 0 )
						Shenlong.oraMiddleware = Shenlong.omw.OracleClient;
					else if ( string.Compare(returnedString, Shenlong.omw.OleDb.ToString()) == 0 )
						Shenlong.oraMiddleware = Shenlong.omw.OleDb;
					else if ( string.Compare(returnedString, Shenlong.omw.oo4o.ToString()) == 0 )
						Shenlong.oraMiddleware = Shenlong.omw.oo4o;
					else
						Shenlong.oraMiddleware = Shenlong.omw.OleDb;
				}

				// クエリー前に抽出条件入力ダイアログを表示する
				Shenlong.showParamInputDlg = bool.Parse(ConfigurationManager.AppSettings[CONSET_SHOW_PARAM_INPUT_DLG]);
			}
			catch ( Exception exp )
			{
				messageApplicationError = exp.Message;
				return false;
			}

			return true;
		}

		/// <summary>
		/// App.config の yes|no を bool 型に変換する
		/// </summary>
		/// <param name="keyName"></param>
		/// <returns></returns>
		private static bool GetAppSettings(string keyName)
		{
			string returnedString = ConfigurationManager.AppSettings[keyName];
			returnedString = (returnedString == "yes") ? true.ToString() : (returnedString == "no" ? false.ToString() : returnedString);
			return bool.Parse(returnedString);
		}

		/// <summary>
		/// プログラムの更新版があれば、更新して再起動する
		/// </summary>
		/// <returns></returns>
		private static bool ProgramUpdateRelaunch()
		{
			string[] appProductNames = { Application.ProductName + ".exe", "shenlong.chm"/*, "Microsoft.Office.Interop.Excel.dll"*/ };
			// overWriteConfigKeys オプション有り
			string[,] configKeys = { {CONSET_LATEST_PROGRAM_FOLDER, string.Empty},
									 {CONSET_URL_MAIL_TO_DEVELOPER, string.Empty}/*,
									 {CONSET_RELOAD_PREV_FIELD_ON_STARTUP, null},
									 {CONSET_SELECT_FIELD_BY_DRAG_DROP, null},
									 {CONSET_CLEAR_COLUMN_BY_SELTBL, null}*/ };
			/*// overWriteConfigKeys オプション無し
			string[,] configKeys = { {CONSET_RELOAD_LAST_COLUMNS_ON_STARTUP, (Shenlong.reloadLastColumnsOnStartup) ? CONFIG_YES : CONFIG_NO},
									 {CONSET_SELECT_COLUMN_BY_DRAG_DROP, (Shenlong.selectColumnByDragDrop)? CONFIG_YES : CONFIG_NO},
									 {CONSET_SHOW_SYNONYM_OWNER, (Shenlong.showSynonymOwner)? CONFIG_YES: CONFIG_NO},
									 {CONSET_CLEAR_COLUMN_BY_SELTBL, (Shenlong.clearColumnBySelTbl)? CONFIG_YES: CONFIG_NO},
									 {CONSET_EDITABLE_COLUMN_NAME, (Shenlong.editableColumnName)? CONFIG_YES: CONFIG_NO},
									 {CONSET_PASTE_COLUMN_COMMENTS, (Shenlong.pasteColumnComments)? CONFIG_YES: CONFIG_NO},
									 {CONSET_SAVE_QUERY_OUTPUT_FILE, (Shenlong.saveQueryOutputFile)? CONFIG_YES: CONFIG_NO},
									 {CONSET_TEXT_QUERY_OUTPUT_FILE_NAME, Shenlong.textQueryOutputFileName},
									 {CONSET_PASTE_QUERY_RESULT_TO_EXCEL, Shenlong.pasteQueryResultToExcel.ToString()} };*/

			uint option = (uint)update.options.overWriteConfigKeys/*0*/;
			option |= (showProgUpdateMessage) ? (uint)update.options.showProgUpdateMessage : 0;

			bool completedProgramUpdate = update.CheckProgramUpdate(Shenlong.latestProgramFolder, appProductNames, configKeys, Shenlong.windowRectangle, option);
			if ( completedProgramUpdate )
			{
				if ( update.RelaunchExe(appProductNames, Shenlong.appTitle, null, null, option) )
					return true;
			}

			return false;
		}
	}
}