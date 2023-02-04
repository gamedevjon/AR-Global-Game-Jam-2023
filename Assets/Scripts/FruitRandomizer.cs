using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] _fruits;

    private void Start()
    {
        int RNG = Random.Range(0, _fruits.Length);
        _fruits[RNG].SetActive(true);
    }
}
