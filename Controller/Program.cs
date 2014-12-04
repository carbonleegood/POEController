using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.GameCall;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Controller;
namespace Controller
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        public static TTransport transport = null;
        public static TProtocol protocol = null;
        public static GameFuncCall.Client client = null;
        public static ConfigData config = null;
        public static RunTimeData runtime = null;
        public static GlobeData gdata = new GlobeData();
        public static Controller UI = null;
        public static HashSet<char> NumberChar = new HashSet<char>();
        [STAThread]
        static void Main()
        {
            NumberChar.Add('.');
            NumberChar.Add('0');
            NumberChar.Add('1');
            NumberChar.Add('2');
            NumberChar.Add('3');
            NumberChar.Add('4');
            NumberChar.Add('5');
            NumberChar.Add('6');
            NumberChar.Add('7');
            NumberChar.Add('8');
            NumberChar.Add('9');

          //  String commandLineString = System.Environment.CommandLine;
            String[] args = System.Environment.GetCommandLineArgs();
            string StartParam=null;
            int n = args.Length;
      //      MessageBox.Show(commandLineString);
            if(n>1)
            {
                StartParam = args[1];
            }
            
          //  aaa();
          //  return;
            
           bbb();

           Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           //载入地图数据
           Login login = new Login();

           if (StartParam != null)
               login.bAutoLogin = true;

           DialogResult ret = login.ShowDialog();
           if (ret == DialogResult.Cancel)
               return;
          //  Application.Run(new Trance());
        //  Application.Run(new FormControl());
           UI = new Controller();
           if (StartParam != null)
           {
               UI.bNeedAutoStart = true;
               UI.AutoConfig = StartParam;
           }
          Application.Run(UI);
        }


        static void bbb()
        {
            Program.gdata.AllDungeonMapID.Add(0x9225);
            Program.gdata.AllDungeonMapID.Add(0x70C6);
            Program.gdata.AllDungeonMapID.Add(0xCA19);
            Program.gdata.AllDungeonMapID.Add(0xD747);
            Program.gdata.AllDungeonMapID.Add(0xC11F);
            Program.gdata.AllDungeonMapID.Add(0xDD87);
            Program.gdata.AllDungeonMapID.Add(0xEF29);
            Program.gdata.AllDungeonMapID.Add(0x7441);
            Program.gdata.AllDungeonMapID.Add(0xB593);
            Program.gdata.AllDungeonMapID.Add(0x93F5);
            Program.gdata.AllDungeonMapID.Add(0xF17F);
            Program.gdata.AllDungeonMapID.Add(0x8F82);
            Program.gdata.AllDungeonMapID.Add(0x1420);
            Program.gdata.AllDungeonMapID.Add(0x43F7);
            Program.gdata.AllDungeonMapID.Add(0xB469);
            Program.gdata.AllDungeonMapID.Add(0x9155);
            Program.gdata.AllDungeonMapID.Add(0x8F4);
            Program.gdata.AllDungeonMapID.Add(0x43B4);
            Program.gdata.AllDungeonMapID.Add(0xE5C);
            Program.gdata.AllDungeonMapID.Add(0x3BA1);
            Program.gdata.AllDungeonMapID.Add(0x296B);
            Program.gdata.AllDungeonMapID.Add(0xAD52);
            Program.gdata.AllDungeonMapID.Add(0xCB66);
            Program.gdata.AllDungeonMapID.Add(0x1920);
            Program.gdata.AllDungeonMapID.Add(0xDBD9);
            Program.gdata.AllDungeonMapID.Add(0x6BC9);
            Program.gdata.AllDungeonMapID.Add(0x4CE0);
            Program.gdata.AllDungeonMapID.Add(0x2259);
            Program.gdata.AllDungeonMapID.Add(0x6FED);
            Program.gdata.AllDungeonMapID.Add(0xA980);
            Program.gdata.AllDungeonMapID.Add(0xFF7);
            Program.gdata.AllDungeonMapID.Add(0x845);
            Program.gdata.AllDungeonMapID.Add(0xB470);
            Program.gdata.AllDungeonMapID.Add(0x2113);
            Program.gdata.AllDungeonMapID.Add(0x3D0F);
            Program.gdata.AllDungeonMapID.Add(0x6D7);
            Program.gdata.AllDungeonMapID.Add(0xE98A);
            Program.gdata.AllDungeonMapID.Add(0x3F68);
            Program.gdata.AllDungeonMapID.Add(0xCB99);
            Program.gdata.AllDungeonMapID.Add(0xD769);
            Program.gdata.AllDungeonMapID.Add(0xCE5B);
            Program.gdata.AllDungeonMapID.Add(0xBEDB);
            Program.gdata.AllDungeonMapID.Add(0xF9B4);
            Program.gdata.AllDungeonMapID.Add(0xA5B4);
            Program.gdata.AllDungeonMapID.Add(0x7C9D);
            Program.gdata.AllDungeonMapID.Add(0x637A);
            Program.gdata.AllDungeonMapID.Add(0xD353);
            Program.gdata.AllDungeonMapID.Add(0xCE7);
            Program.gdata.AllDungeonMapID.Add(0xF5EB);
            Program.gdata.AllDungeonMapID.Add(0x80C4);
            Program.gdata.AllDungeonMapID.Add(0x3D3C);
            Program.gdata.AllDungeonMapID.Add(0xCB4B);
            /////////////////////////////////////////////////////////////
            BattleMapInfo map = null;
         //   string filename = @"d:\llllllll.bat";
            string filename = @"llllllll.bat";
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception e)
            {
                return;
            }
            int n = 0;
            BinaryFormatter binFormat = new BinaryFormatter();
            while (true)
            {
                try
                {
                    map = (BattleMapInfo)binFormat.Deserialize(fStream);
                    string Key = null;
                    switch (map.Level)
                    {
                        case 1:
                            Key = map.strMapName + "*一般";
                            break;
                        case 2:
                            Key = map.strMapName + "*殘酷";
                            break;
                        case 3:
                            Key = map.strMapName + "*無情";
                            break;
                    }
                    try
                    {
                        gdata.AllBattleMap.Add(Key, map);
                    }
                    catch(Exception ee)
                    {
                    }
                }
                catch (Exception e)
                {
                    break;
                }
                ++n;
            }
       //     MessageBox.Show(n.ToString());
            fStream.Close();
            /////////////////////////////////////
            TownMapInfo townmap = null;
      //      string filenametown = @"d:\lllllllll.bat";
            string filenametown = @"lllllllll.bat";
            Stream ftownStream = null;
            try
            {
                ftownStream = new FileStream(filenametown, FileMode.Open, FileAccess.ReadWrite);
                
            }
            catch (Exception e)
            {
                return;
            }
            BinaryFormatter binTownFormat = new BinaryFormatter();
            while (true)
            {
                try
                {
                    townmap = (TownMapInfo)binTownFormat.Deserialize(ftownStream);
                    gdata.AllTownMap.Add(townmap.MapID,townmap);
                    gdata.AllTownMapID.Add(townmap.MapID);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            ftownStream.Close();
            ////////////////////////////////////////
        }
        static void ccc()
        {
            FileStream fs1 = new FileStream("d:\\GEN地图ID.txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs1);
 
            ////////////////////////////////////////
            FileStream fs = new FileStream("d:\\地图ID.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            int n = 0;
            while (true)
            {
                int Level = 0;
                string genstr = null;
                try
                {
                    string text = sr.ReadLine();
                    if (text.Length < 1)
                        break;

                    string[] ret = text.Split('|');

                    int id = Convert.ToInt32(ret[2], 16);
                    if (ret[0].Substring(0, 1) == "1")
                    {
                        Level = 1;
                        genstr = "一般," + ret[1] + "," + id.ToString();
                        Console.WriteLine("一般," + ret[1] + "," + id.ToString());
                    }
                    else if (ret[0].Substring(0, 1) == "2")
                    {
                        Level = 2;
                        genstr = "殘酷," + ret[1] + "," + id.ToString();
                        Console.WriteLine("殘酷," + ret[1] + "," + id.ToString());
                    }
                    else if (ret[0].Substring(0, 1) == "3")
                    {
                        Level = 3;
                        genstr = "無情," + ret[1] + "," + id.ToString();
                        Console.WriteLine("無情," + ret[1] + "," + id.ToString());
                    }

                    if (ret[1] == "獅眼守望")
                    {
                        
                        continue;
                    }
                    if (ret[1] == "森林營地")
                    {
                       
                        continue;
                    }
                    if (ret[1] == "薩恩營地")
                    {
                   
                        continue;
                    }
                    
                    
                    sw.WriteLine(genstr);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            MessageBox.Show(n.ToString());
            fs.Close();
            fs1.Close();
        //    fStream.Close();
         //   ftownStream.Close();
        }
        static void aaa()
        {
            HashSet<int> filter = new HashSet<int>();
            FileStream fsFilter = new FileStream("D:\\中文\\GEN地图ID.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader srFilter = new StreamReader(fsFilter);
            while (true)
            {
                try
                {
                    string text = srFilter.ReadLine();
                    if (text.Length < 1)
                        break;

                    string[] ret = text.Split(',');
                    int f=  Convert.ToInt32(ret[2], 10);
                    filter.Add(f);
                }
                catch(Exception e)
                {
                    break;
                }
            }
            fsFilter.Close();
            //创建新的导出文件
            BattleMapInfo map = new BattleMapInfo();
            map.Block = 0;
            map.Type = 0;
            //map.strMapName = "测试测试";
            string filename = @"llllllll.bat";
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
            }
            catch (Exception e)
            {
                return;
            }
            BinaryFormatter binFormat = new BinaryFormatter();
            /////////////////////////////////////
            TownMapInfo townmap = new TownMapInfo();
            string filenametown = @"lllllllll.bat";
            Stream ftownStream = null;
            try
            {
                ftownStream = new FileStream(filenametown, FileMode.Create, FileAccess.ReadWrite);
            }
            catch (Exception e)
            {
                return;
            }
            BinaryFormatter binTownFormat = new BinaryFormatter();
            ////////////////////////////////////////
            FileStream fs = new FileStream("d:\\所有地图ID.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            int n = 0;
            while (true)
            {
                try
                {
                    string text = sr.ReadLine();
                    if (text.Length < 1)
                        break;

                    string[] ret = text.Split('|');

                    int id = Convert.ToInt32(ret[2], 16);
                    string strGroup=null;
                    if(ret[0].Substring(2,1)=="1")
                    {
                        strGroup="第一章";
                        map.Group = 1;
                    }
                    else if(ret[0].Substring(2,1)=="2")
                    {
                        strGroup="第二章";
                        map.Group = 2;
                    }
                    else if(ret[0].Substring(2,1)=="3")
                    {
                        strGroup="第三章";
                        map.Group = 3;
                    }

                    if (ret[0].Substring(0, 1) == "1")
                    {
                        map.Level = 1;
                        Console.WriteLine("一般," + ret[1] + "," + id.ToString() + "," + strGroup);
                    }
                    else if (ret[0].Substring(0, 1) == "2")
                    {
                        map.Level = 2;
                        Console.WriteLine("殘酷," + ret[1] + "," + id.ToString() + "," + strGroup);
                    }
                    else if (ret[0].Substring(0, 1) == "3")
                    {
                        map.Level = 3;
                        Console.WriteLine("無情," + ret[1] + "," + id.ToString() + "," + strGroup);
                    }

                    if (ret[1] == "獅眼守望")
                    {
                        townmap.Level = map.Level;
                        townmap.MapID = id;
                        townmap.strMapName = ret[1];
                        townmap.NPCNum = 1;
                        townmap.NPCSellMenu=21;
                        townmap.SellNPCPos.x = 275;
                        townmap.SellNPCPos.y = 263;
                        townmap.WaypointPos.x = 186;
                        townmap.WaypointPos.y = 164;
                        townmap.TransferPos.x = 153;
                        townmap.TransferPos.y = 231;
                        townmap.StoragePos.x = 243;
                        townmap.StoragePos.y = 262;

                        binTownFormat.Serialize(ftownStream, townmap);
                        Console.WriteLine("主城:" + ret[1]);
                        continue;
                    }
                    if (ret[1] == "森林營地")
                    {
                        townmap.Level = map.Level;
                        townmap.MapID = id;
                        townmap.strMapName = ret[1];
                        townmap.NPCNum = 2;
                        townmap.NPCSellMenu=18;
                        townmap.SellNPCPos.x = 200;
                        townmap.SellNPCPos.y = 179;
                        townmap.WaypointPos.x = 188;
                        townmap.WaypointPos.y = 116;
                        townmap.TransferPos.x = 249;
                        townmap.TransferPos.y = 163;

                        townmap.StoragePos.x = 193;
                        townmap.StoragePos.y = 199;
                        binTownFormat.Serialize(ftownStream, townmap);
                        continue;
                    }
                    if (ret[1] == "薩恩營地")
                    {
                        townmap.Level = map.Level;
                        townmap.MapID = id;
                        townmap.strMapName = ret[1];
                        townmap.NPCNum = 3;
                        townmap.NPCSellMenu=50;
                        townmap.SellNPCPos.x = 281;
                        townmap.SellNPCPos.y = 357;
                        townmap.WaypointPos.x = 219;
                        townmap.WaypointPos.y = 211;
                        townmap.TransferPos.x = 248;
                        townmap.TransferPos.y = 220;

                        townmap.StoragePos.x = 211;
                        townmap.StoragePos.y = 307;
                        binTownFormat.Serialize(ftownStream, townmap);
                        continue;
                    }
                    if (false == filter.Contains(id))
                        continue;
                    //if (map.Level!=1)
                    //    Console.WriteLine("");
                    map.MapID = id;
                    map.strMapName = ret[1];
                    ++n;
                    binFormat.Serialize(fStream, map);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            MessageBox.Show(n.ToString());
            fs.Close();
            fStream.Close();
            ftownStream.Close();
        }

        //static void GenIndexFilter()
        //{
        //    IndexFilter.Clear();
        //    //建立索引过滤器
        //    foreach (var OneFilter in AllFilter)
        //    {
        //        foreach (var OneProperty in OneFilter.rules)
        //        {
        //            List<List<Property>> GroupFilter = null;
        //            bool bRet = IndexFilter.TryGetValue(OneProperty.strInfo, out GroupFilter);
        //            if (bRet)//如果已经有了
        //            {
        //                GroupFilter.Add(OneFilter.rules);
        //            }
        //            else
        //            {
        //                GroupFilter = new List<List<Property>>();
        //                GroupFilter.Add(OneFilter.rules);
        //                IndexFilter.Add(OneProperty.strInfo, GroupFilter);
        //            }
        //        }
        //    }
        //}
        //static void InitFilter()
        //{
        //    string filename = @"trophy.bat";
        //    Stream fStream = null;
        //    try
        //    {
        //        fStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);


        //        BinaryFormatter binFormat = new BinaryFormatter();

        //        AllFilter = (List<Filter>)binFormat.Deserialize(fStream);
        //    }
        //    catch (Exception e)
        //    {
        //        if (fStream != null)
        //            fStream.Close();
        //        return;
        //    }
        //    fStream.Close();
        //    //从文件读取过滤器
        //    GenIndexFilter();
        //}
        //static void AddFilter(string strName, string strProperty)
        //{
        //    //保存过滤器
        //    List<Property> data = GenOneFilter(strProperty);
        //    Filter filter = new Filter();
        //    filter.strName = strName;
        //    filter.rules = data;
        //    AllFilter.Add(filter);
        //}
        //static void RemoveFilter(string strName)
        //{
        //    bool bContinue = false;
        //    do
        //    {
        //        bContinue = false;
        //        foreach (var item in AllFilter)
        //        {
        //            if (item.strName == strName)
        //            {
        //                AllFilter.Remove(item);
        //                bContinue = true;
        //                break;
        //            }
        //        }
        //    }
        //    while (bContinue);
        //}
        //static void SaveAllFilter()
        //{
        //    string filename = @"trophy.bat";
        //    Stream fStream = null;
        //    try
        //    {
        //        fStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);

        //        BinaryFormatter binFormat = new BinaryFormatter();

        //        binFormat.Serialize(fStream, AllFilter);
        //    }
        //    catch (Exception e)
        //    {
        //        if (fStream != null)
        //            fStream.Close();
        //        return;
        //    }
        //    fStream.Close();
        //}
        //List<List<Property>> SearchFilter(Dictionary<string, List<double>> TrophyProperty)
        //{
        //    List<List<Property>> GroupFilter = null;
        //    foreach (var item in TrophyProperty)
        //    {
        //        bool bRet = IndexFilter.TryGetValue(item.Key, out GroupFilter);
        //        if (bRet)
        //            break;
        //        GroupFilter = null;
        //    }
        //    return GroupFilter;
        //}
        //bool CompareProperty(Dictionary<string, List<double>> TrophyProperty, List<Property> Filter)
        //{
        //    //判断过滤器中的每一条,是否都在目标中
        //    foreach (var FilterItem in Filter)
        //    {
        //        List<double> tempData = null;
        //        bool bRet = TrophyProperty.TryGetValue(FilterItem.strInfo, out tempData);
        //        if (bRet == false)
        //            return false;
        //        if (tempData.Count != FilterItem.data.Count)
        //            return false;
        //        for (int i = 0; i < tempData.Count; ++i)
        //        {
        //            if (tempData[i] < FilterItem.data[i])
        //                return false;
        //        }
        //    }
        //    return true;
        //}
        //bool IsFilterTrophy(string strProperty)
        //{
        //    Dictionary<string, List<double>> TrophyProperty = GenTrophyProperty(strProperty);
        //    List<List<Property>> GroupFilter = SearchFilter(TrophyProperty);
        //    if (GroupFilter == null)
        //        return false;
        //    foreach (var item in GroupFilter)
        //    {
        //        bool bRet = CompareProperty(TrophyProperty, item);
        //        if (bRet == true)
        //            return true;
        //    }
        //    return false;
        //}
    }
}
