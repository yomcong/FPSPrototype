using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float _deactivateTime = 5.0f;

    [SerializeField]
    private float _casingSpin = 1.0f;

    [SerializeField]
    private AudioClip _audioClips;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private MemoryPool _memoryPool;

    public void SetUp(MemoryPool pool, Vector3 direction)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _memoryPool = pool;

        _rigidbody.velocity = new Vector3(direction.x, 1.0f, direction.z);
        _rigidbody.angularVelocity = new Vector3(Random.Range(-_casingSpin, _casingSpin),
                                                Random.Range(-_casingSpin, _casingSpin),
                                                Random.Range(-_casingSpin, _casingSpin));



        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.clip = _audioClips;
        _audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_deactivateTime);

        _memoryPool.DeactivatePoolObject(gameObject);
    }

}
