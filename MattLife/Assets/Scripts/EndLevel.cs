using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
	public GameObject effect;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Debug.Log("You reached the end of this level ! ");
			Instantiate(effect, collision.transform.position, Quaternion.identity);
		}
	}
}
