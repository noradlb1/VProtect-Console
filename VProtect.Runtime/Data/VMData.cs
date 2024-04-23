using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using VProtect.Runtime.Dynamic;
using VProtect.Runtime.Execution;
using System.Reflection.Emit;
using VProtect.Runtime.Execution.Internal;

namespace VProtect.Runtime.Data
{
    internal unsafe class VMData
    {
        public Dictionary<int, VMethodInfo> VMethodInfos
        {
            get;
            private set;
        }

        public Dictionary<int, List<TryBlock>> TryBlocks
        {
            get;
            private set;
        }

        public Dictionary<int, VInstruction[]> VInstructions
        {
            get;
            private set;
        }

        public VMData(Module module)
        {
            VMethodInfos = new Dictionary<int, VMethodInfo>();
            TryBlocks = new Dictionary<int, List<TryBlock>>();
            VInstructions = new Dictionary<int, VInstruction[]>();

            byte* data = (byte*)VMDataInitializer.GetData(this);

            int mdCount = *(int*)(data);
            int offset = sizeof(int);

            for (int m = 0; m < mdCount; m++)
            {
                int id = *(int*)(data + offset);
                offset += sizeof(int);

                int len = *(int*)(data + offset);
                offset += sizeof(int);

                byte* arrPtr = data + offset;

                #region Method Infos
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var vmdInfo = new VMethodInfo();
                vmdInfo.IsConstructor = *(bool*)arrPtr;
                arrPtr += sizeof(bool);

                uint methodToken = Utils.FromCodedToken(*(uint*)arrPtr);
                arrPtr += sizeof(uint);

                if (vmdInfo.IsConstructor)
                    vmdInfo.Method = (ConstructorInfo)module.ResolveMethod((int)methodToken);
                else
                    vmdInfo.Method = module.ResolveMethod((int)methodToken);

                vmdInfo.Method_MDToken = vmdInfo.Method.MetadataToken;

                vmdInfo.Type = vmdInfo.Method.DeclaringType;
                vmdInfo.TypeToken = vmdInfo.Type.MetadataToken;

                #region Locals
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                int localsCount = *(int*)arrPtr;
                arrPtr += sizeof(int);

                var _locs = new object[localsCount];
                for (int i = 0; i < localsCount; i++)
                    _locs[i] = null;

                vmdInfo.Locals = _locs;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                VMethodInfos[id] = vmdInfo;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Try Blocks
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var _tryBlocks = new List<TryBlock>();

                int ehsLen = *(int*)arrPtr;
                arrPtr += sizeof(int);

                for (int i = 0; i < ehsLen; i++)
                {
                    var tryStart = *(int*)arrPtr;
                    arrPtr += sizeof(int);

                    var tryEnd = *(int*)arrPtr;
                    arrPtr += sizeof(int);

                    var filterStart = *(int*)arrPtr;
                    arrPtr += sizeof(int);

                    var type = (byte)*(int*)arrPtr;
                    arrPtr += sizeof(int);

                    var catchType = (*(uint*)arrPtr) == 0 ? typeof(object) : module.ResolveType((int)Utils.FromCodedToken(*(uint*)arrPtr));
                    arrPtr += sizeof(uint);

                    var handlerStart = *(int*)arrPtr;
                    arrPtr += sizeof(int);

                    TryBlock tryBlock = null;
                    for (var tbs = 0; tbs < _tryBlocks.Count; tbs++)
                    {
                        var current = _tryBlocks[tbs];
                        if (current.Begin() == tryStart && current.End() == tryEnd)
                        {
                            tryBlock = current;
                            break;
                        }
                    }

                    if (tryBlock == null)
                    {
                        tryBlock = new TryBlock(tryStart, tryEnd);

                        bool isInserted = false;
                        for (var tb = 0; tb < _tryBlocks.Count; tb++)
                        {
                            var current = _tryBlocks[tb];
                            if (tryBlock.CompareTo(current) < 0)
                            {
                                _tryBlocks.Insert(tb, tryBlock);
                                isInserted = true;
                                break;
                            }
                        }

                        if (!isInserted)
                            _tryBlocks.Add(tryBlock);
                    }

                    tryBlock.AddCatchBlock(type, handlerStart, filterStart, catchType);
                }

                TryBlocks[id] = _tryBlocks;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Codes
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                int vinstrLength = *(int*)arrPtr;
                arrPtr += sizeof(int);

                var vinstr = new VInstruction[vinstrLength];

                for (int c = 0; c < vinstr.Length; c++)
                {
                    ushort ilcode = *(ushort*)arrPtr;
                    arrPtr += sizeof(ushort);

                    int operandsCount = *(int*)arrPtr;
                    arrPtr += sizeof(int);

                    var operands = new object[operandsCount];

                    for (int o = 0; o < operands.Length; o++)
                        operands[o] = Utils.VRead(ref arrPtr);

                    vinstr[c] = new VInstruction(ilcode, operands);
                }

                VInstructions[id] = vinstr;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                offset += len;
            }
        }
    }
}
