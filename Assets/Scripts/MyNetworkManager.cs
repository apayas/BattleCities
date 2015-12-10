using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public GameObject spawnerPrefab;

    private ArrayList spawners;



    public override void OnServerConnect(NetworkConnection conn)
    {
        
    }
    public override void OnStartServer()
    {
        spawners = new ArrayList();
    }
    public override void OnStopServer()
    {
        foreach (GameObject spawner in spawners)
        {
            Destroy(spawner);
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        float x = 0;
        float y = 0;
        if (spawners.Count == 1)
        {
            x = LevelBuilder.m * 0.32f;
        }
        GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(x, y), new Quaternion());
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        GameObject spawner = (GameObject)Instantiate(spawnerPrefab, new Vector3(x, y), new Quaternion());
        spawner.GetComponent<Spawner>().playerControllerId = playerControllerId;
        spawner.GetComponent<Spawner>().conn = conn;
        spawners.Add(spawner);

        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {

        foreach (GameObject spawner in spawners)
        {
            if (spawner.GetComponent<Spawner>().conn == conn)
            {
                GameObject tank = spawner.GetComponent<Spawner>().tank;
                spawners.Remove(spawner);
                Destroy(spawner);
                NetworkServer.Destroy(tank);
                break;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
