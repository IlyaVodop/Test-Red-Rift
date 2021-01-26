using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button _gameBtn;
    void Start()
    {
        _gameBtn.onClick.AddListener(GameManager.Instance.RandomCharacteristic);
    }

}
