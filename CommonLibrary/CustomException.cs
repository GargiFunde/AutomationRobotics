using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
   public static class CustomException
    {
        public static Boolean CheckFileExists(string filepath)
        {
            
                StackFrame frame = new StackFrame(1);

                if (File.Exists(filepath) == true)
                {
                    return true;
                }
                else
                {
                    throw (new FileNotFoundException(" FileNotFoundException Generated in  " + frame.GetMethod().DeclaringType.Name +","+ filepath + " ####Does not Exist"));
                    
                //return false;
                 }
            
            
           
           
        }


        public static Boolean CheckDatatableEmpty(System.Data.DataTable DT)
        {
            StackFrame frame = new StackFrame(1);
            
            if (DT!=null && DT.Rows.Count>0)
            {
                return true;
            }
            else
            {
                 throw (new DatatableEmptyException(" DatatableEmptyException Generated in  " + frame.GetMethod().DeclaringType.Name + "," +"Datatable Is Empty"));
               
            }
        }

        public static Boolean ValidateFileName(string filename)
        {
            StackFrame frame = new StackFrame(1);
            if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
            {
                return true;
            }
            else
            {
                throw (new IncorrectFileNameException(" IncorrectFileNameException Generated in " + frame.GetMethod().DeclaringType.Name +" Activity, file name cannot contain any of the following special characters: \\ / : * ? \" < >| "));
            }
        }
    }

     

    
    public class FileNotFoundException : Exception
    { 
        public FileNotFoundException(): base()
        {
             
        }
        public FileNotFoundException(string message) : base(message)
        {

        }

      
    }

    public class DatatableEmptyException : Exception
    {
        public DatatableEmptyException()
        {

        }
        public DatatableEmptyException(string message) : base(message)
        {

        }
    }

    public class IncorrectFileNameException : Exception
    {
        public IncorrectFileNameException(string message) : base(message)
        {

        }
    }

}
