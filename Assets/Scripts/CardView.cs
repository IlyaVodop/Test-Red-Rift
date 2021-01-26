using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


public class CardView : MonoBehaviour
{

    public static event Action CardDied;
    public TextMesh _cardName;
    [SerializeField]
    public TextMesh _mpCounter;
    [SerializeField]
    public TextMesh _hpCounter;
    [SerializeField]
    public TextMesh _damage;

    public SpriteRenderer _cardSprite;
    private const string RANDOM_IMAGE_URL = "https://picsum.photos/190/180";

    public bool IsDestroyed { get; private set; }
    public void Init(int cardNumber)
    {
        int startMP = Random.Range(1, 10);
        int startHP = Random.Range(1, 10);
        int startDamage = Random.Range(1, 10);

        _mpCounter.text = startMP.ToString();
        _hpCounter.text = startHP.ToString();
        _damage.text = startDamage.ToString();
        _cardName.text = "CARD № " + (cardNumber + 1);

        StartCoroutine(GetTexture());
    }

    #region Setters

    public void SetHealth(int health)
    {
        StartCoroutine(AnimatedCount(GetHealth(), health, _hpCounter, CheckHealth));
    }
    public void SetDamage(int damage)
    {
        StartCoroutine(AnimatedCount(GetDamage(), damage, _damage, null));
    }
    public void SetMana(int mana)
    {
        StartCoroutine(AnimatedCount(GetMana(), mana, _mpCounter, null));
    }


    #endregion

    #region Getters

    public int GetHealth()
    {
        int.TryParse(_hpCounter.text, out int result);
        return result;
    }
    public int GetMana()
    {
        int.TryParse(_mpCounter.text, out int result);
        return result;
    }
    public int GetDamage()
    {
        int.TryParse(_damage.text, out int result);
        return result;
    }

    #endregion


    void CheckHealth()
    {
        if (GetHealth() < 1)
        {
            gameObject.SetActive(false);
            IsDestroyed = true;
            CardDied?.Invoke();
        }
    }



    IEnumerator AnimatedCount(int startValue, int endValue, TextMesh text, Action callback)
    {
        if (startValue == endValue)
        {
            yield break;
        }

        int count = Mathf.Abs(startValue - endValue);

        for (int i = 0; i < count; i++)
        {
            float forLerp = ((float)(i + 1) / count);
            float result = Mathf.Lerp((float)startValue, (float)endValue, forLerp);
            text.text = ((int)result).ToString();
            yield return null;
        }
        callback?.Invoke();

    }




    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(RANDOM_IMAGE_URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _cardSprite.sprite = Sprite.Create((Texture2D)myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }

}
