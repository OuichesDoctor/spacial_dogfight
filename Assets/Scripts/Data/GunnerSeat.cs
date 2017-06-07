using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunnerSeat : Seat {

    public override void ProcessDirection(Vector2 direction) {
        if (direction.magnitude > 0.5f) {
            ship.SetTurretLook(direction);
            ship.Fire();
        }
    }

    public override void ProcessLook(Vector2 look) {
    }

    public override void ProcessFire() {

    }

}