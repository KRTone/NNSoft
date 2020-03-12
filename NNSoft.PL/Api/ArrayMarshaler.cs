using System;
using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    // The class that does the marshaling. Making it generic is not required, but
    // will make it easier to use the same custom marshaler for multiple array types.
    public class ArrayMarshaler<T> : ICustomMarshaler
    {
        long arrayLength;
        T[] array;

        // All custom marshalers require a static factory method with this signature.
        public static ICustomMarshaler GetInstance(String cookie)
        {
            return new ArrayMarshaler<T>();
        }

        // This is the function that builds the managed type - in this case, the managed
        // array - from a pointer. You can just return null here if only sending the 
        // array as an in-parameter.
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (IntPtr.Zero == pNativeData)
            {
                return null;
            }

            int elSiz = Marshal.SizeOf<T>();
            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = Marshal.PtrToStructure<T>(pNativeData + (elSiz * i));
            }

            return array;
        }

        public IntPtr MarshalManagedToNative(Object ManagedObject)
        {
            if (null == ManagedObject)
            {
                return IntPtr.Zero;
            }
            array = (T[])ManagedObject;
            arrayLength = array.Length;
            int elSiz = Marshal.SizeOf<T>();
            int size = sizeof(int) + (elSiz * array.Length);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.WriteInt32(ptr, array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                Marshal.StructureToPtr<T>(array[i], ptr + sizeof(int) + (elSiz * i), false);
            }

            return ptr;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeHGlobal(pNativeData);
        }

        public void CleanUpManagedData(Object ManagedObj)
        { }

        public int GetNativeDataSize()
        {
            return sizeof(int) + Marshal.SizeOf<T>();
        }
    }
}
