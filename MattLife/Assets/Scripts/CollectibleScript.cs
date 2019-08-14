using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
	public int score;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.tag);
		if (collision.tag == "Player")
		{
			Debug.Log("Hit a coin !");
			GameMaster.Instance.UpdateGold();
			GameMaster.Instance.UpdateScore(score);
			Destroy(gameObject);
		}
	}
}
