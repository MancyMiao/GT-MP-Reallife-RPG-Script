﻿using GrandTheftMultiplayer.Server.API;
using System;
using System.Net;
using System.Net.Sockets;

namespace LocalTelnetAdmin
{
    public class LocalTelnetAdmin : Script
    {
        TcpListener server = null;

        public LocalTelnetAdmin()
        {
            API.onResourceStart += CreateTelnetServer;
        }

        public void CreateTelnetServer()
        {
            try
            {
                Int32 port = 9090;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Telnet Adminconnection can be recieved on: " + localAddr.ToString() + ":" + port);

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    String clientIp = ((IPEndPoint) client.Client.RemoteEndPoint).Address.ToString();
                    Console.WriteLine("Recieved Adminconnection from " + clientIp);

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);


                        

                        // Process the data sent by the client.
//                        data = data.ToUpper();

//                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

//                         Send back a response.
//                        stream.Write(msg, 0, msg.Length);
//                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }
    }
}