using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem _particle;
    private MemoryPool _memoryPool;

    public void Setup(MemoryPool pool)
    {
        _particle = GetComponent<ParticleSystem>();
        _memoryPool = pool;
    }
    void Update()
    {
        if (_particle.isPlaying == false)
        {
            _memoryPool.DeactivatePoolObject(gameObject);
        }
    }
}
