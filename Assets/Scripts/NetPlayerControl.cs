using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetPlayerControl : NetworkBehaviour {
    public float sensitivity;
    TankControl tankControl;

    float periodSvrRpc = 0.02f; //как часто сервер шлёт обновление картинки клиентам, с.
    float timeSvrRpcLast = 0; //когда последний раз сервер слал обновление картинки
    
        // Use this for initialization
        void Start () {
        tankControl = GetComponent<TankControl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.isLocalPlayer)
        {
            if (Input.GetAxis("Left") > sensitivity)
            {
                tankControl.CmdMoveLeft();
            }
            if (Input.GetAxis("Right") > sensitivity)
            {
                tankControl.CmdMoveRight();
            }
            if (Input.GetAxis("Down") > sensitivity)
            {
                tankControl.CmdMoveDown();
            }
            if (Input.GetAxis("Up") > sensitivity)
            {
                tankControl.CmdMoveUp();
            }
            if (Input.GetAxis("Shoot") > 0.5)
            {
                tankControl.CmdShoot();
            }
        }

    }
}
