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
        int CityPosPolicy()
        {
            //目标位置:城镇,藏身处
            //if(TargetCity==Town)
            //{

            //}
            //else//藏身处
            //{

            //}
            if (!InTown())
            {
                LastInTownTime = 0;
                //使用物品,
                if (UseBagTransDoor() == false)
                {
                    ReturnRolePolicy();
                    Thread.Sleep(2000);
                }
                bNeedCastBattleOnceSkill = true;

                Thread.Sleep(1000);
                int nObj = Program.client.GetNearbyGoCityTransferDoorObjPtr();
                if (nObj != 0)
                {
                    Thread.Sleep(1000);
                    Program.client.ActiveTarget(nObj);
                    Thread.Sleep(1000 * 2);
                }
                return 0;
            }
            return 1;
        }
        //城镇地图
        int TownMapPolicy()
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
            if (bNeedSave == false)
                return 1;
            if (Saveing)//正在存仓
            {
                ItemInfo NeedSaveItem = SearchSaveItem();
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
                        Thread.Sleep(1000);
                        Program.client.ActiveTarget(nObj);
                        Thread.Sleep(1000 * 2);
                        Program.client.ClickNPCMenu(GetCurTownNPCSellMenuID());
                        Thread.Sleep(1000);
                        Selling = true;
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

            Program.client.ReturnChoseRole();
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

    }
}