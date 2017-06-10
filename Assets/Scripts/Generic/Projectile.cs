using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float moveSpeed = 5f;
    public float lifeSpan = 2f;
    public Vector2 direction;
    public int myShipId;
    public GameObject explosionPrefab;

    private float _programmedDeath;
    private PhysicObject _po;

	void Start () {
        direction = gameObject.transform.up;
        _programmedDeath = Time.realtimeSinceStartup + lifeSpan;
        _po = GetComponent<PhysicObject>();
	}
	
	void Update () {
        if (Time.realtimeSinceStartup > _programmedDeath)
            Destroy(gameObject);
	}

    void OnPOCollisionEnter(object[] result) {
        var coll = (Collider2D)result[1];
        var netShip = coll.gameObject.GetComponent<NetShip>();
        if (netShip != null && netShip.shipId == myShipId)
            return;

        if (coll.gameObject.layer == LayerMask.NameToLayer("ships") || coll.gameObject.layer == LayerMask.NameToLayer("rooms")) {
            GameObject.Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        _po.SetDirection(direction);
    }
}
