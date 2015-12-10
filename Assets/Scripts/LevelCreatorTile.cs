using UnityEngine;
using System.Collections;

public class LevelCreatorTile : MonoBehaviour {

	public Sprite wall;//1
	public Sprite hardWall;//2
	public Sprite gameBase;//3
	public Sprite water;//4
	public Sprite forest;//5

	public int tileNum = 0;

	void OnMouseEnter() 
	{
		GetComponent<SpriteRenderer> ().sprite = GetSprite (FindObjectOfType<LevelCreator> ().currTile);
	}

	void OnMouseExit()
	{
		GetComponent<SpriteRenderer> ().sprite = GetSprite (tileNum);
	}

	void OnMouseUpAsButton()
	{
		tileNum = FindObjectOfType<LevelCreator> ().currTile;
		GetComponent<SpriteRenderer> ().sprite = GetSprite (tileNum);
	}

	public void RenderSprite()
	{
		GetComponent<SpriteRenderer> ().sprite = GetSprite (tileNum);
	}

	private Sprite GetSprite(int num)
	{
		switch (num) {
		case 1:
			return wall;
		case 2:
			return hardWall;
		case 3:
			return gameBase;
		case 4:
			return water;
		case 5:
			return forest;
		default:
			return null;
		}
	}
}
