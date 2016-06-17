using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wirtualny_Pulpit
{
    class Desktop
    {
        ////////////////////////////////
        //////////// PULPIT ////////////
        ////////////////////////////////

        [DllImport("user32.dll")]
        public static extern IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice, IntPtr pDevmode, int dwFlags, long dwDesiredAccess, IntPtr lpsa);

        [DllImport("user32.dll")]
        public static extern IntPtr OpenDesktop(string lpszDesktop, int dwFlags, bool fInherit, long dwDesiredAccess);

        [DllImport("user32.dll")]
        public static extern IntPtr OpenInputDesktop(int dwFlags, bool fInherit, long dwDesiredAccess);

        [DllImport("user32.dll")]
        public static extern bool SwitchDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        public static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        private static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, IntPtr pvInfo, int nLength, ref int lpnLengthNeeded);

        private const long prawa = 0x0002L | 0x0040L | 0x0080L | 0x0100L | 0x0004L | 0x0008L | 0x0001L | 0x0010L | 0x0020;

        ////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////// FUNCKJE ///////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static IntPtr Stworz(string nazwa)
        {
            return CreateDesktop(nazwa, IntPtr.Zero, IntPtr.Zero, 0, prawa, IntPtr.Zero);
        }

        public static IntPtr Otworz(string nazwa)
        {
            return OpenDesktop(nazwa, 0, true, prawa);
        }

        public static bool Przelacz(string nazwa)
        {
            if (Otworz(nazwa) == IntPtr.Zero)
            {
                return false;
            }
            return SwitchDesktop(Otworz(nazwa));
        }

        public static bool CzyIstnieje(string nazwa)
        {
            if (Otworz(nazwa) == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        public static string NazwaPulpitu(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return string.Empty;
            }

            int dlugosc = 0;
            GetUserObjectInformation(handle, 2, IntPtr.Zero, 0, ref dlugosc);
            IntPtr wskaznik = Marshal.AllocHGlobal(dlugosc);
            bool wynik = GetUserObjectInformation(handle, 2, wskaznik, dlugosc, ref dlugosc);
            string nazwa = Marshal.PtrToStringAnsi(wskaznik);
            Marshal.FreeHGlobal(wskaznik);

            if (!wynik)
            {
                return string.Empty;
            }
            return nazwa;
        }

        ////////////////////////////////
        /////////// PROCESY ////////////
        ////////////////////////////////

        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, ref PROCESS_INFORMATION lpProcessInformation);


        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        private const int NORMAL_PRIORITY_CLASS = 0x00000020;

        ////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////// FUNCKJE ///////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static bool StworzProces(string nazwa, string sciezka, string args)
        {
            try
            {
                PROCESS_INFORMATION informacje = new PROCESS_INFORMATION();
                STARTUPINFO startupInfo = new STARTUPINFO();
                startupInfo.cb = Marshal.SizeOf(startupInfo);
                startupInfo.lpDesktop = nazwa;
                string sciezka2 = string.Format("\"{0}\" {1}", sciezka, args);
                bool result = CreateProcess(sciezka, sciezka2, IntPtr.Zero, IntPtr.Zero, true,
                        NORMAL_PRIORITY_CLASS, IntPtr.Zero, null, ref startupInfo, ref informacje);

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
