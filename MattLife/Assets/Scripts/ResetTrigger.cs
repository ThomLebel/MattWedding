using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
	public AudioClip inSound;
	public AudioClip outSound;

	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch (collision.transform.tag)
		{
			case "Player":
				collision.transform.GetComponent<Player>().Hit();
				collision.transform.GetComponent<Player>().Fall();
				break;
			case "Enemy":
				collision.transform.GetComponent<Enemy>().DeleteMonster();
				break;
		}

		source.clip = inSound;
		source.Play();
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		source.clip = outSound;
		source.Play();
	}
}
