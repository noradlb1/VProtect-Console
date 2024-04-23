using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.Writer;

using VProtect.Core.VM;
using VProtect.Core.Services;
using dnlib.DotNet.Emit;

namespace VProtect.Core.Runtime
{
    internal class RTRenamer
    {
        private VMContext context;

        private readonly Dictionary<string, string> nameMap = new Dictionary<string, string>();
        private static RandomGenerator _RND = new RandomGenerator(32);

        public RTRenamer(VMContext context)
        {
            this.context = context;

            nameMap = new Dictionary<string, string>();
            _RND = new RandomGenerator(32);
        }

        public string NewName(string name)
        {
            string result;
            if (!nameMap.TryGetValue(name, out result))
                result = nameMap[name] = string.Format("{0:X}", GetRandom());

            return result;
        }

        public string GetRandom()
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 4; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(32 + (decimal)_RND.NextInt32('z') - 32)));
                builder.Append(ch);
            }

            /*/
             * HEX'e dönüştürülmesini istemiyorsan return kısmını builder.ToString() yap.
             * kaç sayıda random str üretmeyi seçmek için for (int i = 0; i < 4; i++) kısmındaki "4" kısmını değiştirebilirsin.
            /*/
            return string.Join(string.Empty, builder.ToString().Select(c => string.Format("{0:X2}", Convert.ToInt32(c))).ToArray());
        }

        public void Process()
        {
            foreach (var type in context.RTModule.Types)
            {
                //if (type.Namespace == "VProtect.Runtime.Variants")
                //    continue;

                type.Namespace = string.Empty;
                type.Name = NewName(type.Name);

                foreach (var GenParam in type.GenericParameters)
                    GenParam.Name = NewName(GenParam.Name);

                var isDelegate = type.BaseType != null &&
                                 (type.BaseType.FullName == "System.Delegate" ||
                                  type.BaseType.FullName == "System.MulticastDelegate");

                foreach (var method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        foreach (var instr in method.Body.Instructions)
                        {
                            var memberRef = instr.Operand as MemberRef;
                            if (memberRef != null)
                            {
                                var typeDef = memberRef.DeclaringType.ResolveTypeDef();

                                if (memberRef.IsMethodRef && typeDef != null)
                                {
                                    var target = typeDef.ResolveMethod(memberRef);
                                    if (target != null && target.IsRuntimeSpecialName)
                                        typeDef = null;
                                }

                                if (typeDef != null)
                                    memberRef.Name = NewName(memberRef.Name);
                            }
                        }
                    }

                    foreach (var Param in method.Parameters)
                        Param.Name = NewName(Param.Name);

                    if (method.IsRuntimeSpecialName || isDelegate)
                        continue;

                    method.Name = NewName(method.Name);
                }

                for (int i = 0; i < type.Fields.Count; i++)
                {
                    var field = type.Fields[i];
                    if (field.IsLiteral)
                    {
                        type.Fields.RemoveAt(i--);
                        continue;
                    }

                    if (field.IsRuntimeSpecialName)
                        continue;

                    field.Name = NewName(field.Name);
                }

                type.Properties.Clear();
                type.Events.Clear();
                type.CustomAttributes.Clear();
            }
        }
    }
}
