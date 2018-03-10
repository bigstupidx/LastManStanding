using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : BaseClass
{
    /// <summary>
    /// Velocidade do player
    /// </summary>
    public int speed = 2;

    /// <summary>
    /// Texto usado para mostrar o tempo final
    /// </summary>
    public GUIText endTimeText;

    /// <summary>
    /// Vetor que representa o movimento do player
    /// </summary>
    private Vector2 movement;

    /// <summary>
    /// Vetor de direçao do movimento do jogador no momento do pulo
    /// </summary>
    //    private Vector2 jumpMomentVector;

    /// <summary>
    /// Flag que indica se o player esta pulando
    /// </summary>
    private bool isJumping;

    /// <summary>
    /// Flag que indica se o player esta no dash
    /// </summary>
    private bool isDashing;

    /// <summary>
    /// Tempo total de jogo
    /// </summary>
    private float totalTime;

    private bool alive = true;
    private bool _ativo = false;

    //public Joystick joystick;
    /*
    //-----EVENT MANAGER-----
    public delegate void PlayerHandler();
    public static event PlayerHandler onPlayerDeath;
    //-----------------------
    */

    public AudioSource musicSrc;
    public AudioSource fxSrc;
    public GameObject pause;

    void Start()
    {
        EventManager.onSetAtivo += onSetAtivo;

        this.trailRendererCache.enabled = false;
    }

    void onSetAtivo(bool ativo)
    {
        _ativo = ativo;
    }
    /*
	float joyStickInput (Joystick joy)
	{
		Vector2 absJoyPos = new Vector2 (Mathf.Abs(joy.position.x),
		                         Mathf.Abs(joy.position.y));
		float xDirection = (joy.position.x > 0) ? 1 : -1;
		float yDirection = (joy.position.y > 0) ? 1 : -1;
		return ( ( absJoyPos.x > absJoyPos.y) ? absJoyPos.x * xDirection : absJoyPos.y * yDirection);
	}
    */

    float CheckJoystickAxis(float value)
    {
        float retorno = 0;

        if (value > 0)
        {
            retorno = 1;
        }
        else if (value < 0)
        {
            retorno = -1;
        }

        return retorno;
    }

    void Update()
    {
        if (alive && _ativo)
        {
            // Recupera o input do jogador
            float inputX = 0;
            float inputY = 0;

            if (Input.GetAxis("Horizontal") != 0)
            {
                inputX = Input.GetAxis("Horizontal");
            }
            else
            {
                //inputX = CheckJoystickAxis(joystick.position.x);
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                inputY = Input.GetAxis("Vertical");
            }
            else
            {
                //inputY = CheckJoystickAxis(joystick.position.y);
            }


            // float inputX = Input.GetAxis("Horizontal") ? Input.GetAxis ("Horizontal") : joyStickInput(moveJoystick);;
            //float inputY = Input.GetAxis("Vertical") ? Input.GetAxis ("Vertical") : joyStickInput(moveJoystick);

            // Seta o vetor de movimento de acordo com o input
            movement = (new Vector2(inputX, inputY)).normalized * speed;


            /*
            // FIXME Quando o player esta parado antes de pular, e possivel mudar de direçao
            // Se o player esta pulando, ele pode mover-se apenas na mesma direçao com que iniciou o pulo
            if (isJumping && jumpMomentVector.magnitude != 0) && !_IsInSameDirection(movement, jumpMomentVector))
            {
                movement = new Vector2(0, 0);
            }*/

            if (Input.GetKeyDown("joystick button 6") ||  Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.A))
            {
                PressedPauseButton();
            }

            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
            {
                //	            jumpMomentVector = new Vector2(inputX, inputY);
                PressedDashButton();
            }
        }
    }

    void PressedDashButton()
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    void PressedPauseButton()
    {
        if (Time.timeScale == 0)
        {

            StartCoroutine(ExitPause());
        }
        else
        {
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        musicSrc.Pause();
        fxSrc.Play();

        pause.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(1);
    }

    IEnumerator ExitPause()
    {

        StartCoroutine(Pause());
        pause.SetActive(false);

        musicSrc.Play();
        fxSrc.Play();

        pause.SetActive(false);

        Time.timeScale = 1;
        yield return new WaitForSeconds(1);
        /*
        fxSrc.Play();


        UILabel label = pause.transform.GetChild(0).GetComponent<UILabel>();
        label.text = 3f + "";
        label.text = 2f + "";
        yield return new WaitForSeconds(1);
        label.text = 1f + "";
        yield return new WaitForSeconds(1);
        label.text = "";

        pause.SetActive(false);

        Time.timeScale = 1;
        musicSrc.Play();*/
    }

    void FixedUpdate()
    {
        this.rigidbodyCache2D.velocity = movement;
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            StartCoroutine(OnPlayerDeath());
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Trap")
        {
            StartCoroutine(OnPlayerDeath());
        }
    }

    /// <summary>
    /// Metodo que realiza as atividades relacionadas a morte do player
    /// </summary>
    private IEnumerator OnPlayerDeath()
    {
        if (alive)
        {
            //totalTime = TimerScript.Instance.GetTotalTime();
            //endTimeText.text = string.Format("{0:f2}", totalTime);
            alive = false;

            movement = new Vector2(0, 0);
            this.circleColliderCache2D.enabled = false;

            //GetComponent<Animator>().SetTrigger("die");
            this.spriteRendererCache.enabled = false;
            this.particleSystemCache.Play();

            Component halo = GetComponent("Halo");
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);

            EventManager.Instance.onPlayerDeathEvent();

            yield return new WaitForSeconds(1f);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Verifica se dois vetores estao na mesma direçao
    /// </summary>
    private bool _IsInSameDirection(Vector2 vector1, Vector2 vector2)
    {
        Vector2 vector1Normalized = vector1.normalized;
        Vector2 vector2Normalized = vector2.normalized;

        // Os vetores estao no mesmo sentido ou em sentidos opostos
        return (vector1Normalized.x == vector2Normalized.x && vector1Normalized.y == vector2Normalized.y) ||
            (vector1Normalized.x == -vector2Normalized.x && vector1Normalized.y == -vector2Normalized.y);
    }

    /// <summary>
    /// Corotina de pulo
    /// </summary>
    private IEnumerator JumpCoroutine()
    {
        isJumping = true;

        renderer.material.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);

        isJumping = false;
        renderer.material.color = new Color(255, 255, 255);
    }


    /// <summary>
    /// Corotina de dash
    /// </summary>
    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        this.speed += 2;
        //renderer.material.color = Color.blue;//new Color(255, 0, 0);
        this.trailRendererCache.enabled = true;
        transform.GetChild(0).gameObject.GetComponent<Light>().color = Color.blue;

        yield return new WaitForSeconds(.5f);

        this.trailRendererCache.enabled = false;

        this.transformCache.GetChild(0).gameObject.GetComponent<Light>().color = Color.red;
        //transform.GetChild(0).gameObject.GetComponent<Light>().intensity /= 2;

        this.speed -= 2;

        yield return new WaitForSeconds(5f);

        this.transformCache.GetChild(0).gameObject.GetComponent<Light>().color = Color.white;
        //transform.GetChild(0).gameObject.GetComponent<Light>().intensity *= 2;
        isDashing = false;
        //renderer.material.color = new Color(255, 255, 255);
    }
}