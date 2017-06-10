using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TargetedShoot : ShootMode {

    private List<GameObject> _cannons;
    private NetShip _ship;

    public void Start() {
        _ship = GetComponent<NetShip>();
        
        // Collect cannons
        _cannons = new List<GameObject>(_ship.leftTurrets);
        _cannons.AddRange(_ship.rightTurrets);
        _cannons.AddRange(_ship.backTurrets);
        _cannons.AddRange(_ship.frontTurrets);
    }

    public override void DoShoot(AbstractShip myShip, Vector2 target) {
        foreach(var c in _cannons) {
            var projection = (Vector3)target - c.transform.position;

            // Check in cannons are facing the right direction
            Debug.Log(Vector2.Angle(c.transform.up, projection));
            if(Vector2.Angle(c.transform.up, projection) <= 5f) {
                var proj = GameObject.Instantiate(myShip.bullet, c.transform.position + c.transform.up * 1.5f, c.transform.rotation);
                proj.GetComponent<Projectile>().myShipId = ((NetShip)myShip).shipId;
                NetworkServer.Spawn(proj);
            }
        }
    }

    public override void ProcessLookDirection(AbstractShip myShip) {
        foreach (var c in _cannons) {
            // Check in cannons are facing the right direction
            var cannon = c.GetComponent<Cannon>();
            var mousePos = myShip.shipCamera.ScreenToWorldPoint(Input.mousePosition);
            var sightDirection = (Vector3)mousePos - c.transform.position;
            sightDirection.z = 0;
            cannon.SetSightDirection(sightDirection);
        }
    }

}
