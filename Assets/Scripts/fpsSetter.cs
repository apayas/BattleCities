using UnityEngine;
using System.Collections;

public class fpsSetter : MonoBehaviour {
    public int fps =24;
    void Awake()
    {
        fps = 24;
        
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.targetFrameRate != fps)
        {
            Application.targetFrameRate = fps;
        }
    }
}
