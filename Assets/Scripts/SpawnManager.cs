using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    Transform[] _areaSpawnPoints;
    bool _isSpawning = false;
    private int _currentWave = 1;
    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else _instance = this;
    }
        
    public void GatherSpawnPoints(Transform[] spawnPointArray)
    {
        _areaSpawnPoints = spawnPointArray;
    }

    public void ActivateSpawn(bool isOn)
    {
        _isSpawning = isOn;
        if (_isSpawning)
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        int count = 0;
        while(_isSpawning) 
        {
            int RNG = Random.Range(0, _areaSpawnPoints.Length);
            Instantiate(_enemyPrefab, _areaSpawnPoints[RNG].position, Quaternion.identity);
            count++;
            yield return new WaitForSeconds(5f);
            if (count > GetSpawnCount()) ActivateSpawn(false);
        }

        //spawning ended. Increase wave
        _currentWave++;
    }

    [ContextMenu("TestSpawn")]
    public void TestSpawn()
    {
        GameObject.FindObjectOfType<HomeBase>().BaseSet();
        ActivateSpawn(true);
    }

    int GetSpawnCount()
    {
        return _currentWave * 5;
    }
}
