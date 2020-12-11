using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;
using System.Diagnostics;

namespace Project
{
    public partial class Form2 : Form
    {
        Database1Entities context = new Database1Entities();
        public Form2()
        {
            InitializeComponent();

            BindingList<Covid> covids = new BindingList<Covid>();
            context.Covid.Load();
            database1DataSetBindingSource.DataSource = context.Covid.Local;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Covid' table. You can move, or remove it, as needed.
            this.covidTableAdapter.Fill(this.database1DataSet.Covid);
            dataGridView1.Visible = false;
            bindingNavigator1.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible==false)
            {
                dataGridView1.Visible = true;
                bindingNavigator1.Visible = true;
            }
            else
            {
                dataGridView1.Visible = false;
                bindingNavigator1.Visible = false;
            }  
            
            
        }
    }
}
