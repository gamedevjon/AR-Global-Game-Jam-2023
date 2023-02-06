using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public delegate void WaveCompleted();
    public static event WaveCompleted onWaveComplete;

    [SerializeField] GameObject _vegPlanter, _towPlanter, _buttonContainer;


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else _instance = this;
    }

    private int _score;
    private int _cash = 10;

    private void Start()
    {
        UIManager.Instance.UpdateCoins(_cash);
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddCash()
    {
        _cash++;
        UIManager.Instance.UpdateCoins(_cash);
    }

    public void RemoveCash(int cash)
    {
        _cash -= cash;
        UIManager.Instance.UpdateCoins(_cash);
    }

    public void WaveComplete()
    {
        onWaveComplete?.Invoke();
        UIManager.Instance.DisplayPlanterButtons(true);
    }

    public void PlacePlant()
    {
        if (_cash >= 1)
        {
            RemoveCash(1);
            _vegPlanter.SetActive(true);
            _buttonContainer.SetActive(false);
        }
        else
            return;
    }

    public void PlaceTower()
    {
        if (_cash >= 5)
        {
            RemoveCash(5);
            _towPlanter.SetActive(true);
            _buttonContainer.SetActive(false);
        }
        else
            return;
    }
}
