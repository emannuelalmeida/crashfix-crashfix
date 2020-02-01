using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    GridMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = ScriptableObject.CreateInstance<GridMap>();
        map.Initialize("Default", Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
