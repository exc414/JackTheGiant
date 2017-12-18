using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public float speed = 8f;
	public float maxVelocity = 4f;

	private Rigidbody2D playerBody;
	private Animator animator;

	//Called first, Start second and then Update
	void Awake()
	{
		//Gets the component from the game object
		playerBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Start() 
	{
		
	}
	
	//This function gets called every single frame. 60 FPS = 60 calls
	void Update() { }

	//Gets updated every couple of frames. Could 3 to 4. Best place to put code related
	//to physics.
	void FixedUpdate()
	{
		playerMoveKeyboard();
	}

	void playerMoveKeyboard()
	{
		float forceX = 0f;
		float velocity = Mathf.Abs(playerBody.velocity.x);

		//Using raw it gives -1 (Left) 0 (Nothing) 1 (Right)
		//Gives the direction of the player.
		float horizontalAxis = Input.GetAxisRaw("Horizontal");

		if(horizontalAxis > 0)
		{
			//Moving to the right (forward). Apply positive speed
			if(velocity < maxVelocity)
			{
				forceX = speed;
			}
		}
		else if(horizontalAxis < 0)
		{
			if (velocity < maxVelocity)
			{
				//Moving to the left (backwards). Apply negative speed
				forceX = -speed;
			}
		}

		//Apply the set force onto the player depending on forward/backward speed.
		playerBody.AddForce(new Vector2(forceX, 0));
	}

} //Player
