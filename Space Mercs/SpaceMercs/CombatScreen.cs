using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceMercs
{
    public partial class CombatScreen : Form
    {
        public CombatScreen()
        {
            InitializeComponent();

            Bitmap ship1;
            ship1.FromFile("..\\Resources\\Ship1.jpg", true);


        }
    }
}
