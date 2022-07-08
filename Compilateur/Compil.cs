﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace code2.Compilateur
{
    internal class Compil
    {

        public void CompileAndRun(string code)
        {
            var t = Application.OpenForms["Form1"].Controls["richTextBox2"] as RichTextBox;

            var CompilerParams = new CompilerParameters();

            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";

            try
            {
                string[] fileEntries = Directory.GetFiles(@"ref/", "*.dll");
                foreach (string fileName in fileEntries)
                {
                    CompilerParams.ReferencedAssemblies.Add(fileName);
                }
            }
            catch
            {
                // ignored
            }

            var provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);

            if (compile.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (object ce in compile.Errors)
                {
                    text += ce.ToString();
                }
                t.Text = "";
                t.Text = $"{text}";
            }

            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null)
            {
                mt = module.GetType("EXO.Program");
            }

            if (mt != null)
            {
                //methInfo = mt.GetMethod("Main");
                object obj = Activator.CreateInstance(mt);
                object output = mt.GetMethod("Main").Invoke(obj, null);
            }

            if (methInfo != null)
            {

                //return (string)methInfo.Invoke(null, new object[] { 5, 10 });
            }

        }
    }
}
