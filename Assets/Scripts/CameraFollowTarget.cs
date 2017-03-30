using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour {

    public GameObject target;
    public bool matchRotation;

	void LateUpdate () {
        gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        if(matchRotation)
            gameObject.transform.rotation = target.transform.rotation;
	}
}
