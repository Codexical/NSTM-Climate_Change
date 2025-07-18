using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sence3Background : MonoBehaviour
{
    [SerializeField] private Sprite[] _backgrounds;
    private Image _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<Image>();
        _spriteRenderer.sprite = _backgrounds[0];
    }

    public void warning()
    {
        StartCoroutine(FlashWarning());
    }

    private IEnumerator FlashWarning()
    {
        float flashDuration = 0.2f;
        _spriteRenderer.sprite = _backgrounds[1];
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.sprite = _backgrounds[0];
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.sprite = _backgrounds[1];
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.sprite = _backgrounds[0];
    }
}
