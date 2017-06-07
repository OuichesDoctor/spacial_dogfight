using Assets.Scripts.Ships.Seats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainSceneManager : MonoBehaviour {

    public static MainSceneManager Instance { get; protected set; }

    public float moveSpeed;
    public float rotationSpeed;
    public GameObject[] ships;
    public Seat mySeat;
    public GameObject myShip;

	void Start () {
        if(Instance != null) {
            DestroyImmediate(this);
            return;
        }

        Instance = this;

        var netMgr = NetworkManager.Instance;
        var currentSlot = (from slot in netMgr.playerSlots where slot.Value == netMgr.playerID select slot.Key).First<int>();
        switch(currentSlot) {
            case 1:
                SetShip(0);
                mySeat = new PilotSeat(moveSpeed, rotationSpeed);
                break;
            case 2:
                SetShip(0);
                mySeat = new GunnerSeat();
                break;
            case 3:
                SetShip(1);
                mySeat = new PilotSeat(moveSpeed, rotationSpeed);
                break;
            case 4:
                SetShip(1);
                mySeat = new GunnerSeat();
                break;
            default:
                return;
        }

        mySeat.Initialize(myShip);
	}
	
	void Update () {
        var leftStickX = Input.GetAxis("Horizontal");
        var leftStickY = Input.GetAxis("Vertical");

        var leftStickVector = new Vector2(leftStickX, leftStickY);
        mySeat.ProcessLeftStick(leftStickVector);

        var rightStickX = Input.GetAxis("HorizontalZ");
        var rightStickY = Input.GetAxis("VerticalZ");

        var rightStickVector = new Vector2(rightStickX, rightStickY);
        mySeat.ProcessRightStick(rightStickVector);
    }

    void FixedUpdate() {
        mySeat.FixedUpdate();
    }

    private void SetShip(int shipID) {
        myShip = ships[shipID];
        var cam = Camera.main.gameObject;
        var followTarget = cam.AddComponent<CameraFollowTarget>();
        followTarget.target = myShip;
    }
}
