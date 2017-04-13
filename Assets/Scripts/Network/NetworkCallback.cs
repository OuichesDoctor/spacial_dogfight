using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProtocol;

public abstract class NetworkCallback : MonoBehaviour {

    private void Start() {
        Register();
    }

    public abstract void Register();
    public abstract void ProcessUpdate(Update update);

}
