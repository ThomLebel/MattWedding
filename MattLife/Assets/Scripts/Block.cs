using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	public int id = -1;

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
		if (id == -1)
		{
			throw new Exception("Il faut assigner un id à ce block");
		}
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
