using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField]
	private bool open = false;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player" && !open)
		{
			//Do stuff
			open = true;
			animator.SetTrigger("BlockOpen");
		}
	}
}
