using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject menuPause;
	public string state = "";
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
		}
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
 