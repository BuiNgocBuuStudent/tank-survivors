using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] Button _continueBtn;
    [SerializeField] Button _selectTankBtn;
    [SerializeField] Button _unlockTankBtn;
    [SerializeField] Sprite[] _tankSprites;
    [SerializeField] Image _tankImage;
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _unlockTankCostText;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _selectTankText;
    [SerializeField] GameObject[] _lockPanelList;
    private UpgradeManager _upgradeManager;

    private int _selectedTank;
    [SerializeField] private int _currentSpriteIndex;
    private int _coins;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
            _continueBtn.interactable = false;

        _upgradeManager = UpgradeManager.Instance;

        _selectedTank = _upgradeManager.SelectedTankId;
        UpdateCoinUI();
        _currentSpriteIndex = _selectedTank;
        SetSelectTankText();

        _animator.SetInteger("TankId", _currentSpriteIndex);

        _upgradeManager.OnCoinsChanged += UpdateCoinUI;
    }

    #region Unlock Tank
    private void UpdateCoinUI()
    {
        _coins = _upgradeManager.PlayerCoins;
        _coinText.text = _coins.ToString();
    }
    public void OnStartButtonClicked()
    {
        DisableButton();
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("PlayScene");

    }

    public void OnContinueButtonClicked()
    {
        DisableButton();
        SceneManager.LoadSceneAsync("PlayScene");
    }

    private void DisableButton()
    {
        _startBtn.interactable = false;
        _continueBtn.interactable = false;
    }

    public void OnRightArrowClicked()
    {
        _currentSpriteIndex++;

        if (_currentSpriteIndex > _tankSprites.Length - 1)
            _currentSpriteIndex = 0;

        _tankImage.sprite = _tankSprites[_currentSpriteIndex];
        SetSelectTankText();
        SetSwapTankUI();

        _animator.SetInteger("TankId", _currentSpriteIndex);
    }
    public void OnLeftArrowClicked()
    {
        _currentSpriteIndex--;

        if (_currentSpriteIndex < 0)
            _currentSpriteIndex = _tankSprites.Length - 1;

        _tankImage.sprite = _tankSprites[_currentSpriteIndex];
        SetSelectTankText();
        SetSwapTankUI();

        _animator.SetInteger("TankId", _currentSpriteIndex);
    }


    public void OnSelectButtonClicked()
    {
        if (_upgradeManager.SetSelectedTankId(_currentSpriteIndex))
        {
            _selectedTank = _currentSpriteIndex;
            SetSelectTankText();
        }
    }

    public void OnUnlockTankButtonClicked()
    {
        if (_upgradeManager.UnlockTank(_currentSpriteIndex))
        {
            SetUnlockTankUI(true);
        }
    }

    private void SetSelectTankText()
    {
        _selectTankText.text = _selectedTank == _currentSpriteIndex ? "đã chọn" : "chọn";        
    }
     
    private void SetSwapTankUI()
    {
        if (_upgradeManager.IsTankUnlocked(_currentSpriteIndex))
        {
            SetUnlockTankUI(true);
        }
        else
        {
            _unlockTankCostText.text = _upgradeManager.GetTankUnlockCost(_currentSpriteIndex).ToString();
            SetUnlockTankUI(false);
        }
    }
    private void SetUnlockTankUI(bool isUnlocked)
    {
        _selectTankBtn.gameObject.SetActive(isUnlocked);
        _unlockTankBtn.gameObject.SetActive(!isUnlocked);
        foreach (GameObject panel in _lockPanelList)
        {
            panel.SetActive(!isUnlocked);
        }
    }
    #endregion


}
