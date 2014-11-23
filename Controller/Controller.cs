using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Thrift.GameCall;
using System.Runtime.Serialization.Formatters.Binary;

//using System.Speech.Synthesis; //用于生成响应的事件
//using System.Speech;
//using System.Speech.Recognition;

using System.Media;

namespace Controller
{
    public partial class Controller : Form
    {
        Worker worker = new Worker();
        public bool bWorking=false;
        public string AutoConfig=null;
        public Controller()
        {
            InitializeComponent();
        }
        void StartClick()
        {
            if (bWorking)
                return;
            bWorking = true;
            // listBox1.Items.Add("gg");
            //加载配置文件,运行时文件
            if (Program.config == null)
            {
                string filename = "Config\\" + cbConfig.Text;
                if (filename.Length < 1)
                {
                    MessageBox.Show("請選擇一個配置方案,首次使用,點擊設定配置以創建新配置方案");
                    return;
                }
                Stream fStream = null;
                try
                {
                    fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    BinaryFormatter binFormat = new BinaryFormatter();
                    Program.config = (ConfigData)binFormat.Deserialize(fStream);
                    fStream.Close();

                    //设置运行时配置,全局配置在程序载入时即加载进内存
                    lbMap.Items.Clear();
                    foreach (var item in Program.config.MissionMapList)
                    {
                        lbMap.Items.Add(item);
                    }
                    List<LootType> lootTypeList = new List<LootType>();
                    foreach (var item in Program.config.LootTypeList)
                    {
                        LootType newItem = new LootType();
                        newItem.Type = item.Key;
                        newItem.Color = item.Value;
                        lootTypeList.Add(newItem);
                    }
                    Program.client.SetLootTypeList(
                        lootTypeList,
                        Program.config.LootSocketFilter,
                        Program.config.LootSocketConnectFilter,
                        Program.config.LootThreeColor,
                        Program.config.LootSkillQuality);

                    Program.runtime = new RunTimeData();
                    lbMap.SelectedIndex = 0;
                    Program.runtime.reset();
                    lbNotice.Text = "使用中配置:" + cbConfig.Text + ",如果您修改了該配置方案或選擇了其他配置方案,請點擊重載配置.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("載入配置文件失敗,文件不存在或已損壞,請重新創建");
                    return;
                }
            }
            else
                Program.runtime.curMissionMapIndex = lbMap.SelectedIndex;
            if (cbHandModel.Checked)
                worker.bHandModel = true;
            else
                worker.bHandModel = false;
            Program.client.ReloadPollutantGateName();//重载门名称列表
            worker.UI = this;
            worker.begin();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartClick();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            worker.stop();
            bWorking = false;
            //listBox1.SelectedItem = listBox1.Items[0];
            //int n=listBox1.SelectedIndex;
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            if (cbConfig.Text.Length < 4)
            {
                FormCFGName cfgName = new FormCFGName();
                DialogResult ret=cfgName.ShowDialog();
                if (ret != DialogResult.OK)
                    return;
                cbConfig.Items.Add(cfgName.CFGName);
                cbConfig.Text = cfgName.CFGName;
            }
            if (0 == setting.LoadData(cbConfig.Text))
                setting.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bWorking)
            {
                MessageBox.Show("程序運行中,無法重載配置");
                return;
            }
            string filename = "Config\\"+cbConfig.Text;
            if (filename.Length < 1)
            {
                MessageBox.Show("請選擇一個配置方案,首次使用,點擊設定配置以創建新配置方案");
                return;
            }
            Stream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryFormatter binFormat = new BinaryFormatter();
                Program.config = (ConfigData)binFormat.Deserialize(fStream);
                fStream.Close();

                //设置运行时配置,全局配置在程序载入时即加载进内存
                lbMap.Items.Clear();
                foreach (var item in Program.config.MissionMapList)
                {
                    lbMap.Items.Add(item);
                }

                List<LootType> lootTypeList = new List<LootType>();
                foreach (var item in Program.config.LootTypeList)
                {
                    LootType newItem = new LootType();
                    newItem.Type = item.Key;
                    newItem.Color = item.Value;
                    lootTypeList.Add(newItem);
                }
               // Program.client.SetLootTypeList(lootTypeList);
                Program.client.SetLootTypeList(
                       lootTypeList,
                       Program.config.LootSocketFilter,
                       Program.config.LootSocketConnectFilter,
                       Program.config.LootThreeColor,
                       Program.config.LootSkillQuality);

                Program.runtime = new RunTimeData();
                lbMap.SelectedIndex = 0;
             //   Program.runtime.curMissionMapIndex = 0;
                lbNotice.Text = "使用中配置:" + cbConfig.Text + ",如果您修改了該配置方案或選擇了其他配置方案,請點擊重載配置.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("載入配置文件失敗,文件不存在或已損壞,請重新創建");
                return;
            }
        }

        private void cbConfig_DropDown(object sender, EventArgs e)
        {
            cbConfig.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(Environment.CurrentDirectory + "\\Config\\");
            foreach (FileInfo nextFile in theFolder.GetFiles())
            {
             //   if (nextFile.Extension == ".cfg")
                    cbConfig.Items.Add(nextFile.Name);
            }
        }
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "FindWindowA")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImportAttribute("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam          //参数2
        );

        int WM_RobotExist = 1224;
        public bool bNeedAutoStart = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            IntPtr hWnd = FindWindow(null, "POEHelpMonite");
            if (hWnd != null)
                PostMessage(hWnd, WM_RobotExist, 0, 0);

            if (bWorking == false)
            {
                strSpeekText = null;
                if (AutoConfig != null&&bNeedAutoStart)
                {
                    cbConfig.Items.Clear();
                    cbConfig.Items.Add(AutoConfig);
                    cbConfig.SelectedIndex = 0;
                    StartClick();
                    bNeedAutoStart = false;
                }
                return;
            }
            else
            {
                if(strSpeekText!=null)
                {
                    System.Media.SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = @"explore.wav";
                    sp.Play();
                    sp.Dispose();
                }
            }
            if (Program.config == null)
                return;
           if( lbMap.Items.Count!=Program.config.MissionMapList.Count)
           {
               lbMap.Items.Clear();
               foreach (var item in Program.config.MissionMapList)
               {
                   lbMap.Items.Add(item);
               }
           }
           lbMap.SelectedIndex = Program.runtime.curMissionMapIndex;
        }
       // SpeechSynthesizer synth = new SpeechSynthesizer();
        string strSpeekText=null;
        public void Speak(string strText)
        {
            strSpeekText = strText;
            //SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.Speak(strText);
            //synth.Dispose();
        }
        //public void SpeakOnce(string strText)
        //{
        //    SpeechSynthesizer synth = new SpeechSynthesizer();
        //    synth.Speak(strText);
        //    synth.Dispose();
        //}
        private void button2_Click(object sender, EventArgs e)
        {
            //cbConfig.Items.Clear();
            //cbConfig.Items.Add("aaa");
            //cbConfig.SelectedIndex = 0;

            //IntPtr hWnd = FindWindow(null, "POEHelpMonite");
            //if (hWnd != IntPtr.Zero)
            //    PostMessage(hWnd, WM_RobotExist, 0, 0);
            //SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.Speak("explore complete");
            //synth.Dispose();
          //  System.Media.SystemSounds.Beep.Play();
          //  System.Media.SystemSounds.Question.Play();
           // for (int i = 0; i < 10; ++i)
          //  {
          //      System.Media.SystemSounds.Asterisk.Play();
         //   }

            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @"explore.wav";
            sp.Play();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Program.client.LogGateName();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.client.ReloadPollutantGateName();
        }
       
    }
}
