using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _placeButtonsHolder, _homeBaseUI, _gameOverUI;
    [SerializeField] Slider _homeBaseSizeSlider;
    [SerializeField] Button _homeSetButton;
    [SerializeField] TMP_Text _coinText;
    [SerializeField] TMP_Text _killText;



    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else _instance = this;
    }

    private void Start()
    {
        if (_placeButtonsHolder.activeInHierarchy)
            _placeButtonsHolder.SetActive(false);
    }

    public void HomeBase(HomeBase homeBase)
    {

        _homeBaseSizeSlider.onValueChanged.AddListener(x => { homeBase.SetPlayArea(_homeBaseSizeSlider); });
        _homeSetButton.onClick.AddListener(homeBase.BaseSet);
    }

    public void DisplayHomeBasUI(bool isOn)
    {
        _homeBaseUI.SetActive(isOn);
    }

    public void DisplayPlanterButtons(bool isOn)
    {
        _placeButtonsHolder.SetActive(isOn);
    }

    public void UpdateCoins(int coins)
    {
        _coinText.text = coins.ToString();
    }

    public void UpdateKillCount()
    {
        _killText.text = $"Total Spiders Killed: {GameManager.Instance.GetScore()}";
    }

    public void GameOverScreen()
    {
        _gameOverUI.SetActive(true);
    }

    [ContextMenu("TestLoad")]
    public void SetHome()
    {
        HomeBase homeBase = GameObject.Find("HomeBase").GetComponent<HomeBase>();
        _homeBaseSizeSlider.onValueChanged.AddListener(x => { homeBase.SetPlayArea(_homeBaseSizeSlider); });
    }
}
