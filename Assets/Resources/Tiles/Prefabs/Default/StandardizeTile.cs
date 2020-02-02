using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardizeTile : MonoBehaviour
{
    private const float expectedSize = 1.6f;

    // Start is called before the first frame update
    void Start()
    {
        var meshSize = GetComponent<MeshRenderer>().bounds.size;

        var expectedScale = expectedSize / meshSize.x;

        transform.localScale = new Vector3(expectedScale, expectedScale, expectedScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
