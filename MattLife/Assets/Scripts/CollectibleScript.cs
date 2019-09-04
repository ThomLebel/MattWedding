using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
	public int score;
	public GameObject effect;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Instantiate(effect, transform.position, Quaternion.identity);
			GameMaster.Instance.UpdateGold();
			GameMaster.Instance.UpdateScore(score);
			Destroy(gameObject);
		}
	}
}
