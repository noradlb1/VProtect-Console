using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using VProtect.Core.VM;
using VProtect.Core.Services;

namespace VProtect.Core.Utilites
{
    internal static class MDTokenResolver
    {
        public static uint Resolve(VMContext ctx, dynamic obj)
        {
            return ResolveRaw(ctx.Module, ctx.ModuleWriter, obj);
        }

        private static uint ResolveRaw(ModuleDef module, ModuleWriterBase writer, dynamic obj)
        {
            if (module == null)
                module = writer.Module;

            if (true)
            {
                MDToken _reloadedToken = new MDToken();
                GenericArguments genericArguments = new GenericArguments();

                if (obj is TypeDef)
                {
                    var val = ((TypeDef)obj).TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }
                else if (obj is TypeRef)
                {
                    var val = ((TypeRef)obj).TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }
                else if (obj is TypeSpec)
                {
                    var val = ((TypeSpec)obj).TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }
                else if (obj is IMethod)
                {
                    var val = ((IMethod)obj).DeclaringType.TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }
                else if (obj is IMethodDefOrRef)
                {
                    var val = ((IMethodDefOrRef)obj).DeclaringType.TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }
                else if (obj is IField)
                {
                    var val = ((IField)obj).DeclaringType.TryGetGenericInstSig();
                    if (val != null)
                        genericArguments.PushTypeArgs(val.GenericArguments);
                }

                if (writer != null)
                {
                    try
                    {
                        if (obj is TypeSig)
                            obj = genericArguments.ResolveType((TypeSig)obj).ToTypeDefOrRef();
                        else if (obj is TypeRef)
                            obj = genericArguments.ResolveType(((TypeRef)obj).ToTypeSig()).ToTypeDefOrRef();
                        else if (obj is TypeSpec)
                            obj = genericArguments.ResolveType(((TypeSpec)obj).ToTypeSig()).ToTypeDefOrRef();
                        else if (obj is TypeDef)
                            obj = genericArguments.ResolveType(((TypeDef)obj).ToTypeSig()).ToTypeDefOrRef();

                        _reloadedToken = writer.Metadata.GetToken(obj);
                    }
                    catch
                    {
                        if (obj is IMethod)
                            _reloadedToken = ((IMethod)obj).MDToken;
                        else if (obj is IMethodDefOrRef)
                            _reloadedToken = ((IMethodDefOrRef)obj).MDToken;
                        else if (obj is IField)
                            _reloadedToken = ((IField)obj).MDToken;
                        else if (obj is IType)
                        {
                            if (obj is TypeSig)
                                _reloadedToken = genericArguments.ResolveType((TypeSig)obj).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeRef)
                                _reloadedToken = genericArguments.ResolveType(((TypeRef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeSpec)
                                _reloadedToken = genericArguments.ResolveType(((TypeSpec)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeDef)
                                _reloadedToken = genericArguments.ResolveType(((TypeDef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else
                                _reloadedToken = ((IType)obj).MDToken;
                        }
                        else if (obj is ITypeDefOrRef)
                        {
                            if (obj is TypeSig)
                                _reloadedToken = genericArguments.ResolveType((TypeSig)obj).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeRef)
                                _reloadedToken = genericArguments.ResolveType(((TypeRef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeSpec)
                                _reloadedToken = genericArguments.ResolveType(((TypeSpec)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else if (obj is TypeDef)
                                _reloadedToken = genericArguments.ResolveType(((TypeDef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                            else
                                _reloadedToken = ((ITypeDefOrRef)obj).MDToken;
                        }
                    }
                }
                else
                {
                    if (obj is IMethod)
                        _reloadedToken = ((IMethod)obj).MDToken;
                    else if (obj is IMethodDefOrRef)
                        _reloadedToken = ((IMethodDefOrRef)obj).MDToken;
                    else if (obj is IField)
                        _reloadedToken = ((IField)obj).MDToken;
                    else if (obj is IType)
                    {
                        if (obj is TypeSig)
                            _reloadedToken = genericArguments.ResolveType((TypeSig)obj).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeRef)
                            _reloadedToken = genericArguments.ResolveType(((TypeRef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeSpec)
                            _reloadedToken = genericArguments.ResolveType(((TypeSpec)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeDef)
                            _reloadedToken = genericArguments.ResolveType(((TypeDef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else
                            _reloadedToken = ((IType)obj).MDToken;
                    }
                    else if (obj is ITypeDefOrRef)
                    {
                        if (obj is TypeSig)
                            _reloadedToken = genericArguments.ResolveType((TypeSig)obj).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeRef)
                            _reloadedToken = genericArguments.ResolveType(((TypeRef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeSpec)
                            _reloadedToken = genericArguments.ResolveType(((TypeSpec)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else if (obj is TypeDef)
                            _reloadedToken = genericArguments.ResolveType(((TypeDef)obj).ToTypeSig()).ToTypeDefOrRef().MDToken;
                        else
                            _reloadedToken = ((ITypeDefOrRef)obj).MDToken;
                    }
                }

                return GetCodedToken(_reloadedToken);
            }
        }

        public static uint GetCodedToken(MDToken token)
        {
            switch (token.Table)
            {
                case Table.TypeDef:
                    return token.Rid << 3 | 1;
                case Table.TypeRef:
                    return token.Rid << 3 | 2;
                case Table.TypeSpec:
                    return token.Rid << 3 | 3;
                case Table.MemberRef:
                    return token.Rid << 3 | 4;
                case Table.Method:
                    return token.Rid << 3 | 5;
                case Table.Field:
                    return token.Rid << 3 | 6;
                case Table.MethodSpec:
                    return token.Rid << 3 | 7;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
