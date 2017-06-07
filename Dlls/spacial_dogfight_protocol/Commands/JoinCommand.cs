using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
    public class JoinCommand : Command {

        public JoinCommand() {
            Category = 1;
            Code = 4;
            Data = new JoinMessage();
        }

        public override string DebugString() {
            var jMessage = (JoinMessage)Data;
            return "JOIN";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var jMessage = (JoinMessage)Data;
            var datastore = Datastore.Instance;

            int currentSlotOwner;
            foreach(var slot in jMessage.playerSlots) {
                currentSlotOwner = datastore.GetSlotOwner(slot.Key);
                if(currentSlotOwner != slot.Value) {
                    if (currentSlotOwner == 0 && datastore.GetPlayerSlot(slot.Value) == 0) {
                        datastore.TakeSlot(slot.Key, slot.Value);
                    }
                }
            }

            var update = new JoinUpdate();
            ((JoinMessage)update.Data).playerSlots = jMessage.playerSlots;

            return update;
        }

    }
}
