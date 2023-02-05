using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantBehavior : MonoBehaviour, IDamagable
{
    public int Health { get; set; }

    [SerializeField] float _deathCoolDownTime;
    [SerializeField] GameObject _leafs, _fruit, _healthBarCanvas;
    Slider _healthSlider;

    [SerializeField] int _health;


    private void Start()
    {
        
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

    

    // Update is called once per frame
    void Update()
    {

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

    [ContextMenu("HitTest")]
    public void HitTest()
    {
        Damage(1);
    }
}
