using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;
    private Coroutine _flashRoutine;
    [SerializeField] Material _flashMaterial;

    [SerializeField] float _duration;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    public void ResetMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }
    private void OnDisable()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
    }
    public void Flash()
    {
        _flashRoutine = StartCoroutine(FlashRoutine());
    }
    IEnumerator FlashRoutine()
    {
        _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_duration);
        ResetMaterial();
        _flashRoutine = null;
    }
}
