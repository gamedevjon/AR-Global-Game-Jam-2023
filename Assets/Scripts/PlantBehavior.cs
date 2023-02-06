using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantBehavior : MonoBehaviour, IDamagable
{
    [SerializeField] float _deathCoolDownTime;
    [SerializeField] GameObject _leafs, _fruit, _healthBarCanvas;
    Slider _healthSlider;

    [SerializeField] int _health;

    public int Health { get => _health; set => _health = value; }

    private void OnEnable()
    {
        GameManager.onWaveComplete += CashGenerate;
    }

    private void Start()
    {
        _healthBarCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        _healthSlider = _healthBarCanvas.GetComponentInChildren<Slider>();
        Health = _health;
        _healthSlider.maxValue = _health;
        _healthSlider.value = _health;
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
        _leafs.SetActive(false);
        _fruit.SetActive(false);
        _healthBarCanvas.SetActive(false); 
        StartCoroutine(DeathCooldown());
    }

    IEnumerator DeathCooldown()
    {
        float deathCountDown = _deathCoolDownTime;
        while (deathCountDown > 0)
        {
            yield return new WaitForEndOfFrame();
            deathCountDown -= Time.deltaTime;
        }
        Destroy(this.gameObject);
    }

    private void CashGenerate()
    {
        GameManager.Instance.AddCash();
    }

    [ContextMenu("HitTest")]
    public void HitTest()
    {
        Damage(1);
    }

    private void OnDisable()
    {
        GameManager.onWaveComplete += CashGenerate;
    }
}
