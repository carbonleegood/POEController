using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;
using Thrift.GameCall;
using System.Media;
namespace Controller
{
    public partial class Worker
    {
        Random rd = new Random();
        void RandomPos(GPoint pos)
        {
            pos.x += (ushort)rd.Next(-1, 1);
            pos.y += (ushort)rd.Next(-1, 1);
        }
        //void ActKillMonster(GPoint pos,double dis)
        //{
            
        //     //如果距离过远,则走过去
        //    if(dis>attackDis)
        //    {
        //        Program.client.Move(pos.x, pos.y);
        //        nSleepTime = 50;
        //        return;
        //    }
        //    RandomPos(pos);
        //    //Program.client.StopMove();
        //    if (player.HP > 20)
        //        Program.client.CastUntargetSkill((short)pos.x, (short)pos.y, 3, 8);
        //    else
        //        Program.client.CastUntargetSkill((short)pos.x, (short)pos.y, 0, 8);
        //    nSleepTime = 600;
        //}
        void ActKillMonsterNew(GPoint pos, int Skill, int TargetID, int nAttackSpeed)
        {
            if (TargetID == 0)
            {
                Program.client.CastUntargetSkill((short)pos.x, (short)pos.y,(short) Skill, 8);
                if (Program.config.bUseSafeAttSpeed || nAttackSpeed == 0)
                    Program.client.WaitCastComplete();
                else
                    Thread.Sleep(nAttackSpeed);
            }
            else
            {
                Program.client.CastTargetSkill(TargetID, (short)Skill, 8);
                if (Program.config.bUseSafeAttSpeed || nAttackSpeed==0)
                    Program.client.WaitCastComplete();
                else
                    Thread.Sleep(nAttackSpeed);
            }
            nSleepTime = 0;
        }
        void ActKillMonster(GPoint pos, double dis, int Skill, int TargetID, int nAttackSpeed)
        {
            double SkillDis = 0;
            int SleepTime = 0;
            switch (Skill)//普通攻击
            {
                case 0:
                    SkillDis = Program.config.NorAttDis;
                    SleepTime = Program.config.NorAttStep;
                    break;
                case 3:
                    SkillDis = Program.config.SinAttDis;
                    SleepTime = Program.config.SinAttStep;
                    break;
                case 4:
                    SkillDis = Program.config.MulAttDis;
                    SleepTime = Program.config.MulAttStep;
                    break;
                default:
                    SkillDis = 40.0;
                    break;
            }
            if (dis > SkillDis)
            {
                Program.client.Move(pos.x, pos.y);
                nSleepTime = 50;
                return;
            }
            if (TargetID == 0)
            {
                Program.client.CastUntargetSkill((short)pos.x, (short)pos.y, (short)Skill, 8);
                if (Program.config.bUseSafeAttSpeed || nAttackSpeed == 0)
                    Program.client.WaitCastComplete();
                else
                    Thread.Sleep(nAttackSpeed);
                SleepTime = 0;
            }
            else
            {
                Program.client.CastTargetSkill(TargetID, (short)Skill, 8);
                if (Program.config.bUseSafeAttSpeed || nAttackSpeed == 0)
                    Program.client.WaitCastComplete();
                else
                    Thread.Sleep(nAttackSpeed);
                SleepTime = 0;
            }
            nSleepTime = SleepTime;
        }
        void SingleAttack(GPoint pos, double dis, int TargetID)
        {
            //如果有魔法,且有单体魔法
            if ((Program.config.nSinAttKey != -1) && player.MP > 20)
            {
                int curTime = System.Environment.TickCount; ;
                //如果有陷阱类魔法,放陷阱
                //foreach (var item in Program.config.singleTrapSkill)
                //{
                //    for (int i = 0; i < item.CastTime.Count; ++i)
                //    {
                //        if ((curTime - item.CastTime[i]) > (item.Step * 1000))
                //        {
                //            item.CastTime[i] = curTime;
                //            ActKillMonster(pos, dis, (short)item.Key, 0);
                //            return;
                //        }
                //    }
                //}
                ActKillMonster(pos, dis, Program.config.nSinAttKey, TargetID, Program.config.SinAttStep);
            }
            else if (Program.config.nNorAttKey != -1)//使用普通单体攻击
            {
                ActKillMonster(pos, dis, Program.config.nNorAttKey, TargetID, Program.config.NorAttStep);
            }
            else
                Thread.Sleep(200);
            //如果距离远,走过去
            return;
        }
        void AttackNearbyMonster(List<SMonsterInfo> NearbyMonster)
        {
            if(NearbyMonster.Count>1&&Program.config.nMulAttKey!=-1)
            {
                ActKillMonsterNew(player.Pos, Program.config.nMulAttKey, 0, Program.config.MulAttStep);
            }
            else 
            {
                ActKillMonsterNew(player.Pos, Program.config.nSinAttKey, NearbyMonster[0].ID, Program.config.SinAttStep);
            }
        }
        void SingleAttackNew(GPoint pos, int TargetID)
        {
            //如果有魔法,且有单体魔法
            if ((Program.config.nSinAttKey != -1) && player.MP > 20)
            {
                int curTime = System.Environment.TickCount; ;
                //如果有陷阱类魔法,放陷阱
                ActKillMonsterNew(pos, Program.config.nSinAttKey, TargetID, Program.config.SinAttStep);
            }
            else if (Program.config.nNorAttKey != -1)//使用普通单体攻击
            {
                ActKillMonsterNew(pos, Program.config.nNorAttKey, TargetID, Program.config.NorAttStep);
            }
            else
                Thread.Sleep(200);
            //如果距离远,走过去
            return;
        }
        void MultiAttackNew(GPoint pos)
        {
            int curTime = System.Environment.TickCount; 
            //如果有陷阱类魔法,放陷阱
            foreach (var item in Program.config.multiTrapSkill)
            {
                for (int i = 0; i < item.CastTime.Count; ++i)
                {
                    if ((curTime - item.CastTime[i]) > (item.Step))
                    {
                        item.CastTime[i] = curTime;
                        ActKillMonsterNew(pos, (short)item.Key, 0,0);
                        return;
                    }
                }
            }
            //直接使用群攻
            ActKillMonsterNew(pos, Program.config.nMulAttKey, 0, Program.config.MulAttStep);
            return;
        }
        void MultiAttack(GPoint pos, double dis)
        {
            int curTime = System.Environment.TickCount; ;
            //如果有陷阱类魔法,放陷阱
            foreach (var item in Program.config.multiTrapSkill)
            {
                for (int i = 0; i < item.CastTime.Count; ++i)
                {
                    if ((curTime - item.CastTime[i]) > (item.Step))
                    {
                        item.CastTime[i] = curTime;
                        ActKillMonster(pos, dis, (short)item.Key, 0,0);
                        return;
                    }
                }
            }
            //直接使用群攻
            ActKillMonster(pos, dis, Program.config.nMulAttKey, 0, Program.config.MulAttStep);
            return;
        }

        void ActMove(GPoint pos)
        {
            Program.client.Move(pos.x,pos.y);
        }
        int safeBlockCount = 0;
        GPoint lastSafeMovePos = new GPoint();
        void ActSafeMove(GPoint pos)
        {
            if (CurMapID == pollutantMapID)
            {
                PollutantMoveToPoint(pos.x, pos.y, player.Pos.x, player.Pos.y);
            }
            else
            {
                MoveToPoint(pos.x, pos.y, player.Pos.x, player.Pos.y);
            }

            //检测卡位
            if (player.Pos == lastSafeMovePos)
            {
                safeBlockCount++;
            }
            else
            {
                lastSafeMovePos.x = player.Pos.x;
                lastSafeMovePos.y = player.Pos.y;
                safeBlockCount = 0;
            }
            if (safeBlockCount > 20)
            {
                safeBlockCount = 0;
                //小退
                ReturnRolePolicy();
            }

        }
        int CastShieldSkill(int curTickCount)
        {
            if (Program.config.shieldSkill.Count > 0)
            {
                foreach (var item in Program.config.shieldSkill)
                {
                    //符合条件则释放,退出
                    switch (item.Key)
                    {
                        case 0:
                            if ((item.Step) < (curTickCount - LastLeftSkillCastTime))
                            {
                                LastLeftSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 0, 8);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 1:
                            if ((item.Step ) < (curTickCount - LastMidSkillCastTime))
                            {
                                LastMidSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 1, 8);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 2:
                            if ((item.Step ) < (curTickCount - LastRightSkillCastTime))
                            {
                                LastRightSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 2, 8);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 3:
                            if ((item.Step ) < (curTickCount - LastQSkillCastTime))
                            {
                                LastQSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 3, 8);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 4:
                            if ((item.Step ) < (curTickCount - LastWSkillCastTime))
                            {
                                LastWSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 4, 8);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 5:
                            if ((item.Step ) < (curTickCount - LastESkillCastTime))
                            {
                                LastESkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 5, 8);
                                //   Thread.Sleep(1000);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 6:
                            if ((item.Step ) < (curTickCount - LastRSkillCastTime))
                            {
                                LastRSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 6, 8);
                                //  Thread.Sleep(1000);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 7:
                            if ((item.Step ) < (curTickCount - LastTSkillCastTime))
                            {
                                LastTSkillCastTime = curTickCount;
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 7, 8);
                                // Thread.Sleep(1000);
                                Program.client.WaitCastComplete();
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                    }
                }
            }
            return 1;
        }
        int CastShieldSkillNow(int curTickCount)
        {
            if (Program.config.shieldSkill.Count > 0)
            {
                foreach (var item in Program.config.shieldSkill)
                {
                    //符合条件则释放,退出
                    switch (item.Key)
                    {
                        case 0:
                            LastLeftSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 0, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 1:
                            LastMidSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 1, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 2:
                            LastRightSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y,2, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 3:
                            LastQSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 3, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 4:
                            LastWSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 4, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 5:
                            LastESkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 5, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 6:
                            LastRSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 6, 8);
                            Program.client.WaitCastComplete();
                            break;
                        case 7:
                            LastTSkillCastTime = curTickCount;
                            Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 7, 8);
                            Program.client.WaitCastComplete();
                            break;
                    }
                }
            }
            return 0;
        }
        int CastSummonerSkill(int curTickCount)
        {
            if (Program.config.summonerSkill.Count > 0)
            {
                foreach (var item in Program.config.summonerSkill)
                {
                    //符合条件则释放,退出
                    switch (item.Key)
                    {
                        case 0:
                            if ((item.Step) < (curTickCount - LastLeftSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastLeftSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 0, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastLeftSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 0, 8);
                                        Program.client.WaitCastComplete();
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 1:
                            if ((item.Step ) < (curTickCount - LastMidSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList=GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastMidSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 1, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastMidSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 1, 8);
                                        Program.client.WaitCastComplete();
                                    //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 2:
                            if ((item.Step ) < (curTickCount - LastRightSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastRightSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 2, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastRightSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 2, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 3:
                            if ((item.Step ) < (curTickCount - LastQSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastQSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 3, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastQSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 3, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 4:
                            if ((item.Step ) < (curTickCount - LastWSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastWSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 4, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastWSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 4, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 5:
                            if ((item.Step ) < (curTickCount - LastESkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastESkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 5, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastESkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 5, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 6:
                            if ((item.Step ) < (curTickCount - LastRSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastRSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 6, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastRSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y,6, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                        case 7:
                            if ((item.Step) < (curTickCount - LastTSkillCastTime))
                            {
                                if (item.NeedCorpse)
                                {
                                    //搜索尸体,找到了则释放
                                    List<int> CorpseList = GetNearbyCorpse();
                                    if (item.Count <= CorpseList.Count)//数量够则释放
                                    {
                                        LastTSkillCastTime = curTickCount;
                                        for (int i = 0; i < item.Count; ++i)
                                        {
                                            Program.client.CastTargetSkill(CorpseList[i], 7, 8);
                                            Program.client.WaitCastComplete();
                                        }
                                        CorpseList.Clear();
                                        nSleepTime = 0;
                                        return 0;
                                    }
                                    else//否则返回
                                    {
                                        CorpseList.Clear();
                                        continue;
                                    }

                                }
                                else
                                {
                                    LastTSkillCastTime = curTickCount;
                                    for (int i = 0; i < item.Count; ++i)
                                    {
                                        Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 7, 8);
                                        Program.client.WaitCastComplete();
                                        //    Thread.Sleep(1000);
                                    }
                                    nSleepTime = 0;
                                    return 0;
                                }
                            }
                            break;
                    }

                }
            }
            return 1;
        }
        int CastTTSkill(int curTickCount)
        {
            if (Program.config.ttSkill.Count > 0)
            {
                foreach (var item in Program.config.ttSkill)
                {
                    //符合条件则释放,退出
                    switch (item.Key)
                    {
                        case 0:
                            if ((item.Step ) < (curTickCount - LastLeftSkillCastTime))
                            {
                                LastLeftSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 0, 8);
                                    // Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 1:
                            if ((item.Step ) < (curTickCount - LastMidSkillCastTime))
                            {
                                LastMidSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 1, 8);
                                    // Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 2:
                            if ((item.Step ) < (curTickCount - LastRightSkillCastTime))
                            {
                                LastRightSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 2, 8);
                                    // Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 3:
                            if ((item.Step ) < (curTickCount - LastQSkillCastTime))
                            {
                                LastQSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 3, 8);
                                    // Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 4:
                            if ((item.Step ) < (curTickCount - LastWSkillCastTime))
                            {
                                LastWSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 4, 8);
                                    // Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 5:
                            if ((item.Step ) < (curTickCount - LastESkillCastTime))
                            {
                                LastESkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 5, 8);
                                    //   Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 6:
                            if ((item.Step ) < (curTickCount - LastRSkillCastTime))
                            {
                                LastRSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)(player.Pos.x + i * 5), (short)(player.Pos.y + i * 5), 6, 8);
                                    //  Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                        case 7:
                            if ((item.Step ) < (curTickCount - LastTSkillCastTime))
                            {
                                LastTSkillCastTime = curTickCount;
                                for (int i = 0; i < item.Count; ++i)
                                {
                                    Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 7, 8);
                                    //  Thread.Sleep(1000);
                                    Program.client.WaitCastComplete();
                                }
                                nSleepTime = 0;
                                return 0;
                            }
                            break;
                    }

                }
            }
            return 1;
        }
        int CastTTSkillNow(int curTickCount)
        {
            if (Program.config.ttSkill.Count > 0)
            {
                foreach (var item in Program.config.ttSkill)
                {
                    //符合条件则释放,退出
                    switch (item.Key)
                    {
                        case 0:
                            LastLeftSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y,0, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 1:
                            LastMidSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 1, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 2:
                            LastRightSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 2, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 3:
                            LastQSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 3, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 4:
                            LastWSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 4, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 5:
                            LastESkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 5, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 6:
                            LastRSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)(player.Pos.x + i * 5), (short)(player.Pos.y + i * 5), 6, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                        case 7:
                            LastTSkillCastTime = curTickCount;
                            for (int i = 0; i < item.Count; ++i)
                            {
                                Program.client.CastUntargetSkill((short)player.Pos.x, (short)player.Pos.y, 7, 8);
                                Program.client.WaitCastComplete();
                            }
                            break;
                    }
                }
            }
            return 0;
        }

        int CastTrapSkillNow(int curTickCount,GPoint pos)
        {
            if (Program.config.multiTrapSkill.Count > 0)
            {
                foreach  (var item in Program.config.multiTrapSkill)
                {
                    for (int i = 0; i < item.CastTime.Count; ++i)
                    {
                        Program.client.CastUntargetSkill((short)pos.x, (short)pos.y, (short)item.Key, 8);
                        item.CastTime[i] = curTickCount;
                        Program.client.WaitCastComplete();
                    }
                }
            }
            return 1;
         }
        int ActNormalExplore()
        {
            TargetPoint.x = player.Pos.x;
            TargetPoint.y = player.Pos.y;
            //没有则探索
            int nRet = GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y);
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
                SetExploredPoint(TargetPoint.x, TargetPoint.y);
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
                //MessageBox.Show("获取不到点了");
                //bWorking = false;
                //设置任务点为下一个地图
                ChangeToNextMissionMap();
                curStatus = Status.GotoBack;
                SetNeedGoBack();
                bNeedResetBattleMapInfo = true;
                //bNeedResetBattleMapID = true;

                //pollutantMapID=0;//污染地穴地图ID
                //LoadPollutantMapID = 0;
                //pollutantGatePos = null;//门位置
                //pollutantComplete=false;//已经刷完
                //bEnterPollutanting = false;//正在进入污染地穴
            }
            else if (nRet == 2)
                nSleepTime = 50;
            return 0;
        }
        public Controller UI = null;
        int ActHandNormalExplore()//手动探索模式
        {
            TargetPoint.x = player.Pos.x;
            TargetPoint.y = player.Pos.y;
            //没有则探索
            int nRet = GetExplorePoint(ref TargetPoint.x, ref TargetPoint.y);
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
                SetExploredPoint(TargetPoint.x, TargetPoint.y);
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
                UI.Speak("Explore Complete");
                System.Windows.Forms.MessageBox.Show("探索完成");
                bWorking = false;
                UI.bWorking = false;
                //设置任务点为下一个地图
             //  ChangeToNextMissionMap();
                curStatus = Status.GotoBack;
                SetNeedGoBack();
                bNeedResetBattleMapInfo = true;
               // bNeedResetBattleMapID = true;
            }
            else if (nRet == 2)
                nSleepTime = 50;
            return 0;
        }
        void ActFarMove(GPoint tPos,GPoint pPos)
        {
            MoveToPoint(tPos.x, tPos.y, pPos.x, pPos.y);
        }
        int ActMoveToCrossPoint()//移动到穿越点
        {
            //在穿越点附近
            double dis = CalcDis(tempCrossInfo.beginPos, player.Pos);
            if (dis < 25.0f)
            {
                bCrossing = true;
            }
            else
                ActFarMove(tempCrossInfo.beginPos,player.Pos);
            return 0;
        }
        int ActCrossGroup()//穿越
        {
            curGroup = GetCurGroup(player.Pos.x, player.Pos.y);//已实现,声明一下即可

            if (curGroup == tempCrossInfo.beginGroup)//如果相等,重新穿越
            {
                curGroup = -1;
                //重新穿越
                int ObjPtr = Program.client.GetNearbyCrossObjPtr();
                Program.client.ActiveTarget(ObjPtr);
                Thread.Sleep(1000 * 5);
                return 0;
            }

            if (tempCrossInfo.endGroup == -1)
            {
                //穿越完成,切换到探索模式
                allCrossPoint[endCrossInfo.ListNum].secondGroup = curGroup;
                AreaCrossInfo areaCross=GetNearestAreaCross();
                allCrossPoint[endCrossInfo.ListNum].secondPos.x = areaCross.Pos.x;
                allCrossPoint[endCrossInfo.ListNum].secondPos.y = areaCross.Pos.y;
                AddedCrossPoint.Add(areaCross.Pos);//穿越后的点,外面搜索的时候也要好好处理一下
                bNeedCross = false;
                bCrossing = false;
                return 1;
            }
            else
            {
                tempCrossInfo.beginGroup = endCrossInfo.beginGroup;
                tempCrossInfo.beginPos.x = endCrossInfo.beginPos.x;
                tempCrossInfo.beginPos.y = endCrossInfo.beginPos.y;

                tempCrossInfo.endGroup = endCrossInfo.endGroup;
                tempCrossInfo.endPos.x = endCrossInfo.endPos.x;
                tempCrossInfo.endPos.y = endCrossInfo.endPos.y;
                tempCrossInfo.ListNum = endCrossInfo.ListNum;

                bCrossing = false;
                return 2;//切换到移动到穿越点状态
            }
        }
        int ActAttNearbyMonsterAndBox()
        {
            //搜索身边的怪物和箱子
            foreach(var item in monsterList)
            {
                if (item.MaxHP == 0 || item.HP == 0)
                    continue;
                if (FilterMonsterList.Contains(item.Pos))
                    continue;

                double dis = CalcDis(player.Pos, item.Pos);
                if (dis > 15.0)
                {
                    continue;
                }
                SingleAttackNew(item.Pos, item.ID);
                return 1;
            }
            foreach(var item in boxList)
            {
                if (ClickedBoxList.Contains(item.ID))
                    continue;
                if (item.Color > Program.config.BoxFilterColor)
                {
                    ClickedBoxList.Add(item.ID);
                    continue;
                }
                double dis = CalcDis(player.Pos, item.Pos);
                if (dis > 15.0)
                    continue;
                Program.client.ActiveTarget(item.ObjPtr);
                return 1;
            }
            return 0;
        }
        int ActOpenDoor(SDoor door)
        {
            //if (player.Pos == lastPlayerPos && door.Pos == lastTargetPoint)
            //{
            //    blockCount++;
            //    nSleepTime = 50;
            //}
            //else
            //    blockCount = 0;
            //lastPlayerPos.x = player.Pos.x;
            //lastPlayerPos.y = player.Pos.y;
            //lastTargetPoint.x = door.Pos.x;
            //lastTargetPoint.y = door.Pos.y;
            //if (blockCount > 3)
            //{
            //    TargetBox.ID = 0;
            //    nSleepTime = 50;
            //    return 0;
            //}
            //获取A*距离,过大的走过去
            double dis = CalcDis(door.Pos, player.Pos);
            if (dis > 15)
            {
                if (ActAttNearbyMonsterAndBox() == 1)
                {
                    return 1;
                }
                ActSafeMove(door.Pos);
            }
            else
            {
                Program.client.ActiveTarget(door.ObjPtr);
            }

            nSleepTime = 50;
            return 0;
        }
        int LastUseFlaskTime1 = 0;
        int FlaskHitCount1 = 0;

        int LastUseFlaskTime2 = 0;
        int FlaskHitCount2 = 0;

        int LastUseFlaskTime3 = 0;
        int FlaskHitCount3 = 0;

        int LastUseFlaskTime4 = 0;
        int FlaskHitCount4 = 0;

        int LastUseFlaskTime5 = 0;
        int FlaskHitCount5 = 0;
        int ActDrinkSpeedFlask(int curTickCount)
        {
            if(Program.config.Flask1.Use==true&&Program.config.Flask1.Type==3)
            {
                if (Program.config.Flask1.KeyList.Count > 0)
                {
                    int step = curTickCount - LastUseFlaskTime1;
                    if ((Program.config.Flask1.Step * 1000) < step)
                    {
                        int key = FlaskHitCount1 % Program.config.Flask1.KeyList.Count;
                        Program.client.HitKey(Program.config.Flask1.KeyList[key]);
                        FlaskHitCount1++;
                        LastUseFlaskTime1 = curTickCount;
                    }
                }
            }
            if (Program.config.Flask2.Use == true && Program.config.Flask2.Type == 3)
            {
                if (Program.config.Flask2.KeyList.Count > 0)
                {
                    int step = curTickCount - LastUseFlaskTime2;
                    if ((Program.config.Flask2.Step * 1000) < step)
                    {
                        int key = FlaskHitCount2 % Program.config.Flask2.KeyList.Count;
                        Program.client.HitKey(Program.config.Flask2.KeyList[key]);
                        FlaskHitCount2++;
                        LastUseFlaskTime2 = curTickCount;
                    }
                }
            }
            if (Program.config.Flask3.Use == true && Program.config.Flask3.Type == 3)
            {
                if (Program.config.Flask3.KeyList.Count > 0)
                {
                    int step = curTickCount - LastUseFlaskTime3;
                    if ((Program.config.Flask3.Step * 1000) < step)
                    {
                        int key = FlaskHitCount3 % Program.config.Flask3.KeyList.Count;
                        Program.client.HitKey(Program.config.Flask3.KeyList[key]);
                        FlaskHitCount3++;
                        LastUseFlaskTime3 = curTickCount;
                    }
                }
            }
            if (Program.config.Flask4.Use == true && Program.config.Flask4.Type == 3)
            {
                if (Program.config.Flask4.KeyList.Count > 0)
                {
                    int step = curTickCount - LastUseFlaskTime4;
                    if ((Program.config.Flask4.Step * 1000) < step)
                    {
                        int key = FlaskHitCount4 % Program.config.Flask4.KeyList.Count;
                        Program.client.HitKey(Program.config.Flask4.KeyList[key]);
                        FlaskHitCount4++;
                        LastUseFlaskTime4 = curTickCount;
                    }
                }
            }
            if (Program.config.Flask5.Use == true && Program.config.Flask5.Type == 3)
            {
                if (Program.config.Flask5.KeyList.Count > 0)
                {
                    int step = curTickCount - LastUseFlaskTime5;
                    if ((Program.config.Flask5.Step * 1000) < step)
                    {
                        int key = FlaskHitCount5 % Program.config.Flask5.KeyList.Count;
                        Program.client.HitKey(Program.config.Flask5.KeyList[key]);
                        FlaskHitCount5++;
                        LastUseFlaskTime5 = curTickCount;
                    }
                }
            }
            return 0;
        }
        int ActDrinkFlask(int curTickCount)
        {
            if(Program.config.Flask1.Use==true&&Program.config.Flask1.Type!=3)
            {
                int PlayerData=-1;
                switch(Program.config.Flask1.Type)
                {
                    case 0://生命
                        PlayerData=player.HP;
                        break;
                    case 1://魔法
                        PlayerData=player.MP;
                        break;
                    case 2://护盾
                        PlayerData=player.Shield;
                        break;
                }
                if(Program.config.Flask1.CalcData>PlayerData)
                {
                    if (Program.config.Flask1.KeyList.Count > 0)
                    {
                        int step = curTickCount - LastUseFlaskTime1;
                        if ((Program.config.Flask1.Step * 1000) < step)
                        {
                            int key = FlaskHitCount1 % Program.config.Flask1.KeyList.Count;
                            Program.client.HitKey(Program.config.Flask1.KeyList[key]);
                            FlaskHitCount1++;
                            LastUseFlaskTime1 = curTickCount;
                        }
                    }
                }
            }

            if(Program.config.Flask2.Use==true&&Program.config.Flask2.Type!=3)
            {
                int PlayerData=-1;
                switch(Program.config.Flask2.Type)
                {
                    case 0://生命
                        PlayerData=player.HP;
                        break;
                    case 1://魔法
                        PlayerData=player.MP;
                        break;
                    case 2://护盾
                        PlayerData=player.Shield;
                        break;
                }
                if (Program.config.Flask2.CalcData > PlayerData)
                {
                    if (Program.config.Flask2.KeyList.Count > 0)
                    {
                        int step = curTickCount - LastUseFlaskTime2;
                        if ((Program.config.Flask2.Step * 1000) < step)
                        {
                            int key = FlaskHitCount2 % Program.config.Flask2.KeyList.Count;
                            Program.client.HitKey(Program.config.Flask2.KeyList[key]);
                            FlaskHitCount2++;
                            LastUseFlaskTime2 = curTickCount;
                        }
                    }
                }
            }
            if(Program.config.Flask3.Use==true&&Program.config.Flask3.Type!=3)
            {
                int PlayerData=-1;
                switch(Program.config.Flask3.Type)
                {
                    case 0://生命
                        PlayerData=player.HP;
                        break;
                    case 1://魔法
                        PlayerData=player.MP;
                        break;
                    case 2://护盾
                        PlayerData=player.Shield;
                        break;
                }
                if (Program.config.Flask3.CalcData > PlayerData)
                {
                    if (Program.config.Flask3.KeyList.Count > 0)
                    {
                        int step = curTickCount - LastUseFlaskTime3;
                        if ((Program.config.Flask3.Step * 1000) < step)
                        {
                            int key = FlaskHitCount3 % Program.config.Flask3.KeyList.Count;
                            Program.client.HitKey(Program.config.Flask3.KeyList[key]);
                            FlaskHitCount3++;
                            LastUseFlaskTime3 = curTickCount;
                        }
                    }
                }
            }
            if(Program.config.Flask4.Use==true&&Program.config.Flask4.Type!=3)
            {
                int PlayerData=-1;
                switch(Program.config.Flask4.Type)
                {
                    case 0://生命
                        PlayerData=player.HP;
                        break;
                    case 1://魔法
                        PlayerData=player.MP;
                        break;
                    case 2://护盾
                        PlayerData=player.Shield;
                        break;
                }
                if (Program.config.Flask4.CalcData > PlayerData)
                {
                    if (Program.config.Flask4.KeyList.Count > 0)
                    {
                        int step = curTickCount - LastUseFlaskTime4;
                        if ((Program.config.Flask4.Step * 1000) < step)
                        {
                            int key = FlaskHitCount4 % Program.config.Flask4.KeyList.Count;
                            Program.client.HitKey(Program.config.Flask4.KeyList[key]);
                            FlaskHitCount4++;
                            LastUseFlaskTime4 = curTickCount;
                        }
                    }
                }
            }
            if(Program.config.Flask5.Use==true&&Program.config.Flask5.Type!=3)
            {
                int PlayerData=-1;
                switch(Program.config.Flask5.Type)
                {
                    case 0://生命
                        PlayerData=player.HP;
                        break;
                    case 1://魔法
                        PlayerData=player.MP;
                        break;
                    case 2://护盾
                        PlayerData=player.Shield;
                        break;
                }
                if (Program.config.Flask5.CalcData > PlayerData)
                {
                    if (Program.config.Flask5.KeyList.Count > 0)
                    {
                        int step = curTickCount - LastUseFlaskTime5;
                        if ((Program.config.Flask5.Step * 1000) < step)
                        {
                            int key = FlaskHitCount5 % Program.config.Flask5.KeyList.Count;
                            Program.client.HitKey(Program.config.Flask5.KeyList[key]);
                            FlaskHitCount5++;
                            LastUseFlaskTime5 = curTickCount;
                        }
                    }
                }
            }
            return 0;
        //    foreach (var item in Program.config.HPKeyList)
        //    {
        //        int step = curTickCount - LastHitKeyTime[item.Key];
        //        if ((item.Step * 1000) < step)
        //        {
        //            if (player.HP < item.HP)
        //            {
        //                Program.client.HitKey(item.Key);
        //                LastHitKeyTime[item.Key] = curTickCount;
        //            }
        //        }
        //    }
        //    foreach (var item in Program.config.ShieldKeyList)
        //    {
        //        int step = curTickCount - LastHitKeyTime[item.Key];
        //        if ((item.Step * 1000) < step)
        //        {
        //            if (player.Shield < item.Shield)
        //            {
        //                Program.client.HitKey(item.Key);
        //                LastHitKeyTime[item.Key] = curTickCount;
        //            }
        //        }
        //    }
        //    foreach (var item in Program.config.MPKeyList)
        //    {
        //        int step = curTickCount - LastHitKeyTime[item.Key];
        //        if ((item.Step *1000)< step)
        //        {
        //            if (player.MP < item.MP)
        //            {
        //                Program.client.HitKey(item.Key);
        //                LastHitKeyTime[item.Key] = curTickCount;
        //            }
        //        }
        //    }
        //    return 0;
        //}
        //int ActDrinkFlask(int curTickCount)//喝药剂部分
        //{
        //    if (Program.config.HPKey.Count > 0)
        //    {
        //        if (player.HP < Program.config.HPFlask)
        //        {
        //            int step = curTickCount - LastUseHPTime;
        //            if (step > (Program.config.HPStep * 1000))
        //            {
        //                int index = nDrinkHP % Program.config.HPKey.Count;
        //             //   HitKey(Program.config.HPKey[index]);
        //                Program.client.HitKey(Program.config.HPKey[index]);
        //                ++nDrinkHP;
        //                LastUseHPTime = curTickCount;
        //            }
        //        }
        //    }
        //    if (Program.config.DirHPKey.Count > 0)
        //    {
        //        if (player.HP < Program.config.DirHPFlask)
        //        {
        //            int step = curTickCount - LastUseDirHPTime;
        //            if (step > (Program.config.DirHPStep * 1000))
        //            {
        //                int index = nDrinkDirectHP % Program.config.DirHPKey.Count;
        //              //  HitKey(Program.config.DirHPKey[index]);
        //                Program.client.HitKey(Program.config.DirHPKey[index]);
        //                ++nDrinkDirectHP;
        //                LastUseDirHPTime = curTickCount;
        //            }
        //        }
        //    }
        //    if (Program.config.MPKey.Count > 0)
        //    {
        //        if (player.MP < Program.config.MPFlask)
        //        {
        //            int step = curTickCount - LastUseMPTime;
        //            if (step > (Program.config.MPStep * 1000))
        //            {
        //                int index = nDrinkMP % Program.config.MPKey.Count;
        //              //  HitKey(Program.config.MPKey[index]);
        //                Program.client.HitKey(Program.config.MPKey[index]);
        //                ++nDrinkMP;
        //                LastUseMPTime = curTickCount;
        //            }
        //        }
        //    }
        //    if (Program.config.DirMPKey.Count > 0)
        //    {
        //        if (player.MP < Program.config.DirMPFlask)
        //        {
        //            int step = curTickCount - LastUseDirMPTime;
        //            if (step > (Program.config.DirMPStep * 1000))
        //            {
        //                int index = nDrinkDirectMP % Program.config.DirMPKey.Count;
        //               // HitKey(Program.config.DirMPKey[index]);
        //                Program.client.HitKey(Program.config.DirMPKey[index]);
        //                ++nDrinkDirectMP;
        //                LastUseDirMPTime = curTickCount;
        //            }
        //        }
        //    }
        //    return 0;
        }
        int ActLootTrophy(TrophyInfo trophy)
        {
            //远了走过去,
            GPoint tempPos = new GPoint();
            tempPos.x = (ushort)trophy.X;
            tempPos.y = (ushort)trophy.Y;
            double dis = CalcDis(player.Pos, trophy.X, trophy.Y);
            if (dis > NEARBY_DIS)
            {
                RandomPos(tempPos);
                ActSafeMove(tempPos);//移动这里会有卡位问题,稍后调整
            }
            else
            {
                //判断是否有空间
                bool bBagFree = SearchBagFreeSpace(trophy.Width, trophy.Height);
                if (!bBagFree)
                {
                    curStatus = Status.GotoBack;
                    UseTransDoor = true;
                    SetNeedGoBack();
                    return 1;
                }
                // 否则点击
                if (LastLootTarget == trophy.ObjPtr)
                {
                    LootBlockCount++;
                }
                else
                {
                    LastLootTarget = trophy.ObjPtr;
                    LootBlockCount = 0;
                }

                if (LootBlockCount > 10)
                {
                    LootFilter.Add(trophy.ObjPtr);
                    LootBlockCount = 0;
                }
                else
                {
                    Program.client.ActiveTarget(trophy.ObjPtr);
                    //清空记忆
                    ClearMemory();
                }
            }
            nSleepTime = 50;
            return 0;
            //几次就放弃
        }
        int ActHandLootTrophy(TrophyInfo trophy)
        {
            //远了走过去,
            GPoint tempPos = new GPoint();
            tempPos.x = (ushort)trophy.X;
            tempPos.y = (ushort)trophy.Y;
            double dis = CalcDis(player.Pos, trophy.X, trophy.Y);
            if (dis > NEARBY_DIS)
            {
                RandomPos(tempPos);
                ActSafeMove(tempPos);//移动这里会有卡位问题,稍后调整
            }
            else
            {
                //判断是否有空间
                bool bBagFree = SearchBagFreeSpace(trophy.Width, trophy.Height);
                if (!bBagFree)
                {
                    LootFilter.Add(trophy.ObjPtr);
                    System.Media.SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = @"bagfull.wav";
                    sp.Play();
                    sp.Dispose();
                    return 1;
                }
                // 否则点击
                if (LastLootTarget == trophy.ObjPtr)
                {
                    LootBlockCount++;
                }
                else
                {
                    LastLootTarget = trophy.ObjPtr;
                    LootBlockCount = 0;
                }

                if (LootBlockCount > 10)
                {
                    LootFilter.Add(trophy.ObjPtr);
                    LootBlockCount = 0;
                }
                else
                {
                    Program.client.ActiveTarget(trophy.ObjPtr);
                    //清空记忆
                    ClearMemory();
                }
            }
            nSleepTime = 50;
            return 0;
            //几次就放弃
        }
        int ActKillMonster(SMonsterInfo targetMonster, int targetMonsterRoundCount, int curTickCount)
        {
            double dis = CalcDis(player.Pos, targetMonster.Pos);
            //如果当前目标是上次目标,攻击5次且满血,放弃
            if (targetMonster.ObjPtr == LastTargetMonsterObjPtr)
            {
                if (targetMonster.HP == targetMonster.MaxHP)
                {
                    MonsterBlockCount++;
                }
                else if (targetMonster.HP == LastTargetMonsterHP)
                {
                    MonsterBlockCount++;
                }
                else
                    MonsterBlockCount = 0;
                LastTargetMonsterHP = targetMonster.HP;
            }
            else
            {
                LastTargetMonsterObjPtr = targetMonster.ObjPtr;
                MonsterBlockCount = 0;
            }
            if (MonsterBlockCount > 5)
            {
                FilterMonsterList.Add(targetMonster.Pos);
                MonsterBlockCount = 0;
            }

            //召唤技能
            if (0 == CastSummonerSkill(curTickCount))
            {
                return 0;
            }
            //图腾技能
            if (0 == CastTTSkill(curTickCount))
                return 0;

            //施放护盾技能
            if (0 == CastShieldSkill(curTickCount))
                return 0;

            //先判断目标是单体还是群体,
            if ((Program.config.nMulAttKey != -1) && targetMonsterRoundCount > Program.config.MultiCount && (player.MP > 20))//如果有群攻技能,且目标是群体
            {
                MultiAttack(targetMonster.Pos, dis);
            }
            else//使用单体技能
            {
                SingleAttack(targetMonster.Pos, dis, targetMonster.ID);
            }
            return 0;
        }
        int ActKillMonsterNew(SMonsterInfo targetMonster, int targetMonsterRoundCount, List<SMonsterInfo> NearbyMonster,int curTickCount)
        {
            double dis = CalcDis(player.Pos, targetMonster.Pos);
            //如果当前目标是上次目标,攻击5次且满血,放弃
            if (targetMonster.ObjPtr == LastTargetMonsterObjPtr)
            {
                if (targetMonster.HP == targetMonster.MaxHP)
                {
                    MonsterBlockCount++;
                }
                else if (targetMonster.HP == LastTargetMonsterHP)
                {
                    MonsterBlockCount++;
                }
                else
                    MonsterBlockCount = 0;
                LastTargetMonsterHP = targetMonster.HP;
            }
            else
            {
                LastTargetMonsterObjPtr = targetMonster.ObjPtr;
                MonsterBlockCount = 0;
            }

            if (MonsterBlockCount > 5)
                FilterMonsterList.Add(targetMonster.Pos);

            //召唤技能
            if (0== CastSummonerSkill(curTickCount))
            {
                return 0;
            }
            //图腾技能
            if (0 == CastTTSkill(curTickCount))
                return 0;

            //施放护盾技能
            if (0 == CastShieldSkill(curTickCount))
                return 0;

            //先判断目标是单体还是群体,
            if ((Program.config.nMulAttKey != -1) && targetMonsterRoundCount > Program.config.MultiCount && (player.MP > 20))//如果有群攻技能,且目标是群体
            {
                //如果距离远,走过去
                if (dis > Program.config.MulAttDis)
                {
                    //如果阻塞,干身边的怪
                    if (CalcDis(AttMonsterPos, player.Pos) < 8.0)
                    {
                        //如果身边有怪,则干身边的怪,这里有可能被箱子卡住
                        //没有,则放弃
                        if(NearbyMonster.Count>0)
                        {
                            AttackNearbyMonster(NearbyMonster);
                        }
                        else
                        {
                            FilterMonsterList.Add(targetMonster.Pos);
                        }
                        return 0;
                    }
                    //否则走过去
                    ActSafeMove(targetMonster.Pos);
                    AttMovePos.x = player.Pos.x;
                    AttMovePos.y = player.Pos.y;
                }
                else
                    MultiAttackNew(targetMonster.Pos);
            }
            else//使用单体技能
            {
                if (dis > Program.config.SinAttDis)
                {
                    //如果阻塞,干身边的怪
                    if (CalcDis(AttMonsterPos, player.Pos) < 8.0)
                    {
                        //如果身边有怪,则干身边的怪,这里有可能被箱子卡住
                        //没有,则放弃
                        if (NearbyMonster.Count > 0)
                        {
                            AttackNearbyMonster(NearbyMonster);
                        }
                        else
                        {
                            FilterMonsterList.Add(targetMonster.Pos);
                        }
                        return 0;
                    }
                    //否则走过去
                    ActSafeMove(targetMonster.Pos);
                    AttMovePos.x = player.Pos.x;
                    AttMovePos.y = player.Pos.y;
                }
                else
                    SingleAttackNew(targetMonster.Pos, targetMonster.ID);
            }
            return 0;
        }
        int ActEnterPollutant()
        {
            double dis=CalcDis(player.Pos, pollutantGatePos);
            if(dis>16.5)//移动
            {
                ActSafeMove(pollutantGatePos);
             //   MoveToPoint(pollutantGatePos);
                return 0;
            }
            int nGateObjPtr = Program.client.GetNearbyPollutantGateObjPtr();
            Program.client.ActiveTarget(nGateObjPtr);
            Thread.Sleep(3000);
            //进入,等待获取ID
            bEnterPollutanting = true;
            bNeedCastBattleOnceSkill = true;

            bNeedSearchOutPollutantGate = true;
            return 0;
        }

        //使用回城卷回城或者小退回城
        int ActGotoTown()
        {

            if (UseBagTransDoor())
            {
                Thread.Sleep(1000);
                int nObj = Program.client.GetNearbyGoCityTransferDoorObjPtr();
                if (nObj != 0)
                {
                    Thread.Sleep(1000);
                    Program.client.ActiveTarget(nObj);
                    Thread.Sleep(1000 * 2);
                    return 0;
                }
            }
            ReturnRolePolicy();//小退回城
            return 0;
        }
    }


    //class Action
    //{
    //}
    //public class Actor
    //{
    //    public virtual int DoAction(){return 0;}
    //}
}
