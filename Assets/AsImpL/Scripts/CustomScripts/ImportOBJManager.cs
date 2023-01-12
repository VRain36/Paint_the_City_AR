using UnityEngine;
// using CustomScripts;
using System.Collections;
using ArtItem;
using LoadDB;
using AsImpL;
using System.Text;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace AsImpL
{
    namespace Scripts
    {
        /// <summary>
        /// Demonstrate how to load a model with ObjectImporter.
        /// </summary>
        public class ImportOBJManager : MonoBehaviour
        {
            [SerializeField]
            protected string filePath = "";

            [SerializeField]
            protected string objectName = "";

            [SerializeField]
            protected ImportOptions importOptions = new ImportOptions();

            [SerializeField]
            protected PathSettings pathSettings;

            protected ObjectImporter objImporter;

            public Coroutine coroutine = null;

            public int artNum;
            public int getArtNum;

            public GameObject obj = null;

            public GameObject objPanel;
            public static bool artLoadStart = false;
            public bool test;

            
            private void Awake()
            {
                objImporter = gameObject.GetComponent<ObjectImporter>();
                if (objImporter == null)
                {
                    objImporter = gameObject.AddComponent<ObjectImporter>();
                }
            }

            protected virtual void Start()
            {
                artNum = -1;
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
                getArtNum = ArtItem.ArtImgManager.artItemNum; 

                if (objectName != "" && GameObject.Find(objectName) == true) // 스크롤바 적용하도록, 패널 하위에 작품 넣기 
                {
                    GameObject child = GameObject.Find(objectName);
                    child.transform.SetParent(objPanel.transform);
                    artLoadStart = false; // 찾았으니까 false
                    test = artLoadStart;
                } 

                if (artNum != getArtNum) // 이전과 다른 작품 요구 
                {
                    filePath = FindPath(getArtNum);
                    Debug.Log("Start The Coroutine (check UserID Value O/X).");

                    if (artNum == -1) // obj가 로드되는 시간이 다르므로, obj의 null 여부로 if문 확인하는 것은 맞지 않음
                    {
                        artLoadStart = true;
                        test = artLoadStart;

                        objectName = "User Artwork " + getArtNum.ToString(); // User Artwork도 번호 붙이기
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                    }
                    else// if (artNum >= 0) // 기존에 작품이 있는 경우 
                    {
                        artLoadStart = true;
                        test = artLoadStart; 

                        float delay = 0f;

                        // objectName 을 아직 업데이트하지 않았으므로, 이 이름으로 기존 에셋 찾기 
                        while (GameObject.Find(objectName) == null) // 작품 에셋 찾을 때까지 (작품이 아직 로드되지 않은 경우, 로딩 기다리기)
                        {
                            delay += Time.deltaTime;
                        }
                        
                        obj = GameObject.Find(objectName);
                        Destroy(obj);  // 삭제

                        objectName = "User Artwork " + getArtNum.ToString(); // User Artwork도 번호 붙이기 // 새 이름으로 생성 
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions); // 작품 로드까지 기다린 후, 삭제 + 다시 생성
                    }

                    artNum = getArtNum;     
                }
            }

            public string FindPath(int index)
            {
                string path = "";
                for (int i=0;i<DatabaseManager.artInfo.Length;i++)
                {
                    if (index == DatabaseManager.artInfo[i].getArtworkID())
                    {
                        path = DatabaseManager.artInfo[index].getArtworkUrl();
                        break;
                        
                    }
                }
                return path;
            }
            
        }
    }
}
