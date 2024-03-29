using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using AsImpL;

namespace CustomScripts
{
    /// <summary>
    /// Demonstrate how to load a model with ObjectImporter.
    /// </summary>
    public class ImportOBJManager : MonoBehaviour
    {
        [SerializeField]
        protected string filePath = "";

        [SerializeField]
        protected string objectName = "User Artwork";

        [SerializeField]
        protected ImportOptions importOptions = new ImportOptions();

        [SerializeField]
        protected PathSettings pathSettings;

        protected ObjectImporter objImporter;

        public GameObject objPanel;
        public GameObject locationPanel;
        public GameObject displayPanel;

        public int artNum;
        public int getArtNum;

        public static bool artLoadStart = false;
        
        private void Awake()
        {
            artNum = -1;

            objImporter = gameObject.GetComponent<ObjectImporter>();
            if (objImporter == null)
            {
                objImporter = gameObject.AddComponent<ObjectImporter>();
            }
        }

        private void OnValidate()
        {
            if(pathSettings==null)
            {
                pathSettings = PathSettings.FindPathComponent(gameObject);
            }
        }

        private void Update()
        {
            /// <summary>
            /// 작품 로드 완료되었다면, 슬라이더로 작품 크기 조절할 수 있도록 objPanel 아래에 넣기 
            /// </summary> 

            getArtNum = ArtImgManager.artItemNum; 

            // 작품 로드 완료 여부 확인 
            if (objectName != "" && GameObject.Find(objectName) == true) 
            {
                GameObject child = GameObject.Find(objectName);
                child.transform.SetParent(objPanel.transform);
                artLoadStart = false; 
            } 

            // * test
            GPSManager.user_lat = "1";
            GPSManager.user_lon = "1";
            GPSManager.target_lat = "1";
            GPSManager.target_lon = "1";
            // */

            // 작품 로드 시작 
            if (artNum != getArtNum) 
            { 
                // 해당 위치에 도착했는지 확인
                if ((GPSManager.user_lat == GPSManager.target_lat) && (GPSManager.user_lon == GPSManager.target_lon))
                {
                    artLoadStart = true;
                    filePath = FindPath(getArtNum);
                    DeleteArtwork();
                    objectName = "User Artwork " + getArtNum.ToString(); 
                    objImporter.ImportModelAsync(objectName, filePath, null, importOptions); 
                    artNum = getArtNum; 
                }
                else
                {
                    locationPanel.SetActive(true);
                    ArtImgManager.artItemNum = -1;
                }
            }
        }

        public void DeleteArtwork()
        {
            GameObject obj = GameObject.Find(objectName);
            Destroy(obj); 
        }

        public string FindPath(int index)
        {
            string path = DatabaseManager.artInfo[index].getArtworkUrl();
            return path;
        }

        public void onLocationOkbuttonClick()
        {
            locationPanel.SetActive(false);
        }

        public void onTargetFound()
        {
            displayPanel.SetActive(true);
        }
    }
}
