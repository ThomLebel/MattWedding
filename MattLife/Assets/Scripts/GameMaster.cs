using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public int playerLife = 3;
	public int playerScore = 0;
	public int playerGold = 0;

	public GameObject menuPause;
	public GameObject galerie;
	public GameObject gameUI;
	public Text lifeText;
	public Text goldText;
	public Text scoreText;

	public string state = "";

	public Vector3 playerSpawnPosition = Vector3.zero;

	[SerializeField]
	private bool gamePaused = false;
	private GameObject player;

	static public GameMaster Instance;

    // Start is called before the first frame update
    void Start()
    {
		Instance = this;
		state = "game";
		player = GameObject.FindGameObjectWithTag("Player");
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

	public void UpdateLife(int value)
	{
		playerLife += value;
		if (playerLife <= 0)
		{
			playerLife = 0;
			Debug.Log("GameOver");
			GameOver();
		}
		lifeText.text = playerLife.ToString();

		if (value < 0)
		{
			player.transform.position = playerSpawnPosition;
		}
	}

	public void UpdateGold()
	{
		playerGold++;
		if (playerGold >= 100)
		{
			playerGold = 0;
			UpdateLife(1);
		}
		goldText.text = playerGold.ToString();
	}

	public void UpdateScore(int value)
	{
		playerScore += value;
		if (playerScore <= 0)
		{
			playerScore = 0;
		}
		scoreText.text = playerScore.ToString();
	}

	public void GameOver()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadSceneAsync(scene.name);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
 