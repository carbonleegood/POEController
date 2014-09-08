using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    //基类
    class Analyser
    {
        public virtual ActorCode DoWork() { return ActorCode.NoAction; }
    }
    //前3个用于状态切换
    class IsDead : Analyser
    {
        public override ActorCode DoWork() { return ActorCode.NoAction; }
    }
    class IsOffLine : Analyser
    {
        public override ActorCode DoWork() { return ActorCode.NoAction; }
    }
    class IsNeedGotoCity :Analyser
    {
        public override ActorCode DoWork() { return ActorCode.NoAction; }
    }
    //掉线状态///////////////////////////////////////////////////////////////////////////////////////////

    //死亡状态///////////////////////////////////////////////////////////////////////////////////////////

    //回城状态///////////////////////////////////////////////////////////////////////////////////////////

    //战斗状态///////////////////////////////////////////////////////////////////////////////////////////
    class KillingAnalysis : Analyser
    {
        public override ActorCode DoWork()
        {
            //如果需要喝红

            //喝蓝

            //放技能

            //拾取

            //移动至最近的怪

            //探索地图
            return ActorCode.Move;
        }
    }
    
}
