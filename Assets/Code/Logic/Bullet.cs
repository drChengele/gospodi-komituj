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
        if (lifeLeft <= 0f) {
            Destroy(gameObject);
        }
    }

    public void Fired() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Asteroid") {
            ObjectManager.Instance.GameManager.AsteroidWasHitByBullet(this, other.gameObject);
        }
        if (other.tag == "Canister") {
            ObjectManager.Instance.GameManager.CanisterWasHitByBullet(this, other.gameObject);
        }
    }
}