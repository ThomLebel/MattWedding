﻿using System.Collections;
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
		if (collision.tag == "Player")
		{
			Debug.Log("You reached the end of this level ! ");
			Instantiate(effect, collision.transform.position, Quaternion.identity);
			screenRevealAnimator.SetTrigger("End");
			StartCoroutine("OnCompleteScreenReavealStartAnimation");
		}
	}

	IEnumerator OnCompleteScreenReavealStartAnimation()
	{
		yield return new WaitForSeconds(transitionTime);

		// TODO: Do something when animation did complete
		LoadNextLevel();
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(nextScene);
	}
}
