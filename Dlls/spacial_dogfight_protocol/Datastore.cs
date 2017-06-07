using spacial_dogfight_protocol.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spacial_dogfight_protocol {

    public enum GameState {
        INIT,
        MAIN
    }

    public class Datastore {

        #region SINGLETON
        private static Datastore sInstance;
        public static Datastore Instance {
          get {
                if (sInstance == null)
                    sInstance = new Datastore();

                return sInstance;
          }
        }

        private Datastore() {
            _gameObjects = new Dictionary<KeyValuePair<int, int>, GameObjectState>();
            _playerSlots = new Dictionary<int, int>();
            _ships = new Dictionary<int, ShipState>();
        }
        #endregion

        private Dictionary<KeyValuePair<int, int>, GameObjectState> _gameObjects;
        private Dictionary<int, int> _playerSlots;
        private Dictionary<int, ShipState> _ships;

        public GameState gameState = GameState.INIT;

        public void StoreGameObject(GameObjectState go) {
            var key = new KeyValuePair<int, int>(go.playerID, go.gid);
            if (_gameObjects.ContainsKey(key))
                _gameObjects[key] = go;
            else {
                _gameObjects.Add(key, go);
            }
        }

        public GameObjectState RetrieveGameObject(int playerID, int gid) {
            var key = new KeyValuePair<int, int>(playerID, gid);
            if (_gameObjects.ContainsKey(key))
                return _gameObjects[key];
            else
                return null;
        }

        public int GetPlayerSlot(int playerID) {
            if (!_playerSlots.ContainsValue(playerID))
                return 0;

            return _playerSlots.Where(x => x.Value == playerID).First().Key;
        }

        public Dictionary<int, int> GetPlayerSlots() {
            return _playerSlots;
        }

        public bool IsSlotAvailable(int playerSlot) {
            if(_playerSlots.ContainsKey(playerSlot))
                return _playerSlots[playerSlot] == 0;

            return true;
        }

        public void TakeSlot(int playerSlot, int playerID) {
            if (_playerSlots.ContainsKey(playerSlot))
                _playerSlots[playerSlot] = playerID;
            else
                _playerSlots.Add(playerSlot, playerID);
        }

        public void ReleaseSlot(int playerSlot) {
            if(_playerSlots.ContainsKey(playerSlot))
                _playerSlots[playerSlot] = 0;
        }

        public int GetSlotOwner(int playerSlot) {
            if(_playerSlots.ContainsKey(playerSlot))
                return _playerSlots[playerSlot];

            return 0;
        }

        public void StoreShipState(ShipState state) {
            _ships[state.shipID] = state;
        }

        public ShipState GetShipState(int shipID) {
            if (_ships.ContainsKey(shipID))
                return _ships[shipID];

            return null;
        }
        
        public Dictionary<int, ShipState> GetShipStates() {
            return _ships;
        }
    }
}
