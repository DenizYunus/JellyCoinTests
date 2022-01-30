using System.Collections;
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
        yield return StartCoroutine(
        setImage(_bitmapLink, (result =>
        {
            Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
            bitmapImage.overrideSprite = sp;
        })));

        gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            StartCoroutine(
            setImage(_imageLink, (result =>
                {
                    Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                    FindObjectOfType<HardTransforms>().selectedLevelImage.overrideSprite = sp;
                }
                )));
        });
    }

    IEnumerator setImage(string url, System.Action<Texture2D> callback)
    {
        //Texture2D texture = new Texture2D(256, 256);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(FirebaseStorageHelper.ParseGSLink(url));
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (callback != null) callback(texture);
        }
    }

    //    void setImage(string _imageLink, System.Action<Texture2D> callback)
    //    {
    //        StorageReference gsReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(_imageLink);

    //        gsReference.GetDownloadUrlAsync().ContinueWithOnMainThread(async task =>
    //                {
    //                    if (!task.IsFaulted && !task.IsCanceled)
    //                    {
    //                        Debug.Log(task.Result);
    //                        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(task.Result))
    //                        {
    //                            var asyncOp = www.SendWebRequest();

    //                        // await until it's done: 
    //                        while (asyncOp.isDone == false)
    //                                await Task.Delay(1000 / 30);//30 hertz

    //                        if (www.result != UnityWebRequest.Result.Success)// for Unity >= 2020.1
    //                        {
    //#if DEBUG
    //                            Debug.Log($"{www.error}, URL:{www.url}");
    //#endif
    //                        }
    //                            else
    //                            {
    //                                // return valid results:
    //                                Texture2D texture = DownloadHandlerTexture.GetContent(www);

    //                                if (callback != null) callback(texture);
    //                            }
    //                        }
    //                    }
    //                });
    //        }
}



/*using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelButtonContainer : MonoBehaviour
{
    public string levelname;
    public Image bitmapImage;
    public Text coinCount;

    public void UpdateContainer(string _levelname, string _bitmapLink, string _imageLink)
    {
        levelname = _levelname;

        /*
        yield return StartCoroutine(setImage(_bitmapLink, (result =>
        {
            Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
            bitmapImage.overrideSprite = sp;
        } )));* /

        StorageReference gsReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(_bitmapLink);

        gsReference.GetDownloadUrlAsync().ContinueWithOnMainThread(async task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(task.Result))
                {
                    var asyncOp = www.SendWebRequest();

                    // await until it's done: 
                    while (asyncOp.isDone == false)
                        await Task.Delay(1000 / 30);//30 hertz

                    if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
                    {
#if DEBUG
                        Debug.Log($"{www.error}, URL:{www.url}");
#endif
                    }
                    else
                    {
                        // return valid results:
                        Texture2D texture = DownloadHandlerTexture.GetContent(www);

                        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        bitmapImage.overrideSprite = sp;
                    }
                }
/*
                    Texture2D texture = new Texture2D(256, 256);

                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(task.Result);
                    yield return request.SendWebRequest();
                    if (request.isNetworkError || request.isHttpError)
                        Debug.Log(request.error);
                    else
                    {
                        texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    }

                    Sprite sp = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                    bitmapImage.overrideSprite = sp;
                }* /

            }
        });

        gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            StorageReference gsReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(_imageLink);

            gsReference.GetDownloadUrlAsync().ContinueWithOnMainThread(async task =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(task.Result))
                    {
                        var asyncOp = www.SendWebRequest();

                        // await until it's done: 
                        while (asyncOp.isDone == false)
                            await Task.Delay(1000 / 30);//30 hertz

                        if (www.result != UnityWebRequest.Result.Success)// for Unity >= 2020.1
                        {
#if DEBUG
                            Debug.Log($"{www.error}, URL:{www.url}");
#endif
                        }
                        else
                        {
                            // return valid results:
                            Texture2D texture = DownloadHandlerTexture.GetContent(www);

                            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                            FindObjectOfType<HardTransforms>().selectedLevelImage.overrideSprite = sp;
                        }
                    }
                }
            });
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
}*/