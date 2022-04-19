using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _alphaSpeed;

    [SerializeField]
    private Canvas _canvas;
    private Camera _textCanvasCamera;
    private TextMeshProUGUI _textDamage;
    private Color _alpha;

    private RectTransform _rectParent;
    private RectTransform _rectHP;
    private Vector3 _target;
    private Vector3 _offset;


    // Start is called before the first frame update
    private void Awake()
    {
        _textDamage = GetComponent<TextMeshProUGUI>();
        _alpha = _textDamage.color;
        
        _rectHP = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        _offset += new Vector3(0, _moveSpeed * Time.deltaTime, 0);

        var screenPosition = Camera.main.WorldToScreenPoint(_target + _offset);

        if (screenPosition.z < 0.0f)
        {
            screenPosition *= -1.0f;
        }

        Vector2 localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, screenPosition
            , _textCanvasCamera, out localPos);

        _rectHP.localPosition = localPos;

        _alpha.a = Mathf.Lerp(_alpha.a, 0, _alphaSpeed * Time.deltaTime);
        _textDamage.color = _alpha;

        if (_textDamage.color.a < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int damage, Canvas canvas, Vector3 target)
    {
        _textDamage.text = $"{damage}";
        _textDamage.color = Color.red;
        _alpha = _textDamage.color;

        _canvas = canvas;
        _textCanvasCamera = _canvas.worldCamera;

        _rectParent = _canvas.GetComponent<RectTransform>();

        _target = target;
        _offset = Vector3.zero;
    }
}
