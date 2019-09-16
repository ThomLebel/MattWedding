using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public int moneyLimit = 30;

	public GameObject menuPause;
	public GameObject galerie;
	public GameObject gameUI;
	public Text lifeText;
	public Text goldText;
	public Text scoreText;

	//public string state = "";
	public States state;
	public string musicName;
	public Vector3 playerSpawnPosition = Vector3.zero;

	//[SerializeField]
	//private bool gamePaused = false;
	private GameObject player;
	private Player playerScript;
	[SerializeField]
	private float timeToMouseFadeOut = 2f;
	private float timeBeforeMouseFadeOut;
	private bool mouseInvisible = false;

	private Animator screenRevealAnimator;

	static public GameMaster Instance;

    // Start is called before the first frame update
    void Start()
    {
		Instance = this;

		screenRevealAnimator = GameObject.FindGameObjectWithTag("ScreenReveal").GetComponent<Animator>();
		//screenRevealAnimator.SetTrigger("End");
		StartCoroutine("OnCompleteScreenReavealEndAnimation");

		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();

		player.transform.position = playerSpawnPosition;

		lifeText.text = playerScript.playerLife.ToString();
		
		AudioManager.instance.PlayMusic(musicName);
		AudioManager.instance.FadeFromMusic(musicName, 1.5f);
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
		Sound m = Array.Find(AudioManager.instance.musics, music => music.name == GameMaster.Instance.musicName);
		AudioManager.instance.FadeToMusic(m.name, 1f, m.gamePausedVolume);
		menuPause.SetActive(true);
		Time.timeScale = 0;
		state = States.pause;
	}

	public void ResumeGame()
	{
		Sound m = Array.Find(AudioManager.instance.musics, music => music.name == GameMaster.Instance.musicName);
		AudioManager.instance.FadeToMusic(m.name, 1f, m.volume);
		menuPause.SetActive(false);
		Time.timeScale = 1;
		state = States.game;
	}

	public void UpdateLife(int value)
	{
		playerScript.playerLife += value;
		if (playerScript.playerLife <= 0)
		{
			playerScript.playerLife = 0;
			state = States.gameOver;
			GameOver();
		}
		lifeText.text = playerScript.playerLife.ToString();
	}

	public void UpdateGold()
	{
		playerScript.playerGold++;
		if (playerScript.playerGold >= moneyLimit)
		{
			playerScript.playerGold = 0;
			UpdateLife(1);
		}
		string zero = "";
		if (playerScript.playerGold < 10)
		{
			zero = "0";
		}
		goldText.text = zero + playerScript.playerGold.ToString();
	}

	public void UpdateScore(int value)
	{
		playerScript.playerScore += value;
		if (playerScript.playerScore <= 0)
		{
			playerScript.playerScore = 0;
		}
		scoreText.text = playerScript. playerScore.ToString();
	}


	IEnumerator OnCompleteScreenReavealEndAnimation()
	{
		while (screenRevealAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
			yield return null;

		// TODO: Do something when animation did complete
		state = States.game;
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
		pause,
		gameOver
	}
}
 