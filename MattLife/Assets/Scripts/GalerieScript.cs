using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalerieScript : MonoBehaviour
{
	public List<GameObject> souvenirsList;
	public Sprite revealedBackground;

    public void RevealSouvenirs(int ID, Sprite sprite)
	{
		//Change the souvenirs background
		souvenirsList[ID].GetComponent<Image>().sprite = revealedBackground;

		GameObject souvenirs = new GameObject();
		Image souvenirsSprite = souvenirs.AddComponent<Image>();
		souvenirsSprite.sprite = sprite;
		souvenirsSprite.preserveAspect = true;
		souvenirs.GetComponent<RectTransform>().SetParent(souvenirsList[ID].GetComponent<RectTransform>());
		souvenirs.GetComponent<RectTransform>().offsetMax = souvenirsList[ID].GetComponent<RectTransform>().offsetMax;
		souvenirs.GetComponent<RectTransform>().offsetMin = souvenirsList[ID].GetComponent<RectTransform>().offsetMin;
		souvenirs.GetComponent<RectTransform>().anchorMax = souvenirsList[ID].GetComponent<RectTransform>().anchorMax;
		souvenirs.GetComponent<RectTransform>().anchorMin = souvenirsList[ID].GetComponent<RectTransform>().anchorMin;
		souvenirs.GetComponent<RectTransform>().pivot = souvenirsList[ID].GetComponent<RectTransform>().pivot;
		souvenirs.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f,0f);
	}
}
