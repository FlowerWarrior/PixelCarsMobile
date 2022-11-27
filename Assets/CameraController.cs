using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    Vector3 _offset;
    Vector3 initialPos;

    // Shake Parameters
    public float shakeDuration = 2f;
    public float shakeAmount = 0.7f;

    private bool canShake = false;
    private float _shakeTimer;

    void OnEnable()
    {
        PlayerController.PlayerTookDamage += ShakeCamera;
    }

    void OnDisable()
    {
        PlayerController.PlayerTookDamage -= ShakeCamera;
    }

    void Start()
    {
        _offset = transform.position - _target.position;
        initialPos = transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = _target.position + _offset;
        newPos.x = initialPos.x;

        if (canShake)
        {
            if (_shakeTimer > 0)
            {
                newPos += Random.insideUnitSphere * shakeAmount;
                _shakeTimer -= Time.deltaTime;
            }
            else
            {
                _shakeTimer = 0f;
                canShake = false;
            }
        }

        transform.position = newPos;
    }
    
    void ShakeCamera()
    {
        canShake = true;
        _shakeTimer = shakeDuration;
    }
}
