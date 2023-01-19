using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _speed = 4f;    
    private Player _player;
    Animator _animator;
    [SerializeField]
    private AudioSource _explosionAudiosource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionAudiosource = GetComponent<AudioSource>();

        if(_player == null) {
            Debug.LogError("Player is null");
        }
        _animator = GetComponent<Animator>();

        if(_animator == null) {
            Debug.LogError("Animator is null");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        FireEnemyLaser();
    }

    void Movement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y <= -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    void FireEnemyLaser()
    {        
        if(Time.time > _canFire) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0,-2f,0), Quaternion.identity);
            Laser[] laser = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < laser.Length; i++) {
                laser[i].AssignEnemy();
            }
        }
        
    
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();

            if(player != null) {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudiosource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if(_player != null) {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudiosource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
    
}
