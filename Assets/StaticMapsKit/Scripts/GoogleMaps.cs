using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
/// <summary>
/// Static Maps from Google Maps
/// For the full references please visit https://developers.google.com/maps/documentation/staticmaps/intro
/// </summary>
public class GoogleMaps : MonoBehaviour
{
    private string url = "https://maps.googleapis.com/maps/api/staticmap?";

    // to get your api key visit https://console.developers.google.com/flows/enableapi?apiid=static_maps_backend&keyType=SERVER_SIDE
    public string ApiKey = "AIzaSyBZEd1t4Mg3ZovHw60Jc14fNsL3hb4drIw";
    public bool loadOnStart = true;
    public bool autoAdjustCenter = true;
    public GoogleMapLocation centerLocation;
    //supports value between 0 and 22
    public int zoom = 3;
    public GoogleMapType mapType;
    //Max value is 640 (without api key)
    public int height = 640;
    //Max value is 640 (without api key)
    public int width = 640;
    public bool doubleResolution = false;
    public GoogleMapMarker[] markers;
    public GoogleMapPath[] paths;
    public ImageFormat format = ImageFormat.png;
    public RawImage image;
    void Start()
    {
        //if (loadOnStart)
        //{
            //GetMapTexture();
        //}
    }

    public void GetMapTexture()
    {
        try
        {
            if (autoAdjustCenter && (markers.Length == 0 && paths.Length == 0))
            {
                Debug.LogError("Adjusting center of the map works only with markers or paths");
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
        Debug.Log(querryString);
        var request = new WWW(url + querryString);
        Debug.Log(request.url.ToString());
        yield return request;

        //GetComponent<Renderer>().material.mainTexture = request.texture;
        image.texture = request.texture;
    }

    //Creates querry for loading map image
    private string CreateQuerryString()
    {
        var querry = "";
        if (!autoAdjustCenter)
        {
            querry = AddCenterToUrl(querry);
        }
        querry += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{1}", height, width));
        querry += "&scale=" + (doubleResolution ? "2" : "1");
        querry += "&maptype=" + mapType.ToString().ToLower();

        if (!string.IsNullOrEmpty(ApiKey))
        {
            querry += "&key=" + ApiKey;
        }

        querry += "&format=" + format.ToString();

#if UNITY_IPHONE
        querry = AddSensorToUrl(querry);
#endif

        querry = AddMarkersToUrl(querry);

        querry = AddPathToUrl(querry);
        return querry;
    }

    //Converts and sets sensor to the url
    private static string AddSensorToUrl(string querry)
    {
        var usingSensor = false;

        usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;

        querry += "&sensor=" + (usingSensor ? "true" : "false");
        return querry;
    }

    //Converts and sets center of the map
    private string AddCenterToUrl(string qs)
    {
        if (centerLocation.Address != "")
            qs += "center=" + WWW.UnEscapeURL(centerLocation.Address);
        else
        {
            qs += "center=" + WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.Latitude, centerLocation.Longitude));
        }

        qs += "&zoom=" + zoom.ToString();
        return qs;
    }

    ///Converts markers to url value if any added
    private string AddMarkersToUrl(string querry)
    {
        foreach (var marker in markers)
        {
            //querry += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}|{3}", marker.Size.ToString().ToLower(), marker.Color, marker.Label, marker.Locations[0].Latitude + "," + marker.Locations[0].Longitude);
            querry += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", marker.Size.ToString().ToLower(), marker.Color, marker.Label);
            querry = AddLocationToUrl(querry, marker.Locations);
        }
        Debug.Log(querry);
        return querry;
    }

    ///Converts path to url value if any added
    private string AddPathToUrl(string querry)
    {
        foreach (var path in paths)
        {
            querry += "&path=" + string.Format("weight:{0}|color:{1}", path.Weight, path.Color);

            if (path.Fill)
            {
                querry += "|fillcolor:" + path.FillColor;
            }

            querry = AddLocationToUrl(querry, path.Locations);
        }
        return querry;
    }

    ///Converts location to url 
    private static string AddLocationToUrl(string querry, GoogleMapLocation[] googleMapLocation)
    {
        foreach (var location in googleMapLocation)
        {
            if (location.Address != "")
            {
                querry += "|" + WWW.UnEscapeURL(location.Address);
            }
            else
            {
                querry += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", location.Latitude, location.Longitude));
            }
        }
        return querry;
    }
}

public enum ImageFormat
{
    png,
    jpg,
}

public enum GoogleMapColor
{
    black,
    brown,
    green,
    purple,
    yellow,
    blue,
    gray,
    orange,
    red,
    white
}

public enum GoogleMapType
{
    RoadMap,
    Satellite,
    Terrain,
    Hybrid
}

[System.Serializable]
public class GoogleMapLocation
{
    public string Address;
    public float Latitude;
    public float Longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
    public enum GoogleMapMarkerSize
    {
        Tiny,
        Small,
        Mid
    }
    public GoogleMapMarkerSize Size;
    public GoogleMapColor Color;
    public string Label;
    public GoogleMapLocation[] Locations;

}

[System.Serializable]
public class GoogleMapPath
{
    public int Weight = 5;
    public GoogleMapColor Color;
    public bool Fill = false;
    public GoogleMapColor FillColor;
    public GoogleMapLocation[] Locations;
}