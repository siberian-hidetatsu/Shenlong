//#define	ENABLE_OMW_OO4O
#define	UPDATE_20150406
#define	UPDATE_20160316
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using System.IO;
using System.Collections;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	// http://www.atmarkit.co.jp/fdotnet/dotnettips/436bgworker/bgworker.html ���Ԃ̂����鏈�����o�b�N�O���E���h�Ŏ��s����ɂ́H�Q��

	public partial class ExecuteQueryDlg : Form
	{
		private OracleConnection oraConn = null;
		private string sql = null;
		private string columnComments = null;
		private List<string> logTableNames = null;
		private string password = null;
		public StringBuilder queryOutput = null;
		public string[] dataTypeName = null;
		public int fileHeaderOutputed = 0;

		private int scalar = -1;
		public bool queryExecuted = false;

		/// <summary>
		/// ExecuteQueryDlg
		/// �߂�l
		/// DialogResult.OK: �N�G���[�͐������ďI������
		/// DialogResult.No: �N�G���[�̓G���[�ŏI������
		/// DialogResult.Cancel: �����̓L�����Z�����ꂽ
		/// </summary>
		/// <param name="oraConn"></param>
		/// <param name="sql"></param>
		/// <param name="columnComments"></param>
		public ExecuteQueryDlg(OracleConnection oraConn, string sql, string columnComments, List<string> logTableNames, string password)
		{
			InitializeComponent();

			this.oraConn = oraConn;
			this.sql = sql;
			this.columnComments = columnComments;
			this.logTableNames = logTableNames;
			this.password = password;
			queryOutput = new StringBuilder();
		}

		/// <summary>
		/// ExecuteQueryDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExecuteQueryDlg_Load(object sender, EventArgs e)
		{
			buttonYes.Visible = false;
			buttonNo.Visible = false;
			buttonCancel.Visible = false;
			VisibleStatusStrip(false);

			//int fromIndex = sql.IndexOf("FROM "/*, StringComparison.CurrentCultureIgnoreCase*/);
			int fromIndex;
			if ( Shenlong.showQueryRecordCount && ((fromIndex = ShenGlobal.GetIndexOfWord(sql, "FROM")) != -1) )
			{
				pictureBox.Image = imageList.Images[1];
				labelMessage.Text = "�������������m�F���ł�\r\n���΂炭���҂�������...";
				toolStripStatusLabel.Text = "COUNT(*)";
				VisibleStatusStrip(true);
				this.Cursor = Cursors.WaitCursor;
				Application.DoEvents();

				// �������m�F����X���b�h���N������
				bgWorkerExecuteScalar.RunWorkerAsync(fromIndex);
				toolStripProgressBar.Style = ProgressBarStyle.Marquee;
			}
			else
			{
				BeginExecuteQueryThread();
			}
		}

		/// <summary>
		/// �X�e�[�^�X �o�[�̕\����؂�ւ���
		/// </summary>
		/// <param name="visible"></param>
		private void VisibleStatusStrip(bool visible)
		{
			Size formSize = this.Size;
			if ( statusStrip.Visible = visible )
			{
				formSize.Height += statusStrip.Height;
			}
			else
			{
				formSize.Height -= statusStrip.Height;
				toolStripStatusLabel.Text = "Ready";
			}
			this.Size = formSize;
		}

		/// <summary>
		/// �������m�F����X���b�h
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerExecuteScalar_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				//BackgroundWorker bgWorker = (BackgroundWorker)sender;

				string countSql = MakeCountSql((int)e.Argument);
				if ( countSql == null )
				{
					e.Result = "1";
					return;
				}

				if ( Shenlong.oraMiddleware == Shenlong.omw.OracleClient )
				{
					e.Result = ExecuteScalarOracleClient(countSql);
				}
				else if ( Shenlong.oraMiddleware == Shenlong.omw.OleDb )
				{
					e.Result = ExecuteScalarOleDb(countSql);
				}
#if ENABLE_OMW_OO4O
				else if ( Shenlong.oraMiddleware == Shenlong.omw.oo4o )
				{
					e.Result = ExecuteScalarOo4o(countSql);
				}
#endif
			}
			catch ( Exception exp )
			{
				e.Result = "[ERROR]";
				queryOutput = new StringBuilder(exp.Message);
			}
		}

		/// <summary>
		/// �����m�F�p�� SQL ���\�z����
		/// </summary>
		/// <param name="fromIndex"></param>
		/// <returns></returns>
		private string MakeCountSql(int fromIndex)
		{
			string _sql = sql;
			StringBuilder countColumn = new StringBuilder("*");

			string[] groupFunc = { "SUM(", "AVG(", "MIN(", "MAX(", "COUNT(" };
			int groupFuncIndex = GetIndexOfAnyString(_sql.Substring(0, fromIndex), groupFunc);

			int untilFromIndex;
			List<string> tableNames = ShenGlobal.GetTableNameInSQL(_sql, false, false, out untilFromIndex);

#if false
			int orderByIndex = _sql.IndexOf("ORDER BY", untilFromIndex + 1, StringComparison.OrdinalIgnoreCase);
			if ( orderByIndex != -1 )
			{
				_sql = _sql.Substring(0, orderByIndex);	// �Ƃ肠�����AORDER BY �ȍ~����菜��
			}
#else
			int orderByIndex = _sql.IndexOf("ORDER BY", untilFromIndex + 1, StringComparison.OrdinalIgnoreCase);
			while ( orderByIndex != -1 )
			{
				int endOfOrderBy = orderByIndex;
#if UPDATE_20150406
				int parenthesesCount = 0;	// ORDER BY ��̒��Ɋ��ʂň͂܂ꂽ�J���������������ꍇ�̑΍�
				for ( ; endOfOrderBy < _sql.Length; endOfOrderBy++ )
				{
					if ( _sql[endOfOrderBy] == '(' )
					{
						parenthesesCount++;
					}
					else if ( _sql[endOfOrderBy] == ')' )
					{
						if ( --parenthesesCount < 0 )	// �T�u�N�G���̕����ʁH
							break;
					}
				}
#else
				for ( ; endOfOrderBy < _sql.Length; endOfOrderBy++ )
				{
					if ( _sql[endOfOrderBy] == ')' )
					{
						break;
					}
				}
#endif
				//endOfOrderBy -= (endOfOrderBy == _sql.Length ? 1 : 0);

				_sql = _sql.Substring(0, orderByIndex) + _sql.Substring(endOfOrderBy);	// ORDER BY �����菜��
				orderByIndex = _sql.IndexOf("ORDER BY", orderByIndex, StringComparison.OrdinalIgnoreCase);
			}
#endif

			if ( groupFuncIndex == -1 ) // SELECT ����J�����ɃO���[�v�֐��͖����H
			{
				if ( _sql.IndexOf("DISTINCT", 0, fromIndex, StringComparison.OrdinalIgnoreCase) != -1 )
				{
					List<string> selectColumns = ShenGlobal.GetSelectColumnInSQL(_sql, true);
					countColumn = new StringBuilder("DISTINCT" + " " + selectColumns[0]);
				}
			}
			else
			{
				if ( _sql.IndexOf("GROUP BY", untilFromIndex + 1, StringComparison.OrdinalIgnoreCase) != -1 )
				{
#if false
					int columnIndex = _sql.IndexOf("SELECT");
					for ( columnIndex += 6; !Char.IsLetter(_sql[columnIndex]); columnIndex++ ) ;
					string[] columns = _sql.Substring(columnIndex, fromIndex - columnIndex).Split(',');	// SELECT �Ώۂ̃J����
					countColumn = new StringBuilder();
					for ( int i = 0; i < columns.Length; i++ )
					{
						if ( (columns[i].IndexOf('(') == -1) || (GetIndexOfAnyString(columns[i], groupFunc) == -1) )
							continue;
						countColumn.Append(((countColumn.Length == 0) ? "" : ",") + ShenGlobal.GetPlainTableFieldName(columns[i])/*columns[i].Trim()*/);
						/*int leftRoundBracket = GetCharCountInString(columns[i], '(') - GetCharCountInString(columns[i], ')');
						for ( i++; (i < columns.Length) && (0 < leftRoundBracket); i++ )	// ���炩��SQL�֐�(����)���g�p����A�����̕���(�J���})�ŕ�������Ă��܂����H
						{
							string val = Shenlong.GetPlainTableFieldName(columns[i]);
							countColumn.Append("," + val);
							int rightRoundBracket = GetCharCountInString(val, ')');
							leftRoundBracket -= rightRoundBracket;
						}*/
						break;	// �J�E���g����͍̂ŏ��̍��ڂ����ł����H
					}
#else
					List<string> selectColumns = ShenGlobal.GetSelectColumnInSQL(_sql, true);

					foreach ( string column in selectColumns )
					{
						if ( GetIndexOfAnyString(column, groupFunc) != -1 )
						{
							countColumn = new StringBuilder(column);
							break;
						}
					}
#endif
				}
				else
				{
					return null;	// �O���[�v�֐��݂̂� SELECT ���鎞�́A�Ƃ肠�����P�s�Ƃ���
				}
			}

			string countSql = "SELECT COUNT(" + countColumn + ") " + _sql.Substring(fromIndex);
#if false
#if !ENABLED_SUBQUERY
			int orderBy = countSql.IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
#else
			int orderBy = countSql.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
			if ( (orderBy != -1) && (countSql.IndexOf(')', orderBy) != -1) )	// �T�u�N�G���̒��� ORDER BY ?
			{
				orderBy = -1;
			}
#endif
			if ( orderBy != -1 )
			{
				countSql = countSql.Substring(0, orderBy);	// �Ƃ肠�����AORDER BY �ȍ~����菜��
			}
#endif
			Debug.WriteLine(countSql);
			return countSql;
		}

		/// <summary>
		/// �����񂩂畡���̕�������������ăC���f�b�N�X���擾����
		/// </summary>
		/// <param name="str"></param>
		/// <param name="strs"></param>
		/// <returns></returns>
		private int GetIndexOfAnyString(string str, string[] strs)
		{
			int index = -1;

			for ( int i = 0; i < strs.Length; i++ )
			{
				if ( (index = str.IndexOf(strs[i], StringComparison.OrdinalIgnoreCase)) != -1 )
					break;
			}

			return index;
		}

		/// <summary>
		/// ������Ɋ܂܂�镶���̐����擾����
		/// </summary>
		/// <param name="str"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		private int GetCharCountInString(string str, char c)
		{
			int count = 0;

			for ( int i = 0; i < str.Length; i++ )
			{
				if ( str[i] == c )
					count++;
			}

			return count;
		}

		/// <summary>
		/// ExecuteScalarOracleClient
		/// System.Data.OracleClient �o�[�W����
		/// </summary>
		/// <param name="countSql"></param>
		/// <returns></returns>
		private string ExecuteScalarOracleClient(string countSql)
		{
			OracleCommand oraCmd = null;

			try
			{
				oraCmd = new OracleCommand(countSql, oraConn);
				scalar = int.Parse(oraCmd.ExecuteScalar().ToString());
				return scalar.ToString();
			}
			finally
			{
				if ( oraCmd != null )
				{
					oraCmd.Dispose();
					oraCmd = null;
				}
			}
		}

		/// <summary>
		/// ExecuteScalarOleDb
		/// System.Data.OleDb �o�[�W����
		/// </summary>
		/// <param name="countSql"></param>
		/// <returns></returns>
		private string ExecuteScalarOleDb(string countSql)
		{
			OleDbConnection oleConn = null;
			OleDbCommand oleCmd = null;

			try
			{
				string dataSource, userId, password;
				GetOraDsnUidPwd(out dataSource, out userId, out password);
				oleConn = new OleDbConnection("Provider=MSDAORA;Data Source=" + dataSource + ";" +
											  "user id=" + userId + ";password=" + password + ";" +
											  "persist security info=false;");
				oleConn.Open();

				oleCmd = new OleDbCommand(countSql, oleConn);
				scalar = int.Parse(oleCmd.ExecuteScalar().ToString());
				return scalar.ToString();
			}
			finally
			{
				if ( oleCmd != null )
				{
					oleCmd.Dispose();
					oleCmd = null;
				}

				if ( oleConn != null )
				{
					if ( oleConn.State == ConnectionState.Open )
					{
						oleConn.Close();
					}
					oleConn.Dispose();
					oleConn = null;
				}
			}
		}

#if ENABLE_OMW_OO4O
		/// <summary>
		/// ExecuteScalarOo4o
		/// OO4O �o�[�W�����F�Q�Ɛݒ�� OracleInProcServer �̒ǉ����K�v
		/// </summary>
		/// <param name="countSql"></param>
		/// <returns></returns>
		private string ExecuteScalarOo4o(string countSql)
		{
			OracleInProcServer.OraSessionClassClass oo4oSession = null;
			OracleInProcServer.OraDatabase oo4oDatabase = null;
			OracleInProcServer.OraDynaset oo4oDynaset = null;

			try
			{
				string dataSource, userId, password;
				GetOraDsnUidPwd(out dataSource, out userId, out password);

				oo4oSession = new OracleInProcServer.OraSessionClassClass();
				oo4oDatabase = (OracleInProcServer.OraDatabase)oo4oSession.get_OpenDatabase(dataSource, userId + "/" + password, 0);

				Object obj = System.Reflection.Missing.Value;
				oo4oDynaset = (OracleInProcServer.OraDynaset)oo4oDatabase.get_CreateDynaset(countSql, 0, ref obj);

				scalar = int.Parse(((OracleInProcServer.OraField)((OracleInProcServer.OraFields)oo4oDynaset.Fields)[0]).Value.ToString());
				return scalar.ToString();
			}
			finally
			{
				if ( oo4oDynaset != null )
				{
					oo4oDynaset.Close();
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oDynaset) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oDynaset);
					}
					oo4oDynaset = null;
				}

				if ( oo4oDatabase != null )
				{
					oo4oDatabase.Close();
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oDatabase) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oDatabase);
					}
					oo4oDatabase = null;
				}

				if ( oo4oSession != null )
				{
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oSession) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oSession);
					}
					oo4oSession = null;
				}
			}
		}
#endif

		/// <summary>
		/// GetOraDsnUidPwd
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="userId"></param>
		/// <param name="password"></param>
		private void GetOraDsnUidPwd(out string dataSource, out string userId, out string password)
		{
			try
			{
				string[] connString = oraConn.ConnectionString.Split(';');
				dataSource = connString[0].Substring(connString[0].IndexOf('=') + 1);
				userId = connString[1].Substring(connString[1].IndexOf('=') + 1);

				if ( !string.IsNullOrEmpty(this.password) )
				{
					password = this.password;
				}
				else
				{
					/*string xmlLogOnFileName = Application.StartupPath + LogOnDlg.LOGON_FILE_NAME;
					System.Xml.XmlDocument xmlLogOn = new System.Xml.XmlDocument();
					xmlLogOn.Load(xmlLogOnFileName);
					string xpath = "/" + LogOnDlg.tagRoot + "/" + LogOnDlg.tagLogOn + "[@" + LogOnDlg.attrSID + "='" + dataSource + "'][" + LogOnDlg.tagUserName + "='" + userId + "']";
					System.Xml.XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
					password = CommonFunctions.common.DecodePassword(logOnNode[LogOnDlg.tagPassword].InnerText);*/
					password = LogOnDlg.GetLogOnPassword(dataSource, userId);
				}
			}
			catch ( Exception exp )
			{
				System.Diagnostics.Debug.WriteLine(exp.Message);
				dataSource = userId = password = "";
			}
		}

		/// <summary>
		/// �������m�F����X���b�h���I������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerExecuteScalar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			toolStripProgressBar.Style = ProgressBarStyle.Blocks;
			toolStripProgressBar.Value = 0;
			toolStripProgressBar.Text = "";
			VisibleStatusStrip(false);
			this.Cursor = Cursors.Default;

			string result = (string)e.Result;
			if ( !result.StartsWith("[ERROR]") )
			{
				//if ( Shenlong.oraMiddleware != Shenlong.omw.OracleClient )
				{
					this.Text += (" (" + Shenlong.oraMiddleware.ToString() + ")");
				}
				pictureBox.Image = imageList.Images[0];
				labelMessage.Text = "������ " + result + " �ł�\r\n���s���܂����H";
				buttonYes.Visible = true;
				buttonNo.Visible = true;
			}
			else
			{
				BeginExecuteQueryThread();
			}
		}

		/// <summary>
		/// [�͂�] �{�^���������ꂽ
		/// �N�G���[�𑱍s����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonYes_Click(object sender, EventArgs e)
		{
			BeginExecuteQueryThread();
		}

		/// <summary>
		/// [������] �{�^���������ꂽ
		/// �N�G���[�𑱍s���Ȃ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonNo_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// �N�G���[�̃X���b�h���N������
		/// </summary>
		private void BeginExecuteQueryThread()
		{
			buttonYes.Visible = false;
			buttonNo.Visible = false;
			buttonCancel.Visible = true;

			pictureBox.Image = imageList.Images[1];
			labelMessage.Text = "�������f�[�^�x�[�X�ɖ₢���킹���ł�\r\n���΂炭���҂�������...";
			toolStripStatusLabel.Text = "SELECT";
			VisibleStatusStrip(true);
			this.Cursor = Cursors.WaitCursor;
			Application.DoEvents();

			bgWorkerExecuteQuery.RunWorkerAsync();
			if ( scalar == -1 )
			{
				toolStripProgressBar.Style = ProgressBarStyle.Marquee;
			}
		}

		/// <summary>
		/// [�L�����Z��] �{�^���������ꂽ
		/// �N�G���[�̎��s���L�����Z�����ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			bgWorkerExecuteQuery.CancelAsync();
			labelMessage.Text = "�L�����Z���������ł�";
			buttonCancel.Enabled = false;
			Application.DoEvents();
		}

		/// <summary>
		/// �N�G���[�����s����X���b�h
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerExecuteQuery_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				WriteAccessLog();

				BackgroundWorker bgWorker = (BackgroundWorker)sender;

				if ( Shenlong.oraMiddleware == Shenlong.omw.OracleClient )
				{
					ExecuteQueryOracleClient(ref bgWorker, ref e);
				}
				else if ( Shenlong.oraMiddleware == Shenlong.omw.OleDb )
				{
					ExecuteQueryOleDb(ref bgWorker, ref e);
				}
#if ENABLE_OMW_OO4O
				else if ( Shenlong.oraMiddleware == Shenlong.omw.oo4o )
				{
					ExecuteQueryOo4o(ref bgWorker, ref e);
				}
#endif

				e.Result = "[OK]";
				queryExecuted = true;
			}
			catch ( Exception exp )
			{
				e.Result = "[ERROR]";
				queryOutput = new StringBuilder(exp.Message);
			}
		}

#if WITHIN_SHENGLOBAL
		/// <summary>
		/// �A�N�Z�X ���O���e�[�u���ɕۑ�����
		/// </summary>
		private void WriteAccessLog()
		{
			OracleConnection oraInfoPub = null;
			OracleCommand oraCmd = null;

			try
			{
				if ( logTableNames == null )
					return;

				string infoPubSID = "dbsv01", infoPubUser = "shenlong", infoPubPwd = "amkj1shen";

				/*try
				{
					string xmlLogOnFileName = Application.StartupPath + LogOnDlg.LOGON_FILE_NAME;
					XmlDocument xmlLogOn = new XmlDocument();
					xmlLogOn.Load(xmlLogOnFileName);
					string xpath = "/" + LogOnDlg.tagRoot + "/" + LogOnDlg.tagLogOn + "[@" + LogOnDlg.attrSID + "='" + infoPubSID + "']" + "[" + LogOnDlg.tagUserName + "='" + infoPubUser + "']";
					XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
					if ( logOnNode != null )
					{
						// LogOn.xml �ɓo�^����Ă���p�X���[�h��D�悷��
						infoPubSID = logOnNode.Attributes[LogOnDlg.attrSID].Value;
						infoPubUser = logOnNode[LogOnDlg.tagUserName].InnerText;
						infoPubPwd = common.DecodePassword(logOnNode[LogOnDlg.tagPassword].InnerText);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
				}*/

				string conStr = "Data Source=" + infoPubSID + ";User Id=" + infoPubUser + ";Password=" + infoPubPwd;
				oraInfoPub = new OracleConnection(conStr);
				oraInfoPub.Open();							// �����J�T�[�o�ɐڑ�����

				string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");	// ACCESS_DATE
				string serviceName = string.Empty, userName = string.Empty, pcName;

				string[] connectionString = oraConn.ConnectionString.Split(';');
				foreach ( string conn in connectionString )
				{
					if ( conn.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) )
						serviceName = conn.Substring(12);
					else if ( conn.StartsWith("User Id=", StringComparison.OrdinalIgnoreCase) )
						userName = conn.Substring(8);
				}

				try
				{
					pcName = System.Net.Dns.GetHostName().ToLower();// PC_NAME
				}
				catch ( Exception exp )
				{
					pcName = exp.Message;
				}

				foreach ( string tableName in logTableNames )
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
#else
		/// <summary>
		/// �A�N�Z�X ���O���e�[�u���ɕۑ�����
		/// </summary>
		private void WriteAccessLog()
		{
			try
			{
				if ( logTableNames == null )
					return;

				string serviceName = string.Empty, userName = string.Empty;

				string[] connectionString = oraConn.ConnectionString.Split(';');
				foreach ( string conn in connectionString )
				{
					if ( conn.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) )
						serviceName = conn.Substring(12);
					else if ( conn.StartsWith("User Id=", StringComparison.OrdinalIgnoreCase) )
						userName = conn.Substring(8);
				}

				ShenGlobal.WriteAccessLog(Shenlong.writeLogDsnUidPwd, serviceName, userName, logTableNames, ShenGlobal.pno.shenlong);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine("[" + MethodBase.GetCurrentMethod().Name + "] " + exp.Message);
			}
		}
#endif

		/// <summary>
		/// ExecuteQueryOracleClient
		/// System.Data.OracleClient �o�[�W����
		/// </summary>
		/// <param name="bgWorker"></param>
		/// <param name="e"></param>
		private void ExecuteQueryOracleClient(ref BackgroundWorker bgWorker, ref DoWorkEventArgs e)
		{
			OracleCommand oraCmd = null;
			OracleDataReader oraReader = null;

			try
			{
				oraCmd = new OracleCommand(sql, oraConn);
				oraReader = oraCmd.ExecuteReader();

				string[] columnNames = new string[oraReader.FieldCount];
				dataTypeName = new string[oraReader.FieldCount];

				for ( int i = 0; i < oraReader.FieldCount; i++ )		// �t�B�[���h�����擾
				{
					columnNames[i] = oraReader.GetName(i);
#if UPDATE_20160316
					dataTypeName[i] = oraReader.GetDataTypeName(i).ToUpper();
#else
					dataTypeName[i] = oraReader.GetDataTypeName(i);
#endif
				}

				OutputHeader(columnNames);								// �w�b�_���o�͂���

				int counter = 0, prevPercentage = 0;

				Object[] values = new Object[oraReader.FieldCount];

				while ( oraReader.Read() )								// �P�s���ǂݍ���
				{
#if false
					for ( int i = 0; i < oraReader.FieldCount; i++ )
					{
						string value = oraReader[i].ToString();
						queryOutput.Append(value + Shenlong.sepOutput);
					}
#else
					oraReader.GetOracleValues(values);
					foreach ( Object value in values )
					{
						string strValue = value.ToString();
#if UPDATE_20160316
						queryOutput.Append(((string.Compare(strValue, "null", true) != 0) ? strValue : "") + ShenGlobal.sepOutput);
#else
						queryOutput.Append(((strValue != "Null") ? strValue : "") + ShenGlobal.sepOutput);
#endif
					}
#endif
					queryOutput.Append("\r\n");

					// �L�����Z������ĂȂ�������I�Ƀ`�F�b�N
					if ( bgWorker.CancellationPending )
					{
						e.Cancel = true;
						return;
					}

					// �i����
					if ( (scalar != -1) && (counter < scalar) )
					{
						int percentage = ++counter * 100 / scalar;
						if ( (percentage % 5 == 0) && (prevPercentage < percentage) )
						{
							bgWorker.ReportProgress(percentage);
							prevPercentage = percentage;
							//Debug.WriteLine(percentage + "%");
						}
					}
				}
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
			}
		}

		/// <summary>
		/// �w�b�_���o�͂���
		/// </summary>
		/// <param name="columnNames"></param>
		private void OutputHeader(string[] columnNames)
		{
			// �J�������̏o�̓t���O���I���B�܂��́A�R�����g�̏o�̓t���O���I���Ȃ̂ɃR�����g�������H
			if ( ((Shenlong.fileHeaderOutput & (int)ShenGlobal.header.columnName) != 0) ||
				 (((Shenlong.fileHeaderOutput & (int)ShenGlobal.header.comment) != 0) && (columnComments == null)) )
			{
				// �J���������o�͂���
				queryOutput.Append(string.Join(ShenGlobal.sepOutput, columnNames));
				queryOutput.Append("\r\n");

				fileHeaderOutputed |= (int)ShenGlobal.header.columnName;
			}

			// �R�����g�̏o�̓t���O���I���ŁA�R�����g������H
			if ( ((Shenlong.fileHeaderOutput & (int)ShenGlobal.header.comment) != 0) && (columnComments != null) )
			{
				// �J���������o�͂��ꂽ�H
				if ( (Shenlong.fileHeaderOutput & (int)ShenGlobal.header.columnName) != 0 )
				{
					// �R�����g�����̂܂܏o�͂���
					queryOutput.Append(columnComments + "\r\n");
				}
				// �R�����g�݂̂��o�͂���
				else
				{
					string[] colComments = columnComments.Split(ShenGlobal.sepOutput[0]);
					for ( int i = 0; i < colComments.Length; i++ )
					{
						// �R�����g�̐ݒ肪������́A�J���������o�͂���
						queryOutput.Append((colComments[i] == ShenGlobal.propNoComment) ? columnNames[i] : colComments[i]);
						queryOutput.Append(ShenGlobal.sepOutput);
					}
					queryOutput.Append("\r\n");
				}

				fileHeaderOutputed |= (int)ShenGlobal.header.comment;
			}
		}

		/// <summary>
		/// ExecuteQueryOleDb
		/// System.Data.OleDb �o�[�W����
		/// </summary>
		/// <param name="bgWorker"></param>
		/// <param name="e"></param>
		private void ExecuteQueryOleDb(ref BackgroundWorker bgWorker, ref DoWorkEventArgs e)
		{
			OleDbConnection oleConn = null;
			OleDbCommand oleCmd = null;
			OleDbDataReader oleReader = null;

			try
			{
				string dataSource, userId, password;
				GetOraDsnUidPwd(out dataSource, out userId, out password);
				oleConn = new OleDbConnection("Provider=MSDAORA;Data Source=" + dataSource + ";" +
											  "user id=" + userId + ";password=" + password + ";" +
											  //"DistribTX=0;" +			/* "Oracle Provider for OLE DB" �ł� DBLINK �΍�H */
											  "persist security info=false;");
				oleConn.Open();
				oleCmd = new OleDbCommand(sql, oleConn);
				oleReader = oleCmd.ExecuteReader();

				string[] columnNames = new string[oleReader.FieldCount];
				dataTypeName = new string[oleReader.FieldCount];

				for ( int i = 0; i < oleReader.FieldCount; i++ )		// �t�B�[���h�����擾
				{
					columnNames[i] = oleReader.GetName(i);
					dataTypeName[i] = GetOraDataTypeName(oleReader.GetDataTypeName(i));
				}

				OutputHeader(columnNames);								// �w�b�_���o�͂���

				int counter = 0, prevPercentage = 0;

				Object[] values = new Object[oleReader.FieldCount];

				while ( oleReader.Read() )								// �P�s���ǂݍ���
				{
#if false
					for ( int i = 0; i < oraReader.FieldCount; i++ )
					{
						string value = oraReader[i].ToString();
						queryOutput.Append(value + Shenlong.sepOutput);
					}
#else
					oleReader.GetValues(values);
					foreach ( Object value in values )
					{
						string strValue = value.ToString();
						queryOutput.Append(((strValue != "Null") ? strValue : "") + ShenGlobal.sepOutput);
					}
#endif
					queryOutput.Append("\r\n");

					// �L�����Z������ĂȂ�������I�Ƀ`�F�b�N
					if ( bgWorker.CancellationPending )
					{
						e.Cancel = true;
						return;
					}

					// �i����
					if ( (scalar != -1) && (counter < scalar) )
					{
						int percentage = ++counter * 100 / scalar;
						if ( (percentage % 5 == 0) && (prevPercentage < percentage) )
						{
							bgWorker.ReportProgress(percentage);
							prevPercentage = percentage;
							//Debug.WriteLine(percentage + "%");
						}
					}
				}
			}
			finally
			{
				if ( oleReader != null )
				{
					oleReader.Close();
					oleReader.Dispose();
					oleReader = null;
				}

				if ( oleCmd != null )
				{
					oleCmd.Dispose();
					oleCmd = null;
				}

				if ( oleConn != null )
				{
					if ( oleConn.State == ConnectionState.Open )
					{
						oleConn.Close();
					}
					oleConn.Dispose();
					oleConn = null;
				}
			}
		}

		/// <summary>
		/// OleDb �̌^���I���N���̌^�ɕϊ�����
		/// </summary>
		/// <param name="dataTypeName"></param>
		/// <returns></returns>
		private string GetOraDataTypeName(string dataTypeName)
		{
			switch ( dataTypeName )
			{
				case "DBTYPE_VARCHAR":
					return ("VARCHAR2");
				case "DBTYPE_DBTIMESTAMP":
					return ("DATE");
				default:
					if ( dataTypeName.StartsWith("DBTYPE_") )
					{
						return dataTypeName.Substring(7);
					}
					return (dataTypeName);
			}
		}

#if ENABLE_OMW_OO4O
		/// <summary>
		/// ExecuteQueryOo4o
		/// OO4O �o�[�W�����F�Q�Ɛݒ�� OracleInProcServer �̒ǉ����K�v
		/// </summary>
		/// <param name="bgWorker"></param>
		/// <param name="e"></param>
		private void ExecuteQueryOo4o(ref BackgroundWorker bgWorker, ref DoWorkEventArgs e)
		{
			OracleInProcServer.OraSessionClassClass oo4oSession = null;
			OracleInProcServer.OraDatabase oo4oDatabase = null;
			OracleInProcServer.OraDynaset oo4oDynaset = null;

			try
			{
				//string executeQueryLogFileName = Application.StartupPath + @"\" + "~execquery.log";
				//using ( StreamWriter swExecQueryLog = new StreamWriter(executeQueryLogFileName, false, Encoding.GetEncoding("shift_jis")) )
				{
					//swExecQueryLog.WriteLine("ExecuteQueryOo4o");

					string dataSource, userId, password;

					//swExecQueryLog.WriteLine(oraConn.ConnectionString);
					GetOraDsnUidPwd(out dataSource, out userId, out password);

					//swExecQueryLog.WriteLine("beginning " + dataSource + " " + userId + " " + password);
					oo4oSession = new OracleInProcServer.OraSessionClassClass();
					//swExecQueryLog.WriteLine(oo4oSession.ToString());
					oo4oDatabase = (OracleInProcServer.OraDatabase)oo4oSession.get_OpenDatabase(dataSource, userId + "/" + password, 0);
					//swExecQueryLog.WriteLine(oo4oDatabase.ToString());
					Object obj = System.Reflection.Missing.Value;
					//swExecQueryLog.WriteLine(sql);
					oo4oDynaset = (OracleInProcServer.OraDynaset)oo4oDatabase.get_CreateDynaset(sql, 0, ref obj);
					//swExecQueryLog.WriteLine(oo4oDynaset.ToString());

					OracleInProcServer.OraFields oraFields = (OracleInProcServer.OraFields)oo4oDynaset.Fields;
					//swExecQueryLog.WriteLine(oraFields.ToString());
					dataTypeName = new string[oraFields.Count];
					//swExecQueryLog.WriteLine(oraFields.Count);

					for ( int i = 0; i < oraFields.Count; i++ )		// �t�B�[���h�����擾
					{
						OracleInProcServer.OraField oraField = (OracleInProcServer.OraField)oraFields[i];
						queryOutput.Append(oraField.Name + Shenlong.sepOutput);
						//swExecQueryLog.WriteLine(oraField.Name + "\t");
						dataTypeName[i] = GetOraIDataType(oraField.OraIDataType);
					}
					queryOutput.Append("\r\n");
					//swExecQueryLog.Write("\r\n");

					if ( columnComments != null )
					{
						queryOutput.Append(columnComments + "\r\n");
					}

					int counter = 0, prevPercentage = 0;

					while ( !oo4oDynaset.EOF )
					{
						for ( int i = 0; i < oraFields.Count; i++ )
						{
							string value = ((OracleInProcServer.OraField)((OracleInProcServer.OraFields)oo4oDynaset.Fields)[i]).Value.ToString();
							queryOutput.Append(value + Shenlong.sepOutput);
							//swExecQueryLog.Write(value + "\t");
						}
						queryOutput.Append("\r\n");
						//swExecQueryLog.Write("\r\n");
						oo4oDynaset.MoveNext();

						// �L�����Z������ĂȂ�������I�Ƀ`�F�b�N
						if ( bgWorker.CancellationPending )
						{
							e.Cancel = true;
							return;
						}

						// �i����
						if ( (scalar != -1) && (counter < scalar) )
						{
							int percentage = ++counter * 100 / scalar;
							if ( (percentage % 5 == 0) && (prevPercentage < percentage) )
							{
								bgWorker.ReportProgress(percentage);
								prevPercentage = percentage;
								//Debug.WriteLine(percentage + "%");
							}
						}
					}
				}
			}
			finally
			{
				if ( oo4oDynaset != null )
				{
					oo4oDynaset.Close();
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oDynaset) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oDynaset);
					}
					oo4oDynaset = null;
				}

				if ( oo4oDatabase != null )
				{
					oo4oDatabase.Close();
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oDatabase) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oDatabase);
					}
					oo4oDatabase = null;
				}

				if ( oo4oSession != null )
				{
					if ( System.Runtime.InteropServices.Marshal.IsComObject(oo4oSession) )
					{
						int count = System.Runtime.InteropServices.Marshal.ReleaseComObject(oo4oSession);
					}
					oo4oSession = null;
				}
			}
		}

		/// <summary>
		/// OraIDataType �𕶎���ɕϊ�����
		/// </summary>
		/// <param name="oraIDataType"></param>
		/// <returns></returns>
		private string GetOraIDataType(int oraIDataType)
		{
			switch ( oraIDataType )
			{
				case (int)OracleInProcServer.serverType.ORATYPE_VARCHAR2:
					return ("VARCHAR2");
				case (int)OracleInProcServer.serverType.ORATYPE_NUMBER:
					return ("NUMBER");
				case (int)OracleInProcServer.serverType.ORATYPE_DATE:
					return ("DATE");
				case (int)OracleInProcServer.serverType.ORATYPE_RAW:
					return ("RAW");
				case (int)OracleInProcServer.serverType.ORATYPE_CHAR:
					return ("CHAR");
				case (int)OracleInProcServer.serverType.ORATYPE_MLSLABEL:
					return ("MLSLABEL");
				case (int)OracleInProcServer.serverType.ORATYPE_OBJECT:
					return ("OBJECT");
				case (int)OracleInProcServer.serverType.ORATYPE_REF:
					return ("REF");
				case (int)OracleInProcServer.serverType.ORATYPE_CLOB:
					return ("CLOB");
				case (int)OracleInProcServer.serverType.ORATYPE_BLOB:
					return ("BLOB");
				case (int)OracleInProcServer.serverType.ORATYPE_BFILE:
					return ("BFILE");
				case (int)OracleInProcServer.serverType.ORATYPE_VARRAY:
					return ("VARRAY");
				case (int)OracleInProcServer.serverType.ORATYPE_TABLE:
					return ("NESTED TABLE");
				default:
					return ("unknown");
			}
		}
#endif

		/// <summary>
		/// �N�G���[�̐i����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerExecuteQuery_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// ���� Text �v���p�e�B���I�[�o�[���C�h���Ă��A�v���O���X�o�[��Ƀe�L�X�g���\�������킯�ł͂Ȃ��A
			// ToolTipText �̑���iAutoToolTip = true, StatusStrip �̕��� ShowItemToopTip = true�j�� Text ���g���悤�ɂȂ邾���H
			toolStripProgressBar.Text = e.ProgressPercentage + "%";
			toolStripProgressBar.Value = e.ProgressPercentage;
		}

		/// <summary>
		/// �N�G���[�����s����X���b�h���I������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgWorkerExecuteQuery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			toolStripProgressBar.Style = ProgressBarStyle.Blocks;
			toolStripProgressBar.Value = 0;
			toolStripProgressBar.Text = "";
			VisibleStatusStrip(false);
			this.Cursor = Cursors.Default;

			if ( e.Cancelled )
			{
				DialogResult = DialogResult.Cancel;
			}
			else
			{
				string result = (string)e.Result;
				DialogResult = (result.StartsWith("[OK]")) ? DialogResult.OK : DialogResult.No;
			}

			this.Close();
		}
	}
}