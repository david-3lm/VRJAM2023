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

        rb.AddForce(transform.parent.forward.normalized * force);

        Invoke("TimeDestroy", lifeTime);
    }

    private void TimeDestroy()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController s_ec = other.GetComponent<EnemyController>();
        if(s_ec != null)
        {
            s_ec.ReceiveDamage(damage);
        }
    }
}
