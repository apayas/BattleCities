using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        adjustCamera();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void adjustCamera() {
        int n = LevelBuilder.n;
        int m = LevelBuilder.m;
        float width = m * 0.16f;
        float height = n * 0.16f;
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = height * 1.2f;
        transform.position = new Vector3(width , height , -10) + new Vector3(-0.16f, -0.16f, 0);
        //transform.position = camera.ViewportToWorldPoint(new Vector3(1, 1, 0))+new Vector3(-0.16f,-0.16f,0);
    }

}
