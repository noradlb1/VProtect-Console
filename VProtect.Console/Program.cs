using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using dnlib.DotNet;

using VProtect.Core;
using dnlib.DotNet.MD;

using DConsole = System.Console;
using System.Reflection.Emit;

namespace VProtect.Console
{
    internal unsafe class Program
    {
        static void Main(string[] args)
        {
            var input = args[0];
            var inputDir = Path.GetDirectoryName(input);
            var output = Path.Combine(inputDir, $"{Path.GetFileNameWithoutExtension(input)}_Protected{Path.GetExtension(input)}");

            var module = ModuleDefMD.Load(input, ModuleDef.CreateModuleContext());
            var vm = new VMRun(module);

            //var md = module.Find("Test_Appz.Form1", true).FindMethod("button1_Click");
            //var md1 = module.Find("Test_Appz.Form1", true).FindMethod("InitializeComponent");
            //var md2 = module.Find("Test_Appz.Form1", true).FindMethod("Dispose");
            //var md3 = module.Find("Test_Appz.Form1", true).FindMethod("test");

            var mdlist = new List<MethodDef>();
            //mdlist.Add(md);
            //mdlist.Add(md1);
            //mdlist.Add(md2);
            //mdlist.Add(md3);
            //mdlist.Add(module.EntryPoint);

            for (int i = 0; i < module.Types.Count; i++)
            {
               //if (module.Types[i].Namespace == "System.Json")
                {
                    var mds = module.Types[i].Methods;

                    for (int c = 0; c < mds.Count; c++)
                    {
                        MethodDef md = mds[c];
                        //if (md.Name == "textSourceCode_TextChanged")
                        {
                            mdlist.Add(md);
                            //break;
                        }
                    }
                }
            }

            vm.Run(mdlist);

            module.Write(output, vm.Options);

            vm.Final();

            DConsole.ReadKey();
        }
    }
}
