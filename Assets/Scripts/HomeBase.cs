using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class HomeBase : MonoBehaviour, IDamagable
{
    GameObject _arSessionOrigin;
    BaseSpawnManager _baseManager;
    ARPlaneManager _planeManager;
    [SerializeField] GameObject[] _playAreas;
    [SerializeField] int _activePlayArea = 0;
    [SerializeField] GameObject _healthBarCanvas;
    Slider _healthSlider;
    [SerializeField] private int _health;

    public int Health { get => _health; set => _health = value; }

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

    public void Damage(int DamageAmount)
    {
        if (!_healthBarCanvas.activeInHierarchy)
            _healthBarCanvas.SetActive(true);

        Health -= DamageAmount;
        _healthSlider.SetValueWithoutNotify(Health);

        if (Health <= 0)
            OnDefeat();
    }

    public void OnDefeat()
    {
        UIManager.Instance.GameOverScreen();
    }
}
