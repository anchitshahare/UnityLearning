using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // every variable has following things:
    // public of private reference
    // data type (int, float, bool ,string)
    // every variable has a name
    // optional value assigned
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotEnabled = false;
    private bool _isSpeedEnabled = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightWingDamage;
    [SerializeField]
    private GameObject _leftWingDamage;
    private AudioSource _audiosource;
    [SerializeField]
    private AudioClip _laseraudioClip;
    [SerializeField]
    private AudioClip _explosionClip;
    [SerializeField]
    private InputAction _move;
    [SerializeField]
    private InputAction _fire;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audiosource = GetComponent<AudioSource>();
        // _move = GetComponent<InputAction>();
        // _fire = GetComponent<InputAction>();
        

        if(_spawnManager == null) {
            Debug.LogError("Spawn Manager is null");
        }

        if(_uiManager == null){
            Debug.LogError("UI Manager is null");
        }

        if(_audiosource == null) {
            Debug.LogError("Audio Source is null");
        } else {
            _audiosource.clip = _laseraudioClip;
        }
    }

    private void OnEnable() {
        _move.Enable();
        _fire.performed += FireLaser;
        _fire.Enable();
    }

    private void OnDisable() {
        _move.Disable();
        _fire.Disable();
    }


    // Update is called once per frame
    void Update()
    {   
        CalculateMovement();
        // _move.ReadValue<Vector3>();

        // if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        // {
        //     FireLaser();
        // }
        
    }
    
    
    public void CalculateMovement() {
        Vector3 direction = _move.ReadValue<Vector3>();

        if(_isSpeedEnabled == true) {
            _speed = 10.0f;
        }
        else {
            _speed = 5.0f;
        }
        transform.Translate(direction * _speed * Time.deltaTime);

        if(transform.position.y >= 8) 
        {
            transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);
        }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, 8, transform.position.z);
        }

        // Restrict user movement in the y axis
        // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if(transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if(transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }

    public void FireLaser(InputAction.CallbackContext obj)
    {        
        _canFire = Time.time + _fireRate;
        
        if(_isTripleShotEnabled == true) {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, transform.position + new Vector3(0,1.05f,0), Quaternion.identity);
        }
        
        _audiosource.Play();
    }

    public void Damage() {

        if(_isShieldActive == true) {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        _uiManager.UpdateLives(_lives);

        if(_lives == 2) {
            _rightWingDamage.SetActive(true);
        }

        if(_lives == 1) {
            _leftWingDamage.SetActive(true);
        }

        if(_lives == 0) {
            _spawnManager.OnPlayerDeath();
            _uiManager.BestScore(_score);
            Destroy(this.gameObject);
        }
 
    }

    public void TripleShotActive() {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotEnabled = false;
    }

    public void SpeedActive() {
        _isSpeedEnabled = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isSpeedEnabled = false;
    }

    public void ShieldActive() {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points) {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
