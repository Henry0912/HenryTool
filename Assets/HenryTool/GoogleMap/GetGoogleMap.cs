using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGoogleMap : MonoBehaviour
{
    public string url;
    public float lat = 25.028544f;
    public float lon = 121.569712f;
    LocationInfo li;
    public int zoom = 14;
    public int width = 640;
    public int height = 640;
    public int scale;
    public enum MapType { roadmap, satellite, hybrid, terrain }
    public MapType mapType;

    public bool isLoading;
    public IEnumerator mapCoroutine;

    public string googleAPIkey = "AIzaSyA0WEQfyyZZZjrx1yXaxE3sob7edox9y_c";

    // Start is called before the first frame update
    void Start()
    {
        mapCoroutine = GetMap(lat, lon);
        StartCoroutine(mapCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isLoading)
            {
                mapCoroutine = GetMap(lat, lon);
                StartCoroutine(mapCoroutine);
            }
        }

    }

    IEnumerator GetMap(float _lat, float _lon)
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + _lat + "," + _lon
         + "&zoom=" + zoom + "&size=" + width + "x" + height + "&scale=" + scale
             + "&maptype=" + mapType + "&key=" + googleAPIkey;

        Debug.Log(url);
        isLoading = true;

        //Perform the request
        var www = new WWW(url);
        yield return www;

        Debug.Log(www.error);

        gameObject.GetComponent<RawImage>().texture = www.texture;
        StopCoroutine(mapCoroutine);

        isLoading = false;
    }



}
