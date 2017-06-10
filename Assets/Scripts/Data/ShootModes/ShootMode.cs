using UnityEngine;

public abstract class ShootMode : MonoBehaviour {

    public abstract void ProcessLookDirection(AbstractShip myShip);
    public abstract void DoShoot(AbstractShip myShip, Vector2 target);

}
