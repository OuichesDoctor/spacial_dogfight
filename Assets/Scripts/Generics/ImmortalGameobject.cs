using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalGameobject : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject);
	}
}
