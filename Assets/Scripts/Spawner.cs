using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public List<GameObject> mobPool;
    public List<GameObject> bouncyMobPool;
    public List<GameObject> trapPool;

    public List<GameObject> activeMobPool;
    public List<GameObject> activeBouncyMobPool;
    public List<GameObject> activeTrapPool;

    public GameObject mob;
    public GameObject bouncyMob;
    public GameObject trap;

	private float firstTrapSpawnTime = 12f;
    private float trapSpawnTime = 12.444f;
    private float mobSpawnTime = 2f;

    private const float bouncyMobSpawnStartTime = 40;
	private const float bouncyMobSpawnTime = 15;

    private int counter = 1;

    public float boundsX1 = 19f;
    public float boundsX2 = 39f;
    public float boundsY1 = 9f;
    public float boundsY2 = -7f;

    private bool _ativo = false;
    private bool oneTime = true;

    private GameObject player;

    private float time;

    private int trapCount = 0;

    private Vector2[] hexagonArray;

    void onSetAtivo(bool ativo)
    {
        _ativo = ativo;
    }

    void StartHexagon()
    {
        hexagonArray = new Vector2[6];
        hexagonArray[0] = (new Vector2(-1.0f, 2.2f));
        hexagonArray[1] = (new Vector2(1.0f, 2.2f));
        hexagonArray[2] = (new Vector2(2.6f, 0f));
        hexagonArray[3] = (new Vector2(1.0f, -2.2f));
        hexagonArray[4] = (new Vector2(-1.0f, -2.2f));
        hexagonArray[5] = (new Vector2(-2.6f, 0f));        
        /*
        hexagonArray[0] = (new Vector2(-1.6f, 2.8f));
        hexagonArray[1] = (new Vector2(1.6f, 2.8f));
        hexagonArray[2] = (new Vector2(3.2f, 0f));
        hexagonArray[3] = (new Vector2(1.6f, -2.8f));
        hexagonArray[4] = (new Vector2(-1.6f, -2.8f));
        hexagonArray[5] = (new Vector2(-3.2f, 0f));
        */
    }

    void Awake()
    {
        StartHexagon();
        player = GameObject.FindGameObjectWithTag("Player");

        if (mobPool == null) mobPool = new List<GameObject>();
        if (trapPool == null) trapPool = new List<GameObject>();
        if (bouncyMobPool == null) bouncyMobPool = new List<GameObject>();

        InitializeObjects();
    }

    void Start()
    {
        EventManager.onSetAtivo += onSetAtivo;
        EventManager.onMobDeath += onMobDeath;
        EventManager.onBouncyMobDeath += onBouncyMobDeath;
        EventManager.onTrapDeath += onTrapDestroy;
    }

    void Destroy()
    {
        EventManager.onSetAtivo -= onSetAtivo;
        EventManager.onMobDeath -= onMobDeath;
        EventManager.onBouncyMobDeath -= onBouncyMobDeath;
        EventManager.onTrapDeath -= onTrapDestroy;
    }

    void InitializeObjects()
    {
        GameObject obj;

        if(trapPool.Count < 1)
        { 
            for (int i = 0; i < 4; i++)
            {
                obj = Instantiate(trap, GetRandomPositionInsidePoly(hexagonArray), Quaternion.identity) as GameObject;
                obj.SetActive(false);
                trapPool.Add(obj);
            }
        }

        if (mobPool.Count < 1)
        {
            for (int i = 0; i < 30; i++)
            {
                obj = Instantiate(mob, GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f), Quaternion.identity) as GameObject;
                obj.SetActive(false);
                mobPool.Add(obj);
            }
        }

        if (bouncyMobPool.Count < 1)
        {
            for (int i = 0; i < 2; i++)
            {
                obj = Instantiate(bouncyMob, GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f), Quaternion.identity) as GameObject;
                obj.SetActive(false);
                bouncyMobPool.Add(obj);
            }
        }
    }

    void Update()
    {
        if (_ativo && oneTime)
        {
            StartCoroutine(SpawnMob(mobSpawnTime));
            StartCoroutine(SpawnBouncyMob(bouncyMobSpawnStartTime));
			StartCoroutine(SpawnTraps(firstTrapSpawnTime));
            oneTime = false;
            Debug.Log("One Time Only!");
        }

        time += Time.deltaTime;
    }

    void onMobDeath(GameObject deadMob)
    {
        if(_ativo)
        {
            deadMob.SetActive(false);
            mobPool.Add(deadMob);
            activeMobPool.Remove(deadMob);
        }/*
        else
        {
            Destroy(deadMob);
        }*/
    }

    void onBouncyMobDeath(GameObject deadMob)
    {
        if (_ativo)
        {
            deadMob.SetActive(false);
            activeBouncyMobPool.Remove(deadMob);
            bouncyMobPool.Add(deadMob);
        }/*
        else
        {
            Destroy(deadMob);
        }*/
    }

    void onTrapDestroy(GameObject gameObj)
    {
        if (_ativo)
        {
            gameObj.SetActive(false);
            activeTrapPool.Remove(gameObj);
            trapPool.Add(gameObj);
        }/*
        else
        {
            Destroy(gameObj);
        }*/
    }


    IEnumerator SpawnMob(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);

        if (_ativo)
        {
            //Instantiate(mob, GetRandomBoundsPosition(), Quaternion.identity);
            if(mobPool.Count > 0)
            {
                GameObject moby = mobPool[0];
                mobPool.Remove(moby);
                moby.transform.position = GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f);
                moby.SetActive(true);
                FollowPlayerMob fpm = moby.GetComponent<FollowPlayerMob>();
                fpm.StartMob();
                activeMobPool.Add(moby);
                //Instantiate(mob, GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f), Quaternion.identity);
            }/*
            else
            {
                GameObject moby = mobPool[0];
                mobPool.Remove(mob);
                moby.transform.position = GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f);
                moby.SetActive(true);
            }*/

            if (spawnTime > (mobSpawnTime / 4))
            {
                spawnTime -= .05f;
            }
            else
            {
                spawnTime = mobSpawnTime - .3f;
            }

            StartCoroutine(SpawnMob(spawnTime));
        }
    }

    IEnumerator SpawnBouncyMob(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);

        if (_ativo)
        {
            if (bouncyMobPool.Count > 0)
            {
                //Instantiate(bouncyMob, GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f), Quaternion.identity);
                GameObject moby = bouncyMobPool[0];
                bouncyMobPool.Remove(moby);
                moby.transform.position = GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f);
                moby.SetActive(true);
                BouncyMob fpm = moby.GetComponent<BouncyMob>();
                fpm.StartMob();
                activeBouncyMobPool.Add(moby);
            }/*
            else
            {
                GameObject moby = bouncyMobPool[0];
                bouncyMobPool.Remove(mob);
                moby.transform.position = GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f);
                moby.SetActive(true);
            }*/

			StartCoroutine(SpawnBouncyMob(bouncyMobSpawnTime));
        }
    }

    IEnumerator SpawnTraps(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);

        if (_ativo)
        {
            if (trapCount < 4)
            {
                trapCount += 1;
            }
            else
            {
                trapCount = 4;
            }

            for (int i = 0; i < trapCount; i++)
            {
                if (trapPool.Count > 0)
                {
                    //Instantiate(trap, GetRandomPositionInsidePoly(hexagonArray), Quaternion.identity);
                    GameObject trapy = trapPool[0];
                    trapPool.Remove(trapy);
                    trapy.transform.position = GetRandomPositionInsidePolyDistantFromPlayer(hexagonArray, 2f);
                    trapy.SetActive(true);
                    TrapTimer fpm = trapy.GetComponent<TrapTimer>();
                    fpm.StartTrap();
                    activeTrapPool.Add(trapy);
                }
                
                //Instantiate(trap, GetRandomPositionInsidePoly(hexagonArray), Quaternion.identity);

                //Vector3 randomPosition = GetRandomPosition();
                //Debug.Log(ContainsPoint(hexagonArray, new Vector2(randomPosition.x, randomPosition.y)));
                //Debug.Log(new Vector2(randomPosition.x, randomPosition.y));
                //Debug.Log(PointInPolygon(hexagonArray, new Vector2(randomPosition.x, randomPosition.y)));
                //Instantiate(trap, randomPosition, Quaternion.identity);
            }

            StartCoroutine(SpawnTraps(this.trapSpawnTime));
        }
    }

    int calcTimes()
    {
        int times = 1;

        if (time > 20) times = 2;
        if (time > 40) times = 3;
        if (time > 60) times = 4;

        return times;
    }
    Vector3 GetRandomPositionInsidePolyDistantFromPlayer(Vector2[] poly, float distance)
    {
        Vector3 position;

        do
        {
            position = GetRandomPositionInsidePoly(poly);
        } while (Vector3.Distance(position, player.transform.position) < distance);

        return position;
    }

    Vector3 GetRandomPositionInsidePoly(Vector2[] poly)
    {
        Vector3 position;

        do
        {
            position = GetRandomPosition();
        } while (!ContainsPoint(poly, new Vector2(position.x, position.y)));

        return position;
    }

    Vector3 GetRandomPosition()
    {
        Vector3 vector = new Vector3(0, 0, 0);

        vector.x = Random.Range(-4f, 4f);
        vector.y = Random.Range(-4f, 4f);
        //vector.x = Random.Range(boundsX2, boundsX1);
        //vector.y = Random.Range(boundsY2, boundsY1);

        return vector;
    }

    Vector3 GetRandomBoundsPosition()
    {
        float orientation = Random.Range(-1.0F, 1.0F);

        Vector3 vector = new Vector3(0, 0, 0);

        do
        {
            if (orientation < 0)  //Vertical - left, right
            {
                vector.y = Random.Range(boundsY2, boundsY1);

                orientation = Random.Range(-1.0F, 1.0F); // Check if it is X1 or X2 (leftOrRight)

                if (orientation < 0) // X1 left
                {
                    vector.x = boundsX1;
                }
                else // X2 right
                {
                    vector.x = boundsX2;
                }
            }
            else  // Horizontal - top, bot
            {
                vector.x = Random.Range(boundsX2, boundsX1);

                orientation = Random.Range(-1.0F, 1.0F); // Check if it is Y1 or Y2 (topOrBot)

                if (orientation < 0) // Y1 Top
                {
                    vector.y = boundsY1;
                }
                else // Y2 Bot
                {
                    vector.y = boundsY2;
                }
            }
        } while (Vector3.Distance(vector, player.transform.position) < 2f);
        return vector;
    }


    /// <summary>
    /// Checks if a point is inside a polygon
    /// Author: Eric Haines
    /// Source: http://wiki.unity3d.com/index.php?title=PolyContainsPoint
    /// Code was written by Eric in Javascript and I've translated to C#.
    /// </summary>
    /// <param name="polyPoints"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {
        int j = polyPoints.Length - 1;
        bool inside = false;
        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
               (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                inside = !inside;
        }
        return inside;
    }
}