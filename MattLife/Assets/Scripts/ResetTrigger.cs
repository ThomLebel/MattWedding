using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch (collision.tag)
		{
			case "Player":
				GameMaster.Instance.UpdateLife(-1);
				collision.transform.position = GameMaster.Instance.playerSpawnPosition;
				break;
			case "Enemy":
				collision.transform.GetComponent<EnemyScript>().DeleteMonster();
				break;
		}
	}
}
