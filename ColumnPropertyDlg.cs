using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class ColumnPropertyDlg : Form
	{
		private string tableFieldName;
		public string[] property;
		private bool[] bubbPagesEnable;
		private OracleConnection oraConn;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="tableFieldName"></param>
		/// <param name="property"></param>
		/// <param name="bubPagesEnable"></param>
		/// <param name="oraConn"></param>
		public ColumnPropertyDlg(string tableFieldName, string[] property, bool[] bubbPagesEnable, OracleConnection oraConn)
		{
			InitializeComponent();

			this.tableFieldName = tableFieldName;
			this.property = property;
			this.bubbPagesEnable = bubbPagesEnable;
			this.oraConn = oraConn;
		}

		/// <summary>
		/// ColumnPropertyDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ColumnPropertyDlg_Load(object sender, EventArgs e)
		{
			try
			{
				this.Text = tableFieldName + " のプロパティ";

				// [全般] タブ
				textType.Text = property[(int)ShenGlobal.prop.type];
				textLength.Text = property[(int)ShenGlobal.prop.length];
				textNULLABLE.Text = (property[(int)ShenGlobal.prop.nullable] == Shenlong.propNotNullable) ? "NOT NULL" : "";
				textComment.Text = property[(int)ShenGlobal.prop.comment];
				textAlias.Text = property[(int)ShenGlobal.prop.alias];
				textDateFormat.Text = property[(int)ShenGlobal.prop.dateFormat];

				textType.ReadOnly = !Shenlong.editableColumnName;
				textLength.ReadOnly = !Shenlong.editableColumnName;
				labelDateFormat.Enabled = IsDateType();
				textDateFormat.Enabled = IsDateType();

				// [バブ入出力設定] タブ
				toolTip.SetToolTip(labelDropDownList, "SELECT .... FROM .... WHERE .... ORDER BY ....\r\nテキスト,値|テキスト,値|....");

				if ( property[(int)ShenGlobal.prop.bubbles].Length != 0 )
				{
					string[] setting = property[(int)ShenGlobal.prop.bubbles].Split(ShenGlobal.sepBubbSet);

					if ( setting[(int)ShenGlobal.bubbSet.control] == ShenGlobal.bubbCtrl.textBox.ToString() )
						radioTextBox.Checked = true;
					else if ( setting[(int)ShenGlobal.bubbSet.control] == ShenGlobal.bubbCtrl.label.ToString() )
						radioLabel.Checked = true;
					else if ( setting[(int)ShenGlobal.bubbSet.control] == ShenGlobal.bubbCtrl.noVisible.ToString() )
						radioNoVisible.Checked = true;

					if ( setting[(int)ShenGlobal.bubbSet.input] == ShenGlobal.bubbInput.noAppoint.ToString() )
						radioNoAppoint.Checked = true;
					else if ( setting[(int)ShenGlobal.bubbSet.input] == ShenGlobal.bubbInput.necessary.ToString() )
						radioNecessary.Checked = true;

					checkSetValue.Checked = bool.Parse(setting[(int)ShenGlobal.bubbSet.setValue]);

					textDropDownList.Text = setting[(int)ShenGlobal.bubbSet.dropDownList].Replace("<br>", "\r\n");

					textHyperLink.Text = setting[(int)ShenGlobal.bubbSet.hyperLink];

					textClassify.Text = setting[(int)ShenGlobal.bubbSet.classify];
				}

				// タブページの設定
				for ( int i = bubbPagesEnable.Length - 1; 0 <= i; i-- )
				{
					if ( bubbPagesEnable[i] )
					{
						//tabControl.SelectedIndex = i;
					}
					else
					{
						//tabControl.TabPages.RemoveAt(i);
						foreach ( Control control in tabControl.TabPages[1 + i].Controls )
						{
							control.Enabled = false;
						}
					}
				}
				tabControl.SelectedIndex = 0;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// ColumnPropertyDlg_Shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ColumnPropertyDlg_Shown(object sender, EventArgs e)
		{
#if false
			try
			{
				// IME が表示されてしまう＆テキストボックスの文字が全て選択される対策
				tabControl.Select()/*.Focus()*/;
				Control control = this.GetNextControl(this.ActiveControl, true);
				if ( control != null )
				{
					control.Select();
				}
				//this.SelectNextControl(this.ActiveControl, true, false, true, true);
				/*foreach ( Control control in tabControl.SelectedTab.Controls )
				{
					if ( control.TabIndex != 0 )
						continue;
					control.Select();
					break;
				}*/
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
#endif
		}

		/// <summary>
		/// DATE 型か否か
		/// </summary>
		/// <returns></returns>
		private bool IsDateType()
		{
			return (textType.Text == "DATE");
		}

		/// <summary>
		/// [データ型] が変更された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textType_TextChanged(object sender, EventArgs e)
		{
			labelDateFormat.Enabled = IsDateType();
			textDateFormat.Enabled = IsDateType();
		}

		/// <summary>
		/// [表示形式]
		/// radioControl_CheckedChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioControl_CheckedChanged(object sender, EventArgs e)
		{
			checkSetValue.Enabled = (radioTextBox.Checked);

			textDropDownList.Enabled = (radioTextBox.Checked);
		}

		/// <summary>
		/// [テスト] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonTest_Click(object sender, EventArgs e)
		{
			if ( textDropDownList.Text.Length == 0 )
				return;

			OracleDataAdapter oraAdapter = null;
			DataSet dataSet = null;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				StringBuilder queryOutput = new StringBuilder();
				string sql = textDropDownList.Text;

				if ( sql.StartsWith("SELECT", true, null) )
				{
					oraAdapter = new OracleDataAdapter(sql, oraConn);

					dataSet = new DataSet();
					oraAdapter.Fill(dataSet);
					DataTable dataTable = dataSet.Tables[0];

					queryOutput.Append("0: ");
					for ( int j = 0; j < dataTable.Columns.Count; j++ )
					{
						queryOutput.Append(dataTable.Columns[j] + ((j != dataTable.Columns.Count - 1) ? "," : ""));
					}
					queryOutput.Append("\r\n");

					int lo, div = 15;
					for ( lo = 0; (lo < div) && (lo < dataTable.Rows.Count); lo++ )
					{
						queryOutput.Append((lo + 1) + ": ");
						for ( int j = 0; j < dataTable.Columns.Count; j++ )
						{
							queryOutput.Append(dataTable.Rows[lo][j].ToString() + ((j != dataTable.Columns.Count - 1) ? "," : ""));
						}
						queryOutput.Append("\r\n");
					}

					if ( lo < dataTable.Rows.Count )
					{
						int hi = Math.Max(lo + 1, dataTable.Rows.Count - div);
						if ( lo + 1 < hi )
						{
							queryOutput.Append("……\r\n……\r\n……\r\n");
						}
						for ( ; hi < dataTable.Rows.Count; hi++ )
						{
							queryOutput.Append((hi + 1) + ": ");
							for ( int j = 0; j < dataTable.Columns.Count; j++ )
							{
								queryOutput.Append(dataTable.Rows[hi][j].ToString() + ((j != dataTable.Columns.Count - 1) ? "," : ""));
							}
							queryOutput.Append("\r\n");
						}
					}
				}
				else
				{
					string[] items = sql.Replace("\r\n", "").Split('|');
					for ( int i = 0; i < items.Length; i++ )
					{
						string[] item = items[i].Split(',');
						queryOutput.Append((i + 1) + ": " + item[0] + "," + ((item.Length == 1) ? item[0] : string.Join(",", item, 1, item.Length - 1)) + "\r\n");
					}
				}

				Cursor.Current = Cursors.Default;

				MessageBox.Show(queryOutput.ToString(), "テスト結果");
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				if ( dataSet != null )
				{
					dataSet.Dispose();
					dataSet = null;
				}

				if ( oraAdapter != null )
				{
					oraAdapter.Dispose();
					oraAdapter = null;
				}
			}
		}

		/// <summary>
		/// [OK] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			if ( textType.Text.Length != 0 )
			{
				property[(int)ShenGlobal.prop.type] = textType.Text;
			}

			if ( textLength.Text.Length != 0 )
			{
				property[(int)ShenGlobal.prop.length] = textLength.Text;
			}

			property[(int)ShenGlobal.prop.comment] = textComment.Text;
			if ( property[(int)ShenGlobal.prop.comment].Length == 0 )
			{
				property[(int)ShenGlobal.prop.comment] = ShenGlobal.propNoComment;
			}

			property[(int)ShenGlobal.prop.alias] = textAlias.Text.Trim('\"');

			property[(int)ShenGlobal.prop.dateFormat] = textDateFormat.Text;

			string bubbles = string.Empty;
			if ( !IsDefaultSetting() )
			{
				string control = radioTextBox.Checked ? ShenGlobal.bubbCtrl.textBox.ToString() : radioLabel.Checked ? ShenGlobal.bubbCtrl.label.ToString() : ShenGlobal.bubbCtrl.noVisible.ToString();
				string input = radioNoAppoint.Checked ? ShenGlobal.bubbInput.noAppoint.ToString() : ShenGlobal.bubbInput.necessary.ToString();
				string setValue = checkSetValue.Checked.ToString().ToLower();

				// enum bubbSet の順に格納する
				bubbles = control + ShenGlobal.sepBubbSet +
						  input + ShenGlobal.sepBubbSet +
						  setValue + ShenGlobal.sepBubbSet +
						  textDropDownList.Text.Replace("\r\n", "<br>") + ShenGlobal.sepBubbSet +
						  textHyperLink.Text + ShenGlobal.sepBubbSet +
						  textClassify.Text;
			}
			property[(int)ShenGlobal.prop.bubbles] = bubbles;
		}

		/// <summary>
		/// デフォルトの設定か否か
		/// </summary>
		/// <returns></returns>
		private bool IsDefaultSetting()
		{
			return (radioTextBox.Checked &&
					radioNoAppoint.Checked &&
					!checkSetValue.Checked &&
					(textDropDownList.Text.Length == 0) &&
					(textHyperLink.Text.Length == 0) &&
					(textClassify.Text.Length == 0));
		}
	}
}
