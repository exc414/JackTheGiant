using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour 
{
	void Start() 
	{
		
	}

	public void startGame()
	{
		SceneManager.LoadScene("Gameplay");
	}

	public void highScoreMenu()
	{
		SceneManager.LoadScene("Highscore");
	}

	public void optionsMenu()
	{
		SceneManager.LoadScene("OptionsMenu");
	}

	public void quitGame()
	{
		Application.Quit();
	}

	public void musicButton()
	{

	}

}
