using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextHandler : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    private Coroutine _deactivateWait;
    private TextMesh _textMesh;
    public void Init(Color textColor, string textToShow)
    {
        _textMesh = this.GetComponentInChildren<TextMesh>();
        _textMesh.color = textColor;
        _textMesh.text = textToShow;
    }
    private void OnEnable()
    {
        _deactivateWait = StartCoroutine(RepeatLifeTime());
    }
    private void OnDisable()
    {
        if (_deactivateWait != null)
        {
            StopCoroutine(RepeatLifeTime());
            _deactivateWait = null;
        }
    }

    IEnumerator RepeatLifeTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
    }
}
