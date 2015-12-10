using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;

public class LevelBuilder : NetworkBehaviour {

    public Component wall;//1
    public Component hardWall;//2
    public Component gameBase;//3
    public Component water;//4
    public Component forest;//5
    public List<Component> objects;
    private bool build = true;
    private bool createdBase = false;

    public static int n = 12;
    public static int m = 18;
    public Tiles[,] level;

	int[,] LoadLevelFromFile(string name)
	{
		int [,] intLevel;
		using (StreamReader file = new StreamReader(@"maps\"+name))
		{
			int n = System.Convert.ToInt32(file.ReadLine());
			int m = System.Convert.ToInt32(file.ReadLine());
			intLevel = new int[n,m];
			for (int i = 0; i < intLevel.GetLength(0); i++) 
			{
				for(int j = 0; j < intLevel.GetLength(1);j++)
				{
					intLevel[i,j] = System.Convert.ToInt32(file.ReadLine());
				}
			}
		}
		return intLevel;
	}
	
	void GenMap()
    {
        /*int[,] intLevel = new int[,] {  { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 4, 1, 1, 4, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 2, 2, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 1, 1, 0, 0, 0, 5, 5, 0, 2, 2, 0, 5, 5, 0, 0, 0, 1, 1,},
                                        { 3, 1, 0, 0, 0, 5, 5, 0, 2, 2, 0, 5, 5, 0, 0, 0, 1, 3,},
                                        { 1, 1, 0, 0, 0, 5, 5, 0, 2, 2, 0, 5, 5, 0, 0, 0, 1, 1,},
                                        { 0, 0, 0, 0, 0, 5, 5, 4, 1, 1, 4, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                        { 0, 0, 0, 0, 0, 5, 5, 0, 1, 1, 0, 5, 5, 0, 0, 0, 0, 0,},
                                         };*/
		int[,] intLevel = LoadLevelFromFile ("test");
            level = new Tiles[intLevel.GetLength(0), intLevel.GetLength(1)];
            for (int i = 0; i < intLevel.GetLength(0); i++)
            {
                for (int j = 0; j < intLevel.GetLength(1); j++)
                {
                    level[i, j] = (Tiles)intLevel[i, j];
                }
            }

        n = level.GetLength(0);
        m = level.GetLength(1);
        float width = m * 0.16f;
        float height = n * 0.16f;
        TankControl.maxX = (width * 2) - 0.24f;
        TankControl.maxY = (height * 2) - 0.24f;

        //Camera.main.GetComponent<CameraScript>().adjustCamera();

        BuildLevel(level);
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (this.isServer && build)
        {
            GenMap(); // костыль
            build = false;
        }
    }
    public void BuildLevel()
    {
        BuildLevel(level);
    }
    public void BuildLevel(Tiles[,] level)
    {
        createdBase = false;
        for (int i = 0; i < level.GetLength(0); i++) {
            for(int j = 0; j < level.GetLength(1);j++){
                Component block = new Component();
                switch (level[i, j])
                {
                    case Tiles.Empty:
                        continue;
                    case Tiles.Wall:
                        block = wall;
                        break;
                    case Tiles.HardWall:
                        block = hardWall;
                        break;
                    case Tiles.GameBase:
                        block = gameBase;
                        break;
                    case Tiles.Water:
                        block = water;
                        break;
                    case Tiles.Forest:
                        block = forest;
                        break;
                }
                int yCoordinate = level.GetLength(0) - i-1;
                Component currBlock = Instantiate(
                block,
                new Vector3(j * 0.32f, yCoordinate * 0.32f, 0),
                new Quaternion()) as Component;
                if (level[i, j] == Tiles.GameBase)
                {
                    if (!createdBase)
                    {
                        currBlock.GetComponent<BaseHealth>().side = 1;
                        createdBase = true;
                    }
                    else
                    {
                        currBlock.GetComponent<BaseHealth>().side = 2;
                    }
                    NetworkServer.Spawn(currBlock.gameObject);
                }
                else if (level[i, j] == Tiles.Wall)
                {
                    currBlock.GetComponent<Wall>().Spawn();
                }
                else
                {
                    NetworkServer.Spawn(currBlock.gameObject);
                }
                objects.Add(currBlock);
            }
        }
    }
    public void RestartLevel() {
        RestartLevel(level);
    }

    public void RestartLevel(Tiles[,] level)
    {
        foreach(Component obj in objects)
        {
            NetworkServer.Destroy(obj.gameObject);
        }
        objects.Clear();
        deleteBullets();
        BuildLevel(level);
    }
    public static void RestartLevelStatic()
    {
       Spawner[] spawners =  Component.FindObjectsOfType<Spawner>();
        foreach(Spawner spawner in spawners)
        {
            spawner.lives = Spawner.defaultLives;
            NetworkServer.Destroy(spawner.tank.gameObject);
        }
        Component.FindObjectOfType<LevelBuilder>().RestartLevel();
    }
    public void deleteBullets()
    {
            foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            {
                NetworkServer.Destroy(bullet.gameObject);
            }
     
    }
}

public enum Tiles
{
    Empty,Wall,HardWall,GameBase,Water,Forest
}