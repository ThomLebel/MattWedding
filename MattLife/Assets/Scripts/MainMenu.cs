using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject player;
	public GameObject galerie;

	public State state;

	[SerializeField]
	private float timeToMouseFadeOut = 2f;
	private float timeBeforeMouseFadeOut;
	private bool mouseInvisible = false;

	private MenuPause menuPauseScript;

	private void Awake()
	{
		menuPauseScript = gameObject.GetComponent<MenuPause>();
	}

	// Start is called before the first frame update
	void Start()
    {
		state = State.menu;
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
			if (state == State.galerie)
			{
				CloseGalerie();
			}
			else if (state == State.stream)
			{
				gameObject.GetComponent<StreamSouvenir>().StopStream();
			}
		}
	}

	public void LaunchGame()
	{
		Debug.Log("Launch the game !");
	}

	public void DisplayGalerie()
	{
		galerie.SetActive(true);
		menuPauseScript.enabled = false;
		state = State.galerie;
	}

	public void CloseGalerie()
	{
		galerie.SetActive(false);
		menuPauseScript.enabled = true;
		state = State.menu;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public enum State
	{
		menu,
		galerie,
		stream
	}
}
