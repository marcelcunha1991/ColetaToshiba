using System;
using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;

namespace ColetaInjetoraToshiba.util
{
    class UtilsTcp
    {

        public static String getMacAddress()
        {
            String macaddress = "desconhecido";

            INetworkInterface[] allInterfaces = new INetworkInterface[0];

            allInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (allInterfaces.Length > 0)
                macaddress = allInterfaces[0].GetPhysicalAddress().ToString();

            return macaddress;
        }
    }
}
