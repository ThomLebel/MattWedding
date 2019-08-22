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
				collision.transform.GetComponent <Player>().Hit();
				collision.transform.GetComponent <Player>().Fall();
				//GameMaster.Instance.UpdateLife(-1);
				//collision.transform.position = GameMaster.Instance.playerSpawnPosition;
				//collision.transform.position = new Vector3(collision.transform.position.x, 10f, collision.transform.position.z);
				break;
			case "Enemy":
				collision.transform.GetComponent<EnemyScript>().DeleteMonster();
				break;
		}
	}
}
