using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour 
{
	private GameObject[] backgrounds;
	private float lastBackgroundPositionY;
	
	void Start() 
	{
		getBackgroundAndSetLastYPosition();
	}

	void getBackgroundAndSetLastYPosition()
	{
		//Gets all game objects in the scene with the tag Background.
		backgrounds = GameObject.FindGameObjectsWithTag("Background");

		lastBackgroundPositionY = backgrounds[0].transform.position.y;

		//Compare all other background objects position with the first element
		//positions.
		for(int i = 1; i < backgrounds.Length; i++)
		{
			//Compare Y Axis position if bigger then assign that position to the last 
			//Y position. Remember we go downwards thus we use greater than.
			if(lastBackgroundPositionY > backgrounds[i].transform.position.y)
			{
				lastBackgroundPositionY = backgrounds[i].transform.position.y;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D target)
	{
		if(target.tag == "Background")
		{
			if(target.transform.position.y == lastBackgroundPositionY)
			{
				Vector3 saveLastBackgroundPosition = target.transform.position;
				float height = ((BoxCollider2D)target).size.y;

				for(int i = 0; i < backgrounds.Length; i++)
				{
					if(!backgrounds[i].activeInHierarchy)
					{
						saveLastBackgroundPosition.y -= height;
						lastBackgroundPositionY = saveLastBackgroundPosition.y;
						backgrounds[i].transform.position = saveLastBackgroundPosition;
						backgrounds[i].SetActive(true);
					}
				}
			}
		}
	}
}
