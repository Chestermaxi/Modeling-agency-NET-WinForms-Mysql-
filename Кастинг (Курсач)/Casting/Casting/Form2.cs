using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casting
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            form.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form = new Form6();
            form.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form8 form = new Form8();
            form.Show();
            Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form10 form = new Form10();
            form.Show();
            Hide();
        }
    }
}
