using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBlock : Block
{
	public Sprite photo;
	public StreamSouvenir photoPlayer;
	public GalerieScript galerie;
	[SerializeField]
	private bool photoDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
		
	}

	private void Update()
	{
		if (open && !photoDisplayed)
		{
			photoDisplayed = true;
			photoPlayer.StartPhotoStream(photo);
			galerie.RevealSouvenirs(id, photo);
		}
	}
}
