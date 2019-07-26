using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBlock : Block
{
	public VideoClip videoClip;
	public Sprite videoMiniature;
	public StreamSouvenir videoPlayer;
	public GalerieScript galerie;
	[SerializeField]
	private bool blockRevealed = false;

	private void Update()
	{
		if (open && !blockRevealed)
		{
			blockRevealed = true;
			videoPlayer.StartVideoStream(videoClip);
			galerie.RevealSouvenirs(id, videoMiniature, videoClip);
		}
	}
}
