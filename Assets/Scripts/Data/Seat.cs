using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Seat : MonoBehaviour {

    public static Seat None = null;

    public AbstractShip ship;
    public bool available = true;
    public float nextAvailable = 0;
    public int index;

    public abstract void ProcessDirection(Vector2 direction);
    public abstract void ProcessLook(Vector2 look);
    public abstract void ProcessFire();

}
