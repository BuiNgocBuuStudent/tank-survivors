using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTrackSpawner : MonoBehaviour
{
    [SerializeField] GameObject _trackPrefab;
    [SerializeField] float _trackDistance;
    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = this.transform.position;
    }
    
    public void SpawnTrack()
    {
        float distance = Vector2.Distance(lastPosition, this.transform.position);
        if (distance >= _trackDistance)
        {
            lastPosition = this.transform.position;
            GameObject track = ObjectPooler.Instance.GetObject(_trackPrefab);
            track.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            track.SetActive(true);
        }
    }
}
