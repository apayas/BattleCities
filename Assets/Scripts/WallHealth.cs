using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WallHealth : Health
{
    public Sprite wall_0;
    public Sprite wall_1;
    public Sprite wall_2;
    public Sprite wall_3;
    public Sprite wall_4;
    public Sprite wall_5;
    public Sprite wall_6;
    public Sprite wall_7;
    public Sprite wall_8;
    public Sprite wall_9;
    public Sprite wall_10;
    public Sprite wall_11;
    public Sprite wall_12;
    public Sprite wall_13;
    public Sprite wall_14;
    public Sprite wall_15;

    private GameObject wall;
    [SyncVar]
    int id;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = GetSprite(id);
    }
    

    public void Init(int sprite, GameObject wallRef)
    {
        GetComponent<SpriteRenderer>().sprite = GetSprite(sprite);
        gameObject.tag = "wallShard";
        wall = wallRef;
        id = sprite;
        NetworkServer.Spawn(gameObject);
    }

    public override void TakeDamageFromBullet(int amount, Bullet bullet)
    {
        Component otherWall;
        //if bullet is moving horizontally
        if (Mathf.Abs(bullet.velocity.x) > Mathf.Abs(bullet.velocity.y))
        {
            otherWall = wall.GetComponent<Wall>().GetWallShard(id ^ 4);
        }
        else
        {
            otherWall = wall.GetComponent<Wall>().GetWallShard(id ^ 1);
        }
        if (otherWall != null)
        {
            NetworkServer.Destroy(otherWall.gameObject);//.GetComponent<Health>().TakeDamage(amount);
        }
        NetworkServer.Destroy(gameObject);
        base.TakeDamage(amount);
    }
    
    Sprite GetSprite(int n)
    {
        switch (n)
        {
            case 0: return wall_0;
            case 1: return wall_1;
            case 2: return wall_2;
            case 3: return wall_3;
            case 4: return wall_4;
            case 5: return wall_5;
            case 6: return wall_6;
            case 7: return wall_7;
            case 8: return wall_8;
            case 9: return wall_9;
            case 10: return wall_10;
            case 11: return wall_11;
            case 12: return wall_12;
            case 13: return wall_13;
            case 14: return wall_14;
            case 15: return wall_15;
        }
        return null;
    }
}
