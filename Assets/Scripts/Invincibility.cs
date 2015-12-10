using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Invincibility : NetworkBehaviour
{

    public float duration = 3;
    public float startTime;

    private int initialHealth;
    //animation
    public Sprite sprite1;
    public Sprite sprite2;
    // Use this for initialization
    public GameObject owner;
	void Start () {
        startTime = Time.time;
        if (owner != null)
        {
            Health health = owner.GetComponent<Health>();
            if (health != null)
            {
                //temporarily increases health to max, making parent object kind of invincible
                initialHealth = health.health;
                health.health = int.MaxValue;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (owner != null)
        {
            transform.position = owner.transform.position;
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == sprite1)
        {
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            spriteRenderer.sprite = sprite1;
        }
        if (Time.time - startTime > duration) {

            if (owner != null)
            {
                Health health = owner.GetComponent<Health>();
                if (health != null)
                {
                    //return health to normal
                    health.health = initialHealth;
                }
            }       
            NetworkServer.Destroy(gameObject);
        }
	}
}
