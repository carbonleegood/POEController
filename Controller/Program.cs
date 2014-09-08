using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.GameCall;
namespace Controller
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        public static TTransport transport = null;
        public static TProtocol protocol = null;
        public static GameFuncCall.Client client = null;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login login = new Login();
            DialogResult ret = login.ShowDialog();
            if (ret == DialogResult.Cancel)
                return;
            Application.Run(new FormControl());
        }
    }
}
