using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour
{
    public AudioClip buttonSound;

    private AudioSource audioSource;

    private bool playOnce = true;

    void Start()
    {
		audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (Input.anyKey && playOnce)
        {
            playOnce = false;
			audioSource.PlayOneShot(buttonSound, 1.2f);
            StartCoroutine(LoadMenu());
        }
	}

    IEnumerator LoadMenu()
    {
        GameObject.FindGameObjectWithTag("StaticManager").SendMessage("ChangeScene", "Menu", SendMessageOptions.DontRequireReceiver);

        gameObject.GetComponent<Animator>().SetTrigger("CreditsExit");

        yield return new WaitForSeconds(1);

        Application.LoadLevel("Menu");
    }
}
