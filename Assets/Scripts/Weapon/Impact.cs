using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem particle;
    private MemoryPool memoryPool;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void SetUp(MemoryPool pool)
    {
        memoryPool = pool;
    }

    void Update()
    {
        if (particle.isPlaying == false)
        {
            memoryPool.DeactivatePoolObject(gameObject);
        }
    }
}
