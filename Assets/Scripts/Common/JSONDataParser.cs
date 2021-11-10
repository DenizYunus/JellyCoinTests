using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONDataParser
{
    //HERE IS LEVEL DETAILS PARSER
    [System.Serializable]
    public class ParsedJSONClass
    {
        public LevelData[] levels;

        public static ParsedJSONClass CreateFromJSON(string JsonString)
        {
            return JsonUtility.FromJson<ParsedJSONClass>(JsonString);
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public string levelname, levelbitmap, levelimage;
        public MapDetails mapDetails;
    }

    [System.Serializable]
    public class MapDetails
    {
        public ObjectTransform[] players, obstacles;
    }

    [System.Serializable]
    public class ObjectTransform
    {
        public int x, y, z, rx, ry, rz;
    }




    //HERE IS NOTIFICATIONS PARSER
    [System.Serializable]
    public class AnnouncementParser
    {
        public AnnouncementDetails[] announcements;

        public static AnnouncementParser CreateFromJSON(string JsonString)
        {
            return JsonUtility.FromJson<AnnouncementParser>(JsonString);
        }
    }

    [System.Serializable]
    public class AnnouncementDetails
    {
        public string announcement;
        public string date;
    }


}