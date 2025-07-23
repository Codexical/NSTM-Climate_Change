using UnityEngine;

public class QuestionController : MonoBehaviour
{
    [SerializeField] private Sprite[] _QuestionsList;
    private SpriteRenderer _spriteRenderer;

    public void SetQuestion(int index)
    {
        // Ensure the index is within bounds
        if (index < 0 || index >= _QuestionsList.Length)
        {
            Debug.LogError("Index out of bounds for QuestionsList.");
            return;
        }
        Debug.Log($"Setting question at index: {index}");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _QuestionsList[index];

    }
}
