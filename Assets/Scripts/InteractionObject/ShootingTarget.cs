using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : InteractionObjectBase
{
    [SerializeField]
    private AudioClip _clipTargetUp;
    [SerializeField]
    private AudioClip _clipTargetDown;
    private float _tagetUpDelayTime = 2;

    [SerializeField]
    private GameObject _textDamage;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Transform _textDamageSpawnPoint;

    private AudioSource _audioSource;
    private bool _isPossibleHit = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage(int damage)
    {
        if (_isPossibleHit)
        {
            _isPossibleHit = false;

            GameObject textDamage = Instantiate(_textDamage, _textDamageSpawnPoint.position, Quaternion.identity, _canvas.transform);
            textDamage.GetComponent<DamageText>()?.Setup(damage, _canvas, _textDamageSpawnPoint.position);

            StartCoroutine("OnTargetDown");
        }
    }

    private IEnumerator OnTargetDown()
    {
        _audioSource.clip = _clipTargetDown;
        _audioSource.Play();

        yield return StartCoroutine(OnAnmation(0, 90));

        TutorialTargetEvent tutorialCheck = GetComponent<TutorialTargetEvent>();
        if (tutorialCheck != null)
        {
            tutorialCheck.OnHitEvent();
        }
        else
        {
            StartCoroutine("OnTargetUp");
        }
    }

    private IEnumerator OnTargetUp()
    {
        yield return new WaitForSeconds(_tagetUpDelayTime);

        _audioSource.clip = _clipTargetUp;
        _audioSource.Play();

        yield return StartCoroutine(OnAnmation(90, 0));

        

        _isPossibleHit = true;
    }

    private IEnumerator OnAnmation(float start, float end)
    {
        float percent = 0;
        float current = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current;

            transform.rotation = Quaternion.Slerp(Quaternion.Euler(start, 0, 0), Quaternion.Euler(end, 0, 0), percent);

            yield return null;
        }
    }
}
