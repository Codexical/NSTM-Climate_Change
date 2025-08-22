using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] private HandTracker _handTracker;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private float _spawnInterval = 2.0f;
    private List<GameObject> _clones = new List<GameObject>();
    private Coroutine _spawnCoroutine;
    private bool _isGaming = false;

    private void OnEnable()
    {
        _clones.Clear();
        _spawnCoroutine = StartCoroutine(Spawn());
        _isGaming = true;
    }

    private void OnDisable()
    {
        StopAndClear();
        _isGaming = false;
    }

    private void SpawnObstacles()
    {
        GameObject obj = Instantiate(_obstacle, _parent);
        _clones.Add(obj);
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnObstacles();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    public void StopAndClear()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }

        foreach (GameObject obj in _clones)
        {
            if (obj != null)
                Destroy(obj);
        }

        _clones.Clear();
    }


    private void Update()
    {
        if (!_isGaming)
        {
            return;
        }

        var coordinates = _handTracker.GetClickArea();
        if (coordinates == null)
        {
            return;
        }

        foreach (GameObject obj in _clones)
        {
            if (obj != null)
            {
                int posX = (int)obj.transform.position.x * 16;
                int posY = (int)obj.transform.position.y * 16;
                int width = 40;
                int height = 40;

                int offsetX = 320;
                int offsetY = 180;
                int minX = (posX - width / 2) * 640 / 3413;
                int maxX = (posX + width / 2) * 640 / 3413;
                int minY = (posY - height / 2) * 360 / 1920;
                int maxY = (posY + height / 2) * 360 / 1920;

                bool isAreaTrigger = false;
                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        if (coordinates[y + offsetY, x + offsetX])
                        {
                            isAreaTrigger = true;
                            break;
                        }
                    }
                }
                if (isAreaTrigger)
                {
                    Monster monster = obj.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.MonsterHit();
                    }
                }
            }
        }
    }
}
