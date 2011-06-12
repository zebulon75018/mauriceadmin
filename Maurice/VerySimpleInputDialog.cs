// ******************************************************************
//
//	If this code works it was written by:
//		Malcolm
//		MamSoft / Manniff Computers
//		© 2008 - 2008...
//
//	if not, I have no idea who wrote it.
//
// ******************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
	public partial class VerySimpleInputDialog : Form
	{
		private string defaultValue = string.Empty;

		public VerySimpleInputDialog()
		{
			InitializeComponent();

			this.lblPrompt.Text = "";
		}

		public VerySimpleInputDialog(string prompt)
			: this()
		{
			if (!string.IsNullOrEmpty(prompt))
			{
				this.lblPrompt.Text = prompt;
			}
		}

		public VerySimpleInputDialog(string prompt, string defaultInput)
			: this()
		{
			if (!string.IsNullOrEmpty(prompt))
			{
				this.lblPrompt.Text = prompt;
			}

			this.txtInput.Text = this.defaultValue = defaultInput;
		}

		public string UserInput
		{
			get
			{
				return this.txtInput.Text;
			}
            set
            {
                this.txtInput.Text = value;
            }
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			// Don't bother if no default
			if (!string.IsNullOrEmpty(this.defaultValue))
			{
				this.txtInput.Text = this.defaultValue;
			}
		}
	}
}
