using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Thrift;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.GameCall;
using System.Runtime.InteropServices;
namespace Controller
{
    public partial class Login : Form
    {
        [DllImport("MapServer.dll", EntryPoint = "GGGGGG", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 StartCheck(string uid, string pwd,int nTemp);
        [DllImport("MapServer.dll", EntryPoint = "CheckUID", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 CheckUID(string uid, string pwd);

        [DllImport("MapServer.dll", EntryPoint = "GetGGPid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetGGPid(byte[] buff);

        [DllImport("MapServer.dll", EntryPoint = "HelloKitty", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 HelloKitty();

        [DllImport("MapServer.dll", EntryPoint = "HelloKitty2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 InjectDLL();

        public bool bAutoLogin = false;
        public Login()
        {
            InitializeComponent();

            string strUID = null;
            string strPWD = null;
            string filename = @"Mith.bat";
            GetUID();
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                return;
            }
            BinaryFormatter binFormat = new BinaryFormatter();
       //     strUID = (string)binFormat.Deserialize(fStream);
            strPWD = (string)binFormat.Deserialize(fStream);
        //    tbUID.Text = strUID;
            tbPWD.Text = strPWD;
            fStream.Close();
            if (bAutoLogin)
                Start();
        }
        void GetUID()
        {
            byte[] buff = new byte[128];

            Int32 nRet = GetGGPid(buff);
            if (nRet != 0)
            {
                MessageBox.Show("請啟動Garena:"+nRet.ToString());
                return;
            }

            tbUID.Text = Encoding.ASCII.GetString(buff);
            //textBox2.Text = strName;
        }
        void Start()
        {
            int nRet = CheckUID(tbUID.Text, tbPWD.Text);
            if (0 != nRet)
            {
                switch (nRet)
                {
                    case 3:
                        MessageBox.Show("連接驗證服務器失敗");
                        break;
                    case 6:
                        MessageBox.Show("賬號未註冊");
                        break;
                    case 15:
                        MessageBox.Show("密碼錯誤");
                        break;
                    case 7:
                        MessageBox.Show("賬號沒有使用時間");
                        break;
                    case 8:
                        MessageBox.Show("未知錯誤,請聯繫管理員");
                        break;
                }

                return;
            }
            //string strUID = tbUID.Text;
            //string strPWD = tbPWD.Text;
            //string strParam = @"E:\流亡暗道\Worker\Debug" + "," + strUID + "," + strPWD;
            try
            {
                EventWaitHandle evtOld = EventWaitHandle.OpenExisting("lglglg");
                evtOld.Close();
                //连接
                //    Program.transport = new TSocket("localhost", 9998);
                Program.transport = new TPipe(@".", "xsxsxs");
                Program.protocol = new TBinaryProtocol(Program.transport);
                Program.client = new GameFuncCall.Client(Program.protocol);
                Program.transport.Open();
                this.DialogResult = DialogResult.OK;
                StartCheck(tbUID.Text, tbPWD.Text,0);
                HelloKitty();
                return;
            }
            catch
            {
            }

            EventWaitHandle evt = new EventWaitHandle(false, EventResetMode.AutoReset, "lglglg");
            //调用CREATER
            //   Process child = Process.Start(@"E:\流亡暗道\Creater\Debug\Creater.exe", null);
            int nInjectRet=InjectDLL();
            if (nInjectRet != 0)
            {
                MessageBox.Show("注入遊戲失敗,錯誤碼:" + nInjectRet);
                return;
            }
            //Process child = Process.Start(@"Creater.exe", null);
            ////等待CREATER返回事件
            //child.WaitForExit();
            //if (child.ExitCode != 0)
            //{
            //    MessageBox.Show("注入遊戲失敗,錯誤碼:" + child.ExitCode);
            //    return;
            //}
            //等待DLL初始化完成事件发生
            bool bRet = evt.WaitOne(1000 * 20, true);
            evt.Close();
            if (bRet == false)
            {
                MessageBox.Show("初始化失敗,請重新啟動遊戲");
                this.Close();
            }
            //连接
            // Program.transport = new TSocket("localhost", 9998);
            Program.transport = new TPipe(@".", "xsxsxs");
            Program.protocol = new TBinaryProtocol(Program.transport);
            Program.client = new GameFuncCall.Client(Program.protocol);
            Program.transport.Open();
            this.DialogResult = DialogResult.OK;

            HelloKitty();
            //启动验证
            StartCheck(tbUID.Text, tbPWD.Text,0);
            return;
        }
        private void btnInject_Click(object sender, EventArgs e)
        {
            Start();
        }


        private void btnSaveUID_Click(object sender, EventArgs e)
        {
         //   string strUID = tbUID.Text;
            string strPWD = tbPWD.Text;
            string filename = @"Mith.bat";
            Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);

            BinaryFormatter binFormat = new BinaryFormatter();
      //      binFormat.Serialize(fStream, strUID);
            binFormat.Serialize(fStream, strPWD);
            fStream.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nRet = CheckUID(tbUID.Text, tbPWD.Text);
            if (0 != nRet)
            {
                switch (nRet)
                {
                    case 3:
                        MessageBox.Show("連接驗證服務器失敗");
                        break;
                    case 6:
                        MessageBox.Show("賬號未註冊");
                        break;
                    case 15:
                        MessageBox.Show("密碼錯誤");
                        break;
                    case 7:
                        MessageBox.Show("賬號沒有使用時間");
                        break;
                    case 8:
                        MessageBox.Show("未知錯誤,請聯繫管理員");
                        break;
                }

                return;
            }
            else
            {
                MessageBox.Show("賬號可以使用");
            }
            //else if (ret == 6)
            //{
            //    MessageBox.Show("账号不存在或密码错误");
            //}
            //else if (ret == 7)
            //{
            //    MessageBox.Show("账号没有余额");
            //}
            //else if (ret == 3)
            //{
            //    MessageBox.Show("链接服务器失败");
            //}
            return;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.fsowg.com"); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bAutoLogin)
            {
            //    MessageBox.Show("Test");
                Start();
                bAutoLogin = false;
            }
        }

    }
}
