using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance { get; private set; }

    public Camera distantCamera;
    public Camera closeCamera;

    public ShipDriving shipControl;
    public PlayerControl playerControl;

    public void Start() {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this);

        distantCamera = Camera.main;
    }

    void Update () {
        if(playerControl == null) {
            playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        }

        if (Input.GetButtonDown("Fire3")) {
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
