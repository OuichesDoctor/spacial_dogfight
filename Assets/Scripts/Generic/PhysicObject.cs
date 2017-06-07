using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour {

    public    float             moveSpeed           = 5f;
    public    LayerMask         collisionMask;

    protected bool              _cancelMove         = false;
    protected RaycastHit2D      _cancelHit          = new RaycastHit2D();
    protected Vector2           _direction          = Vector2.zero;

    protected List<Collider2D>  _touching           = new List<Collider2D>();

    void FixedUpdate () {
        _cancelMove = false;
        var colliders = GetComponentsInChildren<Collider2D>();
        var currentPos = (Vector2) gameObject.transform.position;
        var currentAngle = gameObject.transform.rotation.eulerAngles.z;
        var timedDirection = _direction * moveSpeed * Time.fixedDeltaTime;
        var nextPos = currentPos + timedDirection;
        var hitColliders = new List<Collider2D>();
        bool              _hitHorizontally    = false;
        bool              _hitVertically      = false;
        RaycastHit2D closestHorizontalHit = new RaycastHit2D();
        RaycastHit2D closestVerticalHit = new RaycastHit2D();

        foreach (var col in colliders) {
            RaycastHit2D bestHit = new RaycastHit2D();
            if(typeof(BoxCollider2D) == col.GetType()) {
                bestHit = Physics2D.BoxCast(currentPos, col.bounds.size, currentAngle, timedDirection.normalized, timedDirection.magnitude, collisionMask);
                Debug.DrawLine(currentPos, nextPos, Color.red);
            }
            else if(typeof(CircleCollider2D) == col.GetType()) {
                var circleCol = (CircleCollider2D)col;
                bestHit = Physics2D.CircleCast(currentPos, circleCol.radius, timedDirection.normalized, timedDirection.magnitude, collisionMask);
                Debug.DrawLine(currentPos, nextPos, Color.red);
                Debug.DrawLine(currentPos, bestHit.point, Color.green);

            }
            else if(typeof(CapsuleCollider2D) == col.GetType()) {
                var capsuleCol = (CapsuleCollider2D)col;
                bestHit = Physics2D.CapsuleCast(currentPos, capsuleCol.size, capsuleCol.direction, currentAngle, timedDirection.normalized, timedDirection.magnitude, collisionMask);
                Debug.DrawLine(currentPos, nextPos, Color.red);
            }
            else if (typeof(PolygonCollider2D) == col.GetType()) {
                var polygonCol = (PolygonCollider2D)col;
                var closest = Mathf.Infinity;
                RaycastHit2D hit = new RaycastHit2D();
                foreach(var point in polygonCol.points) {
                    hit = Physics2D.Raycast(point, timedDirection.normalized, timedDirection.magnitude, collisionMask);
                    Debug.DrawLine(currentPos, nextPos, Color.red);
                    if (hit.collider != null && hit.distance < closest) {
                        bestHit = hit;
                        closest = hit.distance;
                        break;
                    }
                }
            }

            if(bestHit.collider != null) {
                if(!Array.Exists(colliders, x => x == bestHit.collider)) {
                    if(!hitColliders.Contains(bestHit.collider))
                        hitColliders.Add(bestHit.collider);
                    SendMessageUpwards("OnPOCollisionEnter", new object[] { col, bestHit.collider, bestHit }, SendMessageOptions.DontRequireReceiver);
                    bestHit.collider.gameObject.SendMessageUpwards("OnPOCollisionEnter", new object[] { bestHit.collider, col, bestHit }, SendMessageOptions.DontRequireReceiver);
                    if (!bestHit.collider.isTrigger) {
                        var vector = bestHit.point - currentPos;
                        var angle = Vector2.Angle(vector, _direction);
                        if (angle <= 120f)
                            _cancelMove = true;

                        var deltaX = Mathf.Abs(vector.x);
                        var deltaY = Mathf.Abs(vector.y);

                        if (deltaX > 0.01f) {
                            if (closestHorizontalHit.collider == null || deltaX < Mathf.Abs((closestHorizontalHit.centroid - currentPos).x)) {
                                closestHorizontalHit = bestHit;
                            }

                            _hitHorizontally = true;
                        }

                        if (deltaY > 0.01f) {
                            if (closestVerticalHit.collider == null || deltaY < Mathf.Abs((closestVerticalHit.centroid - currentPos).y)) {
                                closestVerticalHit = bestHit;
                            }

                            _hitVertically = true;
                        }
                    }
                }
            }
        }

        foreach(var t in _touching) {
            if(!hitColliders.Contains(t)) {
                SendMessage("OnPOCollisionLeave", new object[] { t }, SendMessageOptions.DontRequireReceiver);
            }
        }

        _touching = hitColliders;

        if(!_cancelMove) {
            gameObject.transform.position = nextPos;
        }
        else {
            Vector2 newPos = Vector2.zero;
            var hOrigin = closestHorizontalHit.point - timedDirection;
            var vOrigin = closestVerticalHit.point - timedDirection;
            var deltaX = hOrigin - currentPos;
            var deltaY = vOrigin - currentPos;
            //var newX = (closestHorizontalHit.point - deltaX).x - 0.01f * Mathf.Sign(timedDirection.x);
            //var newY = (closestVerticalHit.point - deltaY).y - 0.01f * Mathf.Sign(timedDirection.y);
            var newX = closestHorizontalHit.centroid.x;
            var newY = closestVerticalHit.centroid.y;

            if (_hitHorizontally && _hitVertically) {
                newPos = new Vector2(newX, newY);
            }
            else if (_hitHorizontally) {
                newPos = new Vector2(newX, nextPos.y);
            }
            else if (_hitVertically) {
                newPos = new Vector2(nextPos.x, newY);
            }
            else {
                // Hit to close, don't move
                newPos = currentPos;
                gameObject.transform.position = (Vector3)newPos;
                return;
            }

            gameObject.transform.position = (Vector3)newPos;
        }
	}

    public void CancelNextMove(RaycastHit2D hit) {
        _cancelHit = hit;
        _cancelMove = true;
    }

    public void SetDirection(Vector2 direction) {
        _direction = direction;
    }
}
