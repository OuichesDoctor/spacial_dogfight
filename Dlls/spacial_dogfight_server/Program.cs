using GameServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer {
    /// <summary>
    /// Entry point of the server execution
    /// </summary>
    class Program {

        /// <summary>
        /// Instantiate server and start it.
        /// </summary>
        public static void Main() {
            var server = new MyServer();
            var error = server.Initialize();
            if (error == null)
                server.Start();
            else
                Console.WriteLine(error);

            while (server.Status != ServerStatus.Stopped) ;

            Console.WriteLine("Exiting...");
        }
    }
}
