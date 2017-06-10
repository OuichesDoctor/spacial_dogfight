using UnityEngine;
using UnityEngine.Networking;

public class LateralizedShoot : ShootMode {

    public override void DoShoot(AbstractShip myShip, Vector2 target) {
        var currentX = myShip.turretLook.x;
        var currentY = myShip.turretLook.y;
        var absX = Mathf.Abs(currentX);
        var absY = Mathf.Abs(currentY);

        GameObject[] cannons;
        if (absX > absY) {
            if (currentX > 0) {
                cannons = myShip.rightTurrets;
            }
            else {
                cannons = myShip.leftTurrets;
            }
        }
        else {
            if (currentY > 0) {
                cannons = myShip.frontTurrets;
            }
            else {
                cannons = myShip.backTurrets;
            }
        }

        foreach (var go in cannons) {
            var proj = GameObject.Instantiate(myShip.bullet, go.transform.position + go.transform.up * 1, go.transform.rotation);
            proj.GetComponent<Projectile>().myShipId = ((NetShip)myShip).shipId;
            NetworkServer.Spawn(proj);
        }
    }

    public override void ProcessLookDirection(AbstractShip myShip) {

    }

}