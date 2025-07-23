using UnityEngine;

public class Sence1 : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)
        || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)
        || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            _gameManager.SenceChange(2);
        }

    }
}
