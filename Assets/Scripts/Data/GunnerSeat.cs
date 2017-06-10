using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunnerSeat : Seat {

    public override void ProcessDirection(Vector2 direction) {
        
    }

    public override void ProcessLook(Vector2 look) {
        ship.SetTurretLook(look);
    }

    public override void ProcessFire() {
        var mousePos = Input.mousePosition;
        mousePos = ship.shipCamera.ScreenToWorldPoint(mousePos);
        ship.Fire(mousePos);
    }
}