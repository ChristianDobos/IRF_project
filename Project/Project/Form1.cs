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
        int számláló = 30;

       

        List<User> Users = new List<User>();

        public Form1()
        {
            InitializeComponent();

            

            User user1 = new User();
            user1.Username = "User1"; user1.Password = "Password1"; user1.aktív = false; Users.Add(user1);

            User user2 = new User();
            user2.Username = "User2"; user2.Password = "Password2"; user2.aktív = true; Users.Add(user2);

            User user3 = new User();
            user3.Username = "User3"; user3.Password = "Password3";user3.aktív = true;Users.Add(user3);

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
            
            
            bool beléphet = false;
            if (próbálkozás < 3)
            {
                foreach (User u in Users)
                {
                    if (textBox1.Text == u.Username.ToString() & textBox2.Text == u.Password.ToString() & u.aktív == true)
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
                    MessageBox.Show("Hibás felhasználónév vagy jelszó / Inaktív falhasználó");
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
            if (számláló == 0)
            {
                t.Stop();
                számláló = 30;
                próbálkozás = 1;
                textBox1.Enabled = true; textBox2.Enabled = true;
                button1.Enabled = true; checkBox1.Enabled = true;
                label3.Visible = false;
            }
        }


    }
    
    //UNIT TEST
    public class Belépő
    {
        public User Felhasználó { get; set; }
        public bool beléphete(User user)
        {
            if (user.aktív==true)
            {
                return true;
            }
            return false;
        }

    }

    public class User
    {
        public bool aktív { get; set; }
        public string Username;
        public string Password;
    }
}
