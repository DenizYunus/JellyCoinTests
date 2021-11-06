using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeGalleryNamespace;
using System.IO;

public class SetImageToPlane : MonoBehaviour
{
    public GameObject plane;

    void Start()
    {
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.ShouldAsk || NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.Denied)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Read);
        }

        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.Granted)
            NativeGallery.GetImageFromGallery(MediaPicked, "", "image/*");
    }

    void Update()
    {
        
    }

    public void MediaPicked(string path)
    {
        Debug.Log("Media Picked Entered.");
        byte[] byteArray = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(1024, 1024);
        texture.LoadImage(byteArray);
        Debug.Log("Texture created.");
        //Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);

        Material material = new Material(plane.GetComponent<MeshRenderer>().material);
        material.mainTexture = texture;

        plane.GetComponent<MeshRenderer>().material = material;
    }
}