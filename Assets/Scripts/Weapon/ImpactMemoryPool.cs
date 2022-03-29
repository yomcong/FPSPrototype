using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType { Normal = 0, Obstacle, Enemy, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] imapctPrefab;
    private MemoryPool[] memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool[imapctPrefab.Length];
        for (int i = 0; i < imapctPrefab.Length; ++i)
        {
            memoryPool[i] = new MemoryPool(imapctPrefab[i]);
        }
    }
    public void SpawnImpact(RaycastHit hit)
    {
        //if (hit.transform.CompareTag("ObstacleObject"))
        //{
        //    OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        //}
        /*else */if (hit.transform.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else
        {
            Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
            if( color == null)
            {
                color = Color.black;
            }
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal), color);
        }
    }

    public void SpawnImpact(Collider other, Transform knifeTransform)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else
        {
            Color color = other.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.Normal, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation), color);
        }
    }


    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject _object = memoryPool[(int)type].ActivatePoolObject();
        _object.transform.position = position;
        _object.transform.rotation = rotation;
        _object.GetComponent<Impact>().SetUp(memoryPool[(int)type]);

        if (type == ImpactType.Normal)
        {
            ParticleSystem.MainModule main = _object.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }
}
