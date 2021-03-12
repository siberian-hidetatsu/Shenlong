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
		/// コンストラクタ
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
				for ( int i = (sysMenuItemCount - 2) - 1; 0 <= i; i-- )	// -2:[閉じる][セパレータ]
				{
					if ( i == 1 )	// 移動？
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
		/// [OK] ボタンが押された
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
					// e.Cancel = true　でCancel を true にすると正しく入力しないと次に行けない。
					e.Cancel = true;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, Shenlong.appTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// 入力されたタマゴ権限の書式を検証する
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

					if ( permission.IndexOf('\\') != -1 )		// \\ドメイン名\ユーザーID指定？
					{
						if ( permission[0] == '\\' )
						{
							if ( (permission[1] != '\\') || (permission.IndexOf('\\', 2) == -1) )
								return "\\\\ドメイン名\\ユーザーIDの書式が不正です";
						}

						string[] domainUser = permission.TrimStart('\\').Split('\\');
						if ( domainUser.Length != 2 )
							return "\\\\ドメイン名\\ユーザーIDの区切りが不正です";

						for ( int i = 0; i < 2; i++ )
						{
							if ( !(new Regex(@"^[*\-0-9_A-Za-z]+$")).IsMatch(domainUser[i]) )
								return (i == 0) ? "ドメイン" : "ユーザー" + "名が不正です";
							if ( (errorMessage = ValidateAsteriskFormat(domainUser[i])) != null )
								return errorMessage;
						}
					}
					else if ( Char.IsLetter(permission[0]) )	// パソコン名指定？
					{
						if ( (errorMessage = ValidateAsteriskFormat(permission)) != null )
							return errorMessage;
					}
					else if ( Char.IsDigit(permission[0]) )		// IPアドレス指定？
					{
						string[] ipAddresses = permission.Split('-');

						if ( ipAddresses.Length == 1 )
						{
							return "IPアドレスは範囲指定のみ";
						}
						else if ( ipAddresses.Length != 2 )
						{
							return "IPアドレスの範囲指定が多すぎます";
						}
						else
						{
							foreach ( string ipAddress in ipAddresses )
							{
								string _ipAddress = ipAddress.Trim();
								if ( _ipAddress.Length == 0 )
								{
									return "空のIPアドレスは無効です";
								}
								else
								{
									string[] segments = _ipAddress.Split('.');
									if ( segments.Length != 4 )
									{
										return "セグメントの数が不正です";
									}
									else
									{
										foreach ( string segment in segments )
										{
											if ( !(new Regex(@"^[0-9]+$")).IsMatch(segment) )
											{
												return "セグメントは数字のみ";
											}
											else if ( 3 < segment.Length )
											{
												return "セグメントは数字３桁以内です";
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
							return "IPアドレスの範囲が不正です";
						}
					}
					else
					{
						return "不明な端末の設定です";
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
		/// アスタリスクの書式を検証する
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
					return "* の数が多すぎます";
				}
				else if ( text[text.Length - 1] != '*' )
				{
					return "* の設定は後ろのみ";
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
					// e.Cancel = true　でCancel を true にすると正しく入力しないと次に行けない。
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
		/// サブクエリー用のクエリー項目ファイルのドラッグが開始された
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
		/// サブクエリー用のクエリー項目ファイルがドラッグされた
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
					// 絶対パスを相対パスに変換する
					string _xmlShenlongColumnFileName = fileName.Replace((xmlShenlongColumnFileName != null ? Path.GetDirectoryName(xmlShenlongColumnFileName) : Application.StartupPath), ShenGlobal.SUBQUERY_RELATIVE_PATH);

					// ファイル名の '　' を '□' に変換しておく
					_xmlShenlongColumnFileName = Path.GetDirectoryName(_xmlShenlongColumnFileName) + "\\" + Path.GetFileName(_xmlShenlongColumnFileName).Replace(' ', '□');

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
		/// listBoxSubQuery でキーが押された
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
				//背景を描画する
				//項目が選択されている時は強調表示される
				e.DrawBackground();

				//ListBoxが空のときにListBoxが選択されるとe.Indexが-1になる
				if ( e.Index > -1 )
				{
					//文字を描画する色の選択
					Brush brush = null;
					if ( (e.State & DrawItemState.Selected) != DrawItemState.Selected )
					{
						brush = new SolidBrush(Color.Black);
					}
					else
					{
						//選択されている時はそのままの前景色を使う
						brush = new SolidBrush(e.ForeColor);
					}

					//描画する文字列の取得
					string txt = ((ListBox)sender).Items[e.Index].ToString();

					StringFormat formatText = new StringFormat();
					formatText.Trimming = StringTrimming.EllipsisPath;

					//文字列の描画
					e.Graphics.DrawString(txt, e.Font, brush, e.Bounds, formatText);

					//後始末
					brush.Dispose();
				}

				//フォーカスを示す四角形を描画
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