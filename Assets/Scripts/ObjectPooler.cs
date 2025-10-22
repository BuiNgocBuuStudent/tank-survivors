using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    Dictionary<GameObject, List<GameObject>> _pool = new Dictionary<GameObject, List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject GetObject(GameObject prefab)
    {
        List<GameObject> listObj = new List<GameObject>();
        if (_pool.ContainsKey(prefab))
        {
            listObj = _pool[prefab];
        }
        else
        {
            _pool.Add(prefab, listObj);
        }

        foreach (GameObject obj in listObj)
        {
            if (obj.activeInHierarchy)
                continue;
            return obj;

        }

        GameObject newObject = Instantiate(prefab, this.transform.position, Quaternion.identity);
        newObject.SetActive(false);
        listObj.Add(newObject);
        return newObject;
    }

    public T GetComp<T>(T prefab) where T : MonoBehaviour
    {
        return this.GetObject(prefab.gameObject).GetComponent<T>();
    }


}
