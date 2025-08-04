using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;


public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private HandLandmarkerRunner _HandTracker;
    [SerializeField] private GameCalibration _gameCalibration;
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
        if (_isGaming)
        {
            List<Vector2> coordinates = _HandTracker.GetResults();
            if (coordinates == null || coordinates.Count < 1)
            {
                return;
            }
            foreach (var coordinate in coordinates)
            {
                var position = _gameCalibration.transformToGameArea(coordinate);
                var x = position[0];
                var y = position[1];
                Debug.Log("coordinate: " + coordinate);
                Debug.Log("position  : " + position);
                foreach (GameObject obj in _clones)
                {
                    if (obj != null)
                    {
                        if (Vector2.Distance(new Vector2(obj.transform.position.x, obj.transform.position.y), new Vector2(x, y)) < 8f)
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
    }
}
