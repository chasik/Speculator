using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SpeculatorServices.Transaq
{
    public static class MarshalUtf8
    {
        private static readonly UTF8Encoding Utf8;

        static MarshalUtf8()
        {
            Utf8 = new UTF8Encoding();
        }

        public static IntPtr StringToHGlobalUtf8(string data)
        {
            var dataEncoded = Utf8.GetBytes(data + "\0");
            var size = Marshal.SizeOf(dataEncoded[0]) * dataEncoded.Length;
            var pData = Marshal.AllocHGlobal(size);
            Marshal.Copy(dataEncoded, 0, pData, dataEncoded.Length);

            return pData;
        }

        public static string PtrToStringUtf8(IntPtr pData)
        {
            var errStr = Marshal.PtrToStringAnsi(pData);
            Debug.Assert(errStr != null, "errStr != null");
            var length = errStr.Length;
            var data = new byte[length];
            Marshal.Copy(pData, data, 0, length);

            return Utf8.GetString(data);
        }
    }
}
