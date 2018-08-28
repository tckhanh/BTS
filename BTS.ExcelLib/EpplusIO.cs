using BTS.Common;
using BTS.Model.Models;
using BTS.Service;
using OfficeOpenXml;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.Services;
using System.Web.UI;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTS.ExcelLib
{
    // Class ExcelIO tuong duong nhu Class BaseController
    public class EpplusIO
    {
        private IErrorService _errorService;

        public EpplusIO(IErrorService errorService)
        {
            _errorService = errorService;
        }

        public void LogError(Exception e, string description = "")
        {
            try
            {
                WebMsgBox.Show(e.Message);

                Error error = new Error();
                error.Controller = e.Source;
                error.CreatedDate = DateTime.Now;
                error.Message = e.Message;
                error.Description = description;
                error.StackTrace = e.StackTrace;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public DataTable ReadSheet(string fullFileName, string sheetName, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(fullFileName))
            {
                return tbl;
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fullFileName)))
                {
                    // đặt tên người tạo file
                    excelPackage.Workbook.Properties.Author = "VCC2";
                    // đặt tiêu đề cho file
                    excelPackage.Workbook.Properties.Title = "VCC2";

                    foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                    {
                        if (xlWorkSheet.Name == sheetName)
                        {
                            foreach (var firstRowCell in xlWorkSheet.Cells[1, 1, 1, xlWorkSheet.Dimension.End.Column])
                            {
                                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                            }
                            var startRow = hasHeader ? 2 : 1;
                            for (int rowNum = startRow; rowNum <= xlWorkSheet.Dimension.End.Row; rowNum++)
                            {
                                var wsRow = xlWorkSheet.Cells[rowNum, 1, rowNum, xlWorkSheet.Dimension.End.Column];
                                DataRow row = tbl.Rows.Add();
                                foreach (var cell in wsRow)
                                {
                                    row[cell.Start.Column - 1] = cell.Text;
                                }
                            }
                            return tbl;
                        }
                    }
                    return tbl;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
                Trace.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool FormatColumns(string fullFileName, string[] columnNames, string strFormat)
        {
            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(fullFileName))
            {
                return false;
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fullFileName)))
                {
                    // đặt tên người tạo file
                    excelPackage.Workbook.Properties.Author = "VCC2";

                    // đặt tiêu đề cho file
                    excelPackage.Workbook.Properties.Title = "VCC2";

                    foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                    {
                        for (int col = xlWorkSheet.Dimension.Start.Column; col <= xlWorkSheet.Dimension.End.Column; col++)
                        {
                            if (xlWorkSheet.Cells[1, col].Value != null)
                            {
                                if (Array.Exists(columnNames, x => x == xlWorkSheet.Cells[1, col].Value.ToString()))
                                {
                                    xlWorkSheet.Column(col).AutoFit();
                                    xlWorkSheet.Column(col).Style.Numberformat.Format = strFormat;
                                }
                            }
                        }
                    }

                    excelPackage.Save();
                }
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
                Trace.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool AddNewColumns(string fullFileName, string sheetName, string colNames)
        {
            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(fullFileName))
            {
                return false;
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fullFileName)))
                {
                    // đặt tên người tạo file
                    excelPackage.Workbook.Properties.Author = "VCC2";

                    // đặt tiêu đề cho file
                    excelPackage.Workbook.Properties.Title = "VCC2";

                    foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                    {
                        if (xlWorkSheet.Name == sheetName)
                        {
                            int colCount = xlWorkSheet.Dimension.End.Column;

                            string[] columns = colNames.Split(new char[] { ';' });
                            for (int i = 0; i < columns.Length; i++)
                            {
                                xlWorkSheet.Cells[1, colCount + i + 1].Value = columns[i];
                            }
                        }

                        excelPackage.Save();
                    }
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
                Trace.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool UpdateColums(string fullFileName, string sheetName, string updateColNames, string updateValue)
        {
            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(fullFileName))
            {
                return false;
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fullFileName)))
                {
                    // đặt tên người tạo file
                    excelPackage.Workbook.Properties.Author = "VCC2";

                    // đặt tiêu đề cho file
                    excelPackage.Workbook.Properties.Title = "VCC2";

                    foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                    {
                        if (xlWorkSheet.Name == sheetName)
                        {
                            int colCount = xlWorkSheet.Dimension.End.Column;
                            int rowCount = xlWorkSheet.Dimension.End.Row;

                            string[] columns = updateColNames.Split(new char[] { ';' });
                            for (int z = 0; z < columns.Length; z++)
                            {
                                for (int i = 0; i < colCount; i++)
                                {
                                    if (xlWorkSheet.Cells[1, i + 1].Value.ToString() == columns[z])
                                    {
                                        for (int j = 0; j < rowCount; j++)
                                        {
                                            xlWorkSheet.Cells[j + 1, i + 1].Value = updateValue;
                                        }
                                    }
                                }
                            }
                        }
                        excelPackage.Save();
                    }
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
                Trace.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool UpdateCells(string fullFileName, string sheetName, string updateColName, string updateValue, string keyColName, string keyValue)
        {
            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(fullFileName))
            {
                return false;
            }

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fullFileName)))
                {
                    // đặt tên người tạo file
                    excelPackage.Workbook.Properties.Author = "VCC2";

                    // đặt tiêu đề cho file
                    excelPackage.Workbook.Properties.Title = "VCC2";

                    foreach (ExcelWorksheet xlWorkSheet in excelPackage.Workbook.Worksheets)
                    {
                        if (xlWorkSheet.Name == sheetName)
                        {
                            int colCount = xlWorkSheet.Dimension.End.Column;
                            int rowCount = xlWorkSheet.Dimension.End.Row;
                            int indexUpdateCol = -1;
                            int indexKeyCol = -1;

                            for (int i = 0; i < colCount; i++)
                            {
                                if (xlWorkSheet.Cells[1, i + 1].Value.ToString() == updateColName)
                                {
                                    indexUpdateCol = i;
                                }

                                if (xlWorkSheet.Cells[1, i + 1].Value.ToString() == keyColName)
                                {
                                    indexKeyCol = i;
                                }
                            }

                            if (indexUpdateCol >= 0 && indexKeyCol >= 0)
                            {
                                for (int i = 0; i < rowCount; i++)
                                {
                                    if (xlWorkSheet.Cells[i + 1, indexKeyCol + 1].Value.ToString() == keyValue)
                                    {
                                        xlWorkSheet.Cells[i + 1, indexUpdateCol + 1].Value = updateValue;
                                    }
                                }
                            }
                        }
                        excelPackage.Save();
                    }
                    return true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string description = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
                    }
                }

                LogError(ex, description);
                Trace.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                LogError(ex);
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }

        private DataTable BuildHeadersFromFirstRowThenRemoveFirstRow(DataTable dt)
        {
            DataRow firstRow = dt.Rows[0];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(firstRow[i].ToString())) // handle empty cell
                    dt.Columns[i].ColumnName = firstRow[i].ToString().Trim();
            }

            dt.Rows.RemoveAt(0);

            return dt;
        }

        //public void ExportToExcel()
        //{
        //    try
        //    {
        //        SqlConnection cnn;
        //        string connectionString = null;
        //        string sql = null;
        //        string data = null;
        //        int i = 0;
        //        int j = 0;

        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        connectionString = "data source=servername;initial catalog=databasename;user id=username;password=password;";
        //        cnn = new SqlConnection(connectionString);
        //        cnn.Open();
        //        sql = "SELECT * FROM Product";
        //        SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
        //        DataSet ds = new DataSet();
        //        dscmd.Fill(ds);

        //        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //        {
        //            for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
        //            {
        //                data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
        //                xlWorkSheet.Cells[i + 1, j + 1] = data;
        //            }
        //        }

        //        xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }

        //    //MessageBox.Show("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        //}

        //private void AddSheet(string fullFileName, string sheetName)
        //{
        //    try
        //    {
        //        Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

        //        if (xlApp == null)
        //        {
        //            Trace.WriteLine("Excel is not properly installed!!");
        //            return;
        //        }

        //        xlApp.DisplayAlerts = false;
        //        Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(fullFileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
        //        Excel.Sheets worksheets = xlWorkBook.Worksheets;

        //        var xlNewSheet = (Excel.Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
        //        xlNewSheet.Name = sheetName;

        //        xlWorkBook.Save();
        //        xlWorkBook.Close();

        //        releaseObject(xlNewSheet);
        //        releaseObject(worksheets);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        Trace.WriteLine("New Worksheet Created!");
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void DeleteSheet(string fullFileName, string sheetName)
        //{
        //    try
        //    {
        //        Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

        //        if (xlApp == null)
        //        {
        //            Trace.WriteLine("Excel is not properly installed!!");
        //            return;
        //        }

        //        xlApp.DisplayAlerts = false;
        //        Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(fullFileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
        //        Excel.Sheets worksheets = xlWorkBook.Worksheets;
        //        worksheets[sheetName].Delete();
        //        xlWorkBook.Save();
        //        xlWorkBook.Close();

        //        releaseObject(worksheets);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        Trace.WriteLine("Worksheet Deleted!");
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //Format Excel cells to store values as text

        //Formating Excel cells to text format will solve the problem of losing leading zeo values when you export data from other data sources to excel.

        //    Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("a1", "b1");
        //    formatRange.NumberFormat = "@";
        //    xlWorkSheet.Cells[1, 1] = "098";
        //Excel Number Formatting

        //    Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("a1", "b1");
        //    formatRange.NumberFormat = "#,###,###";
        //    xlWorkSheet.Cells[1, 1] = "1234567890";
        //Excel Currency Formatting

        //    Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("a1", "b1");
        //    formatRange.NumberFormat = "$ #,###,###.00";
        //    xlWorkSheet.Cells[1, 1] = "1234567890";
        //Excel Date Formatting

        //    Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("a1", "b1");
        //    formatRange.NumberFormat = "mm/dd/yyyy";
        //    //formatRange.NumberFormat = "mm/dd/yyyy hh:mm:ss";
        //    xlWorkSheet.Cells[1, 1] = "31/5/2014";
        //Bold the fonts of a specific row or cell

        //Bold entire row

        //    Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("a1");
        //    formatRange.EntireRow.Font.Bold = true;
        //    xlWorkSheet.Cells[1, 5] = "Bold";
        //Bold specific cell

        //    workSheet.Cells[2, 1].Font.Bold = true;
        //        Add border to a specific cell

        //    Excel.Range formatRange = xlWorkSheet.UsedRange;
        //        Excel.Range cell = formatRange.Cells[3, 3];
        //        Excel.Borders border = cell.Borders;
        //        border.LineStyle = Excel.XlLineStyle.xlContinuous;
        //    border.Weight = 2d;
        //Border around multiple cells in excel

        //Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("b2", "e9");
        //formatRange.BorderAround(Excel.XlLineStyle.xlContinuous,
        //Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic,
        //Excel.XlColorIndex.xlColorIndexAutomatic);
        //Excel Cell coloring

        //Cell background color

        //Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("b1", "b1");
        //formatRange.Interior.Color = System.Drawing.
        //ColorTranslator.ToOle(System.Drawing.Color.Red);
        //xlWorkSheet.Cells[1, 2] = "Red";
        //Cell font color , size

        //Excel.Range formatRange;
        //        formatRange = xlWorkSheet.get_Range("b1", "b1");
        //formatRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //formatRange.Font.Size  = 10;
        //xlWorkSheet.Cells[1, 2] = "Red";
        //Excel Styles to named range

        //Excel.Style myStyle = Globals.ThisWorkbook.Styles.Add("myStyle");
        //        myStyle.Font.Name = "Verdana";
        //myStyle.Font.Size = 12;
        //myStyle.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //myStyle.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
        //myStyle.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
        //How to merge Excel cells

        //    xlWorkSheet.get_Range("b2", "e3").Merge(false);
        //        Adding Custom header to the excel file

        //            xlWorkSheet.get_Range("b2", "e3").Merge(false);
        //        chartRange = xlWorkSheet.get_Range("b2", "e3");
        //	chartRange.FormulaR1C1 = "Your Heading Here";
        //	chartRange.HorizontalAlignment = 3;
        //	chartRange.VerticalAlignment = 3;

        // The following C# program create a mark list in Excel file and format the cells.
        // First we MERGE excel cell and create the heading , then the students name and totals make as BOLD .
        // And finally create a border for the whole mark list part.

        //private void CreateMarkList()
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;
        //    Excel.Range chartRange;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    //add data
        //    xlWorkSheet.Cells[4, 2] = "";
        //    xlWorkSheet.Cells[4, 3] = "Student1";
        //    xlWorkSheet.Cells[4, 4] = "Student2";
        //    xlWorkSheet.Cells[4, 5] = "Student3";

        //    xlWorkSheet.Cells[5, 2] = "Term1";
        //    xlWorkSheet.Cells[5, 3] = "80";
        //    xlWorkSheet.Cells[5, 4] = "65";
        //    xlWorkSheet.Cells[5, 5] = "45";

        //    xlWorkSheet.Cells[6, 2] = "Term2";
        //    xlWorkSheet.Cells[6, 3] = "78";
        //    xlWorkSheet.Cells[6, 4] = "72";
        //    xlWorkSheet.Cells[6, 5] = "60";

        //    xlWorkSheet.Cells[7, 2] = "Term3";
        //    xlWorkSheet.Cells[7, 3] = "82";
        //    xlWorkSheet.Cells[7, 4] = "80";
        //    xlWorkSheet.Cells[7, 5] = "65";

        //    xlWorkSheet.Cells[8, 2] = "Term4";
        //    xlWorkSheet.Cells[8, 3] = "75";
        //    xlWorkSheet.Cells[8, 4] = "82";
        //    xlWorkSheet.Cells[8, 5] = "68";

        //    xlWorkSheet.Cells[9, 2] = "Total";
        //    xlWorkSheet.Cells[9, 3] = "315";
        //    xlWorkSheet.Cells[9, 4] = "299";
        //    xlWorkSheet.Cells[9, 5] = "238";

        //    xlWorkSheet.get_Range("b2", "e3").Merge(false);

        //    chartRange = xlWorkSheet.get_Range("b2", "e3");
        //    chartRange.FormulaR1C1 = "MARK LIST";
        //    chartRange.HorizontalAlignment = 3;
        //    chartRange.VerticalAlignment = 3;
        //    chartRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        //    chartRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //    chartRange.Font.Size = 20;

        //    chartRange = xlWorkSheet.get_Range("b4", "e4");
        //    chartRange.Font.Bold = true;
        //    chartRange = xlWorkSheet.get_Range("b9", "e9");
        //    chartRange.Font.Bold = true;

        //    chartRange = xlWorkSheet.get_Range("b2", "e9");
        //    chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

        //    xlWorkBook.SaveAs("d:\\csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlApp);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlWorkSheet);

        //    Trace.WriteLine("File created !");
        //}

        //private void InsertPicture()
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    //add some text
        //    xlWorkSheet.Cells[1, 1] = "http://csharp.net-informations.com";
        //    xlWorkSheet.Cells[2, 1] = "Adding picture in Excel File";

        //    xlWorkSheet.Shapes.AddPicture("C:\\csharp-xl-picture.JPG", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 50, 50, 300, 45);

        //    xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlApp);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlWorkSheet);

        //    Trace.WriteLine("File created !");
        //}

        //private void InsertBackgroundPicture()
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    xlWorkSheet.SetBackgroundPicture("C:\\csharp-xl-picture.JPG");

        //    //add some text
        //    xlWorkSheet.Cells[1, 1] = "http://csharp.net-informations.com";
        //    xlWorkSheet.Cells[2, 1] = "Adding background in Excel File";

        //    xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlApp);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlWorkSheet);

        //    Trace.WriteLine("File created !");
        //}

        //private void CreateChart()
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    //add data
        //    xlWorkSheet.Cells[1, 1] = "";
        //    xlWorkSheet.Cells[1, 2] = "Student1";
        //    xlWorkSheet.Cells[1, 3] = "Student2";
        //    xlWorkSheet.Cells[1, 4] = "Student3";

        //    xlWorkSheet.Cells[2, 1] = "Term1";
        //    xlWorkSheet.Cells[2, 2] = "80";
        //    xlWorkSheet.Cells[2, 3] = "65";
        //    xlWorkSheet.Cells[2, 4] = "45";

        //    xlWorkSheet.Cells[3, 1] = "Term2";
        //    xlWorkSheet.Cells[3, 2] = "78";
        //    xlWorkSheet.Cells[3, 3] = "72";
        //    xlWorkSheet.Cells[3, 4] = "60";

        //    xlWorkSheet.Cells[4, 1] = "Term3";
        //    xlWorkSheet.Cells[4, 2] = "82";
        //    xlWorkSheet.Cells[4, 3] = "80";
        //    xlWorkSheet.Cells[4, 4] = "65";

        //    xlWorkSheet.Cells[5, 1] = "Term4";
        //    xlWorkSheet.Cells[5, 2] = "75";
        //    xlWorkSheet.Cells[5, 3] = "82";
        //    xlWorkSheet.Cells[5, 4] = "68";

        //    Excel.Range chartRange;

        //    Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
        //    Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(10, 80, 300, 250);
        //    Excel.Chart chartPage = myChart.Chart;

        //    chartRange = xlWorkSheet.get_Range("A1", "d5");
        //    chartPage.SetSourceData(chartRange, misValue);
        //    chartPage.ChartType = Excel.XlChartType.xlColumnClustered;

        //    //export chart as picture file
        //    chartPage.Export(@"C:\excel_chart_export.bmp", "BMP", misValue);

        //    xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlWorkSheet);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlApp);

        //    Trace.WriteLine("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        //}

        //private void Validation()
        //{
        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    Excel.Range chartRange;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    chartRange = xlWorkSheet.get_Range("b2", "e9");
        //    xlWorkSheet.get_Range("B5", "D5").Validation.Add(Excel.XlDVType.xlValidateInputOnly, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, misValue, misValue);
        //    xlWorkSheet.get_Range("B5", "D5").Validation.IgnoreBlank = true;
        //    xlWorkSheet.get_Range("B5", "B5").FormulaR1C1 = "Click Here to see Notes";
        //    xlWorkSheet.get_Range("B5", "D5").Validation.InputTitle = "csharp.net-informations.com";
        //    xlWorkSheet.get_Range("B5", "D5").Validation.ErrorTitle = "Error in Title";
        //    xlWorkSheet.get_Range("B5", "D5").Validation.InputMessage = "Here is the notes embeded - you can enter 255 characters maximum in notes ";
        //    xlWorkSheet.get_Range("B5", "D5").Validation.ErrorMessage = "Error in Notes";
        //    xlWorkSheet.get_Range("B5", "D5").Validation.ShowInput = true;
        //    xlWorkSheet.get_Range("B5", "D5").Validation.ShowError = true;

        //    xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlWorkSheet);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlApp);

        //    Trace.WriteLine("Excel file created , you can find the file c:\\csharp-Excel.xls");
        //}

        //private void ReadDataToDataSet()
        //{
        //    try
        //    {
        //        OleDbConnection MyConnection;
        //        DataSet DtSet;
        //        OleDbDataAdapter MyCommand;
        //        MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='c:\\csharp.net-informations.xls';Extended Properties=Excel 8.0;");
        //        MyCommand = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
        //        MyCommand.TableMappings.Add("Table", "TestTable");
        //        DtSet = new DataSet();
        //        MyCommand.Fill(DtSet);
        //        //dataGridView1.DataSource = DtSet.Tables[0];
        //        MyConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void InsertDataByOleDB()
        //{
        //    try
        //    {
        //        OleDbConnection MyConnection;
        //        OleDbCommand myCommand = new OleDbCommand();
        //        string sql = null;
        //        using (MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='c:\\csharp.net-informations.xls';Extended Properties=Excel 8.0;"))
        //        {
        //            MyConnection.Open();
        //            myCommand.Connection = MyConnection;
        //            sql = "Insert into [Sheet1$] (id,name) values('5','e')";
        //            myCommand.CommandText = sql;
        //            myCommand.ExecuteNonQuery();
        //            MyConnection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //public void ClearDataInSheet(string excelConnectionString, String SheetName)
        //{
        //    try
        //    {
        //        OleDbConnection MyConnection;
        //        OleDbCommand myCommand = new OleDbCommand();
        //        string sql = null;
        //        using (MyConnection = new OleDbConnection(excelConnectionString))
        //        {
        //            MyConnection.Open();
        //            myCommand.Connection = MyConnection;
        //            sql = "DELETE FROM [" + SheetName + "$]";
        //            myCommand.CommandText = sql;
        //            myCommand.ExecuteNonQuery();
        //            MyConnection.Close();
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //public void InsertDataInSheet(string excelConnectionString, String SheetName, DataTable dt)
        //{
        //    try
        //    {
        //        OleDbConnection MyConnection;
        //        OleDbCommand myCommand = new OleDbCommand();
        //        string sql = null;
        //        using (MyConnection = new OleDbConnection(excelConnectionString))
        //        {
        //            MyConnection.Open();
        //            myCommand.Connection = MyConnection;

        //            sql = "Insert into [" + SheetName + "]$ (id,name) values('5','e')";
        //            myCommand.CommandText = sql;
        //            myCommand.ExecuteNonQuery();

        //            MyConnection.Close();
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void UpdateDataByOleDB()
        //{
        //    try
        //    {
        //        OleDbConnection MyConnection;
        //        OleDbCommand myCommand = new OleDbCommand();
        //        string sql = null;
        //        using (MyConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='c:\\csharp.net-informations.xls';Extended Properties=Excel 8.0;"))
        //        {
        //            MyConnection.Open();
        //            myCommand.Connection = MyConnection;
        //            sql = "Update [Sheet1$] set name = 'New Name' where id=1";
        //            myCommand.CommandText = sql;
        //            myCommand.ExecuteNonQuery();
        //            MyConnection.Close();
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void ImportFromDatabase()
        //{
        //    try
        //    {
        //        SqlConnection cnn;
        //        string connectionString = null;
        //        string sql = null;
        //        string data = null;
        //        int i = 0;
        //        int j = 0;

        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        connectionString = "data source=servername;initial catalog=databasename;user id=username;password=password;";
        //        using (cnn = new SqlConnection(connectionString))
        //        {
        //            cnn.Open();
        //            sql = "SELECT * FROM Product";
        //            SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
        //            DataSet ds = new DataSet();
        //            dscmd.Fill(ds);

        //            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //            {
        //                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
        //                {
        //                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
        //                    xlWorkSheet.Cells[i + 1, j + 1] = data;
        //                }
        //            }

        //            xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //            xlWorkBook.Close(true, misValue, misValue);
        //            xlApp.Quit();

        //            releaseObject(xlWorkSheet);
        //            releaseObject(xlWorkBook);
        //            releaseObject(xlApp);

        //            Trace.WriteLine("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void ImportFromGridView(DataTable dataGridView1)
        //{
        //    try
        //    {
        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //        int i = 0;
        //        int j = 0;

        //        for (i = 0; i <= dataGridView1.Rows.Count - 1; i++)
        //        {
        //            for (j = 0; j <= dataGridView1.Columns.Count - 1; j++)
        //            {
        //                string cell = dataGridView1.Rows[i].ItemArray[j].ToString();
        //                xlWorkSheet.Cells[i + 1, j + 1] = cell;
        //            }
        //        }

        //        xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        Trace.WriteLine("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string description = "";
        //        foreach (var eve in ex.EntityValidationErrors)
        //        {
        //            Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //            description += ($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.\n");
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                description += ($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n");
        //            }
        //        }

        //        LogError(ex, description);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        Trace.WriteLine(ex.ToString());
        //    }
        //}

        //private void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        obj = null;
        //        Trace.WriteLine("Exception Occured while releasing object " + ex.ToString());
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}
    }
}