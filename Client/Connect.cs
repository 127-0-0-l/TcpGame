﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    public partial class MainWindow
    {
        TcpClient client;
        NetworkStream ns;
        Thread thread;

        private void grdConnect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Connect(IPAddress.Parse(tbIP.Text));
                rctConnectIndicator.Fill = colorIndicatorGreen;
                grdConnect.IsEnabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Connect(IPAddress ip)
        {
            int port = 5000;
            client = new TcpClient();
            client.Connect(ip, port);
            ns = client.GetStream();
            thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);
        }

        private void Disconnect()
        {
            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
        }

        private void ReceiveData(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;
            string s;

            while ((byte_count = stream.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                s = Encoding.Unicode.GetString(receivedBytes, 0, byte_count);

                switch (s)
                {
                    case "red":
                        playerColor = PlayerColor.Red;
                        break;
                    case "blue":
                        playerColor = PlayerColor.Blue;
                        break;
                    case "start":
                        EnableMap();
                        break;
                    default:
                        Dispatcher.Invoke(() =>
                        {
                            PlayerColor plColor = playerColor == PlayerColor.Blue ? PlayerColor.Red : PlayerColor.Blue;
                            bool isVertical = s[0] == 'v';

                            PaintLine(
                                isVertical,
                                int.Parse(s[1].ToString()),
                                int.Parse(s[2].ToString()),
                                plColor
                            );
                            IncrementSquareTag(
                                int.Parse(s[1].ToString()),
                                int.Parse(s[2].ToString()),
                                plColor
                            );

                            if (isVertical)
                            {
                                IncrementSquareTag(
                                    int.Parse(s[1].ToString()) + 1,
                                    int.Parse(s[2].ToString()),
                                    plColor
                                );
                            }
                            else
                            {
                                IncrementSquareTag(
                                    int.Parse(s[1].ToString()),
                                    int.Parse(s[2].ToString()) + 1,
                                    plColor
                                );
                            }
                            EnableMap();
                        });
                        break;
                }
            }
        }
    }
}