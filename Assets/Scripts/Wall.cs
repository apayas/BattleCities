using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Wall : MonoBehaviour {
    public Component wallShard;
    Component[] wall;

    void Start()
    {
        
    }

    public void Spawn()
    {
        if (wall == null) wall = new Component[16];
        int index = 0;
        for (float i = -0.12f; i <= 0.12f; i += 0.08f)
        {
            for (float j = -0.12f; j <= 0.12f; j += 0.08f)
            {
                //wall[index] = new WallHealth(index, new Vector3(j, i));
                Component go = Instantiate(wallShard, transform.position + new Vector3(j, i), new Quaternion()) as Component;
                go.GetComponent<WallHealth>().Init(index, gameObject);
                wall[index] = go;
                index++;
            }
        }
    }
    public Component GetWallShard(int index)
    {
        return wall[index];
    }
}
