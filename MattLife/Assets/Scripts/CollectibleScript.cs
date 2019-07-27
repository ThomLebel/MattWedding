using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
	public int score;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			GameMaster.Instance.UpdateGold();
			GameMaster.Instance.UpdateScore(score);
			Destroy(gameObject);
		}
	}
}
