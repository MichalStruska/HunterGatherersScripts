using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;

public class NightLightManager : MonoBehaviour
{

    public Camera activeCamera;
    public GameObject DayNight;
    public GameObject Clouds;
    private Light NightLight;

    void Start()
    {
        NightLight = gameObject.GetComponent<Light>();
    }

    void Update()
    {

        switch (DayNight.GetComponent<WeatherMakerDayNightCycleManagerScript>().TimeOfDayCategory.ToString())
        {
            case "Day":
            case "Dawn, Day":
                NightLight.enabled = false;
                break;
            case "Day, Dusk":
                NightLight.enabled = true;
                NightLight.intensity = 4;
                MoveNightLight();
                break;
            default:
                NightLight.enabled = true;
                NightLight.intensity = 6;
                MoveNightLight();
                break;
        }

        if (Clouds.GetComponent<WeatherMakerFullScreenCloudsScript>().CloudProfile.CloudCoverTotal > 0.75f)
        {
            NightLight.enabled = true;
            NightLight.intensity = 4;
            MoveNightLight();
        }

    }

    private void MoveNightLight()
    {
        activeCamera = GetComponentInParent<CameraManager>().mainCamera;
        transform.position = new Vector3(activeCamera.transform.position.x, 300, activeCamera.transform.position.z);
    }
}
