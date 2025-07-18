using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Sprite[] _healthSprites;

    private SpriteRenderer _spriteRenderer;

    private int _healthIndex = 0;

    public void Init()
    {
        _healthIndex = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _healthSprites[0];
    }
    public bool bloodLoss()
    {
        _healthIndex++;
        if (_healthIndex < _healthSprites.Length)
        {
            _spriteRenderer.sprite = _healthSprites[_healthIndex];
            return true;
        }
        else
        {
            return false;
        }
    }
}
