using GameProtocol;
using spacial_dogfight_protocol.Messages;
using spacial_dogfight_protocol.States;
using spacial_dogfight_protocol.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol.Commands {
    public class ShipMoveCommand : Command {

        public ShipMoveCommand() {
            Category = 1;
            Code = 7;
            Data = new ShipMoveMessage();
        }

        public override string DebugString() {
            return "Position Command";
        }

        public override Update ProcessCommand(PlayerSession player) {
            var data = (ShipMoveMessage)Data;
            var dataStore = Datastore.Instance;

            var ships = dataStore.GetShipStates();
            lock(ships) {
                var state = new ShipState();
                state.shipID = data.shipId;
                state.x = data.x;
                state.y = data.y;
                state.rotation = data.rotation;
                dataStore.StoreShipState(state);
            }

            var update = new ShipMoveUpdate();
            ((ShipMoveMessage)update.Data).shipId = data.shipId;
            ((ShipMoveMessage)update.Data).x = data.x;
            ((ShipMoveMessage)update.Data).y = data.y;
            ((ShipMoveMessage)update.Data).rotation = data.rotation;

            return update;
        }
    }
}
