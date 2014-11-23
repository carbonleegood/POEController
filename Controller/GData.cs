using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Thrift.GameCall;


namespace Controller
{
    [Serializable]
    public class TTSkill
    {
        public int Key;
        public int Step=10;
        public int Count=1;//每次释放数量
    }
    [Serializable]
    public class ShieldSkill//护盾
    {
        public int Key;
        public int Step=10;
    }
    [Serializable]
    public class SummonerSkill
    {
        public int Key;
        public int Step = 10;
        public int Count = 1;//每次释放数量
        public bool NeedCorpse = false;
    }
     [Serializable]
    public class TrapSkill
    {
        public int Key;
        public int Step = 5;
        public List<int> CastTime = new List<int>();
    }
     [Serializable]
    class HPKey
    {
        public int Key;
        public int HP;
        public int Step = 7000;
    }

     [Serializable]
    class MPKey
    {
        public int Key;
        public int MP;
        public int Step=7000;
    }

     [Serializable]
    class ShieldKey
    {
        public int Key;
        public int Shield;
        public int Step = 7000;
    }

     [Serializable]
    class SpeedKey
    {
        public int Key;
        public int Step = 5000;
    }

    [Serializable]
     class FlaskInfo
    {
        public bool Use = false;
        public int Type = 3;
        public int CalcData = 0;
        public List<int> KeyList = new List<int>();
        public int Step = 5;

    }
    [Serializable]
    class ConfigData
    {
        public List<string> MissionMapList = new List<string>();//需要刷的地图列表
        //技能
        public float NorAttDis = 35.0F;
        public int NorAttStep = 700;
        public int nNorAttKey = -1;

    //    public bool bUseSinAtt=true;
        public float SinAttDis = 35.0F;
        public int SinAttStep = 700;
        public int nSinAttKey = -1;

     //   public bool bUseMulAtt=true;
        public float MulAttDis = 35.0F;
        public int MulAttStep = 700;
        public int MultiCount = 2;
        public int nMulAttKey = -1;
        //技能
        public bool bAutoUpSkill=false;

        public int nGobackSkill = -1;
       
        public List<int> haloSkill = new List<int>();
        public List<TTSkill> ttSkill = new List<TTSkill>();
        public List<ShieldSkill> shieldSkill = new List<ShieldSkill>();
        public List<int> battleOnceSkill = new List<int>();
        public List<SummonerSkill> summonerSkill=new List<SummonerSkill>();//召唤
        public List<TrapSkill> singleTrapSkill = new List<TrapSkill>();
        public List<TrapSkill> multiTrapSkill = new List<TrapSkill>();


        //藥劑
        public FlaskInfo Flask1 = new FlaskInfo();
        public FlaskInfo Flask2 = new FlaskInfo();
        public FlaskInfo Flask3 = new FlaskInfo();
        public FlaskInfo Flask4 = new FlaskInfo();
        public FlaskInfo Flask5 = new FlaskInfo();


        public SortedList<int, int> SellTypeList = new SortedList<int, int>();//售卖类型列表
        public SortedList<int, int> LootTypeList = new SortedList<int, int>();//拾取类型列表
        public SortedList<int, int> SaveTypeList = new SortedList<int, int>();//存仓列表 类型,颜色

        public int BoxFilterColor = 1;

        public bool LootThreeColor = true;
        public short LootSocketFilter = 6;
        public short LootSocketConnectFilter = 5;
        public short LootSkillQuality = 0;

        public bool SellThreeColor = true;
        public short SellSocketFilter = 5;
        public short SellSocketConnectFilter = 4;

        public bool SaveThreeColor = true;
        public short SaveSocketFilter = 6;
        public short SaveSocketConnectFilter = 5;


        public bool bDungeonModel = false;
        public bool bBattleRelive = false;

        public int LogoutType = 0;//生命值
        public int LogOutData = 100;

        public bool bNeedIdentity = true;

        public bool bTrimStorage=true;
        public bool bNoIdentifyRing = false;
        public bool bNoIdentifyAmulet = false;
        public bool bNoIdentifyBelt = false;

        public bool bExplorePollutant = true;
        public bool bPriorityAttack = false;
    }
    class GlobeData//持久数据,地图,任务等使用
    {
        //全部地图数据
        public Dictionary<string, BattleMapInfo> AllBattleMap=new Dictionary<string,BattleMapInfo>();
        public Dictionary<int, TownMapInfo> AllTownMap=new Dictionary<int,TownMapInfo>();
        public HashSet<int> AllTownMapID = new HashSet<int>();
        public Dictionary<string, int> LootName = new Dictionary<string, int>();
        //GlobeData()
        //{
        //    //持久化读取数据
        //    for (int i = 0; i < 10;++i )
        //    {
        //        BattleMapInfo temp = null;
        //        string Key = null;
        //        switch (temp.Level)
        //        {
        //            case 1:
        //                Key = temp.strMapName + "*一般";
        //                break;
        //            case 2:
        //                Key = temp.strMapName + "*殘酷";
        //                break;
        //            case 3:
        //                Key = temp.strMapName + "*無情";
        //                break;
        //        }
        //        AllBattleMap.Add(Key, temp);
        //    }
        //}
    }
    class RunTimeData
    {
        public int curMissionMapIndex=0;
        public int reset()
        {
            curMissionMapIndex = 0;
            return 0;
        }
    }
    enum ActorCode
    {
        NoAction,
        ChangeStatus,
        Login,//登录
        ChangeRole,//小退,换角色

        //复活

        //回城
        GotoCity,//回城路上
        Repair,//修理
        Sale,//售卖
        //存仓

        //战场
        GoBattle,//去战场
        Rest,//休息
        KillMonstor,//杀怪
        Move,//走路
    };
    enum Status
    {
        Login,//登录中
        Resurrection,//复活中
        GotoBack,//回城中
        Mission,//任务中
    }
    [Serializable]
    class GPoint
    {
        public UInt16 x=0;
        public UInt16 y=0;

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            GPoint p = obj as GPoint;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(GPoint p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == p.x) && (y == p.y);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
        public static bool operator ==(GPoint a,GPoint b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.x == b.x && a.y == b.y ;
            
        }
        public static bool operator != (GPoint g1,GPoint g2)
        {
            return !(g1==g2);
        }
    }
    [Serializable]
    class BattleMapInfo
    {
        public string strMapName = null;
        public int MapID = 0;
        public int Group = 0;//章节
        public short Level = 1;//难度,分3等
        public short Block = 0;//阻塞指数
        public short Type = 0;//城镇,传送点可达战场,传送点非可达
        //public GPoint WaypointPos;
        //public GPoint SellNPCPos;
        //public GPoint TransferPos;//传送门位置
        //GPoint //仓库位置
        //可达其他地图列表
    }
    [Serializable]
    class TownMapInfo
    {
        public string strMapName = null;
        public int MapID = 0;
        //    public short Type;//城镇,传送点可达战场,传送点非可达
        public short Level;//难度,分3等
        public short NPCNum;
        public short NPCSellMenu;
        public GPoint WaypointPos = new GPoint();
        public GPoint SellNPCPos = new GPoint();
        public GPoint TransferPos = new GPoint();//传送门位置
        public GPoint StoragePos = new GPoint();//仓库位置
        //GPoint //仓库位置
    }
    class StorageSpace
    {
        public int StackObjPtr = 0;
        public short Left;
        public short Top;
        public int ContainerObjPtr;
    }
    class SPlayerInfo
    {
        public int ObjPtr;
        public int ID;
        public int EnemyID;
        public int HP;
        public int MaxHP;
        public int MP;
        public int MaxMP;
        public int Shield;
        public int Level;
        public GPoint Pos = new GPoint();
        public string Name;
    }
    class SMonsterInfo
    {
        public int ObjPtr=0;
        public int ID=0;
        public int EnemyID = 0;
        public int HP = 0;
        public int MaxHP = 0;
        public int Priority = 0;//优先攻击策略
        public GPoint Pos = new GPoint();
   //     public string Name=null; 
        public SMonsterInfo(ObjInfo item)
        {
            this.ObjPtr = item.ObjPtr;
            this.ID = item.ID;
            this.EnemyID = item.EnemyID;
            this.HP = item.HP;
            this.MaxHP = item.MaxHP;
            this.Priority = item.Color;
            this.Pos.x = (ushort)item.X;
            this.Pos.y = (ushort)item.Y;
        }
    }
    //class STrophy
    //{
    //    public int ObjPtr;
    //    public int ID;
    //    public short Color;
    //    public short Type;
    //    public short Width;
    //    public short Height;
    //    public GPoint Pos=new GPoint();
    //    public STrophy(ObjInfo item)
    //    {
    //        this.ObjPtr = item.ObjPtr;
    //        this.ID = item.ID;
    //        this.Pos.x = (ushort)item.X;
    //        this.Pos.y = (ushort)item.Y;
    //    }
    //}
    class AreaCrossInfo
    {
        public int ObjPtr;
        public int ID;
        public GPoint Pos = new GPoint();
        public AreaCrossInfo(ObjInfo item)
        {
            this.ObjPtr = item.ObjPtr;
            this.ID = item.ID;
            this.Pos.x = (ushort)item.X;
            this.Pos.y = (ushort)item.Y;
        }
    }
    class SDoor
    {
        public int ObjPtr;
        public int ID;
        public GPoint Pos = new GPoint();
        public SDoor(ObjInfo item)
        {
            this.ObjPtr = item.ObjPtr;
            this.ID = item.ID;
            this.Pos.x = (ushort)item.X;
            this.Pos.y = (ushort)item.Y;
        }
    }
    class SShrine
    {
        public int ObjPtr;
        public int ID;
        public GPoint Pos = new GPoint();
        public SShrine(ObjInfo item)
        {
            this.ObjPtr = item.ObjPtr;
            this.ID = item.ID;
            this.Pos.x = (ushort)item.X;
            this.Pos.y = (ushort)item.Y;
        }
    }
    class SBox
    {
        public int ObjPtr;
        public int ID;
        public int Color;
        public GPoint Pos = new GPoint();
        public SBox(ObjInfo item)
        {
            this.ObjPtr = item.ObjPtr;
            this.ID = item.ID;
            this.Color = item.Color;
            this.Pos.x = (ushort)item.X;
            this.Pos.y = (ushort)item.Y;
        }
        public SBox()
        {
            this.ObjPtr = 0;
            this.ID = 0;
            this.Pos.x = 0;
            this.Pos.y = 0;
        }
    }
    public partial class Worker
    {
        bool bWorking = false;
        int nSleepTime=200;
        double attackDis = 13;//40

        ///
        int nDrinkHP = 0;
        int nDrinkDirectHP = 0;
        int nDrinkMP = 0;
        int nDrinkDirectMP = 0;
        int nDrinkSpeed = 0;

        int[] LastHitKeyTime = new int[6];
        //int LastUseHPTime = 0;
        //int LastUseDirHPTime = 0;
        //int LastUseMPTime = 0;
        //int LastUseDirMPTime = 0;
        //int LastUseSpeedTime = 0;
       
        //检测卡位用的
        int blockCount = 0;
        GPoint lastTargetPoint = new GPoint();
        GPoint lastPlayerPos = new GPoint();

        GPoint AttMovePos = new GPoint();
        //SortedList<ActorCode, Actor> AllActor = null;
        //ActorCode actCode;
        Status curStatus=Status.Mission;

        List<ObjInfo> round = null;

        GPoint TargetPoint = new GPoint();
   
        GPoint AttMonsterPos = new GPoint();

        SBox TargetBox = new SBox();
     //   int NeedActiveDoorObjPtr = 0;
        SPlayerInfo player = new SPlayerInfo();
        List<SMonsterInfo> monsterList = new List<SMonsterInfo>();
        List<TrophyInfo> trophyList = new List<TrophyInfo>();
        List<SDoor> doorList = new List<SDoor>();//门
        List<SBox> boxList = new List<SBox>();//桶,宝箱
        HashSet<int> ClickedBoxList = new HashSet<int>();
        HashSet<GPoint> PassedDoor = new HashSet<GPoint>();
        GPoint NearbyPollutantGatePos = null;

        List<AreaCrossInfo> areaCrossList = new List<AreaCrossInfo>();
     

        List<TrophyBaseInfo> trophyIDList = new List<TrophyBaseInfo>();

        HashSet<GPoint> FilterMonsterList = new HashSet<GPoint>();
        const int MONSTER_TYPE = 1;
        const int TROPHY_TYPE = 2;
        const double NEARBY_DIS = 15.0;

        int MonsterBlockCount = 0;
        int LastTargetMonsterObjPtr = 0;
        int LastTargetMonsterHP = 0;

        int LastLootTarget = 0;
        int LootBlockCount = 0;
        HashSet<int> LootFilter = new HashSet<int>();
        //当前地图传送点

      //  bool bNeedGoback = false;
        bool UseTransDoor = false;
        bool bNeedResetBattleMapID = false;
        bool bNeedSell = false;
        bool Selling = false;
        bool Saveing = false;
        bool bNeedSave = false;
        bool bNeedIdentity = false;


        int CurMapID = 0;
     //   int TargetMissionMapID = 0;
        TownMapInfo CurTownMap = null;
        BattleMapInfo CurMissionMap = null;
        int LoadTownMapID = 0;
        int LoadBattleMapID = 0;

        public int LastLeftSkillCastTime = 0;
        public int LastMidSkillCastTime = 0;
        public int LastRightSkillCastTime = 0;
        public int LastQSkillCastTime = 0;
        public int LastWSkillCastTime = 0;
        public int LastESkillCastTime = 0;
        public int LastRSkillCastTime = 0;
        public int LastTSkillCastTime = 0;
  //      bool bNeedCastHaloSkill = true;
        bool bNeedCastBattleOnceSkill = true;
    //    SortedList<int, int> SellTypeList = new SortedList<int, int>();
        //short AttX;
        //short AttY;
        //bool bAttacking = false;
        List<CrossPoint> allCrossPoint = new List<CrossPoint>();//全部穿越点
        HashSet<GPoint> AddedCrossPoint = new HashSet<GPoint>();//加过的穿越点

        
        CrossInfo tempCrossInfo = new CrossInfo();
        CrossInfo endCrossInfo = new CrossInfo();
        bool bGroupExploreModel = false;//判断
        bool bNeedCross = false;//判断探索还是穿越
        bool bCrossing = false;//判断穿越中还是移动到穿越点
        int curGroup = -1;


        GPoint FarBlockPoint = new GPoint();
        int FarBlockPointCount = 0;

        //贫血小退
        int lastReturnTime = 0;
        int LogoutCount = 0;


        //最后检测光环时间
        int LastCheckHaloTime=0;
        int LastInTownTime = 0;//最后在城里的时间

        //那个啥,污染地穴
       // int BaseMapID;//所在地图ID
        int pollutantMapID;//污染地穴地图ID
        int LoadPollutantMapID=0;
        GPoint pollutantGatePos=null;//门位置
        bool pollutantComplete=false;//已经刷完
        bool bEnterPollutanting = false;//正在进入污染地穴

        
    }

    class CrossPoint
    {
        public int firstGroup = -1;
        public GPoint firstPos = new GPoint();
        public int secondGroup = -1;
        public GPoint secondPos = new GPoint();
    }
    class CrossInfo
    {
        public int beginGroup = -1;
        public GPoint beginPos = new GPoint();
        public int endGroup = -1;
        public GPoint endPos = new GPoint();
        public int ListNum=-1;
    }
    class TrimInfo
    {
        public int StoragePtr;
        public int TargetServiceID;
        public int SourceServiceID;
        public short SourceLeft;
        public short SourceTop;
    }
}
