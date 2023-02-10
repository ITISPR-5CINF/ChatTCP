using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

static class DLLWrapper_Netapi32
{
    /// <summary>
    /// L'Api NetServerEnum() di Netapi32.dll restituisce tutti i nomi delle macchine in rete (quale, se piu' di una scheda?)
    /// Si puo' specificare quale tipo di macchina (Server e macchine normali) con flag opportuni
    /// </summary>
    /// 
    [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
    private static extern int NetServerEnum(string ServerNane, int dwLevel, ref IntPtr pBuf, int dwPrefMaxLen, out int dwEntriesRead, out int dwTotalEntries, int dwServerType, string domain, out int dwResumeHandle);

    /// <summary>
    /// L'Api NetApiBufferFree() di Netapi32.dll dealloca i buffer usati da NetServerEnum()
    /// </summary>
    [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
    private static extern int NetApiBufferFree(IntPtr pBuf);

    // Struttura SERVER_INFO_100 STRUCTURE di supporto a NetServerEnum()
    [StructLayout(LayoutKind.Sequential)]
    private struct SERVER_INFO_100
    {
        internal int sv100_platform_id;  // contiene l'id. di OS, tra PLATFORM_ID_DOS, PLATFORM_ID_OS2, PLATFORM_ID_NT, PLATFORM_ID_OSF, or PLATFORM_ID_VMS in lmcons.h
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string sv100_name;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> Ritorna un Arraylist che rappresenta la lista delle macchine in rete</returns>
    public static List<string> GetNetworkComputers()
    {
        List<string> networkComputers = new List<string>();
        const int MAX_PREFERRED_LENGTH = -1;
        int SV_TYPE_WORKSTATION = 1;
        int SV_TYPE_SERVER = 2;
        IntPtr buffer = IntPtr.Zero;
        int sizeofINFO = Marshal.SizeOf(typeof(SERVER_INFO_100));

        try
        {
            // Richiedo le macchine, di tipo SV_TYPE_WORKSTATION | SV_TYPE_SERVER (sono tutte?)
            int ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH, out int entriesRead,
                                    out int totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out int resHandle);

            if (ret == 0) // Ok
            {
                // Si cicla per estrarre i singoli nomi
                for (int i = 0; i < totalEntries; i++)
                {
                    IntPtr tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
                    SERVER_INFO_100 svrInfo = (SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(SERVER_INFO_100));

                    // Ecco il nome della macchina
                    networkComputers.Add(svrInfo.sv100_name);
                }
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return null;
        }
        finally
        {
            // Comunque dealloco i buffer usati
            if (buffer != IntPtr.Zero)
            {
                NetApiBufferFree(buffer);
            }
        }

        // Ecco la lista delle macchine
        return networkComputers;
    }
}
