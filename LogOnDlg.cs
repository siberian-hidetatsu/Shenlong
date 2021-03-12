using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;
using System.Diagnostics;
using CommonFunctions;

namespace Shenlong
{
	public partial class LogOnDlg : Form
	{
		private const string LOGON_FILE_NAME = @"\LogOn.xml";

		private const string tagRoot = "root";
		private const string tagLogOn = "logOn";
		private const string attrSID = "sid";
		private const string tagUserName = "userName";
		private const string tagPassword = "password";
		
		private string xmlLogOnFileName = null;
		private XmlDocument xmlLogOn = null;

		public OracleConnection oraConn = null;

		public enum usages { manual, resume, require, auto };

		private usages usage;
		private string sid;
		private string userName;
		private int maxLogOnHistoryCount;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="usage"></param>
		/// <param name="sid"></param>
		/// <param name="userName"></param>
		/// <param name="maxLogOnHistoryCount"></param>
		public LogOnDlg(usages usage, string sid, string userName, int maxLogOnHistoryCount)
		{
			InitializeComponent();

			this.usage = usage;
			this.sid = sid;
			this.userName = userName;
			this.maxLogOnHistoryCount = maxLogOnHistoryCount;

			this.ShowInTaskbar = ((usage == usages.require) || (usage == usages.auto));
		}

		/// <summary>
		/// LogOn_Load
		/// </summary>
		private void LogOn_Load(object sender, EventArgs e)
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

				checkSavePassword.Enabled = false;

				buttonCancel.Location = buttonOK.Location;
				buttonCancel.SendToBack();
				
				// xmlLogOnFileName ��ǂݍ���
				xmlLogOnFileName = Application.StartupPath + LOGON_FILE_NAME;
				xmlLogOn = new XmlDocument();

				if ( !File.Exists(xmlLogOnFileName) )
				{
					XmlDeclaration decl = xmlLogOn.CreateXmlDeclaration("1.0", "utf-8", null);
					xmlLogOn.AppendChild(decl);

					XmlElement elem = xmlLogOn.CreateElement(tagRoot);	// <root>
					xmlLogOn.AppendChild(elem);

					xmlLogOn.Save(xmlLogOnFileName);
				}

				xmlLogOn.Load(xmlLogOnFileName);

				// �t�H�[��������������
				VisibleStatusStrip(false);

				/*if ( Shenlong.windowRectangle != Rectangle.Empty )
				{
					this.Location = new Point(Shenlong.windowRectangle.X + (Shenlong.windowRectangle.Width - this.Width) / 2, Shenlong.windowRectangle.Y + (Shenlong.windowRectangle.Height - this.Height) / 2);
				}*/

				foreach ( XmlNode logOnNode in xmlLogOn.DocumentElement )
				{
					comboUserName.Items.Add(logOnNode[tagUserName].InnerText);	// <userName>
				}

				if ( comboUserName.Items.Count != 0 )
				{
					comboUserName.SelectedIndex = 0;

					if ( textPassword.Text.Length == 0 )
					{
						textPassword.Select();
					}
				}

				if ( Shenlong.resumeAppendLogOnHis != null )
				{
					checkAppendLogOnHis.Checked = (bool)Shenlong.resumeAppendLogOnHis;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
				toolStripStatusLabel.Text = "";
				formSize.Height -= statusStrip.Height;
			}

			this.Size = formSize;
		}

		/// <summary>
		/// LogOn_Shown
		/// </summary>
		private void LogOn_Shown(object sender, EventArgs e)
		{
			try
			{
				if ( usage == usages.resume )
				{
					if ( comboUserName.Items.Count != 0 )
					{
						buttonOK.PerformClick();
					}
				}
				else if ( (usage == usages.require) || (usage == usages.auto) )
				{
					bool found = false;
					XmlNodeList logOnList = xmlLogOn.SelectNodes("/" + tagRoot + "/" + tagLogOn);
					for ( int i = 0; i < logOnList.Count; i++ )
					{
						if ( (string.Compare(logOnList[i].Attributes[attrSID].Value, sid, true) == 0) &&
							 (string.Compare(logOnList[i][tagUserName].InnerText, userName, true) == 0) )
						{
							comboUserName.SelectedIndex = i;
							found = true;
							if ( usage == usages.auto )
							{
								buttonOK.PerformClick();
							}
							break;
						}
					}

					if ( !found )
					{
						comboUserName.Text = userName;
						textPassword.Text = "";
						textSID.Text = sid;
						toolTip.SetToolTip(textPassword, string.Empty);
					}
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// USER NAME �̑I�����ύX���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboUserName_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ( !buttonOK.Enabled )
				return;

#if true
			XmlNode logOnNode = xmlLogOn.DocumentElement.ChildNodes[comboUserName.SelectedIndex];
#else
			string xpath = "/" + tagRoot + "/" + tagLogOn + "[" + tagUserName + "='" + comboUserName.Text + "']";
			XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
#endif
			if ( logOnNode != null )
			{
				textPassword.Text = common.DecodePassword(logOnNode[tagPassword].InnerText);
				textSID.Text = logOnNode.Attributes[attrSID].Value;
				if ( Shenlong.logOnPwdToolTip )
				{
					toolTip.SetToolTip(textPassword, textPassword.Text);
				}
			}
		}

		/// <summary>
		/// [OK] �{�^���������ꂽ
		/// </summary>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{	
					// ���O�I���������S�č폜���ꂽ���ׂ̈ɁA�Ƃ肠�����ۑ����Ă���
					if ( (comboUserName.Items.Count == 0) && (xmlLogOn.DocumentElement.ChildNodes.Count == 0) )
					{
						xmlLogOn.Save(xmlLogOnFileName);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
				}

				if ( comboUserName.Text.Length == 0 )
				{
					comboUserName.Select();
					return;
				}
				if ( textPassword.Text.Length == 0 )
				{
					textPassword.Select();
					return;
				}
				if ( textSID.Text.Length == 0 )
				{
					textSID.Select();
					return;
				}

				buttonOK.Enabled = false;
#if true
				bool toLower = true;
				foreach ( char c in textPassword.Text )
				{
					if ( char.IsUpper(c) )
					{
						// �p�X���[�h�ɑ啶�����܂܂�Ă��鎞�͐ڑ����̏������ϊ������Ȃ��悤�ɂ����i2013/06/12�j
						toLower = false;
						break;
					}
				}
				if ( toLower )
				{
#endif
					textSID.Text = textSID.Text.ToLower();
					comboUserName.Text = comboUserName.Text.ToLower();
					textPassword.Text = textPassword.Text.ToLower();
#if true
				}
#endif
				toolStripStatusLabel.Text = "���O�I�����ł�...";
				VisibleStatusStrip(true);
				Application.DoEvents();
				Cursor.Current = Cursors.WaitCursor;

				string conStr = "Data Source=" + /*"(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = host name)(PORT = 1521))(CONNECT_DATA = (SID = ANS)))"*/textSID.Text + ";User Id=" + comboUserName.Text + ";Password=" + textPassword.Text;
				oraConn = new OracleConnection(conStr);
				oraConn.Open();

				string[] serverVersion = oraConn.ServerVersion.Split('\n');
				int len = serverVersion[0].IndexOf('.');
				if ( len != -1 )
				{
					int majorVer = int.Parse(serverVersion[0].Substring(0, len));
					if ( majorVer < 8 )
					{
						MyMessageBox.Show("���O�I�������I���N���̃o�[�W�����͓���ΏۊO�ł�\n�o�[�W�����W�ȏ�Ŏg�p���ĉ�����\n" + serverVersion[0], "���O�I��", MessageBoxButtons.OK, MessageBoxIcon.Warning, new Rectangle(Location, Size), 100);
						oraConn.Close();
						oraConn.Dispose();
						oraConn = null;
						return;
					}
				}

				SaveLogOnXmlFile();

				try
				{
					if ( Shenlong.resumeAppendLogOnHis != null )
					{
						Shenlong.resumeAppendLogOnHis = checkAppendLogOnHis.Checked;
						api.WritePrivateProfileString(Shenlong.SETTINGS_SECTION, Shenlong.KEY_RESUME_APPEND_LOGON_HIS, checkAppendLogOnHis.Checked.ToString().ToLower(), Shenlong.shenlongIniFileName);
					}
				}
				catch ( Exception exp )
				{
					Debug.WriteLine(exp.Message);
				}

				DialogResult = DialogResult.OK;
				this.Close();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);

				if ( oraConn != null )
				{
					oraConn.Close();
					oraConn.Dispose();
					oraConn = null;
				}
			}
			finally
			{
				if ( !buttonOK.Enabled )
				{
					VisibleStatusStrip(false);
					buttonOK.Enabled = true;
					Cursor.Current = Cursors.Default;
				}
			}
		}

		/// <summary>
		/// ���O�I���������t�@�C���ɕۑ�����
		/// </summary>
		private bool SaveLogOnXmlFile()
		{
			try
			{
				// �V���O�I���������쐬����
				XmlDocument xmlNewLogOn = new XmlDocument();
				XmlDeclaration decl = xmlNewLogOn.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlNewLogOn.AppendChild(decl);
				XmlNode newRootNode = xmlNewLogOn.CreateNode(XmlNodeType.Element, tagRoot, null);		// <root>
				xmlNewLogOn.AppendChild(newRootNode);

				// ���O�I�������ɒǉ�����H
				if ( checkAppendLogOnHis.Checked )
				{
					XmlNode newLogOnNode = xmlNewLogOn.CreateNode(XmlNodeType.Element, tagLogOn, null);	// <logOn>
					XmlAttribute attr = xmlNewLogOn.CreateAttribute(attrSID);							// @sid
					attr.Value = textSID.Text;
					newLogOnNode.Attributes.Append(attr);
					XmlElement elem = xmlNewLogOn.CreateElement(tagUserName);							// <userName>
					elem.InnerText = comboUserName.Text;
					newLogOnNode.AppendChild(elem);
					elem = xmlNewLogOn.CreateElement(tagPassword);										// <password>
					if ( checkSavePassword.Checked )
					{
						elem.InnerText = common.EncodePassword(textPassword.Text);
					}
					else
					{
						elem.IsEmpty = true;
					}
					newLogOnNode.AppendChild(elem);
					newRootNode.AppendChild(newLogOnNode);	// ���O�I������V���[�g�̒����ɒǉ�����
				}

				// �����̃��O�I��������ǉ����Ă���
				XmlNodeList logOnList = xmlLogOn.DocumentElement.ChildNodes;
				int newChildCount = xmlNewLogOn.DocumentElement.ChildNodes.Count;

				for ( int i = 0; (i < logOnList.Count) && (newChildCount < maxLogOnHistoryCount); i++ )
				{
					XmlNode logOnNode = logOnList[i];

					// ���͂��ꂽ���O�I�����͗����ɑ��݂��Ă���H
					if ( (string.Compare(textSID.Text, logOnNode.Attributes[attrSID].Value, true) == 0) &&
						 (string.Compare(comboUserName.Text, logOnNode[tagUserName].InnerText, true) == 0) &&
						 ((string.Compare(textPassword.Text, common.DecodePassword(logOnNode[tagPassword].InnerText), true) == 0) || logOnNode[tagPassword].IsEmpty) )
					{
						if ( checkAppendLogOnHis.Checked )
							continue;
						// �����̃��O�I������V���[�g�̒����Ɏ����Ă���
						newRootNode.InsertAfter(xmlNewLogOn.ImportNode(logOnNode, true), null);
					}
					else
					{
						newRootNode.AppendChild(xmlNewLogOn.ImportNode(logOnNode, true));
					}

					newChildCount++;
				}

				xmlNewLogOn.Save(xmlLogOnFileName);

				return true;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
		}

		/// <summary>
		/// LogOnDlg_KeyUp
		/// this.KeyPreview = true; �Ƃ��Ă����Ȃ��Ƃ��̃C�x���g�͔������Ȃ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LogOnDlg_KeyUp(object sender, KeyEventArgs e)
		{
			// Ctrl + D
			if ( (e.Control) && (e.KeyCode == Keys.D) )
			{
				RemoveLogOnHis();
				e.SuppressKeyPress = true;
			}
		}

		/// <summary>
		/// ���ݑI������Ă��郍�O�I���������폜����
		/// </summary>
		private void RemoveLogOnHis()
		{
			try
			{
				int selectedIndex = comboUserName.SelectedIndex;
				if ( selectedIndex == -1 )
					return;

				string xpath = "/" + tagRoot + "/" + tagLogOn + "[@" + attrSID + "='" + textSID.Text + "'][" + tagUserName + "='" + comboUserName.Text + "']";
				XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
				if ( logOnNode != null )
				{
					xmlLogOn.DocumentElement.RemoveChild(logOnNode);
				}

				comboUserName.Items.RemoveAt(selectedIndex);
				if ( comboUserName.Items.Count == 0 )
				{
					comboUserName.SelectedIndex = -1;
					comboUserName.Text = string.Empty;
					textSID.Text = string.Empty;
					textPassword.Text = string.Empty;
					toolTip.SetToolTip(textPassword, string.Empty);
				}
				else
				{
					if ( comboUserName.Items.Count <= selectedIndex )
					{
						selectedIndex--;
					}
					comboUserName.SelectedIndex = selectedIndex;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// GetLogOnPassword
		/// </summary>
		/// <param name="sid"></param>
		/// <param name="uid"></param>
		/// <returns></returns>
		public static string GetLogOnPassword(string sid, string uid)
		{
			string password = string.Empty;

			string xmlLogOnFileName = Application.StartupPath + LOGON_FILE_NAME;
			XmlDocument xmlLogOn = new XmlDocument();
			xmlLogOn.Load(xmlLogOnFileName);

			string xpath = "/" + tagRoot + "/" + tagLogOn + "[@" + attrSID + "='" + sid + "'][" + tagUserName + "='" + uid + "']";
			XmlNode logOnNode = xmlLogOn.SelectSingleNode(xpath);
			if ( logOnNode != null )
			{
				password = common.DecodePassword(logOnNode[tagPassword].InnerText);
			}

			return password;
		}

		/// <summary>
		/// textPassword_TextChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textPassword_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if ( Shenlong.logOnPwdToolTip )
				{
					toolTip.SetToolTip(textPassword, textPassword.Text);
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// checkAppendLogOnHis_CheckedChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkAppendLogOnHis_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				bool enable = checkAppendLogOnHis.Checked;
				checkSavePassword.Checked = enable;
				checkSavePassword.Enabled = enable;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
	}
}