using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public int playerLife = 32;
	public int playerScore = 0;
	public int playerGold = 0;

	public GameObject menuPause;
	public GameObject galerie;
	public GameObject gameUI;
	public Text lifeText;
	public Text goldText;
	public Text scoreText;

	//public string state = "";
	public States state;

	public Vector3 playerSpawnPosition = Vector3.zero;

	[SerializeField]
	private bool gamePaused = false;
	private GameObject player;
	[SerializeField]
	private float timeToMouseFadeOut = 2f;
	private float timeBeforeMouseFadeOut;
	private bool mouseInvisible = false;

	static public GameMaster Instance;

    // Start is called before the first frame update
    void Start()
    {
		Instance = this;
		state = States.game;
		player = GameObject.FindGameObjectWithTag("Player");
		lifeText.text = playerLife.ToString();

	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
		{
			timeBeforeMouseFadeOut = timeToMouseFadeOut;
			Cursor.visible = true;
			mouseInvisible = false;
		}
		else
		{
			timeBeforeMouseFadeOut -= Time.unscaledDeltaTime;
		}

		if (timeBeforeMouseFadeOut <= 0 && !mouseInvisible)
		{
			mouseInvisible = true;
			Cursor.visible = false;
		}

		if (Input.GetButtonDown("Cancel"))
		{
			if (state == States.game)
			{
				PauseGame();
			}
			else if (state == States.souvenirs)
			{
				gameObject.GetComponent<StreamSouvenir>().StopStream();
			}
			else if (state == States.galerie)
			{
				CloseGalerie();
			}else if (state == States.pause)
			{
				ResumeGame();
			}
		}
		if (Input.GetButtonDown("Jump") && state == States.souvenirs)
		{
			gameObject.GetComponent<StreamSouvenir>().StopStream();
		}
    }

	public void DisplayGalerie()
	{
		galerie.SetActive(true);
		menuPause.SetActive(false);
		state = States.galerie;
	}

	public void CloseGalerie()
	{
		galerie.SetActive(false);
		menuPause.SetActive(true);
		state = States.pause;
	}

	public void PauseGame()
	{
		gamePaused = true;
		menuPause.SetActive(true);
		Time.timeScale = 0;
		state = States.pause;
	}

	public void ResumeGame()
	{
		gamePaused = false;
		menuPause.SetActive(false);
		Time.timeScale = 1;
		state = States.game;
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
	}

	public void UpdateGold()
	{
		playerGold++;
		if (playerGold >= 100)
		{
			playerGold = 0;
			UpdateLife(1);
		}
		string zero = "";
		if (playerGold < 10)
		{
			zero = "0";
		}
		goldText.text = zero + playerGold.ToString();
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

	public enum States
	{
		menu,
		game,
		galerie,
		souvenirs,
		pause
	}
}
 