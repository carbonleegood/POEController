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
using System.Runtime.Serialization.Formatters.Binary;
using Thrift.GameCall;
namespace Controller
{
    public partial class FormSetting : Form
    {
        string fileName;
        ConfigData data = null;
        public FormSetting()
        {
            InitializeComponent();
            rbLevel1.Checked = true;
        }
        const short byTrophy_SkillStone = 1;//技能石
        const short byTrophy_Currency = 2;//卷轴宝石
        const short byTrophy_Flask = 3;//血瓶
        const short byTrophy_Armour = 4;//装备
        const short byTrophy_Ring = 5;//戒指
        const short byTrophy_Amulet = 6;//项链
        const short byTrophy_Belt = 7;//腰带
        const short byTrophy_Weapon = 8;//武器
        const short byTrophy_Maps = 11;//地图
        const short byTrophy_Money = 12;//通货
        public int LoadData(string strFileName)
        {
            int ret = 0;

            fileName = "Config\\" + strFileName;
            try
            {
                Stream fStream = null;
                fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter binFormat = new BinaryFormatter();
                data = (ConfigData)binFormat.Deserialize(fStream);
                fStream.Close();
            }
            catch (Exception e)
            {
                data = new ConfigData();
                MessageBox.Show("载入配置文件失败,配置文件可能已损坏,载入默认配置");
            }

            tbSkillQuality.Enabled = false;
            cmLootMaps.Enabled = false;
            cmLootFlask.Enabled = false;
            cmLootArmour.Enabled = false;
            cmLootWeapon.Enabled = false;
            cmLootBelt.Enabled = false;
            cmLootRing.Enabled = false;
            cmLootAmulet.Enabled = false;

            cmSellMaps.Enabled = false;
            cmSellFlask.Enabled = false;
            cmSellArmour.Enabled = false;
            cmSellWeapon.Enabled = false;
            cmSellBelt.Enabled = false;
            cmSellRing.Enabled = false;
            cmSellAmulet.Enabled = false;

            cmSaveMaps.Enabled = false;
            cmSaveFlask.Enabled = false;
            cmSaveArmour.Enabled = false;
            cmSaveWeapon.Enabled = false;
            cmSaveBelt.Enabled = false;
            cmSaveRing.Enabled = false;
            cmSaveAmulet.Enabled = false;

            cbLeftSkill.Enabled = false;
            tbLeftAttStep.Enabled = false;
            tbLeftAttCnt.Enabled = false;
            cbMidSkill.Enabled = false;
            tbMidAttStep.Enabled = false;
            tbMidAttCnt.Enabled = false;
            cbRightSkill.Enabled = false;
            tbRightAttStep.Enabled = false;
            tbRightAttCnt.Enabled = false;
            cbQSkill.Enabled = false;
            tbQAttStep.Enabled = false;
            tbQAttCnt.Enabled = false;
            cbWSkill.Enabled = false;
            tbWAttStep.Enabled = false;
            tbWAttCnt.Enabled = false;
            cbESkill.Enabled = false;
            tbEAttStep.Enabled = false;
            tbEAttCnt.Enabled = false;
            cbRSkill.Enabled = false;
            tbRAttStep.Enabled = false;
            tbRAttCnt.Enabled = false;
            cbTSkill.Enabled = false;
            tbTAttStep.Enabled = false;
            tbTAttCnt.Enabled = false;

            cbType1.Enabled = false;
            tbType1.Enabled = false;
            tbStep1.Enabled = false;
            cbType2.Enabled = false;
            tbType2.Enabled = false;
            tbStep2.Enabled = false;
            cbType3.Enabled = false;
            tbType3.Enabled = false;
            tbStep3.Enabled = false;
            cbType4.Enabled = false;
            tbType4.Enabled = false;
            tbStep4.Enabled = false;
            cbType5.Enabled = false;
            tbType5.Enabled = false;
            tbStep5.Enabled = false;


            cbDungeonModel.Checked = data.bDungeonModel;
            cbReliveModel.Checked = data.bBattleRelive;
            cbExplorePollutant.Checked = data.bExplorePollutant;
            cbPriorityAttack.Checked = data.bPriorityAttack;
            cbUseHideHome.Checked = data.bDungeonHome;
            cbFullHide.Checked = data.bFullHide;
            foreach (var item in data.MissionMapList)
            {
                lbMissionMap.Items.Add(item);
            }

            //技能
            cbAutoUpSkill.Checked = data.bAutoUpSkill;

            cbDefaultAttSpeed.Checked=data.bUseSafeAttSpeed;

            tbNorAttDis.Text=data.NorAttDis.ToString();
            if (data.nNorAttKey != -1)
            {
                cbKeyLeft.Checked = true;
                cbLeftSkill.SelectedIndex = 0;
                tbLeftAttStep.Text = data.NorAttStep.ToString();
            }

            if(data.nGobackSkill != -1)
            {
                cbKeyMid.Checked = true;
                cbMidSkill.SelectedIndex = 0;
            }

            if (data.MoveSkillKey != -1)
            {
                cbKeyRight.Checked = true;
                cbRightSkill.SelectedIndex = 0;
                tbRightAttStep.Text = data.MoveSkillStep.ToString();
            }

            tbSinAttDis.Text=data.SinAttDis.ToString();
            if (data.nSinAttKey != -1)
            {
                cbKeyQ.Checked = true;
                cbQSkill.SelectedIndex = 0;
                tbQAttStep.Text = data.SinAttStep.ToString();
            }
            
            

            tbMulAttDis.Text=data.MulAttDis.ToString();
            if(data.nMulAttKey!=-1)
            {
                cbKeyW.Checked = true;
                cbWSkill.SelectedIndex = 0;
                tbWAttStep.Text = data.MulAttStep.ToString();
            }


            tbMultiCount.Text= data.MultiCount.ToString();

            foreach (var item in data.haloSkill)
            {
                switch (item)
                {
                    case 0:
                        cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 1;
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 1;
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 1;
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 1;
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 1;
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 1;
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 1;
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 1;
                        break;
                }
            }
            foreach(var item in data.ttSkill)
            {
                switch (item.Key)
                {
                    case 0:
                        cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 2;
                        tbLeftAttCnt.Text = item.Count.ToString();
                        tbLeftAttStep.Text = item.Step.ToString();
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 2;
                        tbMidAttCnt.Text = item.Count.ToString();
                        tbMidAttStep.Text = item.Step.ToString();
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 2;
                        tbRightAttCnt.Text = item.Count.ToString();
                        tbRightAttStep.Text = item.Step.ToString();
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 2;
                        tbQAttCnt.Text = item.Count.ToString();
                        tbQAttStep.Text = item.Step.ToString();
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 2;
                        tbWAttCnt.Text = item.Count.ToString();
                        tbWAttStep.Text = item.Step.ToString();
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 2;
                        tbEAttCnt.Text = item.Count.ToString();
                        tbEAttStep.Text = item.Step.ToString();
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 2;
                        tbRAttCnt.Text = item.Count.ToString();
                        tbRAttStep.Text = item.Step.ToString();
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 2;
                        tbTAttCnt.Text = item.Count.ToString();
                        tbTAttStep.Text = item.Step.ToString();
                        break;
                }
            }
            foreach(var item in data.shieldSkill)
            {
                switch (item.Key)
                {
                    case 0:
                        cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 3;
                        tbLeftAttStep.Text = item.Step.ToString();
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 3;
                        tbMidAttStep.Text = item.Step.ToString();
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 3;
                        tbRightAttStep.Text = item.Step.ToString();
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 3;
                        tbQAttStep.Text = item.Step.ToString();
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 3;
                        tbWAttStep.Text = item.Step.ToString();
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 3;
                        tbEAttStep.Text = item.Step.ToString();
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 3;
                        tbRAttStep.Text = item.Step.ToString();
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 3;
                        tbTAttStep.Text = item.Step.ToString();
                        break;
                }
            }
            foreach (var item in data.battleOnceSkill)
            {
                switch (item)
                {
                    case 0:
                        cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 4;
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 4;
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 4;
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 4;
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 4;
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 4;
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 4;
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 4;
                        break;
                }
            }
            foreach (var item in data.summonerSkill)//召唤类技能
            {
                switch (item.Key)
                {
                    case 0:
                         cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 5;
                        tbLeftAttCnt.Text = item.Count.ToString();
                        tbLeftAttStep.Text = item.Step.ToString();
                        cbLeftCorpse.Checked = item.NeedCorpse;
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 5;
                        tbMidAttCnt.Text = item.Count.ToString();
                        tbMidAttStep.Text = item.Step.ToString();
                        cbMidCorpse.Checked = item.NeedCorpse;
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 5;
                        tbRightAttCnt.Text = item.Count.ToString();
                        tbRightAttStep.Text = item.Step.ToString();
                        cbRightCorpse.Checked = item.NeedCorpse;
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 5;
                        tbQAttCnt.Text = item.Count.ToString();
                        tbQAttStep.Text = item.Step.ToString();
                        cbQCorpse.Checked = item.NeedCorpse;
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 5;
                        tbWAttCnt.Text = item.Count.ToString();
                        tbWAttStep.Text = item.Step.ToString();
                        cbWCorpse.Checked = item.NeedCorpse;
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 5;
                        tbEAttCnt.Text = item.Count.ToString();
                        tbEAttStep.Text = item.Step.ToString();
                        cbECorpse.Checked = item.NeedCorpse;
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 5;
                        tbRAttCnt.Text = item.Count.ToString();
                        tbRAttStep.Text = item.Step.ToString();
                        cbRCorpse.Checked = item.NeedCorpse;
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 5;
                        tbTAttCnt.Text = item.Count.ToString();
                        tbTAttStep.Text = item.Step.ToString();
                        cbTCorpse.Checked = item.NeedCorpse;
                        break;
                }
            }

            foreach (var item in data.multiTrapSkill)//陷阱类技能
            {
                switch (item.Key)
                {
                    case 0:
                        cbKeyLeft.Checked = true;
                        cbLeftSkill.SelectedIndex = 6;
                        tbLeftAttCnt.Text = item.CastTime.Count.ToString();
                        tbLeftAttStep.Text = item.Step.ToString();
                        break;
                    case 1:
                        cbKeyMid.Checked = true;
                        cbMidSkill.SelectedIndex = 6;
                        tbMidAttCnt.Text = item.CastTime.Count.ToString();
                        tbMidAttStep.Text = item.Step.ToString();
                        break;
                    case 2:
                        cbKeyRight.Checked = true;
                        cbRightSkill.SelectedIndex = 6;
                        tbRightAttCnt.Text = item.CastTime.Count.ToString();
                        tbRightAttStep.Text = item.Step.ToString();
                        break;
                    case 3:
                        cbKeyQ.Checked = true;
                        cbQSkill.SelectedIndex = 6;
                        tbQAttCnt.Text = item.CastTime.Count.ToString();
                        tbQAttStep.Text = item.Step.ToString();
                        break;
                    case 4:
                        cbKeyW.Checked = true;
                        cbWSkill.SelectedIndex = 6;
                        tbWAttCnt.Text = item.CastTime.Count.ToString();
                        tbWAttStep.Text = item.Step.ToString();
                        break;
                    case 5:
                        cbKeyE.Checked = true;
                        cbESkill.SelectedIndex = 6;
                        tbEAttCnt.Text = item.CastTime.Count.ToString();
                        tbEAttStep.Text = item.Step.ToString();
                        break;
                    case 6:
                        cbKeyR.Checked = true;
                        cbRSkill.SelectedIndex = 6;
                        tbRAttCnt.Text = item.CastTime.Count.ToString();
                        tbRAttStep.Text = item.Step.ToString();
                        break;
                    case 7:
                        cbKeyT.Checked = true;
                        cbTSkill.SelectedIndex = 6;
                        tbTAttCnt.Text = item.CastTime.Count.ToString();
                        tbTAttStep.Text = item.Step.ToString();
                        break;
                }
            }

            //补给
            cbLogoutType.SelectedIndex = data.LogoutType;
            tbLogoutHP.Text = data.LogOutData.ToString();//贫血小退

            cbUseType1.Checked=data.Flask1.Use;
            cbType1.SelectedIndex=data.Flask1.Type;
            tbType1.Text = data.Flask1.CalcData.ToString();
            tbStep1.Text = data.Flask1.Step.ToString();
            foreach (var item in data.Flask1.KeyList)
            {
                switch (item)
                {
                    case 1:
                        cbKey1.SelectedIndex = 0;
                        break;
                    case 2:
                        cbKey2.SelectedIndex = 0;
                        break;
                    case 3:
                        cbKey3.SelectedIndex = 0;
                        break;
                    case 4:
                        cbKey4.SelectedIndex = 0;
                        break;
                    case 5:
                        cbKey5.SelectedIndex = 0;
                        break;
                }
            }
            cbUseType2.Checked = data.Flask2.Use;
            cbType2.SelectedIndex = data.Flask2.Type;
            tbType2.Text = data.Flask2.CalcData.ToString();
            tbStep2.Text = data.Flask2.Step.ToString();
            foreach (var item in data.Flask2.KeyList)
            {
                switch (item)
                {
                    case 1:
                        cbKey1.SelectedIndex = 1;
                        break;
                    case 2:
                        cbKey2.SelectedIndex = 1;
                        break;
                    case 3:
                        cbKey3.SelectedIndex = 1;
                        break;
                    case 4:
                        cbKey4.SelectedIndex = 1;
                        break;
                    case 5:
                        cbKey5.SelectedIndex = 1;
                        break;
                }
            }
            cbUseType3.Checked = data.Flask3.Use;
            cbType3.SelectedIndex = data.Flask3.Type;
            tbType3.Text = data.Flask3.CalcData.ToString();
            tbStep3.Text = data.Flask3.Step.ToString();
            foreach (var item in data.Flask3.KeyList)
            {
                switch (item)
                {
                    case 1:
                        cbKey1.SelectedIndex = 2;
                        break;
                    case 2:
                        cbKey2.SelectedIndex = 2;
                        break;
                    case 3:
                        cbKey3.SelectedIndex = 2;
                        break;
                    case 4:
                        cbKey4.SelectedIndex = 2;
                        break;
                    case 5:
                        cbKey5.SelectedIndex = 2;
                        break;
                }
            }
            cbUseType4.Checked = data.Flask4.Use;
            cbType4.SelectedIndex = data.Flask4.Type;
            tbType4.Text = data.Flask4.CalcData.ToString();
            tbStep4.Text = data.Flask4.Step.ToString();
            foreach (var item in data.Flask4.KeyList)
            {
                switch (item)
                {
                    case 1:
                        cbKey1.SelectedIndex = 3;
                        break;
                    case 2:
                        cbKey2.SelectedIndex = 3;
                        break;
                    case 3:
                        cbKey3.SelectedIndex = 3;
                        break;
                    case 4:
                        cbKey4.SelectedIndex = 3;
                        break;
                    case 5:
                        cbKey5.SelectedIndex = 3;
                        break;
                }
            }
            cbUseType5.Checked = data.Flask5.Use;
            cbType5.SelectedIndex = data.Flask5.Type;
            tbType5.Text = data.Flask5.CalcData.ToString();
            tbStep5.Text = data.Flask5.Step.ToString();
            foreach (var item in data.Flask5.KeyList)
            {
                switch (item)
                {
                    case 1:
                        cbKey1.SelectedIndex = 4;
                        break;
                    case 2:
                        cbKey2.SelectedIndex = 4;
                        break;
                    case 3:
                        cbKey3.SelectedIndex = 4;
                        break;
                    case 4:
                        cbKey4.SelectedIndex = 4;
                        break;
                    case 5:
                        cbKey5.SelectedIndex = 4;
                        break;
                }
            }
       

            //拾取
            foreach (var item in data.LootTypeList)
            {
                if (item.Key == byTrophy_SkillStone)
                {
                    cbLootSkill.Checked = true;
                    tbSkillQuality.Enabled = true;
                    tbSkillQuality.Text = data.LootSkillQuality.ToString();
                }
                else if (item.Key == byTrophy_Currency)
                {
                    cbLootCurrency.Checked = true;
                }
                else if (item.Key == byTrophy_Money)
                {
                    cbLootMoney.Checked = true;
                }
                else if (item.Key == byTrophy_Maps)
                {
                    cbLootMaps.Checked = true;
                    cmLootMaps.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Flask)
                {
                    cbLootFlask.Checked = true;
                    cmLootFlask.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Armour)
                {
                    cbLootArmour.Checked = true;
                    cmLootArmour.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Ring)
                {
                    cbLootRing.Checked = true;
                    cmLootRing.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Amulet)
                {
                    cbLootAmulet.Checked = true;
                    cmLootAmulet.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Belt)
                {
                    cbLootBelt.Checked = true;
                    cmLootBelt.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Weapon)
                {
                    cbLootWeapon.Checked = true;
                    cmLootWeapon.SelectedIndex = item.Value;
                }
            }
            cbLootThreeColor.Checked=data.LootThreeColor;
            cmLootSocket.SelectedIndex=data.LootSocketFilter;
            cmLootConnect.SelectedIndex=data.LootSocketConnectFilter;
            cmOpenBoxColor.SelectedIndex=data.BoxFilterColor;

            //售卖
            foreach (var item in data.SellTypeList)
            {
                if (item.Key == byTrophy_Maps)
                {
                    cbSellMaps.Checked = true;
                    cmSellMaps.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Flask)
                {
                    cbSellFlask.Checked = true;
                    cmSellFlask.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Armour)
                {
                    cbSellArmour.Checked = true;
                    cmSellArmour.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Ring)
                {
                    cbSellRing.Checked = true;
                    cmSellRing.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Amulet)
                {
                    cbSellAmulet.Checked = true;
                    cmSellAmulet.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Belt)
                {
                    cbSellBelt.Checked = true;
                    cmSellBelt.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Weapon)
                {
                    cbSellWeapon.Checked = true;
                    cmSellWeapon.SelectedIndex = item.Value;
                }
            }
            cbSellThreeColor.Checked = data.SellThreeColor;
            cmSellSocket.SelectedIndex = data.SellSocketFilter;
            cmSellConnect.SelectedIndex = data.SellSocketConnectFilter;
            //存仓
            if (data.SaveTypeList == null)
                data.SaveTypeList = new SortedList<int, int>();
            foreach (var item in data.SaveTypeList)
            {
                if (item.Key == byTrophy_SkillStone)
                {
                    cbSaveSkill.Checked = true;
                }
                else if (item.Key == byTrophy_Currency)
                {
                    cbSaveCurrency.Checked = true;
                }
                else if (item.Key == byTrophy_Money)
                {
                    cbSaveMoney.Checked = true;
                }
                else if (item.Key == byTrophy_Maps)
                {
                    cbSaveMaps.Checked = true;
                    cmSaveMaps.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Flask)
                {
                    cbSaveFlask.Checked = true;
                    cmSaveFlask.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Armour)
                {
                    cbSaveArmour.Checked = true;
                    cmSaveArmour.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Ring)
                {
                    cbSaveRing.Checked = true;
                    cmSaveRing.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Amulet)
                {
                    cbSaveAmulet.Checked = true;
                    cmSaveAmulet.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Belt)
                {
                    cbSaveBelt.Checked = true;
                    cmSaveBelt.SelectedIndex = item.Value;
                }
                else if (item.Key == byTrophy_Weapon)
                {
                    cbSaveWeapon.Checked = true;
                    cmSaveWeapon.SelectedIndex = item.Value;
                }
            }
            cbSaveThreeColor.Checked = data.SaveThreeColor;
            cmSaveSocket.SelectedIndex = data.SaveSocketFilter;
            cmSaveConnect.SelectedIndex = data.SaveSocketConnectFilter;
            cbIdentityItem.Checked=data.bNeedIdentity;
            cbNoIdenAmulet.Checked=data.bNoIdentifyAmulet ;
            cbNoIdenRing.Checked=data.bNoIdentifyRing;
            cbNoIdenBelt.Checked=data.bNoIdentifyBelt;

            cbTrimStorage.Checked=data.bTrimStorage;

            //////////////优先攻击
            FileStream file = new FileStream("MonsterName.txt", FileMode.Open);
            byte[] buff = new byte[22];
            byte[] name = new byte[20];
            while (true)
            {
                int nRead = file.Read(buff, 0, 22);
                if (nRead < 1)
                    break;
                Array.Copy(buff, name, 20);
                string strName = Encoding.Unicode.GetString(name).TrimEnd('\0');
                string strPrior = buff[20].ToString();

                string strInfo = strName + "*"+strPrior;
                lbPriorMonster.Items.Add(strInfo);
                //  listBox1.Items.Add(strName);
            }
            file.Close();

            Program.gdata.LootName.Clear();
            FileStream LootFile = new FileStream("LootName.txt", FileMode.Open);
            byte[] LootBuff = new byte[22];
            byte[] LootName = new byte[20];
            while (true)
            {
                int nRead = LootFile.Read(LootBuff, 0, 22);
                if (nRead < 1)
                    break;
                Array.Copy(LootBuff, LootName, 20);
                string strName = Encoding.Unicode.GetString(LootName).TrimEnd('\0');
               // string strType = LootBuff[20].ToString();
                Program.gdata.LootName.Add(strName, LootBuff[20]);              
              //  string strInfo = strName + "*" + strType;
               // lbLootName.Items.Add(strInfo);
                //  listBox1.Items.Add(strName);
            }
            LootFile.Close();
            cbLootNameTypeFilter.SelectedIndex = 0;
            updateLootNameText();
            //高级存仓
            foreach (var item in data.AllFilter)
            {
                lbAllFilter.Items.Add(item.strName);
            }
            FlashNameSaveCtrl();
           // lbAllFilter.Items
            return ret;
        }

        private void rbLevel1_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbLevel1.Checked)
                return;
            int nGroup=cbGroup.SelectedIndex;
            ++nGroup;
            //显示所有的一般难度的地图
            lbAllMap.Items.Clear();
            foreach (var item in Program.gdata.AllBattleMap)
            {
                if (nGroup == 0)
                {
                    if (item.Value.Level == 1)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
                else
                {
                    if (item.Value.Level == 1&&item.Value.Group==nGroup)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
            }
        }

        private void rbLevel2_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbLevel2.Checked)
                return;
            int nGroup = cbGroup.SelectedIndex;
            ++nGroup;
            //显示所有的一般难度的地图
            lbAllMap.Items.Clear();
            foreach (var item in Program.gdata.AllBattleMap)
            {
                if (nGroup == 0)
                {
                    if (item.Value.Level == 2)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
                else
                {
                    if (item.Value.Level == 2&& item.Value.Group == nGroup)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
            }
        }

        private void rbLevel3_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbLevel3.Checked)
                return;
            int nGroup = cbGroup.SelectedIndex;
            ++nGroup;
            //显示所有的一般难度的地图
            lbAllMap.Items.Clear();
            foreach (var item in Program.gdata.AllBattleMap)
            {
                if (nGroup == 0)
                {
                    if (item.Value.Level == 3)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
                else
                {
                    if (item.Value.Level == 3 && item.Value.Group == nGroup)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
            }
        }

        private void g_Click(object sender, EventArgs e)
        {
            if (lbMissionMap.Items.Contains(lbAllMap.SelectedItem) == false)
                lbMissionMap.Items.Add(lbAllMap.SelectedItem);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lbMissionMap.Items.Remove(lbMissionMap.SelectedItem);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
        private void btnSave_Click(object sender, EventArgs e)
        {

            data.bDungeonModel = cbDungeonModel.Checked;
            data.bBattleRelive = cbReliveModel.Checked;
            data.bExplorePollutant = cbExplorePollutant.Checked;
            data.bPriorityAttack = cbPriorityAttack.Checked;
            data.bDungeonHome = cbUseHideHome.Checked;
            data.bFullHide = cbFullHide.Checked;

            data.MissionMapList.Clear();
            foreach (var item in lbMissionMap.Items)
            {
                data.MissionMapList.Add(item.ToString());
            }
            //技能

            data.bAutoUpSkill = cbAutoUpSkill.Checked;

            data.bUseSafeAttSpeed = cbDefaultAttSpeed.Checked;

            float.TryParse(tbNorAttDis.Text, out data.NorAttDis);
          //  int.TryParse(tbNorAttStep.Text, out data.NorAttStep);

          //  data.bUseSinAtt = cbUseSinAtt.Checked;
            float.TryParse(tbSinAttDis.Text, out data.SinAttDis);
         //   int.TryParse(tbSinAttStep.Text, out data.SinAttStep);

        //    data.bUseMulAtt = cbUseMulAtt.Checked;
            float.TryParse(tbMulAttDis.Text, out data.MulAttDis);
         //   int.TryParse(tbMulAttStep.Text, out data.MulAttStep);
            int.TryParse(tbMultiCount.Text,out  data.MultiCount);

            data.nSinAttKey = -1;
            data.nMulAttKey = -1;
            data.nNorAttKey = -1;
            data.nGobackSkill = -1;
            data.MoveSkillKey = -1;
            data.MoveSkillStep = 5000;

            data.haloSkill.Clear();
            data.ttSkill.Clear();
            data.shieldSkill.Clear();
            data.battleOnceSkill.Clear();
            data.summonerSkill.Clear();
            data.multiTrapSkill.Clear();
            if (cbKeyLeft.Checked)
            {
                switch (cbLeftSkill.SelectedIndex)
                {
                    case 0://普通攻击
                        data.nNorAttKey = 0;
                        int.TryParse(tbLeftAttStep.Text, out  data.NorAttStep);
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(0);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 0;
                            int.TryParse(tbLeftAttStep.Text, out skill.Step);
                            int.TryParse(tbLeftAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 0;
                            int.TryParse(tbLeftAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(0);
                        }
                        break;
                    case 5:
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 0;
                            int.TryParse(tbLeftAttStep.Text, out skill.Step);
                            int.TryParse(tbLeftAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbLeftCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 0;
                            int.TryParse(tbLeftAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbLeftAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyMid.Checked)
            {
                switch (cbMidSkill.SelectedIndex)
                {
                    case 0:
                        data.nGobackSkill = 1;
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(1);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 1;
                            int.TryParse(tbMidAttStep.Text, out skill.Step);
                            int.TryParse(tbMidAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 1;
                            int.TryParse(tbMidAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(1);
                        }
                        break;
                    case 5:
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 1;
                            int.TryParse(tbMidAttStep.Text, out skill.Step);
                            int.TryParse(tbMidAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbMidCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 1;
                            int.TryParse(tbMidAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbMidAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyRight.Checked)
            {
                switch (cbRightSkill.SelectedIndex)
                {
                    case 0://普通攻击
                        data.MoveSkillKey = 2;
                        int.TryParse(tbRightAttStep.Text, out data.MoveSkillStep);
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(2);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 2;
                            int.TryParse(tbRightAttStep.Text, out skill.Step);
                            int.TryParse(tbRightAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 2;
                            int.TryParse(tbRightAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(2);
                        }
                        break;
                    case 5:
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 2;
                            int.TryParse(tbRightAttStep.Text, out skill.Step);
                            int.TryParse(tbRightAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbRightCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 2;
                            int.TryParse(tbRightAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbRightAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyQ.Checked)
            {
                switch (cbQSkill.SelectedIndex)
                {
                    case 0://单体攻击
                        data.nSinAttKey = 3;
                        int.TryParse(tbQAttStep.Text, out  data.SinAttStep);
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(3);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 3;
                            int.TryParse(tbQAttStep.Text, out skill.Step);
                            int.TryParse(tbQAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 3;
                            int.TryParse(tbQAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(3);
                        }
                        break;
                    case 5:
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 3;
                            int.TryParse(tbQAttStep.Text, out skill.Step);
                            int.TryParse(tbQAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbQCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 3;
                            int.TryParse(tbQAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbQAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyW.Checked)
            {
                switch (cbWSkill.SelectedIndex)
                {
                    case 0://群体攻击
                        data.nMulAttKey = 4;
                        int.TryParse(tbWAttStep.Text, out  data.MulAttStep);
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(4);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 4;
                            int.TryParse(tbWAttStep.Text, out skill.Step);
                            int.TryParse(tbWAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 4;
                            int.TryParse(tbWAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(4);
                        }
                        break;
                    case 5:
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 4;
                            int.TryParse(tbWAttStep.Text, out skill.Step);
                            int.TryParse(tbWAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbWCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 4;
                            int.TryParse(tbWAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbWAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyE.Checked)
            {
                switch(cbESkill.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(5);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 5;
                            int.TryParse(tbEAttStep.Text, out skill.Step);
                            int.TryParse(tbEAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 5;
                            int.TryParse(tbEAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(5);
                        }
                        break;
                    case 5://召唤技能
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 5;
                            int.TryParse(tbEAttStep.Text, out skill.Step);
                            int.TryParse(tbEAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbECorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 5;
                            int.TryParse(tbEAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbEAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyR.Checked)
            {
                switch (cbRSkill.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(6);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 6;
                            int.TryParse(tbRAttStep.Text, out skill.Step);
                            int.TryParse(tbRAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 6;
                            int.TryParse(tbRAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(6);
                        }
                        break;
                    case 5://圖騰技能
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 6;
                            int.TryParse(tbRAttStep.Text, out skill.Step);
                            int.TryParse(tbRAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbRCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 6;
                            int.TryParse(tbRAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbRAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            if (cbKeyT.Checked)
            {
                switch (cbTSkill.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1: //光環技能
                        data.haloSkill.Add(7);
                        break;
                    case 2://圖騰技能
                        {
                            TTSkill skill = new TTSkill();
                            skill.Key = 7;
                            int.TryParse(tbTAttStep.Text, out skill.Step);
                            int.TryParse(tbTAttCnt.Text, out skill.Count);
                            data.ttSkill.Add(skill);
                        }
                        break;
                    case 3://護盾技能
                        {
                            ShieldSkill skill = new ShieldSkill();
                            skill.Key = 7;
                            int.TryParse(tbTAttStep.Text, out skill.Step);
                            data.shieldSkill.Add(skill);
                        }
                        break;
                    case 4:
                        {
                            data.battleOnceSkill.Add(7);
                        }
                        break;
                    case 5://圖騰技能
                        {
                            SummonerSkill skill = new SummonerSkill();
                            skill.Key = 7;
                            int.TryParse(tbTAttStep.Text, out skill.Step);
                            int.TryParse(tbTAttCnt.Text, out skill.Count);
                            skill.NeedCorpse = cbTCorpse.Checked;
                            data.summonerSkill.Add(skill);
                        }
                        break;
                    case 6://陷阱技能
                        {
                            TrapSkill skill = new TrapSkill();
                            skill.Key = 7;
                            int.TryParse(tbTAttStep.Text, out skill.Step);
                            int nCount = 0;
                            int.TryParse(tbTAttCnt.Text, out nCount);
                            for (int i = 0; i < nCount; ++i)
                            {
                                skill.CastTime.Add(0);
                            }
                            data.multiTrapSkill.Add(skill);
                        }
                        break;
                }
            }
            ////////////////////////药剂
            data.LogoutType = cbLogoutType.SelectedIndex;
            int.TryParse(tbLogoutHP.Text, out data.LogOutData);

            data.Flask1.Use = cbUseType1.Checked;
            data.Flask1.Type = cbType1.SelectedIndex;
            int.TryParse(tbType1.Text,out data.Flask1.CalcData);
            int.TryParse(tbStep1.Text, out data.Flask1.Step);

            data.Flask2.Use = cbUseType2.Checked;
            data.Flask2.Type = cbType2.SelectedIndex;
            int.TryParse(tbType2.Text, out data.Flask2.CalcData);
            int.TryParse(tbStep2.Text, out data.Flask2.Step);

            data.Flask3.Use = cbUseType3.Checked;
            data.Flask3.Type = cbType3.SelectedIndex;
            int.TryParse(tbType3.Text, out data.Flask3.CalcData);
            int.TryParse(tbStep3.Text, out data.Flask3.Step);

            data.Flask4.Use = cbUseType4.Checked;
            data.Flask4.Type = cbType4.SelectedIndex;
            int.TryParse(tbType4.Text, out data.Flask4.CalcData);
            int.TryParse(tbStep4.Text, out data.Flask4.Step);

            data.Flask5.Use = cbUseType5.Checked;
            data.Flask5.Type = cbType5.SelectedIndex;
            int.TryParse(tbType5.Text, out data.Flask5.CalcData);
            int.TryParse(tbStep5.Text, out data.Flask5.Step);

            data.Flask1.KeyList.Clear();
            data.Flask2.KeyList.Clear();
            data.Flask3.KeyList.Clear();
            data.Flask4.KeyList.Clear();
            data.Flask5.KeyList.Clear();

            switch(cbKey1.SelectedIndex)
            {
                case 0:
                    data.Flask1.KeyList.Add(1);
                    break;
                case 1:
                    data.Flask2.KeyList.Add(1);
                    break;
                case 2:
                    data.Flask3.KeyList.Add(1);
                    break;
                case 3:
                    data.Flask4.KeyList.Add(1);
                    break;
                case 4:
                    data.Flask5.KeyList.Add(1);
                    break;
            }
            switch (cbKey2.SelectedIndex)
            {
                case 0:
                    data.Flask1.KeyList.Add(2);
                    break;
                case 1:
                    data.Flask2.KeyList.Add(2);
                    break;
                case 2:
                    data.Flask3.KeyList.Add(2);
                    break;
                case 3:
                    data.Flask4.KeyList.Add(1);
                    break;
                case 4:
                    data.Flask5.KeyList.Add(2);
                    break;
            }
            switch (cbKey3.SelectedIndex)
            {
                case 0:
                    data.Flask1.KeyList.Add(3);
                    break;
                case 1:
                    data.Flask2.KeyList.Add(3);
                    break;
                case 2:
                    data.Flask3.KeyList.Add(3);
                    break;
                case 3:
                    data.Flask4.KeyList.Add(3);
                    break;
                case 4:
                    data.Flask5.KeyList.Add(3);
                    break;
            }
            switch (cbKey4.SelectedIndex)
            {
                case 0:
                    data.Flask1.KeyList.Add(4);
                    break;
                case 1:
                    data.Flask2.KeyList.Add(4);
                    break;
                case 2:
                    data.Flask3.KeyList.Add(4);
                    break;
                case 3:
                    data.Flask4.KeyList.Add(4);
                    break;
                case 4:
                    data.Flask5.KeyList.Add(4);
                    break;
            }
            switch (cbKey5.SelectedIndex)
            {
                case 0:
                    data.Flask1.KeyList.Add(5);
                    break;
                case 1:
                    data.Flask2.KeyList.Add(5);
                    break;
                case 2:
                    data.Flask3.KeyList.Add(5);
                    break;
                case 3:
                    data.Flask4.KeyList.Add(5);
                    break;
                case 4:
                    data.Flask5.KeyList.Add(5);
                    break;
            }


            ///////////////////////////////拾取
            data.LootTypeList.Clear();
            if(cbLootSkill.Checked)
            {
                data.LootTypeList.Add(byTrophy_SkillStone, 0);
            }
            if (cbLootCurrency.Checked)
            {
                data.LootTypeList.Add(byTrophy_Currency, 0);
            }
            if (cbLootMoney.Checked)
            {
                data.LootTypeList.Add(byTrophy_Money, 0);
            }
            if (cbLootMaps.Checked)
            {
                data.LootTypeList.Add(byTrophy_Maps, cmLootMaps.SelectedIndex);
            }
            if (cbLootFlask.Checked)
            {
                data.LootTypeList.Add(byTrophy_Flask, cmLootFlask.SelectedIndex);
            }
            if (cbLootArmour.Checked)
            {
                data.LootTypeList.Add(byTrophy_Armour, cmLootArmour.SelectedIndex);
            }
            if (cbLootRing.Checked)
            {
                data.LootTypeList.Add(byTrophy_Ring, cmLootRing.SelectedIndex);
            }
            if (cbLootAmulet.Checked)
            {
                data.LootTypeList.Add(byTrophy_Amulet, cmLootAmulet.SelectedIndex);
            }
            if (cbLootBelt.Checked)
            {
                data.LootTypeList.Add(byTrophy_Belt, cmLootBelt.SelectedIndex);
            }
            if (cbLootWeapon.Checked)
            {
                data.LootTypeList.Add(byTrophy_Weapon, cmLootWeapon.SelectedIndex);
            }

            data.LootThreeColor = cbLootThreeColor.Checked;
            data.LootSocketFilter=(short)cmLootSocket.SelectedIndex;
            data.LootSocketConnectFilter=(short)cmLootConnect.SelectedIndex;
            data.LootSkillQuality = 0;
            short.TryParse(tbSkillQuality.Text, out data.LootSkillQuality);

            data.BoxFilterColor = cmOpenBoxColor.SelectedIndex;
            
            ///////////////////////////////售卖
            data.SellTypeList.Clear();
            if (cbSellMaps.Checked)
            {
                data.SellTypeList.Add(byTrophy_Maps, cmSellMaps.SelectedIndex);
            }
            if (cbSellFlask.Checked)
            {
                data.SellTypeList.Add(byTrophy_Flask, cmSellFlask.SelectedIndex);
            }
            if (cbSellArmour.Checked)
            {
                data.SellTypeList.Add(byTrophy_Armour, cmSellArmour.SelectedIndex);
            }
            if (cbSellRing.Checked)
            {
                data.SellTypeList.Add(byTrophy_Ring, cmSellRing.SelectedIndex);
            }
            if (cbSellAmulet.Checked)
            {
                data.SellTypeList.Add(byTrophy_Amulet, cmSellAmulet.SelectedIndex);
            }
            if (cbSellBelt.Checked)
            {
                data.SellTypeList.Add(byTrophy_Belt, cmSellBelt.SelectedIndex);
            }
            if (cbSellWeapon.Checked)
            {
                data.SellTypeList.Add(byTrophy_Weapon, cmSellWeapon.SelectedIndex);
            }

            data.SellThreeColor = cbSellThreeColor.Checked;
            data.SellSocketFilter = (short)cmSellSocket.SelectedIndex;
            data.SellSocketConnectFilter = (short)cmSellConnect.SelectedIndex;


            /////////////////////////////////////////////////////////
            ///////////////////////////////存仓

            data.SaveTypeList.Clear();
            if (cbSaveSkill.Checked)
            {
                data.SaveTypeList.Add(byTrophy_SkillStone, 0);
            }
            if (cbSaveCurrency.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Currency, 0);
            }
            if (cbSaveMoney.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Money, 0);
            }
            if (cbSaveMaps.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Maps, cmSaveMaps.SelectedIndex);
            }
            if (cbSaveFlask.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Flask, cmSaveFlask.SelectedIndex);
            }
            if (cbSaveArmour.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Armour, cmSaveArmour.SelectedIndex);
            }
            if (cbSaveRing.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Ring, cmSaveRing.SelectedIndex);
            }
            if (cbSaveAmulet.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Amulet, cmSaveAmulet.SelectedIndex);
            }
            if (cbSaveBelt.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Belt, cmSaveBelt.SelectedIndex);
            }
            if (cbSaveWeapon.Checked)
            {
                data.SaveTypeList.Add(byTrophy_Weapon, cmSaveWeapon.SelectedIndex);
            }
            data.bTrimStorage = cbTrimStorage.Checked;
            data.bNeedIdentity=cbIdentityItem.Checked;

            data.bNoIdentifyAmulet = cbNoIdenAmulet.Checked;
            data.bNoIdentifyRing = cbNoIdenRing.Checked;
            data.bNoIdentifyBelt = cbNoIdenBelt.Checked;

            data.SaveThreeColor = cbSaveThreeColor.Checked;
            data.SaveSocketFilter = (short)cmSaveSocket.SelectedIndex;
            data.SaveSocketConnectFilter = (short)cmSaveConnect.SelectedIndex;

            ///////////////////////////////////////////////////////////
            //序列化到文件
            Stream fStream = null;
            fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(fStream,data);
           
            fStream.Close();
            this.Close();

            //優先攻擊
            FileStream file = new FileStream("MonsterName.txt", FileMode.Create);
            foreach (var item in lbPriorMonster.Items)
            {
                byte[] buff = new byte[22];
                string strInfo = item.ToString();
                string strName = strInfo.Substring(0, strInfo.Length - 3);
                string strPrior = strInfo.Substring(strInfo.Length - 2, 2);
                byte byPrior = 16;
                byte.TryParse(strPrior, out byPrior);
                byte[] temp = Encoding.Unicode.GetBytes(strName.ToString());
                Array.Copy(temp, buff, temp.Length);
                buff[20] = byPrior;
                file.Write(buff, 0, 22);
            }
            file.Close();
 
            //名称拾取
            FileStream LootFile = new FileStream("LootName.txt", FileMode.Create);
            foreach (var item in Program.gdata.LootName)
            {
                byte[] buff = new byte[22];
                string strName = item.Key;
                byte byType =(byte)item.Value;
                byte[] temp = Encoding.Unicode.GetBytes(strName.ToString());
                Array.Copy(temp, buff, temp.Length);
                buff[20] = byType;
                LootFile.Write(buff, 0, 22);
            }
            LootFile.Close();
            Program.gdata.LootName.Clear();
        }

        private void tpSell_Click(object sender, EventArgs e)
        {

        }

        private void cbLootSkill_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootSkill.Checked)
            {
                tbSkillQuality.Enabled = true;
                tbSkillQuality.Text = "0";
            }
            else
                tbSkillQuality.Enabled = false;
        }
        private void cbLootMaps_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootMaps.Checked)
            {
                cmLootMaps.Enabled = true;
                cmLootMaps.SelectedIndex = 1;
            }
            else
                cmLootMaps.Enabled = false;
        }
        private void cbLootFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootFlask.Checked)
            {
                cmLootFlask.Enabled = true;
                cmLootFlask.SelectedIndex = 1;
            }
            else
                cmLootFlask.Enabled = false;
        }

        private void cbLootArmour_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootArmour.Checked)
            {
                cmLootArmour.Enabled = true;
                cmLootArmour.SelectedIndex = 1;
            }
            else
                cmLootArmour.Enabled = false;
        }

        private void cbLootWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootWeapon.Checked)
            {
                cmLootWeapon.Enabled = true;
                cmLootWeapon.SelectedIndex = 1;
            }
            else
                cmLootWeapon.Enabled = false;
        }

        private void cbLootBelt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootBelt.Checked)
            {
                cmLootBelt.Enabled = true;
                cmLootBelt.SelectedIndex = 1;
            }
            else
                cmLootBelt.Enabled = false;
        }

        private void cbLootRing_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootRing.Checked)
            {
                cmLootRing.Enabled = true;
                cmLootRing.SelectedIndex = 1;
            }
            else
                cmLootRing.Enabled = false;
        }

        private void cbLootAmulet_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLootAmulet.Checked)
            {
                cmLootAmulet.Enabled = true;
                cmLootAmulet.SelectedIndex = 1;
            }
            else
                cmLootAmulet.Enabled = false;
        }

        private void cbSellMaps_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellMaps.Checked)
            {
                cmSellMaps.Enabled = true;
                cmSellMaps.SelectedIndex = 1;
            }
            else
                cmSellMaps.Enabled = false;
        }
        private void cbSellFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellFlask.Checked)
            {
                cmSellFlask.Enabled = true;
                cmSellFlask.SelectedIndex = 1;
            }
            else
                cmSellFlask.Enabled = false;
        }

        private void cbSellArmour_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellArmour.Checked)
            {
                cmSellArmour.Enabled = true;
                cmSellArmour.SelectedIndex = 1;
            }
            else
                cmSellArmour.Enabled = false;
        }

        private void cbSellWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellWeapon.Checked)
            {
                cmSellWeapon.Enabled = true;
                cmSellWeapon.SelectedIndex = 1;
            }
            else
                cmSellWeapon.Enabled = false;
        }

        private void cbSellBelt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellBelt.Checked)
            {
                cmSellBelt.Enabled = true;
                cmSellBelt.SelectedIndex = 1;
            }
            else
                cmSellBelt.Enabled = false;
        }

        private void cbSellRing_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellRing.Checked)
            {
                cmSellRing.Enabled = true;
                cmSellRing.SelectedIndex = 1;
            }
            else
                cmSellRing.Enabled = false;
        }

        private void cbSellAmulet_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSellAmulet.Checked)
            {
                cmSellAmulet.Enabled = true;
                cmSellAmulet.SelectedIndex = 1;
            }
            else
                cmSellAmulet.Enabled = false;
        }
        private void cbSaveMaps_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveMaps.Checked)
            {
                cmSaveMaps.Enabled = true;
                cmSaveMaps.SelectedIndex = 1;
            }
            else
                cmSaveMaps.Enabled = false;
        }
        private void cbSaveFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveFlask.Checked)
            {
                cmSaveFlask.Enabled = true;
                cmSaveFlask.SelectedIndex = 1;
            }
            else
                cmSaveFlask.Enabled = false;
        }

        private void cbSaveArmour_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveArmour.Checked)
            {
                cmSaveArmour.Enabled = true;
                cmSaveArmour.SelectedIndex = 2;
            }
            else
                cmSaveArmour.Enabled = false;
        }

        private void cbSaveWeapon_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveWeapon.Checked)
            {
                cmSaveWeapon.Enabled = true;
                cmSaveWeapon.SelectedIndex = 2;
            }
            else
                cmSaveWeapon.Enabled = false;
        }

        private void cbSaveBelt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveBelt.Checked)
            {
                cmSaveBelt.Enabled = true;
                cmSaveBelt.SelectedIndex = 2;
            }
            else
                cmSaveBelt.Enabled = false;
        }

        private void cbSaveRing_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveRing.Checked)
            {
                cmSaveRing.Enabled = true;
                cmSaveRing.SelectedIndex = 2;
            }
            else
                cmSaveRing.Enabled = false;
        }

        private void cbSaveAmulet_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveAmulet.Checked)
            {
                cmSaveAmulet.Enabled = true;
                cmSaveAmulet.SelectedIndex = 2;
            }
            else
                cmSaveAmulet.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nLevel=0;
            int nGroup=cbGroup.SelectedIndex;
            ++nGroup;
        //    MessageBox.Show(nGroup.ToString());
            if (rbLevel1.Checked)
                nLevel = 1;
            else if (rbLevel2.Checked)
                nLevel = 2;
            else if(rbLevel3.Checked)
                nLevel = 3;

            lbAllMap.Items.Clear();
            foreach (var item in Program.gdata.AllBattleMap)
            {
                if (nGroup == 0)
                {
                    if (item.Value.Level == nLevel)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
                else
                {
                    if (item.Value.Level == nLevel && item.Value.Group == nGroup)
                    {
                        lbAllMap.Items.Add(item.Key);
                    }
                }
            }

        }

        private void cbKeyE_CheckedChanged(object sender, EventArgs e)
        {
            if(cbKeyE.Checked)
            {
                cbESkill.Enabled = true;
                cbESkill.SelectedIndex = 0;
                tbEAttStep.Enabled = true;
                tbEAttCnt.Enabled = true;
            }
            else
            {
                cbESkill.Enabled = false;
                tbEAttStep.Enabled = false;
                tbEAttCnt.Enabled = false;
            }
        }

        private void cbKeyR_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyR.Checked)
            {
                cbRSkill.Enabled = true;
                cbRSkill.SelectedIndex = 0;
                tbRAttStep.Enabled = true;
                tbRAttCnt.Enabled = true;
            }
            else
            {
                cbRSkill.Enabled = false;
                tbRAttStep.Enabled = false;
                tbRAttCnt.Enabled = false;
            }
        }

        private void cbKeyT_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyT.Checked)
            {
                cbTSkill.Enabled = true;
                cbTSkill.SelectedIndex = 0;
                tbTAttStep.Enabled = true;
                tbTAttCnt.Enabled = true;
            }
            else
            {
                cbTSkill.Enabled = false;
                tbTAttStep.Enabled = false;
                tbTAttCnt.Enabled = false;
            }
        }
        private void cbKeyLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyLeft.Checked)
            {
                cbLeftSkill.Enabled = true;
                cbLeftSkill.SelectedIndex = 0;
                tbLeftAttStep.Enabled = true;
                tbLeftAttCnt.Enabled = true;
            }
            else
            {
                cbLeftSkill.Enabled = false;
                tbLeftAttStep.Enabled = false;
                tbLeftAttCnt.Enabled = false;
            }
        }
        private void cbKeyMid_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyMid.Checked)
            {
                cbMidSkill.Enabled = true;
                cbMidSkill.SelectedIndex = 0;
                tbMidAttStep.Enabled = true;
                tbMidAttCnt.Enabled = true;
            }
            else
            {
                cbMidSkill.Enabled = false;
                tbMidAttStep.Enabled = false;
                tbMidAttCnt.Enabled = false;
            }
        }
        private void cbKeyRight_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyRight.Checked)
            {
                cbRightSkill.Enabled = true;
                cbRightSkill.SelectedIndex = 0;
                tbRightAttStep.Enabled = true;
                tbRightAttCnt.Enabled = true;
            }
            else
            {
                cbRightSkill.Enabled = false;
                tbRightAttStep.Enabled = false;
                tbRightAttCnt.Enabled = false;
            }
        }

        private void cbKeyQ_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyQ.Checked)
            {
                cbQSkill.Enabled = true;
                cbQSkill.SelectedIndex = 0;
                tbQAttStep.Enabled = true;
                tbQAttCnt.Enabled = true;
            }
            else
            {
                cbQSkill.Enabled = false;
                tbQAttStep.Enabled = false;
                tbQAttCnt.Enabled = false;
            }
        }

        private void cbKeyW_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeyW.Checked)
            {
                cbWSkill.Enabled = true;
                cbWSkill.SelectedIndex = 0;
                tbWAttStep.Enabled = true;
                tbWAttCnt.Enabled = true;
            }
            else
            {
                cbWSkill.Enabled = false;
                tbWAttStep.Enabled = false;
                tbWAttCnt.Enabled = false;
            }
        }

        private void cbUseType1_CheckedChanged(object sender, EventArgs e)
        {
            if(cbUseType1.Checked)
            {
                cbType1.Enabled = true;
                tbType1.Enabled = true;
                tbStep1.Enabled = true;
            }
            else
            {
                cbType1.Enabled = false;
                tbType1.Enabled = false;
                tbStep1.Enabled = false;
            }
        }

        private void cbUseType2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseType2.Checked)
            {
                cbType2.Enabled = true;
                tbType2.Enabled = true;
                tbStep2.Enabled = true;
            }
            else
            {
                cbType2.Enabled = false;
                tbType2.Enabled = false;
                tbStep2.Enabled = false;
            }
        }

        private void cbUseType3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseType3.Checked)
            {
                cbType3.Enabled = true;
                tbType3.Enabled = true;
                tbStep3.Enabled = true;
            }
            else
            {
                cbType3.Enabled = false;
                tbType3.Enabled = false;
                tbStep3.Enabled = false;
            }
        }

        private void cbUseType4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseType4.Checked)
            {
                cbType4.Enabled = true;
                tbType4.Enabled = true;
                tbStep4.Enabled = true;
            }
            else
            {
                cbType4.Enabled = false;
                tbType4.Enabled = false;
                tbStep4.Enabled = false;
            }
        }

        private void cbUseType5_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseType5.Checked)
            {
                cbType5.Enabled = true;
                tbType5.Enabled = true;
                tbStep5.Enabled = true;
            }
            else
            {
                cbType5.Enabled = false;
                tbType5.Enabled = false;
                tbStep5.Enabled = false;
            }
        }

        private void btnAddMonster_Click(object sender, EventArgs e)
        {
            string strMonsterName = tbMonsterName.Text;
            if(strMonsterName.Length>10)
            {
                MessageBox.Show("怪物名稱長度不能大於10個字符");
                return;
            }
            int nPrior = 0;
            int.TryParse(tbPrior.Text,out nPrior);
            if(nPrior>99||nPrior<16)
            {
                MessageBox.Show("請將優先級設定為16-99之間的數字");
                return;
            }
            string strPriorMonsterInfo = strMonsterName + "*" + tbPrior.Text;
            lbPriorMonster.Items.Add(strPriorMonsterInfo);
        }

        private void btnDelMonster_Click(object sender, EventArgs e)
        {
            lbPriorMonster.Items.Remove(lbPriorMonster.SelectedItem);
        }

        private void btnAddLootName_Click(object sender, EventArgs e)
        {
            string strLootName = tbLootName.Text;
            if (strLootName.Length > 10)
            {
                MessageBox.Show("拾取物品名稱長度不能大於10個字符");
                return;
            }
            if(cbLootNameType.SelectedIndex<0)
            {
                MessageBox.Show("請選擇物品類型");
                return;
            }
            int nType = cbLootNameType.SelectedIndex + 1;
            if (true == Program.gdata.LootName.ContainsKey(strLootName))
            {
                MessageBox.Show("已經有此名稱的物品");
                return;
            }
            Program.gdata.LootName.Add(strLootName, nType);

            updateLootNameText();
        }

        private void btnDelLootName_Click(object sender, EventArgs e)
        {
            string strKey=lbLootName.SelectedItem.ToString();
            Program.gdata.LootName.Remove(strKey);
            updateLootNameText();
        }
        void updateLootNameText()
        {
            int nType = cbLootNameTypeFilter.SelectedIndex;
            lbLootName.Items.Clear();
            foreach (var item in Program.gdata.LootName)
            {
                if (nType == 0)
                {
                    lbLootName.Items.Add(item.Key);
                }
                else
                {
                    if (item.Value == nType)
                    {
                        lbLootName.Items.Add(item.Key);
                    }
                }
            }
        }
        private void cbLootNameTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLootNameText();
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        string CurFilterName = null;
        List<string> CurFilterStrings = new List<string>();
        void FlashFilterItems()
        {
            //lbFilter.Items.Clear();
            //foreach (var item in CurFilterStrings)
            //{
            //    lbFilter.Items.Add(item);
            //}
        }

        void FlashAllFilterCtrl()
        {
            lbAllFilter.Items.Clear();
            foreach (var item in data.AllFilter)
            {
                lbAllFilter.Items.Add(item.strName);
            }
        }
        private void btnCreateFilter_Click(object sender, EventArgs e)
        {
          //名称
            string strName = tbSaveRuleName.Text;
            if (strName.Length < 1)
            {
                MessageBox.Show("請輸入方案名稱");
                return;
            }
            foreach (var item in data.AllFilter)
            {
                if (item.strName == strName)
                {
                    MessageBox.Show("此過方案稱已經存在");
                    return;
                }
            }
            //类型
            int n = cbSaveRuleType.SelectedIndex;
            if(n<0)
            {
                MessageBox.Show("請選擇物品類型");
                return;
            }
            short type = 0;
        //     const short byTrophy_SkillStone = 1;//技能石
        //const short byTrophy_Currency = 2;//卷轴宝石
        //const short byTrophy_Flask = 3;//血瓶
        //const short byTrophy_Armour = 4;//装备
        //const short byTrophy_Ring = 5;//戒指
        //const short byTrophy_Amulet = 6;//项链
        //const short byTrophy_Belt = 7;//腰带
        //const short byTrophy_Weapon = 8;//武器
        //const short byTrophy_Maps = 11;//地图
        //const short byTrophy_Money = 12;//通货
        switch (n)
        {
            case 0:
                type = byTrophy_Flask;
                break;
            case 1:
                type = byTrophy_Armour;
                break;
            case 2:
                type = byTrophy_Weapon;
                break;
            case 3:
                type = byTrophy_Belt;
                break;
            case 4:
                type = byTrophy_Ring;
                break;
            case 5:
                type = byTrophy_Amulet;
                break;
        }
           
            SaveFilter filter = new SaveFilter();
            filter.strName = strName;
            filter.type=type;
            //7个属性
            if (tbProperty1.Text.Length>0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty1.Text;
                if (false == short.TryParse(tbV11.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV12.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //2
            if (tbProperty2.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty2.Text;
                if (false == short.TryParse(tbV21.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV22.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //3
            if (tbProperty3.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty3.Text;
                if (false == short.TryParse(tbV31.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV32.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //4
            if (tbProperty4.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty4.Text;
                if (false == short.TryParse(tbV41.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV42.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //5
            if (tbProperty5.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty5.Text;
                if (false == short.TryParse(tbV51.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV52.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //6
            if (tbProperty6.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty6.Text;
                if (false == short.TryParse(tbV61.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV62.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }
            //7
            if (tbProperty7.Text.Length > 0)
            {
                Property rule = new Property();
                rule.strInfo = tbProperty7.Text;
                if (false == short.TryParse(tbV71.Text, out rule.n1))
                    rule.n1 = -1;
                if (false == short.TryParse(tbV72.Text, out rule.n2))
                    rule.n2 = -1;
                filter.rules.Add(rule);
            }

            //保存刷新
            data.AllFilter.Add(filter);
            FlashAllFilterCtrl();
        }

        private void btnDelFilter_Click(object sender, EventArgs e)
        {
            int n = lbAllFilter.SelectedIndex;
            if (n < 0)
                return;
            data.AllFilter.Remove(data.AllFilter[n]);
            FlashAllFilterCtrl();
        }
        void  ClearValueCtrl()
        {
            tbProperty1.Text = "";
            tbV11.Text = "";
            tbV12.Text = "";
            tbProperty2.Text = "";
            tbV21.Text = "";
            tbV22.Text = "";
            tbProperty3.Text = "";
            tbV31.Text = "";
            tbV32.Text = "";
            tbProperty4.Text = "";
            tbV41.Text = "";
            tbV42.Text = "";
            tbProperty5.Text = "";
            tbV51.Text = "";
            tbV52.Text = "";
            tbProperty6.Text = "";
            tbV61.Text = "";
            tbV62.Text = "";
            tbProperty7.Text = "";
            tbV71.Text = "";
            tbV72.Text = "";
        }
        private void lbAllFilter_DoubleClick(object sender, EventArgs e)
        {
            int n = lbAllFilter.SelectedIndex;
            if (n < 0)
            {
                return;
            }
            SaveFilter filter=data.AllFilter[n];
            //名称
            tbSaveRuleName.Text = filter.strName;

            //类型
            switch (filter.type)
            {
                case byTrophy_Flask:
                    cbSaveRuleType.SelectedIndex = 0;
                    break;
                case byTrophy_Armour:
                    cbSaveRuleType.SelectedIndex = 1;
                    break;
                case byTrophy_Weapon:
                    cbSaveRuleType.SelectedIndex = 2;
                    break;
                case byTrophy_Belt:
                    cbSaveRuleType.SelectedIndex = 3;
                    break;
                case byTrophy_Ring:
                    cbSaveRuleType.SelectedIndex = 4;
                    break;
                case byTrophy_Amulet:
                    cbSaveRuleType.SelectedIndex = 5;
                    break;
            }
            
            //7个属性
            ClearValueCtrl();
            n = 1;
            foreach(var item in filter.rules)
            {
                switch(n)
                {
                    case 1:
                        tbProperty1.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV11.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV12.Text = item.n2.ToString();
                        break;
                    case 2:
                        tbProperty2.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV21.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV22.Text = item.n2.ToString();
                        break;
                    case 3:
                        tbProperty3.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV31.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV32.Text = item.n2.ToString();
                        break;
                    case 4:
                        tbProperty4.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV41.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV42.Text = item.n2.ToString();
                        break;
                    case 5:
                        tbProperty5.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV51.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV52.Text = item.n2.ToString();
                        break;
                    case 6:
                        tbProperty6.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV61.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV62.Text = item.n2.ToString();
                        break;
                    case 7:
                        tbProperty7.Text = item.strInfo;
                        if (item.n1 >= 0)
                            tbV71.Text = item.n1.ToString();
                        if (item.n2 >= 0)
                            tbV72.Text = item.n2.ToString();
                        break;
                }
                ++n;
            }
        }
        void FlashNameSaveCtrl()
        {
            lbAllSaveName.Items.Clear();
            foreach(var item in data.NameSaveList)
            {
                lbAllSaveName.Items.Add(item);
            }
        }
        private void btnAddSaveName_Click(object sender, EventArgs e)
        {
            CreateFilter dlg = new CreateFilter();
            DialogResult ret=dlg.ShowDialog();
            if (ret != DialogResult.OK)
                return;
            string strNameSaveItem=dlg.FilterName;
            if(data.NameSaveList.Contains(strNameSaveItem))
            {
                MessageBox.Show("已經包含此名稱");
                return;
            }
            data.NameSaveList.Add(strNameSaveItem);
            FlashNameSaveCtrl();
        }

        private void btnDelSaveName_Click(object sender, EventArgs e)
        {
            int n=lbAllSaveName.SelectedIndex;
            if (n < 0)
                return;
            string strDelItem = (string)lbAllSaveName.SelectedItem;
            data.NameSaveList.Remove(strDelItem);
            FlashNameSaveCtrl();
        }
    }
}
