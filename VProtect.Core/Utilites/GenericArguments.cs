using System;
using System.Collections.Generic;

using dnlib.DotNet;

namespace VProtect.Core.Utilites
{
    /// <summary>
    /// Replaces generic type/method var with its generic argument
    /// </summary>
    public sealed class GenericArguments
    {
        GenericArgumentsStack typeArgsStack = new GenericArgumentsStack(true);
        GenericArgumentsStack methodArgsStack = new GenericArgumentsStack(false);

        public void PushTypeArgs(IList<TypeSig> typeArgs)
        {
            typeArgsStack.Push(typeArgs);
        }

        public IList<TypeSig> PopTypeArgs()
        {
            return typeArgsStack.Pop();
        }

        public void PushMethodArgs(IList<TypeSig> methodArgs)
        {
            methodArgsStack.Push(methodArgs);
        }

        public IList<TypeSig> PopMethodArgs()
        {
            return methodArgsStack.Pop();
        }

        public TypeSig Resolve(TypeSig typeSig)
        {
            if (typeSig == null)
                return null;

            var sig = typeSig;

            var genericMVar = sig as GenericMVar;
            if (genericMVar != null)
            {
                var newSig = methodArgsStack.Resolve(genericMVar.Number);
                if (newSig == null || newSig == sig)
                    return sig;
                return newSig;
            }

            var genericVar = sig as GenericVar;
            if (genericVar != null)
            {
                var newSig = typeArgsStack.Resolve(genericVar.Number);
                if (newSig == null || newSig == sig)
                    return sig;
                return newSig;
            }

            return sig;
        }

        public TypeSig ResolveType(TypeSig typeSig)
        {
            switch (typeSig.ElementType)
            {
                case ElementType.Ptr:
                    return new PtrSig(ResolveType(typeSig.Next));

                case ElementType.ByRef:
                    return new ByRefSig(ResolveType(typeSig.Next));

                case ElementType.SZArray:
                    return new SZArraySig(ResolveType(typeSig.Next));

                case ElementType.Array:
                    var arraySig = (ArraySig)typeSig;
                    return new ArraySig(ResolveType(typeSig.Next), arraySig.Rank, arraySig.Sizes, arraySig.LowerBounds);

                case ElementType.Pinned:
                    return new PinnedSig(ResolveType(typeSig.Next));

                case ElementType.Var:
                case ElementType.MVar:
                    return Resolve(typeSig);

                case ElementType.GenericInst:
                    var genInst = (GenericInstSig)typeSig;
                    var typeArgs = new List<TypeSig>();
                    foreach (var arg in genInst.GenericArguments)
                        typeArgs.Add(ResolveType(arg));
                    return new GenericInstSig(genInst.GenericType, typeArgs);

                case ElementType.CModReqd:
                    return new CModReqdSig(((CModReqdSig)typeSig).Modifier, ResolveType(typeSig.Next));

                case ElementType.CModOpt:
                    return new CModOptSig(((CModOptSig)typeSig).Modifier, ResolveType(typeSig.Next));

                case ElementType.ValueArray:
                    return new ValueArraySig(ResolveType(typeSig.Next), ((ValueArraySig)typeSig).Size);

                case ElementType.Module:
                    return new ModuleSig(((ModuleSig)typeSig).Index, ResolveType(typeSig.Next));
            }

            if (typeSig.IsTypeDefOrRef)
            {
                var s = (TypeDefOrRefSig)typeSig;
                if (s.TypeDefOrRef is TypeSpec)
                    throw new NotSupportedException(); // TODO: ?
            }

            return typeSig;
        }
    }
}
