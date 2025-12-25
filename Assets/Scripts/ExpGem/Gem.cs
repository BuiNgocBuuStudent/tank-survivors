using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

    [SerializeField] private ExpGemData _expGemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_expGemData != null)
            GameManager.Instance.ExpManager.UpdateExp(_expGemData.exp);
        gameObject.SetActive(false);
    }
}
