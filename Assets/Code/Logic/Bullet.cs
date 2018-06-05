using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float maxLifetime;
    [SerializeField] float speed;

    float lifeLeft;
    private void Awake() {
        lifeLeft = maxLifetime;
    }
    private void Update() {
        lifeLeft -= Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        if (lifeLeft <= 0f) Destroy(gameObject);

        // raycast
        var ray = new Ray(transform.position, transform.forward);
        var hits = Physics.SphereCastAll(ray, 1f, speed * Time.deltaTime, ObjectManager.Instance.WorldLayerMask);
        foreach (var hit in hits) {
            if (hit.collider.gameObject != gameObject) {
                Hit(hit.collider.gameObject);
            }
        }
    }

    private void Hit(GameObject gameObject) {
        if (gameObject.tag == "Asteroid") {
            ObjectManager.Instance.GameManager.AsteroidWasHitByBullet(this, gameObject);
        } else if (gameObject.tag == "Canister") {
            ObjectManager.Instance.GameManager.CanisterWasHitByBullet(this, gameObject);
        }
    }

    internal void DieSoon() {
        lifeLeft = -1f;
    }

    public void Fired() {

    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.tag == "Asteroid") {
    //        ObjectManager.Instance.GameManager.AsteroidWasHitByBullet(this, other.gameObject);
    //    }
    //    if (other.tag == "Canister") {
    //        ObjectManager.Instance.GameManager.CanisterWasHitByBullet(this, other.gameObject);
    //    }
    //}
}