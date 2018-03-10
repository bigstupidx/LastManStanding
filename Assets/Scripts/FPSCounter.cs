using UnityEngine;
using System.Collections;

public class FPSCounter : BaseClass
{
    private double frameCount = 0;
    private double nextUpdate = 0.0;
    private double fps = 0.0;
    private double updateRate = 4.0;  // 4 updates per sec.

    void Start()
    {
        nextUpdate = Time.time;
    }

    void Update()
    {
        frameCount++;
        if (Time.time > nextUpdate)
        {
            nextUpdate += 1.0 / updateRate;
            fps = frameCount * updateRate;
            this.textMeshCache.text = fps.ToString();
            frameCount = 0;
        }
    }
}
