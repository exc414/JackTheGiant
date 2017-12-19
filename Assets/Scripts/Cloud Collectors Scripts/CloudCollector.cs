using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCollector : MonoBehaviour 
{
	//Once the cloud collector touches a cloud this function will get triggered.
	//The cloud that gets touched will be the target parameter.
	void OnTriggerEnter2D(Collider2D target)
	{
		//Sets the game object to not active in the scene. This means the game object
		//will not be visible. Think android, this is a version of GONE instead of invisible.
		if(target.tag == "Cloud" || target.tag == "Deadly")
		{
			target.gameObject.SetActive(false);
		}
	}
}
