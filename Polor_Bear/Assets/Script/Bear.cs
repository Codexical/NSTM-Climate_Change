using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bear : MonoBehaviour
{
    [SerializeField] private Sprite[] _bearBase;
    [SerializeField] private Sprite[] _bearHit;
    [SerializeField] private Timer _timer;
    private SpriteRenderer _spriteRenderer;
    private int _bearHealth = 0;
    private int[] _bearBaseHeight = { 1561, 1966, 2048, 1886, 1608, 1000 };
    private Sence3 _parent;
    private float fadeDuration = 0.5f;

    private void Start()
    {
        _parent = GetComponentInParent<Sence3>();
    }

    public void Init()
    {
        _bearHealth = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _bearBase[_bearHealth];
    }

    public void hit()
    {
        _bearHealth++;
        if (_bearHealth < _bearBase.Length)
        {
            StartCoroutine(bearHit(_bearHealth));
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _bearBaseHeight[_bearHealth] / 2.05f);
        }
        else
        {
            _timer.StopTimer();
            _parent.GameFailed();
        }
    }

    private IEnumerator bearHit(int index)
    {
        _spriteRenderer.sprite = _bearHit[index - 1];
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.sprite = _bearBase[index];
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.sprite = _bearHit[index - 1];
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.sprite = _bearBase[index];
    }

    public int health()
    {
        return _bearHealth;
    }
}
