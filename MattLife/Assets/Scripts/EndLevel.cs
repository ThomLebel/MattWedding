using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
	public GameObject effect;
	public string nextScene;
	public float transitionTime = 1.5f;
	
	private Animator screenRevealAnimator;

	private void Start()
	{
		screenRevealAnimator = GameObject.FindGameObjectWithTag("ScreenReveal").GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Debug.Log("You reached the end of this level ! ");
			Instantiate(effect, collision.transform.position, Quaternion.identity);
			screenRevealAnimator.SetTrigger("End");
			AudioManager.instance.FadeToMusic(GameMaster.Instance.musicName, transitionTime);
			StartCoroutine("OnCompleteScreenReavealStartAnimation");
		}
	}

	IEnumerator OnCompleteScreenReavealStartAnimation()
	{
		yield return new WaitForSeconds(transitionTime + 0.2f);

		// TODO: Do something when animation did complete
		LoadNextLevel();
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(nextScene);
	}
}
