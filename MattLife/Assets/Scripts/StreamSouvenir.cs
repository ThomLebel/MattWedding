using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamSouvenir : MonoBehaviour
{
	public RawImage videoProjector;
	public Image photoProjector;
	public VideoPlayer videoPlayer;
	public Image videoBackup;
	public AudioSource audioSource;
	public GameObject souvenirProjector;
	public Button leftButton;
	public Button rightButton;

	private Souvenir[] slider;
	private int currentSlide = 0;
	private float timeToNextSlide;
	[SerializeField]
	private float timeBetweenSlide = 0.5f;

	private void Update()
	{
		if (GameMaster.Instance.state != "souvenirs" || slider == null)
		{
			return;
		}

		float input = Input.GetAxisRaw("Horizontal");
		int dir = 0;
		if (input > 0)
		{
			dir = 1;
		}else if (input < 0)
		{
			dir = -1;
		}

		if (timeToNextSlide <= 0)
		{
			if (dir != 0)
			{
				Slide(dir);
				timeToNextSlide = timeBetweenSlide;
			}
		}
		else
		{
			timeToNextSlide = timeToNextSlide - Time.unscaledDeltaTime;
		}
	}

	public void AnimateSouvenirStream(Vector3 blockPosition)
	{
		iTween.ScaleFrom(souvenirProjector, iTween.Hash(
			"scale", Vector3.zero,
			"time", 1.5f,
			"ignoretimescale", true
		));
		iTween.FadeFrom(souvenirProjector, iTween.Hash(
			"alpha", 0f,
			"time", 1.5f,
			"ignoretimescale", true
		));
		iTween.MoveFrom(souvenirProjector, iTween.Hash(
			"position", blockPosition,
			"time", 1.5f,
			"oncomplete", "RevealAnimationComplete",
			"ignoretimescale", true
		));
	}

	private void RevealAnimationComplete()
	{
		Time.timeScale = 0;
	}

	public void StartVideoStream(Souvenir souvenir, Vector3 blockPosition)
	{
		videoPlayer.clip = souvenir.video;
		videoBackup.sprite = souvenir.photo;

		souvenirProjector.gameObject.SetActive(true);
		videoBackup.gameObject.SetActive(true);
		AnimateSouvenirStream(blockPosition);

		StartCoroutine(PlayVideo());
		GameMaster.Instance.state = "souvenirs";
	}

	public void StartPhotoStream(Souvenir[] souvenirs, Vector3 blockPosition)
	{
		slider = souvenirs;
		photoProjector.sprite = slider[currentSlide].photo;
		photoProjector.preserveAspect = true;
		photoProjector.enabled = true;
		souvenirProjector.gameObject.SetActive(true);
		leftButton.gameObject.SetActive(true);
		rightButton.gameObject.SetActive(true);

		AnimateSouvenirStream(blockPosition);

		GameMaster.Instance.state = "souvenirs";
	}

	public void StartPhotoStream(Souvenir souvenir, Vector3 blockPosition)
	{
		photoProjector.sprite = souvenir.photo;
		photoProjector.preserveAspect = true;
		photoProjector.enabled = true;
		souvenirProjector.gameObject.SetActive(true);

		AnimateSouvenirStream(blockPosition);

		GameMaster.Instance.state = "souvenirs";
	}

	public void Slide(int dir)
	{
		if (dir == 0)
		{
			return;
		}
		currentSlide += dir;
		if (currentSlide < 0)
		{
			currentSlide = slider.Length - 1;
		}else if (currentSlide >= slider.Length)
		{
			currentSlide = 0;
		}
		photoProjector.sprite = slider[currentSlide].photo;
		photoProjector.preserveAspect = true;
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
		videoPlayer.Play();
		audioSource.Play();
		videoPlayer.loopPointReached += EndReached;
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		StopStream();
	}

	public void StopStream()
	{
		slider = null;
		photoProjector.sprite = null;
		photoProjector.enabled = false;
		videoPlayer.clip = null;
		videoProjector.texture = null;
		videoProjector.enabled = false;
		videoBackup.sprite = null;
		souvenirProjector.gameObject.SetActive(false);
		videoBackup.gameObject.SetActive(false);
		leftButton.gameObject.SetActive(false);
		rightButton.gameObject.SetActive(false);
		if (GameMaster.Instance.galerie.activeSelf)
		{
			GameMaster.Instance.state = "galerie";
		}
		else
		{
			GameMaster.Instance.state = "game";
		}
		Time.timeScale = 1;
	}
}
