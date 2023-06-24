using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanyonBullet : MonoBehaviour
{
    public Rigidbody rb;

    private int damage;

    private float force;

    private float lifeTime = 5.0f;
    public void SetBullet(int d, float f)
    {
        damage = d;
        force = f;

        rb.AddForce(transform.forward.normalized * force);

        Invoke("TimeDestroy", lifeTime);
    }

    private void TimeDestroy()
    {
        Destroy(gameObject);
    }
}
