using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public Seat mySeat;
    public string joypadId;

	void Update () {
        if (isLocalPlayer) {
            var xAxis = Input.GetAxis("Horizontal" + joypadId);
            var yAxis = Input.GetAxis("Vertical" + joypadId);


            var xLookAt = Input.GetAxis("HorizontalZ" + joypadId);
            var yLookAt = Input.GetAxis("VerticalZ" + joypadId);

            var directionLook = new Vector2(xLookAt, yLookAt);
            if (directionLook.magnitude < 0.5)
                directionLook = Vector2.zero;

            if(directionLook == Vector2.zero) {
                var mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                directionLook = (Vector2)mousePos - (Vector2)gameObject.transform.position;
                if (directionLook.magnitude < 0.5)
                    directionLook = Vector2.zero;
            }

            CmdSendDirection(new Vector2(xAxis, yAxis).normalized, directionLook.normalized);

            if(Input.GetButton("Fire1" + joypadId)) {
                CmdSendFire();
            }
        }
    }

    [Command]
    void CmdSendDirection(Vector2 moveDirection, Vector2 lookDirection) {
        mySeat.ProcessDirection(moveDirection);
        mySeat.ProcessLook(lookDirection);
    }

    [Command]
    void CmdSendFire() {
        mySeat.ProcessFire();
    }
}
