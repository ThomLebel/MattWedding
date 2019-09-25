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
		//foreach (Souvenir souvenir in souvenirsList)
		//{
		//	souvenir.revealed = false;
		//}
	}
}
