using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GPSCore : MonoBehaviour

{

    public float[] Lat;
    public float[] Lon;
    internal int PointCounter = 0; // the amount of points that we want to check in GPS process
    private double distance;
    private Vector3 TargetPosition;
    private Vector3 OriginalPosition;
    public float Radius = 5f; // Range of Target function start
    public float TimeUpdate = 3f; // Time of compare current coordinates with target per second
    public GameObject[] PointObjects; // The list of objects for every points that we want to show
    public GameObject[] TargetPopUp; // the popup UI pages when the user reach the target position
    public bool TargetPupUpOneTime = false; // for check the popup appear one time not everytime when we are in the range of target
    public UnityEvent EventStartGPS; // This event will work when the GPS system start to work
    public UnityEvent EventReachGPSPoint; // This event will work when player reached the GPS point
    public UnityEvent EventOutGPSPointRange; // This event will work when player is out of the GPS point range
    public GameObject NoGPSPopUp; // the popup UI page when the user's device location is off
    public bool NoGPSPupUpOneTime = false; // for check the popup appear one time not everytime

    // 위도, 경도 확인 용도로 추가한 코드
    public float current_lat, current_lon, target_lat, target_lon;
    public Text text_current_lat, text_current_lon, text_target_lat, text_target_lon, text_distance;

    private void Start()
    {
        // Call the GPS connection in native and try to connect to the satelite

        Input.location.Start();

        StartCoroutine("GPSProcess");

        if (EventStartGPS != null)
        {
            EventStartGPS.Invoke();
        }
    }

    public IEnumerator GPSProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeUpdate);

            // 목표 위도, 경도 확인 용도로 추가한 코드
            text_target_lat.text = Lat[PointCounter].ToString();
            text_target_lon.text = Lon[PointCounter].ToString();
            target_lat = float.Parse(Lat[PointCounter].ToString());
            target_lon = float.Parse(Lon[PointCounter].ToString());

            /*
            // 무조건 실행되도록 변수 값 설정 (테스트 용도)
            float a = float.Parse(Lat[PointCounter].ToString());
            float b = float.Parse(Lon[PointCounter].ToString());
            float c = (float)50;
            float d = (float)100;
            text_current_lat.text = c.ToString();
            text_current_lon.text = d.ToString();
            Calc(a, b, c, d);
            */
            
            if (Input.location.isEnabledByUser == true)
            {
                Input.location.Start();

                LocationInfo input = Input.location.lastData;

                text_current_lat.text = input.latitude.ToString();
                text_current_lon.text = input.longitude.ToString();

                current_lat = float.Parse(input.latitude.ToString());
                current_lon = float.Parse(input.longitude.ToString());

                Calc(target_lat, target_lon, current_lat, current_lon);
            }
            
            if (Input.location.isEnabledByUser == false && NoGPSPupUpOneTime == false)
            {
                NoGPSPopUp.SetActive(true);

                TargetPupUpOneTime = true;

                // 위도, 경도 확인 용도로 추가한 코드
                text_current_lat.text = "GPS off";
                text_current_lon.text = "GPS off";
                text_distance.text = "GPS off";
            }
        }
    }

    public void Calc(float lat1, float lon1, float lat2, float lon2)
    {
        var R = 6378.137; // Radius of earth in KM

        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;

        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +

            Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *

            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        distance = R * c;

        // distance = distance * 1000f;

        distance = distance * 1f;

        // 확인 용도 코드
        text_distance.text = distance.ToString();

        if (distance < Radius)
        {
            if (TargetPupUpOneTime == false)
            {
                for (int i = 0; i < TargetPopUp.Length; i++)
                {
                    TargetPopUp[i].SetActive(false);

                    PointObjects[i].SetActive(false);
                }

                TargetPopUp[PointCounter].SetActive(true);

                PointObjects[PointCounter].SetActive(true);

                if (EventReachGPSPoint != null)
                {
                    EventReachGPSPoint.Invoke();
                }
            }
        }

        if (distance > Radius)
        {
            for (int i = 0; i < TargetPopUp.Length; i++)
            {
                TargetPopUp[i].SetActive(false);

                PointObjects[i].SetActive(false);
            }

            PointCounter++;

            if (PointCounter == Lat.Length)
            {
                PointCounter = 0;
            }

            if (EventOutGPSPointRange != null)
            {
                EventOutGPSPointRange.Invoke();
            }
        }
    }

    public void HideTargetPopUp()   
    {
        TargetPopUp[PointCounter].SetActive(false);

        PointObjects[PointCounter].SetActive(false);

        TargetPupUpOneTime = true;
    }

    public void HideNoGPSPopUp()
    {
        NoGPSPopUp.SetActive(false);
    }
}