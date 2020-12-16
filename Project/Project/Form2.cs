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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;



namespace Project
{
    public partial class Form2 : Form
    {
        Database1Entities context = new Database1Entities();
        List<Covid> Covids;

        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        public Form2()
        {
            InitializeComponent();

            BindingList<Covid> covids = new BindingList<Covid>();
            context.Covid.Load();
            database1DataSetBindingSource.DataSource = context.Covid.Local;
            

            chart1.Visible = false;
            label1.Visible = false; label2.Visible = false;label3.Visible = false;textBox1.Visible = false;textBox2.Visible = false; buttonR.Visible = false;

            
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
                label1.Visible = false; label2.Visible = false; label3.Visible = false; textBox1.Visible = false; textBox2.Visible = false; buttonR.Visible = false;
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
                label1.Visible = false; label2.Visible = false; label3.Visible = false; textBox1.Visible = false; textBox2.Visible = false; buttonR.Visible = false;
            }
            else
            {
                chart1.Visible = false;
            }
         
        }
        //EXCEL GENERÁLÁS
        private void button3_Click(object sender, EventArgs e)
        {
            LoadData();

            CreateExcel();
        }

        public void LoadData()
        {
            Covids = context.Covid.ToList();
        }

        // SEGÉDFÜGGVÉNY EXCEL KOORDINÁTÁK MEGHATÁROZÁSÁHOZ
        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        //CREATEEXCEL
        public void CreateExcel()
        {

            try
            {

                xlApp = new Excel.Application();



                xlWB = xlApp.Workbooks.Add(Missing.Value);


                xlSheet = xlWB.ActiveSheet;


                CreateTable();


                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");


                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        //CREATETABLE
        public void CreateTable()
        {

            xlSheet.Cells[1, 1] = "Date";
            xlSheet.Cells[1, 2] = "Cases";
            xlSheet.Cells[1, 3] = "Deaths";
            xlSheet.Cells[1, 4] = "Population";
            xlSheet.Cells[1, 5] = "Commulative";

            object[,] values = new object[Covids.Count, 5];

            int counter = 0;
            foreach (Covid c in Covids)
            {
                values[counter, 0] = c.Date;
                values[counter, 1] = c.Cases;
                values[counter, 2] = c.Deaths;
                values[counter, 3] = c.Population;
                values[counter, 4] = c.Commulative;
                counter++;
            }

            xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;

            int LastRowID = (xlSheet.UsedRange.Rows.Count + 1);


            //DIAGRAM
            object misValue = System.Reflection.Missing.Value;

            Excel.Range chartrange;
            Excel.Range chartrange2;
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);

            Excel.ChartObjects xlCharts2 = (Excel.ChartObjects)xlSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart2 = (Excel.ChartObject)xlCharts2.Add(10, 80, 300, 250);

            Excel.Chart chartPage = myChart.Chart;
            Excel.Chart chartPage2 = myChart2.Chart;

            chartrange = xlSheet.get_Range(GetCell(1, 2), GetCell(LastRowID, 2));
            chartrange2 = xlSheet.get_Range(GetCell(1, 3), GetCell(LastRowID, 3));
            chartPage.SetSourceData(chartrange, misValue); chartPage2.SetSourceData(chartrange2, misValue);

            
        }
        //R számítás
        private void buttonR_Click(object sender, EventArgs e)
        {
           

            double rt0 = double.Parse(textBox1.Text);
            double rt2 = double.Parse(textBox2.Text);

            if (rt0!=0)
            {
                label1.Text = (rt2 / rt0).ToString();
            }
            else
            {
                MessageBox.Show("R értéke t időpontban nem lehet 0");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label1.Visible==false)
            {
                label1.Visible = true;label2.Visible = true;label3.Visible = true;buttonR.Visible = true;textBox1.Visible = true;textBox2.Visible = true;
            }
            else
            {
                label1.Visible = false; label2.Visible = false; label3.Visible = false; textBox1.Visible = false; textBox2.Visible = false; buttonR.Visible = false;
            }
        }
    }

    
   
}
