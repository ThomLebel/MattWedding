using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBlock : Block
{
	public VideoClip videoClip;
	public StreamSouvenir videoPlayer;
	[SerializeField]
	private bool streamLaunched = false;

	private void Update()
	{
		if (open && !streamLaunched)
		{
			streamLaunched = true;
			videoPlayer.StartVideoStream(videoClip);
		}
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
	}
}
