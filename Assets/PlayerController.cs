using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float[] _layLinesX; // 0, 1, 2
    [SerializeField] float _moveSpeed;
    [SerializeField] float _lineChangeSpeed;
    [SerializeField] private int initialHealth;
    [SerializeField] float difficultyIncrease;

    #region Firework Vars

    #region Mesh Vars
    [Header("Meshes Descending Entropy")]
    [SerializeField] Mesh[] carMeshes;
    MeshFilter _MeshFilter;
    #endregion

    [Header("Firework boost:")]
    [SerializeField] GameObject boostParticles;
    [SerializeField] float boostTime;
    [SerializeField] float boostSpeedAdd;
    bool isBoosting = false;

    #endregion

    int _currentLine;
    float _lineTimer = 2;
    float _maxRotAngle = 14F;
    int _rotScalar = 1;
    Vector3 _previousRot;
    Vector3 _previousPos;
    Rigidbody _rb;
    float _startMoveSpeed, _startLineChangeSpeed;
    bool isAlive = true;
    public static bool isGame = false;

    // Public
    public static System.Action PlayerHit;
    public static event System.Action PlayerTookDamage;
    public static event System.Action<PlayerController> PlayerDied;
    public static int health {get; private set;}
    public static int maxHealth {get; private set;}

    void OnEnable()
    {
        PlayerHit += TakeDamage;
        Firework.Collected += EnableBoost;
    }

    void OnDisable()
    {
        PlayerHit -= TakeDamage;
        Firework.Collected -= EnableBoost;
    }

    void EnableBoost()
    {
        StopAllCoroutines();
        StartCoroutine(Boost());
    }
    
    IEnumerator Boost()
    {
        if (!isBoosting)
        {
            boostParticles.SetActive(true);
            _moveSpeed += boostSpeedAdd;
            isBoosting = true;
        }

        yield return new WaitForSeconds(boostTime);

        if (isBoosting)
        {
            _moveSpeed -= boostSpeedAdd;
            isBoosting = false;
            boostParticles.SetActive(false);
        }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _MeshFilter = GetComponent<MeshFilter>();
        maxHealth = initialHealth;
        health = initialHealth;

        _startMoveSpeed = _moveSpeed;
        _startLineChangeSpeed  = _lineChangeSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Go to center line
        _rb.transform.position = new Vector3(_layLinesX[1], _rb.transform.position.y, 0);
        _currentLine = 1;
    }

    void TakeDamage()
    {
        health--;

        if (health >= 0)
        {
            PlayerTookDamage?.Invoke();

            if (health > 0)
            {
                _MeshFilter.mesh = carMeshes[(carMeshes.Length - 1) - (health - 1)];
            }

            if (health == 0)
            {
                // Died
                isAlive = false;  
                PlayerDied?.Invoke(this);
                
                // Disable all children
                for (int i = 0; i < transform.childCount; i++)
                {
                    StopCoroutine(Boost());
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        
    }

    void GoToLine(int index)
    {
        if (index >= _layLinesX.Length || index < 0)
            return;
        
        if (index > _currentLine)
        {
            _rotScalar = 1;
        }
        else if ((index < _currentLine))
        {
            _rotScalar = -1;
        }
        
        _currentLine = index;
        _lineTimer = 0;
        _previousRot = _rb.transform.rotation.eulerAngles;
        _previousPos = _rb.transform.position;
    }

    void FixedUpdate() 
    {
        if (!isAlive)
            return;

        Vector3 newPos = _rb.transform.position + Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
        Vector3 newRot = _rb.transform.rotation.eulerAngles;

        if (_lineTimer <= 1)
        {
            _lineTimer += Time.fixedDeltaTime * _lineChangeSpeed;

            newPos.x = Mathf.Lerp(_previousPos.x, _layLinesX[_currentLine], Mathf.SmoothStep(0, 1, _lineTimer));
            
            if (_lineTimer <= 0.5F)
            {
                newRot.y = Mathf.LerpAngle(_previousRot.y, _rotScalar * _maxRotAngle,  Mathf.SmoothStep(0, 1, _lineTimer / 0.5F));
            }
            else
            {
                newRot.y = Mathf.Lerp(_rotScalar * _maxRotAngle, 0, Mathf.SmoothStep(0, 1, (_lineTimer - 0.5F) / 0.5F));
            }
        }

        _rb.MovePosition(newPos);
        _rb.MoveRotation(Quaternion.Euler(newRot));
        
        if (isGame)
        {
            _moveSpeed += difficultyIncrease * Time.fixedDeltaTime * _startMoveSpeed;
            _lineChangeSpeed += difficultyIncrease * Time.fixedDeltaTime * _startLineChangeSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ClickedRight();
            Debug.Log("clicked d");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ClickedLeft();
            Debug.Log("clicked a");
        }
    }

    public void ClickedRight()
    {
        GoToLine(_currentLine + 1);
    }

    public void ClickedLeft()
    {
        GoToLine(_currentLine - 1);
    }
}
