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
using System.Xml;


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
            

            chart1.Visible = false;
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Covid' table. You can move, or remove it, as needed.
            this.covidTableAdapter.Fill(this.database1DataSet.Covid);
            dataGridView1.Visible = false;
            bindingNavigator1.Visible = false;

        }
        //STATISZTIKA
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible==false)
            {
                dataGridView1.Visible = true;
                bindingNavigator1.Visible = true;
                chart1.Visible = false;
            }
            else
            {
                dataGridView1.Visible = false;
                bindingNavigator1.Visible = false;
            }  
            
            
        }
        //CSV EXPORT
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                foreach (var x in context.Covid)
                {
                    sw.Write(x.Date.ToString());
                    sw.Write(";");
                    sw.Write(x.Cases.ToString());
                    sw.Write(";");
                    sw.Write(x.Deaths.ToString());
                    sw.Write(";");
                    sw.Write(x.Population.ToString());
                    sw.Write(";");
                    sw.Write(x.Commulative.ToString());
                    sw.WriteLine();
                }
            }
        }
        //ÚJ ELEM
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            covidBindingSource.EndEdit();
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            dataGridView1.Refresh();
        }
        //XML IMPORT
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.DefaultExt = ".xml";
            ofd.AddExtension = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(ofd.FileName);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Refresh();
            }
            else
            {
                return;
            }



        }

        //GRAFIKON
        private void button2_Click(object sender, EventArgs e)
        {
            if (chart1.Visible==false)
            {
                chart1.Visible = true;
                dataGridView1.Visible = false; bindingNavigator1.Visible = false;
            }
            else
            {
                chart1.Visible = false;
            }
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
    }


    public class GrafikonEsetek
    {
        public string Dátum { get; set; }
        public int? Esetek { get; set; }
    }

    public class GrafikonHalálok
    {
        public string Dátum { get; set; }
        public int? Halálok { get; set; }
    }
}
