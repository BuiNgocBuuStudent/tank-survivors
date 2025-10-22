using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereFollower : MonoBehaviour
{
    [SerializeField] Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followPos = _player.position;
        followPos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = followPos;
    }
}
