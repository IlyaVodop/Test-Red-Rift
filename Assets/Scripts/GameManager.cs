using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private Vector3[] positionAndRotation = new Vector3[]
    {
        new Vector3(5.7f, -3.04f, -22.25f), new Vector3(4.87f, -2.77f, -15.2f),
        new Vector3(3.66f, -2.62f, -1.76f), new Vector3(2.87f, -2.68f, 7.6f), 
        new Vector3(1.99f, -2.82f, 13.16f), new Vector3(1.13f, -3.07f, 18.4f),
    };
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            var posAndRot = positionAndRotation[i];
            var spawnedObject = Instantiate(_prefab, new Vector3(posAndRot.x, posAndRot.y, i*2), Quaternion.identity);
            spawnedObject.transform.eulerAngles = new Vector3(0, 0, posAndRot.z);
            spawnedObject.GetComponent<CardView>().Init();
        }
    }
}
