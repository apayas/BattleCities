using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TankControl : NetworkBehaviour
{
    //movement
    public float speed;
    private Vector3 velocity;
    private Vector3 serverPosition;
    private Vector3 interpolateMove;
    private float serverTime;

    //bounds fields
    public static float maxX = 2.66f;
    public static float maxY = 1.63f;
	public static float minX = -0.3f;//-0.01f;
	public static float minY = -0.3f;//-0.03f;

    //animation fields
    public Sprite tank1;
    public Sprite tank2;
    public float animationDelay;
    private float lastAnimationChange;

    //shooting fields
    public float shotDelay;
    public float bulletSpeed;
    float timeOfLastShot = -1000;
    public Component bullet;
    private int damage = 1;

    private new Rigidbody2D rigidbody;

    float periodSvrRpc = 0.02f; //как часто сервер шлёт обновление картинки клиентам, с.
    float timeSvrRpcLast = 0; //когда последний раз сервер слал обновление картинки

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (this.isServer)
        {
			//fix of collision
			Vector3 direction = transform.up;
			direction.Scale(new Vector3(0.2f, 0.2f, 0.2f));
			if (Physics2D.Raycast(transform.position + direction, direction, speed).collider != null)
			{
				velocity.Scale(new Vector3(0.1f, 0.1f, 0.1f));
			}
            //keeping tank inside of bounds
            Vector2 newPosition = new Vector2(transform.position.x + velocity.x, transform.position.y + velocity.y);
            if (newPosition.x > maxX)
            {
                transform.position = new Vector3(maxX, transform.position.y, 0);
                velocity = new Vector3();
            }
            if (newPosition.x < minX)
            {
                transform.position = new Vector3(minX, transform.position.y, 0);
                velocity = new Vector3();
            }
            if (newPosition.y > maxY)
            {
                transform.position = new Vector3(transform.position.x, maxY, 0);
                velocity = new Vector3();
            }
            if (newPosition.y < minY)
            {
                transform.position = new Vector3(transform.position.x, minY, 0);
                velocity = new Vector3();
            }

            //if tank is moving
            if (velocity.magnitude != 0)
            {
                //move tank and change sprite direction
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                transform.position += velocity;
                transform.up = velocity;
                velocity = new Vector3(0, 0, 0);
            }

            if (timeSvrRpcLast + periodSvrRpc < Time.time)
            //Если пора, то выслать координаты всем моим аватарам
            {
                timeSvrRpcLast = Time.time;
                RpcUpdateUnitPosition(this.transform.position);
                RpcUpdateUnitOrientation(this.transform.rotation);
            }
        }
        if (this.isClient)
        {
            if (transform.position != serverPosition)
            {
                //if it's time to change sprite
                //if (Time.time - lastAnimationChange > animationDelay) {
                lastAnimationChange = Time.time;
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer.sprite == tank1)
                {
                    spriteRenderer.sprite = tank2;
                }
                else
                {
                    spriteRenderer.sprite = tank1;
                }
                //}
                //move tank and change sprite direction
                
                //transform.position += interpolateMove;
                //transform.up = velocity;
            }
        }
    }

    [Command(channel = 0)]
    public void CmdMoveRight()
    {
        velocity = new Vector3(speed, 0,0);
    }
    [Command(channel = 0)]
    public void CmdMoveLeft()
    {
        velocity = new Vector3(-speed, 0,0);
    }
    [Command(channel = 0)]
    public void CmdMoveDown()
    {
        velocity = new Vector3(0,-speed,0);
    }
    [Command(channel = 0)]
    public void CmdMoveUp()
    {
        velocity = new Vector3(0, speed,0);
    }
    [Command(channel = 0)]
    public void CmdShoot()
    {
        if ((Time.time - timeOfLastShot) > shotDelay)
        {

            Vector3 direction = transform.up;
            Vector3 offset = transform.up * 0.16f;

            Component bulletInstance = Instantiate(
                bullet,
                transform.position + offset,
                new Quaternion()) as Component;
            bulletInstance.GetComponent<Bullet>().velocity = new Vector3(direction.x, direction.y, 0) * bulletSpeed;
            bulletInstance.GetComponent<Bullet>().damage = damage;
            bulletInstance.GetComponent<Bullet>().owner = gameObject;
            NetworkServer.Spawn(bulletInstance.gameObject);
            timeOfLastShot = Time.time;
        }
        //throw new System.NotImplementedException();
    }

    [ClientRpc(channel = 0)]
    void RpcUpdateUnitPosition(Vector3 posNew)
    {
        if (this.isClient)
        {
            serverPosition = posNew;
            this.transform.position = posNew;
        }
    }
    [ClientRpc(channel = 0)]
    void RpcUpdateUnitOrientation(Quaternion oriNew)
    {
        if (this.isClient)
        {
            this.transform.rotation = oriNew;
        }
    }

}
