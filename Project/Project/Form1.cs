using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.Entity;


namespace Project
{
    public partial class Form1 : Form
    {
        int próbálkozás = 1;
        Timer t = new Timer();
        int számláló=30;

        Database1Entities context = new Database1Entities();
        List<Login> Logins;

        public Form1()
        {
            InitializeComponent();

            BindingList<Login> logins = new BindingList<Login>();
            context.Login.Load();
            

            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            label3.Visible = false;
            label3.Text = számláló.ToString();
            label3.ForeColor = Color.Red;
            
        }
        //CHECKBOX
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        

        //LOGIN
        private void button1_Click(object sender, EventArgs e)
        {
            Logins = context.Login.ToList();
            bool beléphet = false;
            if (próbálkozás < 3)
            {
                foreach (Login l in Logins)
                {
                    if (textBox1.Text == l.Username.ToString() & textBox2.Text == l.Password.ToString())
                    {
                        beléphet = true;
                    }

                }

                if (beléphet == true)
                {
                    Form2 f2 = new Form2();
                    f2.Show();
                }
                else
                {
                    MessageBox.Show("Hibás felhasználónév vagy jelszó");
                    próbálkozás++;
                }
            }
            else
            {
                t.Start();
                t.Interval = 1000;
                t.Tick += T_Tick;
                textBox1.Enabled = false; textBox2.Enabled = false;
                button1.Enabled = false; checkBox1.Enabled = false;
                MessageBox.Show("Újra próbálkozhat " + számláló + " másodperc múlva");


            }

        }
        //TIMER
        private void T_Tick(object sender, EventArgs e)
        {
            label3.Visible = true;
            számláló--;
            label3.Text = számláló.ToString();
            if (számláló==0)
            {
                t.Stop();
                számláló = 30;
                próbálkozás = 1;
                textBox1.Enabled = true; textBox2.Enabled = true;
                button1.Enabled = true;  checkBox1.Enabled = true;
                label3.Visible = false;
            }
        }

        
    }
}
