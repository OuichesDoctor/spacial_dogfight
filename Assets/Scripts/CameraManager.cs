using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera distantCamera;
    public Camera closeCamera;

    public ShipDriving shipControl;
    public PlayerControl playerControl;

	void Update () {
        if (Input.GetButtonDown("Fire1")) {
            if (distantCamera.enabled) {
                distantCamera.enabled = false;
                closeCamera.enabled = true;
                shipControl.enabled = false;
                playerControl.enabled = true;
            }
            else {
                distantCamera.enabled = true;
                closeCamera.enabled = false;
                shipControl.enabled = true;
                playerControl.enabled = false;
            }
        }
	}
}
