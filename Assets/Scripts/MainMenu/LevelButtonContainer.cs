using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelButtonContainer : MonoBehaviour
{
    public string levelname;
    public Image bitmapImage;
    public Text coinCount;

    public IEnumerator UpdateContainer(string _levelname, string _bitmapLink, string _imageLink)
    {
        levelname = _levelname;
        yield return StartCoroutine(setImage(_bitmapLink, (result =>
        {
            Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
            bitmapImage.overrideSprite = sp;
        } )));

        gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            StartCoroutine(setImage(_imageLink, (result =>
            {
                Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                FindObjectOfType<HardTransforms>().selectedLevelImage.overrideSprite = sp;
            }
            )));
        });
    }

    IEnumerator setImage(string url, System.Action<Texture2D> callback)
    {
        Texture2D texture = new Texture2D(256, 256);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

        if (callback != null) callback(texture);
    }

}