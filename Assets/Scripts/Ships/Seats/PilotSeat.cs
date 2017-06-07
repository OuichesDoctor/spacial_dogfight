using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ships.Seats {
    public class PilotSeat : Seat {

        private Vector2 _directionThrust;
        private Vector2 _directionLook;

        private Rigidbody2D _rb2d;
        private Transform _transform;
        private float _moveSpeed;
        private float _rotationSpeed;

        public PilotSeat(float moveSpeed, float rotationSpeed) {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
        }

        public override void Initialize(GameObject ship) {
            _rb2d = ship.GetComponent<Rigidbody2D>();
            _transform = ship.GetComponent<Transform>();
            ship.AddComponent<NetworkShipPosition>();
        }

        public override void ProcessLeftStick(Vector2 input) {
            _directionThrust = input;
        }

        public override void ProcessRightStick(Vector2 input) {
            if (input.magnitude < 0.5)
                input = Vector2.zero;

            _directionLook = input;
        }

        public override void ProcessKey(string key) {
            throw new NotImplementedException();
        }

        public override void FixedUpdate() {
            _rb2d.MovePosition((Vector2)_transform.position + _directionThrust.normalized * _moveSpeed * Time.fixedDeltaTime);

            if (_directionLook != Vector2.zero) {
                var currentTransform = _transform;
                var zAngle = Vector2.Angle(_directionLook, currentTransform.up);
                var rightAngle = Vector2.Angle(_directionLook, currentTransform.right);
                var sign = -1;
                if (rightAngle > 90)
                    sign = 1;

                if (Mathf.Abs(currentTransform.rotation.z - zAngle) > 2) {
                    _rb2d.rotation = currentTransform.rotation.eulerAngles.z + _rotationSpeed * Time.fixedDeltaTime * sign;
                }
            }
        }
    }
}
