using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BaseHealth : Health {
    //две команды : команда 1 и команда 2
    [SyncVar]
    public int side;
    public Sprite ruinedBase;
	public override void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = ruinedBase;
            RpcDestroyBase();
        }
    }

    [ClientRpc(channel = 0)]
    void RpcDestroyBase()
    {
        if (this.isClient)
        {
            GetComponent<SpriteRenderer>().sprite = ruinedBase;
            if(side == 1)
            {
                FindObjectOfType<ScoringSystem>().displayScore(3,2);
            }
            if (side == 2)
            {
                FindObjectOfType<ScoringSystem>().displayScore(3,1);
            }
            LevelBuilder.RestartLevelStatic();
            
        }
    }
}
