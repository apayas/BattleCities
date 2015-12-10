using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ScoringSystem : NetworkBehaviour {
    [SyncVar]
    public int team1Score = 0;
    [SyncVar]
    public int team2Score = 0;
    public Component scorePrefab;

    private float delay;
    private float start;
    private GameObject score;


    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (score != null && (Time.time - start) > delay)
        {
            NetworkServer.Destroy(score);
        }
    }
    
    public  void displayScore(int delay,int side)
    {
        if(side == 1)
        {
            team1Score++;
        }
        if(side == 2)
        {
            team2Score++;
        }
        Component score = Instantiate(
               scorePrefab,
               new Vector3(0, 0, 0),
               new Quaternion()) as Component;
        this.score = (score.gameObject);
        score.gameObject.GetComponentInChildren<Text>().text = team1Score + " : " + team2Score;
        NetworkServer.Spawn(score.gameObject);
        
        start = Time.time;
        this.delay = delay;
    }
}
