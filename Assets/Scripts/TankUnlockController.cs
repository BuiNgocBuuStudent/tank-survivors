using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankUnlockController : MonoBehaviour
{
    [SerializeField] Button _selectTankBtn;
    [SerializeField] Button _unlockTankBtn;
    [SerializeField] Sprite[] _tankSprites;
    [SerializeField] Image _tankImage;
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _unlockTankCostText;
    [SerializeField] TextMeshProUGUI _selectTankText;
    [SerializeField] GameObject[] _lockPanelList;
    private UpgradeManager _upgradeManager;

    private int _selectedTank;
    [SerializeField] private int _currentSpriteIndex;

    private void OnDisable()
    {
        if (_upgradeManager != null)
            _upgradeManager.OnCoinsChanged -= OnCoinsChanged;
    }

    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
        _selectedTank = _upgradeManager.SelectedTankId;
        _currentSpriteIndex = _selectedTank;
        SetSelectTankText();
        SetSwapTankUI();
        _animator.SetInteger("TankId", _currentSpriteIndex);

        _upgradeManager.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnCoinsChanged(int coins)
    {
        UpdateUnlockButtonInteractable();
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

        _upgradeManager.PreviewTank(_currentSpriteIndex);
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

        _upgradeManager.PreviewTank(_currentSpriteIndex);
    }

    public void OnSelectButtonClicked()
    {
        if (_upgradeManager.SetSelectedTank(_currentSpriteIndex))
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

        if (!isUnlocked)
            UpdateUnlockButtonInteractable();
    }

    private void UpdateUnlockButtonInteractable()
    {
        if (!_upgradeManager.IsTankUnlocked(_currentSpriteIndex))
        {
            int cost = _upgradeManager.GetTankUnlockCost(_currentSpriteIndex);
            _unlockTankBtn.interactable = _upgradeManager.PlayCoins >= cost;
        }
    }
}
