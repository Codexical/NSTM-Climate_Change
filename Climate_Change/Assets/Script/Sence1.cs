using UnityEngine;

public class Sence1 : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            _gameManager.SenceChange(2);
        }

    }
}
