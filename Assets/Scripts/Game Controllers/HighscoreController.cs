using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreController : MonoBehaviour 
{

	void Awake() { }
	
	public void goBackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
