using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTS.ExcelLib
{
    public static class ExcelIO
    {
        public static DataSet ReadSheet(string excelConnectionString, string sheetName)
        {
            DataSet ds = new DataSet();

            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);

            // Read Data from Sheet
            string query = string.Format("Select * from [{0}]", sheetName + "$");

            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
            {
                dataAdapter.Fill(ds, sheetName);
            }
            excelConnection.Close();
            return ds;
        }

        public static void AddNewColumns(string fullFileName, string sheetName, string colNames)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range rng;
            object misValue = System.Reflection.Missing.Value;

            try
            {
                xlApp = new Excel.Application();
                xlApp.Visible = false;
                xlWorkBook = xlApp.Workbooks.Open(fullFileName, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);   //@"H:\TestFile.xlsx"
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[sheetName];
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets.get_Item(sheetNum);
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(sheetNum);
                rng = xlWorkSheet.UsedRange;

                int colCount = rng.Columns.Count;
                int rowCount = rng.Rows.Count;
                rng = (Excel.Range)xlWorkSheet.Cells[rowCount, colCount];
                Excel.Range newColumn = rng.EntireColumn;

                string[] columns = colNames.Split(new Char[] { ';' });
                for (int i = 0; i < columns.Length; i++)
                {
                    xlWorkSheet.Cells[1, colCount + i + 1] = columns[i];
                }

                //save and quit
                //xlWorkBook.SaveAs(@"H:\TestFile.xlsx", misValue, misValue, misValue, misValue, misValue,
                //    Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Save();
                xlWorkBook.Close(misValue, misValue, misValue);
                xlApp.Quit();

                // release all the application object from the memory
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rng);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}