using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour 
{
	
	void Start() 
	{
		
	}

	public void goBackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
