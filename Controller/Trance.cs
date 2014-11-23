using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.GameCall;
namespace Controller
{
    public partial class Trance : Form
    {
        public Trance()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int MapID = int.Parse(textBox1.Text);
            //int WaypointID = int.Parse(textBox2.Text);
            //Program.client.Transport(MapID, WaypointID, 0);
            int Key = int.Parse(textBox1.Text);
            Program.client.HitKey(Key);
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  int nRet = Program.client.GetNearbyWaypointID();
          //  textBox2.Text = nRet.ToString();
           PlayerInfo player= Program.client.GetPlayerInfo();
           textBox2.Text = player.HP.ToString() + "/" + player.MaxHP.ToString();
        }
    }
}
