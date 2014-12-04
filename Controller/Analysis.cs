using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.GameCall;
using System.Threading;
namespace Controller
{
    public partial class Worker
    {
        int LoginAnalysis()
        {
            //如果已经登录,转到MISSION
            return 0;
        }
        int ResurrectionAnalysis()
        {
            //如果已经复活,转到MISSION
            return 0;
        }
        const int cityReturnRoleTime = 1000 * 60 * 3;//停留在城里小退的时间
        int GotoBackAnalysis()
        {
            //先确定位置
            if (CityPosPolicy() == 0)
                return 0;

            //判断是否需要重置战斗信息
            if(bNeedResetBattleMapInfo)
            {
                ResetBattleMapInfo();
                bNeedResetBattleMapInfo = false;
            }
            //if (bNeedResetBattleMapID)
            //{
            //    LoadBattleMapID = 0;
            //    bNeedResetBattleMapID = false;
            //}
            //城镇全局卡位检测
            int curTickCount = System.Environment.TickCount;//城里卡位检测,3分钟为限
            if (LastInTownTime == 0)
            {
                LastInTownTime = curTickCount;
            }
            if ((curTickCount - LastInTownTime) > cityReturnRoleTime)
            {
                UseTransDoor = false;
                ReturnRolePolicy();
                LastInTownTime = 0;
                return 0;
            }
            //如果当前城镇地图和回城地图不符,重载地图
            //再确定地图信息
            TownMapPolicy();

            BattleWaypointPos = null;
            pollutantOutGatePos = null;
            //鉴定
            if (IndetityPolicy() == 0)
                return 0;

            //存仓
            if (SavePolicy() == 0)
                return 0;

            //售卖
            if (SellPolicy() == 0)
            {
                return 0;
            }
            //如果售卖完毕,切换到任务状态
            curStatus = Status.Mission;
            return 0;
        }
        
        int MissionAnalysis()
        {
            //如果断线,
            //如果死亡
            int curTickCount = System.Environment.TickCount;
            //如果需要回城休整
            if (BattlePosPolicy(curTickCount) == 0)
                return 0;

            BattleMapPolicy();
            //贫血小退
            if (DangerReturnPolicy(curTickCount) == 0)
                return 0;

            //如果需要施放光环         
            if ((curTickCount - LastCheckHaloTime) > (5 * 1000))
            {
                if (Program.config.bAutoUpSkill)
                    Program.client.UpSkill();
                if (Program.config.haloSkill.Count > 0)
                {
                    foreach(var item in Program.config.haloSkill)
                    {
                        if (Program.client.IsBuffExists(item) == false)
                        {
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, (short)item, 8);
                           // Thread.Sleep(1000);
                            Program.client.WaitCastComplete();
                        }
                    }
                }
                LastCheckHaloTime=curTickCount;
            }

            if (bNeedCastBattleOnceSkill)
            {
                if (Program.config.battleOnceSkill.Count > 0)
                {
                    foreach (var item in Program.config.battleOnceSkill)
                    {
                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, (short)item, 8);
                        Thread.Sleep(1000);
                    }
                }
                bNeedCastBattleOnceSkill = false;
            }


            //区域传送点
            if(bGroupExploreModel)
            {
                if (curGroup != -1)
                {
                    foreach (var item in areaCrossList)
                    {
                        //如果没被处理过
                        if (AddedCrossPoint.Contains(item.Pos))
                            continue;
                        CrossPoint crossPoint = new CrossPoint();
                        crossPoint.firstGroup = GetCurGroup(player.Pos.x, player.Pos.y);
                        crossPoint.firstPos.x = item.Pos.x;
                        crossPoint.firstPos.y = item.Pos.y;
                        allCrossPoint.Add(crossPoint);
                        AddedCrossPoint.Add(item.Pos);
                        SetPassAbleArea(item.Pos.x, item.Pos.y);
                    }
                }
            }
            if (Program.config.bExplorePollutant)//如果刷污染地穴
            {
                if (NearbyPollutantGatePos != null)
                {
                    if (pollutantGatePos == null)
                    {
                        pollutantGatePos = new GPoint();
                        pollutantGatePos.x = NearbyPollutantGatePos.x;
                        pollutantGatePos.y = NearbyPollutantGatePos.y;
                        SetPassAbleArea(pollutantGatePos.x, pollutantGatePos.y);
                    }

                }
            }
            //如果需要搜索传送阵
            if(bNeedSearchBattleWaypoint)
            {
                int Pos=Program.client.GetNearbyWaypointPos();
                ushort x = (ushort)(Pos >> 16);
                ushort y = (ushort)(Pos & 65535);
                BattleWaypointPos = new GPoint();
                BattleWaypointPos.x=x;
                BattleWaypointPos.y=y;
                bNeedSearchBattleWaypoint = false;
            }
            if(bNeedSearchOutPollutantGate)
            {
                int Pos = Program.client.GetNearbyOutPollutantGatePos();
                ushort x = (ushort)(Pos >> 16);
                ushort y = (ushort)(Pos & 65535);
                pollutantOutGatePos = new GPoint();
                pollutantOutGatePos.x = x;
                pollutantOutGatePos.y = y;
                bNeedSearchOutPollutantGate = false;
            }
           
            ////////////////////////////////药剂
            
            ActDrinkFlask(curTickCount);

            ////////////////////////////////////
            // //如果有门在身边,则点
            if (DoorPolicy() == 0)
                return 0;
        
            /////////////////////////////////////
            if (AttackAndTrophyPolicy(curTickCount) == 0)
                return 0;

            if (BoxPolicy(curTickCount) == 0)
                return 0;

            //探索
            return ExplorePolicy(curTickCount);
        }
        int HandMissionAnalysis()
        {
            //如果不在任务地点,去任务地点
            
            //如果在城里,如果需要去传送门,则去传送门,否则去传送点
            if (InTown())
            {
                MessageBox.Show("輔助模式,請在目標探索地圖開啟輔助程式");
                return 0;
            }

               

            if (CurMapID != LoadBattleMapID)
            {
                Program.client.Update();
                Thread.Sleep(1000);
                UpdateMap(bGroupExploreModel);


                LoadBattleMapID = CurMapID;
                FilterMonsterList.Clear();
                PassedDoor.Clear();
                areaCrossList.Clear();
                AddedCrossPoint.Clear();
                ClickedBoxList.Clear();
                LootFilter.Clear();
                curGroup = -1;

                Program.client.ClearTrophyFilter();

                //    bGroupExploreModel = false;//判断
                bNeedCross = false;//判断探索还是穿越
                bCrossing = false;//判断穿越中还是移动到穿越点
                bNeedCastBattleOnceSkill = true;
                //    Thread.Sleep(1000);
            }
            int curTickCount = System.Environment.TickCount;
            //贫血小退
            if (Program.config.LogoutType == 0)
            {
                int data = player.HP;
                if (data < Program.config.LogOutData)
                {
                    ReturnRolePolicy();
                    if ((curTickCount - lastReturnTime) < 1000 * 60)
                    {
                        LogoutCount++;
                    }
                    else
                    {
                        LogoutCount = 0;
                    }
                    lastReturnTime = curTickCount;
                    if (LogoutCount > 5)
                    {
                        LogoutCount = 0;
                        curStatus = Status.GotoBack;
                        SetNeedGoBack();
                        bNeedResetBattleMapInfo = true;
                        ChangeToNextMissionMap();
                        //bNeedResetBattleMapID = true;                     
                        //pollutantMapID = 0;//污染地穴地图ID
                        //LoadPollutantMapID = 0;
                        //pollutantGatePos = null;//门位置
                        //pollutantComplete = false;//已经刷完
                        //bEnterPollutanting = false;//正在进入污染地穴
                    }
                    Thread.Sleep(1000);
                    return 0;
                }
            }
            else if (Program.config.LogoutType == 1)
            {
                int data = player.Shield;
                if (data < Program.config.LogOutData)
                {
                    ReturnRolePolicy();
                    if ((curTickCount - lastReturnTime) < 1000 * 60)
                    {
                        LogoutCount++;
                    }
                    else
                    {
                        LogoutCount = 0;
                    }
                    lastReturnTime = curTickCount;
                    if (LogoutCount > 5)
                    {
                        LogoutCount = 0;
                        curStatus = Status.GotoBack;
                        SetNeedGoBack();
                        bNeedResetBattleMapInfo = true;
                        ChangeToNextMissionMap();
                        //bNeedResetBattleMapID = true;
                        //ChangeToNextMissionMap();

                        //pollutantMapID = 0;//污染地穴地图ID
                        //LoadPollutantMapID = 0;
                        //pollutantGatePos = null;//门位置
                        //pollutantComplete = false;//已经刷完
                        //bEnterPollutanting = false;//正在进入污染地穴
                    }
                    Thread.Sleep(1000);
                    return 0;
                }
            }
            //如果需要施放光环
            if ((curTickCount - LastCheckHaloTime) > (5 * 1000))
            {
                if (Program.config.bAutoUpSkill)
                    Program.client.UpSkill();
                if (Program.config.haloSkill.Count > 0)
                {
                    foreach (var item in Program.config.haloSkill)
                    {
                        if (Program.client.IsBuffExists(item) == false)
                        {
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, (short)item, 8);
                            Thread.Sleep(1000);
                        }
                    }
                }
                LastCheckHaloTime = curTickCount;
            }
            if (bNeedCastBattleOnceSkill)
            {
                if (Program.config.battleOnceSkill.Count > 0)
                {
                    foreach (var item in Program.config.battleOnceSkill)
                    {
                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, (short)item, 8);
                        Thread.Sleep(1000);
                    }
                }
                bNeedCastBattleOnceSkill = false;
            }
            //区域传送点
            if (bGroupExploreModel)
            {
                //
                if (curGroup != -1)
                {
                    foreach (var item in areaCrossList)
                    {
                        //如果没被处理过
                        if (AddedCrossPoint.Contains(item.Pos))
                            continue;
                        CrossPoint crossPoint = new CrossPoint();
                        crossPoint.firstGroup = GetCurGroup(player.Pos.x, player.Pos.y);
                        crossPoint.firstPos.x = item.Pos.x;
                        crossPoint.firstPos.y = item.Pos.y;
                        allCrossPoint.Add(crossPoint);
                        AddedCrossPoint.Add(item.Pos);
                        SetPassAbleArea(item.Pos.x, item.Pos.y);
                    }
                }
            }
            
           
            ////////////////////////////////药剂

            ActDrinkFlask(curTickCount);

            ////////////////////////////////////
            // //如果有门在身边,则点
            SDoor Door = SearchDoor();
            if (Door != null)
            {
                //  Program.client.ActiveTarget(DoorObjPtr);
                ActOpenDoor(Door);
                return 0;
            }

            /////////////////////////////////////
            int nMonsterCount = 0;
            List<SMonsterInfo> NearbyMonster = null;
            SMonsterInfo targetMonster = SearchMonsterAndLootNew(out nMonsterCount, out NearbyMonster);
            if (targetMonster != null)
            {
                ActKillMonsterNew(targetMonster, nMonsterCount,NearbyMonster, curTickCount);
                return 0;
            }
            TrophyInfo trophy = SearchLoot();
            if (trophy != null)
            {
                ActHandLootTrophy(trophy);
                return 0;
            }



            SBox Box = null;
            if (TargetBox.ID != 0)
            {
                Box = TargetBox;
            }
            else
            {
                Box = SearchBox();
                if (Box != null)
                {
                    TargetBox.ID = Box.ID;
                    TargetBox.ObjPtr = Box.ObjPtr;
                    TargetBox.Pos.x = Box.Pos.x;
                    TargetBox.Pos.y = Box.Pos.y;
                }
            }
            //如果有宝箱
            if (Box != null)
            {
                //检测阻塞
                if (player.Pos == lastPlayerPos && Box.Pos == lastTargetPoint)
                {
                    blockCount++;
                    nSleepTime = 50;
                }
                else
                    blockCount = 0;
                lastPlayerPos.x = player.Pos.x;
                lastPlayerPos.y = player.Pos.y;
                lastTargetPoint.x = Box.Pos.x;
                lastTargetPoint.y = Box.Pos.y;
                if (blockCount > 3)
                {
                    ClickedBoxList.Add(Box.ID);
                    TargetBox.ID = 0;
                    nSleepTime = 50;
                    return 0;
                }
                //获取A*距离,过大的走过去
                double dis = CalcDis(Box.Pos, player.Pos);
                if (dis > 15)
                    ActSafeMove(Box.Pos);
                else
                {
                    if (Box.Color > 0)
                    {
                        //釋放圖騰
                        CastTTSkillNow(curTickCount);

                        //釋放陷阱
                        CastTrapSkillNow(curTickCount, player.Pos);

                        //施放护盾技能
                        CastShieldSkillNow(curTickCount);
                    }
                    // 否则点击
                    Program.client.ActiveTarget(Box.ObjPtr);
                    //   ClickedBoxList.Add(Box.ID);
                    TargetBox.ID = 0;
                }

                nSleepTime = 50;
                return 0;
            }

            ////卡位检测

            //////////////////////////////////////////打怪探索
            ActDrinkSpeedFlask(curTickCount);

            //if (bGroupExploreModel)
            //{
            //    return GroupExplore();
            //}
            return ActHandNormalExplore();
        }
    }
}
