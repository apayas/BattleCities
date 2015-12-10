using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : MonoBehaviour {
    public GameObject tank;
    public NetworkConnection conn;
    public short playerControllerId;
    public Component tankPrefab;
    public Component invincibilityPrefab;
    public int lives = 3;
    public const int defaultLives = 3;
    public Spawner(NetworkConnection connection, short id)
    {
        conn = connection;
        playerControllerId = id;
    }
	// Use this for initialization
	void Start () {
        if (tank == null)
        {
            tank = conn.playerControllers[0].gameObject;
            //spawnTank();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(conn != null && tank == null && lives > 0)
        {
            spawnTank();
            addInvincibility();
            lives--;
        }
        if (lives <= 0) {
            LevelBuilder.RestartLevelStatic();
        }
	}
    void spawnTank()
    {
        Component tankInstance = Instantiate(
            tankPrefab,
            transform.position,
            new Quaternion()) as Component;
        NetworkServer.ReplacePlayerForConnection(conn, tankInstance.gameObject, playerControllerId);
        tank = tankInstance.gameObject;
    }
    void addInvincibility()
    {
        Component invincibility = Instantiate(
            invincibilityPrefab,
            new Vector3(),
            new Quaternion()) as Component;
        invincibility.GetComponent<Invincibility>().owner = tank;
        NetworkServer.Spawn(invincibility.gameObject);
    }
    static int idCounter = 0;
}
