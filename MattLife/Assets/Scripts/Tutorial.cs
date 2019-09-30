using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public GameObject tutorial;
	public float yOffset = 2f;

	private GameObject tutorialOnScreen;
	private RectTransform tutorialTransform;
	[SerializeField]
	private Canvas canvas;
	private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main;

		tutorialOnScreen = Instantiate(tutorial, transform.position, Quaternion.identity);
		tutorialOnScreen.transform.SetParent(canvas.transform);
		tutorialTransform = tutorialOnScreen.GetComponent<RectTransform>();

		tutorialTransform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}

		tutorialOnScreen.GetComponent<Animator>().SetTrigger("reveal");
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		
		tutorialOnScreen.GetComponent<Animator>().SetTrigger("hide");
	}
}
