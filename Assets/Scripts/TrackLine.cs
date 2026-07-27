using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLine : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    private Coroutine _deactivateWait;

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
