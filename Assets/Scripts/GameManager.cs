using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private GameObject _prefab;
    private List<CardView> _cardView = new List<CardView>();
    private int _count;

    private const int NUMBER_OF_CARDS = 6;

    private Vector3[] _positionAndRotation = new Vector3[]
    {
        new Vector3(5.7f, -3.04f, -22.25f), new Vector3(4.87f, -2.77f, -15.2f),
        new Vector3(3.66f, -2.62f, -1.76f), new Vector3(2.87f, -2.68f, 7.6f),
        new Vector3(1.99f, -2.82f, 13.16f), new Vector3(1.13f, -3.07f, 18.4f),
    };


    void Start()
    {
        for (int i = 0; i < NUMBER_OF_CARDS; i++)
        {
            Vector3 posAndRot = _positionAndRotation[i];
            GameObject spawnedObject = Instantiate(_prefab, new Vector3(posAndRot.x, posAndRot.y, i * 2), Quaternion.identity);
            spawnedObject.transform.eulerAngles = new Vector3(0, 0, posAndRot.z);
            CardView spawnedCard = spawnedObject.GetComponent<CardView>();
            spawnedCard.Init(i);
            _cardView.Add(spawnedCard);
        }

        CardView.CardDied += RepositionCards;
    }

    public void RandomCharacteristic()
    {
        if (IsGameOver())
        {
            Debug.LogError("Game over!");
            return;
        }

        CardView currentCard = _cardView[_count];

        int characteristicChoice = Random.Range(1, 4);
        int randomValue = Random.Range(-2, 10);

        switch (characteristicChoice)
        {
            case 1:
                currentCard.SetDamage(randomValue);
                break;
            case 2:
                currentCard.SetHealth(randomValue);
                break;
            case 3:
                currentCard.SetMana(randomValue);
                break;
        }

        PickNewCard();
    }

    private void PickNewCard()
    {
        _count++;
        if (_count == NUMBER_OF_CARDS)
        {
            _count = 0;
        }
        CheckCard();
    }

    private void CheckCard()
    {
        if (_cardView[_count].IsDestroyed)
        {
            if (!IsGameOver())
            {
                PickNewCard();
                return;
            }

            Debug.LogError("Game Over");
        }
    }

    private void RepositionCards()
    {
        List<CardView> aliveCards = _cardView.Where(x => !x.IsDestroyed).ToList();

        foreach (var alive in aliveCards)
        {
            (Vector3, Vector3) placeAndAngle = GetBestPlaceForMe(alive.transform.position, alive.transform.eulerAngles);
            alive.transform.position = placeAndAngle.Item1;
            alive.transform.eulerAngles = placeAndAngle.Item2;
        }
    }


    private (Vector3, Vector3) GetBestPlaceForMe(Vector3 myPosition, Vector3 myRotation)
    {
        List<CardView> destroyedCards = _cardView.Where(x => x.IsDestroyed).ToList();

        List<CardView> freeCardPlaces = new List<CardView>();
        foreach (var destroyedCard in destroyedCards)
        {
            if (ThisPlaceTaken(destroyedCard.transform.position))
            {
                continue;
            }
            freeCardPlaces.Add(destroyedCard);
        }
        Vector3 centerPos = _positionAndRotation[_positionAndRotation.Length / 2];

        Vector3 bestPos = myPosition;
        Vector3 bestRot = myRotation;

        foreach (var freeCardPlace in freeCardPlaces)
        {
            if (Vector3.Distance(myPosition, centerPos) < Vector3.Distance(freeCardPlace.transform.position, centerPos))
            {
                continue;
            }

            bestPos = freeCardPlace.transform.position;
            bestRot = freeCardPlace.transform.eulerAngles;

            return (bestPos, bestRot);

        }

        return (bestPos, bestRot);
    }

    private bool ThisPlaceTaken(Vector3 position)
    {
        List<CardView> aliveCards = _cardView.Where(x => !x.IsDestroyed).ToList();
        foreach (var aliveCardInner in aliveCards)
        {
            if (IsSameVector(aliveCardInner.transform.position, position))
            {
                return true;
            }
        }

        return false;
    }
    private bool IsSameVector(Vector3 a, Vector3 b)
    {
        if (Mathf.Abs(a.x - b.x) < 0.1f && Mathf.Abs(a.y - b.y) < 0.1f)
        {
            return true;
        }

        return false;
    }


    private bool IsGameOver()
    {
        foreach (var card in _cardView)
        {
            if (!card.IsDestroyed)
            {
                return false;
            }
        }
        return true;
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
