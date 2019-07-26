using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject menuPause;
	public GameObject galerie;
	public string state = "";

	public Vector3 playerSpawnPosition = Vector3.zero;

	[SerializeField]
	private bool gamePaused = false;

	static public GameMaster Instance;

    // Start is called before the first frame update
    void Start()
    {
		Instance = this;
		state = "game";
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Cancel"))
		{
			if (state == "game")
			{
				if (!gamePaused)
				{
					PauseGame();
				}
				else
				{
					ResumeGame();
				}
			}
			else if (state == "souvenirs")
			{
				gameObject.GetComponent<StreamSouvenir>().StopStream();
			}
			else if (state == "galerie")
			{
				CloseGalerie();
			}
		}
    }

	public void DisplayGalerie()
	{
		galerie.SetActive(true);
		menuPause.SetActive(false);
		state = "galerie";
	}

	public void CloseGalerie()
	{
		galerie.SetActive(false);
		menuPause.SetActive(true);
		state = "game";
	}

	public void PauseGame()
	{
		gamePaused = true;
		menuPause.SetActive(true);
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		gamePaused = false;
		menuPause.SetActive(false);
		Time.timeScale = 1;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
 