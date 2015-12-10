using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour
{
    [SyncVar]
    public bool bigExplosion;

    public Sprite[] small;

    public Sprite[] big;

    private int i = 0;
    private SpriteRenderer spriteRenderer;


    private float time = 0.0f;
    private float delay = 0.05f;
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - time > delay)
        {
            time = Time.time;
            //if it's the last sprite, then destroy the object.
            if ((bigExplosion && i >= big.Length)
                || (!bigExplosion && i >= small.Length))
            {
                NetworkServer.Destroy(gameObject);
                return;
            }

            if (bigExplosion)
            {
                spriteRenderer.sprite = big[i++];
            }
            else
            {
                spriteRenderer.sprite = small[i++];
            }
        }
    }
}
