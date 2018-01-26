using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour 
{
	//SerializeField allows this clouds field to be available in the unity editor.
	//Use this instead of public on the variable so that other classes cannot see this variable.
	[SerializeField]
	private GameObject[] clouds;

	//Distance on the Y Axis between the clouds.
	//Needs to be at least 1.3/5 the size of the player.
	private float distanceBetweenClouds = 3f;

	//Cloud position on the X axis. Do not want our clouds to overflow. Meaning go out of 
	//the camera bounds.
	private float minX;
	private float maxX;

	//Knowning the last cloud's position is necessary because it will tell us where we must
	//put the next clouds below it.
	private float lastCloudPositionY;

	//Controls the position at which the clouds spawn in respect of the X axis.
	private float controlCloudPositionX;

	//Clouds that get collected
	[SerializeField]
	private GameObject[] collectables;

	//Reference to the player so that he can be place in the correct starting position.
	private GameObject player;

	//Use this for initialization
	void Awake() 
	{
		controlCloudPositionX = 0;
		setMinAndMaxX();
		createClouds();

		//Find the player
		player = GameObject.Find("Player");
	}

	void Start()
	{
		PositionThePlayer();
	}
	
	/**
	 * Set the max and min position on the X axis that the clouds can spawn at.
	 */
	private void setMinAndMaxX()
	{
		//Convert screen coordinates to unity's world coordinates
		//Vector 3 = X, Y, Z
		Vector3 screenBounds = new Vector3(Screen.width, Screen.height, 0);
		Vector3 worldBounds = Camera.main.ScreenToWorldPoint(screenBounds);

		//Subtract 0.5 so that the cloud cannot be potentially placed further past
		//the edge of the world.
		//Always imagine that at the middle point of the world is 0. So whatever the total
		//width. Say 6. Means left to mid = -3 to 0 and mid to right 0 -> 3
		maxX = worldBounds.x - 0.5f;
		minX = -worldBounds.x + 0.5f; //Since its the min we must ADD instead of subtract.
		//print("Max X Pos : " + maxX + " Min X Pos : " + minX);
	}

	/**
	 * Shuffle/Randomized at which places the clouds are created/spawn at.
	 */
	private void shuffleGameObjects(GameObject[] arrayToShuffle)
	{
		int random;
		int arrLength = arrayToShuffle.Length;

		for(int i = 0; i < arrLength; i++)
		{
			//Save the first game object or else you will loose when you assign it
			//a random game object below.
			GameObject oldGameObject = arrayToShuffle[i];
			random = Random.Range(i, arrLength);
			//Assign a new random object to this position in the array
			arrayToShuffle[i] = arrayToShuffle[random];
			//Save the oldGameObject to the random index's value.
			arrayToShuffle[random] = oldGameObject;
		}
	}

	private void createClouds()
	{
		//Randomized the clouds
		shuffleGameObjects(clouds);

		//Starts at zero because we want to start the Y position at the center of the screen.
		//X position is randomize.
		float positionY = 0f;

		for(int i = 0; i < clouds.Length; i++)
		{
			//Get the vector3 positions for the current cloud.
			Vector3 cloudPosition = clouds[i].transform.position;

			//Starts at the middle of the screen.
			cloudPosition.y = positionY;

			//X on the other hands start randomized.
			//However, the min and max bounds are used to make sure that the cloud will not
			//be display outside of the bouds of the world.
			cloudPosition.x = Random.Range(minX, maxX);

			//Set the clouds X axis positioning.
			cloudPosition.x = setCloudPosition();

			//Save the current's cloud position. Once the loop is over we will have the
			//last cloud's position.
			lastCloudPositionY = positionY;

			//Assign the new X and Y axis values
			clouds[i].transform.position = cloudPosition;
			positionY -= distanceBetweenClouds;
		}
	}

	/**
	 * If a dark cloud (cloud that if you touch it kills you) gets shuffle into
	 * spot number 1 or 2. Then it must be reallocated with a normal cloud. This
	 * is because if this is not done the player will immediatly.
	 */
	private void PositionThePlayer()
	{
		GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
		GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

		for(int i = 0; i < darkClouds.Length; i++)
		{
			//When true this would be mean that the dark cloud is the first cloud on the
			//screen. Therefore we switch it with a normal cloud.
			if(darkClouds[i].transform.position.y == 0f)
			{
				//Get the dark cloud position so that it can be applied to the normal cloud
				//that is going to be swapped.
				Vector3 firstDarkCloudPosition = darkClouds[i].transform.position;
				Vector3 normalCloud = new Vector3(
					cloudsInGame[0].transform.position.x, 
					cloudsInGame[0].transform.position.y, 
					cloudsInGame[0].transform.position.z);

				//Set bac cloud at good cloud position (not first position).
				darkClouds[i].transform.position = normalCloud;

				//Set good cloud at bad cloud position.
				cloudsInGame[0].transform.position = firstDarkCloudPosition;
			}
		}

		Vector3 savedCloudPosition = cloudsInGame[0].transform.position;

		//Start at i at 1 because we already have the first's item position.
		//Go up the cloud chain until the upper most cloud's position is retrieved.
		//The player will be position at this cloud.
		for(int i = 1; i < cloudsInGame.Length; i++)
		{
			if(savedCloudPosition.y < cloudsInGame[i].transform.position.y)
			{
				savedCloudPosition = cloudsInGame[i].transform.position;
			}
		}

		//Add the player's sprite height (and a little more) so that the player will be
		//on top of the clouds. Else the player will be under the cloud.
		savedCloudPosition.y += 0.8f;
		player.transform.position = savedCloudPosition;
	}

	//Executes when a cloud collides with the cloud spawner.
	void OnTriggerEnter2D(Collider2D target)
	{
		//Filter for cloud objects only.
		if(target.tag == "Cloud" || target.tag == "Deadly")
		{
			//The lastCloudPosition comes from the createCloud() method.
			if(target.transform.position.y == lastCloudPositionY)
			{
				//Shuffle all game items before re-laying them out again.
				shuffleGameObjects(clouds);
				shuffleGameObjects(collectables);

				Vector3 targetCloudPoistion = target.transform.position;

				for(int i = 0; i < clouds.Length; i++)
				{
					//If the cloud is not active, meaning is set to GONE. Then
					//activated and position it. Maintaining the distance between
					//clouds in mind. Cloud Collector makes the cloud objects not
					//be active in the hierarchy.
					if(!clouds[i].activeInHierarchy)
					{
						//Set the clouds X axis positioning.
						targetCloudPoistion.x = setCloudPosition();

						//Set target clouds Y axis coordinates and activate it.
						targetCloudPoistion.y -= distanceBetweenClouds;

						//Save the current's cloud position. Once the loop is over we will have 
						//the last cloud's position.
						lastCloudPositionY = targetCloudPoistion.y;

						clouds[i].transform.position = targetCloudPoistion;
						clouds[i].SetActive(true);
					}
				}
			}
		}
	}

	private float setCloudPosition()
	{
		//At the start randomized the X position of the cloud from the MID (0) position to
		//the MAX (3) position which means the cloud will show up somewhere on the RIGHT
		//quandrant of the world. 0 -> MAX WIDTH is right. Then set the position to 1.
		//On the next iteration if its 1 we know that the previous cloud was set to the 
		//RIGHT quadrant. Meaning this one should be to the left. Thus we choose a number
		//for the x position smaller than zero. 0 -> -3 LEFT. The next else if follow
		//the same pattern they just shorten the positions that the clouds could be at
		//to give variation for the cloud placement.
		if (controlCloudPositionX == 0)
		{
			controlCloudPositionX = 1;
			return Random.Range(0.0f, maxX);
		}
		else if(controlCloudPositionX == 1)
		{
			controlCloudPositionX = 2;
			return Random.Range(0.0f, minX);
		}
		else if(controlCloudPositionX == 2)
		{
			controlCloudPositionX = 3;
			return Random.Range(1.0f, maxX);
		}
		else if(controlCloudPositionX == 3)
		{
			controlCloudPositionX = 0;
			return Random.Range(-1.0f, minX);
		}

		return Random.Range(0.0f, maxX);
	}
}