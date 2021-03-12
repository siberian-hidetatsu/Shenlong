using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using CommonFunctions;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class ParamInputDlg : Form
	{
		private XmlDocument xmlShenlongColumn = null;
		private string shenColumnBaseURI = null;
		private Dictionary<string, string> latestSelectParams = null;
		private string commonPassword = null;							// null 以外の時は shencmd から呼ばれた

		private const string SETTINGS_SECTION = "Settings";				// [Settings] セクション
		public const string KEY_MAX_INPUT_PARAM_HISTORY_COUNT = "MaxInputParamHistoryCount";	// 抽出条件ダイアログの入力履歴の最大数

		public const string pmShenlongTextID = "_Text";
		public const string pmShenlongTextIdJoin = "."/*"_"*/;		// shencmd でパラメータを解析するときに baseURI 付きか否かを判断するので "." にしている
		public const string pmShenlongTextIdNo = "#"/*"_"*/;
		private const string pmShenlongLabelID = "_Label";
		private const string SPACE = " ";

		private readonly string selectParamIniFileName = Application.StartupPath + "\\" + "~selectparam.ini";
		private const string KEY_FORM_SIZE = "FormSize";
		private const string KEY_SHEN_VALUE = "ShenValue";

		private string shenlongColumnName = "nonBaseURI";

		public Dictionary<string, string> selectParams = null;		// 選択可能な項目に入力されたパラメータ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="_xmlShenlongColumn"></param>
		/// <param name="_shenColumnBaseURI"></param>
		/// <param name="_latestSelectParams"></param>
		/// <param name="_commonPassword"></param>
		public ParamInputDlg(XmlDocument _xmlShenlongColumn, string _shenColumnBaseURI, Dictionary<string, string> _latestSelectParams, string _commonPassword, bool _showInTaskbar)
		{
			InitializeComponent();

			try
			{
				Cursor.Current/*this.Cursor*/ = Cursors.WaitCursor;

				//this.MinimumSize = new Size(this.Width, this.Height);
				this.MinimumSize = new Size((int)(this.Width * 0.7), (int)(this.Height * 0.6));

				toolStripReloadValue.Enabled = false;
				flowLayoutPanel.Controls.Clear();

				if ( _xmlShenlongColumn == null )
				{
					this.Close();
					return;
				}

				xmlShenlongColumn = _xmlShenlongColumn;
				shenColumnBaseURI = _shenColumnBaseURI;
				latestSelectParams = _latestSelectParams;
				commonPassword = _commonPassword;

				groupBoxInputControl.Anchor |= (AnchorStyles.Right | AnchorStyles.Bottom);

				if ( !string.IsNullOrEmpty(_shenColumnBaseURI) )
				{
					shenlongColumnName = Path.GetFileNameWithoutExtension(_shenColumnBaseURI);
					this.Text = shenlongColumnName + " - " + this.Text;
				}

				if ( _showInTaskbar )	// shencmd から呼ばれた？
				{
					toolStripShenValue.Checked = false;
				}

				StringBuilder returnedString = new StringBuilder(1024);
				// フォームのサイズ
				if ( api.GetPrivateProfileString(shenlongColumnName, KEY_FORM_SIZE, "", returnedString, (uint)returnedString.Capacity, selectParamIniFileName) != 0 )
				{
					string[] formSize = returnedString.ToString().Split(',');
					Size size = new Size(int.Parse(formSize[0]), int.Parse(formSize[1]));
					//if ( this.Size.Width < size.Width || this.Size.Height < size.Height )
					if ( this.MinimumSize.Width <= size.Width || this.MinimumSize.Height <= size.Height )
					{
						this.Size = size;
					}
				}
				// toolStripShenValue のチェック状態
				if ( api.GetPrivateProfileString(shenlongColumnName, KEY_SHEN_VALUE, toolStripShenValue.Checked.ToString(), returnedString, (uint)returnedString.Capacity, selectParamIniFileName) != 0 )
				{
					toolStripShenValue.Checked = bool.Parse(returnedString.ToString());
				}

				/*if ( !string.IsNullOrEmpty(commonPassword) )
				{
					ShowInTaskbar = true;
					StartPosition = FormStartPosition.CenterScreen;
				}*/
				this.ShowInTaskbar = _showInTaskbar;

#if false
				AppendShenlongParamControl(xmlShenlongColumn);

#if ENABLED_SUBQUERY
				XmlNode fileProperty = xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty];
				if ( fileProperty != null )
				{
					if ( (fileProperty[ShenGlobal.tagSubQuery] != null) && (fileProperty[ShenGlobal.tagSubQuery].InnerText.Length != 0) )
					{
						foreach ( string subQuery in fileProperty[ShenGlobal.tagSubQuery].InnerText.Split(ShenGlobal.SUBQUERY_SEPARATOR) )
						{
							AppendShenlongParamControl(ShenGlobal.ReadSubQueryFile(subQuery, shenColumnBaseURI));
						}
					}
				}
#endif
#else
				AppendShenlongParamControlForBase(xmlShenlongColumn);
#endif

				if ( flowLayoutPanel.Controls.Count == 0 )
				{
					this.Close();
					return;
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				this.Close();
			}
			finally
			{
				Cursor.Current/*this.Cursor*/ = Cursors.WaitCursor;
			}
		}

		/*/// <summary>
		/// ProcessCmdKey
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ( (int)keyData == (int)Keys.F5 )
			{
				buttonOK.PerformClick();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}*/
	
		/// <summary>
		/// ParamInputDlg_KeyUp
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParamInputDlg_KeyUp(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.F5 )
			{
				buttonOK.PerformClick();
			}
			else if ( (e.KeyCode == Keys.R) && e.Control )
			{
				toolStripReloadValue.PerformClick();
			}
		}

		/// <summary>
		/// ParamInptDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParamInputDlg_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// ParamInputDlg_Shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParamInputDlg_Shown(object sender, EventArgs e)
		{
			try
			{
				//if ( !string.IsNullOrEmpty(commonPassword) )
				if ( this.Owner == null )
				{
					this.TopMost = true;
					Activate();
					this.TopMost = false;
				}

				buttonOK.Enabled = false;
				buttonCancel.Enabled = false;
				Application.DoEvents();

				Cursor.Current = Cursors.WaitCursor;

				foreach ( Control control in flowLayoutPanel.Controls )
				{
					if ( control is Label )
					{
						if ( (control.Text.Length == 0) || (control.Text == "*") )
							continue;
						Size size = control.Size;
						control.AutoSize = false;
						control.Size = new Size(size.Width, 25);
						((Label)control).TextAlign = ContentAlignment.MiddleLeft;
					}
					else if ( control is ComboBox )
					{
						SetComboBox((ComboBox)control);
					}
				}

				byte[] returnedByte = new byte[0xffff];
				int count = (int)api.GetPrivateProfileSection(shenlongColumnName, returnedByte, (uint)returnedByte.Length, selectParamIniFileName);
#if false
				toolStripReloadValue.Enabled = (count != 0);
#else
				if ( count != 0 )
				{
					foreach ( string key in Encoding.GetEncoding("Shift_JIS").GetString(returnedByte, 0, count - 1).Split('\0') )
					{
						if ( key[0] == '_' )
						{
							toolStripReloadValue.Enabled = true;
							break;
						}
					}
				}
#endif
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			finally
			{
				buttonOK.Enabled = true;
				buttonCancel.Enabled = true;
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// AppendShenlongParamControlForBase
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		private void AppendShenlongParamControlForBase(XmlDocument xmlShenlongColumn)
		{
			AppendShenlongParamControl(xmlShenlongColumn);

#if ENABLED_SUBQUERY
			XmlNode fileProperty = xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty];

			if ( fileProperty == null )
				return;

			if ( (fileProperty[ShenGlobal.tagSubQuery] == null) || (fileProperty[ShenGlobal.tagSubQuery].InnerText.Length == 0) )
				return;

			foreach ( string subQuery in fileProperty[ShenGlobal.tagSubQuery].InnerText.Split(ShenGlobal.SUBQUERY_SEPARATOR) )
			{
				xmlShenlongColumn = ShenGlobal.ReadSubQueryFile(subQuery, shenColumnBaseURI);
				AppendShenlongParamControlForBase(xmlShenlongColumn);
			}
#endif
		}

		/// <summary>
		/// shenlong のパラメータ箇所をテキスト コントロールとして追加する
		/// </summary>
		/// <param name="xmlShenlongColumn"></param>
		/// <returns></returns>
		private int AppendShenlongParamControl(XmlDocument xmlShenlongColumn)
		{
			string baseURI = string.Empty;
			Dictionary<string, int> paramNames = new Dictionary<string, int>();
			int shenlongParamCount = 0;
			Label lastLabelRColOp = null;

			if ( !string.IsNullOrEmpty(xmlShenlongColumn.BaseURI) )
			{
				baseURI = Path.GetFileNameWithoutExtension(xmlShenlongColumn.BaseURI);
			}

			if ( flowLayoutPanel.Controls.Count != 0 )
			{
				flowLayoutPanel.SetFlowBreak(flowLayoutPanel.Controls[flowLayoutPanel.Controls.Count - 1], true);

				/*Label label = new Label();
				label.AutoSize = true;
				flowLayoutPanel.Controls.Add(label);
				flowLayoutPanel.SetFlowBreak(label, true);*/

				/*label = new Label();
				label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				label.Name = pmShenlongLablID + xmlShenlongColumn.BaseURI;
				label.Size = new System.Drawing.Size(256, 2);
				flowLayoutPanel.Controls.Add(label);*/

				//AppendLabel(baseURI, baseURI + " ====", SystemColors.WindowText, null, true);
			}

			if ( !string.IsNullOrEmpty(baseURI) )
			{
				AppendLabel(baseURI, baseURI + " ====", SystemColors.WindowText, null, true);
			}

			XmlNode fileProperty = xmlShenlongColumn.DocumentElement[ShenGlobal.tagProperty];

			string xpath = "/" + ShenGlobal.tagShenlong + "/" + ShenGlobal.tagColumn + "[" + ShenGlobal.qc.expression.ToString() + "!='']";
			XmlNodeList columnWithExpression = xmlShenlongColumn.SelectNodes(xpath);

			foreach ( XmlNode column in columnWithExpression/*xmlShenlongColumn.DocumentElement.SelectNodes(tagColumn)*/ )
			{
				string expression = column[ShenGlobal.qc.expression.ToString()].InnerText;
				/*if ( expression.Length == 0 )
					continue;*/

				string tableName = ShenGlobal.GetTableName(column.Attributes[ShenGlobal.attrTableName].Value, false);
				string fieldName = column[ShenGlobal.qc.fieldName.ToString()].InnerText;
				string comment = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.comment.ToString()].InnerText;
				bool withComment = (comment != ShenGlobal.propNoComment);
				XmlNode bubbles = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.bubbles.ToString()];
				ShenGlobal.bubbCtrl bubbCtrl = ShenGlobal.bubbCtrl.textBox;

				if ( bubbles != null )
				{
					string control = bubbles.Attributes[ShenGlobal.bubbSet.control.ToString()].Value;
					if ( control == ShenGlobal.bubbCtrl.noVisible.ToString() )
						continue;
					else if ( control == ShenGlobal.bubbCtrl.textBox.ToString() )
						bubbCtrl = ((bubbles[ShenGlobal.bubbSet.dropDownList.ToString()] != null && bubbles[ShenGlobal.bubbSet.dropDownList.ToString()].InnerText.Length == 0)/* || (bubbles["dropDownSql"] != null && bubbles["dropDownSql"].InnerText.Length == 0)*/) ? ShenGlobal.bubbCtrl.textBox : ShenGlobal.bubbCtrl.dropDownList;
					else if ( control == ShenGlobal.bubbCtrl.label.ToString() )
						bubbCtrl = ShenGlobal.bubbCtrl.label;
				}

#if NEW_GETPLAINTABLEFIELDNAME
				string fieldAliasName;
				string plainFieldName = ShenGlobal.GetPlainTableFieldName(fieldName, out fieldAliasName);
#else
				int fieldAsIndex;
				string plainFieldName = ShenGlobal.GetPlainTableFieldName(fieldName, out fieldAsIndex);
#endif
				string plainTableFieldName = tableName + "."/*pmShenlongTextIdJoin*/ + plainFieldName;

				int sameParamNo = 0;
				if ( !paramNames.TryGetValue(plainTableFieldName, out sameParamNo) )
				{
					paramNames[plainTableFieldName] = sameParamNo;
				}
				else
				{
					sameParamNo = ++paramNames[plainTableFieldName];
				}

				string columnName = plainFieldName;
#if NEW_GETPLAINTABLEFIELDNAME
				if ( fieldAliasName != null )
				{
					columnName = fieldAliasName.Trim(" \"".ToCharArray());
#else
				if ( fieldAsIndex != -1 )
				{
					columnName = fieldName.Substring(fieldAsIndex + 4).Trim(" \"".ToCharArray());
#endif
				}
				else
				{
					XmlNode alias = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.alias.ToString()];
					if ( alias != null )
					{
#if NEW_GETPLAINTABLEFIELDNAME
						fieldAliasName = alias.InnerText;
						columnName = fieldAliasName;
#else
						columnName = alias.InnerText;
#endif
					}
					else
					{
						if ( withComment )
						{
							columnName = comment;
						}
					}
				}

				if ( flowLayoutPanel.Controls.Count != 0 )
				{
					flowLayoutPanel.SetFlowBreak(flowLayoutPanel.Controls[flowLayoutPanel.Controls.Count - 1], true);
				}

				string value1 = column[ShenGlobal.qc.value1.ToString()].InnerText.Trim();
				string value2 = column[ShenGlobal.qc.value2.ToString()].InnerText.Trim();

				Label label;
				string usersRoundBlanket = ShenGlobal.GetUsersRoundBlanket(ref value2);
				string _text = (usersRoundBlanket != null && usersRoundBlanket[0] == '(' ? usersRoundBlanket : "") +
							   columnName + SPACE + expression/* + SPACE*/;
				string _name = baseURI + pmShenlongTextIdJoin + plainTableFieldName + pmShenlongTextIdNo + sameParamNo;
				string _toolTipText = (withComment || (fieldAliasName != null)/*(fieldAsIndex != -1)*/) ? tableName + "." + plainFieldName/*tableFieldName*/: null;
				label = AppendLabel(_name, _text, SystemColors.WindowText, _toolTipText, false);

				bool necessary = false;

				//string value1 = column[ShenGlobal.qc.value1.ToString()].InnerText.Trim();

				if ( value1.Length != 0 )
				{
					bool setValue = false;

					if ( bubbCtrl == ShenGlobal.bubbCtrl.textBox )
					{
						if ( fileProperty[ShenGlobal.tagSetValue] != null )
						{
							setValue = bool.Parse(fileProperty[ShenGlobal.tagSetValue].InnerText);
						}
						if ( (!setValue) && (bubbles != null) && (bubbles.Attributes[ShenGlobal.bubbSet.setValue.ToString()] != null) )
						{
							setValue = bool.Parse(bubbles.Attributes[ShenGlobal.bubbSet.setValue.ToString()].Value);
						}

						string controlName = baseURI + pmShenlongTextIdJoin + plainTableFieldName + pmShenlongTextIdNo + sameParamNo;
						TextBox textBox = AppendTextBox(controlName, value1, setValue, column);

						AdjustTextBoxStatus(textBox, bubbles, column, ref necessary);
					}
					else if ( bubbCtrl == ShenGlobal.bubbCtrl.label )
					{
						label = AppendLabel(null, value1, SystemColors.WindowText, null, false);
					}
					else if ( bubbCtrl == ShenGlobal.bubbCtrl.dropDownList )
					{
						_name = baseURI + pmShenlongTextIdJoin + plainTableFieldName + pmShenlongTextIdNo + sameParamNo;
						AppendComboBox(_name, value1, (bubbles[ShenGlobal.bubbSet.dropDownList.ToString()] != null) ? bubbles[ShenGlobal.bubbSet.dropDownList.ToString()].InnerText : bubbles["dropDownSql"].InnerText);
					}

					if ( expression == "BETWEEN" )
					{
						if ( bubbCtrl == ShenGlobal.bubbCtrl.dropDownList )
						{
							label.Text = label.Text.Replace(SPACE + "BETWEEN", SPACE + "=");
						}
						else
						{
							label = AppendLabel(null, SPACE + "AND" + SPACE, SystemColors.WindowText, null, false);
						}

						//string value2 = column[ShenGlobal.qc.value2.ToString()].InnerText.Trim();

						if ( bubbCtrl == ShenGlobal.bubbCtrl.textBox )
						{
							string controlName = baseURI + pmShenlongTextIdJoin + plainTableFieldName + pmShenlongTextIdNo + sameParamNo + "HI";
							TextBox textBox = AppendTextBox(controlName, value2, setValue, column);

							AdjustTextBoxStatus(textBox, bubbles, column, ref necessary);
						}
						else if ( bubbCtrl == ShenGlobal.bubbCtrl.label )
						{
							label = AppendLabel(null, value2, SystemColors.WindowText, null, false);
						}
					}

					shenlongParamCount++;
				}
				else
				{
					/**/
					if ( sameParamNo == 0 )
					{
						paramNames.Remove(plainTableFieldName);
					}
					else
					{
						paramNames[plainTableFieldName]--;
					}

					if ( expression.IndexOf("NULL") == -1 )			// IS [NOT] NULL 以外？
					{
						flowLayoutPanel.Controls.Remove(label);
						continue;
					}
					/**/
				}

				if ( necessary )
				{
					label = AppendLabel("Necessary" + shenlongParamCount, "*", Color.Red, null, false);
				}

				string rColOp = column[ShenGlobal.qc.rColOp.ToString()].InnerText;
				_text = (usersRoundBlanket != null && usersRoundBlanket[0] == ')' ? usersRoundBlanket : "") +
						SPACE + ((rColOp.Length != 0) ? rColOp : "AND");
				_name = "RColOp" + plainTableFieldName + pmShenlongTextIdNo + sameParamNo;
				label = AppendLabel(_name, _text, SystemColors.WindowText, null, false);
				lastLabelRColOp = label;
			}

			if ( lastLabelRColOp != null )	// 最後にラベル化された右列連結がある？
			{
				string text = lastLabelRColOp.Text;
				if ( text.EndsWith(SPACE + "AND") )
					lastLabelRColOp.Text = text.Substring(0, text.Length - 4);
				else if ( text.EndsWith(SPACE + "OR") )
					lastLabelRColOp.Text = text.Substring(0, text.Length - 3);
			}

			if ( (shenlongParamCount == 0) && !string.IsNullOrEmpty(baseURI) )	// パラメータが無しで、baseURI がラベルとして存在すれば削除する
			{
				Control control = flowLayoutPanel.Controls[pmShenlongLabelID + baseURI];
				if ( control != null )
				{
					flowLayoutPanel.Controls.Remove(control);
				}
			}

			return shenlongParamCount;
		}

		/// <summary>
		/// ラベルを追加する
		/// </summary>
		/// <param name="text"></param>
		/// <param name="name"></param>
		/// <param name="foreColor"></param>
		/// <param name="toolTipText"></param>
		/// <param name="flowBreak"></param>
		/// <returns></returns>
		private Label AppendLabel(string controlName, string text, Color foreColor, string toolTipText, bool flowBreak)
		{
			Label label = new Label();
			label.Text = text;
			label.Name = pmShenlongLabelID + controlName;
			label.ForeColor = foreColor;
			label.AutoSize = true;

			flowLayoutPanel.Controls.Add(label);

			if ( flowBreak )
			{
				flowLayoutPanel.SetFlowBreak(label, true);
			}

			if ( toolTipText != null )
			{
				toolTip.SetToolTip(label, toolTipText);
			}

			return label;
		}

		/// <summary>
		/// テキストボックスを追加する
		/// </summary>
		/// <param name="controlName"></param>
		/// <param name="value"></param>
		/// <param name="setValue"></param>
		/// <param name="column"></param>
		private TextBox AppendTextBox(string controlName, string value, bool setValue, XmlNode column)
		{
			controlName = pmShenlongTextID + controlName;

			string type = column[ShenGlobal.qc.fieldName.ToString()].Attributes[ShenGlobal.prop.type.ToString()].Value;

			if ( /*setValue && */(type == "DATE") )
			{
				XmlNode dateFormat = column[ShenGlobal.qc.property.ToString()][ShenGlobal.prop.dateFormat.ToString()];
				if ( AppendDateTimePicker(controlName, value, (dateFormat == null) ? ShenGlobal.sqlDateFormat : dateFormat.InnerText) )
					return null;
			}

			TextBox textBox = new TextBox();
			textBox.Text = (setValue) ? value : string.Empty;
			textBox.Name = controlName;
			toolTip.SetToolTip(textBox, /*(setValue) ? string.Empty : */value);
			flowLayoutPanel.Controls.Add(textBox);

			string _value;
			if ( (latestSelectParams != null) && latestSelectParams.TryGetValue(controlName, out _value) )
			{
				textBox.Text = _value;
			}

			return textBox;
		}

		/// <summary>
		/// テキストボックスの状態を設定に合わせて調整する
		/// </summary>
		/// <param name="textBox"></param>
		/// <param name="bubbles"></param>
		/// <param name="column"></param>
		/// <param name="necessary"></param>
		private void AdjustTextBoxStatus(TextBox textBox, XmlNode bubbles, XmlNode column, ref bool necessary)
		{
			if ( textBox == null )
				return;

			if ( (bubbles != null) && (bubbles.Attributes[ShenGlobal.bubbSet.input.ToString()] != null) &&
				 (bubbles.Attributes[ShenGlobal.bubbSet.input.ToString()].Value == ShenGlobal.bubbInput.necessary.ToString()) )
			{
				textBox.Validated += new System.EventHandler(this.textBox_Validated);
				textBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
				necessary = true;
			}

			try
			{
				string length = column[ShenGlobal.qc.fieldName.ToString()].Attributes[ShenGlobal.prop.length.ToString()].Value;
				int _length = (int)(float.Parse(length.Split(',')[0]) * 0.8F);
				int width = (int)Math.Min(this.MaximumSize.Width * 0.8F, this.CreateGraphics().MeasureString(new string('O', _length), this.Font).Width);
				textBox.Width = Math.Max(textBox.Width, width);

				if ( textBox.Text.Length != 0 )
				{
					width = (int)this.CreateGraphics().MeasureString(textBox.Text, this.Font).Width;
					textBox.Width = Math.Max(textBox.Width, width);
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// DateTimePicker として追加する
		/// </summary>
		/// <param name="controlName"></param>
		/// <param name="value"></param>
		/// <param name="dateFormat"></param>
		/// <returns></returns>
		private bool AppendDateTimePicker(string controlName, string value, string dateFormat)
		{
			DateTimePicker dateTimePicker = null;

			try
			{
				string colon = (dateFormat.IndexOf(":") == -1) ? "" : ":";
				bool usersSpecifyTime = ((value.IndexOf("' 00" + colon + "00'") != -1) || (value.IndexOf(" hh24" + colon + "mi'") != -1)) ||
										((value.IndexOf("' 00" + colon + "00" + colon + "00'") != -1) || (value.IndexOf(" hh24" + colon + "mi" + colon + "ss'") != -1));

				string _value;
				if ( (latestSelectParams != null) && latestSelectParams.TryGetValue(controlName, out _value) )
				{
					value = _value;
				}

				if ( value.IndexOf("sysdate", StringComparison.CurrentCultureIgnoreCase) != -1 )
				{
					OracleConnection oleConn = null;
					OracleCommand oleCmd = null;
					OracleDataReader oleReader = null;

					try
					{
						string sid = xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrSID].Value;
						string uid = xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrUserName].Value;
						string password = GetPassword(sid, uid);

						oleConn = OpenOracle(sid, uid, password);
						/*string toChar = (value[0] == '(') ? "to_char" : "";
						string dateQuote = (Char.IsDigit(value[0])) ? "'" : "";
						value = "to_date(" + toChar + dateQuote + value + dateQuote + ",'" + ShenGlobal.sqlDateFormat + "')";*/
						_value = ShenGlobal.ValueToDateFormat(value, dateFormat);
						string sql = "SELECT " + _value + " FROM DUAL";
						oleCmd = new OracleCommand(sql, oleConn);
						oleReader = oleCmd.ExecuteReader();
						if ( oleReader.Read() )
						{
							value = oleReader[0].ToString();

							if ( value.EndsWith(" 0:00:00") || value.EndsWith("00:00:00") )
							{
								if ( !usersSpecifyTime )	// ユーザーからの明示的な時間指定が無ければ、日付のみにする
								{
									value = value.Split(' ')[0];
								}
							}
						}
					}
					finally
					{
						CloseOracle(ref oleConn, ref oleCmd, ref oleReader);
					}
				}
				else if ( value[0] == '@' )
				{
					if ( value == "@TODAY" )
					{
						value = DateTime.Today.ToString("yyyy/MM/dd");
					}
					else if ( value == "@NOW" )
					{
						usersSpecifyTime = true;
						value = DateTime.Now.ToString("yyyy/MM/dd HH:mm" + (dateFormat.IndexOf("ss") == -1 ? "" : ":ss"));
					}
				}

				dateTimePicker = new DateTimePicker();
				dateTimePicker.Tag = (dateFormat.IndexOf('/') == -1) ? "yyyyMMdd" : "yyyy/MM/dd";	// 入力された日時を値に変換するときの書式を入れておく
				int width = 120;

				string[] dateTime = value.Split(' ');
				for ( int i = 0; i < dateTime.Length; dateTime[i] = dateTime[i].Trim(), i++ ) ;

				if ( (new Regex(@"\d{8}")).IsMatch(dateTime[0]) )		// YYYYMMDD ?
				{
					dateTime[0] = dateTime[0].Substring(0, 4) + "/" + dateTime[0].Substring(4, 2) + "/" + dateTime[0].Substring(6, 2);
				}
				value = dateTime[0];

				if ( dateTime.Length == 2 )	// 時間あり？
				{
					if ( (new Regex(@"\d{4}")).IsMatch(dateTime[1]) )	// HHMM ?
					{
						string _dateTime = dateTime[1];
						dateTime[1] = _dateTime.Substring(0, 2) + ":" + _dateTime.Substring(2, 2);

						if ( (new Regex(@"\d{6}")).IsMatch(_dateTime) )	// HHMMSS ?
						{
							dateTime[1] += ":" + _dateTime.Substring(4, 2);
						}
					}
					value += " " + dateTime[1];

					dateTimePicker.Format = DateTimePickerFormat.Custom;
					//dateTimePicker.CustomFormat = "yyyy年MM月dd日 HH時mm分";
					dateTimePicker.CustomFormat = "yyyy年M月d日 HH時mm分";
					dateTimePicker.Tag += " " + ((dateFormat.IndexOf(':') == -1) ? "HHmm" : "HH:mm");
					width += 40;

					if ( dateFormat.EndsWith("ss", StringComparison.CurrentCultureIgnoreCase) )	// 秒がある？
					{
						dateTimePicker.CustomFormat += "ss秒";
						width += 25;
						dateTimePicker.Tag += (dateFormat.IndexOf(':') == -1) ? "ss" : ":ss";
					}
				}

				dateTimePicker.Name = controlName;
				dateTimePicker.Size = new Size(width, dateTimePicker.Height);
				dateTimePicker.Value = DateTime.Parse(value);
				flowLayoutPanel.Controls.Add(dateTimePicker);
				return true;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
				if ( dateTimePicker != null )
				{
					dateTimePicker.Dispose();
					dateTimePicker = null;
				}
				return false;
			}
		}

		/// <summary>
		/// オラクル接続用のパスワードを取得する
		/// </summary>
		/// <param name="sid"></param>
		/// <param name="uid"></param>
		/// <returns></returns>
		private string GetPassword(string sid, string uid)
		{
			if ( !string.IsNullOrEmpty(commonPassword) )
				return commonPassword;

			string password = LogOnDlg.GetLogOnPassword(sid, uid);
			return password;
		}

		/// <summary>
		/// 文字の出現回数をカウント
		/// </summary>
		/// <param name="s"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int CountChar(string s, char c)
		{
			int count = 0;
			for ( int i = 0; i < s.Length; i++ )
			{
				if ( s[i] == c )
				{
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// ComboBox を追加する
		/// </summary>
		/// <param name="controlName"></param>
		/// <param name="value"></param>
		/// <param name="dropDownList"></param>
		private void AppendComboBox(string controlName, string value, string dropDownList)
		{
			ComboBox comboBox = new ComboBox();
			comboBox.Name = pmShenlongTextID  + controlName;
			comboBox.Tag = dropDownList;
			toolTip.SetToolTip(comboBox, value);
			flowLayoutPanel.Controls.Add(comboBox);
		}

		/// <summary>
		/// ComboBox に項目をセットする
		/// </summary>
		/// <param name="comboBox"></param>
		private void SetComboBox(ComboBox comboBox)
		{
			OracleConnection oleConn = null;
			OracleCommand oleCmd = null;
			OracleDataReader oleReader = null;
			Graphics ds = null;

			try
			{
				string _value = null;
				if ( latestSelectParams != null )
				{
					latestSelectParams.TryGetValue(comboBox.Name, out _value);
				}

				string dropDownList = (string)comboBox.Tag;

				//GDI+ 描画面を作成して、文字列の幅を測定します
				ds = this.CreateGraphics();

				//最大幅の項目の値を保持する Float 変数
				float maxWidth = 0;

				if ( dropDownList.StartsWith("SELECT", true, null) )
				{
					string sid = xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrSID].Value;
					string uid = xmlShenlongColumn.DocumentElement.Attributes[ShenGlobal.attrUserName].Value;
					string password = GetPassword(sid, uid);

					if ( string.IsNullOrEmpty(password) )
					{
						comboBox.Items.Add("[ERROR]パスワード未登録");
						return;
					}

					oleConn = OpenOracle(sid, uid, password);

					string sql = dropDownList.Replace("<br>", " ");
					oleCmd = new OracleCommand(sql, oleConn);
					oleReader = oleCmd.ExecuteReader();

					while ( oleReader.Read() )								// １行ずつ読み込む
					{
						string text = oleReader[0].ToString();
						string value = (2 <= oleReader.FieldCount) ? oleReader[1].ToString() : text;
						ShenComboBoxItem scbItem = new ShenComboBoxItem(text, value);
						comboBox.Items.Add(scbItem);
						if ( (comboBox.SelectedItem == null) && (value == _value) )
						{
							comboBox.SelectedItem = scbItem;
						}
						maxWidth = Math.Max(maxWidth, ds.MeasureString(scbItem.ToString(), this.Font).Width);
					}
				}
				else
				{
					string[] items = dropDownList.Replace("<br>", "").Split('|');
					for ( int i = 0; i < items.Length; i++ )
					{
						string[] item = items[i].Split(',');
						string text = item[0];
						string value = (item.Length == 1) ? text : string.Join(",", item, 1, item.Length - 1);
						ShenComboBoxItem scbItem = new ShenComboBoxItem(text, value);
						comboBox.Items.Add(scbItem);
						if ( (comboBox.SelectedItem == null) && (value == _value) )
						{
							comboBox.SelectedItem = scbItem;
						}
						maxWidth = Math.Max(maxWidth, ds.MeasureString(scbItem.ToString(), this.Font).Width);
					}
				}

				if ( (comboBox.SelectedItem == null) && (comboBox.Items.Count != 0) )
				{
					comboBox.SelectedIndex = 0;
				}

				//空白文字用のバッファを
				//テキストに追加します
				maxWidth += 20/*30*/;

				//maxWidth を四捨五入して int にキャストします
				int newWidth = (int)Decimal.Round((decimal)maxWidth, 0);

				//新しく計算した幅よりも小さい場合にのみ
				//既定の幅を変更します
				if ( newWidth > comboBox.DropDownWidth )
				{
					comboBox.Width = newWidth;
					comboBox.DropDownWidth = newWidth;
				}

				comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			}
			catch ( Exception exp )
			{
				comboBox.Items.Add(exp.Message);
				comboBox.DropDownStyle = ComboBoxStyle.DropDown;
			}
			finally
			{
				CloseOracle(ref oleConn, ref oleCmd, ref oleReader);

				//描画面をクリーンアップします
				if ( ds != null )
				{
					ds.Dispose();
				}
			}
		}

		/// <summary>
		/// オラクルの接続を開く
		/// </summary>
		/// <param name="sid"></param>
		/// <param name="uid"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		private OracleConnection OpenOracle(string sid, string uid, string pwd)
		{
			OracleConnection oleConn = null;
			string conStr = "Data Source=" + sid + ";User Id=" + uid + ";Password=" + pwd;
			oleConn = new OracleConnection(conStr);
			oleConn.Open();
			return oleConn;
		}

		/// <summary>
		/// オラクルの接続を閉じる
		/// </summary>
		/// <param name="oleConn"></param>
		/// <param name="oleCmd"></param>
		/// <param name="oleReader"></param>
		private void CloseOracle(ref OracleConnection oleConn, ref OracleCommand oleCmd, ref OracleDataReader oleReader)
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

		/// <summary>
		/// textBox_Validating
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				// [ESC]キーが押された（アクティブ コントロールは移動してない）？
				if ( ActiveControl.Equals(sender) )
					return;

				Control control = (Control)sender;
				if ( control.Text.Length == 0 )
				{
					errorProvider.SetError(control, "必須入力です");
					e.Cancel = true;	// true にすると、正しく入力するまで次に行けない。
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// textBox_Validated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox_Validated(object sender, EventArgs e)
		{
			try
			{
				this.errorProvider.SetError((Control)sender, null);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// [Reload Value] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripReloadValue_Click(object sender, EventArgs e)
		{
			try
			{
				Encoding shiftJis = Encoding.GetEncoding("Shift_JIS");
				byte[] returnedByte = new byte[0xffff];

				// 既存のセクション[shenlongColumnName]を読み込む
				int count = (int)api.GetPrivateProfileSection(shenlongColumnName, returnedByte, (uint)returnedByte.Length, selectParamIniFileName);

				string returnedString = shiftJis.GetString(returnedByte, 0, count - 1);
				string[] keys = returnedString.Split('\0');

				for ( int i = 0; i < keys.Length; i++ )
				{
					string[] keyValue = keys[i].Split('=');
					Control control = flowLayoutPanel.Controls[keyValue[0]];
					if ( control == null )
						continue;

					if ( control is TextBox )
					{
						control.Text = keyValue[1];
					}
					else if ( control is DateTimePicker )
					{
						//((DateTimePicker)control).Value = new DateTime(int.Parse(keyValue[1].Substring(0, 4)), int.Parse(keyValue[1].Substring(4, 2)), int.Parse(keyValue[1].Substring(6, 2)));
						((DateTimePicker)control).Value = DateTime.Parse(keyValue[1]);
					}
					else if ( control is ComboBox )
					{
						foreach ( ShenComboBoxItem scbItem in ((ComboBox)control).Items )
						{
							if ( scbItem.Value == keyValue[1] )
							{
								((ComboBox)control).SelectedItem = scbItem;
								break;
							}
						}
					}
				}

				// 最初の入力欄にフォーカスする
				int c;
				for ( c = 0; c < flowLayoutPanel.Controls.Count; c++ )
				{
					if ( flowLayoutPanel.Controls[c] is Label )
						continue;
					flowLayoutPanel.Controls[c].Select();
					break;
				}
				if ( c == flowLayoutPanel.Controls.Count )
				{
					buttonOK.Select();
				}
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// contextMenuToolStrip が開かれようとしている
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuToolStrip_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				Point point = toolStrip.PointToClient(Cursor.Position);
				Debug.WriteLine(point);
				if ( !toolStripReloadValue.Bounds.Contains(point) )
				{
					e.Cancel = true;
					return;
				}

				toolStripMenuDeleteLatestParams.Enabled = toolStripReloadValue.Enabled;
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// [前回の値を削除する] がクリックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripMenuDeleteLatestParams_Click(object sender, EventArgs e)
		{
			try
			{
#if false
				api.WritePrivateProfileString(shenlongColumnName, null, null, selectParamIniFileName);
#else
				Encoding shiftJis = Encoding.GetEncoding("Shift_JIS");
				byte[] returnedByte = new byte[0xffff];

				// 既存のセクション[shenlongColumnName]を読み込む
				int count = (int)api.GetPrivateProfileSection(shenlongColumnName, returnedByte, (uint)returnedByte.Length, selectParamIniFileName);

				string returnedString = shiftJis.GetString(returnedByte, 0, count - 1);
				string[] keys = returnedString.Split('\0');
				StringBuilder _lpString = new StringBuilder();

				for ( int i = 0; i < keys.Length; i++ )
				{
					if ( keys[i][0] == '_' )
						continue;
					_lpString.Append(keys[i] + "\0");
				}

				byte[] lpString = shiftJis.GetBytes(_lpString.ToString());
				// 値を除いたキーを書き込む
				count = (int)api.WritePrivateProfileSection(shenlongColumnName, lpString, selectParamIniFileName);
#endif

				toolStripReloadValue.Enabled = false;

				selectParams = null;	// この後で [キャンセル] すると、親の latestSelectParams を初期化できるようにする
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}

		/// <summary>
		/// [OK] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
				selectParams = new Dictionary<string, string>();
				StringBuilder keys = new StringBuilder();

				foreach ( Control control in flowLayoutPanel.Controls )
				{
					if ( control is TextBox )
					{
						string text = control.Text;
						if ( (text.Length == 0) && toolStripShenValue.Checked )
						{
							//value = toolTip.GetToolTip(control);
							continue;
						}
						selectParams[control.Name] = text;

						keys.Append(control.Name + "=" + control.Text + "\0");
					}
					else if ( control is DateTimePicker )
					{
						//string value = ((DateTimePicker)control).Value.ToString("yyyyMMdd");
						DateTime dateTime = ((DateTimePicker)control).Value;
						/*string value = dateTime.ToString("yyyy/MM/dd");
						if ( (dateTime.Hour != 0) || (dateTime.Minute != 0) )	// 時間がある？
						{
							value += (" " + dateTime.ToString("HH:mm"));
						}

						selectParams[control.Name] = value.Replace("/", "").Replace(":", "");*/
						string value = dateTime.ToString((string)control.Tag);

						selectParams[control.Name] = value;

						keys.Append(control.Name + "=" + value + "\0");
					}
					else if ( control is ComboBox )
					{
						object item = ((ComboBox)control).SelectedItem;
						string value;
						if ( item == null )
						{
							if ( toolStripShenValue.Checked )
								continue;
							value = "";
						}
						else
						{
							value = ((ShenComboBoxItem)item).Value;
						}

						selectParams[control.Name] = value;

						keys.Append(control.Name + "=" + value + "\0");
					}
				}

				//if ( keys.Length != 0 )
				{
					keys.Append("\0");

					SaveSelectParamsFile(keys.ToString());
				}
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		/// <summary>
		/// 入力された抽出条件をファイルに保存する
		/// </summary>
		/// <param name="keys"></param>
		private void SaveSelectParamsFile(string keys)
		{
			try
			{
				Encoding shiftJis = Encoding.GetEncoding("Shift_JIS");
				byte[] returnedByte = new byte[0xffff];

				// 既存のセクション[shenlongColumnName]があれば削除する
				int count = (int)api.GetPrivateProfileSection(shenlongColumnName, returnedByte, (uint)returnedByte.Length, selectParamIniFileName);
				if ( count != 0 )
				{
					api.WritePrivateProfileString(shenlongColumnName, null, null, selectParamIniFileName);
				}

				// セクションの一覧を取得する
				count = (int)api.GetPrivateProfileString/*ByByteArray*/(null, null, "", returnedByte, (uint)returnedByte.Length, selectParamIniFileName);
				if ( count != 0 )
				{
					string returnedString = shiftJis.GetString(returnedByte, 0, count - 1);
					string[] sections = returnedString.Split('\0');
	
					// 最大数を超えたセクションは削除する
					int maxSection = (int)api.GetPrivateProfileInt(SETTINGS_SECTION, KEY_MAX_INPUT_PARAM_HISTORY_COUNT, 64, Application.StartupPath + "\\" + Application.ProductName + ".ini");
					for ( int i = sections.Length - maxSection; 0 <= i; i-- )
					{
						api.WritePrivateProfileString(sections[i], null, null, selectParamIniFileName);
					}
				}

				// フォームのサイズ
				string formSize = KEY_FORM_SIZE + "=" + this.Width + "," + this.Height + "\0";

				// toolStripShenValue のチェック状態
				string shenValue = KEY_SHEN_VALUE + "=" + toolStripShenValue.Checked.ToString().ToLower() + "\0";

				byte[] lpString = shiftJis.GetBytes(formSize + shenValue + keys);
				count = (int)api.WritePrivateProfileSection(shenlongColumnName, lpString, selectParamIniFileName);
			}
			catch ( Exception exp )
			{
				Debug.WriteLine(exp.Message);
			}
		}
	}

	/// <summary>
	/// ComboBox の Item 用クラス
	/// </summary>
	public class ShenComboBoxItem
	{
		private string m_text = "";
		private string m_value = "";

		//コンストラクタ
		public ShenComboBoxItem(string text, string value)
		{
			m_text = text;
			m_value = value;
		}

		//表示名称
		//(このプロパティはこのサンプルでは使わないのでなくても良い)
		public string Text
		{
			get
			{
				return m_text;
			}
		}

		//実際の値
		public string Value
		{
			get
			{
				return m_value;
			}
		}

		//オーバーライドしたメソッド
		//これがコンボボックスに表示される
		public override string ToString()
		{
			return m_text;
		}
	}
}
