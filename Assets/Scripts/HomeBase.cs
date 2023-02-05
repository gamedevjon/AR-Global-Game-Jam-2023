using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeBase : MonoBehaviour
{
    GameObject _arSessionOrigin;
    BaseSpawnManager _baseManager;
    [SerializeField] GameObject[] _playAreas;

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _baseManager = _arSessionOrigin.GetComponent<BaseSpawnManager>();
    }

    public void SetPlayArea(Slider slider)
    {
        foreach(GameObject area in _playAreas)
        {
            area.SetActive(false);
        }
        _playAreas[(int)slider.value].SetActive(true);
    }

    public void BaseSet()
    {
        _baseManager.enabled = false;
        UIManager.Instance.DisplayHomeBasUI(false);
    }
}
