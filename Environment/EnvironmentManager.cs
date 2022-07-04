using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject Clouds;
    public GameObject Sun;
    public float sunElevation;

    void Start()
    {

    }

    void Update()
    {
        //if (Clouds.GetComponent<WeatherMakerFullScreenCloudsScript>().CloudProfile.CloudCoverTotal > 0.5f)
        //{

        //}

        Vector3 SunDirection = Vector3.zero - new Vector3(Sun.transform.position.x, Sun.transform.position.y, Sun.transform.position.z);
        Vector3 SunDirectionHorizontal = Vector3.zero - new Vector3(Sun.transform.position.x, 0, Sun.transform.position.z);
        sunElevation = AngleBetween(SunDirection, SunDirectionHorizontal);

    }

    float AngleBetween(Vector3 vectorA, Vector3 vectorB)
    {
        float angle = Vector3.Angle(vectorA, vectorB);
        if (Sun.transform.position.y > 0)
        {
            angle = angle;
        }
        else
        {
            angle = 0f;
        }
        return angle;
    }

}
