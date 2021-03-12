using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using CommonFunctions;
#if WITHIN_SHENGLOBAL
using ShenGlobal = Shenlong.Shenlong;
#endif

namespace Shenlong
{
	public partial class CopyQueryColumnDlg : Form
	{
		public enum modes { copy, cut };

		private XmlDocument xmlShenlongColumn;
		private modes mode;

		private const string attrSelected = "selected";
		public const string attrIndex = "index";

		public XmlDocument xmlCopiedShenlongColumn = null;				// コピーされたクエリー項目

		private bool nowLoading = true;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="_xmlShenlongColumn"></param>
		/// <param name="_mode"></param>
		public CopyQueryColumnDlg(XmlDocument _xmlShenlongColumn, modes _mode)
		{
			InitializeComponent();

			xmlShenlongColumn = _xmlShenlongColumn;
			mode = _mode;
		}

		/// <summary>
		/// CopyQueryColumnDlg_Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopyQueryColumnDlg_Load(object sender, EventArgs e)
		{
			try
			{
				/*// Create a Bitmap object from an image file.
				Bitmap bitmap = Properties.Resources.excel;
				// Get an Hicon for myBitmap. 
				IntPtr hIcon = bitmap.GetHicon();
				// Create a new icon from the handle. 
				Icon dlgIcon = Icon.FromHandle(hIcon);
				// Write Icon to File Stream
				//System.IO.MemoryStream ms = new System.IO.MemoryStream();
				//dlgIcon.Save(ms);
				this.Icon = dlgIcon;
				//ms.Close();*/

				IntPtr sysMenuHandle = api.GetSystemMenu(this.Handle, false);
				int sysMenuItemCount = api.GetMenuItemCount(sysMenuHandle);
				for ( int i = (sysMenuItemCount - 2) - 1; 0 <= i; i-- )	// -2:[閉じる][セパレータ]
				{
					if ( i == 1 || i == 2 )	// 移動 || サイズ変更？
						continue;
					api.RemoveMenu(sysMenuHandle, (uint)i, api.MF_BYPOSITION);
				}

				this.Icon = (mode == modes.cut) ? Properties.Resources.cut : Properties.Resources.copy;
				this.Text = "[" + ((mode == modes.cut) ? "切り取り" : "コピー") + "] クエリー項目を選択";

				this.MinimumSize = this.Size;

				foreach ( XmlNode column in xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn) )
				{
					SetListQueryColumnItem(-1, column);
				}

				if ( xmlShenlongColumn.DocumentElement[ShenGlobal.tagTableJoin] == null )
				{
					checkWithTableJoin.Checked = false;
					checkWithTableJoin.Enabled = false;
				}

				radioColUp.Enabled = false;
				radioColDown.Enabled = false;
				buttonColMove.Enabled = false;

				checkSortByTname.Enabled = (mode == modes.copy);

				nowLoading = false;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				DialogResult = DialogResult.Cancel;
				this.Close();
			}
		}

		/// <summary>
		/// SetListQueryColumnItem
		/// </summary>
		private void SetListQueryColumnItem(int index, XmlNode column)
		{
			if ( index == -1 )
			{
				// <application> ノードをリストビューへ追加する
				ListViewItem item = new ListViewItem(column.Attributes[ShenGlobal.attrTableName].Value);
				XmlAttribute attr = column.Attributes[attrSelected];
				if ( attr == null )
				{
					attr = xmlShenlongColumn.CreateAttribute(attrSelected);
					attr.Value = false.ToString().ToLower();
					column.Attributes.Append(attr);
				}
				item.Checked = bool.Parse(attr.Value);
				item.SubItems.Add(column[ShenGlobal.qc.fieldName.ToString()].InnerText);
				item.SubItems.Add(column[ShenGlobal.qc.showField.ToString()].InnerText == true.ToString().ToLower() ? "レ" : "");
				item.SubItems.Add(column[ShenGlobal.qc.expression.ToString()].InnerText);
				item.SubItems.Add(column[ShenGlobal.qc.value1.ToString()].InnerText);
				item.SubItems.Add(column[ShenGlobal.qc.groupFunc.ToString()].InnerText);
				listViewQueryColumn.Items.Add(item);
			}
			else
			{
				ListViewItem item = listViewQueryColumn.Items[index];
				item.Checked = bool.Parse(column.Attributes[attrSelected].Value);
				item.SubItems[0].Text = column.Attributes[ShenGlobal.attrTableName].Value;
				item.SubItems[1].Text = column[ShenGlobal.qc.fieldName.ToString()].InnerText;
				item.SubItems[2].Text = column[ShenGlobal.qc.showField.ToString()].InnerText == true.ToString().ToLower() ? "レ" : "";
				item.SubItems[3].Text = column[ShenGlobal.qc.expression.ToString()].InnerText;
				item.SubItems[4].Text = column[ShenGlobal.qc.value1.ToString()].InnerText;
				item.SubItems[5].Text = column[ShenGlobal.qc.groupFunc.ToString()].InnerText;
			}
		}

		/// <summary>
		/// [全て選択] がチェックされた
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
		{
			foreach ( ListViewItem lvi in listViewQueryColumn.Items )
			{
				lvi.Checked = (checkSelectAll.Checked);
			}
		}

		/// <summary>
		/// [OK] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			try
			{
				xmlCopiedShenlongColumn = new XmlDocument();
				XmlDeclaration decl = xmlCopiedShenlongColumn.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlCopiedShenlongColumn.AppendChild(decl);

				XmlNode root = xmlCopiedShenlongColumn.CreateNode(XmlNodeType.Element, ShenGlobal.tagShenlong, null);	// <shenlong>
				xmlCopiedShenlongColumn.AppendChild(root);

				// クエリー項目
				XmlNodeList columnList = xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn);

				for ( int i = 0; i < listViewQueryColumn.Items.Count; i++ )
				{
					if ( !listViewQueryColumn.Items[i].Checked )
						continue;

					XmlNode column = xmlCopiedShenlongColumn.DocumentElement.AppendChild(xmlCopiedShenlongColumn.ImportNode(columnList[i], true));

					XmlAttribute attr = xmlCopiedShenlongColumn.CreateAttribute(attrIndex);
					attr.Value = i.ToString();
					column.Attributes.Append(attr);
				}

				// テーブル結合
				if ( checkWithTableJoin.Checked )
				{
					foreach ( XmlNode tableJoin in xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagTableJoin) )
					{
						string[] tableColumn = tableJoin.Attributes[ShenGlobal.tabJoin.leftTabCol.ToString()].Value.Split('.');
						string xpath = "/" + ShenGlobal.tagShenlong + "/" + ShenGlobal.tagColumn + "[@" + ShenGlobal.attrTableName + "='" + tableColumn[0] + "'][" + ShenGlobal.qc.fieldName + "='" + tableColumn[1] + "']";
						if ( xmlCopiedShenlongColumn.SelectSingleNode(xpath) == null )
							continue;

						tableColumn = tableJoin.Attributes[ShenGlobal.tabJoin.rightTabCol.ToString()].Value.Split('.');
						xpath = "/" + ShenGlobal.tagShenlong + "/" + ShenGlobal.tagColumn + "[@" + ShenGlobal.attrTableName + "='" + tableColumn[0] + "'][" + ShenGlobal.qc.fieldName + "='" + tableColumn[1] + "']";
						if ( xmlCopiedShenlongColumn.SelectSingleNode(xpath) == null )
							continue;

						xmlCopiedShenlongColumn.DocumentElement.AppendChild(xmlCopiedShenlongColumn.ImportNode(tableJoin, true));
					}
				}

				if ( Program.debMode )
				{
					xmlCopiedShenlongColumn.Save(Application.StartupPath + "\\" + "~copiedShenlongColumn.xml");
				}

				DialogResult = (xmlCopiedShenlongColumn.DocumentElement.ChildNodes.Count != 0) ? DialogResult.OK : DialogResult.Cancel;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				DialogResult = DialogResult.Cancel;
				xmlCopiedShenlongColumn = null;
			}

			this.Close();
		}

		/// <summary>
		/// listViewQueryColumn_SelectedIndexChanged
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listViewQueryColumn_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if ( mode == modes.cut )
					return;

				radioColUp.Enabled = false;
				radioColDown.Enabled = false;
				buttonColMove.Enabled = false;

				if ( listViewQueryColumn.SelectedItems.Count == 0 )
					return;

				if ( listViewQueryColumn.SelectedIndices[0] != 0 )
					radioColUp.Enabled = true;
				if ( listViewQueryColumn.SelectedIndices[0] != listViewQueryColumn.Items.Count - 1 )
					radioColDown.Enabled = true;
				if ( !radioColDown.Enabled )
					radioColUp.Checked = true;
				else if ( !radioColUp.Enabled )
					radioColDown.Checked = true;
				buttonColMove.Enabled = (radioColUp.Enabled || radioColDown.Enabled)/*true*/;
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// listViewQueryColumn_ItemCheck
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listViewQueryColumn_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			try
			{
				if ( nowLoading )
					return;

				xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn)[e.Index].Attributes[attrSelected].Value = (e.NewValue == CheckState.Checked).ToString().ToLower();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [移動] ボタンが押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonColMove_Click(object sender, EventArgs e)
		{
			try
			{
				int selected = listViewQueryColumn.SelectedIndices[0];
				int reference = (radioColUp.Checked) ? selected - 1 : selected + 1;
				XmlNode newChild = xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn)[selected];
				XmlNode refChild = xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn)[reference];
				if ( radioColUp.Checked )
					xmlShenlongColumn.DocumentElement.InsertBefore(newChild, refChild);
				else
					xmlShenlongColumn.DocumentElement.InsertAfter(newChild, refChild);

				SetListQueryColumnItem(selected, refChild);
				SetListQueryColumnItem(reference, newChild);

				listViewQueryColumn.Items[reference].Selected = true;
				listViewQueryColumn.EnsureVisible(reference);
				buttonColMove.Select();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// [テーブル名でソート] が押された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkSortByTname_Click(object sender, EventArgs e)
		{
			try
			{
				//if ( checkSortByTname.Checked )
				//	return;

				Cursor.Current = Cursors.WaitCursor;

				bool ascending = (checkSortByTname.Tag == null || (string)checkSortByTname.Tag == "descending");

				// 子供達を配列に転写し
				XmlNodeList columnList = xmlShenlongColumn.DocumentElement.SelectNodes(ShenGlobal.tagColumn);
				XmlElement[] children = new XmlElement[columnList.Count];
				int i = 0;
				foreach ( XmlNode child in columnList )
				{
					children[i++] = (XmlElement)child;
				}

				//  属性の値でソートして
				Array.Sort(children,
						   delegate(XmlElement x, XmlElement y)
						   {
							   int comp = x.Attributes[ShenGlobal.attrTableName].Value.CompareTo(y.Attributes[ShenGlobal.attrTableName].Value);
							   if ( comp == 0 )
							   {
								   comp = x[ShenGlobal.qc.fieldName.ToString()].InnerText.CompareTo(y[ShenGlobal.qc.fieldName.ToString()].InnerText);
							   }

							   return comp * ((ascending) ? 1 : -1);
						   }
						   );

				listViewQueryColumn.Items.Clear();
				foreach ( XmlNode column in columnList )
				{
					xmlShenlongColumn.DocumentElement.RemoveChild(column);
				}

				// 書き出す
				XmlNode refChild = null;
				foreach ( XmlNode child in children )
				{
					refChild = xmlShenlongColumn.DocumentElement.InsertAfter(child, refChild);
					SetListQueryColumnItem(-1, child);
				}

				//checkSortByTname.Checked = true;
				checkSortByTname.Tag = (ascending) ? "ascending" : "descending";
				listViewQueryColumn.Select();
			}
			catch ( Exception exp )
			{
				MessageBox.Show(exp.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}
}