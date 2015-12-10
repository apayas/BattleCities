using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MenuScript : NetworkBehaviour {
    const int NETWORK_PORT = 4585;
    const int MAX_CONNECTIONS = 6;
    const bool USE_NAT = false;
    [SyncVar]
    public bool serverStarted = false;

    private string remoteServer = "127.0.0.1";

    public void CreateButtonClick()
    {
        MyNetworkManager networkManager = FindObjectOfType<MyNetworkManager>();
        NetworkClient nc = networkManager.StartHost();
        if (nc != null)
        {
            FindObjectOfType<Canvas>().gameObject.SetActive(false);
        }
    }
    public void ConnectButtonClick()
    {
        MyNetworkManager networkManager = FindObjectOfType<MyNetworkManager>();
        
        NetworkClient nc = networkManager.StartClient();
        //нужна проверка подключился ли клиент или нет
        if (nc != null)
        {
            FindObjectOfType<Canvas>().gameObject.SetActive(false);
        }
        
    }
}
