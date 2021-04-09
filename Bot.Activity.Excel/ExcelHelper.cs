
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace Bot.Activity.Excel
//{
//    internal sealed class ExcelHelper : IDisposable
//    {
//        private const string EXCEL_PROG_ID = "Excel.Application";

//        private const uint RPC_SERVER_UNAVAILABLE = 0x800706BA;

//        public static ExcelHelper Shared = new ExcelHelper();

//        private dynamic App { get; set; }

//        public dynamic GetApp()
//        {
//            if (this.App == null)
//            {
//                try
//                {
//                    if (Type.GetTypeFromProgID(EXCEL_PROG_ID) is Type excelType)
//                    {
//                        this.App = Activator.CreateInstance(excelType);
//                    }
//                    else
//                    {
//                        throw new ExcelNotFoundException();
//                    }
//                }
//                catch
//                {
//                    throw;
//                }
//            }
//            else
//            {
//                try
//                {
//                    var version = this.App.Version;
//                }
//                catch
//                {
//                    this.Dispose();
//                    return this.GetApp();
//                }
//            }
//            this.App.Visible = true;
//            return this.App;
//        }

//        public dynamic GetWorkbookByName(string wbName, bool throwIfNotFound)
//        {

//            try
//            {
//                var xlWb = this.GetApp().Workbooks.Item(wbName);

//                if (xlWb == null)
//                {
//                    if (throwIfNotFound)
//                    {
//                        throw new ExcelWorkbookNotFoundException();
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//                xlWb.Activate();
//                return xlWb;
//            }
//            catch(Exception ex)
//            {
//                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
//            }
//            return null;
//        }

//        public dynamic GetWorksheetByName(string wbName, string wsName, bool throwIfNotFound)
//        {
//            try
//            {
//                var xlWs = this.GetWorkbookByName(wbName, true).Worksheets.Item(wsName);
//                if (xlWs == null)
//                {
//                    if (throwIfNotFound)
//                    {
//                        throw new ExcelWorksheetNotFoundException();
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//                xlWs.Activate();
//                return xlWs;
//            }
//            catch (Exception ex)
//            {

//                if (ex.HResult == -2147352565)
//                {

//                }
//                else
//                {
//                    Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
//                }

//            }
//            return null;
//        }

//        public dynamic GetRange(string wbName, string wsName, string rangeAddr)
//        {
//            try
//            {
//                return ExcelHelper.Shared.GetWorksheetByName(wbName, wsName, true).Range(rangeAddr);
//            }
//            catch (Exception ex)
//            {
//                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
//            }
//            return null;
//        }

//        private ExcelHelper()
//        {
//            ;
//        }

//        public void Dispose()
//        {
//            try
//            {
//                this.App.Quit();
//            }
//            catch
//            {

//            }
//            try
//            {
//                Marshal.FinalReleaseComObject(this.App);
//            }
//            catch (COMException ex)
//            {
//                switch ((uint)ex.ErrorCode)
//                {
//                    // 
//                    case RPC_SERVER_UNAVAILABLE:
//                        break;

//                    default:
//                        throw;
//                }
//            }

//            this.App = null;
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//        }
//    }

//    public abstract class ExcelException : Exception
//    {

//    }

//    public sealed class ExcelNotFoundException : ExcelException
//    {

//    }

//    public sealed class ExcelWorkbookNotFoundException : ExcelException
//    {

//    }

//    public sealed class ExcelWorksheetNotFoundException : ExcelException
//    {

//    }
//}

// <copyright file=ExcelHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>

using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using MOIExcel = Microsoft.Office.Interop.Excel;


namespace Bot.Activity.Excel
{
    internal sealed class ExcelHelper : IDisposable
    {
        private const string EXCEL_PROG_ID = "Excel.Application";

        private const uint RPC_SERVER_UNAVAILABLE = 0x800706BA;

        public static ExcelHelper Shared = new ExcelHelper();

        // private dynamic App { get; set; }
        public dynamic App { get; set; }
        public dynamic GetApp(bool excelFileVisible)
        {
            if (this.App == null)
            {
                try
                {
                    if (Type.GetTypeFromProgID(EXCEL_PROG_ID) is Type excelType)
                    {
                        this.App = Activator.CreateInstance(excelType);
                    }
                    else
                    {
                        throw new ExcelNotFoundException();
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    var version = this.App.Version;
                }
                catch
                {
                    this.Dispose();
                    return this.GetApp(excelFileVisible);
                }
            }
            if (true == excelFileVisible)
            {
                this.App.Visible = true;
            }
            else
            {
                this.App.Visible = false;
            }

            return this.App;
        }

        public void Close_OpenedFile(String workbookFullName)
        {
            FileStream stream = null;
            
            try
            {
                stream = File.Open(workbookFullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                String workbookName = Path.GetFileName(workbookFullName);
                this.Close_OpenedWorkbook(workbookName);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                    
            }
        }
        public void Close_OpenedWorkbook(String workbookName)
        {
           
            try
            {
                MOIExcel.Application oExcelApp = null;
                MOIExcel.Workbook wbkOriginal = null;
                int ExcelCount = 0;

                oExcelApp = (MOIExcel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                ExcelCount = oExcelApp.Workbooks.Count;

                if (ExcelCount != 0)
                {
                
                    foreach (MOIExcel.Workbook wbk in oExcelApp.Workbooks)
                    {
                        if (wbk.Name == workbookName)
                        {
                            wbkOriginal = wbk;
                            wbk.Close(SaveChanges: false);
                            break;
                        }
                    }
                    if (1 == ExcelCount && wbkOriginal != null)
                    {
                        oExcelApp.Quit();
                        Marshal.FinalReleaseComObject(oExcelApp);  
                        oExcelApp = null;
                    }
                }
                if (ExcelCount == 0)
                {
                    oExcelApp.Quit();
                    Marshal.FinalReleaseComObject(oExcelApp);  
                    oExcelApp = null;
                }
                // Marshal.ReleaseComObject(oExcelApp);
                //Marshal.FinalReleaseComObject(oExcelApp);
                //oExcelApp = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
               
            }
            catch (Exception ex)
            {
            }
        }
        public string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;



            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }
        //public dynamic GetWorkbookByName(string wbName, bool throwIfNotFound)
        //      {

        //          try
        //          {
        //              var xlWb = this.GetApp(false).Workbooks.Item(wbName);


        //              if (xlWb == null)
        //              {
        //                  if (throwIfNotFound)
        //                  {
        //                      throw new ExcelWorkbookNotFoundException();
        //                  }
        //                  else
        //                  {
        //                      return null;
        //                  }
        //              }
        //              xlWb.Activate();
        //              return xlWb;
        //          }
        //          catch (Exception ex)
        //          {
        //              Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
        //          }
        //          return null;
        //      }

        public dynamic GetWorkbookByName(string wbName, bool throwIfNotFound)
        {
            try
            {
                var xlWb = this.GetApp(false).Workbooks.Item(wbName);



                if (xlWb == null)
                {
                    if (throwIfNotFound)
                    {
                        throw new ExcelWorkbookNotFoundException();
                    }
                    else
                    {
                        return null;
                    }
                }
                xlWb.Activate();
                return xlWb;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147352565)
                { }
                else
                {
                    Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
                }
            }
            return null;
        }



        public int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }
        public object[,] ConvertDataTableToArray(DataTable dt)
        {
            var ret = Array.CreateInstance(typeof(object), dt.Rows.Count, dt.Columns.Count) as object[,];
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    ret[i, j] = dt.Rows[i][j];
                }
            }
            return ret;
        }




        public DataTable Convert2DArraytoDatatable(Object[,] numbers)
        {
            DataTable dt = new DataTable();
            int colCount = numbers.GetLength(1);
            int rowCount = numbers.GetLength(0);



            dt = new DataTable();
            int noofrow = 1;



            for (int c = 1; c <= colCount; c++)
            {
                dt.Columns.Add("Column" + (c));
                noofrow = 1;
            }



            for (int i = noofrow; i <= rowCount; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    dr[j - 1] = numbers[i, j];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


        public dynamic GetApp()
        {
            if (this.App == null)
            {
                try
                {
                    if (Type.GetTypeFromProgID(EXCEL_PROG_ID) is Type excelType)
                    {
                        this.App = Activator.CreateInstance(excelType);
                    }
                    else
                    {
                        throw new ExcelNotFoundException();
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    var version = this.App.Version;
                }
                catch
                {
                    this.Dispose();
                    return this.GetApp();
                }
            }
            this.App.Visible = true;
            return this.App;
        }


        //public dynamic GetWorkbookByName(string wbName, bool throwIfNotFound)
        //{

        //    try
        //    {
        //        var xlWb = this.GetApp(false).Workbooks.Item(wbName);

        //        if (xlWb == null)
        //        {
        //            if (throwIfNotFound)
        //            {
        //                throw new ExcelWorkbookNotFoundException();
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        xlWb.Activate();
        //        return xlWb;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
        //    }
        //    return null;
        //}

        public dynamic GetWorksheetByName(string wbName, string wsName, bool throwIfNotFound)
        {
            try
            {
                var xlWs = this.GetWorkbookByName(wbName, true).Worksheets.Item(wsName);
                if (xlWs == null)
                {
                    if (throwIfNotFound)
                    {
                        throw new ExcelWorksheetNotFoundException();
                    }
                    else
                    {
                        return null;
                    }
                }
                xlWs.Activate();
                return xlWs;
            }
            catch (Exception ex)
            {

                if (ex.HResult == -2147352565)
                {

                }
                else
                {
                    Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
                }

            }
            return null;
        }



        public dynamic GetRange(string wbName, string wsName, string rangeAddr)
        {
            try
            {
                return ExcelHelper.Shared.GetWorksheetByName(wbName, wsName, true).Range(rangeAddr);
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
            }
            return null;
        }

        private ExcelHelper()
        {
            ;
        }

        public void Dispose()
        {
            try
            {
                this.App.Quit();
            }
            catch
            {

            }
            try
            {
              //  Marshal.FinalReleaseComObject(this.App);   --9oct
            }
            catch (COMException ex)
            {
                switch ((uint)ex.ErrorCode)
                {
                    // 
                    case RPC_SERVER_UNAVAILABLE:
                        break;

                    default:
                        throw;
                }
            }

            this.App = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public abstract class ExcelException : Exception
    {

    }

    public sealed class ExcelNotFoundException : ExcelException
    {

    }

    public sealed class ExcelWorkbookNotFoundException : ExcelException
    {

    }

    public sealed class ExcelWorksheetNotFoundException : ExcelException
    {

    }

   



    
}



