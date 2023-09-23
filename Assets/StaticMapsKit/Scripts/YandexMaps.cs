using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Static Maps from Yandex
/// For the full references please visit https://tech.yandex.com/maps/doc/staticapi/1.x/dg/concepts/input_params-docpage/
/// </summary>
public class YandexMaps : MonoBehaviour
{
    private string url = "http://static-maps.yandex.ru/1.x/?";
    public bool loadOnStart = true;
    /// <summary>
    /// Supports values from 0 to 17
    /// </summary>
    public int zoom = 17;
    public YandexMapLayers mapLayer;
    public YandexMapLanguage language = YandexMapLanguage.English;
    public YandexMapMarker[] markers;
    //Maximum 600
    public int height = 600;
    //Maximum 450
    public int width = 450;
    public float latitude = 37.620070f;
    public float longitude = 55.753630f;
    public bool autoAdjustCenter;
    public bool showTraffic;

    void Start()
    {
        if (loadOnStart)
        {
            GetMapTexture();
        }
    }

    public void GetMapTexture()
    {
        try
        {
            if (autoAdjustCenter && markers.Length == 0)
            {
                Debug.LogError("Adjusting center of the map works only with markers");
            }
            StartCoroutine(LoadImage());
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    //Loads specified map image and sets it as this game object texture
    IEnumerator LoadImage()
    {
        var querryString = CreateQuerryString();
        Debug.Log(url + querryString);
        var request = new WWW(url + querryString);
        yield return request;
        GetComponent<Renderer>().material.mainTexture = request.texture;
    }

    //Creates querry for loading map image
    private string CreateQuerryString()
    {
        var querry = "";

        querry += "lang=" + ConvertLanguage(language);

        if (!autoAdjustCenter)
        {
            querry = AddCenterToUrl(querry);
        }

        querry += "&size=" + WWW.UnEscapeURL(string.Format("{0},{1}", height, width));

        querry += "&l=" + ConvertMapLayer(mapLayer);

        if (showTraffic)
            querry += ",trf";

        querry = AddMarkersToUrl(querry);

        return querry;
    }

    //Converts and sets center of the map
    private string AddCenterToUrl(string querry)
    {
        querry += "&ll=" + WWW.UnEscapeURL(string.Format("{0},{1}", latitude, longitude));
        querry += "&z=" + zoom.ToString();
        return querry;
    }

    ///Converts markers to url value if any added
    private string AddMarkersToUrl(string querry)
    {
        for (var i = 0; i < markers.Length; i++)
        {
            if (i == 0)
            {
                querry += "&pt=" + string.Format("{0},{1},{2}{3}{4}{5}", markers[i].Latitude, markers[i].Longitude,
                    markers[i].Type, ConvertColor(markers[i].Color), ConvertSize(markers[i].Size), CheckLabelValue(markers[i].Label));
            }
            else
            {
                querry += "~" + string.Format("{0},{1},{2}{3}{4}{5}", markers[i].Latitude, markers[i].Longitude,
                    markers[i].Type, ConvertColor(markers[i].Color), ConvertSize(markers[i].Size), CheckLabelValue(markers[i].Label));
            }
        }

        return querry;
    }

    ///Converts Map Layer to url value
    private string ConvertMapLayer(YandexMapLayers mapLayer)
    {
        switch (mapLayer)
        {
            case YandexMapLayers.Roadmap:
                return "map";
            case YandexMapLayers.Satellite:
                return "sat";
            case YandexMapLayers.Hybrid:
                return "sat,skl";
            default:
                return "map";
        }
    }

    ///Converts Language to url value
    private string ConvertLanguage(YandexMapLanguage language)
    {
        switch (language)
        {
            case YandexMapLanguage.English:
                return "en_US";
            case YandexMapLanguage.Russian:
                return "ru_RU";
            case YandexMapLanguage.Turkish:
                return "tr_TR";
            default:
                return "en_US";
        }
    }

    //Check label for correct value
    private int CheckLabelValue(int label)
    {
        if (label < 0 && label > 99)
        {
            Debug.LogError("Marker label supports only values between 0 and 99");
            label = 0;
        }

        return label;
    }

    ///Converts Color to url value
    private string ConvertColor(YandexMapMarker.MarkerColor markerColor)
    {
        switch (markerColor)
        {
            case YandexMapMarker.MarkerColor.White:
                return "wt";
            case YandexMapMarker.MarkerColor.Yellow:
                return "yw";
            case YandexMapMarker.MarkerColor.Violet:
                return "vv";
            case YandexMapMarker.MarkerColor.Red:
                return "rd";
            case YandexMapMarker.MarkerColor.Pink:
                return "pn";
            case YandexMapMarker.MarkerColor.Orange:
                return "or";
            case YandexMapMarker.MarkerColor.Night:
                return "nt";
            case YandexMapMarker.MarkerColor.LightBlue:
                return "lb";
            case YandexMapMarker.MarkerColor.Grey:
                return "gr";
            case YandexMapMarker.MarkerColor.Green:
                return "gn";
            case YandexMapMarker.MarkerColor.DarkOrange:
                return "do";
            case YandexMapMarker.MarkerColor.DarkBlue:
                return "db";
            case YandexMapMarker.MarkerColor.Blue:
                return "bl";
            default:
                return "wt";
        }
    }

    ///Converts Size to url value
    private string ConvertSize(YandexMapMarker.MarkerSize markerSize)
    {
        switch (markerSize)
        {
            case YandexMapMarker.MarkerSize.Small:
                return "s";
            case YandexMapMarker.MarkerSize.Medium:
                return "m";
            case YandexMapMarker.MarkerSize.Large:
                return "l";
            default:
                return "s";
        }
    }
}

public enum YandexMapLanguage
{
    Russian,
    English,
    Turkish
}

[System.Serializable]
public enum YandexMapLayers
{
    Roadmap,
    Satellite,
    Hybrid
}

[System.Serializable]
public class YandexMapMarker
{
    public enum MarkerColor
    {
        White,
        DarkOrange,
        DarkBlue,
        Blue,
        Green,
        Grey,
        LightBlue,
        Night,
        Orange,
        Pink,
        Red,
        Violet,
        Yellow
    }

    public enum MarkerType
    {
        pm,
        pm2,
        flag,
        vk
    }

    public enum MarkerSize
    {
        Small,
        Medium,
        Large
    }

    public MarkerSize Size = MarkerSize.Small;
    public MarkerColor Color = MarkerColor.White;
    public MarkerType Type = MarkerType.pm;
    //Value between 0 and 99
    public int Label = 1;
    public float Latitude = 37.620070f;
    public float Longitude = 55.753630f;
}