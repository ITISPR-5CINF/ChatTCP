using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatTCP.Client
{
    static class BroadcastDomainUtils
    {
        // funzione per prendere dinamicamete i primi 3 ottetti del getway
        static string GetSubnet()
        {
            // Ottiene un elenco di tutte le interfacce di rete disponibili nel computer
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Scorre tutte le interfacce di rete per cercare l'indirizzo IP del gateway predefinito
            foreach (NetworkInterface ni in interfaces)
            {
                // Ottiene le proprietà dell'interfaccia di rete
                IPInterfaceProperties ipProps = ni.GetIPProperties();

                // Scorre tutti gli indirizzi IP dei gateway predefiniti dell'interfaccia di rete
                foreach (GatewayIPAddressInformation addr in ipProps.GatewayAddresses)
                {
                    // Verifica che l'indirizzo del gateway sia un indirizzo IPv4
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        // Ottiene i primi tre ottetti dell'indirizzo IP del gateway per identificare il prefisso di rete
                        string[] gatewayOctets = addr.Address.ToString().Split('.');
                        return $"{gatewayOctets[0]}.{gatewayOctets[1]}.{gatewayOctets[2]}";
                    }
                }
            }

            // Se il prefisso di rete non viene trovato, viene sollevata un'eccezione
            throw new Exception("Subnet non trovato");
        }

        // funzione che popola la combobox degli indirizzi
        public static List<string> GetNetComputers()
        {
            List<string> NetComputers = new List<string>();

            // Ottiene il prefisso di rete per la rete locale
            string subnet = GetSubnet();

            // Crea una lista di task per il ping di tutti i possibili indirizzi IP nella rete
            var tasks = new List<Task<PingReply>>();
            for (int i = 1; i < 255; i++)
            {
                string host = $"{subnet}.{i}";
                var ip = IPAddress.Parse(host);
                Ping ping = new Ping();
                tasks.Add(Task.Run(() => ping.SendPingAsync(ip, 100)));
            }

            // Esegue tutti i task in parallelo e ottiene i risultati
            var results = Task.WhenAll(tasks).GetAwaiter().GetResult();

            // Aggiunge gli indirizzi IP degli host che hanno risposto al ping all'elenco di host disponibili
            for (int i = 1; i < results.Length; i++)
            {
                var result = results[i];
                string host = $"{subnet}.{i + 1}";
                if (result.Status == IPStatus.Success)
                {
                    NetComputers.Add(host);
                }
            }

            return NetComputers;
        }
    }
}
