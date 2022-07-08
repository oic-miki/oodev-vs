using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VendingMachine
{
    public partial class Form2 : Form
    {

        private Form1 form1;

        public Form2(Form1 form1)
        {

            InitializeComponent();

            this.form1 = form1;

        }

        public Label getLabel()
        {

            return label1;

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            form1.refund();

        }
    }
}
