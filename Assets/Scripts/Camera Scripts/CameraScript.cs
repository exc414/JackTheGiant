using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{
	private float speed = 1f;
	private float accel = 0.2f;
	private float maxSpeed = 3.2f;

	//Annotation used when you need a public variable but dont want to see if in the
	//inspector.
	[HideInInspector]
	public bool moveCameraBool;
	
	void Start() 
	{
		moveCameraBool = true;
	}
	
	void Update() 
	{
		if(moveCameraBool) { moveCamera(); }
	}

	private void moveCamera()
	{
		Vector3 cameraPosition = transform.position;

		float startY = cameraPosition.y;
		float endY = cameraPosition.y - (speed * Time.deltaTime);

		//The Clamp function will not let the cameraPosition.y variable be
		//less than the startY or more than the endY.
		cameraPosition.y = Mathf.Clamp(cameraPosition.y, startY, endY);

		//Assign the new position to the camera's position.
		transform.position = cameraPosition;

		//Increase the speed over time.
		speed += accel * Time.deltaTime;

		//However when the speed gets to be bigger than the maxSpeed set it
		//to the maxSpeed
		if(speed > maxSpeed) { speed = maxSpeed; }
	}
}
