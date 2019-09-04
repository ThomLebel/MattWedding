﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBlock : Block
{
	public Souvenir souvenir;
	//public VideoClip videoClip;
	//public Sprite videoMiniature;
	public StreamSouvenir videoPlayer;
	public GalerieScript galerie;
	[SerializeField]
	private bool blockRevealed = false;

	private void Update()
	{
		if (open && !blockRevealed)
		{
			Vector3 blockPosition = Camera.main.WorldToScreenPoint(transform.position);
			blockRevealed = true;
			videoPlayer.StartVideoStream(souvenir, blockPosition);
			galerie.RevealSouvenirs(souvenir);
		}
	}
}
