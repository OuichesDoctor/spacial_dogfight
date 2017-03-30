using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipModuleBehaviour : MonoBehaviour {

    public int health;

    public void Update() {
        PreModuleUpdate();

        if (health <= 0)
            Death();

        PostModuleUpdate();

        GameObject.Destroy(gameObject);
    }

    protected void Death() {
        Debug.Log("Le module est détruit");
    }

    protected abstract void PreModuleUpdate();
    protected abstract void PostModuleUpdate();

}
