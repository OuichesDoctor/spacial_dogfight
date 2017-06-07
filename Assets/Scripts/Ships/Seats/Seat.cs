using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ships.Seats {
    public abstract class Seat {
        public abstract void Initialize(GameObject ship);
        public abstract void ProcessLeftStick(Vector2 input);
        public abstract void ProcessRightStick(Vector2 input);
        public abstract void ProcessKey(string key);
        public abstract void FixedUpdate();
    }
}
