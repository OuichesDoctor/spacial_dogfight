using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour {

    public static NetworkSpawner Instance { get; private set; }

    public GameObject[] spawnPoint;

    public NetworkSpawner() {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this);
    }

    public GameObject GetRandomSpawnPoint() {
        var index = Random.Range(0, spawnPoint.Length - 1);
        return spawnPoint[index];
    }

}
