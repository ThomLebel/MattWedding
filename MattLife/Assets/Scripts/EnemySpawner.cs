using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Transform spawn;
	public GameObject monsterToSpawn;
	public int nbrToSpawn;
	public float delayBetweenSpawn;
	public List<GameObject> monsterList;

	private IEnumerator spawnCoroutine;

	// Start is called before the first frame update
	void Start()
    {
		monsterList = new List<GameObject>();
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag != "Player")
		{
			return;
		}

		if (collision.transform.position.x > transform.position.x)
		{
			SpawnMonster();
		}
		/*else
		{
			DeleteMonster();
		}*/
	}

	private void SpawnMonster()
	{
		if (monsterList.Count >= nbrToSpawn)
		{
			StopCoroutine(spawnCoroutine);
			return;
		}

		GameObject monster = Instantiate(monsterToSpawn, spawn.position, Quaternion.identity);
		monster.GetComponent<EnemyScript>().spawner = this;
		monsterList.Add(monster);
		GameMaster.Instance.monsterList.Add(monster);

		spawnCoroutine = SpawnDelay();
		StartCoroutine(spawnCoroutine);
	}

	private IEnumerator SpawnDelay()
	{
		yield return new WaitForSeconds(delayBetweenSpawn);
		SpawnMonster();
	}

	private void DeleteMonster()
	{
		if (monsterList.Count <= 0)
		{
			return;
		}

		for (int i=monsterList.Count-1; i>=0; i--)
		{
			monsterList[i].GetComponent<EnemyScript>().DeleteMonster();
		}

		monsterList = new List<GameObject>();
	}
}
