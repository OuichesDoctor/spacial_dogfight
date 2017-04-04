using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProtocol {
    public abstract class Command {

        public static Dictionary<string,Command> commandList;

        static Command() {
            var typeList = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.IsSubclassOf(typeof(Command))
                            select type;

            commandList = new Dictionary<string,Command>();
            foreach (var type in typeList) {
                var cmd = (Command)Activator.CreateInstance(type);
                commandList.Add(cmd.Category + "x" + cmd.Code, cmd);
            }
        }

        public static Command GetCommand(ref byte[] received_data) {
            if(received_data.Length < 2) {
                Console.WriteLine("invalid packet received.");
                return null;
            }

            var category = (int) received_data[0];
            var code = (int) received_data[1];

            byte[] dataBuffer = new byte[received_data.Length - 2];
            Buffer.BlockCopy(received_data, 2, dataBuffer, 0, received_data.Length - 2);
            received_data = dataBuffer;

            var id = category + "x" + code;
            if(commandList.ContainsKey(id)) {
                return commandList[id];
            }

            return null;
        }

        public int Category { get; protected set; }
        public int Code { get; protected set; }
        public Message Data { get; protected set; }

        public abstract byte[] BuildCommand();
        public abstract Update ProcessCommand(PlayerSession player);
        public abstract string DebugString();
    }
}
