using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    
    void Start()
    {
    }
    public int health;
    //public int maxHealth;
    public virtual void TakeDamageFromBullet(int amount,Bullet bullet)
    {
        TakeDamage(amount);
    }
    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
