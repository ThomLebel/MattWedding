﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBlock : Block
{
	public Souvenir[] souvenirs;
	//public Sprite photo;
	public StreamSouvenir photoPlayer;
	public GalerieScript galerie;
	[SerializeField]
	private bool blockRevealed = false;

	private void Update()
	{
		if (open && !blockRevealed)
		{
			Vector3 blockPosition = Camera.main.WorldToScreenPoint(transform.position);
			blockRevealed = true;
			photoPlayer.StartPhotoStream(souvenirs, blockPosition);
			foreach (Souvenir souvenir in souvenirs)
			{
				galerie.RevealSouvenirs(souvenir);
			}
		}
	}
}
