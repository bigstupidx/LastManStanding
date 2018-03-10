using UnityEngine;
using System.Collections;
using System;

public class FollowPlayerMob : Mob
{
    private float speed = 2f;

    private GameObject target;

    private Vector3 _direction;

    public AudioClip dieSound;

	private Vector2 movement;

	protected override Collider2D _Collider
	{ 
		get
		{
			return this.boxColliderCache2D;
		}
	}

	public void StartMob()
    {
		StartCoroutine(Spawn());
    }

	IEnumerator Spawn()
	{
		target = GameObject.FindGameObjectWithTag("Player");
        this.animatorCache.SetBool("die", false);

		_Collider.enabled = false;
		yield return new WaitForSeconds (.75f);
		_Collider.enabled = true;
		
        alive = true;
		_ativo = true;

	}

    void FixedUpdate()
    {
		if (target == null)
		{
            OnDeath();
            return;
		}

        if (alive)
        {
            if (this.spriteRendererCache.enabled == false) this.spriteRendererCache.enabled = true;
			_direction = target.transform.position - this.transformCache.position;
			this.rigidbodyCache2D.velocity = _direction.normalized * speed;
        }
    }
}
