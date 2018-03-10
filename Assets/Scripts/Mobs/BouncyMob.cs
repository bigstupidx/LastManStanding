using UnityEngine;
using System.Collections;
using System;

public class BouncyMob : Mob
{
    public float speed = 5f;

    private GameObject target;

    private Vector3 _direction;

    public AudioClip dieSound;

    private Vector2 movement;

	protected override Collider2D _Collider
	{ 
		get
		{
			return this.circleColliderCache2D;
		}
	}


	public void StartMob ()
    {
        StartCoroutine(Spawn());
	}

    bool once = true;

    void Move()
    {
        _direction = target.transform.position - this.transformCache.position;
    }

    void FixedUpdate()
    {
		// Se o player morreu, destroi o objeto
		if(target == null)
		{
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                StartCoroutine(Spawn());
            }
            else
            {
                OnDeath();
            }
            return;
		}

        if (alive && once)
        {
            once = false;
            Move();
        }

        if (alive)
        {
            this.rigidbodyCache2D.AddForce(_direction.normalized * 200);
        }
    }

    IEnumerator Spawn()
    {
		target = GameObject.FindGameObjectWithTag("Player");

        _Collider.enabled = false;
        yield return new WaitForSeconds(.75f);
		_Collider.enabled = true;

        alive = true;
		_ativo = true;
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Arena")
            Move();
    }
}
