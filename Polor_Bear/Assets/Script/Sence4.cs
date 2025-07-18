using UnityEngine;

public class Sence4 : MonoBehaviour
{
    [SerializeField] private Sprite[] _healthList;
    [SerializeField] private Sprite[] _bearWin;
    [SerializeField] private GameObject _health;
    [SerializeField] private GameObject _bear;

    public void SetBearHealth(int health)
    {
        _health.GetComponent<SpriteRenderer>().sprite = _healthList[health];
        _bear.GetComponent<SpriteRenderer>().sprite = _bearWin[health];
    }
}
