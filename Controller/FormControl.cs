using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.GameCall;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
namespace Controller
{
    public partial class FormControl : Form
    {
        // int GetExplorePoint(ULONG* px, ULONG* py);
         [DllImport("MapServer.dll", EntryPoint = "GetExplorePoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetExplorePoint(ref UInt16 x, ref UInt16 y);

         [DllImport("MapServer.dll", EntryPoint = "SetExploredPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void SetExploredPoint(UInt16 x, UInt16 y);

         [DllImport("MapServer.dll", EntryPoint = "UpdateMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void UpdateMap();

         [DllImport("MapServer.dll", EntryPoint = "DrawMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void DrawMap();

       // public static extern void MyGetExplorePoint();
        List<GPoint> path = null;
        public FormControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Pos pos= Program.client.GetPlayerPos();
           textBox1.Text = pos.X.ToString();
           textBox2.Text = pos.Y.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double x = 0;
            double y = 0;
            double.TryParse(textBox1.Text,out x);
            double.TryParse(textBox2.Text,out y);
            int nx =(int)( x * 0.092);
            int ny = (int)(y * 0.092);
            textBox3.Text = nx.ToString();
            textBox4.Text = ny.ToString();
            nx+=50;
            ny+=50;
            Program.client.Move(nx,ny);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.client.Update();
        }
        bool bWorking=false;
        void caijiThread()
        {
            path=new List<GPoint>();
            while(bWorking)
            {
                Pos pos = Program.client.GetPlayerPos();
                 GPoint pt=new GPoint();
               pt.x = (UInt16)(pos.X * 0.092);
               pt.y = (UInt16)(pos.Y * 0.092);
               path.Add(pt);
               Thread.Sleep(100);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (bWorking)
                return;
            bWorking = true;
            Thread t = new Thread(caijiThread);
            t.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bWorking = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (path == null)
                return;
            FileStream fs = new FileStream("d:\\path.txt", FileMode.Create,FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);
            //循环写入文件
            foreach(var item in path)
            {
                bw.Write(item.x);
                bw.Write(item.y);
            }
            bw.Close();
            fs.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
          //  Int32 nRet = 99;
            UInt32 x = 10;
            UInt32 y = 50;
          //  MyGetExplorePoint();
          //  Int32 nRet = GetExplorePoint( ref x,ref  y);
         //   MessageBox.Show("Test" + nRet + "," + x + "'" + y);
        }
        double CalcDis(GPoint P1,GPoint P2)
        {
            int x=Math.Abs(P1.x - P2.x);
            int y=Math.Abs(P1.y - P2.y);
            double z = x * x + y * y;
            double dis = Math.Sqrt(z);
            return dis;
        }
        void exploreThread()
        {
           
            GPoint TargetPoint = new GPoint();
            GPoint CurPos = new GPoint();

            Pos pos = Program.client.GetPlayerPos();
            TargetPoint.x = (UInt16)(pos.X * 0.092);
            TargetPoint.y = (UInt16)(pos.Y * 0.092);

            
            Int32 X, Y;
            while (bWorking)
            {
                //获取玩家位置
                pos = Program.client.GetPlayerPos();
                CurPos.x = (UInt16)(pos.X * 0.092);
                CurPos.y = (UInt16)(pos.Y * 0.092);
               //与目标点对比,如果到了,则获取下一点

                double dis = CalcDis(CurPos, TargetPoint);
                if(dis<15.0)
                {
                    SetExploredPoint(TargetPoint.x, TargetPoint.y);
                    if (GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y) != 0)
                    {
                        MessageBox.Show("获取不到点了");
                        bWorking = false;
                        break;
                    }    
                }
                X = TargetPoint.x;
                Y = TargetPoint.y;
                Program.client.Move(X, Y);
                Thread.Sleep(200);
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (bWorking)
                return;
            bWorking = true;
            Thread t = new Thread(exploreThread);
            t.Start();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdateMap();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DrawMap();
        }
    }
    class GPoint
    {
        public UInt16 x;
        public UInt16 y;
    }
}
