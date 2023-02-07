using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CustomScripts
{
    public class DatabaseManager : MonoBehaviour
    {
        public static int nRows;
        public static int publicnRows = 0;
        public static int privatenRows = 0;

        public static ArtInfoData[] artInfo;
        
        public static List<int> publicIndexArray = new List<int>(); 
        public static List<int> privateIndexArray = new List<int>(); 

        public string artlist_API_url = "[API 주소]";

        private void Start()
        {
            StartCoroutine(GetMySQLData());
        }
    
        private IEnumerator GetMySQLData()
        {
            WWWForm form = new WWWForm(); 
            UnityWebRequest webRequest = UnityWebRequest.Post(artlist_API_url, form);
            /// <summary>
            /// |를 기준으로 잘라서 배열 result에 저장
            /// using System.Linq 활용하여 |를 기준으로 저장한 배열의 마지막 값 가져오지 않도록 함 
            /// 긱 요소 (ex. artwork_id, user_id) 별로 >를 기준으로 저장
            /// </summary>

            yield return webRequest.SendWebRequest(); 

            string[] php_result = webRequest.downloadHandler.text.Split(new char[] { '|' });  
            webRequest.Dispose();
            nRows = php_result.Length; 
            artInfo = new ArtInfoData[nRows]; 

            for (int i=0; i<nRows; i++)
            {
                artInfo[i] = new ArtInfoData();
            }

            for (int i = 0; i < php_result.Length ; i++)
            {
                string[] php_result_row = php_result[i].Split(new char[] { '>' });
                artInfo[i].setArtworkID(Int32.Parse(php_result_row[0]));
                artInfo[i].setUserID(php_result_row[1]);
                artInfo[i].setArtImgUrl(php_result_row[2]);
                artInfo[i].setArtName(php_result_row[3]);
                artInfo[i].setArtworkUrl(php_result_row[4] + "artwork.obj");
                artInfo[i].setPublicMode(Int32.Parse(php_result_row[5]));

                if (Int32.Parse(php_result_row[5]) == 1) 
                {
                    // public mode 
                    publicnRows += 1;
                    publicIndexArray.Add(i);
                }
                else
                {
                    // private mode 
                    privatenRows += 1;
                    privateIndexArray.Add(i);
                }
            }

            Debug.Log("[Loading Completed] ArtInfo Data from php");
        }

        public class ArtInfoData
        {
            private int ArtworkID;
            private string UserID;
            private string ArtImgUrl;
            private string ArtName;
            private string ArtworkUrl;
            private int PublicMode;

            public ArtInfoData()
            {
                ArtworkID = -1;
                UserID = "";
                ArtImgUrl = "";
                ArtName = "";
                ArtworkUrl = "";
                PublicMode = -1;
            }

            public void setArtworkID(int id)
            {
                this.ArtworkID = id;
            }

            public void setUserID(string id)
            {
                this.UserID = id;
            }

            public void setArtImgUrl(string url)
            {
                this.ArtImgUrl = url;
            }

            public void setArtName(string name)
            {
                this.ArtName = name;
            }

            public void setArtworkUrl(string url)
            {
                this.ArtworkUrl = url;
            }

            public void setPublicMode(int mode)
            {
                this.PublicMode = mode;
            }

            public int getArtworkID()
            {
                return this.ArtworkID;
            }

            public string getUserID()
            {
                return this.UserID;
            }

            public string getArtImgUrl()
            {
                return this.ArtImgUrl;
            }

            public string getArtName()
            {
                return this.ArtName;
            }

            public string getArtworkUrl()
            {
                return this.ArtworkUrl;
            }

            public int getPublicMode()
            {
                return this.PublicMode;
            }
        }
    }
}


