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
        [STAThread]
        static void Main()
        {
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
                login.bAutoLogin=true;

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
        }//
        //static void monite()
        //{
        //    if(UI!=null)
        //    {
        //        UI.cbConfig.Items.Clear();
        //        UI.cbConfig.Items.Add("aaa");
        //        UI.cbConfig.SelectedIndex = 0;
        //    }
        //}
        static void bbb()
        {
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
    }
}
