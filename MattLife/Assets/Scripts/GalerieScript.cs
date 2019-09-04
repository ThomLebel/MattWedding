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

	private void Start()
	{
		foreach (GameObject button in souvenirsList)
		{
			button.GetComponent<Button>().onClick.AddListener(() => DisplayUnrevealedMessage());
		}
	}

	public void RevealSouvenirs(Souvenir souvenir)
	{
		//Change the souvenirs background
		souvenirsList[souvenir.id].GetComponent<Image>().sprite = revealedBackground;
		RectTransform souvenirTransform = souvenirsList[souvenir.id].GetComponent<RectTransform>();

		//Remove the unerevealed message from the button and add the correct stream
		souvenirsList[souvenir.id].GetComponent<Button>().onClick.RemoveAllListeners();
		if (souvenir.video != null)
		{
			souvenirsList[souvenir.id].GetComponent<Button>().onClick.AddListener(() => streamSouvenir.StartVideoStream(souvenir, souvenirsList[souvenir.id].transform.position));
		}
		else
		{
			souvenirsList[souvenir.id].GetComponent<Button>().onClick.AddListener(() => streamSouvenir.StartPhotoStream(souvenir, souvenirsList[souvenir.id].transform.position));
		}

		GameObject souvenirs = new GameObject();
		Image souvenirsSprite = souvenirs.AddComponent<Image>();
		RectTransform rectTransform = souvenirs.GetComponent<RectTransform>();

		souvenirsSprite.sprite = souvenir.photo;
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
