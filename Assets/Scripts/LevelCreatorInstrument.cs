using UnityEngine;
using System.Collections;

public class LevelCreatorInstrument : MonoBehaviour {
	
	public int spriteNum;

	void OnMouseUpAsButton()
	{
		FindObjectOfType<LevelCreator> ().currTile = spriteNum;
	}
}
