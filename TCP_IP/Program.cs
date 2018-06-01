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
            IPAddress IP_1 = IPAddress.Parse("169.254.71.226"); // Pi's IP address 169.254.71.226

            // IP endpoint for connection
            IPEndPoint EndPoint_1 = new IPEndPoint(IP_1, 5005); // Pi's port number 5005

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
            int[] variables = { 1, 3, -1, 250, 0, 0 };

            /* Creating packet information array
             * packet[0] = selected camera bit
             * packet[1] = selected motor bit
             * packet[2] = direction bit
             * packet[3] = steps bit
             * packet[4] = assumed postion bit
             * packet[5] = complete bit
            */
            byte[] packet = new byte[6];
            for(int i=0; i<6; i++)
            {
                packet[i] = (byte)variables[i];
            }

            // Attepting to send data packet to the Raspberry Pi
            Console.WriteLine("Sending data packet to the Raspberry Pi.");
            try
            {
                RaspPi_1.BeginSend(packet, 0, packet.Length, SocketFlags.Broadcast, new AsyncCallback(SendData), RaspPi_1);
            }
            catch(SocketException sockExcept)
            {
                Console.WriteLine(sockExcept.ErrorCode.ToString());
            }

            // Attempting to receive data packet from the Raspberry Pi
            Console.WriteLine("Receiveing data packet from the Raspberry Pi.");
            try
            {
                RaspPi_1.BeginReceive(packet, 0, packet.Length, SocketFlags.Broadcast, new AsyncCallback(ReceiveData), RaspPi_1);
            }
            catch(SocketException sockExcept)
            {
                Console.WriteLine(sockExcept.ErrorCode.ToString());
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        // Callback function for canceling the data receive state
        public static void ReceiveData(IAsyncResult callback)
        {
            Socket tempSocket = (Socket)callback.AsyncState;    // Temporary socket for ending the data receiving phase
            int dataReceived = tempSocket.EndReceive(callback); // Ends the data receiving phase on tempSocket
            Console.WriteLine("Data received.");                // Writes to the console that the data has been received
            for (int i=0; i<6; i++)
            {
                variables[i] = (int)packet[i];
            }
            Console.WriteLine("Data:");
            Console.WriteLine("Camera:    %d", variables[0]);
            Console.WriteLine("Motor:     %d", variables[1]);
            Console.WriteLine("Direction: %d", variables[2]);
            Console.WriteLine("Steps:     %d", variables[3]);
            Console.WriteLine("Position:  %d", variables[4]);
            Console.WriteLine("Complete:  %d", variables[5]);
        }

        public static void SendData(IAsyncResult callback)
        {
            Socket tempSocket = (Socket)callback.AsyncState; // Temporary socket for ending the data sending phase
            int dataSent = tempSocket.EndSend(callback);     // Ends the data sending phase on tempSocket
            Console.WriteLine("Data sent.");                 // Writes to the console that the data has been sent
        }
    }
}