#define HenryTest
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HenryTool
{
    public delegate void DelegateVoidOfVoid();
    public delegate void DelegateVoidOfString(string _str);
    public delegate void DelegateVoidOfInt(int _intValue);

    public delegate void DelegateVoidOfBytes(params byte[] _items);
    public delegate void DelegateVoidOfInts(params int[] _items);
    public delegate void DelegateVoidOfFloats(params float[] _items);
    public delegate void DelegateVoidOfStrings(params string[] _items);
    public delegate void DelegateVoidOfVector3s(params Vector3[] _items);

    public delegate bool DelegateBoolOfVoid();


    delegate IEnumerator DelegateCoroutine();

    public delegate void UnitListEvent(UnitBasic _unit);

    public struct UnitStruct {
        public uint id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string name;

        public Vector3 position;

    }

}






