using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterParticleDestroy : MonoBehaviour {

    public GameObject toDestroy;

    private ParticleSystem _system;

	void Start () {
        _system = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if(_system.time > 4f) {
            var renderers = toDestroy.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
                r.enabled = false;
        }

        if (_system.isStopped) {
            GameObject.Destroy(toDestroy);
        }
	}
}
