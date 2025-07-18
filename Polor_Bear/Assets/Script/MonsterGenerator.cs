using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private float _spawnInterval = 2.0f;
    private List<GameObject> _clones = new List<GameObject>();
    private Coroutine _spawnCoroutine;

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAndClear();
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

}
