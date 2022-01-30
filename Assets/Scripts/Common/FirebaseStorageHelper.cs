using UnityEngine;

public static class FirebaseStorageHelper
{
    public static string ParseGSLink(string _gsLink)
    {
        //template = gs://jelly-coins.appspot.com/prev4_orig.png

        int bucketLinkFrom = _gsLink.IndexOf("gs://") + "gs://".Length;
        int bucketLinkTo = _gsLink.LastIndexOf(".appspot.com");

        int imageLinkFrom = _gsLink.IndexOf(".com/") + ".com/".Length;
        int imageLinkTo = _gsLink.Length;

        string origString = "https://firebasestorage.googleapis.com/v0/b/{0}.appspot.com/o/{1}?alt=media";

        string finalString = string.Format(origString, _gsLink.Substring(bucketLinkFrom, bucketLinkTo - bucketLinkFrom), _gsLink.Substring(imageLinkFrom, imageLinkTo - imageLinkFrom));
        Debug.Log(finalString);
        return finalString;
    }
}
