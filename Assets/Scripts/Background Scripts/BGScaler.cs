using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour 
{

	//Use this for initialization
	void Start () 
	{
		//Scale the background to the window width
		SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
		
		Vector3 bgScale = transform.localScale;

		//print("BG Scale from localScale : " + bgScale);

		//Gives the width of the sprite (background image)
		float width = spriteRender.sprite.bounds.size.x;

		//print("Sprite Render Bound Size X : " + width);

		//Camera orthographicSize gives half of the world's size.
		//Thus must be multiply by 2 to get the full height.
		//You can see this by counting the grid square on the unity editor.
		float worldHeight = Camera.main.orthographicSize * 2f;

		//print("World Height from Camera - orthographicSize: " + worldHeight);

		//This is the formula to give width of the Camera meaning the world size.
		//The camera in this game is 6 units wide. Which is the result this caculation
		//will give. 10 / 800 * 480 = 6
		float worldWidth = worldHeight / Screen.height * Screen.width;

		//print("Screen Height : " + Screen.width);
		//print("Screen Height : " + Screen.height);
		
		//print("World Width from (worldHeight / Screen.height * Screen.width) " + worldWidth);

		//The world width is 6 divided by the sprite (background image) widht of 4.8.
		//This gives the exact scale on the X that the image must be subjected to able
		//to fill the entire width of the Camera (world). In this case 6 / 4.8 = 1.25.
		//Tried in the editor manually and works perfectly.
		bgScale.x = worldWidth / width;
		//print("bgScale.x after calculation " + bgScale.x);

		transform.localScale = bgScale;
	}
}
