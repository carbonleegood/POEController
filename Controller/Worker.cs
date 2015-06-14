using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Thrift.GameCall;
using System.Threading;
using System.Runtime.InteropServices;
namespace Controller
{
    public partial class Worker
    {
#if Protect
        [DllImport("llllllll.dll", EntryPoint = "HitKey", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void HitKey(int x);

        [DllImport("llllllll.dll", EntryPoint = "GetExplorePoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetExplorePoint(ref UInt16 x, ref UInt16 y);

        [DllImport("llllllll.dll", EntryPoint = "SetExploredPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetExploredPoint(UInt16 x, UInt16 y);

        [DllImport("llllllll.dll", EntryPoint = "UpdateMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void UpdateMap();

        [DllImport("llllllll.dll", EntryPoint = "DrawMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void DrawMap();

        [DllImport("llllllll.dll", EntryPoint = "GetAstarDis", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetAstarDis(UInt16 x, UInt16 y, UInt16 px, UInt16 py);

        [DllImport("llllllll.dll", EntryPoint = "UpdateTownMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 UpdateTownMap(UInt16 npcx, UInt16 npcy, UInt16 wpx, UInt16 wpy, UInt16 tdx, UInt16 tdy, UInt16 sx, UInt16 sy);

        [DllImport("llllllll.dll", EntryPoint = "MoveToSellNPC", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToSellNPC(UInt16 x, UInt16 y);

        [DllImport("llllllll.dll", EntryPoint = "MoveToTownWaypoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTownWaypoint(UInt16 x, UInt16 y);

        [DllImport("llllllll.dll", EntryPoint = "MoveToTransferDoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTransferDoor(UInt16 x, UInt16 y);

        [DllImport("llllllll.dll", EntryPoint = "MoveToStorage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToStorage(UInt16 x, UInt16 y);
//////////////////
        [DllImport("MapServer.dll", EntryPoint = "HitKey", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void HitKey(int x);

        [DllImport("MapServer.dll", EntryPoint = "GetExplorePoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetExplorePoint(ref UInt16 x, ref UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "SetExploredPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetExploredPoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "UpdateMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void UpdateMap();

        [DllImport("MapServer.dll", EntryPoint = "DrawMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void DrawMap();

        [DllImport("MapServer.dll", EntryPoint = "GetAstarDis", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetAstarDis(UInt16 x, UInt16 y, UInt16 px, UInt16 py);

        [DllImport("MapServer.dll", EntryPoint = "UpdateTownMap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 UpdateTownMap(UInt16 npcx, UInt16 npcy, UInt16 wpx, UInt16 wpy, UInt16 tdx, UInt16 tdy, UInt16 sx, UInt16 sy);

        [DllImport("MapServer.dll", EntryPoint = "MoveToSellNPC", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToSellNPC(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTownWaypoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTownWaypoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTransferDoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTransferDoor(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToStorage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToStorage(UInt16 x, UInt16 y);
#endif   
        [DllImport("MapServer.dll", EntryPoint = "helloCraker", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void HitKey(int x);
        // int GetExplorePoint(ULONG* px, ULONG* py);

        [DllImport("MapServer.dll", EntryPoint = "GetCrakerjjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetPollutantExplorePoint(ref UInt16 x, ref UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "GetCrakerjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetExplorePoint(ref UInt16 x, ref UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "CrakerMujj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetGroupExplorePoint(ref UInt16 x, ref UInt16 y, int Group);

        [DllImport("MapServer.dll", EntryPoint = "SetCrakerBigJJ", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetGroupExploredPoint(UInt16 x, UInt16 y,int Group);

        [DllImport("MapServer.dll", EntryPoint = "SetCrakerDajijiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetPollutantExploredPoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "SetCrakerDajiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetExploredPoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "GetCrakerJJ", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetCurGroup(ushort x,ushort y);

        [DllImport("MapServer.dll", EntryPoint = "CutCrakerDajijiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PollutantMoveToPoint(UInt16 x, UInt16 y, UInt16 px, UInt16 py);

        [DllImport("MapServer.dll", EntryPoint = "CutCrakerDajiji", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToPoint(UInt16 x, UInt16 y, UInt16 px, UInt16 py);
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
        public static extern Int32 UpdateTownMap(UInt16 npcx, UInt16 npcy, UInt16 wpx, UInt16 wpy, UInt16 tdx, UInt16 tdy, UInt16 sx, UInt16 sy);

        [DllImport("MapServer.dll", EntryPoint = "MoveToSellNPC", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToSellNPC(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTownWaypoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTownWaypoint(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "MoveToTransferDoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToTransferDoor(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "GGGGGG", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 StartCheck(string uid, string pwd, int nTemp);

        [DllImport("MapServer.dll", EntryPoint = "GetGGPid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 GetGGPid(byte[] buff);

        [DllImport("MapServer.dll", EntryPoint = "GetCrakerJJCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetGroupCount();

        [DllImport("MapServer.dll", EntryPoint = "MoveToStorage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MoveToStorage(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "jjjjjjjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SetPollutantPassAbleArea(UInt16 x, UInt16 y);

        [DllImport("MapServer.dll", EntryPoint = "jjjjjjj", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SetPassAbleArea(UInt16 x, UInt16 y);
        /// ///////////////////////////////////////////////
        /// 
        bool ChangeToNextMissionMap()
        {
            FilterMonsterList.Clear();
            PassedDoor.Clear();
            areaCrossList.Clear();
            AddedCrossPoint.Clear();
            ClickedBoxList.Clear();
            LootFilter.Clear();
            Program.client.ClearTrophyFilter();

            bGroupExploreModel = false;//判断
            bNeedCross = false;//判断探索还是穿越
            bCrossing = false;//判断穿越中还是移动到穿越点


            Program.runtime.curMissionMapIndex++;
            if (Program.config.MissionMapList.Count == Program.runtime.curMissionMapIndex)
                Program.runtime.curMissionMapIndex = 0;
            string strKey = Program.config.MissionMapList[Program.runtime.curMissionMapIndex];
            if (Program.gdata.AllBattleMap.TryGetValue(strKey, out CurMissionMap) == false)
            {
                CurMissionMap = null;
                System.Windows.Forms.MessageBox.Show("Test");
                return false;
            }
            if (CurMissionMap.MapID == 9837 || CurMissionMap.MapID == 63025 || CurMissionMap.MapID == 21866)
                bGroupExploreModel = true;
            else
                bGroupExploreModel = false;
            return true;
        }
        bool  Init()
        {
            Pos pos = Program.client.GetPlayerPos();
            TargetPoint.x = (ushort)(pos.X * 0.092);
            TargetPoint.y = (ushort)(pos.Y * 0.092);
         //   GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y);
            ClickedBoxList.Clear();

            //设置初始状态,拿到任务地图列表,当前任务地图
            string strKey=Program.config.MissionMapList[Program.runtime.curMissionMapIndex];
            if(Program.gdata.AllBattleMap.TryGetValue(strKey, out CurMissionMap)==false)
            {
                System.Windows.Forms.MessageBox.Show("Test");
                return false;
            }
            if (CurMissionMap.MapID == 9837 || CurMissionMap.MapID == 63025 || CurMissionMap.MapID == 21866)
                bGroupExploreModel = true;
            else
                bGroupExploreModel = false;
            //Program.runtime.curMissionMapIndex;

             bNeedCastBattleOnceSkill = true;

             Selling = false;
             Saveing = false;
         //   bNeedSave = true;
         //   bSaledComplete = false;
            //设置起始状态
            curStatus = Status.Mission;
            Program.client.ClearTrophyFilter();
            return true;
        }
        bool HandInit()
        {
            Pos pos = Program.client.GetPlayerPos();
            TargetPoint.x = (ushort)(pos.X * 0.092);
            TargetPoint.y = (ushort)(pos.Y * 0.092);
            //   GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y);
            ClickedBoxList.Clear();

            //设置初始状态,拿到任务地图列表,当前任务地图
        
            bGroupExploreModel = false;
            //Program.runtime.curMissionMapIndex;

            bNeedCastBattleOnceSkill = true;

            //   bNeedSave = true;
            //   bSaledComplete = false;
            //设置起始状态
            curStatus = Status.Mission;
            Program.client.ClearTrophyFilter();
            return true;
        }
        const sbyte MonsterType=1;
        const sbyte ChestType=2;
        const sbyte DoorType=3;
        const sbyte TrophyType=4;
        const sbyte WaypointType=5;//传送门
        const sbyte AreaCrossType=7;//地图连接门
        const sbyte PollutantGateType = 9;//地图连接门

        void UpdateGameInfo()
        {
            round = Program.client.GetRoundList();
            monsterList.Clear();
            boxList.Clear();
            doorList.Clear();
            trophyList.Clear();
            trophyIDList.Clear();
            NearbyPollutantGatePos = null;
            foreach(var item in round)
            {
                //掉落物品
                //门
                //桶箱
                //传送门
                switch (item.Type)
                {
                    case MonsterType:
                        SMonsterInfo monster = new SMonsterInfo(item);
                        monsterList.Add(monster);
                        break;
                    case ChestType:
                        SBox box = new SBox(item);
                        boxList.Add(box);
                        break;
                    case DoorType:
                        SDoor door = new SDoor(item);
                        doorList.Add(door);
                        break;
                    case TrophyType:
                        TrophyBaseInfo trophy = new TrophyBaseInfo();
                        trophy.ObjPtr = item.ObjPtr;
                        trophy.X =(short) item.X;
                        trophy.Y = (short)item.Y;
                        trophyIDList.Add(trophy);
                        break;
                    case AreaCrossType:
                        AreaCrossInfo areaCross=new AreaCrossInfo(item);
                        areaCrossList.Add(areaCross);
                        break;
                    case WaypointType:
                        break;
                    case PollutantGateType:
                        NearbyPollutantGatePos=new GPoint();
                        NearbyPollutantGatePos.x = (ushort)item.X;
                        NearbyPollutantGatePos.y = (ushort)item.Y;
                        break;
                }
            }
            CurMapID = Program.client.GetCurrentMapID();
            trophyList = Program.client.GetTrophyList(trophyIDList);

            PlayerInfo tempPlayer = Program.client.GetPlayerInfo();
            player.Level = tempPlayer.Level;
            player.HP = tempPlayer.HP;
            player.MaxHP = tempPlayer.MaxHP;
            player.MP = tempPlayer.MP;
            player.Shield = tempPlayer.Shield;
            player.MaxMP = tempPlayer.MaxMP;
            player.Pos.x = (ushort)tempPlayer.X;
            player.Pos.y = (ushort)tempPlayer.Y;
        }
        void WorkThread(object o)
        {
            //获取人物当前坐标
            //初始化TARGETPOINT
            Init();//启动一些后台检测线程,比如说任务是否完成等
            //获取周围怪物列表
            while (bWorking)
            {
                int nLogin=Program.client.ReadLoginState();
                if (nLogin==3)//读图中
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (nLogin == 1)
                {
                   // HitKey(6);
                    Program.client.HitKey(6);
                    Thread.Sleep(3000);
                    continue;
                }
                if (nLogin == 0)
                {
                    System.Windows.Forms.MessageBox.Show("出错了,请重启游戏!");
                    continue;
                }
                nSleepTime = 150;
                UpdateGameInfo();
                //判断是否需要攻击
                if (player.HP == 0)
                {
                    if (Program.config.bBattleRelive)
                        Program.client.Relive(0);
                    else
                        Program.client.Relive(1);
                    Thread.Sleep(1000);
                    continue;
                }
                Analysis();
           //     DoAction();
                Thread.Sleep(nSleepTime);
            }
          //  setevent();
        }
        void HandWorkThread(object o)
        {
            //获取人物当前坐标
            //初始化TARGETPOINT
            HandInit();//启动一些后台检测线程,比如说任务是否完成等
            //获取周围怪物列表
            while (bWorking)
            {
                int nLogin = Program.client.ReadLoginState();
                if (nLogin == 3)//读图中
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (nLogin == 1)
                {
                    // HitKey(6);
                    Program.client.HitKey(6);
                    Thread.Sleep(3000);
                    continue;
                }
                if (nLogin == 0)
                {
                    System.Windows.Forms.MessageBox.Show("出错了,请重启游戏!");
                    continue;
                }
                nSleepTime = 150;
                UpdateGameInfo();
                //判断是否需要攻击
                if (player.HP == 0)
                {//半自动模式,只在记录点复活

                    Program.client.Relive(0);

                    Thread.Sleep(1000);
                    continue;
                }
                HandAnalysis();
                //     DoAction();
                Thread.Sleep(nSleepTime);
            }
            //  setevent();
        }
        public bool bHandModel = false;
        public void begin()
        {
            if (bWorking)
                return;
            bWorking = true;
            Thread t =null;
            if(bHandModel)
                t = new Thread(HandWorkThread);
            else
             t = new Thread(WorkThread);
            t.IsBackground = true;
            t.Start();
        }
        public void stop()
        {
            bWorking=false;
        }

        void Analysis()
        {
           int bRet=0;
            do
            {
                switch (curStatus)//在相应的状态,就切换到那个状态的分析器
                {
                    case Status.Login://登录中
                        bRet=LoginAnalysis();
                        break;
                    case Status.Resurrection://复活中
                        bRet=ResurrectionAnalysis();
                        break;
                    case Status.GotoBack://回城中
                        bRet=GotoBackAnalysis();
                        break;
                    case Status.Mission://任务中
                        bRet=MissionAnalysis();
                        break;
                }
                
            } while (bRet!=0);//如果不切换状态,则退出分析
        }
        void HandAnalysis()
        {
            int bRet = 0;
            do
            {
                switch (curStatus)//在相应的状态,就切换到那个状态的分析器
                {
                    case Status.Login://登录中
                        bRet = LoginAnalysis();
                        break;
                    case Status.Resurrection://复活中
                        bRet = ResurrectionAnalysis();
                        break;
                    case Status.GotoBack://回城中
                        //
                        System.Windows.Forms.MessageBox.Show("輔助模式不支援回城");
                        break;
                    case Status.Mission://任务中
                        bRet = HandMissionAnalysis();
                        break;
                }

            } while (bRet != 0);//如果不切换状态,则退出分析
        }
       
        void DoAction()
        {
            //AllActor[actCode].DoAction();
        }
    }
}
