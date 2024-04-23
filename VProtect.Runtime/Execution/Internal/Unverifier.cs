using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

#pragma warning disable SYSLIB0003
#pragma warning disable SYSLIB0025

namespace VProtect.Runtime.Execution.Internal
{
    internal static class Unverifier
    {
        public static readonly Module Module;

        static Unverifier()
        {
#if NETFRAMEWORK
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("<>c__VProtect"), AssemblyBuilderAccess.Run);
#elif NETCOREAPP
            var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("<>c__VProtect"), AssemblyBuilderAccess.Run);
#endif

            var mod = asm.DefineDynamicModule("<>c__VModule");

            mod.SetCustomAttribute(new CustomAttributeBuilder(typeof(SecurityPermissionAttribute).GetConstructor(new[] { typeof(SecurityAction) }),
                    new object[] { SecurityAction.Assert },
                    new[] { typeof(SecurityPermissionAttribute).GetProperty("SkipVerification") },
                    new object[] { true }));

            mod.SetCustomAttribute(new CustomAttributeBuilder(typeof(SuppressIldasmAttribute).GetConstructor(new Type[0]), new object[0]));

            Module = mod.DefineType(" ").CreateType().Module;
        }
    }
}