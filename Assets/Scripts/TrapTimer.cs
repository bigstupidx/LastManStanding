using UnityEngine;
using System.Collections;

public class TrapTimer : BaseClass
{
    public float TrapLifeTime;

    private float timer = 7f;

	private Animator animator;
	private Collider2D collider;

    private bool alive;

	public void StartTrap ()
    {
        EventManager.onSetAtivo += onSetAtivo;

        animator = this.animatorCache;
		collider = this.polygonColliderCache2D;
		collider.enabled = false;

        alive = true;

		StartCoroutine(OnTrapAnimationEnterEnded());

        StartCoroutine(Timer());
	}

    void onSetAtivo(bool ativo)
    {
        if (!ativo)
        {
            alive = ativo;
            EventManager.onSetAtivo -= onSetAtivo;
            EventManager.Instance.onTrapDeathEvent(this.gameObjectCache);
            //Destroy(gameObject);
        }
    }

	IEnumerator OnTrapAnimationEnterEnded()
	{
		yield return new WaitForSeconds(.75f);
		collider.enabled = true;
	}
	
	IEnumerator Timer()
	{
		yield return new WaitForSeconds(timer);
        
        if(alive)
        {
            StartCoroutine(DestroyTrap());
        }
    }

    IEnumerator DestroyTrap()
    {
        EventManager.onSetAtivo -= onSetAtivo;

		animator.SetTrigger("Destroi");
		collider.enabled = false;

		yield return new WaitForSeconds(.75f);

		//Destroy(gameObject);
        EventManager.Instance.onTrapDeathEvent(this.gameObjectCache);
	}

    void FixedUpdate()
    {
		// Se o player morreu, destroi o objeto
		if(GameObject.FindGameObjectWithTag("Player") == null)
		{
            alive = false;
            StartCoroutine(DestroyTrap());
            return;
		}
    }
}
