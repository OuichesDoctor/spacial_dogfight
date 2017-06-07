using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDriving : MonoBehaviour {

    public GameObject pilotSlot;
    public GameObject gunnerSlot;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public bool useInertie = false;
    public Camera shipCamera;

    private Vector2 _directionThrust;
    private Vector2 _directionLook;

    private Rigidbody2D _rb2d;

	void Start () {
        CameraManager.Instance.closeCamera = shipCamera;
        CameraManager.Instance.shipControl = this;
        CameraManager.Instance.distantCamera.GetComponent<CameraFollowTarget>().target = gameObject;
        _rb2d = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        var xAcceleration = Input.GetAxis("Horizontal");	
        var yAcceleration = Input.GetAxis("Vertical");

        _directionThrust = new Vector2(xAcceleration, yAcceleration);

        var xLookAt = Input.GetAxis("HorizontalZ");
        var yLookAt = Input.GetAxis("VerticalZ");

        _directionLook = new Vector2(xLookAt, yLookAt);
        if (_directionLook.magnitude < 0.5)
            _directionLook = Vector2.zero;
    }

    void FixedUpdate() {
        if(useInertie)
            _rb2d.AddForce(_directionThrust.normalized * moveSpeed * Time.fixedDeltaTime * _rb2d.mass, ForceMode2D.Impulse);
        else
            _rb2d.MovePosition((Vector2)gameObject.transform.position + _directionThrust.normalized * moveSpeed * Time.fixedDeltaTime);

        if (_directionLook != Vector2.zero) {
            var currentTransform = gameObject.transform;
            var zAngle = Vector2.Angle(_directionLook, currentTransform.up);
            var rightAngle = Vector2.Angle(_directionLook, currentTransform.right);
            var sign = -1;
            if (rightAngle > 90)
                sign = 1;

            if (Mathf.Abs(currentTransform.rotation.z - zAngle) > 2) {
                _rb2d.rotation = currentTransform.rotation.eulerAngles.z + rotationSpeed * Time.fixedDeltaTime * sign;           
            }
        }
    }
}
