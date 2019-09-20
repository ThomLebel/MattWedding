using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SouvenirsHolder : MonoBehaviour
{
	public List<Souvenir> souvenirsList;

	//[SerializeField]
	//private int currentSouvenirIndex = 0;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		/*foreach (Souvenir souvenir in souvenirsList)
		{
			souvenir.revealed = false;
		}*/
	}

	// Start is called before the first frame update
	/*void Start()
    {
		GameObject[] souvenirsBlock = GameObject.FindGameObjectsWithTag("SouvenirBlock");
		List<Transform> souvenirsBlockTransform = new List<Transform>();
		foreach (GameObject souvenir in souvenirsBlock)
		{
			souvenirsBlockTransform.Add(souvenir.transform);
		}

		souvenirsBlockTransform.OrderBy(x => Vector3.Distance(x.transform.position, Vector3.zero)).ToList();

		foreach (Transform souvenirBlock in souvenirsBlockTransform)
		{
			Block blockScript = souvenirBlock.GetComponent<Block>();

			switch (blockScript.type)
			{
				case "photo":
					PhotoBlock photoBlockScript = souvenirBlock.GetComponent<PhotoBlock>();

					break;
				case "video":
					VideoBlock videoBlockScript = souvenirBlock.GetComponent<VideoBlock>();

					for (int i = 0; i<souvenirsList.Count; i++)
					{
						Souvenir souvenir = souvenirsList[i];
						if (souvenir.video != null)
						{
							videoBlockScript.souvenir = souvenir;
							return;
						}
					}
					break;
			}
		}

	}*/
}
