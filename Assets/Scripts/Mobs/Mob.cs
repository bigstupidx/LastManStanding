using UnityEngine;
using System.Collections;
using System;

public abstract class Mob : BaseClass
{
	public static event EventHandler<MobDeathEventArgs> onMobDie;
    //public static event EventHandler onMobDeath;
	protected bool alive;
	protected bool _ativo;

	protected abstract Collider2D _Collider { get; }

	void Start()
	{
		EventManager.onSetAtivo += onSetAtivo;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Trap")
		{
			StartCoroutine(Die(collider.gameObject));
		}
	}
	
	IEnumerator Die(GameObject trapGameObject)
	{	
		//onMobDie(this, new MobDeathEventArgs(trapGameObject));
		this.audioSourceCache.Play();
		alive = false;
		
		_Collider.enabled = false;
		this.animatorCache.SetBool("die", true);
		this.particleSystemCache.Play();
		yield return new WaitForSeconds(1f);
		
		EventManager.onSetAtivo -= onSetAtivo;
        OnDeath();
	}
	
	void onSetAtivo(bool ativo)
	{
		_ativo = ativo;
		if (!ativo)
		{
			alive = false;
			EventManager.onSetAtivo -= onSetAtivo;
            OnDeath();
		}
	}

    public void OnDeath()
    {
        //Destroy(gameObject);
		this.animatorCache.SetBool("die", true);
        if(GetComponent<FollowPlayerMob>() != null)
        {
            EventManager.Instance.onMobDeathEvent(this.gameObjectCache);
        }
        else
        {
            EventManager.Instance.onBouncyMobDeathEvent(this.gameObjectCache);
        }
    }
}

public class MobDeathEventArgs : EventArgs
{
	/// <summary>
	/// Objeto com que o mob colidiu
	/// </summary>
	public GameObject GameObject { get; private set; }
	
	public MobDeathEventArgs(GameObject gameObject)
	{
		this.GameObject = gameObject;
	}
}
