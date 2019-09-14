using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

	public string type;
	[SerializeField]
	protected bool open = false;

	private Animator animator;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && !open)
		{
			open = true;
			animator.SetTrigger("BlockOpen");
		}
	}
}
