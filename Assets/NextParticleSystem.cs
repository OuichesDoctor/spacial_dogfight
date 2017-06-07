using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextParticleSystem : MonoBehaviour {

    public ParticleSystem nextSystem;

    private ParticleSystem _currentSystem;

	void Start () {
        _currentSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (_currentSystem.isStopped) {
            nextSystem.gameObject.SetActive(true);
        }
	}
}
