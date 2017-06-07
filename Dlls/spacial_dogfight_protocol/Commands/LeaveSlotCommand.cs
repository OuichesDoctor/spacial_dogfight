using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
    public class LeaveSlotCommand : Command {
        public LeaveSlotCommand() {
            Category = 1;
            Code = 5;
            Data = new LeaveSlotMessage();
        }

        public override string DebugString() {
            var jMessage = (LeaveSlotMessage)Data;
            return "LEAVE";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var jMessage = (LeaveSlotMessage)Data;
            var datastore = Datastore.Instance;

            var currentSlotOwner = datastore.GetSlotOwner(jMessage.playerSlot);
            if (currentSlotOwner == jMessage.playerID) {
                datastore.ReleaseSlot(jMessage.playerSlot);
            }

            var update = new JoinUpdate();
            ((JoinMessage)update.Data).playerSlots = datastore.GetPlayerSlots();

            return update;
        }
    }
}
