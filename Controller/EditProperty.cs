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
    public partial class EditProperty : Form
    {
        public string strProperty = null;
        public EditProperty()
        {
            InitializeComponent();
          //  tbProperty.Text = strProperty;
        }
        public void SetPropertyString(string Property)
        {
            tbProperty.Text = Property;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbProperty.Text.Length < 1)
            {
                MessageBox.Show("屬性信息過短");
                return;
            }
            strProperty = tbProperty.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
