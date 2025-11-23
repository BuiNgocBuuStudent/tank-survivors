using System.Collections;
using UnityEngine;
public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private Coroutine _flashRoutine;

    [SerializeField] private float _duration;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }
    public void Flash()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    public void ResetMaterial()
    {
        _spriteRenderer.material = _originalMaterial;
    }
    private IEnumerator FlashRoutine()
    {
        _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_duration);
        ResetMaterial();
        _flashRoutine = null;
    }
}
