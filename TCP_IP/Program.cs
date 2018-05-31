using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP_IP
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("TCP_IP Test.");

            Console.WriteLine("Setting up connection to Raspberry Pi.");

            // Creates the socket for communication
            Socket RaspPi_1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // IP address of Raspberry Pi 1
            IPAddress IP_1 = IPAddress.Parse(""); // Insert Pi IP address

            // IP endpoint for connection
            IPEndPoint EndPoint_1 = new IPEndPoint(IP_1, 8080); // Change 8080 to Pi's port number

            // Binds the socket to the IPEndPoint
            RaspPi_1.Bind(EndPoint_1);

            // Connects to the IPEndPoint
            RaspPi_1.Connect(EndPoint_1);

            /* Creating packet information array
             * varaibles[0] = selected camera bit
             * varaibles[1] = selected motor bit
             * varaibles[2] = direction bit
             * varaibles[3] = steps bit
             * varaibles[4] = assumed postion bit
             * varaibles[5] = complete bit
            */
            int[] variables = { 1, 6, -1, 250, 0, 0 };

            /* Creating packet information array
             * packet[0] = selected camera bit
             * packet[1] = selected motor bit
             * packet[2] = direction bit
             * packet[3] = steps bit
             * packet[4] = assumed postion bit
             * packet[5] = complete bit
            */
            byte[] packet = new byte[6];

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
