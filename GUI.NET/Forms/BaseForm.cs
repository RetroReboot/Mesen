﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public class BaseForm : Form
	{
		protected ToolTip toolTip;
		private System.ComponentModel.IContainer components;
		private bool _iconSet = false;

		public delegate bool ProcessCmdKeyHandler(Keys keyData);
		public event ProcessCmdKeyHandler OnProcessCmdKey;

		public BaseForm()
		{
			InitializeComponent();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool? result = OnProcessCmdKey?.Invoke(keyData);
			if(result == true) {
				return true;
			} else {
				return base.ProcessCmdKey(ref msg, keyData);
			}
		}

		public void Show(object sender, IWin32Window owner = null)
		{
			if(sender is ToolStripMenuItem) {
				ToolStripItem menuItem = (ToolStripMenuItem)sender;
				if(menuItem.Image == null) {
					menuItem = menuItem.OwnerItem;
				}
				this.Icon = menuItem.Image;
			}

			CenterOnParent(owner);
			base.Show();
		}

		private void CenterOnParent(IWin32Window owner)
		{
			Form parent = (Form)owner;
			Point point = parent.PointToScreen(new Point(parent.Width / 2, parent.Height / 2));

			this.StartPosition = FormStartPosition.Manual;
			this.Top = point.Y - this.Height / 2;
			this.Left = point.X - this.Width / 2;
		}

		public DialogResult ShowDialog(object sender, IWin32Window owner = null)
		{
			if(sender is ToolStripMenuItem) {
				ToolStripItem menuItem = (ToolStripMenuItem)sender;
				if(menuItem.Image == null) {
					menuItem = menuItem.OwnerItem;
				}
				this.Icon = menuItem.Image;
			}
			return base.ShowDialog(owner);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!DesignMode) {
				if(!_iconSet) {
					base.Icon = Properties.Resources.MesenIcon;
				}
			}

			int tabIndex = 0;
			InitializeTabIndexes(this, ref tabIndex);
			ResourceHelper.ApplyResources(this);
		}

		private void InitializeTabIndexes(TableLayoutPanel tlp, ref int tabIndex)
		{
			tlp.TabIndex = tabIndex;
			tabIndex++;

			for(int i = 0; i < tlp.RowCount; i++) {
				for(int j = 0; j < tlp.ColumnCount; j++) {
					Control ctrl = tlp.GetControlFromPosition(j, i);
					if(ctrl != null) {
						if(ctrl is TableLayoutPanel) {
							InitializeTabIndexes(((TableLayoutPanel)ctrl), ref tabIndex);
						} else {
							InitializeTabIndexes(ctrl, ref tabIndex);
						}
					}
				}
			}
		}

		private void InitializeTabIndexes(Control container, ref int tabIndex)
		{
			if(Program.IsMono) {
				if(container is TextBox) {
					((TextBox)container).BorderStyle = BorderStyle.FixedSingle;
				} else if(container is CheckBox) {
					((CheckBox)container).FlatStyle = FlatStyle.Flat;
				} else if(container is Button) {
					((Button)container).FlatStyle = FlatStyle.Flat;
					((Button)container).BackColor = ((Button)container).Enabled ? Color.FromArgb(230, 230, 230) : Color.FromArgb(180, 180, 180);
					((Button)container).EnabledChanged += (object sender, EventArgs e) => {
						((Button)sender).BackColor = ((Button)sender).Enabled ? Color.FromArgb(230, 230, 230) : Color.FromArgb(180, 180, 180);
					};
				} else if(container is ComboBox) {
					((ComboBox)container).FlatStyle = FlatStyle.Flat;
					((ComboBox)container).BackColor = ((ComboBox)container).Enabled ? Color.FromArgb(230, 230, 230) : Color.FromArgb(180, 180, 180);
					((ComboBox)container).EnabledChanged += (object sender, EventArgs e) => {
						((ComboBox)sender).BackColor = ((ComboBox)sender).Enabled ? Color.FromArgb(230, 230, 230) : Color.FromArgb(180, 180, 180);
					};
				} else if(container is TabPage) {
					((TabPage)container).BackColor = Color.White;
				} else if(container is MenuStrip) {
					((MenuStrip)container).RenderMode = ToolStripRenderMode.System;
				} else if(container is ToolStrip) {
					((ToolStrip)container).RenderMode = ToolStripRenderMode.System;
				}
			}			
			container.TabIndex = tabIndex;
			tabIndex++;

			foreach(Control ctrl in container.Controls) {
				if(ctrl is TableLayoutPanel) {
					InitializeTabIndexes(((TableLayoutPanel)ctrl), ref tabIndex);
				} else {
					InitializeTabIndexes(ctrl, ref tabIndex);
				}
			}
		}

		public new Image Icon
		{
			set
			{
				if(value != null) {
					Bitmap b = new Bitmap(value);
					Icon i = System.Drawing.Icon.FromHandle(b.GetHicon());
					base.Icon = i;
					i.Dispose();

					_iconSet = true;
				}
			}				
		}
		
		public new SizeF AutoScaleDimensions
		{
			set 
			{ 
				if(!Program.IsMono) { 
					base.AutoScaleDimensions = value; 
				}
			}
		} 
				
		public new AutoScaleMode AutoScaleMode
		{
			set {
				if(Program.IsMono) { 
					base.AutoScaleMode = AutoScaleMode.None;
				} else {
					base.AutoScaleMode = value;
				}
			}
		}		

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 32700;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 10;
			// 
			// BaseForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "BaseForm";
			this.ResumeLayout(false);

		}
	}
}
