//using Logger;
//using Microsoft.Office.Interop.Word;
//using System;
//using System.Activities;
//using System.ComponentModel;

//namespace Word
//{
//    public class Word_Replace_Text : BaseNativeActivity
//    {

//        [Category("Input Paramaters")]
//        [DisplayName("File Path")]
//        [Description("Set File Path")]
//        [RequiredArgument]
//        public InArgument<string> FilePath { get; set; }

//        [Category("Input Paramaters")]
//        [DisplayName("Find Text")]
//        [Description("The text to be replaced")]
//        [RequiredArgument]
//        public InArgument<string> FindText { get; set; }

//        [Category("Input Paramaters")]
//        [DisplayName("Replace With Text")]
//        [Description("The replacement text")]
//        [RequiredArgument]
//        public InArgument<string> ReplaceWithText { get; set; }

//        [Category("Output Paramaters")]
//        [DisplayName("Result")]
//        [Description("Get Result")]
//        public OutArgument<bool> Result { get; set; }
//        protected override void Execute(NativeActivityContext context)
//        {

//            try
//            {
//                string filePath = FilePath.Get(context);
//                string findText = FindText.Get(context);
//                string replaceWithText = ReplaceWithText.Get(context);
//                bool result = false;

//                Application wordApp = new Application { Visible =false };
//                Document aDoc = wordApp.Documents.Open(filePath, ReadOnly: false, Visible: false);
//                aDoc.Activate();
//                result= FindAndReplace(wordApp, findText, replaceWithText);

//                if (false == result)
//                {
//                    Log.Logger.LogData("The text " + findText +" does not exist in activity Word_Replace_Text", LogLevel.Error);
//                }
//                aDoc.Save();
//                aDoc.Close();

//               Dispose(wordApp);


//                Result.Set(context,result);
//            }
//            catch (Exception ex)
//            {
//                Result.Set(context, false);
//                Log.Logger.LogData(ex.Message + " in activity Word_Replace_Text", LogLevel.Error);
//                if (context != null)
//                {
//                    context.Abort();
//                }
//            }         
//        }
//        public void Dispose(Application wordApp)
//        {


//            try
//            {
//                wordApp.Quit();
//                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wordApp);
//            }
//            catch (Exception ex)
//            {
//                Log.Logger.LogData(ex.Message + " in activity Word_Replace_Text", LogLevel.Error);
//            }

//            wordApp = null;
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//        }

//        public bool FindAndReplace(Application doc, object findText, object replaceWithText)
//        {
//            bool result = false;
//            //options
//            object matchCase = true;
//            object matchWholeWord = true;
//            object matchWildCards = false;
//            object matchSoundsLike = false;
//            object matchAllWordForms = false;
//            object forward = true;
//            object format = false;
//            object matchKashida = false;
//            object matchDiacritics = false;
//            object matchAlefHamza = false;
//            object matchControl = false;
//            object read_only = false;
//            object visible = true;
//            object replace = 2;
//            object wrap = 1;

//            //execute find and replace
//            result = doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
//                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
//                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);

//            return result;

//        }
//    }
//}


using Logger;
using Microsoft.Office.Interop.Word;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Word
{

    [ToolboxBitmap("Resources/Word_Replace_Text.png")]
    [Designer(typeof(Word_Replace_Text_ActivityDesigner))]
    public class Word_Replace_Text : BaseNativeActivity
    {

        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        //[Category("Input Paramaters")]
        //[DisplayName("Find Text")]
        //[Description("The text to be replaced")]
        //[RequiredArgument]
        //public InArgument<string> FindText { get; set; }

        //[Category("Input Paramaters")]
        //[DisplayName("Replace With Text")]
        //[Description("The replacement text")]
        //[RequiredArgument]
        //public InArgument<string> ReplaceWithText { get; set; }


        [Category("Input Paramaters")]
        [DisplayName("Find Text")]
        [Description("The text to be replaced")]
        [RequiredArgument]
        public InArgument<ICollection<string>> FindText { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Replace With Text")]
        [Description("The replacement text")]
        [RequiredArgument]
        public InArgument<ICollection<string>> ReplaceWithText { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Result")]
        [Description("Get Result")]
        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                string filePath = FilePath.Get(context);
                // string findText = FindText.Get(context);
                // string replaceWithText = ReplaceWithText.Get(context);
                bool result = false;
                ICollection<string> findTextCollection = FindText.Get<ICollection<string>>(context);
                ICollection<string> replaceWithTextCollection = ReplaceWithText.Get<ICollection<string>>(context);

                string[] findTextArray = new string[findTextCollection.Count];
                findTextCollection.CopyTo(findTextArray, 0);

                string[] replaceWithTextArray = new string[replaceWithTextCollection.Count];
                replaceWithTextCollection.CopyTo(replaceWithTextArray, 0);

                if (findTextArray.Length > replaceWithTextArray.Length)
                {
                    Log.Logger.LogData("Provide all replacement text (\"Replace With Text\") in activity Word_Replace_Text", LogLevel.Error);
                }

                else if (findTextArray.Length < replaceWithTextArray.Length)
                {
                    Log.Logger.LogData("Provide all text to be replaced (\"Find Text\") in activity Word_Replace_Text", LogLevel.Error);
                }

                else if (findTextArray.Length == replaceWithTextArray.Length)
                {
                    Application wordApp = new Application { Visible = false };
                    Document aDoc = wordApp.Documents.Open(filePath, ReadOnly: false, Visible: true);
                    aDoc.Activate();

                    for (int i = 0; i < findTextArray.Length; i++)
                    {
                        //find and replace
                        result = wordApp.Selection.Find.Execute(findTextArray[i], true, true,
                                 false, false, false, true, 1, false, replaceWithTextArray[i], 2,
                                 false, false, false, false);

                        if (false == result)
                        {
                            Log.Logger.LogData("The text \"" + findTextArray[i] + "\" does not exist in activity Word_Replace_Text", LogLevel.Error);
                        }
                    }

                    aDoc.Save();
                    aDoc.Close();
                    Dispose(wordApp);
                }
                else
                {
                    result = false;
                }

                // result = FindAndReplace(wordApp, findText, replaceWithText);
                //if (false == result)
                //{
                //    Log.Logger.LogData("The text " + findText +" does not exist in activity Word_Replace_Text", LogLevel.Error);
                //}

                Result.Set(context, result);
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity Word_Replace_Text", LogLevel.Error);
                if (context != null)
                {
                    context.Abort();
                }
            }
        }
        public void Dispose(Application wordApp)
        {
            try
            {
                wordApp.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wordApp);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Word_Replace_Text", LogLevel.Error);
            }

            wordApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //public bool FindAndReplace(Application doc, object findText, object replaceWithText)
        //{
        //    bool result = false;
        //    //options
        //    object matchCase = true;
        //    object matchWholeWord = true;
        //    object matchWildCards = false;
        //    object matchSoundsLike = false;
        //    object matchAllWordForms = false;
        //    object forward = true;
        //    object format = false;
        //    object matchKashida = false;
        //    object matchDiacritics = false;
        //    object matchAlefHamza = false;
        //    object matchControl = false;
        //    object read_only = false;
        //    object visible = true;
        //    object replace = 2;
        //    object wrap = 1;

        //    //execute find and replace
        //    result = doc.Selection.Find.Execute(ref findText, true, true,
        //        false, false, false, true, 1, false, ref replaceWithText, 2,
        //        false, false, false, false);

        //    //execute find and replace
        //    result = doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
        //        ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
        //        ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);

        //    return result;

        //}
    }
}
