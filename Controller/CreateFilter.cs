using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Controller
{
    public partial class CreateFilter : Form
    {
        public string FilterName = null;
        public CreateFilter()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbName.Text.Length < 1)
            {
                MessageBox.Show("请输入過濾器名称");
                return;
            }

            FilterName = tbName.Text;
            this.DialogResult = DialogResult.OK;
            return;
        }
    }
}
