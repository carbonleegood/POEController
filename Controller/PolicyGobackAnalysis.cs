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
        //位置(藏身处)
        int GotoDungeonHomePolicy()
        {
            //确定位置
             //*****已经在藏身处
            if (InDungeonHome())
            {
                return 1;
            }
            //*****如果在城镇
            else if (InTown())
            {
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

                //走到传送点,传送到藏身处
                GPoint pos = GetCurTownWaypointPos();
                double dis = CalcDis(player.Pos, pos);
                if (dis <= 16.5)
                {
                    int nObjPtr = Program.client.GetNearbyWaypointObjPtr();
                    Program.client.ActiveTarget(nObjPtr);
                    Thread.Sleep(1000);
                    if(Program.client.TransHideHome()!=0)
                        MessageBox.Show("無法傳送到藏身處");
                    Thread.Sleep(2000); ;
                }
                else
                {
                    MoveToTownWaypoint(player.Pos.x, player.Pos.y);
                }
                return 0;
            }
            //*****如果在污染地穴
            else if(InPollutant())
            {
                //如果快速回城模式,直接用传送门或者小退回城
                if(Program.config.bFullHide==false)
                {
                    ActGotoTown();
                }
                //如果没有污染地穴门信息和地图传送点信息,直接用传送门或者小退回城
                else if (BattleWaypointPos == null || pollutantOutGatePos == null)
                {
                    ActGotoTown();
                }
                //否则走到污染地穴门,在附近则穿越
                else
                {
                    //如果在门附近,穿越
                    double dis = CalcDis(pollutantOutGatePos, player.Pos);
                    if (dis < 20.0f)//距离近就点门
                    {
                        //Program.client.ActiveTarget();
                        Thread.Sleep(2000);
                    }
                    else
                        ActSafeMove(pollutantOutGatePos);
                }
                return 0;
            }
            //*****如果在野外普通地图
            else if(InBattleArea())
            {
                //如果快速回城模式,直接用传送门或者小退回城
                if (Program.config.bFullHide == false)
                {
                    ActGotoTown();
                }
                //如果地图传送点信息,直接用传送门或者小退回城
                else if (BattleWaypointPos == null)
                {
                    ActGotoTown();
                }
                else
                {
                    double dis = CalcDis(BattleWaypointPos, player.Pos);
                    if (dis < 12.5f)//距离近就点门
                    {
                        int nObjPtr = Program.client.GetNearbyWaypointObjPtr();
                        Program.client.ActiveTarget(nObjPtr);
                        Thread.Sleep(1000);
                        if (Program.client.TransHideHome() != 0)
                            ActGotoTown();
                        Thread.Sleep(2000);
                    }
                    else
                        ActSafeMove(BattleWaypointPos);
                }
                return 0;
                //否则走到传送点,在附近则传送到藏身处
            }
        //    int nnID = CurMapID;
            //未知情况,直接回城
            ActGotoTown();
           // ReturnRolePolicy();
            return 0;
        }
        int GotoPublicHomePolicy()//回城模式
        {
            if (!InTown())
            {
                LastInTownTime = 0;
                ActGotoTown();
                return 0;
            }
            return 1;
        }
        int CityPosPolicy()
        {
            //目标位置:城镇,藏身处
            if (Program.config.bDungeonHome)//藏身处
            {
                return GotoDungeonHomePolicy();
            }
            else
            {
                return GotoPublicHomePolicy();
            }
        }
        //城镇地图
        int TownMapPolicy()
        {
            if (InDungeonHome())
                return 1;
            if (CurMapID != LoadTownMapID)
            {
                Program.client.Update();
                Thread.Sleep(1000);
                if (false == Program.gdata.AllTownMap.TryGetValue(CurMapID, out CurTownMap))
                {
                    MessageBox.Show("地图数据错误");
                    return 1;
                }
                UpdateTownMap
                    (
                    CurTownMap.SellNPCPos.x,
                    CurTownMap.SellNPCPos.y,
                    CurTownMap.WaypointPos.x,
                    CurTownMap.WaypointPos.y,
                    CurTownMap.TransferPos.x,
                    CurTownMap.TransferPos.y,
                    CurTownMap.StoragePos.x,
                    CurTownMap.StoragePos.y
                );
                LoadTownMapID = CurMapID;
                //    Thread.Sleep(1000);
            }
            return 1;
        }
        //鉴定
        int IndetityPolicy()
        {
            if (Program.config.bNeedIdentity && bNeedIdentity)
            {
                Thread.Sleep(2000);
                List<ItemInfo> IdentityList = GenIdentityList();
                IdentityItem(IdentityList);
                bNeedIdentity = false;
                return 0;
            }
            return 1;
        }
        //存仓
        int SavePolicy()
        {
            //不需要存仓,则返回
            if (bNeedSave == false)
                return 1;

            if (Saveing)//正在存仓
            {
                ItemFullInfo NeedSaveItem = SearchSaveItem();
                if (NeedSaveItem != null)
                {
                    int nRet = SaveItem(NeedSaveItem);
                    if (nRet == 1)//没有空间
                    {
                        Saveing = false;
                        bNeedSave = false;
                        Program.client.HitKey(7);
                        MessageBox.Show("沒有存儲空間!");
                        return 0;
                    }
                    else if (nRet == 2)//右键有物品
                    {
                        bNeedSave = false;
                        ReturnRolePolicy();
                    }
                }
                else
                {
                    Saveing = false;
                    bNeedSave = false;
                    int TrimRet = 0;
                    if (Program.config.bTrimStorage)
                    {
                        TrimRet = TrimStorage();
                    }
                    if (TrimRet == 0)
                        Program.client.HitKey(7);
                    else
                    {
                        ReturnRolePolicy();
                    }
                }
                return 0;
            }
            //走到仓库部分

            //如果是藏身处策略
            if (InDungeonHome())
            {
                //获取附近仓库位置
                int nPos=Program.client.GetNearbyStoragePos();
                //计算距离
                short x=(short)(nPos>>16);
                short y=(short)(nPos&65535);
                double dis = CalcDis(player.Pos, x, y);
                if (dis > 16.5)
                {
                    //藏身处移动到仓库
                    Program.client.Move(x, y);
                }
                int nHideHomeStorageObj = Program.client.GetNearbyStorageObjPtr();
                if (nHideHomeStorageObj != 0)
                {
                    Thread.Sleep(1000);
                    Program.client.ActiveTarget(nHideHomeStorageObj);//点开箱子
                    Thread.Sleep(1000 * 2);
                    Saveing = true;
                }
                return 0;
            }

            //如果离箱子很近
            GPoint storagePos = GetCurTownStoragePos();
            double storageDis = CalcDis(player.Pos, storagePos);
            if (storageDis > 16.5)
            {
                MoveToStorage(player.Pos.x, player.Pos.y);
                return 0;
            }

            //点击箱子
            int nObj = Program.client.GetNearbyStorageObjPtr();
            if (nObj != 0)
            {
                Thread.Sleep(1000);
                Program.client.ActiveTarget(nObj);//点开箱子
                Thread.Sleep(1000 * 2);
                Saveing = true;
                return 0;
            }
            return 0;
        }
        //售卖
        int SellPolicy()
        {
            if (bNeedSell==false)
                return 1;
            if (Selling)
            {
                ItemInfo NeedSellItem = GetSellItem();
                if (NeedSellItem == null)
                {
                    Program.client.ConfirmSell();
                    Thread.Sleep(1000);
                    Program.client.HitKey(7);
                    bNeedSell = false;
                    Selling = false;
                }
                else
                    PutToSellWindow(NeedSellItem);
                nSleepTime = 0;
                return 0;
            }
            if (InDungeonHome())
            {
                //获取附近NPC位置
                long nPos = Program.client.GetNearbySellNPCPos();
                int NPCNum = (int)(nPos >> 32);
                short x = (short)((nPos >> 16) & 65535);
                short y = (short)(nPos &65535); 
                //距离远则移动到NPC
                double dis = 0;
                if(dis>18.5)
                {
                    Program.client.Move(x, y);
                }

                //距离近则直接点击
                int nObj = Program.client.GetNearbySellNPCObjPtr(NPCNum);
                if (nObj != 0)
                {
                    Thread.Sleep(2000);
                    Program.client.ActiveTarget(nObj);
                    Thread.Sleep(1000 * 2);
                    Program.client.ClickNPCSellMenu();
                    Thread.Sleep(1000);
                    Selling = true;
                }
                else
                {
                    bNeedSell = false;
                }
                return 0;
            }
            //如果在城里,离NPC很近
            GPoint npcPos = GetCurTownNPCPos();
            double NPCdis = CalcDis(player.Pos, npcPos);
            if (NPCdis <= 20.0)
            {
                if (NPCdis < 15.0)
                {
                    //更新包裹信息
                    int nNPCNum = GetCurTownNPCNum();
                    int nObj = Program.client.GetNearbySellNPCObjPtr(nNPCNum);
                    if (nObj != 0)
                    {
                        Thread.Sleep(2000);
                        Program.client.ActiveTarget(nObj);
                        Thread.Sleep(1000 * 2);
                        Program.client.ClickNPCMenu(GetCurTownNPCSellMenuID());
                        Thread.Sleep(1000);
                        Selling = true;
                    }
                    else
                    {
                        bNeedSell = false;
                    }
                }
                else
                {
                    Program.client.Move(npcPos.x, npcPos.y);
                }
                return 0;
            }
            else
            {
                MoveToSellNPC(player.Pos.x, player.Pos.y);
            }
            return 0;
        }
        //小退策略
        int ReturnRolePolicy()
        {
            Selling = false;
            Saveing = false;
            bEnterPollutanting = false;
            UseTransDoor = false;
            LastInTownTime = 0;

            bNeedCastBattleOnceSkill = true;//需要施放一次性BUFF
            Program.client.ReturnChoseRole();
            Thread.Sleep(2000);
            return 0;
        }
        int SetNeedGoBack()
        {
            bNeedSave = true;
            bNeedSell = true;
            bNeedIdentity = true;

            Selling = false;
            Saveing = false;
            
            return 0;
        }
        void ResetBattleMapInfo()
        {
       //     bNeedResetBattleMapID = true;

            LoadBattleMapID = 0;


            pollutantMapID = 0;//污染地穴地图ID
            LoadPollutantMapID = 0;
            pollutantGatePos = null;//门位置
            pollutantComplete = false;//已经刷完
            bEnterPollutanting = false;//正在进入污染地穴
        }

    }
}