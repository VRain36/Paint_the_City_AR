using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LoadDB{
    public class DatabaseManager : MonoBehaviour
    {
        public static int nRows;
        public static int publicnRows = 0;
        public static int privatenRows = 0;
        public static ArtInfoData[] artInfo;

        public static List<int> publicIndexArray = new List<int>(); 
        public static List<int> privateIndexArray = new List<int>(); 

        private void Start()
        {
            StartCoroutine(GetMySQLData());
        }
    
        private IEnumerator GetMySQLData()
        {
            string site_addr = "localhost";
            string serverPath = string.Format("http://{0}/LoadMySQL.php", site_addr); //PHP 파일의 위치를 저장
    
            WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터(AddField로 넘겨줄 수 있음)
    
            using (UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form)) //웹 서버에 요청
            {
                yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

                string[] php_result = webRequest.downloadHandler.text.Split(new char[] { '|' });  // |를 기준으로 잘라서 배열 result 에 넣어라.
                php_result = php_result.Take(php_result.Length-1).ToArray(); // using System.Linq; 활용하여 마지막 값 가져오지 않기 
                nRows = php_result.Length; // php에서 num of rows 받아오기로 변경하기
                artInfo = new ArtInfoData[nRows]; // lens 변수에 5개의 Class 메모리 할당
                for (int i=0; i<nRows; i++){
                    artInfo[i] = new ArtInfoData();
                }

                for (int i = 0; i < php_result.Length ; i++)
                {
                    string[] php_result_row = php_result[i].Split(new char[] { '>' });
                    artInfo[i].setArtworkID(Int32.Parse(php_result_row[0]));
                    artInfo[i].setUserID(php_result_row[1]);
                    artInfo[i].setArtImgUrl(php_result_row[2]);
                    artInfo[i].setArtName(php_result_row[3]);
                    artInfo[i].setArtworkUrl(php_result_row[4]);
                    artInfo[i].setPublicMode(Int32.Parse(php_result_row[5]));

                    if (Int32.Parse(php_result_row[5]) == 1) // public 
                    {
                        publicnRows += 1;
                        // publicIndexArray.Append(i);
                        publicIndexArray.Add(i);
                    }
                    else // private 
                    {
                        privatenRows += 1;
                        // privateIndexArray.Append(i);
                        privateIndexArray.Add(i);
                    }
                }

                Debug.Log("==================php로부터 데이터 송신 완료==============");

                // publicIndexArray = new int[publicnRows];
                // privateIndexArray = new int[privatenRows];
            }
        }

        ///
        /*
        public T[] Append<T>(this T[] array, T item) // 배열에 원소 추가 
        {
            if (array == null) {
                return new T[] { item };
            }
            T[] result = new T[array.Length + 1];
            array.CopyTo(result, 0);
            result[array.Length] = item;
            return result;
        }
        */


        ////

        public class ArtInfo{
            private ArtInfoData[] artInfoList; // Lens 클래스의 배열 선언
            public ArtInfo(int nRows) // Camera 클래스 생성자,  예제로 Class 생성자가 nLens라는 값을 초기 변수를 갖는다고 가정
            {
                artInfoList = new ArtInfoData[nRows]; // lens 변수에 5개의 Class 메모리 할당
                for (int i=0; i<nRows; i++)
                    artInfoList[i] = new ArtInfoData();  // 개별 lens 요소에 Lens 클래스 초기화 선언 (diameter=3.0 으로 초기화)
            }
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


