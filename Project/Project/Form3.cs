using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data.Entity;

namespace Project
{
    public partial class Form3 : Form
    {
        Database1Entities context = new Database1Entities();

        List<Covid> Covids;



        Excel.Application xlApp;
        Excel.Workbook xlWB; 
        Excel.Worksheet xlSheet; 

        public Form3()
        {
            InitializeComponent();

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

           

        }

        

    }

    
}
