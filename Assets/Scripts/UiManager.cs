using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance = null;

    [SerializeField] private Button _gameBtn;
    void Start()
    {
        _gameBtn.onClick.AddListener(GameManager.Instance.RandomCharacteristic);
    }

    void Update()
    {

    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }

}
