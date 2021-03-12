using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using CommonFunctions;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class OptionDlg : Form
	{
		private const string defaultTextQueryOutputFileName = @".\~QueryOutput.txt";

		private string shenlongIniFileName = Shenlong.shenlongIniFileName;

		/// <summary>
		/// OptionDlg
		/// </summary>
		public OptionDlg()
		{
			InitializeComponent();

			if ( Program.expertMode )
			{
				if ( !Program.debMode )
				{
					checkWriteAccessLog.Visible = false;
					checkLogOnPwdToolTip.Visible = false;
				}
			}
			else
			{
				tabControl.TabPages.Remove(tabPageExpertSettings);
			}
		}

		/// <summary>
		/// OptionDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OptionDlg_Load(object sender, EventArgs e)
		{
			try
			{
				IntPtr sysMenuHandle = api.GetSystemMenu(this.Handle, false);
				int sysMenuItemCount = api.GetMenuItemCount(sysMenuHandle);
				for ( int i = (sysMenuItemCount - 2) - 1; 0 <= i; i-- )	// -2:[����][�Z�p���[�^]
				{
					if ( i == 1 )	// �ړ��H
						continue;
					api.RemoveMenu(sysMenuHandle, (uint)i, api.MF_BYPOSITION);
				}

				tabPageExpertSettings.ToolTipText = Shenlong.SETTINGS_SECTION + "@" + Shenlong.SHENLONG_INI_FILE_NAME;

				StringBuilder returnedString = new StringBuilder(1024);

				checkReloadLastColumnsOnStartup.Checked = Shenlong.reloadLastColumnsOnStartup;

				checkSelectColumnByDragDrop.Checked = Shenlong.selectColumnByDragDrop;

				checkShowSynonymOwner.Checked = Shenlong.showSynonymOwner;

				switch ( Shenlong.tableSelectedAction )
				{
					case (int)Shenlong.tableSelAct.showColumns:
						radioShowColumns.Checked = true; break;
					case (int)Shenlong.tableSelAct.clearSelectedColumns:
						radioClearSelectedColumns.Checked = true; break;
					case (int)Shenlong.tableSelAct.appendAllColumns:
						radioAppendAllColumns.Checked = true; break;
				}

				checkEditableColumnName.Checked = Shenlong.editableColumnName;

				comboSqlDateFormat.Items.Add(ShenGlobal.sqlDateFormat);
				comboSqlDateFormat.SelectedIndex = 0;

				// SQL ���t�̏�������
				string defaultComboSqlDateFormat = "yyyy/mm/dd hh24:mi|yyyy/mm/dd";
				api.GetPrivateProfileString(Shenlong.SETTINGS_SECTION, Shenlong.KEY_COMBO_SQL_DATE_FORMAT, defaultComboSqlDateFormat, returnedString, (uint)returnedString.Capacity, Shenlong.shenlongIniFileName);
				string[] sqlDateFormats = returnedString.ToString().Split('|');
				foreach ( string format in sqlDateFormats )
				{
					if ( format == ShenGlobal.sqlDateFormat )
						continue;
					comboSqlDateFormat.Items.Add(format);
				}

				checkMultiInstanceEnabled.Checked = Program.multiInstanceEnabled;

				checkClearQueryColumnWhenOraLogOn.Checked = Shenlong.clearQueryColumnWhenOraLogOn;
				checkClearQueryColumnWhenOraLogOn.Visible = Shenlong.selectableClearColumnLogOn;

				checkPasteColumnComments.Checked = Shenlong.pasteColumnComments;

				checkSaveQueryOutputFile.Checked = Shenlong.saveQueryOutputFile;
				textQueryOutputFileName.Text = Shenlong.textQueryOutputFileName;

				switch ( Shenlong.pasteQueryResultToExcel )
				{
					case Shenlong.pasteExcel.none:
						radioExcelPasteNone.Checked = true; break;
					case Shenlong.pasteExcel.newBookActSheet:
						radioExcelPasteNewBookActSheet.Checked = true; break;
					case Shenlong.pasteExcel.actBookActSheet:
						radioExcelPasteActBookActSheet.Checked = true; break;
					case Shenlong.pasteExcel.actBookNewSheet:
						radioExcelPasteActBookNewSheet.Checked = true; break;
					case Shenlong.pasteExcel.shenBookNewSheet:
						radioExcelPasteShenBookNewSheet.Checked = true; break;
				}

				radioExcelPasteNone.Enabled = Shenlong.enableExcelPasteNone;

				comboOraMiddleware.Items.Add(Shenlong.omw.OracleClient.ToString());
				comboOraMiddleware.Items.Add(Shenlong.omw.OleDb.ToString());
				//comboOracleAccess.Items.Add(Shenlong.omw.oo4o.ToString());
				comboOraMiddleware.SelectedIndex = (int)Shenlong.oraMiddleware;

				//checkShowParamInputDlg.Checked = Shenlong.showParamInputDlg;

				// Settings@shenlong.ini
				ReadExpertSettings();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}
		}

		/// <summary>
		/// buttonSelectQueryOutputFile_Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSelectQueryOutputFile_Click(object sender, EventArgs e)
		{
			try
			{
				//saveFileDialog.Reset();	// �������Ȃ��ƑO��I�������f�B���N�g�����L���ɂȂ��Ă��܂��H
				saveFileDialog.RestoreDirectory = true;
				saveFileDialog.InitialDirectory = Path.GetDirectoryName(textQueryOutputFileName.Text);
				saveFileDialog.FileName = Path.GetFileName(textQueryOutputFileName.Text);
				if ( saveFileDialog.ShowDialog(this) != DialogResult.OK )
					return;

				textQueryOutputFileName.Text = saveFileDialog.FileName;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// buttonOK_Click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
				Shenlong.reloadLastColumnsOnStartup = checkReloadLastColumnsOnStartup.Checked;

				//Shenlong.selectColumnByDragDrop = checkSelectColumnByDragDrop.Checked;

				Shenlong.showSynonymOwner = checkShowSynonymOwner.Checked;

				if ( radioShowColumns.Checked )
					Shenlong.tableSelectedAction = (int)Shenlong.tableSelAct.showColumns;
				else if ( radioClearSelectedColumns.Checked )
					Shenlong.tableSelectedAction = (int)Shenlong.tableSelAct.clearSelectedColumns;
				else if ( radioAppendAllColumns.Checked )
					Shenlong.tableSelectedAction = (int)Shenlong.tableSelAct.appendAllColumns;

				Shenlong.editableColumnName = checkEditableColumnName.Checked;

				ShenGlobal.sqlDateFormat = comboSqlDateFormat.Text;

				Program.multiInstanceEnabled = checkMultiInstanceEnabled.Checked;

				Shenlong.clearQueryColumnWhenOraLogOn = checkClearQueryColumnWhenOraLogOn.Checked;

				Shenlong.pasteColumnComments = checkPasteColumnComments.Checked;

				Shenlong.saveQueryOutputFile = checkSaveQueryOutputFile.Checked;
				if ( !File.Exists(textQueryOutputFileName.Text) )
				{
					textQueryOutputFileName.Text = defaultTextQueryOutputFileName;
				}
				Shenlong.textQueryOutputFileName = textQueryOutputFileName.Text;

				if ( radioExcelPasteNone.Checked )
					Shenlong.pasteQueryResultToExcel = Shenlong.pasteExcel.none;
				else if ( radioExcelPasteNewBookActSheet.Checked )
					Shenlong.pasteQueryResultToExcel = Shenlong.pasteExcel.newBookActSheet;
				else if ( radioExcelPasteActBookActSheet.Checked )
					Shenlong.pasteQueryResultToExcel = Shenlong.pasteExcel.actBookActSheet;
				else if ( radioExcelPasteActBookNewSheet.Checked )
					Shenlong.pasteQueryResultToExcel = Shenlong.pasteExcel.actBookNewSheet;
				else if ( radioExcelPasteShenBookNewSheet.Checked )
					Shenlong.pasteQueryResultToExcel = Shenlong.pasteExcel.shenBookNewSheet;

				if ( comboOraMiddleware.Text == Shenlong.omw.OracleClient.ToString() )
					Shenlong.oraMiddleware = Shenlong.omw.OracleClient;
				else if ( comboOraMiddleware.Text == Shenlong.omw.OleDb.ToString() )
					Shenlong.oraMiddleware = Shenlong.omw.OleDb;
				else if ( comboOraMiddleware.Text == Shenlong.omw.oo4o.ToString() )
					Shenlong.oraMiddleware = Shenlong.omw.oo4o;

				//Shenlong.showParamInputDlg = checkShowParamInputDlg.Checked;

				string appName = Process.GetCurrentProcess().ProcessName;
#if (DEBUG)
				appName = Shenlong.appTitle;
#endif
				File.Copy(Application.StartupPath + "\\" + appName + ".exe.config", Application.StartupPath + "\\" + appName + ".exe.config.bak", true);

				AppConfig appConfig = new AppConfig(appName);
				appConfig.SetValue(Program.CONSET_RELOAD_LAST_COLUMNS_ON_STARTUP, checkReloadLastColumnsOnStartup.Checked.ToString().ToLower()/*(checkReloadLastColumnsOnStartup.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_SELECT_COLUMN_BY_DRAG_DROP, checkSelectColumnByDragDrop.Checked.ToString().ToLower()/*(checkSelectColumnByDragDrop.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_SHOW_SYNONYM_OWNER, checkShowSynonymOwner.Checked.ToString().ToLower()/*(checkShowSynonymOwner.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_TABLE_SELECTED_ACTION, ((int)Shenlong.tableSelectedAction).ToString());
				appConfig.SetValue(Program.CONSET_EDITABLE_COLUMN_NAME, checkEditableColumnName.Checked.ToString().ToLower()/*(checkEditableColumnName.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_SQL_DATE_FORMAT, comboSqlDateFormat.Text);
				appConfig.SetValue(Program.CONSET_MULTI_INSTANCE_ENABLED, checkMultiInstanceEnabled.Checked.ToString().ToLower());
				appConfig.SetValue(Program.CONSET_PASTE_COLUMN_COMMENTS, checkPasteColumnComments.Checked.ToString().ToLower()/*(checkPasteColumnComments.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_SAVE_QUERY_OUTPUT_FILE, checkSaveQueryOutputFile.Checked.ToString().ToLower()/*(checkSaveQueryOutputFile.Checked) ? Program.CONFIG_YES : Program.CONFIG_NO*/);
				appConfig.SetValue(Program.CONSET_TEXT_QUERY_OUTPUT_FILE_NAME, textQueryOutputFileName.Text);
				appConfig.SetValue(Program.CONSET_PASTE_QUERY_RESULT_TO_EXCEL, ((int)Shenlong.pasteQueryResultToExcel).ToString());
				appConfig.SetValue(Program.CONSET_ORA_MIDDLEWARE, Shenlong.oraMiddleware.ToString());
				//appConfig.SetValue(Program.CONSET_SHOW_PARAM_INPUT_DLG, checkShowParamInputDlg.Checked.ToString().ToLower());

				// SQL ���t�̏�������
				comboSqlDateFormat.Items.Remove(comboSqlDateFormat.Text);
				string[] sqlDateFormats = new string[Math.Min(comboSqlDateFormat.Items.Count, 8 - 1)];
				for ( int i = 0; i < sqlDateFormats.Length; i++ )
				{
					sqlDateFormats[i] = (string)comboSqlDateFormat.Items[i];
				}
				api.WritePrivateProfileString(Shenlong.SETTINGS_SECTION, Shenlong.KEY_COMBO_SQL_DATE_FORMAT, string.Join("|", sqlDateFormats), Shenlong.shenlongIniFileName);

				// Settings@shenlong.ini
				SaveExpertSettings();

				this.Close();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#region Settings@shenlong.ini
		/// <summary>
		/// �g���ݒ��ǂݍ���
		/// �e�ϐ��̃f�t�H���g���{���̃f�t�H���g�ƕs��v���Ȃ��悤�ɒ��ӂ��鎖
		/// </summary>
		private void ReadExpertSettings()
		{
			try
			{
				if ( !Program.expertMode )
					return;

				// ���O�I�������ɒǉ������Ԃ̕����ݒ�
				SetExpertSettingsCheckBox(Shenlong.KEY_RESUME_APPEND_LOGON_HIS, null);

				// ���O�I����������Ő؂�ւ���ݒ�
				SetExpertSettingsCheckBox(Shenlong.KEY_AUTO_CHANGE_LOGON, false);

				// ���O�I�����ɃN�G�����ڂ��N���A���邩�ۂ���I���ł���ݒ�
				SetExpertSettingsCheckBox(Shenlong.KEY_SELECTABLE_CLEAR_COLUMN_LOGON, false);

				// ���O�I�������̍ő吔
				SetExpertSettingsTextBox(Shenlong.KEY_MAX_LOGON_HISTORY_COUNT, " " + "16");

				// �N�G���[���ڂ̍ő吔
				SetExpertSettingsTextBox(Shenlong.KEY_MAX_QUERY_COLUMN_COUNT, " " + "256");

				// �N�G���[���ڂ𔽓]�\�����鎞��(ms)
				SetExpertSettingsTextBox(Shenlong.KEY_REVERSE_QUERY_COLUMN_TIME, " " + "1500");

				// �e�[�u���������j���[�ŁA�����J��������ʕ\���ɂ���ݒ�
				SetExpertSettingsCheckBox(Shenlong.KEY_INTELLI_TABLE_JOIN_MENU, true);

				// �t�H�[���̍ő�T�C�Y
				SetExpertSettingsTextBox(Shenlong.KEY_FORM_MAXIMUM_SIZE, "0,0" + " ");

				// �I���N���� SQL*Plus �̃p�X
				SetExpertSettingsTextBox(Shenlong.KEY_ORACLE_SQLPLUS, @"C:\oracle\product\10.2.0\client\bin\sqlplusw.exe" + " ");

				// �G�L�X�p�[�g�p�ŋN�����邩�ۂ�
				SetExpertSettingsCheckBox(Shenlong.KEY_EXPERT_MODE, false);

				// �I�[�i�[���Ⴄ�e�[�u������ "OWNER." ��t���邩�ۂ�
				SetExpertSettingsCheckBox(Shenlong.KEY_PUT_DIFF_OWNER_TO_TABLE, false);

				// "Excel �֓\��t���Ȃ�" �I�v�V������L���ɂ���
				SetExpertSettingsCheckBox(Shenlong.KEY_ENABLE_EXCEL_PASTE_NONE, false);

				// �N�G���[�O�Ƀ��R�[�h������\������
				SetExpertSettingsCheckBox(Shenlong.KEY_SHOW_QUERY_RECORD_COUNT, true);

				// TABLE, VIEW �̃e�[�u�������擾���� SELECT ��
				//string sql = "select sub.owner,sub.tname,sub.tabtype,comments from all_tab_comments,(select all_tables.owner,all_tables.table_name as tname,tab.tabtype from all_tables,tab where all_tables.table_name=tab.tname(+)) sub where sub.tname=all_tab_comments.table_name(+)";
				string sql = "select sub.owner,sub.tname,sub.tabtype,comments from all_tab_comments ,(select all_tables.owner as owner,all_tables.table_name as tname,tab.tabtype as tabtype from all_tables,tab where all_tables.table_name = tab.tname(+))sub where sub.tname = all_tab_comments.table_name(+) and sub.owner = all_tab_comments.owner(+) union select all_views.owner as owner,all_views.view_name as tname,all_views.view_type as tabtype,comments from all_tab_comments,all_views where all_views.view_name = all_tab_comments.table_name(+)";
				SetExpertSettingsTextBox(Shenlong.KEY_SELECT_TABLE_NAME, sql + " ");

				// SYNONYM �̃e�[�u�������擾���� SELECT ��
				sql = "select ...";
				SetExpertSettingsTextBox(Shenlong.KEY_SELECT_SYNONYM_NAME, sql);

				// �I�����ꂽ�e�[�u���̃J�������擾���� SELECT ��
				sql = "select all_tab_columns.column_name,all_tab_columns.data_type,all_tab_columns.nullable,nvl(all_tab_columns.data_precision,all_tab_columns.data_length) as length,all_tab_columns.data_scale,all_col_comments.comments from all_tab_columns%dblink%,all_col_comments%dblink% where all_tab_columns.table_name='%tablename%' and ((all_tab_columns.column_name=all_col_comments.column_name(+)) and (all_tab_columns.table_name=all_col_comments.table_name(+))) order by all_tab_columns.column_id";
				SetExpertSettingsTextBox(Shenlong.KEY_SELECT_COLUMNS, sql + " ");

				// �J�����ꗗ�̔w�i�F��
				SetExpertSettingsTextBox(Shenlong.KEY_COLUMN_LIST_BACK_COLOR_NAME, "GhostWhite" + " ");

				// �N�G���[���ڂ̃e�[�u�����̎��ʐF��
				SetExpertSettingsTextBox(Shenlong.KEY_QUERY_COLUMN_COLOR_NAMES, "Black,Blue,DarkGreen,Purple,SteelBlue,Chocolate,Indigo,DarkSlateGray,Maroon,Olive,DodgerBlue,PaleVioletRed,DarkOliveGreen,DarkGoldenrod,YellowGreen,DarkGray" + " ");

				// ���o�����_�C�A���O�̓��͗����̍ő吔
				SetExpertSettingsTextBox(ParamInputDlg.KEY_MAX_INPUT_PARAM_HISTORY_COUNT, " " + "64");

				if ( Program.debMode )
				{
					// �A�N�Z�X ���O��ۑ�����ݒ�
					SetExpertSettingsCheckBox(Shenlong.KEY_WRITE_ACCESS_LOG, true);

					// ���O�I�� �p�X���[�h�� tooltip ��\������ݒ�
					SetExpertSettingsCheckBox(Shenlong.KEY_LOGON_PWD_TOOLTIP, false);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// �g���ݒ��ۑ�����
		/// </summary>
		private void SaveExpertSettings()
		{
			try
			{
				if ( !Program.expertMode )
					return;

				string text;
				bool? check;

				// ���O�I�������ɒǉ������Ԃ̕����ݒ�
				check = PutExpertSettingsCheckBox(Shenlong.KEY_RESUME_APPEND_LOGON_HIS);
				Shenlong.resumeAppendLogOnHis = (bool?)((check == null) ? checkResumeAppendLogOnHis.Tag : check);

				// ���O�I����������Ő؂�ւ���ݒ�
				check = PutExpertSettingsCheckBox(Shenlong.KEY_AUTO_CHANGE_LOGON);
				Shenlong.autoChangeLogOn = (bool)((check == null) ? checkAutoChangeLogOn.Tag : check);

				// ���O�I�����ɃN�G�����ڂ��N���A���邩�ۂ���I���ł���ݒ�
				check = PutExpertSettingsCheckBox(Shenlong.KEY_SELECTABLE_CLEAR_COLUMN_LOGON);
				Shenlong.selectableClearColumnLogOn = (bool)((check == null) ? checkSelectableClearColumnLogOn.Tag : check);

				// ���O�I�������̍ő吔
				PutExpertSettingsTextBox(Shenlong.KEY_MAX_LOGON_HISTORY_COUNT);

				// �N�G���[���ڂ̍ő吔
				PutExpertSettingsTextBox(Shenlong.KEY_MAX_QUERY_COLUMN_COUNT);

				// �N�G���[���ڂ𔽓]�\�����鎞��(ms)
				text = PutExpertSettingsTextBox(Shenlong.KEY_REVERSE_QUERY_COLUMN_TIME);
				Shenlong.reverseQueryColumnTime = int.Parse((text == null) ? textReverseQueryColumnTime.Tag.ToString().Trim() : text);

				// �e�[�u���������j���[�ŁA�����J��������ʕ\���ɂ���ݒ�
				check = PutExpertSettingsCheckBox(Shenlong.KEY_INTELLI_TABLE_JOIN_MENU);
				Shenlong.intelliTableJoinMenu = (bool)((check == null) ? checkIntelliTableJoinMenu.Tag : check);

				// �t�H�[���̍ő�T�C�Y
				PutExpertSettingsTextBox(Shenlong.KEY_FORM_MAXIMUM_SIZE);

				// �I���N���� SQL*Plus �̃p�X
				Shenlong.oracleSqlPlusPath = PutExpertSettingsTextBox(Shenlong.KEY_ORACLE_SQLPLUS);

				// �G�L�X�p�[�g�p�ŋN�����邩�ۂ�
				PutExpertSettingsCheckBox(Shenlong.KEY_EXPERT_MODE);

				// �I�[�i�[���Ⴄ�e�[�u������ "OWNER." ��t���邩�ۂ�
				check = PutExpertSettingsCheckBox(Shenlong.KEY_PUT_DIFF_OWNER_TO_TABLE);
				Shenlong.putDiffOwnerToTable = (bool)((check == null) ? checkPutDiffOwnerToTable.Tag : check);

				// "Excel �֓\��t���Ȃ�" �I�v�V������L���ɂ���
				check = PutExpertSettingsCheckBox(Shenlong.KEY_ENABLE_EXCEL_PASTE_NONE);
				Shenlong.enableExcelPasteNone = (bool)((check == null) ? checkEnableExcelPasteNone.Tag : check);

				// �N�G���[�O�Ƀ��R�[�h������\������
				check = PutExpertSettingsCheckBox(Shenlong.KEY_SHOW_QUERY_RECORD_COUNT);
				Shenlong.showQueryRecordCount = (bool)((check == null) ? checkShowQueryRecordCount.Tag : check);

				// TABLE, VIEW �̃e�[�u�������擾���� SELECT ��
				PutExpertSettingsTextBox(Shenlong.KEY_SELECT_TABLE_NAME);

				// SYNONYM �̃e�[�u�������擾���� SELECT ��
				PutExpertSettingsTextBox(Shenlong.KEY_SELECT_SYNONYM_NAME);

				// �I�����ꂽ�e�[�u���̃J�������擾���� SELECT ��
				PutExpertSettingsTextBox(Shenlong.KEY_SELECT_COLUMNS);

				// �J�����ꗗ�̔w�i�F��
				text = PutExpertSettingsTextBox(Shenlong.KEY_COLUMN_LIST_BACK_COLOR_NAME);
				Shenlong.columnListBackColorName = (text == null) ? textColumnListBackColorName.Tag.ToString().Trim() : text;
				Shenlong.columnListBackColor.Dispose();
				Shenlong.columnListBackColor = new SolidBrush(Color.FromName(Shenlong.columnListBackColorName));

				// �N�G���[���ڂ̃e�[�u�����̎��ʐF��
				text = PutExpertSettingsTextBox(Shenlong.KEY_QUERY_COLUMN_COLOR_NAMES);
				Shenlong.queryColumnColorNames = (text == null) ? textQueryColumnColorNames.Tag.ToString().Trim() : text;
				Shenlong.SetQueryColumnBrushes();

				// ���o�����_�C�A���O�̓��͗����̍ő吔
				PutExpertSettingsTextBox(ParamInputDlg.KEY_MAX_INPUT_PARAM_HISTORY_COUNT);

				if ( Program.debMode )
				{
					// �A�N�Z�X ���O��ۑ�����ݒ�
					check = PutExpertSettingsCheckBox(Shenlong.KEY_WRITE_ACCESS_LOG);
					Shenlong.writeAccessLog = (bool)((check == null) ? checkWriteAccessLog.Tag : check);

					// ���O�I�� �p�X���[�h�� tooltip ��\������ݒ�
					check = PutExpertSettingsCheckBox(Shenlong.KEY_LOGON_PWD_TOOLTIP);
					Shenlong.logOnPwdToolTip = (bool)((check == null) ? checkLogOnPwdToolTip.Tag : check);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// �g���ݒ���e�L�X�g�{�b�N�X�ɃZ�b�g����
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="example"></param>
		private void SetExpertSettingsTextBox(string keyName, string example)
		{
			TextBox textBox = (TextBox)tabPageExpertSettings.Controls["text" + keyName];
			if ( textBox == null )
				return;

			textBox.Tag = example;	// �ݒ��i�f�t�H���g�j

			StringBuilder returnedString = new StringBuilder(1024);

			if ( api.GetPrivateProfileString(Shenlong.SETTINGS_SECTION, keyName, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) == 0 )
			{
				textBox.Text = example;
				textBox.BackColor = Color.WhiteSmoke;
				//textBox.ForeColor = Color.Gray;
			}
			else
			{
				textBox.Text = returnedString.ToString();
			}
		}

		/// <summary>
		/// �g���ݒ�̃e�L�X�g�{�b�N�X�l��ۑ�����
		/// </summary>
		/// <param name="keyName"></param>
		private string PutExpertSettingsTextBox(string keyName)
		{
			TextBox textBox = (TextBox)tabPageExpertSettings.Controls["text" + keyName];
			if ( textBox == null )
				return null;

			string lpString = null;

			if ( (textBox.Text.Length != 0) && (textBox.Text != (string)textBox.Tag) )	// �e�L�X�g�͐ݒ��i�f�t�H���g�j�ł͂Ȃ��H
			{
				lpString = textBox.Text;
			}

			api.WritePrivateProfileString(Shenlong.SETTINGS_SECTION, keyName, lpString, shenlongIniFileName);

			return lpString;
		}

		/// <summary>
		/// �g���ݒ���`�F�b�N�{�b�N�X�ɃZ�b�g����
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="example"></param>
		private void SetExpertSettingsCheckBox(string keyName, bool? example)
		{
			CheckBox checkBox = (CheckBox)tabPageExpertSettings.Controls["check" + keyName];
			if ( checkBox == null )
				return;

			checkBox.Tag = example;

			StringBuilder returnedString = new StringBuilder(1024);
			if ( api.GetPrivateProfileString(Shenlong.SETTINGS_SECTION, keyName, "", returnedString, (uint)returnedString.Capacity, shenlongIniFileName) == 0 )
			{
				checkBox.CheckState = CheckState.Indeterminate;
			}
			else
			{
				checkBox.Checked = bool.Parse(returnedString.ToString());
			}
		}

		/// <summary>
		/// �g���ݒ�̃`�F�b�N�{�b�N�X�l��ۑ�����
		/// </summary>
		/// <param name="keyName"></param>
		/// <returns></returns>
		private bool? PutExpertSettingsCheckBox(string keyName)
		{
			CheckBox checkBox = (CheckBox)tabPageExpertSettings.Controls["check" + keyName];
			if ( checkBox == null )
				return (bool?)null;

			string lpString = null;

			if ( checkBox.CheckState != CheckState.Indeterminate )
			{
				lpString = checkBox.Checked.ToString().ToLower();
			}

			api.WritePrivateProfileString(Shenlong.SETTINGS_SECTION, keyName, lpString, shenlongIniFileName);

			return (lpString == null) ? (bool?)null : checkBox.Checked;
		}

		/// <summary>
		/// �e�L�X�g�{�b�N�X�̒l���ύX���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textExpSet_TextChanged(object sender, EventArgs e)
		{
			try
			{
				TextBox textBox = (TextBox)sender;

				if ( textBox.BackColor == Color.WhiteSmoke )
				{
					textBox.BackColor = SystemColors.Window;
					textBox.ForeColor = SystemColors.WindowText;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// �e�L�X�g�{�b�N�X���t�H�[�J�X��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textExpSet_Leave(object sender, EventArgs e)
		{
			try
			{
				TextBox textBox = (TextBox)sender;

				if ( textBox.Text.Length == 0 )
				{
					textBox.Text = (string)textBox.Tag;
					textBox.BackColor = Color.WhiteSmoke;
					//textBox.ForeColor = Color.Gray;
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
		#endregion
	}
}