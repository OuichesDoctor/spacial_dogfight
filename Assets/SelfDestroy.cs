using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

    public float destroyAfter = 3f;

    protected float _destroyAt = 0f;

	void Start () {
        _destroyAt = Time.realtimeSinceStartup + destroyAfter;		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup > _destroyAt)
            GameObject.Destroy(gameObject);
	}
}
