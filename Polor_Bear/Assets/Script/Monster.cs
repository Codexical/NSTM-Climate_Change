using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Sprite[] _monster1;
    [SerializeField] private Sprite[] _monster2;
    [SerializeField] private Sprite[] _monster3;
    [SerializeField] private Sprite[] _monster4;
    [SerializeField] private GameObject _hitSprite;
    [SerializeField] private float _speed = 5.0f;

    private SpriteRenderer _spriteRenderer;
    private int _monsterIndex = 0;
    private int _monsterHealth = 0;
    private Sence3 _parent;
    private bool enabled = true;


    private void Start()
    {
        _parent = GetComponentInParent<Sence3>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hitSprite.SetActive(false);
        _monsterIndex = Random.Range(0, 4);
        switch (_monsterIndex)
        {
            case 0:
                _spriteRenderer.sprite = _monster1[0];
                break;
            case 1:
                _spriteRenderer.sprite = _monster2[0];
                break;
            case 2:
                _spriteRenderer.sprite = _monster3[0];
                break;
            case 3:
                _spriteRenderer.sprite = _monster4[0];
                break;
        }
        float distance = 50.0f;
        float degree = Random.Range(0.0f, 360.0f);
        float x = distance * Mathf.Cos(degree * Mathf.Deg2Rad);
        float y = distance * Mathf.Sin(degree * Mathf.Deg2Rad);
        transform.position = new Vector3(x, y, 0);
        if (90.0f <= degree && degree < 180.0f)
        {
            degree += 180.0f;
        }
        else if (180.0f <= degree && degree < 360.0f)
        {
            degree += 90.0f;
        }
        else
        {
            degree += 0.0f;
        }
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private void Update()
    {
        if (enabled)
        {
            Vector3 direction = (Vector3.zero - transform.position).normalized;
            if (Vector3.Distance(transform.position, Vector3.zero) < 20.0f)
            {
                StartCoroutine(bearHit(transform.position));
                enabled = false;
            }
            else
            {
                transform.position += direction * _speed * Time.deltaTime;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator bearHit(Vector3 position)
    {
        while (true)
        {
            _parent.bearHit(transform.position);
            Vector3 direction = (Vector3.zero - transform.position).normalized;
            _hitSprite.transform.position = position + direction * 10.0f;
            _hitSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _hitSprite.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _hitSprite.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _hitSprite.SetActive(false);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(HandleClick());
    }
    private IEnumerator HandleClick()
    {
        _monsterHealth++;
        if ((_monsterIndex != 3 && _monsterHealth <= 1) || (_monsterIndex == 3 && _monsterHealth <= 2))
        {
            switch (_monsterIndex)
            {
                case 0:
                    _spriteRenderer.sprite = _monster1[_monsterHealth];
                    break;
                case 1:
                    _spriteRenderer.sprite = _monster2[_monsterHealth];
                    break;
                case 2:
                    _spriteRenderer.sprite = _monster3[_monsterHealth];
                    break;
                case 3:
                    _spriteRenderer.sprite = _monster4[_monsterHealth];
                    break;
            }
        }
        if ((_monsterIndex != 3 && _monsterHealth == 1) || (_monsterIndex == 3 && _monsterHealth == 2))
        {
            float duration = 0.5f;
            float elapsed = 0f;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + Vector3.up * 2.5f;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;

            Destroy(gameObject);
        }
    }
}
