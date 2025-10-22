using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance => _instance;

    private void Awake()
    {
        if (Instance == null)
            _instance = GetComponent<T>();

        if (this.GetInstanceID() != Instance.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}
