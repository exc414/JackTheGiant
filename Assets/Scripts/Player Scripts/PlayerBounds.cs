using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour 
{
	private float leftBound;
	private float rightBound;
	
	void Start() { setMinAndMaxX(); }

	void Update()
	{
		if(transform.position.x < leftBound)
		{
			Vector3 playerPos = transform.position;
			playerPos.x = leftBound;
			transform.position = playerPos;
		}

		if (transform.position.x > rightBound)
		{
			Vector3 playerPos = transform.position;
			playerPos.x = rightBound;
			transform.position = playerPos;
		}
	}

	/**
	 * Set the max and min position on the X axis that the player can spawn at.
	 */
	private void setMinAndMaxX()
	{
		//Convert screen coordinates to unity's world coordinates
		//Vector 3 = X, Y, Z
		Vector3 screenBounds = new Vector3(Screen.width, Screen.height, 0);
		Vector3 worldBounds = Camera.main.ScreenToWorldPoint(screenBounds);

		//Get the full width of the player model. Apply it to the
		//total bounds (on left and right) this makes it so that the player cannot go
		//in half way into the end edge of the world.
		float width = GetComponent<SpriteRenderer>().bounds.size.x / 2;

		rightBound = worldBounds.x - width;
		leftBound = -worldBounds.x + width;
	}
}
