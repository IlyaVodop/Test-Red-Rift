using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private float posX;
    private float[] rotatonPos = new float[] { 25, 16.33f, 2.6f, -1.1f, -18.9f, -37.4f };
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            posX = posX + 1f;
            var spawnedObject = Instantiate(_prefab, new Vector3(posX, _prefab.transform.position.y, _prefab.transform.position.z), Quaternion.identity);
            spawnedObject.transform.eulerAngles = new Vector3(0, 0, rotatonPos[i]);
            spawnedObject.GetComponent<CardView>().Init(i);
        }
    }
}
