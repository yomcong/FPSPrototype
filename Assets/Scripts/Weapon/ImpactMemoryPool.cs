using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType { Normal = 0, Obstacle, Enemy, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _imapctPrefab;
    private MemoryPool[] _memoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool[_imapctPrefab.Length];
        for (int i = 0; i < _imapctPrefab.Length; ++i)
        {
            _memoryPool[i] = _imapctPrefab[i].GetComponent<MemoryPool>();
            _memoryPool[i].setup(_imapctPrefab[i]);

        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        if (hit.transform.CompareTag("ObstacleObject"))
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("TriggerObject"))
        {
            return;
        }
        else
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal), Color.gray);
        }
    }

    public void SpawnImpact(Collider other, Transform knifeTransform)
    {
        if (other.CompareTag("ObstacleObject"))
        {
            OnSpawnImpact(ImpactType.Obstacle, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.transform.CompareTag("TriggerObject"))
        {
            return;
        }
        else
        {
            OnSpawnImpact(ImpactType.Enemy, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
    }


    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject _object = _memoryPool[(int)type].ActivatePoolObject();
        _object.transform.position = position;
        _object.transform.rotation = rotation;
        _object.GetComponent<Impact>().Setup(_memoryPool[(int)type]);
        if (type == ImpactType.Normal)
        {
            ParticleSystem.MainModule main = _object.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }
}
