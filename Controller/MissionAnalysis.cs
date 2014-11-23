using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using Thrift.GameCall;
namespace Controller
{
    public partial class Worker
    {
        double CalcDis(GPoint P1, GPoint P2)
        {
            int x = Math.Abs(P1.x - P2.x);
            int y = Math.Abs(P1.y - P2.y);
            double z = x * x + y * y;
            double dis = Math.Sqrt(z);
            return dis;
        }
        double CalcDis(GPoint P1, short x2,short y2)
        {
            int x = Math.Abs(P1.x - x2);
            int y = Math.Abs(P1.y - y2);
            double z = x * x + y * y;
            double dis = Math.Sqrt(z);
            return dis;
        }
        //double GetBlockCount(GPoint p1, GPoint p2)//一般情况下,大于2的就要放弃,用A*距离 除以直线距离,比例太大的说明有障碍物
        //{
        //    double Astardis = GGetAstarDis(p1.x, p1.y, p2.x, p2.y);
        //    double dis = CalcDis(p1, p2);
        //    double bi = Astardis / dis;
        //    return bi;
        //}
        HashSet<GPoint> LootMemory = new HashSet<GPoint>();
        TrophyInfo TargetTrophy = null;//防止乱跑
        TrophyInfo SearchLoot()
        {
            double NearestDis = 10000.0;
            GPoint tpos = new GPoint();
            TrophyInfo NearestTrophy = null;
            //搜索最近的战利品
            foreach (var item in trophyList)
            {
                if (LootFilter.Contains(item.ObjPtr))
                    continue;
                tpos.x = (ushort)item.X;
                tpos.y = (ushort)item.Y;
                double dis = CalcDis(player.Pos, tpos);
                if (TargetTrophy!=null)
                {
                    if(TargetTrophy.ID==item.ID)
                    {
                        NearestTrophy = item;
                        TargetTrophy = item;
                        break;
                    }
                }
                if (dis < NearestDis)
                {
                    NearestTrophy = item;
                    NearestDis = dis;
                }
            }
            if (TargetTrophy != null && NearestTrophy != null)
            {
                if (TargetTrophy.ID != NearestTrophy.ID)
                {
                    TargetTrophy = NearestTrophy;
                }
            }
            //返回战利品
            return NearestTrophy;
        }
        void ClearMemory()
        {
            bool bDel = false; ;
            do
            {
                bDel = false;
                foreach (var item in LootMemory)
                {
                    double dis = CalcDis(player.Pos, item);
                    if (dis < 30.0)
                    {
                        LootMemory.Remove(item);
                        bDel = true;
                        break;
                    }
                }
            } while (bDel);
        }
        SMonsterInfo SearchMonsterAndLoot(out int MonsterRoundCount)
        {

            //搜索周围战利品,加入到记忆列表
        //    double NearestTrophyDis = 50.0;
            foreach (var item in trophyList)
            {
                if (LootFilter.Contains(item.ObjPtr))
                    continue;
                GPoint tpos = new GPoint();
                tpos.x = (ushort)item.X;
                tpos.y = (ushort)item.Y;
                if (LootMemory.Contains(tpos) == false)
                    LootMemory.Add(tpos);
            }

         //   double SearchNearByDis = NEARBY_DIS;
            MonsterRoundCount = 0;
            GPoint searchCenter = new GPoint();
            searchCenter.x = player.Pos.x;
            searchCenter.y = player.Pos.y;
            double NearestMonsterDis = 1000.0;
            SMonsterInfo targetMonster = null;
            //搜索怪物,找最近的,再找最近的身边的
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;
                double dis = CalcDis(player.Pos, item.Pos);
                if (dis > 50.0)
                {
                    continue;
                }
                if(dis<17.0)
                {
                    targetMonster = item;
                    break;
                }
                if (dis > NearestMonsterDis)
                    continue;
                double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, (ushort)item.Pos.x, (ushort)item.Pos.y);
                
                double bi = Astardis / dis;
                if (bi < 1.3)
                {
                    targetMonster = item;
                    NearestMonsterDis = dis;
                }
            }
            if (targetMonster == null)
                return null;
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;
                if (targetMonster.ID == item.ID)
                    continue;
                double dis = CalcDis(targetMonster.Pos, item.Pos);
                if (dis < 17.0)
                {
                    MonsterRoundCount++;
                }
                
            }
            return targetMonster;
        }
        SMonsterInfo LastTargetMonster = null;
        SMonsterInfo SearchMonsterAndLootNew(out int MonsterRoundCount,out List<SMonsterInfo> NearbyMonster)
        {
            NearbyMonster = new List<SMonsterInfo>();
            //搜索周围战利品,加入到记忆列表
            //    double NearestTrophyDis = 50.0;
            foreach (var item in trophyList)
            {
                if (LootFilter.Contains(item.ObjPtr))
                    continue;
                GPoint tpos = new GPoint();
                tpos.x = (ushort)item.X;
                tpos.y = (ushort)item.Y;
                if (LootMemory.Contains(tpos) == false)
                    LootMemory.Add(tpos);
            }
            //优先攻击怪,近身怪,目标怪身边数量
            //金色和优先攻击怪物的放弃时间加长
            //   double SearchNearByDis = NEARBY_DIS;
            MonsterRoundCount = 0;
            GPoint searchCenter = new GPoint();
            searchCenter.x = player.Pos.x;
            searchCenter.y = player.Pos.y;
            double NearestMonsterDis = 1000.0;
            SMonsterInfo targetMonster = null;
            //搜索怪物,找最近的,再找最近的身边的
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;
                if (LastTargetMonster != null)
                {
                    if (LastTargetMonster.ID == item.ID)
                    {
                        targetMonster = item;
                        break;
                    }
                }
                double dis = CalcDis(player.Pos, item.Pos);
                if (item.Priority == 0)
                {
                    if (dis > 50.0)
                    {
                        continue;
                    }
                }
                else if (item.Priority > 0)
                {
                    if (dis > 80.0)
                    {
                        continue;
                    }
                }

                if (dis < 17.0)
                {
                    NearbyMonster.Add(item);
                }
                //if (dis > NearestMonsterDis)
                //    continue;
                double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, (ushort)item.Pos.x, (ushort)item.Pos.y);

                double bi = Astardis / dis;
                if (bi < 1.3)
                {
                    //已经有目标
                    if (targetMonster != null)
                    {

                        if (targetMonster.Priority < item.Priority)
                        {
                            targetMonster = item;
                            NearestMonsterDis = dis;
                        }
                        else if (targetMonster.Priority == item.Priority)
                        {
                            if (dis<NearestMonsterDis)
                            {
                                targetMonster = item;
                                NearestMonsterDis = dis;
                            }
                        }
                    }
                    else//没有目标的话
                    {
                        targetMonster = item;
                        NearestMonsterDis = dis;
                    }
                }
            }
            if (targetMonster == null)
            {
                LastTargetMonster = null;
                return null;
            }
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;
                if (targetMonster.ID == item.ID)
                    continue;
                double dis = CalcDis(targetMonster.Pos, item.Pos);
                if (dis < 17.0)
                {
                    MonsterRoundCount++;
                }

            }
            LastTargetMonster = targetMonster;
            return targetMonster;
        }
        int SearchLootAndMonster(out SMonsterInfo monster,out TrophyInfo trophy,out int MonsterRoundCount)
        {
            //过滤列表中的不要
            //优先搜索怪物,如果小于10,则直接攻击.
            double NearestDis = 50.0;//攻击距离,不太合适,以后修改
            MonsterRoundCount = 0;

            bool bNearByMonster = false;
       //     GPoint attPos = null;
            monster = null;
            trophy = null;
            double SearchNearByDis = NEARBY_DIS;
            GPoint searchCenter = new GPoint();
            searchCenter.x = player.Pos.x;
            searchCenter.y = player.Pos.y;
            //搜索近距离怪物
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;

                double dis = CalcDis(searchCenter, item.Pos);
                if (dis > SearchNearByDis)
                {
                    continue;
                }
                MonsterRoundCount++;
                if (MonsterRoundCount > Program.config.MultiCount)
                    break;
                if (bNearByMonster)
                {                   
                    continue;
                }
                searchCenter.x = item.Pos.x;
                searchCenter.y = item.Pos.y;
                SearchNearByDis = 20.0;
                bNearByMonster = true;
                monster = item;          
            }
            if(bNearByMonster)
            {
                return MONSTER_TYPE;
            }
            //如果距离大于10,搜索拾取,拾取小于10的,直接返回
            double NearestTrophyDis = 50.0;
            foreach (var item in trophyList)
            {
                if (LootFilter.Contains(item.ObjPtr))
                    continue;
                double dis = CalcDis(player.Pos, item.X, item.Y);
                if (dis < NEARBY_DIS)
                {
                    trophy = item;
                    return TROPHY_TYPE;
                }
                if (dis < NearestTrophyDis)
                {
                 //   double Astardis = GetAstarDis(player.Pos.x, player.Pos.y, (ushort)item.X, (ushort)item.Y);

                    double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, (ushort)item.X, (ushort)item.Y);
                 
                    double bi = Astardis / dis;
                    if (bi < 2.0)
                    {
                        trophy = item;
                        NearestTrophyDis = dis;
                    }
                }
            }
            if (trophy!=null)
                return TROPHY_TYPE;
            //如果战力品为空,返回怪物,否则优先战利品
               
           //搜索远处怪物数量
            //再遍历一次怪物
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;

                double dis = CalcDis(player.Pos, item.Pos);
                if (dis > 50.0)//SearchAttDis
                    continue;
                if (dis < NearestDis)
                {
                    double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, item.Pos.x, item.Pos.y);
                    double bi = Astardis / dis;
                    if (bi < 1.0)
                    {
                        monster = item;
                        NearestDis = dis;
                    }
                }
            }
            if (monster == null)
                return 0;
            //搜索目标旁边的怪物
            foreach (var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;
                if (monster == item)
                    continue;
                double dis = CalcDis(monster.Pos, item.Pos);
                if (dis < 20.0)
                    MonsterRoundCount++;
                if(MonsterRoundCount>Program.config.MultiCount)
                    break;
            }
            return MONSTER_TYPE; 
        }
        
        GPoint SearchMonster()
        {
            //如果当前目标为空,则搜索最近目标
            double NearestDis = 40.0;//攻击距离,不太合适,以后修改
            GPoint attPos = null;
            foreach (var item in monsterList)
            {
                //if (item.ID == player.ID)
                //    continue;
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                
                double dis = CalcDis(player.Pos, item.Pos);
                if (dis < NearestDis)
                {
                    double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, item.Pos.x, item.Pos.y);
                    double bi = Astardis / dis;
                    if (bi < 2.0)
                    {
                        attPos = item.Pos;
                        NearestDis = dis;
                    }

                }
            }
            return attPos;
        }
        SDoor LastTargetDoor = null;
        SDoor SearchDoor()
        {

            SDoor temp = null;
            foreach (var item in doorList)
            {
                if(PassedDoor.Contains(item.Pos)==false)
                {
                    if (CurMapID == pollutantMapID)
                    {
                        SetPollutantPassAbleArea(item.Pos.x, item.Pos.y);
                    }
                    else
                        SetPassAbleArea(item.Pos.x, item.Pos.y);
                    PassedDoor.Add(item.Pos);
                }
                if (LastTargetDoor!=null)
                {
                    if(item.ID==LastTargetDoor.ID)
                    {
                        temp = item;
                    }
                }
                if (temp != null)
                    continue;
                double dis = CalcDis(player.Pos, item.Pos);
                if (dis < 60.0)
                {
                    double Astardis = GGetAstarDis(item.Pos.x,item.Pos.y,player.Pos.x, player.Pos.y);
                    double bi = Astardis / dis;
                    if (bi < 1.1)
                    {
                        
                        temp = item;
                    //    break;
                    }
                }
            }
            if(temp==null)
            {
                LastTargetDoor = null;
            }
            else if (LastTargetDoor==null)
            {
                LastTargetDoor = temp;
            }
            else if (LastTargetDoor.ID != temp.ID)
            {
                LastTargetDoor = temp;
            }
            return temp;
        }
        SBox SearchBox()
        {
            double NearestDis = 60.0;//
            SBox box = null;
            foreach (var item in boxList)
            {
                if (ClickedBoxList.Contains(item.ID))
                    continue;
                if(item.Color>Program.config.BoxFilterColor)
                {
                    ClickedBoxList.Add(item.ID);
                    continue;
                }
                double dis = CalcDis(player.Pos, item.Pos);
                if (dis < NearestDis)
                {
                    double Astardis = GGetAstarDis(player.Pos.x, player.Pos.y, item.Pos.x, item.Pos.y);
                    double bi = Astardis / dis;
                    if (bi < 1.5)
                    {
                        box = item;
                        NearestDis = dis;
                    }
                }
            }
            return box;
        }
        //TrophyInfo SearchTrophy()
        //{
        //    TrophyInfo NeedLootTrohpy = null;
        //    double NearestDis = 40.0;
        //    foreach (var item in trophyList)
        //    {
        //        //if(item.Type>3)
        //        //{
        //        //    if (item.Color < 2)
        //        //        continue;
        //        //}
        //        double dis = CalcDis(player.Pos, item.X,item.Y);
        //        if (dis < NearestDis )
        //        {
        //            double Astardis = GetAstarDis(player.Pos.x, player.Pos.y,(ushort) item.X,(ushort) item.Y);
        //            double bi = Astardis / dis;
        //            if (bi < 2.0)
        //            {
        //                NeedLootTrohpy = item;
        //                NearestDis = dis;
        //            }
        //        }
        //    }
        //    return NeedLootTrohpy;
        //}
        const int BagWidth = 12;
        const int BagHeight = 5;
        bool SearchBagFreeSpace(short width,short height)
        {
            //查找背包中最大连续空间数,大于8个停止查找
            //返回最大连续空间块数
            //生成空间矩阵
        //    Space FreeSpace = new Space();
            byte[,] BagSpace=new byte[BagWidth,BagHeight]; 
            List<ItemSpaceInfo> bag = Program.client.GetBagItemSpaceInfo();
            foreach (var item in bag)
            {
                for (int i = 0; i < item.Width;++i )
                {
                    for(int j=0;j<item.Height;++j)
                    {
                        BagSpace[item.Left+i, item.Top+j] = 1;
                    }
                }
            }
            //搜索空闲空间
           for(int i=0;i<=(BagWidth-width);++i)
           {
               for(int j=0;j<=(BagHeight-height);++j)
               {
                   bool bFind=true;
                   for(int m=0;m<width;++m)
                   {
                       for(int n=0;n<height;++n)
                       {
                           if( BagSpace[i+m,j+n]==1)
                           {
                               bFind = false;
                               break;
                           }
                       }
                       if (bFind == false)
                           break;
                   }
                   if (bFind)
                       return true;
                  
               }
           }
            return false;      
        }
        List<int> GetNearbyCorpse()
        {
            List<int> corpse = new List<int>();
            foreach(var item in monsterList)
            {
                if(item.MaxHP>0&&item.HP==0)
                {
                    double dis=CalcDis(player.Pos, item.Pos);
                    if(dis<40.0)
                    {
                        corpse.Add(item.ID);
                    }
                }
            }
            return corpse;
        }
       
        
        
       
        int CrosssGroup()
        {
            if (bCrossing)
            {
                ActCrossGroup();
            }
            else
                ActMoveToCrossPoint();//走到换组点        
            return 0;
        }
        int GroupExplore()
        {
            //确定是否要换组
            if(bNeedCross)
            {
                CrosssGroup();
            }
            else
            {
                ExploreCurGroup();
            }
            return 0;
        }
        int ExploreCurGroup()
        {
            //如果取不到当前组的点,取下一组
            //    if(0==GetGroupExplorePoint())//能取到则探索
            //    {
            //
            if (curGroup == -1)
                curGroup = GetCurGroup(player.Pos.x, player.Pos.y);

            TargetPoint.x = player.Pos.x;
            TargetPoint.y = player.Pos.y;
            //没有则探索
            int nRet = GetGroupExplorePoint(ref TargetPoint.x, ref TargetPoint.y, curGroup);
            //检测阻塞
            if (player.Pos == lastPlayerPos && TargetPoint == lastTargetPoint)
            {
                blockCount++;
            }
            else
                blockCount = 0;
            if (blockCount > 3)
            {
                // MessageBox.Show("阻塞了");
                SetGroupExploredPoint(TargetPoint.x, TargetPoint.y, curGroup);
                nSleepTime = 50;
                return 0;
            }
            lastPlayerPos.x = player.Pos.x;
            lastPlayerPos.y = player.Pos.y;
            lastTargetPoint.x = TargetPoint.x;
            lastTargetPoint.y = TargetPoint.y;

            if (nRet == 0)
            {
                // ActGroupMove(TargetPoint, curGroup);
                ActMove(TargetPoint);
                return 0;
            }
            //else if (nRet == -1)//本组找不到点了
            //{
            //    ChangeToNextMissionMap();
            //    curStatus = Status.GotoBack;
            //    bSaledComplete = false;
            //    bNeedSave = true;
            //    bNeedResetBattleMapID = true;

            //    return 0;
            //    //分组信息也要清空
            //}
            else if (nRet == -2)//寻找下一个点,本次找的不可达
            {
                nSleepTime = 50;
                return 0;
            }

            //-1,需要找穿越点

            if(0!=SearchUnExploreCrossPoint())
            {
                //回城重置
                ChangeToNextMissionMap();
                curStatus = Status.GotoBack;
                SetNeedGoBack();
                bNeedResetBattleMapID = true;
                curGroup = -1;

                pollutantMapID = 0;//污染地穴地图ID
                LoadPollutantMapID = 0;
                pollutantGatePos = null;//门位置
                pollutantComplete = false;//已经刷完
                bEnterPollutanting = false;//正在进入污染地穴
            }
            else
            {
                bNeedCross = true;
            }
            return 0;
        }
        AreaCrossInfo GetNearestAreaCross()
        {
            double nearestDis=1000.0;
            AreaCrossInfo nearest = null;
            foreach (var item in areaCrossList)
            {
                double dis=CalcDis(player.Pos,item.Pos);
                if(dis<nearestDis)
                {
                    nearest = item;
                    nearestDis = dis;
                }
            }
            return nearest;
        }
        int SearchMatchCorss(int beginGroup,int endGroup)
        {
            int i = 0;
            foreach (var item in allCrossPoint)
            {
                if(item.firstGroup==beginGroup&&item.secondGroup==endGroup)
                {
                    tempCrossInfo.beginGroup = item.firstGroup;
                    tempCrossInfo.beginPos.x = item.firstPos.x;
                    tempCrossInfo.beginPos.y = item.firstPos.y;
                    tempCrossInfo.endGroup = item.secondGroup;
                    tempCrossInfo.endPos.x = item.secondPos.x;
                    tempCrossInfo.endPos.y = item.secondPos.y;
                    tempCrossInfo.ListNum = i;
                    return 0;
                }
                if(item.secondGroup==beginGroup&&item.firstGroup==endGroup)
                {
                    tempCrossInfo.beginGroup = item.secondGroup;
                    tempCrossInfo.beginPos.x = item.secondPos.x;
                    tempCrossInfo.beginPos.y = item.secondPos.y;
                    tempCrossInfo.endGroup = item.firstGroup;
                    tempCrossInfo.endPos.x = item.firstPos.x;
                    tempCrossInfo.endPos.y = item.firstPos.y;
                    tempCrossInfo.ListNum = i;
                    return 0;
                }
                ++i;
            }
            return 1;
        }
        int SearchUnExploreCrossPoint()//搜索未探索的穿越点
        {
           int curGroup = GetCurGroup(player.Pos.x, player.Pos.y);//已实现,声明一下即可
           int i = 0;
           foreach(var item in allCrossPoint)
           {
               if(item.secondGroup==-1)
               {
                   if(curGroup==item.firstGroup)//直连的
                   {
                       endCrossInfo.beginGroup = item.firstGroup;
                       endCrossInfo.beginPos.x = item.firstPos.x;
                       endCrossInfo.beginPos.y = item.firstPos.y;
                       endCrossInfo.endGroup = -1;
                       endCrossInfo.ListNum =  i;

                       tempCrossInfo.beginGroup = item.firstGroup;
                       tempCrossInfo.beginPos.x = item.firstPos.x;
                       tempCrossInfo.beginPos.y = item.firstPos.y;
                       tempCrossInfo.endGroup = -1;
                       tempCrossInfo.ListNum = i;
                       return 0;
                   }
                   else
                   {
                       endCrossInfo.beginGroup = item.firstGroup;
                       endCrossInfo.beginPos.x = item.firstPos.x;
                       endCrossInfo.beginPos.y = item.firstPos.y;
                       endCrossInfo.endGroup = -1;
                       endCrossInfo.ListNum = i;

                       SearchMatchCorss(curGroup,item.firstGroup);
                       return 0;
                   }
               }
               i++;
           }
           return 1;
        }
        void AddUnExploreCrossPoint(int group,GPoint pos)//增加未知探索点
        {
            CrossPoint temp=new CrossPoint();
            temp.firstGroup=group;
            temp.firstPos.x=pos.x;
            temp.firstPos.y=pos.y;
            allCrossPoint.Add(temp);
        }
        int PollutantExplore()
        {
            //如果不在地图里,则去门
            if(CurMapID!=pollutantMapID)
            {
                 //如果离门近,则进门
                return ActEnterPollutant();
            }
            //如果在地图里,则取下一个探索点
           // ushort x = 0;
           // ushort y = 0;
           //int nRet= GetPollutantExplorePoint(ref x,ref  y);
            ////////////////////////////////////////////////////////////////
            int curTickCount = System.Environment.TickCount;
            ActDrinkSpeedFlask(curTickCount);
           TargetPoint.x = player.Pos.x;
           TargetPoint.y = player.Pos.y;
           //没有则探索
           int nRet = GetPollutantExplorePoint(ref TargetPoint.x, ref TargetPoint.y);
           //检测阻塞
           if (player.Pos == lastPlayerPos && TargetPoint == lastTargetPoint)
           {
               blockCount++;
           }
           else
               blockCount = 0;
           if (blockCount > 3)
           {
               // MessageBox.Show("阻塞了");
               SetPollutantExploredPoint(TargetPoint.x, TargetPoint.y);
               nSleepTime = 50;
               return 0;
           }
           lastPlayerPos.x = player.Pos.x;
           lastPlayerPos.y = player.Pos.y;
           lastTargetPoint.x = TargetPoint.x;
           lastTargetPoint.y = TargetPoint.y;

           if (nRet == 0)
               ActMove(TargetPoint);
           else if (nRet == 1)
           {
               ChangeToNextMissionMap();
               curStatus = Status.GotoBack;
               SetNeedGoBack();
               bNeedResetBattleMapID = true;

               pollutantMapID = 0;//污染地穴地图ID
               LoadPollutantMapID = 0;
               pollutantGatePos = null;//门位置
               pollutantComplete = false;//已经刷完
               bEnterPollutanting = false;//正在进入污染地穴
               //MessageBox.Show("获取不到点了");
               //bWorking = false;
               //设置任务点为下一个地图
              
               //curStatus = Status.GotoBack;


               //pollutantComplete = false;
               //bSaledComplete = false;
               //bNeedSave = true;
               //bNeedIdentity = true;
               //bNeedResetBattleMapID = true;
           }
           else if (nRet == 2)
               nSleepTime = 50;
           return 0;
        }
        double GGetAstarDis(ushort x,ushort y,ushort px,ushort py)
        {
            double Astardis = 100;
            if (CurMapID == pollutantMapID)
                Astardis = GetPollutantAstarDis(x, y, px, py);
            else
                Astardis = GetAstarDis(x, y, px, py);
            return Astardis;
        }
    }
}
