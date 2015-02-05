using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Thrift.GameCall;
using System.Threading;
namespace Controller
{
     public partial class Worker
     {
         const short byTrophy_SkillStone = 1;//技能石
         const short byTrophy_Currency = 2;//卷轴
         const short byTrophy_Flask = 3;//血瓶
         const short byTrophy_Armour = 4;//装备
         const short byTrophy_Ring = 5;//戒指
         const short byTrophy_Amulet = 6;//项链
         const short byTrophy_Belt = 7;//腰带
         const short byTrophy_Weapon = 8;//武器
   //      const short byTrophy_Quiver = 9;//箭包
         const short byTrophy_QuestItem = 10;//任务物品
         const short byTrophy_Maps = 11;//地图
         const short byTrophy_Money = 12;//通货

         List<ItemInfo> GenIdentityList()
         {
             List<ItemInfo> IdentityList = new List<ItemInfo>();
             List<ItemInfo> bag = Program.client.GetContainerItemList(0);
             //if (bag.Count < 1)
             //    System.Windows.Forms.MessageBox.Show("bag error!");
             foreach (var item in bag)
             {
                 if (item.NeedIdentify)
                 {
                     switch(item.Type)
                     {
                         case byTrophy_Ring:
                             if(Program.config.bNoIdentifyRing==false)
                                 IdentityList.Add(item);
                             break;
                         case byTrophy_Amulet:
                             if (Program.config.bNoIdentifyAmulet==false)
                                 IdentityList.Add(item);
                             break;
                         case byTrophy_Belt:
                             if (Program.config.bNoIdentifyBelt==false)
                                 IdentityList.Add(item);
                             break;
                         default:
                             IdentityList.Add(item);
                             break;
                     }
                     
                 }
             }
             return IdentityList;
         }
         int SearchIdentityCurrencyServiceID()
         {
             int nServiceID = -1;
              List<ItemInfo> bag = Program.client.GetContainerItemList(0);
              //if (bag.Count < 1)
              //    System.Windows.Forms.MessageBox.Show("bag error!");
              foreach (var item in bag)
              {
                  if(item.Type==byTrophy_Currency)
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
             foreach(var item in IdentityList)
             {
                 int nServiceID=SearchIdentityCurrencyServiceID();
                 if(nServiceID<0)
                     break;
                 Program.client.IdentityItem(nServiceID, item.ServiceID);
                 Thread.Sleep(500);
             }
         }
         ItemInfo GetSellItem()
         {
             ItemInfo NeedSellItem = null;
             List<ItemInfo> bag = Program.client.GetContainerItemList(0);
             List<ItemInfo> SelledList = Program.client.GetContainerItemList(1);
             HashSet<GPoint> SelledPos = new HashSet<GPoint>(); 
             foreach(var item in SelledList)
             {
                 GPoint temp = new GPoint();
                 temp.x=(ushort)item.Top;
                 temp.y=(ushort)item.Left;
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
         //List<ItemInfo> GenSellList()
         //{
         //    //遍历背包,找到装备,售卖
         //    //
         //    List<ItemInfo> SellList = new List<ItemInfo>();
         //    List<ItemInfo> bag = Program.client.GetContainerItemList(0);
         //    //if (bag.Count < 1)
         //    //    System.Windows.Forms.MessageBox.Show("bag error!");
         //    foreach (var item in bag)
         //    {
         //        int FilterColor = 0;
         //        //如果不是装备.是技能石1,卷轴2,血瓶3,地图,
         //        if (item.Type != 4 && item.Type != 8 && item.Type != 9)
         //        {
         //            if (Program.config.SellTypeList.TryGetValue(item.Type, out FilterColor) == false)
         //                continue;
         //            if (FilterColor < item.Color)
         //                continue;
         //            SellList.Add(item);
         //        }
         //        else//有槽的
         //        {
         //            if (Program.config.SellSocketFilter > 0)
         //            {
         //                if (Program.config.SellSocketFilter >= item.Socket)
         //                {
         //                    SellList.Add(item);
         //                    continue;
         //                }
         //            }
         //            if (Program.config.SellSocketConnectFilter > 0)
         //            {
         //                if (Program.config.SellSocketConnectFilter >= item.SocketConnect)
         //                {
         //                    SellList.Add(item);
         //                    continue;
         //                }
         //            }
         //            if (item.ThreeColorSocket && Program.config.SellThreeColor)
         //            {
         //                SellList.Add(item);
         //                continue;
         //            }

         //            if (Program.config.SellTypeList.TryGetValue(item.Type, out FilterColor) == false)
         //                continue;
         //            if (FilterColor < item.Color)
         //                continue;
         //            SellList.Add(item);
         //        }
         //    }
         //    return SellList;
         //}
         int PutToSellWindow(ItemInfo item)
         {
             int nRet = 0;

             nRet = Program.client.PutToSell(item.Top, item.Left, item.ServiceID, item.WinID);
             //if (nRet != 0)
             //    break;
             Thread.Sleep(1000);
             return nRet;
         }
         //int PutToSellWindow(List<ItemInfo> SellList)
         //{
         //    int nRet = 0;
         //    foreach (var item in SellList)
         //    {
         //        nRet = Program.client.PutToSell(item.Top, item.Left, item.ServiceID, item.WinID);
         //        //if (nRet != 0)
         //        //    break;
         //        Thread.Sleep(1000);
         //    }
         //    return nRet;
         //}
        
         GPoint GetCurTownWaypointPos()
         {
             return CurTownMap.WaypointPos;
         }
         GPoint  GetCurTownTransDoorPos()
         {
             return CurTownMap.TransferPos;
         }
         int  GetCurTownNPCNum()
         {
             return CurTownMap.NPCNum;
         }
         GPoint GetCurTownNPCPos()
         {
             return CurTownMap.SellNPCPos;
         }
         int GetCurTownNPCSellMenuID()
         {
             return CurTownMap.NPCSellMenu;
         }
         GPoint GetCurTownStoragePos()
         {
             return CurTownMap.StoragePos;
         }
         int GetTargetMissionMapID()
         {
             string Key = Program.config.MissionMapList[Program.runtime.curMissionMapIndex];
             BattleMapInfo map=null;
             if (Program.gdata.AllBattleMap.TryGetValue(Key, out map))
                 return map.MapID;
             return 0;
         }
         bool UseBagTransDoor()
         {
             if (Program.config.nGobackSkill != -1)
             {
                 int curTickCount = System.Environment.TickCount;
                 if ((curTickCount - LastMidSkillCastTime)>1000*60)
                 {
                     Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, (short)Program.config.nGobackSkill, 8);
                     LastMidSkillCastTime = curTickCount;
                     Thread.Sleep(3000);
                     return true;
                 }
             }
             int nRet=Program.client.UseTransDoor();
             if (nRet == 0)
                 return true;
             return false;
         }
         bool InPollutant()
         {
             if (CurMapID == pollutantMapID)
                 return true;
             else
                 return false;
         }
         bool InBattleArea()
         {
             if (CurMapID == CurMissionMap.MapID)
                 return true;
             else
                 return false;
         }
         bool InTown()
         {
             //获取当前地图ID,判断是否是
             return Program.gdata.AllTownMapID.Contains(CurMapID);
         }
         bool InDungeonHome()//是否在藏身处
         {
             return Program.gdata.AllDungeonMapID.Contains(CurMapID);
         }
         int SaveItem(ItemFullInfo item)//0成功,1没有空间,2放下失败,右键有物品
         {
         //    int FailCount = 0;

            // StorageSpace spc = null;
            // spc = SearchSpaceInStorage(item.Width, item.Height);
             //搜索空间,找到则直接放
             //string strName =null;
             //if (item.Type == byTrophy_Currency || item.Type == byTrophy_Money)
             //{
             //    sbyte[] bname = item.Name.ToArray();
             //    byte[] bytes = new byte[bname.Length];
             //    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
             //    strName = Encoding.Unicode.GetString(bytes);
             //    //spc = SearchSpaceInStorageWithCount(item.Width, item.Height, strName, item.Count);

             //}
             //else
             //    spc = SearchSpaceInStorageWithCount(item.Width, item.Height, null, 0);
                 //   StorageSpace spc=SearchSpaceInStorage(item.Width,item.Height);

             //失败怎么办
           //  if (spc == null)
           //  {
           //      return 1;
           //  }

             int nStorageSpace = SearchSpaceInStorageWithCount(item.Width, item.Height, item.Count);

             if (nStorageSpace < 0)
                 return 1;

             Program.client.SaveToStorage(nStorageSpace, item.ServiceID);
             //Program.client.PickupItem(item.BagObjPtr, item.ServiceID);
             //Thread.Sleep(500);
             //if (spc.StackObjPtr != 0)
             //{

             //    Program.client.DropdownItemStack(spc.ContainerObjPtr, spc.StackObjPtr);
             //    Thread.Sleep(500);

             //    if (spc.Left > 0)
             //    {
             //        Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
             //        Thread.Sleep(500);
             //    }

             //        //if (Program.client.IsItemOnMouse())
             //        //{
             //        //    //    System.Windows.Forms.MessageBox.Show("drop error!");
             //        //    Program.client.ReturnChoseRole();
             //        //    FailCount = 1;
             //        //    break;
             //        //}
             //}
             //else
             //{
               //  Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
               //  Thread.Sleep(500);

                 //if (Program.client.IsItemOnMouse())
                 //{
                 //    //    System.Windows.Forms.MessageBox.Show("drop error!");
                 //    Program.client.ReturnChoseRole();
                 //    FailCount = 1;
                 //    break;
                 //}

        //     }
             // Thread.Sleep(500);
            // if (Program.client.IsItemOnMouse())
            //     return 2;
             //成功返回0
             return 0;
         }
         Dictionary<string, List<short>> GenTrophyProperty(string strProperty)
         {
             Dictionary<string, List<short>> DealProperty = new Dictionary<string, List<short>>();
             string[] AllProperty = strProperty.Split('|');
             foreach (var OneProperty in AllProperty)
             {
                 if (OneProperty.Length < 1)
                     continue;
                 //提取数字
                 bool bNumbering = false;
                 StringBuilder strNum = null;// new StringBuilder();
                 StringBuilder strKey = new StringBuilder();
                 List<short> data = new List<short>();
                 foreach (var OneChar in OneProperty)
                 {
                     if (Program.NumberChar.Contains(OneChar))//如果是数字
                     {
                         if (!bNumbering)
                         {
                             bNumbering = true;
                             strKey.Append("n");
                             strNum = new StringBuilder();
                         }
                         strNum.Append(OneChar);
                     }
                     else
                     {
                         if (bNumbering)
                         {
                             short dbTemp = 0;
                             if (short.TryParse(strNum.ToString(), out dbTemp))
                             {
                                 data.Add(dbTemp);
                             }
                            // strNum.Clear();
                         }
                         bNumbering = false;
                         strKey.Append(OneChar);
                     }
                 }
                 if (bNumbering)
                 {
                     short dbTemp = 0;
                     if (short.TryParse(strNum.ToString(), out dbTemp))
                     {
                         data.Add(dbTemp);
                     }
                   //  strNum.Clear();
                 }
                 List<short> ExistData = null;
                 bool bExist = DealProperty.TryGetValue(strKey.ToString(), out ExistData);
                 if (bExist)
                 {
                     for (int i = 0; i < data.Count; ++i)
                     {
                         if (data[i] > ExistData[i])
                         {
                             for (int j = 0; j < data.Count; ++j)
                             {
                                 ExistData[j] = data[j];
                             }
                             break;
                         }
                         else if (data[i] < ExistData[i])
                         {
                             break;
                         }
                     }
                 }
                 else
                     DealProperty.Add(strKey.ToString(), data);
             }
             return DealProperty;
         }
         List<SaveFilter> SearchFilter(Dictionary<string, List<short>> TrophyProperty)
         {
             List<SaveFilter> GroupFilter = null;
             foreach (var item in TrophyProperty)
             {
                 bool bRet = Program.gdata.IndexFilter.TryGetValue(item.Key, out GroupFilter);
                 if (bRet)
                     break;
                 GroupFilter = null;
             }
             return GroupFilter;
         }
         bool CompareProperty(Dictionary<string, List<short>> TrophyProperty,SaveFilter Filter)
         {
             //判断过滤器中的每一条,是否都在目标中
             foreach (var FilterItem in Filter.rules)
             {
                 List<short> tempData = null;
                 bool bRet = TrophyProperty.TryGetValue(FilterItem.strInfo, out tempData);
                 if (bRet == false)
                     return false;
                 if (tempData.Count > 0)
                 {
                     if (FilterItem.n1 >= 0)
                     {
                         if (FilterItem.n1 > tempData[0])
                             return false;
                     }
                 }
                 if (tempData.Count > 1)
                 {
                     if (FilterItem.n2 >= 0)
                     {
                         if (FilterItem.n2 > tempData[1])
                             return false;
                     }
                 }
             }
             return true;
         }
         bool IsFilterTrophy(ItemFullInfo trophy)
         {
             try
             {
                 sbyte[] bname = trophy.Name.ToArray();
                 byte[] bytes = new byte[bname.Length];
                 Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                 string strName = Encoding.Unicode.GetString(bytes);
                 if (Program.config.NameSaveList.Contains(strName))
                     return true;

                 ///////////////////////////////////////////////////////////////////////
                 bname = trophy.DescribInfo.ToArray();
                 bytes = new byte[bname.Length];
                 Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                 string strProperty = Encoding.Unicode.GetString(bytes);
                 Dictionary<string, List<short>> TrophyProperty = GenTrophyProperty(strProperty);
                 List<SaveFilter> GroupFilter = SearchFilter(TrophyProperty);
                 if (GroupFilter == null)
                     return false;
                 foreach (var item in GroupFilter)
                 {
                     if (trophy.Type != item.type)
                         continue;
                     bool bRet = CompareProperty(TrophyProperty, item);
                     if (bRet == true)
                         return true;
                 }
             }
             catch (Exception e)
             {
                 sbyte[] bname = trophy.Name.ToArray();
                 byte[] bytes = new byte[bname.Length];
                 Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                 string strName = Encoding.Unicode.GetString(bytes);

                 System.Windows.Forms.MessageBox.Show("程式崩潰,請截圖聯繫管理員:"+strName);
             }
             return false;
         }
         ItemFullInfo SearchSaveItem()
         {
             ItemFullInfo NeedSaveItem = null;
             ItemFullInfo MaxGobackCurrency = null;
             ItemFullInfo MaxIdentityCurrency = null;
             List<ItemFullInfo> bag = Program.client.GetBagItemFullInfo();

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
                     if (strName == "傳送卷軸")//如果是傳送卷軸,保存個數最大的一組
                     {
                         if (MaxGobackCurrency == null)
                         {
                             MaxGobackCurrency = item;
                             continue;
                         }
                         else
                         {
                             if (item.Count > MaxGobackCurrency.Count)
                             {
                                 NeedSaveItem = MaxGobackCurrency;
                                 break;
                             }
                             else
                             {
                                 NeedSaveItem = item;
                                 break;
                             }
                         }
                     }
                     if (strName == "知識卷軸")//如果是傳送卷軸,保存個數最大的一組
                     {
                         if (MaxIdentityCurrency == null)
                         {
                             MaxIdentityCurrency = item;
                             continue;
                         }
                         else
                         {
                             if (item.Count > MaxIdentityCurrency.Count)
                             {
                                 NeedSaveItem = MaxIdentityCurrency;
                                 break;
                             }
                             else
                             {
                                 NeedSaveItem = item;
                                 break;
                             }
                         }
                     }
                 }
                 //如果符合过滤要求的,直接返回
                 if (IsFilterTrophy(item))
                 {
                     NeedSaveItem = item;
                     break;
                 }
                 //如果符合名字的,也直接返回
                 if (item.Type != byTrophy_Armour && item.Type != byTrophy_Weapon)//没孔没槽的
                 {
                     if (item.Type == byTrophy_Money)
                     {
                         sbyte[] bname = item.Name.ToArray();
                         byte[] bytes = new byte[bname.Length];
                         Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                         string strName = Encoding.Unicode.GetString(bytes);
                         if (strName == "改造石碎片" || strName == "蛻變石碎片"
                             || strName == "點金石碎片" || strName == "卷軸碎片")
                             continue;

                     }
                     if (Program.config.SaveTypeList.TryGetValue(item.Type, out FilterColor) == false)
                         continue;
                     if (FilterColor > item.Color)
                         continue;
                     NeedSaveItem=item;
                     break;
                 }
                 else
                 {
                     //这里的逻辑有点乱啊
                     if (Program.config.SaveSocketFilter > 0)
                     {
                         if (Program.config.SaveSocketFilter <= item.Socket)
                         {
                             NeedSaveItem = item;
                             break;
                         }
                     }
                     if (Program.config.SaveSocketConnectFilter > 0)
                     {
                         if (Program.config.SaveSocketConnectFilter <= item.SocketConnect)
                         {
                             NeedSaveItem = item;
                             break;
                         }
                     }
                     if (item.ThreeColorSocket && Program.config.SaveThreeColor)
                     {
                         NeedSaveItem = item;
                         break;
                     }
                     if (Program.config.SaveTypeList.TryGetValue(item.Type, out FilterColor) == false)
                         continue;
                     if (FilterColor > item.Color)
                         continue;
                     NeedSaveItem = item;
                     break;
                 }
             }
             return NeedSaveItem;
         }
         List<ItemInfo> GenSaveList()
         {
             //遍历背包,找到装备,售卖
             //
             //   List<ItemInfo> GobackCurrency = new List<ItemInfo>();
             ItemInfo MaxCurrency = null;
             ItemInfo MaxIdentityCurrency = null;
             List<ItemInfo> SaveList = new List<ItemInfo>();
             List<ItemInfo> bag = Program.client.GetContainerItemList(0);
             //if (bag.Count < 1)
             //    System.Windows.Forms.MessageBox.Show("bag error!");
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
                     if (strName == "傳送卷軸")//如果是傳送卷軸,保存個數最大的一組
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
                     if (strName == "知識卷軸")//如果是傳送卷軸,保存個數最大的一組
                     {
                         if (MaxIdentityCurrency == null)
                             MaxIdentityCurrency = item;
                         else
                         {
                             if (item.Count > MaxIdentityCurrency.Count)
                             {
                                 SaveList.Add(MaxIdentityCurrency);
                                 MaxIdentityCurrency = item;
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
                     if (Program.config.SaveSocketFilter > 0)
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
         const short StorageWidth = 12;
         const short StorageHeight = 12;
         //
         //int SaveToStorage(List<ItemInfo> SaveList)
         //{
         //    int FailCount = 0;
         //     foreach(var item in SaveList)
         //     {
         //         StorageSpace spc =null;
         //         //搜索空间,找到则直接放
         //         if (item.Type == byTrophy_Currency || item.Type == byTrophy_Money)
         //         {
         //             sbyte[] bname = item.Name.ToArray();
         //             byte[] bytes = new byte[bname.Length];
         //             Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
         //             string strName = Encoding.Unicode.GetString(bytes);
         //             spc = SearchSpaceInStorageWithCount(item.Width, item.Height,strName,item.Count);
         //         }
         //         else
         //             spc = SearchSpaceInStorageWithCount(item.Width, item.Height, null, 0);
         //         //   StorageSpace spc=SearchSpaceInStorage(item.Width,item.Height);
         //         if (spc == null)
         //         {
         //             FailCount++;
         //             continue;
         //         }
                  
         //         Program.client.PickupItem(item.BagObjPtr, item.ServiceID);
         //         Thread.Sleep(500);
         //         if (spc.StackObjPtr != 0)
         //         {

         //             Program.client.DropdownItemStack(spc.ContainerObjPtr, spc.StackObjPtr);
         //             Thread.Sleep(500);
         //           //  do
         //           //  {
         //                 if (spc.Left > 0)
         //                 {
         //                     Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
         //                     Thread.Sleep(500);
         //                 }
         //             //    else
         //             //        break;
         //             //}
         //             //while (Program.client.IsItemOnMouse());
         //             if (Program.client.IsItemOnMouse())
         //             {
         //                 //    System.Windows.Forms.MessageBox.Show("drop error!");
         //                 Program.client.ReturnChoseRole();
         //                 FailCount = 1;
         //                 break;
         //             }
         //         }
         //         else
         //         {
         //         //    do
         //          //   {
         //                 Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
         //                 Thread.Sleep(500);
         //          //   }
         //          //   while (Program.client.IsItemOnMouse());
         //             if (Program.client.IsItemOnMouse())
         //             {
         //                 //    System.Windows.Forms.MessageBox.Show("drop error!");
         //                 Program.client.ReturnChoseRole();
         //                 FailCount = 1;
         //                 break;
         //             }
                     
         //         }
         //        // Thread.Sleep(500);
         //     }
         //     return FailCount;
         //}
         void InitStorage()
         {
             //激活所有仓库页
             int PageCount = Program.client.ReadStoragePageNum();
             for (int bp = 0; bp < PageCount; ++bp)
             {
                 Program.client.ActiveStoragePageInfo(bp);
                 Thread.Sleep(500);
             }

             //绑定
         }
         int SearchSpaceInStorageWithCount(short width, short height, int nCount)
         {
             int retStoragePage = -1;
             //FZ_ReadStorageUIPTR(i);//读取仓库页码]
             int PageCount = Program.client.ReadStoragePageNum();
             for (int bp = 0; bp < PageCount; ++bp)
             {
                 ////////////////////////////////////////计算空间
                 retStoragePage = -1;
                 List<ItemInfo> Storage = Program.client.GetContainerItemList(bp + 10);

                 //遍历仓库页物品占用的空间
                 byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
                 foreach (var item in Storage)
                 {
                     //if (strName != null)
                     //{
                     //    if (retStoragePage == -1)
                     //    {
                     //        sbyte[] bname = item.Name.ToArray();
                     //        byte[] bytes = new byte[bname.Length];
                     //        Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
                     //        string strItemName = Encoding.Unicode.GetString(bytes);
                     //        if (strItemName == strName)
                     //        {
                     //            if (item.Count < item.MaxCount)
                     //            {
                     //                //ret = new StorageSpace();
                     //                //ret.StackObjPtr = item.ServiceID;
                     //                //ret.ContainerObjPtr = Program.client.GetStoragePagePtr(bp + 10);
                     //                retStoragePage=Program.client.ReadStorageUIPTR(bp);                  
                     //                if ((item.Count + nCount) <= item.MaxCount)
                     //                {
                     //                    return retStoragePage;
                     //                }
                     //            }
                     //        }
                     //    }
                     //}
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
                             retStoragePage = Program.client.ReadStorageUIPTR(bp);   
                             return retStoragePage;
                         }
                     }
                 }
             }
             return retStoragePage;
         }
         //StorageSpace SearchSpaceInStorageWithCount(short width, short height,string strName,int nCount)
         //{
         //    StorageSpace ret = null;
         //    //FZ_ReadStorageUIPTR(i);//读取仓库页码]
         //    int PageCount = Program.client.ReadStoragePageNum();
         //    for (int bp = 0; bp < PageCount; ++bp)
         //    {
         //        //激活仓库页
         //        Program.client.ActiveStoragePageInfo(bp);
         //        Thread.Sleep(1000);
         //        ////////////////////////////////////////计算空间
         //        ret = null;
         //        List<ItemInfo> Storage = Program.client.GetContainerItemList(bp+10);
         //        //if(Storage.Count<1)
         //        //    System.Windows.Forms.MessageBox.Show("bag error!");
         //        //遍历仓库页物品占用的空间
         //        byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
         //        foreach (var item in Storage)
         //        {
         //            if (strName != null)
         //            {
         //                if (ret == null)
         //                {
         //                    sbyte[] bname = item.Name.ToArray();
         //                    byte[] bytes = new byte[bname.Length];
         //                    Buffer.BlockCopy(bname, 0, bytes, 0, bname.Length);
         //                    string strItemName = Encoding.Unicode.GetString(bytes);
         //                    if (strItemName == strName)
         //                    {
         //                        if (item.Count < item.MaxCount)
         //                        {
         //                            ret = new StorageSpace();
         //                            ret.StackObjPtr = item.ServiceID;
         //                            ret.ContainerObjPtr = Program.client.GetStoragePagePtr(bp + 10);
         //                            if ((item.Count + nCount) <= item.MaxCount)
         //                            {
         //                                return ret;
         //                            }
         //                        }
         //                    }
         //                }
         //            }
         //            for (int i = 0; i < item.Width; ++i)
         //            {
         //                for (int j = 0; j < item.Height; ++j)
         //                {
         //                    storageSpace[item.Left + i, item.Top + j] = 1;
         //                }
         //            }
         //        }
         //        //搜索空闲空间
         //        for (short i = 0; i <= (StorageWidth - width); ++i)
         //        {
         //            for (short j = 0; j <= (StorageHeight - height); ++j)
         //            {
         //                bool bFind = true;
         //                for (short m = 0; m < width; ++m)
         //                {
         //                    for (short n = 0; n < height; ++n)
         //                    {
         //                        if (storageSpace[i + m, j + n] == 1)
         //                        {
         //                            bFind = false;
         //                            break;
         //                        }
         //                    }
         //                    if (bFind == false)
         //                        break;
         //                }
         //                if (bFind)
         //                {
         //                    if (ret == null)
         //                        ret = new StorageSpace();
         //                    ret.Left = i;
         //                    ret.Top = j;
         //                    ret.ContainerObjPtr = Program.client.GetStoragePagePtr(bp+10);
         //                    return ret;
         //                }
         //            }
         //        }
         //    }
         //    return ret;
         //}
         StorageSpace SearchSpaceInStorage(short width, short height)
         {
             StorageSpace ret = null;
             //FZ_ReadStorageUIPTR(i);//读取仓库页码]
             int PageCount = Program.client.ReadStoragePageNum();
             for (int bp = 0; bp < PageCount; ++bp)
             {
                 //激活仓库页
                 Program.client.ActiveStoragePageInfo(bp);
                 Thread.Sleep(500);
                 ////////////////////////////////////////计算空间
                 ret = null;
                 List<ItemInfo> Storage = Program.client.GetContainerItemList(bp + 10);
                 //if(Storage.Count<1)
                 //    System.Windows.Forms.MessageBox.Show("bag error!");
                 //遍历仓库页物品占用的空间
                 byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
                 foreach (var item in Storage)
                 {
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
                 //if (Storage.Count < 1)
                 //    System.Windows.Forms.MessageBox.Show("bag error!");
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

                 for (int i = 0; i < 3; ++i)
                 {
                     if (Program.client.IsItemOnMouse() == false)
                     {
                         nRet = 0;
                         break;
                     }

                     nRet = 1;
                     Thread.Sleep(500);
                 }
                 //异常,搜索背包空间
                 if (nRet == 1)
                 {
                     StorageSpace spc = SearchSpaceInStorage(1, 1);
                     Program.client.DropdownItem(spc.ContainerObjPtr, spc.Left, spc.Top);
                     Thread.Sleep(1000);
                 }
                 if (Program.client.IsItemOnMouse())
                 {
                     break;
                 }
                 else
                 {
                     nRet = 0;
                 }
             }
             return nRet;
         }
         //StorageSpace SearchSpaceInStorage(short width,short height)
         //{
         //    StorageSpace spc = SearchSpaceInStorage1(width, height);
         //    if (spc != null)
         //        return spc;
         //    spc = SearchSpaceInStorage2(width, height);
         //    if (spc != null)
         //        return spc;
         //    spc = SearchSpaceInStorage3(width, height);
         //    if (spc != null)
         //        return spc;
         //    spc = SearchSpaceInStorage4(width, height);
         //    if (spc != null)
         //        return spc;
         //    return null;
         //}
         
         //StorageSpace SearchSpaceInStorage1(short width, short height)
         //{
         //    Program.client.ActiveStoragePageInfo(0);
         //    List<ItemInfo> Storage = Program.client.GetContainerItemList(11);

         //    byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
         //    foreach (var item in Storage)
         //    {
         //        for (int i = 0; i < item.Width; ++i)
         //        {
         //            for (int j = 0; j < item.Height; ++j)
         //            {
         //                storageSpace[item.Left + i, item.Top + j] = 1;
         //            }
         //        }
         //    }
         //    //搜索空闲空间
         //    for (short i = 0; i <= (StorageWidth - width); ++i)
         //    {
         //        for (short j = 0; j <= (StorageHeight - height); ++j)
         //        {
         //            bool bFind = true;
         //            for (short m = 0; m < width; ++m)
         //            {
         //                for (short n = 0; n < height; ++n)
         //                {
         //                    if (storageSpace[i + m, j + n] == 1)
         //                    {
         //                        bFind = false;
         //                        break;
         //                    }
         //                }
         //                if (bFind == false)
         //                    break;
         //            }
         //            if (bFind)
         //            {
         //                StorageSpace ret=new StorageSpace();
         //                ret.Left=i;
         //                ret.Top=j;
         //                ret.ContainerObjPtr = Program.client.GetStoragePagePtr(11);
         //                return ret;
         //            }

         //        }
         //    }
         //    return null;
         //}
         //StorageSpace SearchSpaceInStorage2(short width, short height)
         //{
         //    Program.client.ActiveStoragePageInfo(1);
         //    Thread.Sleep(2000);
         //    List<ItemInfo> Storage = Program.client.GetContainerItemList(12);


         //    byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
         //    foreach (var item in Storage)
         //    {
         //        for (int i = 0; i < item.Width; ++i)
         //        {
         //            for (int j = 0; j < item.Height; ++j)
         //            {
         //                storageSpace[item.Left + i, item.Top + j] = 1;
         //            }
         //        }
         //    }
         //    //搜索空闲空间
         //    for (short i = 0; i <= (StorageWidth - width); ++i)
         //    {
         //        for (short j = 0; j <= (StorageHeight - height); ++j)
         //        {
         //            bool bFind = true;
         //            for (short m = 0; m < width; ++m)
         //            {
         //                for (short n = 0; n < height; ++n)
         //                {
         //                    if (storageSpace[i + m, j + n] == 1)
         //                    {
         //                        bFind = false;
         //                        break;
         //                    }
         //                }
         //                if (bFind == false)
         //                    break;
         //            }
         //            if (bFind)
         //            {
         //                StorageSpace ret = new StorageSpace();
         //                ret.Left = i;
         //                ret.Top = j;
         //                ret.ContainerObjPtr = Program.client.GetStoragePagePtr(12);
         //                return ret;
         //            }

         //        }
         //    }
         //    return null;
         //}
         //StorageSpace SearchSpaceInStorage3(short width, short height)
         //{
         //    Program.client.ActiveStoragePageInfo(2);
         //    Thread.Sleep(2000);
         //    List<ItemInfo> Storage = Program.client.GetContainerItemList(13);

         //    byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
         //    foreach (var item in Storage)
         //    {
         //        for (int i = 0; i < item.Width; ++i)
         //        {
         //            for (int j = 0; j < item.Height; ++j)
         //            {
         //                storageSpace[item.Left + i, item.Top + j] = 1;
         //            }
         //        }
         //    }
         //    //搜索空闲空间
         //    for (short i = 0; i <= (StorageWidth - width); ++i)
         //    {
         //        for (short j = 0; j <= (StorageHeight - height); ++j)
         //        {
         //            bool bFind = true;
         //            for (short m = 0; m < width; ++m)
         //            {
         //                for (short n = 0; n < height; ++n)
         //                {
         //                    if (storageSpace[i + m, j + n] == 1)
         //                    {
         //                        bFind = false;
         //                        break;
         //                    }
         //                }
         //                if (bFind == false)
         //                    break;
         //            }
         //            if (bFind)
         //            {
         //                StorageSpace ret = new StorageSpace();
         //                ret.Left = i;
         //                ret.Top = j;
         //                ret.ContainerObjPtr = Program.client.GetStoragePagePtr(13);
         //                return ret;
         //            }

         //        }
         //    }
         //    return null;
         //}
         //StorageSpace SearchSpaceInStorage4(short width, short height)
         //{
         //    Program.client.ActiveStoragePageInfo(3);
         //    Thread.Sleep(2000);
         //    List<ItemInfo> Storage = Program.client.GetContainerItemList(14);

         //    byte[,] storageSpace = new byte[StorageWidth, StorageHeight];
         //    foreach (var item in Storage)
         //    {
         //        for (int i = 0; i < item.Width; ++i)
         //        {
         //            for (int j = 0; j < item.Height; ++j)
         //            {
         //                storageSpace[item.Left + i, item.Top + j] = 1;
         //            }
         //        }
         //    }
         //    //搜索空闲空间
         //    for (short i = 0; i <= (StorageWidth - width); ++i)
         //    {
         //        for (short j = 0; j <= (StorageHeight - height); ++j)
         //        {
         //            bool bFind = true;
         //            for (short m = 0; m < width; ++m)
         //            {
         //                for (short n = 0; n < height; ++n)
         //                {
         //                    if (storageSpace[i + m, j + n] == 1)
         //                    {
         //                        bFind = false;
         //                        break;
         //                    }
         //                }
         //                if (bFind == false)
         //                    break;
         //            }
         //            if (bFind)
         //            {
         //                StorageSpace ret = new StorageSpace();
         //                ret.Left = i;
         //                ret.Top = j;
         //                ret.ContainerObjPtr = Program.client.GetStoragePagePtr(14);
         //                return ret;
         //            }

         //        }
         //    }
         //    return null;
         //}
     }
}
