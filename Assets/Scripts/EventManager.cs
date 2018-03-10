using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    //-----EVENTS-----------------------------------------
    public delegate void GameHandler(bool ativo);
    public static event GameHandler onSetAtivo;

    public delegate void LoadSceneHandler();
    public static event LoadSceneHandler onLoadNewScene;

    public delegate void PointsHandler(int points);
    public static event PointsHandler onPointsChange;

    public delegate void MobHandler();
    public static event MobHandler onMobDie;

    public delegate void MobDeathHandler(GameObject obj);
    public static event MobDeathHandler onMobDeath;

    public delegate void DeathHandler(GameObject obj);
    public static event DeathHandler onBouncyMobDeath;


    public delegate void TrapDeathHandler(GameObject obj);
    public static event TrapDeathHandler onTrapDeath;

    public delegate void PlayerHandler();
    public static event PlayerHandler onPlayerDeath;
    //----------------------------------------------------

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        Application.targetFrameRate = 120;
    }

    public void onSetAtivoEvent(bool ativo)
    {
        onSetAtivo(ativo);
    }

    public void onLoadNewSceneEvent()
    {
        onLoadNewScene();
    }

    public void onPointsChangeEvent(int points)
    {
        onPointsChange(points);
    }

    public void onMobDieEvent()
    {
        onMobDie();
    }

    public void onMobDeathEvent(GameObject obj)
    {
        onMobDeath(obj);
    }

    public void onBouncyMobDeathEvent(GameObject obj)
    {
        onBouncyMobDeath(obj);
    }   

    public void onTrapDeathEvent(GameObject obj)
    {
        onTrapDeath(obj);
    }

    public void onPlayerDeathEvent()
    {
        onPlayerDeath();
    }
}
