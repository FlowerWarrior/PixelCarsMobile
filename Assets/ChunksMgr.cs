using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksMgr : MonoBehaviour
{
    [SerializeField] Transform[] _chunkPrefabs;
    [SerializeField] float _chunkSpacing;
    [SerializeField] float _maxChunks;
    [SerializeField] float _destroyDistance;
    [SerializeField] Rigidbody _playerRb;
    Vector3 _furthestPos;
    Vector3 _lastPos;

    // Start is called before the first frame update
    void Start()
    {
        _lastPos = new Vector3(0, 0, -_destroyDistance);

        // Destroy initial in scene
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Starting chunks
        for (int i = 0; i < _maxChunks; i++)
        {
            int chunkIndex = Random.Range(0, _chunkPrefabs.Length);
            Vector3 pos = transform.position;
            pos.z = _lastPos.z + _chunkSpacing;
            
            Instantiate(_chunkPrefabs[chunkIndex], pos, Quaternion.identity, transform);
            _lastPos = pos;

            if (i == 0)
            {
                _furthestPos = _lastPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerRb.transform.position.z - _furthestPos.z  > _destroyDistance)
        {
            Destroy(transform.GetChild(0).gameObject);
            _furthestPos = transform.GetChild(0).position;

            int chunkIndex = Random.Range(0, _chunkPrefabs.Length);
            Vector3 pos = transform.position;
            pos.z = _lastPos.z + _chunkSpacing;
            
            Instantiate(_chunkPrefabs[chunkIndex], pos, Quaternion.identity, transform);
            _lastPos = pos;
        }
    }
}
