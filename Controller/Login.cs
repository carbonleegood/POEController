using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Thrift;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.GameCall;
namespace Controller
{
    public partial class Login : Form
    {
        
       
        public Login()
        {
            InitializeComponent();

            string strUID = null;
            string strPWD = null;
            string filename = @"Mith.bat";
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            }
            catch(Exception e)
            {
                return;
            }

            BinaryFormatter binFormat = new BinaryFormatter();
            strUID = (string)binFormat.Deserialize(fStream);
            strPWD = (string)binFormat.Deserialize(fStream);
            tbUID.Text = strUID;
            tbPWD.Text = strPWD;
            fStream.Close();
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            //string strUID = tbUID.Text;
            //string strPWD = tbPWD.Text;
            //string strParam = @"E:\流亡暗道\Worker\Debug" + "," + strUID + "," + strPWD;
            try
            {
                EventWaitHandle evtOld = EventWaitHandle.OpenExisting("lglglg");
                evtOld.Close();
                //连接
                Program.transport = new TSocket("localhost", 9998);
                Program.protocol = new TBinaryProtocol(Program.transport);
                Program.client = new GameFuncCall.Client(Program.protocol);
                Program.transport.Open(); 
                this.DialogResult = DialogResult.OK;
                return;
            }
            catch 
            { 
            }

            EventWaitHandle evt = new EventWaitHandle(false, EventResetMode.AutoReset, "lglglg");
            //调用CREATER
            Process child = Process.Start(@"E:\流亡暗道\Creater\Debug\Creater.exe", null);
            //等待CREATER返回事件
            child.WaitForExit();
            if(child.ExitCode!=0)
            {
                MessageBox.Show("注入游戏失败,错误码:" + child.ExitCode);
                return;
            }
            //等待DLL初始化完成事件发生
            bool bRet = evt.WaitOne(1000 * 20);
            evt.Close();
            if (bRet == false)
            {
                MessageBox.Show("初始化失败,请从新启动游戏");
                this.Close();
            }
            //连接
            Program.transport = new TSocket("localhost", 9998);
            Program.protocol = new TBinaryProtocol(Program.transport);
            Program.client = new GameFuncCall.Client(Program.protocol);
            Program.transport.Open(); 
            this.DialogResult = DialogResult.OK;
            return;
        }


        private void btnSaveUID_Click(object sender, EventArgs e)
        {
            string strUID = tbUID.Text;
            string strPWD = tbPWD.Text;
            string filename = @"Mith.bat";
            Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);

            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(fStream, strUID);
            binFormat.Serialize(fStream, strPWD);
            fStream.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
