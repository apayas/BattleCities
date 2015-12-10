using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
    public Vector3 velocity;
    public int damage;
    public GameObject owner;
    private bool destroyed = false;

    float periodSvrRpc = 0.02f; //как часто сервер шлёт обновление картинки клиентам, с.
    float timeSvrRpcLast = 0; //когда последний раз сервер слал обновление картинки

    public Component explosion;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (this.isServer)
        {
            if(col.tag == "water" || col.gameObject == owner)
            {
                return;
            }
            Health health = col.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamageFromBullet(damage, this);
            }
            destroyed = true;


            Component explosionInstance = Instantiate(
                explosion,
                transform.position,
                new Quaternion()) as Component;
            explosionInstance.GetComponent<Explosion>().bigExplosion = false;
            NetworkServer.Spawn(explosionInstance.gameObject);
            
        }
    }
    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.isServer)
        {
            if (destroyed)
            {
                NetworkServer.Destroy(gameObject);
            }
            transform.position += velocity;
            if (timeSvrRpcLast + periodSvrRpc < Time.time)
            //Если пора, то выслать координаты всем моим аватарам
            {
                RpcUpdateUnitPosition(this.transform.position);
                timeSvrRpcLast = Time.time;
            }
        }
    }
    [ClientRpc(channel = 0)]
    void RpcUpdateUnitPosition(Vector3 posNew)
    {
        if (this.isClient)
        {
            this.transform.position = posNew;
        }
    }
}
