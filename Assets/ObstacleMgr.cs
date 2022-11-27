using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMgr : MonoBehaviour
{
    [SerializeField] Transform[] _prefabs;
    [SerializeField] float[] _layLinesX; // 0, 1, 2
    [SerializeField] float _spacing;
    [SerializeField] float _spawnDistance;
    [SerializeField] Rigidbody _playerRb;
    [SerializeField] int _doubleChance = 50;
    Vector3 _lastPos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SpawnObstacle()
    {
        Vector3 pos = Vector3.zero;
        int lane = Random.Range(0, _layLinesX.Length);
        pos.x = _layLinesX[lane];
        pos.z = _playerRb.transform.position.z + _spawnDistance;

        Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], pos, Quaternion.identity, transform);

        if (Random.Range(0, 100) < _doubleChance)
        {
            List<int> availableLanes = new List<int>{0, 1, 2};
            availableLanes.Remove(lane);

            lane = Random.Range(0, 2);
            pos.x = _layLinesX[availableLanes[lane]];
            Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], pos, Quaternion.identity, transform);
        }

        _lastPos = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerRb.transform.position.z > _lastPos.z + _spacing - _spawnDistance)
        {
            SpawnObstacle();
        }
    }
}
