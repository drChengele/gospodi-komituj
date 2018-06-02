using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour {

    private Vector3 rotationVector;
    private float rotationSpeed;
    [Range(0f,1000f)]
    public float minRotationSpeed;
    [Range(0f, 1000f)]
    public float maxRotationSpeed;

    [Range(1,1000)]
    public int maxHitPoints;
    private int currentHitPoints;
    
    void Awake () {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        GetComponent<Rigidbody>().AddTorque(rotationVector * rotationSpeed);
        currentHitPoints = maxHitPoints;
	}

    void KillMe()
    {
        Destroy(this.gameObject);
    }

    void TakeDamage(int dmg)
    {
        var hp = currentHitPoints - dmg;
        if (hp > 0) currentHitPoints = hp;
        else KillMe();
    }

    //dmg other stuff maybe
    void InflictDamage(int dmg)
    {

    }
}
