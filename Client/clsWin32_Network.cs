using System;
using System.Runtime.InteropServices;  // per DllImport e altro
using System.Collections;  // per ArrayList
using System.Security;  // per SuppressUnmanagedCodeSecurityAttribute

static class clsWin32_Network
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
    [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
    private static extern int NetApiBufferFree(IntPtr pBuf);

    // Struttura _SERVER_INFO_100 STRUCTURE di supporto a NetServerEnum()
    [StructLayout(LayoutKind.Sequential)]
    public struct _SERVER_INFO_100
    {
        internal int sv100_platform_id;  // contiene l'id. di OS, tra PLATFORM_ID_DOS, PLATFORM_ID_OS2, PLATFORM_ID_NT, PLATFORM_ID_OSF, or PLATFORM_ID_VMS in lmcons.h
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string sv100_name;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns> Ritorna un Arraylist che rappresenta la lista delle macchine in rete</returns>
    public static ArrayList GetNetworkComputers()
    {
        ArrayList networkComputers = new ArrayList();
        const int MAX_PREFERRED_LENGTH = -1;
        int SV_TYPE_WORKSTATION = 1;
        int SV_TYPE_SERVER = 2;
        IntPtr buffer = IntPtr.Zero;
        IntPtr tmpBuffer = IntPtr.Zero;
        int entriesRead = 0;
        int totalEntries = 0;
        int resHandle = 0;
        int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));


        try
        {
            // Richiedo le macchine, di tipo SV_TYPE_WORKSTATION | SV_TYPE_SERVER (sono tutte?)
            int ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out resHandle);
            if (ret == 0) // Ok
            {
                // Si cicla per estrarre i singoli nomi
                for (int i = 0; i < totalEntries; i++)
                {
                    tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
                    _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

                    //Ecco il nome della macchina
                    networkComputers.Add(svrInfo.sv100_name);
                }
            }
        }
        catch (Exception ex)
        {
            string strMsg;
            strMsg = ex.Message;
            return null;
        }
        finally
        {
            // Comunque dealloco i buffer usati
            NetApiBufferFree(buffer);
        }

        // Ecco la lista delle macchine
        return networkComputers;

    }

} // della classe
