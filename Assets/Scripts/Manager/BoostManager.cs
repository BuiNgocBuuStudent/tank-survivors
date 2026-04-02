using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public GameObject uiBoost;

    [SerializeField] List<Boost> _boostList;
    [SerializeField] List<Boost> _selectedBoostList;
    [SerializeField] List<TextMeshProUGUI> _textList;

    private float _totalWeight;
    private float _cloneOfTotalWeight;

    public void Init()
    {
        foreach (Boost boost in _boostList)
            _totalWeight += boost.weight;
    }
    public void showBoostPopup()
    {
        _selectedBoostList.Clear();
        SetupLogical();
        uiBoost.SetActive(true);
    }

    private void SetupLogical()
    {
        Boost boostToShow;
        List<Boost> cloneBoostList = new List<Boost>(_boostList);
        _cloneOfTotalWeight = _totalWeight;

        foreach (TextMeshProUGUI text in _textList)
        {
            boostToShow = this.GetRandomBoost(cloneBoostList);
            _selectedBoostList.Add(boostToShow);
            text.text = boostToShow._boostDescription.text;
        }
    }
    private Boost GetRandomBoost(List<Boost> boosts)
    {
        Boost currentBoost = null;

        if (boosts.Count == 0)
            return boosts[0];

        float randomNumber = Random.Range(0, _cloneOfTotalWeight);
        float cumulativeWeight = 0;
        foreach(Boost boost in boosts)
        {
            cumulativeWeight += boost.weight;
            if(randomNumber <= cumulativeWeight)
            {
                _cloneOfTotalWeight -= boost.weight;
                currentBoost = boost;
                break;
            }
        }
        if(currentBoost != null) 
            boosts.Remove(currentBoost);

        return currentBoost;
    }
}
