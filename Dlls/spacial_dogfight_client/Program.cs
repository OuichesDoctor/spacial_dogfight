using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace spacial_dogfight_client
{
    class Program {
        static void Main(string[] args) {
            Boolean done = false;
            Boolean exception_thrown = false;
            #region comments 
            // create an address object and populate it with the IP address that we will use
            // in sending at data to. This particular address ends in 255 meaning we will send
            // to all devices whose address begins with 192.168.2.
            // However, objects of class IPAddress have other properties. In particular, the
            // property AddressFamily. Does this constructor examine the IP address being
            // passed in, determine that this is IPv4 and set the field. If so, the notes
            // in the help file should say so.
            #endregion
            IPAddress send_to_address = IPAddress.Parse("192.168.0.19");
            #region comments
            // IPEndPoint appears (to me) to be a class defining the first or final data
            // object in the process of sending or receiving a communications packet. It
            // holds the address to send to or receive from and the port to be used. We create
            // this one using the address just built in the previous line, and adding in the
            // port number. As this will be a broadcase message, I don't know what role the
            // port number plays in this.
            #endregion
            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, int.Parse(ConfigurationManager.AppSettings["listenPort"]));
            #region comments
            // The below three lines of code will not work. They appear to load
            // the variable broadcast_string witha broadcast address. But that
            // address causes an exception when performing the send.
            //
            //string broadcast_string = IPAddress.Broadcast.ToString();
            //Console.WriteLine("broadcast_string contains {0}", broadcast_string);
            //send_to_address = IPAddress.Parse(broadcast_string);
            #endregion
            Console.WriteLine("Enter text to broadcast via UDP.");
            Console.WriteLine("Enter a blank line to exit the program.");
            var listener = new UdpClient();
            while (!done) {
                Console.WriteLine("Enter text to send, blank line to quit");
                string text_to_send = Console.ReadLine();
                if (text_to_send.Length == 0) {
                    done = true;
                }
                else {
                    // the socket object must have an array of bytes to send.
                    // this loads the string entered by the user into an array of bytes.
                    byte[] send_buffer = { 0, 1 };
                    send_buffer = send_buffer.Concat(Encoding.UTF8.GetBytes(text_to_send)).ToArray();

                    // Remind the user of where this is going.
                    Console.WriteLine("sending to address: {0} port: {1}",
                    sending_end_point.Address,
                    sending_end_point.Port);
                    try {
                        listener.Send(send_buffer, send_buffer.Length, sending_end_point);
                        byte[] rec_buffer = listener.Receive(ref sending_end_point);
                        var received_data = Encoding.UTF8.GetString(rec_buffer);
                        Console.WriteLine(received_data);
                    }
                    catch (Exception send_exception) {
                        exception_thrown = true;
                        Console.WriteLine(" Exception {0}", send_exception.Message);
                    }
                    if (exception_thrown == false) {
                        Console.WriteLine("Message has been sent to the broadcast address");
                    }
                    else {
                        exception_thrown = false;
                        Console.WriteLine("The exception indicates the message was not sent.");
                    }
                }
            } // end of while (!done)
        } // end of main()
    } // end of class Program
}
