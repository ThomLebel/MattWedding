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
	private bool streamLaunched = false;

	private void Update()
	{
		if (open && !streamLaunched)
		{
			streamLaunched = true;
			videoPlayer.StartVideoStream(videoClip);
			galerie.RevealSouvenirs(id, videoMiniature);
		}
	}
}
