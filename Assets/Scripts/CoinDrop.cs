using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] float _attractSpeed;

    private Transform _playerTransform;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            _playerTransform = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            _playerTransform.position,
            Time.deltaTime * _attractSpeed
        );
        if (Vector3.Distance(this.transform.position, _playerTransform.position) < 0.1f)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}
