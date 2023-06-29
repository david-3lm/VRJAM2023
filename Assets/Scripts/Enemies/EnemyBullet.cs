using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody rb;

    private int damage;

    private float force;

    private float lifeTime = 3.0f;

    public void ShootBullet(int d, float f)
    {
        damage = d;
        force = f;

        rb.AddForce(transform.parent.forward.normalized * force);

        Invoke("TimeDestroy", lifeTime);
    }

    private void TimeDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameLoop s_gl = other.GetComponent<GameLoop>();
        if (s_gl != null)
        {
            s_gl.DecreasePLayerLife(damage);
        }
    }
}
