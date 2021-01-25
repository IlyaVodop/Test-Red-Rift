using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;


public class CardView : MonoBehaviour
{
    public TextMesh _cardName;
    public TextMesh _mpCounter;
    public TextMesh _hpCounter;
    public TextMesh _damage;

    public SpriteRenderer _cardSprite;


    public void Init()
    {
        int mpRan = Random.Range(1, 10);
        int hpRan = Random.Range(1, 10);
        int dmgRan = Random.Range(1, 10);

        _mpCounter.text = mpRan.ToString();
        _hpCounter.text = hpRan.ToString();
        _damage.text = dmgRan.ToString();


        StartCoroutine(GetTexture());
    }



    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://picsum.photos/190/180");
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
