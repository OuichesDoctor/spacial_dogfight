using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProtocol {
    public abstract class Update {

        public const Update None = null;
        public static Dictionary<string, Update> updateList;

        static Update() {
            var typeList = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           where type.IsSubclassOf(typeof(Update))
                           select type;

            updateList = new Dictionary<string, Update>();
            foreach (var type in typeList) {
                var update = (Update)Activator.CreateInstance(type);
                updateList.Add(update.Category + "x" + update.Code, update);
            }
        }

        public static Update GetUpdate(ref byte[] received_data) {
            if (received_data.Length < 2) {
                Console.WriteLine("invalid packet received.");
                return null;
            }

            var category = (int)received_data[0];
            var code = (int)received_data[1];

            byte[] dataBuffer = new byte[received_data.Length - 2];
            Buffer.BlockCopy(received_data, 2, dataBuffer, 0, received_data.Length - 2);
            received_data = dataBuffer;

            var id = category + "x" + code;
            if (updateList.ContainsKey(id)) {
                return updateList[id];
            }

            return null;
        }

        public int Category { get; protected set; }
        public int Code { get; protected set; }
        public Message Data { get; protected set; }

        public abstract void ProcessUpdate();
        public abstract string DebugString();
        public abstract byte[] BuildUpdate(PlayerSession player);
    }
}
