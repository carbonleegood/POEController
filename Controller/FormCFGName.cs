using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Controller
{
    public partial class FormCFGName : Form
    {
        public string CFGName;
        public FormCFGName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbName.Text.Length < 1)
            {
                MessageBox.Show("请输入配置方案名称");
                return;
            }
            CFGName = "Config\\"+ tbName.Text+".cfg";
            if (File.Exists(CFGName))
            {
                MessageBox.Show("此配置方案已经存在,请输入其他名称");
                return;
            }
            Stream fStream = null;
            fStream = new FileStream(CFGName, FileMode.Create, FileAccess.ReadWrite);
            BinaryFormatter binFormat = new BinaryFormatter();
            ConfigData data = new ConfigData();
            binFormat.Serialize(fStream,data);
            fStream.Close();
            CFGName = tbName.Text + ".cfg";
            this.DialogResult = DialogResult.OK;
            return;
        }
    }
}
