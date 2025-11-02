using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    [SerializeField] SlideBar _expBar;

    [SerializeField] float _maxExp;
    [SerializeField] float _currentExp;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        _expBar.SetMaxValue(_maxExp);
        _expBar.UpdateValue(_currentExp);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateExp(float value)
    {
        _currentExp += value;
        _expBar.UpdateValue(_currentExp);
    }
}
