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
using System.Text.RegularExpressions;
using CommonFunctions;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class FilePropertyDlg : Form
	{
		public string comment;
		public string author;
		public bool distinct;
		public bool useJoin;
		public int headerOutput;
		public bool download;
		public string eggPermission;
		public string maxRowNum;
		public bool setValue;
		public bool sqlSelect;
		public List<string> subQueries = null;
		public string xmlShenlongColumnFileName;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public FilePropertyDlg()
		{
			InitializeComponent();
		}

		/// <summary>
		/// FilePropertyDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FilePropertyDlg_Load(object sender, EventArgs e)
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

				textComment.Text = comment;

				textAuthor.Text = author;

				checkDistinct.Checked = distinct;
				checkUseJoin.Checked = useJoin;

				checkColumnName.Checked = (headerOutput & (int)ShenGlobal.header.columnName) != 0;
				checkComment.Checked = (headerOutput & (int)ShenGlobal.header.comment) != 0;

				if ( download )
					radioDlPermit.Checked = true;
				else
					radioDlDeny.Checked = true;

				textEggPermission.Text = eggPermission;

				textMaxRowNum.Text = maxRowNum;

				checkSetValue.Checked = setValue;

				checkSqlSelect.Checked = sqlSelect;

				if ( subQueries == null )
				{
					label5.Enabled = false;
					listBoxSubQuery.Enabled = false;
				}
#if ENABLED_SUBQUERY
				else
				{
					foreach ( string _subQuery in subQueries )
					{
						listBoxSubQuery.Items.Add(_subQuery);
					}

					listBoxSubQuery.AllowDrop = true;
					listBoxSubQuery.DrawMode = DrawMode.OwnerDrawFixed;
					this.listBoxSubQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxSubQuery_DragEnter);
					this.listBoxSubQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxSubQuery_DragDrop);
					this.listBoxSubQuery.KeyDown += new KeyEventHandler(this.listBoxSubQuery_KeyDown);
					this.listBoxSubQuery.DrawItem += new DrawItemEventHandler(this.listBoxSubQuery_DrawItem);
				}
#endif
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}
		}

		/// <summary>
		/// [OK] �{�^���������ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			comment = textComment.Text;

			author = textAuthor.Text;

			distinct = checkDistinct.Checked;
			useJoin = checkUseJoin.Checked;

			headerOutput = 0;
			headerOutput |= (checkColumnName.Checked) ? (int)ShenGlobal.header.columnName : 0;
			headerOutput |= (checkComment.Checked) ? (int)ShenGlobal.header.comment : 0;

			download = radioDlPermit.Checked;

			eggPermission = textEggPermission.Text;

			maxRowNum = textMaxRowNum.Text;

			setValue = checkSetValue.Checked;

			sqlSelect = checkSqlSelect.Checked;

			if ( subQueries != null )
			{
				subQueries = new List<string>();
				foreach ( string _subQuery in listBoxSubQuery.Items )
				{
					subQueries.Add(_subQuery);
				}
			}

			DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// textEggPermission_Validating
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textEggPermission_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				TextBox textBox = (TextBox)sender;
				if ( textBox.Text.Length == 0 )
					return;

				string errorMessage = ValidateEggPermissionFormat(textBox.Text);

				if ( errorMessage != null )
				{
					errorProvider.SetError(textBox, errorMessage);
					// e.Cancel = true�@��Cancel �� true �ɂ���Ɛ��������͂��Ȃ��Ǝ��ɍs���Ȃ��B
					e.Cancel = true;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, Shenlong.appTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// ���͂��ꂽ�^�}�S�����̏��������؂���
		/// </summary>
		/// <param name="eggPermission"></param>
		/// <returns></returns>
		private string ValidateEggPermissionFormat(string eggPermission)
		{
			string errorMessage = null;

			try
			{
				string[] permissions = eggPermission.Split(',');

				if ( permissions[0].StartsWith("NOT ", StringComparison.CurrentCultureIgnoreCase) )
				{
					permissions[0] = permissions[0].Substring(4);
				}

				foreach ( string _permission in permissions )
				{
					string permission = _permission.Trim();

					if ( permission.IndexOf('\\') != -1 )		// \\�h���C����\���[�U�[ID�w��H
					{
						if ( permission[0] == '\\' )
						{
							if ( (permission[1] != '\\') || (permission.IndexOf('\\', 2) == -1) )
								return "\\\\�h���C����\\���[�U�[ID�̏������s���ł�";
						}

						string[] domainUser = permission.TrimStart('\\').Split('\\');
						if ( domainUser.Length != 2 )
							return "\\\\�h���C����\\���[�U�[ID�̋�؂肪�s���ł�";

						for ( int i = 0; i < 2; i++ )
						{
							if ( !(new Regex(@"^[*\-0-9_A-Za-z]+$")).IsMatch(domainUser[i]) )
								return (i == 0) ? "�h���C��" : "���[�U�[" + "�����s���ł�";
							if ( (errorMessage = ValidateAsteriskFormat(domainUser[i])) != null )
								return errorMessage;
						}
					}
					else if ( Char.IsLetter(permission[0]) )	// �p�\�R�����w��H
					{
						if ( (errorMessage = ValidateAsteriskFormat(permission)) != null )
							return errorMessage;
					}
					else if ( Char.IsDigit(permission[0]) )		// IP�A�h���X�w��H
					{
						string[] ipAddresses = permission.Split('-');

						if ( ipAddresses.Length == 1 )
						{
							return "IP�A�h���X�͔͈͎w��̂�";
						}
						else if ( ipAddresses.Length != 2 )
						{
							return "IP�A�h���X�͈͎̔w�肪�������܂�";
						}
						else
						{
							foreach ( string ipAddress in ipAddresses )
							{
								string _ipAddress = ipAddress.Trim();
								if ( _ipAddress.Length == 0 )
								{
									return "���IP�A�h���X�͖����ł�";
								}
								else
								{
									string[] segments = _ipAddress.Split('.');
									if ( segments.Length != 4 )
									{
										return "�Z�O�����g�̐����s���ł�";
									}
									else
									{
										foreach ( string segment in segments )
										{
											if ( !(new Regex(@"^[0-9]+$")).IsMatch(segment) )
											{
												return "�Z�O�����g�͐����̂�";
											}
											else if ( 3 < segment.Length )
											{
												return "�Z�O�����g�͐����R���ȓ��ł�";
											}
										}
									}
								}
							}
						}

						byte[] low = System.Net.IPAddress.Parse(ipAddresses[0]).GetAddressBytes();
						byte[] high = System.Net.IPAddress.Parse(ipAddresses[1]).GetAddressBytes();
						for ( int i = 0; i < 4; i++ )
						{
							if ( low[i] <= high[i] )
								continue;
							return "IP�A�h���X�͈̔͂��s���ł�";
						}
					}
					else
					{
						return "�s���Ȓ[���̐ݒ�ł�";
					}
				}
			}
			catch ( Exception exp )
			{
				errorMessage =  exp.Message;
			}

			return errorMessage;
		}

		/// <summary>
		/// �A�X�^���X�N�̏��������؂���
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private string ValidateAsteriskFormat(string text)
		{
			int asteriskCount = 0;

			for ( int i = 0; i < text.Length; i++ )
			{
				if ( text[i] == '*' )
				{
					asteriskCount++;
				}
			}

			if ( asteriskCount != 0 )
			{
				if ( 1 < asteriskCount )
				{
					return "* �̐����������܂�";
				}
				else if ( text[text.Length - 1] != '*' )
				{
					return "* �̐ݒ�͌��̂�";
				}
			}

			return null;
		}

		/// <summary>
		/// textEggPermission_Validated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textEggPermission_Validated(object sender, EventArgs e)
		{
			try
			{
				this.errorProvider.SetError((TextBox)sender, null);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// textMaxRowNum_Validating
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textMaxRowNum_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				TextBox textBox = (TextBox)sender;
				if ( textBox.Text.Length == 0 )
					return;

				Regex regex = new Regex(@"^[0-9]+$");
				bool validate = regex.IsMatch(textBox.Text);

				if ( !validate )
				{
					errorProvider.SetError(textBox, textBox.Tag.ToString());
					// e.Cancel = true�@��Cancel �� true �ɂ���Ɛ��������͂��Ȃ��Ǝ��ɍs���Ȃ��B
					e.Cancel = true;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, Shenlong.appTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// textMaxRowNum_Validated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textMaxRowNum_Validated(object sender, EventArgs e)
		{
			try
			{
				this.errorProvider.SetError((TextBox)sender, null);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

#if ENABLED_SUBQUERY
		/// <summary>
		/// �T�u�N�G���[�p�̃N�G���[���ڃt�@�C���̃h���b�O���J�n���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxSubQuery_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if ( e.Data.GetDataPresent(DataFormats.FileDrop) )
				{
					string[] sourceFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
					if ( System.IO.Path.GetExtension(sourceFileNames[0]) == ".xml" )
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
		/// �T�u�N�G���[�p�̃N�G���[���ڃt�@�C�����h���b�O���ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxSubQuery_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				//object obj = e.Data.GetData(DataFormats.FileDrop);
				//string _xmlShenlongColumnFileName = ((string[])obj)[0];
				List<string> fileNames = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop));
				fileNames.Sort();

				foreach ( string fileName in fileNames )
				{
					// ��΃p�X�𑊑΃p�X�ɕϊ�����
					string _xmlShenlongColumnFileName = fileName.Replace((xmlShenlongColumnFileName != null ? Path.GetDirectoryName(xmlShenlongColumnFileName) : Application.StartupPath), ShenGlobal.SUBQUERY_RELATIVE_PATH);

					// �t�@�C������ '�@' �� '��' �ɕϊ����Ă���
					_xmlShenlongColumnFileName = Path.GetDirectoryName(_xmlShenlongColumnFileName) + "\\" + Path.GetFileName(_xmlShenlongColumnFileName).Replace(' ', '��');

					if ( listBoxSubQuery.Items.IndexOf(_xmlShenlongColumnFileName) != -1 )
						return;

					listBoxSubQuery.Items.Add(_xmlShenlongColumnFileName);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// listBoxSubQuery �ŃL�[�������ꂽ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxSubQuery_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if ( e.KeyCode == Keys.Delete )
				{
					if ( listBoxSubQuery.SelectedItems.Count == 0 )
						return;

					subQueries.Remove((string)listBoxSubQuery.SelectedItem);
					listBoxSubQuery.Items.Remove(listBoxSubQuery.SelectedItem);
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// listBoxSubQuery_DrawItem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBoxSubQuery_DrawItem(object sender, DrawItemEventArgs e)
		{
			try
			{
				//�w�i��`�悷��
				//���ڂ��I������Ă��鎞�͋����\�������
				e.DrawBackground();

				//ListBox����̂Ƃ���ListBox���I��������e.Index��-1�ɂȂ�
				if ( e.Index > -1 )
				{
					//������`�悷��F�̑I��
					Brush brush = null;
					if ( (e.State & DrawItemState.Selected) != DrawItemState.Selected )
					{
						brush = new SolidBrush(Color.Black);
					}
					else
					{
						//�I������Ă��鎞�͂��̂܂܂̑O�i�F���g��
						brush = new SolidBrush(e.ForeColor);
					}

					//�`�悷�镶����̎擾
					string txt = ((ListBox)sender).Items[e.Index].ToString();

					StringFormat formatText = new StringFormat();
					formatText.Trimming = StringTrimming.EllipsisPath;

					//������̕`��
					e.Graphics.DrawString(txt, e.Font, brush, e.Bounds, formatText);

					//��n��
					brush.Dispose();
				}

				//�t�H�[�J�X�������l�p�`��`��
				e.DrawFocusRectangle();
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
#endif
	}
}