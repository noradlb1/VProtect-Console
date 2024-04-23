using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using VProtect.Runtime.Data;
using VProtect.Runtime.Execution;

namespace VProtect.Runtime
{
    internal class VMInstance
    {
        [ThreadStatic] static Dictionary<Module, VMInstance> instances;
        static readonly object initLock = new object();
        static Dictionary<Module, int> initialized = new Dictionary<Module, int>();

        public Module Module
        {
            get;
            private set;
        }

        public VMData Data
        {
            get;
            private set;
        }

        public VMInstance(Module module)
        {
            Module = module;
            Data = new VMData(module);
        }

        public static VMInstance Instance(Module module)
        {
            VMInstance inst;
            if (instances == null)
                instances = new Dictionary<Module, VMInstance>();

            if (!instances.TryGetValue(module, out inst))
            {
                inst = new VMInstance(module);
                instances[module] = inst;

                lock (initLock)
                {
                    if (!initialized.ContainsKey(module))
                    {
                        inst.Initialize();
                        initialized.Add(module, initialized.Count);
                    }
                }
            }

            return inst;
        }

        public void Initialize()
        {
            // Null
        }

        public object Execute(int id, object[] args)
        {
            var ctx = new VMContext(Module, this);
            ctx.SetSettings(id, args);

            try
            {
                return ctx.RunInternal(args);
            }
            finally
            {
                ctx.Release();
            }
        }
    }
}
