using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnnouncementContainer : MonoBehaviour
{
    public TMP_Text notificationText;
    public TMP_Text notificationDate;

    public void UpdateAnnouncement(string _notif, string _date)
    {
        notificationText.text = _notif;
        notificationDate.text = _date;
    }
}
