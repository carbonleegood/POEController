using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Thrift.GameCall;
using System.Threading;

namespace Controller
{
    public partial class Worker
    {
        //位置策略,找到正确的打怪位置
        int BattlePosPolicy(int curTickCount)
        {
            if (CurMapID != CurMissionMap.MapID && CurMapID != pollutantMapID)
            {
                //如果在城里,如果需要去传送门,则去传送门,否则去传送点
                if (InTown())
                {
                    bEnterPollutanting = false;
                    if (LastInTownTime == 0)
                    {
                        LastInTownTime = curTickCount;
                    }
                    if ((curTickCount - LastInTownTime) > cityReturnRoleTime)
                    {
                        ReturnRolePolicy();
                        LastInTownTime = 0;
                        return 0;
                    }
                    if (CurMapID != LoadTownMapID)
                    {
                        Program.client.Update();
                        Thread.Sleep(1000);
                        if (false == Program.gdata.AllTownMap.TryGetValue(CurMapID, out CurTownMap))
                        {
                            MessageBox.Show("地图数据错误");
                            return 1;
                        }
                        UpdateTownMap(
                            CurTownMap.SellNPCPos.x,
                            CurTownMap.SellNPCPos.y,
                            CurTownMap.WaypointPos.x,
                            CurTownMap.WaypointPos.y,
                            CurTownMap.TransferPos.x,
                            CurTownMap.TransferPos.y,
                            CurTownMap.StoragePos.x,
                            CurTownMap.StoragePos.y);
                        LoadTownMapID = CurMapID;
                        //    Thread.Sleep(1000);
                    }
                    if (UseTransDoor)//传送门
                    {
                        GPoint pos = GetCurTownTransDoorPos();
                        double dis = CalcDis(player.Pos, pos);
                        if (dis <= 16.0)
                        {
                            int nObj = Program.client.GetNearbyGoBattleTransfetDoorObjPtr();
                            if (nObj != 0)
                            {
                                Program.client.ActiveTarget(nObj);
                                Thread.Sleep(1000 * 3);
                                UseTransDoor = false;
                            }
                        }
                        else
                        {
                            MoveToTransferDoor(player.Pos.x, player.Pos.y);
                        }
                    }
                    else //传送点
                    {
                        GPoint pos = GetCurTownWaypointPos();
                        double dis = CalcDis(player.Pos, pos);
                        if (dis <= 16.5)
                        {
                            //  if (dis < 10.0)
                            {
                                int nID = Program.client.GetNearbyWaypointID();
                                if (nID != 0)
                                {
                                    int nMapID = GetTargetMissionMapID();
                                    Thread.Sleep(1000);
                                    if (Program.config.bDungeonModel)
                                    {
                                        Program.client.Transport(nMapID, nID, 1);
                                        LoadBattleMapID = 0;

                                        bCrossing = false;
                                        curGroup = -1;

                                        pollutantMapID = 0;//污染地穴地图ID
                                        LoadPollutantMapID = 0;
                                        pollutantGatePos = null;//门位置
                                        pollutantComplete = false;//已经刷完
                                        bEnterPollutanting = false;//正在进入污染地穴

                                    }
                                    else
                                        Program.client.Transport(nMapID, nID, 0);
                                    Thread.Sleep(1000 * 3);
                                }
                            }
                        }

                        else
                        {
                            MoveToTownWaypoint(player.Pos.x, player.Pos.y);
                        }
                    }
                }
                //如果不在城里,则回城
                else if (bEnterPollutanting)//
                {
                    if (pollutantMapID == 0)
                    {
                        pollutantMapID = CurMapID;
                        bEnterPollutanting = false;
                    }
                    return 0;
                }
                else
                {
                    curStatus = Status.GotoBack;
                }
                return 0;
            }
            else
            {
                LastInTownTime = 0;
                return 1;
            }
        }
       
        int BattleMapPolicy()
        {
            if (CurMapID == CurMissionMap.MapID)
            {
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
                }
            }
            if (CurMapID == pollutantMapID)
            {
                if (CurMapID != LoadPollutantMapID)
                {
                    Program.client.Update();
                    Thread.Sleep(1000);
                    UpdatePollutantMap();

                    LoadPollutantMapID = CurMapID;
                    FilterMonsterList.Clear();
                    ClickedBoxList.Clear();
                    LootFilter.Clear();
                    Program.client.ClearTrophyFilter();
                }
            }
            return 1;
        }
        int DangerReturnPolicy(int curTickCount)
        {
            //贫血小退
            if (Program.config.LogoutType == 0)
            {
                int data = player.HP;
                if (data < Program.config.LogOutData)
                {
                    ReturnRolePolicy();
                    //Program.client.ReturnChoseRole();
                    //bEnterPollutanting = false;
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
                        bNeedResetBattleMapID = true;
                        ChangeToNextMissionMap();

                        pollutantMapID = 0;//污染地穴地图ID
                        LoadPollutantMapID = 0;
                        pollutantGatePos = null;//门位置
                        pollutantComplete = false;//已经刷完
                        bEnterPollutanting = false;//正在进入污染地穴
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
                        bNeedResetBattleMapID = true;
                        ChangeToNextMissionMap();

                        pollutantMapID = 0;//污染地穴地图ID
                        LoadPollutantMapID = 0;
                        pollutantGatePos = null;//门位置
                        pollutantComplete = false;//已经刷完
                        bEnterPollutanting = false;//正在进入污染地穴
                    }
                    Thread.Sleep(1000);
                    return 0;
                }
            }
            return 1;
        }
        //开门
        int DoorPolicy()
        {
            SDoor Door = SearchDoor();
            if (Door != null)
            {
                //  Program.client.ActiveTarget(DoorObjPtr);
                if (ActOpenDoor(Door) == 0)
                    return 0;
            }
            return 1;
        }
        
        //攻击
        int AttackAndTrophyPolicy(int curTickCount)
        {
            int nMonsterCount = 0;
            List<SMonsterInfo> NearbyMonster = null;
            if (Program.config.bPriorityAttack)
            {
                SMonsterInfo targetMonster = SearchMonsterAndLootNew(out nMonsterCount, out NearbyMonster);
                if (targetMonster != null)
                {
                    ActKillMonsterNew(targetMonster, nMonsterCount, NearbyMonster, curTickCount);
                    return 0;
                }
                TrophyInfo trophy = SearchLoot();
                if (trophy != null)
                {
                    ActLootTrophy(trophy);
                    return 0;
                }
            }
            else
            {
                SMonsterInfo targetMonster = SearchMonsterAndLoot(out nMonsterCount);
                if (targetMonster != null)
                {
                    ActKillMonster(targetMonster, nMonsterCount, curTickCount);
                    return 0;
                }
                TrophyInfo trophy = SearchLoot();
                if (trophy != null)
                {
                    ActLootTrophy(trophy);
                    return 0;
                }
            }
            return 1;
        }

        ////拾取


        ////拾取策略
        int BoxPolicy(int curTickCount)
        {
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
                if (Box.Color > 0)
                    SetPassAbleArea(Box.Pos.x, Box.Pos.y);
                //获取A*距离,过大的走过去
                double dis = CalcDis(Box.Pos, player.Pos);
                if (dis > 16.5)
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
            return 1;
        }

        ////探索
        int ExplorePolicy(int curTickCount)
        {
            //卡位检测
            double BlockDis = CalcDis(player.Pos, FarBlockPoint);
            //如果距离上个监测点距离大于30.则改变
            if (BlockDis > 30.0)
            {
                FarBlockPoint.x = player.Pos.x;
                FarBlockPoint.y = player.Pos.y;
                FarBlockPointCount = 0;
            }
            else
            {
                FarBlockPointCount++;
            }

            if (FarBlockPointCount > 20)
            {
                FarBlockPointCount = 0;
                ChangeToNextMissionMap();
                curStatus = Status.GotoBack;
                SetNeedGoBack();
                bNeedResetBattleMapID = true;

                pollutantMapID = 0;//污染地穴地图ID
                LoadPollutantMapID = 0;
                pollutantGatePos = null;//门位置
                pollutantComplete = false;//已经刷完
                bEnterPollutanting = false;//正在进入污染地穴
                //小退回城
                ReturnRolePolicy();
                Thread.Sleep(3000);
                //MessageBox.Show("小退回城,卡位了");
                //FarBlockPointCount = 0;
                return 0;
            }

            if (pollutantGatePos != null)
            {
                if (LoadPollutantMapID == 0)
                    ActEnterPollutant();
                else//需要刷地穴
                    PollutantExplore();
                return 0;
            }

            //////////////////////////////////////////打怪探索
            ActDrinkSpeedFlask(curTickCount);
            if (bGroupExploreModel)
            {
                return GroupExplore();
            }
            return ActNormalExplore();
        }
    }
}