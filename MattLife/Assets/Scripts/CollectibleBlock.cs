using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBlock : MonoBehaviour
{
	public Animator coinAnimator;

	public int nbrCollectible = 1;
	[SerializeField]
	private int opend = 0;

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

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			BounceBlock();
		}
	}

	private void BounceBlock()
	{
		if (open)
		{
			return;
		}

		if (opend < nbrCollectible - 1)
		{
			animator.SetTrigger("BlockBounce");
		}
		else
		{
			open = true;
			animator.SetTrigger("BlockOpen");
		}

		coinAnimator.SetTrigger("CoinBounce");
		opend++;
		GameMaster.Instance.UpdateGold();
	}
}
