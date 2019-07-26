using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GalerieScript : MonoBehaviour
{
	public List<GameObject> souvenirsList;
	public Sprite revealedBackground;
	public StreamSouvenir streamSouvenir;

	public void RevealSouvenirs(int ID, Sprite sprite, VideoClip video = null)
	{
		//Change the souvenirs background
		souvenirsList[ID].GetComponent<Image>().sprite = revealedBackground;
		RectTransform souvenirTransform = souvenirsList[ID].GetComponent<RectTransform>();

		//Remove the unerevealed message from the button and add the correct stream
		souvenirsList[ID].GetComponent<Button>().onClick.RemoveAllListeners();
		if (video != null)
		{
			souvenirsList[ID].GetComponent<Button>().onClick.AddListener(() => streamSouvenir.StartVideoStream(video));
		}
		else
		{
			souvenirsList[ID].GetComponent<Button>().onClick.AddListener(() => streamSouvenir.StartPhotoStream(sprite));
		}

		GameObject souvenirs = new GameObject();
		Image souvenirsSprite = souvenirs.AddComponent<Image>();
		RectTransform rectTransform = souvenirs.GetComponent<RectTransform>();

		souvenirsSprite.sprite = sprite;
		souvenirsSprite.preserveAspect = true;
		rectTransform.SetParent(souvenirTransform);
		rectTransform.offsetMax = souvenirTransform.offsetMax;
		rectTransform.offsetMin = souvenirTransform.offsetMin;
		rectTransform.anchorMax = souvenirTransform.anchorMax;
		rectTransform.anchorMin = souvenirTransform.anchorMin;
		rectTransform.pivot = souvenirTransform.pivot;
		rectTransform.anchoredPosition = new Vector2(0f,0f);
	}

	public void DisplayUnrevealedMessage()
	{
		Debug.Log("You haven't revealed this souvenir yet");
	}
}
