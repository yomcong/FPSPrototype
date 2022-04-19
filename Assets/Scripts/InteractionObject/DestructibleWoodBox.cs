using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWoodBox : InteractionObjectBase
{
    [Header("Destructible Barrel")]
    [SerializeField]
    private GameObject _destrctibleWoodPieces;
    [SerializeField]
    private GameObject _textDamage;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Transform _textDamageSpawnPoint;

    private bool _isDestroyed = false;

    public override void TakeDamage(int damage)
    {
        _currentHP -= damage;

        GameObject textDamage = Instantiate(_textDamage, _textDamageSpawnPoint.position, Quaternion.identity, _canvas.transform);
        textDamage.GetComponent<DamageText>()?.Setup(damage, _canvas, _textDamageSpawnPoint.position);

        if (_currentHP <= 0 && _isDestroyed == false)
        {
            _isDestroyed = true;

            Instantiate(_destrctibleWoodPieces, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
