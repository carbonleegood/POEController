//#define DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.GameCall;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Controller
{
#if DEBUG
    public partial class FormControl : Form
    {
        [DllImport("MapServer.dll", EntryPoint = "helloCraker", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void HitKey(int x);
        // int GetExplorePoint(ULONG* px, ULONG* py);
         [DllImport("MapServer.dll", EntryPoint = "GetCrakerjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetExplorePoint(ref UInt16 x, ref UInt16 y);

         [DllImport("MapServer.dll", EntryPoint = "CrakerMujj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern Int32 GetGroupExplorePoint(ref UInt16 x, ref UInt16 y,int Group);

         [DllImport("MapServer.dll", EntryPoint = "GetCrakerJJ", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern Int32 GetCurGroup(ushort x, ushort y);

         [DllImport("MapServer.dll", EntryPoint = "CutCrakerDajiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern Int32 MoveToPoint( UInt16 x,  UInt16 y,UInt16 px,UInt16 py);
         //[DllImport("MapServer.dll", EntryPoint = "SetExploredPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         //public static extern void SetExploredPoint(UInt16 x, UInt16 y);

         [DllImport("MapServer.dll", EntryPoint = "CrakerDajijiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void UpdatePollutantMap();

         [DllImport("MapServer.dll", EntryPoint = "CrakerDajiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void UpdateMap(bool bGroupModel);

         [DllImport("MapServer.dll", EntryPoint = "DrawMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void DrawMap();

        [DllImport("MapServer.dll", EntryPoint = "GetMapData", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         public static extern void GetMapData();

        [DllImport("MapServer.dll", EntryPoint = "AAAAAA", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetPollutantAstarDis(UInt16 x, UInt16 y, UInt16 px, UInt16 py);

        [DllImport("MapServer.dll", EntryPoint = "AAAAAAA", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetAstarDis(UInt16 x, UInt16 y, UInt16 px, UInt16 py);

        [DllImport("MapServer.dll", EntryPoint = "UpdateTownMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 UpdateTownMap(UInt16 npcx, UInt16 npcy, UInt16 wpx, UInt16 wpy, UInt16 tdx, UInt16 tdy);

        [DllImport("MapServer.dll", EntryPoint = "MoveToSellNPC", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToSellNPC(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTownWaypoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTownWaypoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTransferDoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTransferDoor(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "GGGGGG", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 StartCheck(string uid, string pwd);

        [DllImport("MapServer.dll", EntryPoint = "GetGGPid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 GetGGPid(byte[] buff);

        [DllImport("MapServer.dll", EntryPoint = "GetCrakerJJCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetGroupCount();

        [DllImport("MapServer.dll", EntryPoint = "jjjjjjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SetPassAbleArea(UInt16 x, UInt16 y);

        //[DllImport("MapServer.dll", EntryPoint = "GetCurGroup", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //public static extern Int32 GetCurGroup( UInt16 x,  UInt16 y);

        
       // public static extern void MyGetExplorePoint();
        List<GPoint> path = null;
        public FormControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Pos pos= Program.client.GetPlayerPos();
           //textBox1.Text = pos.X.ToString();
           //textBox2.Text = pos.Y.ToString();
           int nx = (int)(pos.X * 0.092);
           int ny = (int)(pos.Y * 0.092);
           textBox3.Text = nx.ToString();
           textBox4.Text = ny.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int.TryParse(textBox3.Text,out x);
            int.TryParse(textBox4.Text,out y);
      //      int nx =(int)( x * 0.092);
      //      int ny = (int)(y * 0.092);
     //       textBox3.Text = nx.ToString();
     //       textBox4.Text = ny.ToString();
            //nx+=50;
            //ny+=50;
            Program.client.Move(x,y);
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
        double CalcDis(ushort x2,ushort y2,ushort x1,ushort y1)
        {
            int x = Math.Abs(x2 - x1);
            int y = Math.Abs(y2 -y1);
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
                TargetPoint.x = CurPos.x;
                TargetPoint.y = CurPos.y;
               //与目标点对比,如果到了,则获取下一点

                //double dis = CalcDis(CurPos, TargetPoint);
                //if(dis<15.0)
                //{
               //     SetExploredPoint(TargetPoint.x, TargetPoint.y);
                    if (GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y) != 0)
                    {
                        MessageBox.Show("获取不到点了");
                        bWorking = false;
                        break;
                    }
                  //  Console.WriteLine("aaaa");
                    MoveToPoint(TargetPoint.x, TargetPoint.y, CurPos.x, CurPos.y);
               // }
                //X = TargetPoint.x;
                //Y = TargetPoint.y;
                //Program.client.Move(X, Y);
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
            UpdateMap(true);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DrawMap();
        }

        private void button12_Click(object sender, EventArgs e)
        {
         //  PlayerInfo pp= Program.client.GetPlayerInfo();
         //  MessageBox.Show(pp.Level.ToString());
           //Pos pos = Program.client.GetPlayerPos();
           //int nx = (int)(pos.X * 0.092);
           //int ny = (int)(pos.Y * 0.092);
           //textBox1.Text = nx.ToString();
           //textBox2.Text = ny.ToString();

           listView1.Items.Clear();
           List<ObjInfo> round = Program.client.GetRoundListTest();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
//            function FZ_CallUnTargetSkill(X, Y, 技能序号, 操作类型: ULONG): ULONG; stdcall;
//// 对坐标释放技能的CALL
//// 参数1 目的地X坐标 用浮点数乘以0.092后的
//// 参数2 目的地Y坐标 用浮点数乘以0.092后的
//// 参数3 技能序号 左中右QWERT分别对应01234567
//// 参数4 正常施放传8，按SHFIT原地施放传9
//            Program.client.CastUnTargetSkill(int x,int y,int skillNum,castType);
            //double x = 0;
            //double y = 0;
            //double.TryParse(textBox1.Text, out x);
            //double.TryParse(textBox2.Text, out y);
            //Int16 nx = (Int16)(x * 0.092);
            //Int16 ny = (Int16)(y * 0.092);
            short nx = 0;
            short ny = 0;
            short.TryParse(textBox3.Text, out nx);
            short.TryParse(textBox4.Text, out ny);
            Program.client.CastUntargetSkill(nx, ny,3,8);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            double x = 0;
            double y = 0;
            double.TryParse(textBox1.Text, out x);
            double.TryParse(textBox2.Text, out y);
            Int16 nx = (Int16)(x * 0.092);
            Int16 ny = (Int16)(y * 0.092);
            Program.client.CastUntargetSkill(nx, ny, 2, 9);
        }
       
        private void button7_Click(object sender, EventArgs e)
        {
            listView1.Columns.Clear();
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Level");
            listView1.Columns.Add("HP");
            listView1.Columns.Add("MaxHP");
            listView1.Columns.Add("Name");
            listView1.Columns.Add("ObjPtr");
            listView1.Columns.Add("X");
            listView1.Columns.Add("Y");
            listView1.Columns.Add("TypeName");
            listView1.Columns.Add("EnemyID");
            listView1.Columns.Add("Color");

           // int Door = 0;
            listView1.Items.Clear();
            List<ObjInfo> round=Program.client.GetRoundListTest();
            foreach(var item in round)
            {
                var SubItem=listView1.Items.Add(item.ID.ToString());
                SubItem.SubItems.Add(item.Level.ToString());
                SubItem.SubItems.Add(item.HP.ToString());
                SubItem.SubItems.Add(item.MaxHP.ToString());

                sbyte[] bname = item.Name.ToArray();
                byte[] bytes = new byte[bname.Length];
                Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                string strName = Encoding.Unicode.GetString(bytes);
                SubItem.SubItems.Add(strName);


                SubItem.SubItems.Add(item.ObjPtr.ToString());
                SubItem.SubItems.Add(item.X.ToString());
                SubItem.SubItems.Add(item.Y.ToString());

                bname = item.TypeName.ToArray();
                bytes = new byte[bname.Length];
                Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                strName = Encoding.Unicode.GetString(bytes);
                SubItem.SubItems.Add(strName);

                SubItem.SubItems.Add(item.EnemyID.ToString());
                SubItem.SubItems.Add(item.Color.ToString());

                //if (item.Type == 3)
                //    Door++;
            }
            MessageBox.Show(listView1.Items.Count.ToString());
       //     MessageBox.Show(Door.ToString());
        }

       
        //void Explore()
        //{
        //    //与目标点对比,如果到了,则获取下一点
        //    double dis = CalcDis(player.Pos, TargetPoint);
        //    if (dis < 15.0)
        //    {
        //        SetExploredPoint(TargetPoint.x, TargetPoint.y);
        //        if (GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y) != 0)
        //        {
        //            MessageBox.Show("获取不到点了");
        //            return;
        //        }
        //    }
        //    Int32 X, Y;
        //    X = TargetPoint.x;
        //    Y = TargetPoint.y;
        //    Program.client.Move(X, Y);
            
        //}
       
        //void UpdateGameInfo()
        //{
        //    round = Program.client.GetRoundList();
        //    MonsterInfo tempPlayer=Program.client.GetPlayerInfo();
        //    player.HP = tempPlayer.HP;
        //    player.ID = tempPlayer.ID;
        //    player.Level = tempPlayer.Level;
        //    player.MaxHP = tempPlayer.MaxHP;
        //    player.ObjPtr = tempPlayer.ObjPtr;
        //    player.Pos.x = (ushort)tempPlayer.X;
        //    player.Pos.y = (ushort)tempPlayer.Y;
        //}
        //void Analysis()
        //{
        //    double NearestDis=1000000.0;
        //    GPoint MonsterPos=new GPoint();
        //    foreach (var item in round)
        //    {
        //        if(item.ID==player.ID)
        //            continue;
        //        if (item.MaxHP == 0 || item.HP == 0)
        //            continue;
        //        MonsterPos.x = (ushort)item.X;
        //        MonsterPos.y = (ushort)item.Y;
        //        double dis=CalcDis(player.Pos, MonsterPos);
        //        if (dis < NearestDis)
        //        {
        //            AttX = (short)item.X;
        //            AttY = (short)item.Y;
        //            NearestDis = dis;
        //        }
        //    }
        //    //更新自己的信息
        //    //及怪物信息

        //    //分析自己周围有没有怪物,
        //    //有则停止移动
        //    if (NearestDis < 40)
        //    {
        //        if (bAttacking == false)
        //        {
        //            Program.client.StopMove();
        //        }
        //        bAttacking = true;
        //        return;
        //    }
        //    bAttacking = false;
        //    return;
        //}
        //void Attack()
        //{
        //    //向目标点施法
        //    Program.client.CastUntargetSkill(AttX, AttY, 3, 8);
        //    Thread.Sleep(500);
        //}
        //void attackThread()
        //{
        //    //获取人物当前坐标
        //    //初始化TARGETPOINT
        //    Init();
        //    //获取周围怪物列表
        //    while (bWorking)
        //    {
        //        UpdateGameInfo();
        //        //判断是否需要攻击
        //        Analysis();
        //        if (bAttacking)
        //            Attack();
        //        else
        //            Explore();
        //        Thread.Sleep(200);
        //    }
        //}
        Worker worker = new Worker();
        private void button15_Click(object sender, EventArgs e)
        {
            //if (bWorking)
            //    return;
            //bWorking = true;
            //Thread t = new Thread(attackThread);
            //t.Start();
            
            worker.begin();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int obj;
            int.TryParse(textBox1.Text, out obj);

            Program.client.ActiveTarget(obj);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            GetMapData();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            worker.stop();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int x1=0;
            int y1=0;
            int x2=0;
            int y2=0;
            x1 = int.Parse(textBox1.Text);
            y1 = int.Parse(textBox2.Text);
            x2 = int.Parse(textBox3.Text);
            y2 = int.Parse(textBox4.Text);
            GPoint p1 = new GPoint();
            GPoint p2 = new GPoint();
            p1.x =(ushort) x1;
            p1.y = (ushort)y1;
            p2.x = (ushort)x2;
            p2.y = (ushort)y2;
            double dis=CalcDis(p1,p2);
            MessageBox.Show(dis.ToString());
        }

        private void button20_Click(object sender, EventArgs e)
        {
          //  Program.client.IsValidServer();
            
           int n = int.Parse(textBox1.Text);
           Program.client.HitKey(n);
      //     HitKey(n);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ushort x1 = 0;
            ushort y1 = 0;
            ushort x2 = 0;
            ushort y2 = 0;
            x1 = ushort.Parse(textBox1.Text);
            y1 = ushort.Parse(textBox2.Text);
            x2 = ushort.Parse(textBox3.Text);
            y2 = ushort.Parse(textBox4.Text);

            double dis = GetAstarDis(x1, y1, x2, y2);
            MessageBox.Show(dis.ToString());
        }

        double GetBlockCount(GPoint p1,GPoint p2)
        {
            double Astardis = GetAstarDis(p1.x,p1.y, p2.x, p2.y);
            double dis = CalcDis(p1, p2);
            double bi = Astardis / dis;
            return bi;
        }
        private void button22_Click(object sender, EventArgs e)
        {
            GPoint p1 = new GPoint();
            GPoint p2 = new GPoint();
            p1.x = ushort.Parse(textBox1.Text);
            p1.y = ushort.Parse(textBox2.Text);
            p2.x = ushort.Parse(textBox3.Text);
            p2.y = ushort.Parse(textBox4.Text);
            double dis = GetBlockCount(p1, p2);
            MessageBox.Show(dis.ToString());
        }
        private void button23_Click(object sender, EventArgs e)
        {
            string filename = @"d:\mapinfo.txt";
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                return;
            }
            BinaryFormatter binFormat = new BinaryFormatter();

            BattleMapInfo temp = null;
            listView1.Items.Clear();
            while (true)
            {
                try
                {
                    temp = (BattleMapInfo)binFormat.Deserialize(fStream);
                    var SubItem = listView1.Items.Add(temp.strMapName.ToString());
                    SubItem.SubItems.Add(temp.Level.ToString());
                    SubItem.SubItems.Add(temp.MapID.ToString());
                }
                catch (Exception eee)
                {
                    break;
                }
            }
            MessageBox.Show(listView1.Items.Count.ToString());

            //List<WaypointInfo> waypointList = Program.client.GetWaypointInfo();

            //listView1.Items.Clear();
            //BattleMapInfo map = new BattleMapInfo();
            //foreach (var item in waypointList)
            //{

            //    var SubItem = listView1.Items.Add(item.ID.ToString());
            //    SubItem.SubItems.Add(item.Mem.ToString());

            //    sbyte[] bname = item.Name.ToArray();
            //    byte[] bytes = new byte[bname.Length];
            //    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
            //    string strName = Encoding.Unicode.GetString(bytes);
            //    SubItem.SubItems.Add(strName);

            //    bname = item.ActName.ToArray();
            //    bytes = new byte[bname.Length];
            //    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
            //    string strActName = Encoding.Unicode.GetString(bytes);
            //    SubItem.SubItems.Add(strActName);
            //    //string strLevel = strActName.Substring(0, 1);

            //    //map.MapID = item.ID;
            //    //map.Level = short.Parse(strLevel);
            //    //map.strMapName = strName;
            //    //binFormat.Serialize(fStream, map);
            //}
            //MessageBox.Show(waypointList.Count.ToString());
            //fStream.Close();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            int MapID = int.Parse(textBox1.Text);
            int WaypointID = int.Parse(textBox2.Text);
            int FBModel = int.Parse(textBox3.Text);
            Program.client.Transport(MapID, WaypointID, FBModel);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            int nRet=Program.client.ReadLoginState();
            textBox1.Text = nRet.ToString();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            WaypointInfo curMap = Program.client.GetCurrentMapInfo();
            textBox1.Text = curMap.Mem.ToString();
            textBox2.Text = curMap.ID.ToString();

            sbyte[] bname = curMap.Name.ToArray();
            byte[] bytes = new byte[bname.Length];
            Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
            string strName = Encoding.Unicode.GetString(bytes);
            textBox3.Text=strName;


            bname = curMap.ActName.ToArray();
            bytes = new byte[bname.Length];
            Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
            string strActName = Encoding.Unicode.GetString(bytes);
            textBox4.Text = strActName;

        }

        private void button27_Click(object sender, EventArgs e)
        {
            int nWinID = int.Parse(textBox1.Text);
            int ServiceID = int.Parse(textBox2.Text);
            Program.client.UseItem(nWinID, ServiceID);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int nType = int.Parse(textBox1.Text);
            List<ItemInfo> bag = Program.client.GetContainerItemList(nType);
              listView1.Items.Clear();
              listView1.Columns.Clear();
              listView1.Columns.Add("ID");
              listView1.Columns.Add("COLOR");
              listView1.Columns.Add("count");
              listView1.Columns.Add("maxCount");
              listView1.Columns.Add("ServiceID");
              listView1.Columns.Add("Name");
              listView1.Columns.Add("left/Ttop");
              listView1.Columns.Add("width/height");
              listView1.Columns.Add("TypeName");
              listView1.Columns.Add("socket");
              listView1.Columns.Add("connect");
              listView1.Columns.Add("threeColor");

              listView1.Columns.Add("WinID");
              listView1.Columns.Add("BagObjPtr");
              
            foreach(var item in bag)
            {
                var SubItem = listView1.Items.Add(item.ID.ToString());
                SubItem.SubItems.Add(item.Color.ToString());
                SubItem.SubItems.Add(item.Count.ToString());
                SubItem.SubItems.Add(item.MaxCount.ToString());
                SubItem.SubItems.Add(item.ServiceID.ToString());

                if (item.Name != null)
                {
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到名字");
                }

                SubItem.SubItems.Add(item.Left.ToString()+","+item.Top.ToString());
                SubItem.SubItems.Add(item.Width.ToString() + "," + item.Height.ToString());

                if (item.TypeName != null)
                {
                    sbyte[] bname = item.TypeName.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strTypeName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strTypeName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到类型");
                }

                SubItem.SubItems.Add(item.Socket.ToString());
                SubItem.SubItems.Add(item.SocketConnect.ToString());
                SubItem.SubItems.Add(item.ThreeColorSocket.ToString());
             //   SubItem.SubItems.Add(item.Type.ToString());

                SubItem.SubItems.Add(item.WinID.ToString());
                SubItem.SubItems.Add(item.BagObjPtr.ToString());
            }

            MessageBox.Show(bag.Count.ToString());
        }

        private void button29_Click(object sender, EventArgs e)
        {
            int MenuID = int.Parse(textBox1.Text);
            Program.client.ClickNPCMenu(MenuID);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            int WinID = int.Parse(textBox1.Text);
            int ServiceID = int.Parse(textBox2.Text);

            int left = int.Parse(textBox3.Text);
            int top = int.Parse(textBox4.Text);
            Program.client.PutToSell(left,top,ServiceID,WinID);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            Program.client.ConfirmSell();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            int ObjPtr = int.Parse(textBox1.Text);
            TrophyInfo trophy=Program.client.GetTrophyInfo(ObjPtr);
            sbyte[] bname = trophy.Name.ToArray();
            byte[] bytes = new byte[bname.Length];
            Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
            string strName = Encoding.Unicode.GetString(bytes);

            MessageBox.Show(trophy.Color+","+strName+","+trophy.Width.ToString());            
        }

        private void button33_Click(object sender, EventArgs e)
        {
            int nRet=Program.client.GetNearbyWaypointID();
            textBox2.Text = nRet.ToString();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            int NPC = int.Parse(textBox2.Text);
            int nRet = Program.client.GetNearbySellNPCObjPtr(NPC);
            textBox1.Text = nRet.ToString();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            int nRet = Program.client.GetNearbyGoCityTransferDoorObjPtr();
            textBox1.Text = nRet.ToString();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            int nRet = Program.client.GetNearbyGoBattleTransfetDoorObjPtr();
            textBox1.Text = nRet.ToString();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Pos pos = Program.client.GetPlayerPos();
                ushort nx = (ushort)(pos.X * 0.092);
                ushort ny = (ushort)(pos.Y * 0.092);
                GPoint p1 = new GPoint();
                p1.x = nx;
                p1.y = ny;
                GPoint p2 = new GPoint();
                p2.x = 275;
                p2.y = 263;
                double dis = CalcDis(p1, p2);
                if (dis < 15.0)
                    break;
                MoveToSellNPC(nx, ny);
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            UpdateTownMap(275, 263, 186, 164, 153, 231);//第一章
        }

        private void button39_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Pos pos = Program.client.GetPlayerPos();
                ushort nx = (ushort)(pos.X * 0.092);
                ushort ny = (ushort)(pos.Y * 0.092);
                GPoint p1 = new GPoint();
                p1.x = nx;
                p1.y = ny;
                GPoint p2 = new GPoint();
                p2.x = 186;
                p2.y = 164;
                double dis = CalcDis(p1, p2);
                if (dis < 15.0)
                    break;
                MoveToTownWaypoint(nx, ny);
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Pos pos = Program.client.GetPlayerPos();
                ushort nx = (ushort)(pos.X * 0.092);
                ushort ny = (ushort)(pos.Y * 0.092);
                GPoint p1 = new GPoint();
                p1.x = nx;
                p1.y = ny;
                GPoint p2 = new GPoint();
                p2.x = 153;
                p2.y = 231;
                double dis = CalcDis(p1, p2);
                if (dis < 15.0)
                    break;
                MoveToTransferDoor(nx, ny);
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            Program.client.UseTransDoor();
        }

        private void button42_Click(object sender, EventArgs e)
        {
            int num = 0;
            int.TryParse(textBox1.Text, out num);
            Program.client.ActiveStoragePageInfo(num);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            int BagPtr=int.Parse(textBox1.Text);
            int ServiceID=int.Parse(textBox2.Text);
            Program.client.PickupItem(BagPtr,ServiceID);
        }

        private void button44_Click(object sender, EventArgs e)
        {
            int SavePtr = int.Parse(textBox1.Text);
            short Left = short.Parse(textBox2.Text);
            short Top = short.Parse(textBox3.Text);
            Program.client.DropdownItem(SavePtr, Left,Top);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            int nptr=Program.client.GetNearbyStorageObjPtr();
            textBox1.Text = nptr.ToString();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            StartCheck(textBox1.Text, textBox2.Text);
        }

        private void button47_Click(object sender, EventArgs e)
        {
            Program.client.ReturnChoseRole();
        }

        private void button48_Click(object sender, EventArgs e)
        {
            byte[] buff = new byte[128];
         //   string strInfo = new string();
            UInt32 nRet = GetGGPid(buff);
            if(nRet!=0)
            {
                MessageBox.Show("請啟動Garena");

            }
           // textBox1.Text = pid.ToString();
          //  Encoding.ASCII.GetString(buff);
            string strName = Encoding.ASCII.GetString(buff);
            textBox2.Text = strName;
        }

        private void button49_Click(object sender, EventArgs e)
        {
            int nCount = GetGroupCount();
            textBox1.Text = nCount.ToString();
        }

        private void button50_Click(object sender, EventArgs e)
        {
            ushort x = 0;
            ushort y = 0;
            ushort.TryParse(textBox3.Text, out x);
            ushort.TryParse(textBox4.Text, out y);
            int nCooup=GetCurGroup(x, y);
            textBox1.Text = nCooup.ToString();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            UpdateMap(false);
            ushort x = 0;
            ushort y = 0;
            ushort.TryParse(textBox3.Text, out x);
            ushort.TryParse(textBox4.Text, out y);

            ushort a = 2253;
            ushort b = 612;
            double dis=CalcDis(a, b, x, y);
            MessageBox.Show(dis.ToString());
         //   SetPassAbleArea(a, b);
            
         //   int ret=MoveToPoint(a, b, x, y);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            int page = 0;
            int.TryParse(textBox1.Text, out page);
            int nRet=Program.client.ReadStorageUIPTR(page);
            textBox2.Text = nRet.ToString();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            int nx = 0;
            int.TryParse(textBox1.Text, out nx);
            Program.client.CastTargetSkill(nx,3, 8);
        }

        private void button54_Click(object sender, EventArgs e)
        {
            int nRet = Program.client.ReadStoragePageNum();
            textBox2.Text = nRet.ToString();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            int bp = 0;
            int.TryParse(textBox4.Text, out bp);
            int ContainerObjPtr = Program.client.GetStoragePagePtr(bp);
            textBox1.Text = ContainerObjPtr.ToString() ;
        }

        private void button56_Click(object sender, EventArgs e)
        {
            int SavePtr = int.Parse(textBox1.Text);
            int TargetServiceID = int.Parse(textBox2.Text);
            Program.client.DropdownItemStack(SavePtr, TargetServiceID);
        }

        //
        const short StorageWidth = 12;
        const short StorageHeight = 12;
        const short byTrophy_Currency = 2;
        const short byTrophy_Money = 12;
        int SaveToStorage(List<ItemInfo> SaveList)
        {
            int FailCount = 0;
            foreach (var item in SaveList)
            {
                StorageSpace spc = null;
                //搜索空间,找到则直接放
                if (item.Type == byTrophy_Currency || item.Type == byTrophy_Money)
                {
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    spc = SearchSpaceInStorageWithCount(item.Width, item.Height, strName, item.Count);
                }
                else
                    spc = SearchSpaceInStorageWithCount(item.Width, item.Height, null, 0);
                //   StorageSpace spc=SearchSpaceInStorage(item.Width,item.Height);
                if (spc == null)
                {
                    FailCount++;
                    continue;
                }

                Program.client.PickupItem(item.BagObjPtr, item.ServiceID);
                Thread.Sleep(500);
                if (spc.StackObjPtr != 0)
                {
                    Program.client.DropdownItemStack(spc.ContainerObjPtr, spc.StackObjPtr);
                    Thread.Sleep(500);
                    if (spc.Left > 0)
                    {
                        Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
                    }
                }
                else
                {
                    Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
                }
                Thread.Sleep(500);
            }
            return FailCount;
        }
        StorageSpace SearchSpaceInStorageWithCount(short width, short height, string strName, int nCount)
        {
            StorageSpace ret = null;
            //FZ_ReadStorageUIPTR(i);//读取仓库页码]
            int PageCount = Program.client.ReadStoragePageNum();
            for (int bp = 0; bp < PageCount; ++bp)
            {
                Program.client.ActiveStoragePageInfo(bp);
                Thread.Sleep(1000);
                ////////////////////////////////////////计算空间
                List<ItemInfo> Storage = Program.client.GetContainerItemList(bp + 10);

                byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
                foreach (var item in Storage)
                {
                    if (strName != null)
                    {
                        if (ret == null)
                        {
                            sbyte[] bname = item.Name.ToArray();
                            byte[] bytes = new byte[bname.Length];
                            Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                            string strItemName = Encoding.Unicode.GetString(bytes);
                            if (strItemName == strName)
                            {
                                if (item.Count < item.MaxCount)
                                {
                                    ret = new StorageSpace();
                                    ret.StackObjPtr = item.ServiceID;
                                    ret.ContainerObjPtr = Program.client.GetStoragePagePtr(bp + 10);
                                    if ((item.Count + nCount) <= item.MaxCount)
                                    {
                                        return ret;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < item.Width; ++i)
                    {
                        for (int j = 0; j < item.Height; ++j)
                        {
                            storageSpace[item.Left + i, item.Top + j] = 1;
                        }
                    }
                }
                //搜索空闲空间
                for (short i = 0; i <= (StorageWidth - width); ++i)
                {
                    for (short j = 0; j <= (StorageHeight - height); ++j)
                    {
                        bool bFind = true;
                        for (short m = 0; m < width; ++m)
                        {
                            for (short n = 0; n < height; ++n)
                            {
                                if (storageSpace[i + m, j + n] == 1)
                                {
                                    bFind = false;
                                    break;
                                }
                            }
                            if (bFind == false)
                                break;
                        }
                        if (bFind)
                        {
                            if (ret == null)
                                ret = new StorageSpace();
                            ret.Left = i;
                            ret.Top = j;
                            ret.ContainerObjPtr = Program.client.GetStoragePagePtr(bp + 10);
                            return ret;
                        }
                    }
                }
            }
            return ret;
        }
        private void button57_Click(object sender, EventArgs e)
        {
            List<ItemInfo> saveList = GenSaveList();
            Thread.Sleep(1000);
            int nRet = SaveToStorage(saveList);
            Thread.Sleep(1000);
        }

        private void button58_Click(object sender, EventArgs e)
        {
            string filename = "Config\\default.cfg" ;
            if (filename.Length < 1)
            {
                MessageBox.Show("請選擇一個配置方案,首次使用,點擊設定配置以創建新配置方案");
                return;
            }
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryFormatter binFormat = new BinaryFormatter();
                Program.config = (ConfigData)binFormat.Deserialize(fStream);
                fStream.Close();

                //设置运行时配置,全局配置在程序载入时即加载进内存
                //lbMap.Items.Clear();
                //foreach (var item in Program.config.MissionMapList)
                //{
                //    lbMap.Items.Add(item);
                //}

                List<LootType> lootTypeList = new List<LootType>();
                foreach (var item in Program.config.LootTypeList)
                {
                    LootType newItem = new LootType();
                    newItem.Type = item.Key;
                    newItem.Color = item.Value;
                    lootTypeList.Add(newItem);
                }
                // Program.client.SetLootTypeList(lootTypeList);
                Program.client.SetLootTypeList(
                       lootTypeList,
                       Program.config.LootSocketFilter,
                       Program.config.LootSocketConnectFilter,
                       Program.config.LootThreeColor,
    Program.config.LootSkillQuality);

                Program.runtime = new RunTimeData();
              //  lbMap.SelectedIndex = 0;
                //   Program.runtime.curMissionMapIndex = 0;
              //  lbNotice.Text = "使用中配置:" + cbConfig.Text + ",如果您修改了該配置方案或選擇了其他配置方案,請點擊重載配置.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("載入配置文件失敗,文件不存在或已損壞,請重新創建");
                return;
            }
        }
        const short byTrophy_Armour = 4;//装备
        const short byTrophy_Ring = 5;//戒指
        const short byTrophy_Amulet = 6;//项链
        const short byTrophy_Belt = 7;//腰带
        const short byTrophy_Weapon = 8;//武器
        List<ItemInfo> GenSaveList()
        {
            //遍历背包,找到装备,售卖
            //
            //   List<ItemInfo> GobackCurrency = new List<ItemInfo>();
            ItemInfo MaxCurrency = null;
            List<ItemInfo> SaveList = new List<ItemInfo>();
            List<ItemInfo> bag = Program.client.GetContainerItemList(0);
            foreach (var item in bag)
            {
                int FilterColor = 0;
                if (item.Type == byTrophy_Currency)//回城卷轴,存储
                {
                    if (Program.config.SaveTypeList.TryGetValue(item.Type, out FilterColor) == false)
                        continue;
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    if (strName == "傳送卷軸")//如果是傳送卷軸,保存20个以上
                    {
                        if (MaxCurrency == null)
                            MaxCurrency = item;
                        else
                        {
                            if (item.Count > MaxCurrency.Count)
                            {
                                SaveList.Add(MaxCurrency);
                                MaxCurrency = item;
                            }
                        }
                        continue;

                    }
                    SaveList.Add(item);
                    continue;
                }
                if (item.Type != byTrophy_Armour && item.Type != byTrophy_Weapon)//没孔没槽的
                {
                    if (Program.config.SaveTypeList.TryGetValue(item.Type, out FilterColor) == false)
                        continue;
                    if (FilterColor > item.Color)
                        continue;
                    SaveList.Add(item);
                }
                else
                {
                    //这里的逻辑有点乱啊
                    if (Program.config.SaveSocketFilter > 0 )
                    {
                        if (Program.config.SaveSocketFilter <= item.Socket)
                        {
                            SaveList.Add(item);
                            continue;
                        }
                    }
                    if (Program.config.SaveSocketConnectFilter > 0)
                    {
                        if (Program.config.SaveSocketConnectFilter <= item.SocketConnect)
                        {
                            SaveList.Add(item);
                            continue;
                        }
                    }
                    if (item.ThreeColorSocket && Program.config.SaveThreeColor)
                    {
                        SaveList.Add(item);
                        continue;
                    }
                    if (Program.config.SaveTypeList.TryGetValue(item.Type, out FilterColor) == false)
                        continue;
                    if (FilterColor > item.Color)
                        continue;
                    SaveList.Add(item);
                }
            }
            return SaveList;
        }
  
        const sbyte TrophyType = 4;
        private void button59_Click(object sender, EventArgs e)
        {
            List<ObjInfo> round = Program.client.GetRoundList();
            List<TrophyBaseInfo> trophyIDList = new List<TrophyBaseInfo>();
            foreach (var item in round)
            {
                switch (item.Type)
                {
                    case TrophyType:
                        TrophyBaseInfo trophy = new TrophyBaseInfo();
                        trophy.ObjPtr = item.ObjPtr;
                        trophy.X = (short)item.X;
                        trophy.Y = (short)item.Y;
                        trophyIDList.Add(trophy);
                        break;
                }
            }
            List<TrophyInfo> sell = Program.client.GetTrophyList(trophyIDList);

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("ID");
            listView1.Columns.Add("COLOR");
            //listView1.Columns.Add("count");
            //listView1.Columns.Add("maxCount");
            //listView1.Columns.Add("ServiceID");
            listView1.Columns.Add("Name");
         //   listView1.Columns.Add("left/Ttop");
            listView1.Columns.Add("width/height");
         //   listView1.Columns.Add("TypeName");
            listView1.Columns.Add("socket");
            listView1.Columns.Add("connect");
            listView1.Columns.Add("threeColor");

            listView1.Columns.Add("type");
         //   listView1.Columns.Add("BagObjPtr");

           // List<TrophyInfo> sell = Program.client.GetTrophyList();
          //  List<ItemInfo> sell=GenSaveList();
            foreach(var item in sell)
            {
                var SubItem = listView1.Items.Add(item.ID.ToString());
                SubItem.SubItems.Add(item.Color.ToString());
                //SubItem.SubItems.Add(item.Count.ToString());
                //SubItem.SubItems.Add(item.MaxCount.ToString());
                //SubItem.SubItems.Add(item.ServiceID.ToString());

                if (item.Name != null)
                {
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到名字");
                }

            //    SubItem.SubItems.Add(item.Left.ToString() + "," + item.Top.ToString());
                SubItem.SubItems.Add(item.Width.ToString() + "," + item.Height.ToString());

                //if (item.TypeName != null)
                //{
                //    sbyte[] bname = item.TypeName.ToArray();
                //    byte[] bytes = new byte[bname.Length];
                //    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                //    string strTypeName = Encoding.Unicode.GetString(bytes);
                //    SubItem.SubItems.Add(strTypeName);
                //}
                //else
                //{
                //    SubItem.SubItems.Add("未获取到类型");
                //}

                SubItem.SubItems.Add(item.Socket.ToString());
                SubItem.SubItems.Add(item.SocketConnect.ToString());
                SubItem.SubItems.Add(item.ThreeColorSocket.ToString());
                SubItem.SubItems.Add(item.Type.ToString());

                //    SubItem.SubItems.Add(item.WinID.ToString());
               // SubItem.SubItems.Add(item.BagObjPtr.ToString());
            }
        }

        private void button60_Click(object sender, EventArgs e)
        {
            int n = 0;
            int.TryParse(textBox1.Text,out n);
            int item = 0;
            int.TryParse(textBox2.Text, out item);
            Program.client.IdentityItem(n,item);
        }
        List<ItemInfo> GenIdentityList()
        {
            List<ItemInfo> IdentityList = new List<ItemInfo>();
            List<ItemInfo> bag = Program.client.GetContainerItemList(0);
            foreach (var item in bag)
            {
                if (item.NeedIdentify)
                    IdentityList.Add(item);
            }
            return IdentityList;
        }
        int SearchIdentityCurrencyServiceID()
        {
            int nServiceID = -1;
            List<ItemInfo> bag = Program.client.GetContainerItemList(0);
            foreach (var item in bag)
            {
                if (item.Type == byTrophy_Currency)
                {
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    if (strName == "知識卷軸")//如果是傳送卷軸,保存20个以上
                    {
                        nServiceID = item.ServiceID;
                        break;
                    }
                }
            }
            return nServiceID;
        }
        void IdentityItem(List<ItemInfo> IdentityList)
        {
            foreach (var item in IdentityList)
            {
                int nServiceID = SearchIdentityCurrencyServiceID();
                if (nServiceID < 0)
                    break;
                Program.client.IdentityItem(nServiceID, item.ServiceID);
                Thread.Sleep(500);
            }
        }
        private void button61_Click(object sender, EventArgs e)
        {
            List<ItemInfo> IdentityList = GenIdentityList();
            IdentityItem(IdentityList);
        }
        TrimInfo SearchTrimItem()
        {
            TrimInfo ret = null;
            Dictionary<string, ItemInfo> Memory = new Dictionary<string, ItemInfo>();
            //遍历仓库页,分别整理每一页
            int PageCount = Program.client.ReadStoragePageNum();
            for (int bp = 0; bp < PageCount; ++bp)
            {
                Program.client.ActiveStoragePageInfo(bp);
                Thread.Sleep(1000);
                Memory.Clear();
                ////////////////////////////////////////检查物品
                List<ItemInfo> Storage = Program.client.GetContainerItemList(bp + 10);

                //如果是未满项,加入到队列

                foreach (var item in Storage)
                {
                    if (item.Count < item.MaxCount)
                    {
                        sbyte[] bname = item.Name.ToArray();
                        byte[] bytes = new byte[bname.Length];
                        Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                        string strItemName = Encoding.Unicode.GetString(bytes);
                        if (Memory.ContainsKey(strItemName))//包含
                        {
                            ret = new TrimInfo();
                            ret.StoragePtr = item.BagObjPtr;
                            ret.TargetServiceID = Memory[strItemName].ServiceID;
                            ret.SourceServiceID = item.ServiceID;
                            ret.SourceTop = item.Top;
                            ret.SourceLeft = item.Left;
                            break;
                        }
                        else//不包含
                        {
                            Memory.Add(strItemName, item);
                        }
                    }
                }
                if (ret != null)
                    break;
            }
            //记录可堆叠的,数量未满的,名称为KEY
            //如果有2个相同的,则返回
            //
            Memory.Clear();
            return ret;
        }
        int TrimStorage()
        {
            int nRet = 0;
            while (true)
            {
                TrimInfo item = SearchTrimItem();
                if (item == null)
                    break;
                //拿起物品
                Program.client.PickupItem(item.StoragePtr, item.SourceServiceID);
                Thread.Sleep(100);
                //堆叠放

                Program.client.DropdownItemStack(item.StoragePtr, item.TargetServiceID);
                Thread.Sleep(100);
                //是否需要放回原位置,需要的话再来个空间放

                if (item.SourceLeft >= 0)
                {
                    Program.client.DropdownItem(item.StoragePtr, item.SourceLeft, item.SourceTop);
                }

                Thread.Sleep(100);

                if (Program.client.IsItemOnMouse())
                {
                    //   System.Windows.Forms.MessageBox.Show("drop error!");
                    nRet = 1;
                    Program.client.ReturnChoseRole();
                    break;
                }
            }
            return nRet;
        }
        private void button62_Click(object sender, EventArgs e)
        {
            TrimStorage();
        }

        private void button63_Click(object sender, EventArgs e)
        {
            Program.client.UpSkill();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            int skill = 0;
            int.TryParse(textBox1.Text, out skill);
            bool bRet=Program.client.IsBuffExists(skill);
            textBox2.Text = bRet.ToString();
        }

        private void button65_Click(object sender, EventArgs e)
        {
            bool bRet = Program.client.IsItemOnMouse();
            textBox2.Text = bRet.ToString();
        }
        int PutToSellWindow(ItemInfo item)
        {
            int nRet = 0;

            nRet = Program.client.PutToSell(item.Top, item.Left, item.ServiceID, item.WinID);
            //if (nRet != 0)
            //    break;
            Thread.Sleep(1000);
            return nRet;
        }
        ItemInfo GetSellItem()
        {
            ItemInfo NeedSellItem = null;
            List<ItemInfo> bag = Program.client.GetContainerItemList(0);
            List<ItemInfo> SelledList = Program.client.GetContainerItemList(1);
            HashSet<GPoint> SelledPos = new HashSet<GPoint>();
            foreach (var item in SelledList)
            {
                GPoint temp = new GPoint();
                temp.x = (ushort)item.Top;
                temp.y = (ushort)item.Left;
                SelledPos.Add(temp);
            }
            GPoint temp1 = new GPoint();
            foreach (var item in bag)
            {
                int FilterColor = 0;
                temp1.x = (ushort)item.Top;
                temp1.y = (ushort)item.Left;
                if (SelledPos.Contains(temp1))
                    continue;
                //如果不是装备.是技能石1,卷轴2,血瓶3,地图,
                if (item.Type != 4 && item.Type != 8 && item.Type != 9)
                {
                    if (Program.config.SellTypeList.TryGetValue(item.Type, out FilterColor) == false)
                        continue;
                    if (FilterColor < item.Color)
                        continue;
                    NeedSellItem = item;
                    break;
                }
                else//有槽的
                {
                    if (Program.config.SellSocketFilter > 0)
                    {
                        if (Program.config.SellSocketFilter >= item.Socket)
                        {
                            NeedSellItem = item;
                            break;
                        }
                    }
                    if (Program.config.SellSocketConnectFilter > 0)
                    {
                        if (Program.config.SellSocketConnectFilter >= item.SocketConnect)
                        {
                            NeedSellItem = item;
                            break;
                        }
                    }
                    if (item.ThreeColorSocket && Program.config.SellThreeColor)
                    {
                        NeedSellItem = item;
                        break;
                    }

                    if (Program.config.SellTypeList.TryGetValue(item.Type, out FilterColor) == false)
                        continue;
                    if (FilterColor < item.Color)
                        continue;
                    NeedSellItem = item;
                    break;
                }
            }
            SelledPos.Clear();
            return NeedSellItem;
        }
        private void button66_Click(object sender, EventArgs e)
        {
            while (true)
            {
                ItemInfo NeedSellItem = GetSellItem();
                if (NeedSellItem == null)
                {
                    Program.client.ConfirmSell();
                    Thread.Sleep(1000);
                    Program.client.HitKey(7);
                    break;

                }
                PutToSellWindow(NeedSellItem);
                Thread.Sleep(1000);
            }
        }

        private void button67_Click(object sender, EventArgs e)
        {
            int nRet=Program.client.ReloadPollutantGateName();
            MessageBox.Show(nRet.ToString());
        }

        private void button68_Click(object sender, EventArgs e)
        {
            int nRet=Program.client.LogGateName();
            MessageBox.Show(nRet.ToString());
        }

        private void button69_Click(object sender, EventArgs e)
        {
           int ObjPtr= Program.client.GetNearbyPollutantGateObjPtr();
           textBox1.Text = ObjPtr.ToString();
        }

        private void button70_Click(object sender, EventArgs e)
        {
            List<ItemFullInfo> bag = Program.client.GetBagItemFullInfo();
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("ID");
            listView1.Columns.Add("COLOR");
            listView1.Columns.Add("count");
            listView1.Columns.Add("maxCount");
            listView1.Columns.Add("ServiceID");
            listView1.Columns.Add("Name");
            listView1.Columns.Add("left/Ttop");
            listView1.Columns.Add("width/height");
            listView1.Columns.Add("TypeName");
            listView1.Columns.Add("socket");
            listView1.Columns.Add("connect");
            listView1.Columns.Add("threeColor");

            listView1.Columns.Add("品质");
            listView1.Columns.Add("属性");

            listView1.Columns.Add("WinID");
            listView1.Columns.Add("BagObjPtr");

            foreach (var item in bag)
            {
                var SubItem = listView1.Items.Add(item.ID.ToString());
                SubItem.SubItems.Add(item.Color.ToString());
                SubItem.SubItems.Add(item.Count.ToString());
                SubItem.SubItems.Add(item.MaxCount.ToString());
                SubItem.SubItems.Add(item.ServiceID.ToString());

                if (item.Name != null)
                {
                    sbyte[] bname = item.Name.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到名字");
                }

                SubItem.SubItems.Add(item.Left.ToString() + "," + item.Top.ToString());
                SubItem.SubItems.Add(item.Width.ToString() + "," + item.Height.ToString());

                if (item.TypeName != null)
                {
                    sbyte[] bname = item.TypeName.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strTypeName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strTypeName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到类型");
                }

                SubItem.SubItems.Add(item.Socket.ToString());
                SubItem.SubItems.Add(item.SocketConnect.ToString());
                SubItem.SubItems.Add(item.ThreeColorSocket.ToString());
                SubItem.SubItems.Add(item.Quality.ToString());

                if (item.DescribInfo != null)
                {
                    sbyte[] bname = item.DescribInfo.ToArray();
                    byte[] bytes = new byte[bname.Length];
                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                    string strName = Encoding.Unicode.GetString(bytes);
                    SubItem.SubItems.Add(strName);
                }
                else
                {
                    SubItem.SubItems.Add("未获取到名字");
                }

                SubItem.SubItems.Add(item.WinID.ToString());
                SubItem.SubItems.Add(item.BagObjPtr.ToString());
            }
        }

        private void button71_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            listView1.Columns.Clear();
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Text");
            
             List<NPCMenuInfo> menu =Program.client.GetNPCMenuInfo();
            foreach(var item in menu)
            {
                ListViewItem SubItem=listView1.Items.Add(item.ID.ToString());

                sbyte[] bname = item.Text.ToArray();
                byte[] bytes = new byte[bname.Length];
                Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                string strName = Encoding.Unicode.GetString(bytes);
                SubItem.SubItems.Add(strName);
            }
        }

        private void button72_Click(object sender, EventArgs e)
        {
            int nID = Program.client.GetNearbyWaypointObjPtr();
            Program.client.ActiveTarget(nID);
            Thread.Sleep(1000);
            Program.client.TransHideHome();
            //Program.client.TransHideHome();
        }

        private void button73_Click(object sender, EventArgs e)
        {
           int Pos= Program.client.GetNearbyWaypointPos();
           int X = Pos >> 16;
            int Y=Pos&65535;
            textBox1.Text = X.ToString();
            textBox2.Text = Y.ToString();
        }

        private void button75_Click(object sender, EventArgs e)
        {
            long Pos = Program.client.GetNearbySellNPCPos();
            long nPos = Program.client.GetNearbySellNPCPos();
            int NPCNum = (int)(nPos >> 32);
            short x = (short)((nPos >> 16) & 65535);
            short y = (short)(nPos & 65535); 
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
            textBox3.Text = NPCNum.ToString();
        }

        private void button76_Click(object sender, EventArgs e)
        {
            int Pos = Program.client.GetNearbyStoragePos();
            int X = Pos >> 16;
            int Y = Pos & 65535;
            textBox1.Text = X.ToString();
            textBox2.Text = Y.ToString();
        }

        private void button77_Click(object sender, EventArgs e)
        {
            Program.client.ClickNPCSellMenu();
        }
        bool InDungeonHome()//是否在藏身处
        {
            int CurMapID=Program.client.GetCurrentMapID();
            return Program.gdata.AllDungeonMapID.Contains(CurMapID);
        }
        private void button78_Click(object sender, EventArgs e)
        {
            bool bRet=InDungeonHome();
            textBox1.Text = bRet.ToString();
        }
    }
#endif
}
