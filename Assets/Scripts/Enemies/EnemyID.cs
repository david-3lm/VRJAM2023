using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyID : MonoBehaviour
{
    public GameLoop gameLoop;

    public int vida = 100;

    public string type;
    
    public void ReceiveDamage(int damage)
    {
        vida -= damage;
        if (vida < 0) gameLoop.DestroyEnemy(this.gameObject, type);
    }
}
