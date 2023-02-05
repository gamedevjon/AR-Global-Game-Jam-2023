using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class HomeBase : MonoBehaviour
{
    GameObject _arSessionOrigin;
    BaseSpawnManager _baseManager;
    ARPlaneManager _planeManager;
    [SerializeField] GameObject[] _playAreas;
    int _activePlayArea = 0;

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _baseManager = _arSessionOrigin.GetComponent<BaseSpawnManager>();
        _planeManager = _arSessionOrigin.GetComponent<ARPlaneManager>();
    }

    public void SetPlayArea(Slider slider)
    {
        foreach(GameObject area in _playAreas)
        {
            area.SetActive(false);
        }
        _activePlayArea = (int)slider.value;
        _playAreas[_activePlayArea].SetActive(true);
    }

    public void BaseSet()
    {
        _baseManager.enabled = false;
        _planeManager.SetTrackablesActive(false);
        UIManager.Instance.DisplayHomeBasUI(false);
        List<Transform> spawnList = new List<Transform>();
        foreach(Transform child in transform.GetChild(_activePlayArea).GetComponentsInChildren<Transform>())
        {
            if (child.TryGetComponent<Renderer>(out Renderer render)) continue;
            spawnList.Add(child);
        }

        SpawnManager.Instance.GatherSpawnPoints(spawnList.ToArray());
    }
}
