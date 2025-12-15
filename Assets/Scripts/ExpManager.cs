using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    private BoostManager _boostManager;
    [SerializeField] SlideBar _expBar;

    [SerializeField] float _maxExp;
    [SerializeField] float _currentExp;
    public float expIncPercentage;
    public void Init()
    {
        _expBar.SetMaxValue(_maxExp);
        _expBar.UpdateValue(_currentExp);

        _boostManager = GameManager.Instance.BoostManager;
    }
    public void UpdateExp(float value)
    {
        _currentExp += value;
        if (_currentExp >= _maxExp)
        {
            _boostManager.showBoostPopup();
            Debug.Log("Level up!!!");
            _currentExp = 0;
            _maxExp *= (1 + expIncPercentage / 100);
            _expBar.SetMaxValue(_maxExp);
        }
        _expBar.UpdateValue(_currentExp);
    }
}
