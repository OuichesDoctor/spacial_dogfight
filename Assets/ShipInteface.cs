using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInteface : MonoBehaviour {

    public NetShip ship;
    public Slider healthDisplay;

	void Update () {
        healthDisplay.value = ship.health / 100;
	}

    void LateUpdate() {
        gameObject.transform.localRotation = Quaternion.Inverse(ship.gameObject.transform.rotation);
    }
}
