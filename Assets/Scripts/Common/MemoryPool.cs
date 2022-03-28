using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    private class PoolObject
    {
        public bool isActive;           
        public GameObject gameObject;   //보여지는 실제 오브젝트
    }

    private int _increaseCount = 5;  // 오브젝트가 부족할 때 추가 생성되는 오브젝트풀 개수
    private int _maxCount;          
    private int _activeCount;       

    private GameObject _poolObjectPrefab; 
    private List<PoolObject> _poolObjectList;
    //private Queue<PoolObject> _poolObjectList;
    public int MaxCount => _maxCount;    
    public int ActiveCount => _activeCount; 

    private Vector3 tempPosition = new Vector3(48, 1, 48);  //임의에 포지션에 위치

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        _poolObjectPrefab = poolObject;

        _poolObjectList = new List<PoolObject>();

        //_poolObjectList = new Queue<PoolObject>();

        InstantiateObject();
    }

    public void InstantiateObject()
    {
        _maxCount += _increaseCount;

        for (int i = 0; i < _increaseCount; ++i)
        {
            PoolObject poolObject = new PoolObject();

            poolObject.isActive = false;
            poolObject.gameObject = GameObject.Instantiate(_poolObjectPrefab);
            poolObject.gameObject.transform.position = tempPosition;
            poolObject.gameObject.SetActive(false);

            _poolObjectList.Add(poolObject);

            //_poolObjectList.Enqueue(poolObject);

        }
    }

    public void DestroyObjects()
    {
        if (_poolObjectList == null)
        {
            return;
        }

        for (int i = 0; i < _poolObjectList.Count; ++i)
        {
            GameObject.Destroy(_poolObjectList[i].gameObject);
        }

        _poolObjectList.Clear();
    }

    public GameObject ActivatePoolObject()
    {
        if (_poolObjectList == null)
        {
            return null;
        }

        if (_maxCount == _activeCount)
        {
            InstantiateObject();
        }

        for (int i = 0; i < _poolObjectList.Count; ++i)
        {
            PoolObject poolObject = _poolObjectList[i];

            if (poolObject.isActive == false)
            {
                _activeCount++;

                poolObject.isActive = true;
                poolObject.gameObject.SetActive(true);

                return poolObject.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolObject(GameObject removeObject)
    {
        if (_poolObjectList == null || removeObject == null)
        {
            return;
        }

        int count = _poolObjectList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolObject poolObject = _poolObjectList[i];

            if (poolObject.gameObject == removeObject)
            {
                _activeCount--;

                poolObject.gameObject.transform.position = tempPosition;
                poolObject.isActive = false;
                poolObject.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolObject()
    {
        if (_poolObjectList == null)
        {
            return;
        }

        int count = _poolObjectList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolObject poolObject = _poolObjectList[i];

            if (poolObject.gameObject != null && poolObject.isActive == true)
            {
                poolObject.gameObject.transform.position = tempPosition;
                poolObject.isActive = false;
                poolObject.gameObject.SetActive(false);

                return;
            }
        }
        _activeCount = 0;
    }
}
