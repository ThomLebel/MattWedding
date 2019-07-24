using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField]
	protected bool open = false;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player" && !open)
		{
			//Do stuff
			open = true;
			animator.SetTrigger("BlockOpen");
		}
	}
}
