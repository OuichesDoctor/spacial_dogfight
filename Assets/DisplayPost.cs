using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPost : MonoBehaviour {

    Text text;

    private void Start() {
        text = GetComponent<Text>();
    }

    void Update () {
        text.text = NetPlayer.local.serverPosition.ToString();		
	}
}
