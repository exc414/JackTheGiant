using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public float speed = 8f;
	public float maxVelocity = 4f;

	private Rigidbody2D playerBody;
	private Animator animator;

	private Vector3 scale;

	private float defaultScaleXValue = 1.3f;

	//Called first, Start second and then Update
	void Awake()
	{
		//Gets the component from the game object
		playerBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Start() { }
	
	//This function gets called every single frame. 60 FPS = 60 calls
	void Update() { }

	//Gets updated every couple of frames. Could 3 to 4. Best place to put code related
	//to physics.
	void FixedUpdate() { playerMoveKeyboard(); }

	void playerMoveKeyboard()
	{
		float forceX = 0f;
		float velocity = Mathf.Abs(playerBody.velocity.x);


		//Using raw it gives -1 (Left) 0 (Nothing) 1 (Right)
		//Gives the direction of the player.
		float horizontalAxis = Input.GetAxisRaw("Horizontal");

		//Imagine that the middle of the screen is 0. If moving to the right
		//it would need positive numbers. If moving to the left of it would need
		//negative numbers.
		if(horizontalAxis > 0) //Right side
		{
			//Moving to the right (forward). Apply positive speed
			if(velocity < maxVelocity)
			{
				forceX = speed;
			}

			//Sprite looks to the right
			playerFacing(defaultScaleXValue);
			isWalking(true);
		}
		else if(horizontalAxis < 0) //Left side
		{
			if(velocity < maxVelocity)
			{
				//Moving to the left (backwards). Apply negative speed
				forceX = -speed;
			}

			//Sprite looks to the left. Note the negative symbol
			playerFacing(-defaultScaleXValue);
			isWalking(true);
		}
		else { isWalking(false); }

		//Apply the set force onto the player depending on forward/backward speed.
		playerBody.AddForce(new Vector2(forceX, 0));
	}

	//Animate from idle to walking and walking to idle
	private void isWalking(bool isWalking)
	{
		animator.SetBool("Walk", isWalking);
	}

	//Local scale gives a reference to the scale set in the unity editor.
	//Basically on positive scale the player sprite will look to the right.
	//This is how it was drawn. If the scale is negative it will look to the left.
	//If the player moves to the left scaleX will negative and right positive.
	private void playerFacing(float scaleX)
	{
		//Vector 3 - X, Y, Z here we only want the X.
		scale = transform.localScale;
		scale.x = scaleX;
		transform.localScale = scale;
	}

} //Player
