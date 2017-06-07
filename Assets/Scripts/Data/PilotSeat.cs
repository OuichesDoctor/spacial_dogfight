using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PilotSeat : Seat {

    public override void ProcessDirection(Vector2 direction) {
        ship.SetMoveDirection(direction);
    }

    public override void ProcessLook(Vector2 look) {
        ship.SetLookDirection(look);
    }

    public override void ProcessFire() {

    }

    

}