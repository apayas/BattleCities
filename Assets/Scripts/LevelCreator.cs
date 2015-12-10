using UnityEngine;
using System.Collections;
using System.IO;

public class LevelCreator : MonoBehaviour {

	public Component tilePrefab;
	public int currTile;

	private Component[,] tiles;
	private string name = "Map name";
	private int n = 12;
	private int m = 18;

	// Use this for initialization
	void Start () {
		tiles = new Component[n, m];
		for (int i = 0; i < tiles.GetLength(0); i++) 
		{
			for(int j = 0; j < tiles.GetLength(1);j++)
			{
				int yCoordinate = tiles.GetLength(0) - i-1;
				tiles[i,j] = Instantiate(
					tilePrefab,
					new Vector3(j * 0.32f, yCoordinate * 0.32f, 0),
					new Quaternion()) as Component;
			}
		}
	}
	
	void OnGUI()
	{
		name = GUI.TextField(new Rect(10, 10, 200, 20), name, 25);
		if (GUI.Button (new Rect(10, 30, 200, 50), "Save map")) 
		{
			using (StreamWriter file = new StreamWriter(@"maps\"+name))
			{
				file.WriteLine(n);
				file.WriteLine(m);
				for (int i = 0; i < tiles.GetLength(0); i++) 
				{
					for(int j = 0; j < tiles.GetLength(1);j++)
					{
						file.WriteLine(tiles[i,j].gameObject.GetComponent<LevelCreatorTile>().tileNum);
					}
				}
			}
		}
		if (GUI.Button (new Rect(10, 80, 200, 50), "Open map")) 
		{
			using (StreamReader file = new StreamReader(@"maps\"+name))
			{
				int newN = System.Convert.ToInt32(file.ReadLine());
				int newM = System.Convert.ToInt32(file.ReadLine());
				if (newN != n || newM != m)
				{
					for (int i = 0; i < tiles.GetLength(0); i++) 
					{
						for(int j = 0; j < tiles.GetLength(1);j++)
						{
							Destroy(tiles[i,j]);
						}
					}
					n = newN;
					m = newM;
					Start ();
				}
				for (int i = 0; i < tiles.GetLength(0); i++) 
				{
					for(int j = 0; j < tiles.GetLength(1);j++)
					{
						tiles[i,j].gameObject.GetComponent<LevelCreatorTile>().tileNum = System.Convert.ToInt32(file.ReadLine());
						tiles[i,j].gameObject.GetComponent<LevelCreatorTile>().RenderSprite();
					}
				}
			}
		}
	}
}
