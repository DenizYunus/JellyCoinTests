using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeGalleryNamespace;
using System.IO;

public class SetImageToPlane : MonoBehaviour
{
    public GameObject plane;

    public void PickImage()
    {
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.ShouldAsk || NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.Denied)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Read);
        }

        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read) == NativeGallery.Permission.Granted)
            NativeGallery.GetImageFromGallery(MediaPicked, "", "image/*");
    }

    public void MediaPicked(string path)
    {
        if (path != null)
        {
            byte[] byteArray = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1024, 1024);
            texture.LoadImage(byteArray);         

            Material material = new Material(plane.GetComponent<MeshRenderer>().material);
            material.mainTexture = texture;

            plane.GetComponent<MeshRenderer>().material = material;

            Timer.StartTimer();
            GlobalTouch.countingTouches = true;
        }
    }
}