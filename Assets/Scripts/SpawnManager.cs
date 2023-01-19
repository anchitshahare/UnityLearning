using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-10f,10f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
        
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false) {
            Vector3 postoSpawn = new Vector3(Random.Range(-10f,10f), 7f, 0);
            int randPowerups = Random.Range(0, 3);
            Instantiate(_powerups[randPowerups], postoSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }
}
