//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Data;
//using System.Text.RegularExpressions;
//using HtmlAgilityPack;
//using mshtml;

//namespace Bot.Activity.Web
//{
//    public class TableHelper
//    {
//        private DataSet ConvertHTMLTableToDataTable(HTMLDocument doc, string TableName)
//        {
//            //HtmlWeb web = new HtmlWeb();
//            //HtmlDocument document = web.Load(pageURL);
//            //List<List<string>> parsedTbl =
//            //  document.DocumentNode.SelectSingleNode("//table[@class='" + className + "']")
//            //  .Descendants("tr")
//            //  .Skip(1) //To Skip Table Header Row
//            //  .Where(tr => tr.Elements("td").Count() > 1)
//            //  .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
//            //  .ToList();

//            //return parsedTbl;
//            return null;

//        }
//        private DataSet ConvertHTMLTablesToDataSet(string HTML)
//        {
//            // Declarations    
//            DataSet ds = new DataSet();
//            DataTable dt = null;
//            DataRow dr = null;
//            string TableExpression = "<table[^>]*>(.*?)</table>";
//            string HeaderExpression = "<th[^>]*>(.*?)</th>";
//            string RowExpression = "<tr[^>]*>(.*?)</tr>";
//            string ColumnExpression = "<td[^>]*>(.*?)</td>";
//            bool HeadersExist = false;
//            int iCurrentColumn = 0;
//            int iCurrentRow = 0;

//            // Get a match for all the tables in the HTML    
//            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

//            // Loop through each table element    
//            foreach (Match Table in Tables)
//            {

//                // Reset the current row counter and the header flag    
//                iCurrentRow = 0;
//                HeadersExist = false;

//                // Add a new table to the DataSet    
//                dt = new DataTable();

//                // Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)    
//                if (Table.Value.Contains("<th"))
//                {
//                    // Set the HeadersExist flag    
//                    HeadersExist = true;

//                    // Get a match for all the rows in the table    
//                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

//                    // Loop through each header element    
//                    foreach (Match Header in Headers)
//                    {
//                        if (!dt.Columns.Contains(Header.Groups[1].ToString()))
//                            dt.Columns.Add(Header.Groups[1].ToString().Replace("&nbsp;", ""));
//                    }
//                }
//                else
//                {
//                    for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
//                    {
//                        dt.Columns.Add("Column " + iColumns);
//                    }
//                }

//                // Get a match for all the rows in the table    
//                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

//                // Loop through each row element    
//                foreach (Match Row in Rows)
//                {

//                    // Only loop through the row if it isn't a header row    
//                    if (!(iCurrentRow == 0 & HeadersExist == true))
//                    {

//                        // Create a new row and reset the current column counter    
//                        dr = dt.NewRow();
//                        iCurrentColumn = 0;

//                        // Get a match for all the columns in the row    
//                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

//                        // Loop through each column element    
//                        foreach (Match Column in Columns)
//                        {
//                            if (Columns.Count - 1 != iCurrentColumn)
//                                // Add the value to the DataRow    
//                                dr[iCurrentColumn] = Convert.ToString(Column.Groups[1]).Replace("&nbsp;", "");

//                            // Increase the current column     
//                            iCurrentColumn += 1;
//                        }

//                        // Add the DataRow to the DataTable    
//                        dt.Rows.Add(dr);

//                    }

//                    // Increase the current row counter    
//                    iCurrentRow += 1;
//                }

//                // Add the DataTable to the DataSet    
//                ds.Tables.Add(dt);

//            }

//            return ds;

//        }
//    }
//}