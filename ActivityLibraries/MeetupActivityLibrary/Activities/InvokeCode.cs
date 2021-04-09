using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.Activities;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using BotDesignCommon;
using System.Drawing.Design;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(InvokeCodeDesigner))]
    [ToolboxBitmap("Resources/InvokeCode.png")]
    public class InvokeCode : BaseNativeActivity
    {
        private object s;

        [Category("Input")]
        [DisplayName("Arguments")]
        [Description("Enter the parameters to be passed")]
        //    public Dictionary<String, Argument> ArgumentsPassed { get; set; }
        public Dictionary<String, Argument> ArgumentsPassed { get; set; }
        [Category("Input")]
        [DisplayName("Code")]
        [Description("Code to be executed")]
      
        public InArgument<String> Code { get; set; }

        [Category("Input")]
        [DisplayName("Language")]
        [Description("Programming Language to be used")]
        public NetLanguage Language { get; set; }

        public InvokeCode()
        {
               this.ArgumentsPassed = new Dictionary<string, Argument>();
        }
        protected override void Execute(NativeActivityContext context)
        {
           
        


            if (Language == NetLanguage.CSharp)
            {
                EvalCSC(Code.Get(context),context);
            }
            else if (Language == NetLanguage.VBNet)
            {
                EvalVB(Code.Get(context),context);
            }
        }

        // Eval > Evaluates C# sourcelanguage
        public  object EvalCSC(string sCSCode, NativeActivityContext context)
        {




            //string varlist = null;
            //foreach (string argumentKey in this.ArgumentsPassed.Keys)
            //{
            //    Argument argument = this.ArgumentsPassed[argumentKey];
            //    RuntimeArgument runtimeArgument = new RuntimeArgument(argumentKey, argument.ArgumentType, argument.Direction);
            //    varlist += "string " + argumentKey + "=\"" + this.ArgumentsPassed[argumentKey] + "\";";
            //}

            //Dictionary<string, object> childArguments = new Dictionary<string, object>();

            //foreach (string index in this.ArgumentsPassed.Keys.AsEnumerable<string>())
            //{

            //    childArguments[index] = this.ArgumentsPassed[index].Get((ActivityContext)context);
              
            //}
            try
            {
                sCSCode = sCSCode.Substring(sCSCode.IndexOf("//Code Start") + 12, sCSCode.IndexOf("//Code End") - (sCSCode.IndexOf("//Code Start") + 12));
                string OutArgsString = null;
                //Argument fake = null;
                Argument[] outArgList = new Argument[this.ArgumentsPassed.Keys.Count];
                //string fakeargkey = null;
                object[] outparameterz = new object[this.ArgumentsPassed.Keys.Count];
                object[] parameterz = new object[this.ArgumentsPassed.Keys.Count];
                //string varlist = null;
                object vala = null;
                //string valb = null;
                string definitionlist = null;
                int counter = 0;
                int outcounter = 0;
                foreach (string argumentKey in this.ArgumentsPassed.Keys)
                {
                    string typename = ArgumentsPassed[argumentKey].ArgumentType.Name;
                    if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("Out") || ArgumentsPassed[argumentKey].Direction.ToString().Equals("InOut"))
                    {
                        //fakeargkey = argumentKey;
                        //fake = this.ArgumentsPassed[argumentKey];
                        if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("Out"))
                        {
                            parameterz[counter] = null;
                            counter++;
                        }
                        outparameterz[outcounter] = argumentKey;
                        OutArgsString += argumentKey + ",";
                        outArgList[outcounter] = this.ArgumentsPassed[argumentKey];
                        outcounter++;
                    }

                    if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("In") || ArgumentsPassed[argumentKey].Direction.ToString().Equals("InOut"))
                    {
                        //vala = this.ArgumentsPassed[argumentKey].Get((ActivityContext)context);

                        //if (typename.Equals("String", StringComparison.CurrentCulture))
                        //{
                        //    valb = "\"" + vala + "\"";
                        //}
                        vala = this.ArgumentsPassed[argumentKey].Get((ActivityContext)context);
                        parameterz[counter] = vala;
                        counter++;

                    }
                    //varlist += typename + " " + argumentKey + "=" + valb + ";";
                    definitionlist += typename + " " + argumentKey + ",";
                }
                if (definitionlist != null) { 
                definitionlist = definitionlist.Substring(0, definitionlist.Length - 1);
                }
                if (OutArgsString != null)
                {
                    OutArgsString = OutArgsString.Substring(0, OutArgsString.Length - 1);
                }
                CSharpCodeProvider c = new CSharpCodeProvider();
                ICodeCompiler icc = c.CreateCompiler();
                CompilerParameters cp = new CompilerParameters();
                //cp.ReferencedAssemblies.Add("Bot.Activity.ActivityLibrary.dll");
                cp.ReferencedAssemblies.Add("system.dll");
                cp.ReferencedAssemblies.Add("system.xml.dll");
                cp.ReferencedAssemblies.Add("system.data.dll");
                cp.ReferencedAssemblies.Add("system.windows.forms.dll");
                cp.ReferencedAssemblies.Add("system.drawing.dll");

                cp.CompilerOptions = "/t:library";
                cp.GenerateInMemory = true;

                StringBuilder sb = new StringBuilder("");
                sb.Append("using System;\n");
                sb.Append("using System.Xml;\n");
                sb.Append("using System.Data;\n");
                sb.Append("using System.Data.SqlClient;\n");
                //sb.Append("using System.Data.SqlClient.Row;\n");
                sb.Append("using System.Windows.Forms;\n");
                sb.Append("using System.IO;\n");
                sb.Append("using System.Drawing;\n");

                sb.Append("namespace CSCodeEvaler{ \n");
                sb.Append("public class CSCodeEvaler{ \n");
                sb.Append("public object[] EvalCode(" + definitionlist + "){\n");
                sb.Append("" + sCSCode + "; \n");
                sb.Append(" return new object[]{" + OutArgsString + "}" + "; \n} \n");
                //sb.Append(" return "+fakeargkey +"; \n} \n");
                sb.Append("} \n");
                sb.Append("}\n");

                CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());


                if (cr.Errors.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("ERROR: " + cr.Errors[0].ErrorText,
                       "Error evaluating cs code", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return null;
                }

                System.Reflection.Assembly a = cr.CompiledAssembly;
                object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");

                Type t = o.GetType();
                MethodInfo mi = t.GetMethod("EvalCode");

                object s = mi.Invoke(o, parameterz);
                object[] sval = (object[])s;
                if (sval!=null)
                {
                    for (int i = 0; i < sval.Length; i++)
                    {
                        outArgList[i].Set((ActivityContext)context, sval[i]);
                    } 
                }
                //   fake.Set((ActivityContext)context, s);
                //     object s = mi.Invoke(o, new object[] {vala});
                return s;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message,Logger.LogLevel.Error);
                context.Abort();
            }
            return null;
        }
        public object EvalVB(string sCSCode , NativeActivityContext context)
        {

            try
            {
                sCSCode = sCSCode.Substring(sCSCode.IndexOf("'Code Start") + 12, sCSCode.IndexOf("'Code End") - (sCSCode.IndexOf("'Code Start") + 12));
                string OutArgsString = null;
                //Argument fake = null;
                Argument[] outArgList = new Argument[this.ArgumentsPassed.Keys.Count];
                //string fakeargkey = null;
                object[] outparameterz = new object[this.ArgumentsPassed.Keys.Count];
                object[] parameterz = new object[this.ArgumentsPassed.Keys.Count];
                //string varlist = null;
                object vala = null;
                //string valb = null;
                string definitionlist = null;
                int counter = 0;
                int outcounter = 0;
                foreach (string argumentKey in this.ArgumentsPassed.Keys)
                {
                    string typename = ArgumentsPassed[argumentKey].ArgumentType.Name;
                    if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("Out") || ArgumentsPassed[argumentKey].Direction.ToString().Equals("InOut"))
                    {
                        //fakeargkey = argumentKey;
                        //fake = this.ArgumentsPassed[argumentKey];
                        if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("Out"))
                        {
                            parameterz[counter] = null;
                            counter++;
                        }
                        outparameterz[outcounter] = argumentKey;
                        OutArgsString += argumentKey + ",";
                        outArgList[outcounter] = this.ArgumentsPassed[argumentKey];
                        outcounter++;
                    }

                    if (ArgumentsPassed[argumentKey].Direction.ToString().Equals("In") || ArgumentsPassed[argumentKey].Direction.ToString().Equals("InOut"))
                    {
                        //vala = this.ArgumentsPassed[argumentKey].Get((ActivityContext)context);

                        //if (typename.Equals("String", StringComparison.CurrentCulture))
                        //{
                        //    valb = "\"" + vala + "\"";
                        //}
                        vala = this.ArgumentsPassed[argumentKey].Get((ActivityContext)context);
                        parameterz[counter] = vala;
                        counter++;

                    }
                    //varlist += typename + " " + argumentKey + "=" + valb + ";";
                    definitionlist += argumentKey + " As " + typename + ",";
                }
                if (definitionlist != null)
                {
                    definitionlist = definitionlist.Substring(0, definitionlist.Length - 1);
                }
                if (OutArgsString != null)
                {
                    OutArgsString = OutArgsString.Substring(0, OutArgsString.Length - 1);
                }








                VBCodeProvider c = new VBCodeProvider();
                ICodeCompiler icc = c.CreateCompiler();
                CompilerParameters cp = new CompilerParameters();

                cp.ReferencedAssemblies.Add("system.dll");
                cp.ReferencedAssemblies.Add("system.xml.dll");
                cp.ReferencedAssemblies.Add("system.data.dll");
                cp.ReferencedAssemblies.Add("system.windows.forms.dll");
                cp.ReferencedAssemblies.Add("system.drawing.dll");

                cp.CompilerOptions = "/t:library";
                cp.GenerateInMemory = true;


                StringBuilder sb = new StringBuilder("");
                sb.Append("Imports System\n");
                sb.Append("Imports System.Xml\n");
                sb.Append("Imports System.Data\n");
                sb.Append("Imports System.Data.SqlClient\n");
                sb.Append("Imports System.Windows.Forms\n");
                sb.Append("Imports System.Drawing\n");

                sb.Append("Namespace VBCodeEvaler \n");
                sb.Append("Class VBCodeEvaler \n");
                sb.Append("Function EvalCode("+ definitionlist + ") \n");
                sb.Append("" + sCSCode + " \n");
                sb.Append("Dim miscData() As Object ={" + OutArgsString + "} \n");
                sb.Append("Return miscdata \n");
                sb.Append("End Function \n");
                sb.Append("End Class\n");
                sb.Append("End Namespace\n");

                CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
                if (cr.Errors.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("ERROR: " + cr.Errors[0].ErrorText,
                       "Error evaluating vb code", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return null;
                }

                System.Reflection.Assembly a = cr.CompiledAssembly;
                object o = a.CreateInstance("VBCodeEvaler.VBCodeEvaler");

                Type t = o.GetType();
                MethodInfo mi = t.GetMethod("EvalCode");

                object s = mi.Invoke(o, parameterz);
                object[] sval = (object[])s;
                if (sval!=null)
                {
                    for (int i = 0; i < sval.Length; i++)
                    {
                        outArgList[i].Set((ActivityContext)context, sval[i]);
                    } 
                }
                return s;
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message, Logger.LogLevel.Error);
                context.Abort();
            }
            return null;

        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {

            try
            {
                foreach (string argumentKey in this.ArgumentsPassed.Keys)
                {
                    Argument argument = this.ArgumentsPassed[argumentKey];
                    RuntimeArgument runtimeArgument = new RuntimeArgument(argumentKey, argument.ArgumentType, argument.Direction);
                    metadata.Bind(argument, runtimeArgument);
                    metadata.AddArgument(runtimeArgument);
                   
                }

                base.CacheMetadata(metadata);
            }
            catch (Exception)
            {


            }

            //try
            //{
            //    DynamicActivity dynamicActivity = this.LoadDynamicActivityFromCache();

            //    if (dynamicActivity != null)
            //    {
            //        this.Validate(metadata, dynamicActivity);
            //    }
            //    else
            //    {
            //        metadata.AddValidationError(Resources.SpecifyValidWorkflowValidationErrorText);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    metadata.AddValidationError(string.Format(Resources.FailedToLoadWorkflowValidationErrorText, ex.Message));
            //}
        }

    }


    public enum NetLanguage
    {
       CSharp = 0, VBNet = 1
    }
}
