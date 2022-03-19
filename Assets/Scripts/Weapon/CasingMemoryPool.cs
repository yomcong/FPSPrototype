using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _casingPrefab;
    private MemoryPool _memoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool(_casingPrefab);
    }

    public void SpawnCasing(Vector3 position, Vector3 direction)
    {
        GameObject _object = _memoryPool.ActivatePoolObject();
        _object.transform.position = position;
        _object.transform.rotation = Random.rotation;
        _object.GetComponent<Casing>().SetUp(_memoryPool, direction);

    }
}