using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Rasp_Pi_TCP
{
    class Program
    {
        private static int[] variables = new int[6];
        private static byte[] packet = new byte[6];

        static void Main(string[] args)
        {
            Console.WriteLine("TCP_IP Test.");

            Console.WriteLine("Setting up connection to Raspberry Pi.");

            // Creates the socket for communication
            Socket PC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // IP address of Raspberry Pi 1
            IPAddress IP = IPAddress.Parse("169.254.224.199"); // PC IP address 169.254.224.199

            // IP endpoint for connection
            IPEndPoint EndPoint_1 = new IPEndPoint(IP, 139); // PC's port number 139

            // Binds the socket to the IPEndPoint
            PC.Bind(EndPoint_1);

            // Connects to the IPEndPoint
            PC.Connect(EndPoint_1);

            /* Creating packet information array
             * varaibles[0] = selected camera bit
             * varaibles[1] = selected motor bit
             * varaibles[2] = direction bit
             * varaibles[3] = steps bit
             * varaibles[4] = assumed postion bit
             * varaibles[5] = complete bit
            */
            // int[] variables = new int[6];

            /* Creating packet information array
             * packet[0] = selected camera bit
             * packet[1] = selected motor bit
             * packet[2] = direction bit
             * packet[3] = steps bit
             * packet[4] = assumed postion bit
             * packet[5] = complete bit
            */
            // byte[] packet = new byte[6];

            // Attempting to receive data packet from the PC
            Console.WriteLine("Receiveing data packet from the PC.");
            try
            {
                PC.BeginReceive(packet, 0, packet.Length, SocketFlags.Broadcast, new AsyncCallback(ReceiveData), PC);
            }
            catch (SocketException sockExcept)
            {
                Console.WriteLine(sockExcept.ErrorCode.ToString());
            }

            int[] temp = variables;

            temp[0] = 0;
            temp[1] = 0;
            temp[2] = 0;
            temp[3] = 0;
            temp[4] = 250;
            temp[5] = 1;

            for(int i=0; i<6; i++)
            {
                packet[i] = (byte)temp[i];
            }

            // Attepting to send data packet to the PC
            Console.WriteLine("Sending data packet to the PC.");
            try
            {
                PC.BeginSend(packet, 0, packet.Length, SocketFlags.Broadcast, new AsyncCallback(SendData), PC);
            }
            catch (SocketException sockExcept)
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
            Console.WriteLine("Camera:    {0}", variables[0]);
            Console.WriteLine("Motor:     {0}", variables[1]);
            Console.WriteLine("Direction: {0}", variables[2]);
            Console.WriteLine("Steps:     {0}", variables[3]);
            Console.WriteLine("Position:  {0}", variables[4]);
            Console.WriteLine("Complete:  {0}", variables[5]);
        }

        public static void SendData(IAsyncResult callback)
        {
            Socket tempSocket = (Socket)callback.AsyncState; // Temporary socket for ending the data sending phase
            int dataSent = tempSocket.EndSend(callback);     // Ends the data sending phase on tempSocket
            Console.WriteLine("Data sent.");                 // Writes to the console that the data has been sent
        }
    }
}
