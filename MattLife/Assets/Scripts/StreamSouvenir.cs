using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamSouvenir : MonoBehaviour
{
	public RawImage videoProjector;
	public Image photoProjector;
	public VideoPlayer videoPlayer;
	public AudioSource audioSource;
	public Button closeButton;

	public void StartVideoStream(VideoClip videoClip)
	{
		videoPlayer.clip = videoClip;
		StartCoroutine(PlayVideo());
		GameMaster.Instance.state = "souvenirs";
	}

	public void StartPhotoStream(Sprite photoSouvenir)
	{
		photoProjector.sprite = photoSouvenir;
		photoProjector.preserveAspect = true;
		photoProjector.enabled = true;
		closeButton.gameObject.SetActive(true);
		GameMaster.Instance.state = "souvenirs";
		Time.timeScale = 0;
	}

	IEnumerator PlayVideo()
	{
		videoPlayer.Prepare();
		WaitForSeconds waitForSeconds = new WaitForSeconds(1);
		while (!videoPlayer.isPrepared)
		{
			yield return waitForSeconds;
			break;
		}

		videoProjector.texture = videoPlayer.texture;
		videoProjector.enabled = true;
		closeButton.gameObject.SetActive(true);
		videoPlayer.Play();
		audioSource.Play();
		videoPlayer.loopPointReached += EndReached;
		Time.timeScale = 0;
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		StopStream();
	}

	public void StopStream()
	{
		photoProjector.sprite = null;
		photoProjector.enabled = false;
		videoPlayer.clip = null;
		videoProjector.texture = null;
		videoProjector.enabled = false;
		closeButton.gameObject.SetActive(false);
		GameMaster.Instance.state = "game";
		Time.timeScale = 1;
	}
}
