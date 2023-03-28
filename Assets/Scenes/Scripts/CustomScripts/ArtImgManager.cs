using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace CustomScripts
{
    public class ArtImgManager : MonoBehaviour
    {
        public GameObject LoginRequestPanel;
        public GameObject scrollContent; 
        public GameObject LoginPanel;

        public Button exampleButton;
        public Button refreshButton;
        public Button publicButton;
        public Button privateButton;

        public int nRows = 0;
        public int numOfChild = 0;

        public Coroutine coroutine = null;

        public bool refreshMode = false;
        public bool publicMode = true;
        public bool publicModeChange = false;
        public bool initialMode = true;

        public string userID;

        public static int artItemNum = -1;
        public static List<int> userIndexArray = new List<int>(); 

        void Update()
        {
            numOfChild = scrollContent.transform.childCount;
            userID = UserInfo.userInfo.getUserID();

            // 초기 상태인지 확인
            if (initialMode == true)  
            {
                /// <summary>
                /// 초기 상태의 조건을 numOfChild 개수 == 0 로 설정하면, 
                /// 만약 현재 존재하는 작품의 개수 0이면 mode 변경될 때마다 중복으로 생성되는 문제 발생하므로, 
                /// initialMode == true 로 조건문 설정
                /// </summary>

                LoadArtList(publicMode);     

                initialMode = false; 
            }


            // 로그인 유무 확인
            if (LoginManager.LoginButtonMode == true)
            {
                /// <summary>
                /// userID != "" 여부는 LoginButtonMode == true 에서 확인하므로, 조건문에 추가하지 않았음

                /// DatabaseManager.artInfo에 저장된 정보를 통해 사용자의 작품만을 찾아서 userIndexArray에 artwork_id를 저장
                /// 이때, 로그인 후 사용자 변경될 수도 있으므로, 초기화 후 새로 리스트에 값 저장하도록 함

                /// 로그인 버튼 클릭 시, LoadArtList()를 통해 새로 값 로드하지 않을 경우, 
                /// private 모드는 public -> private로 모드를 한 번 더 바꿔주지 않으면 값이 로드되지 않는 문제 발생하므로, 이 부분 코드 추가함
                
                /// 작품 리스트 창 밖에서 로그인 한 경우, 초기모드로 인식되지 않아서 중복으로 보일 수도 있으므로,
                /// initialMode = false 로 변경하도록 함
                /// </summary>

                userIndexArray = new List<int>(); 

                for (int i=0;i<DatabaseManager.artInfo.Length;i++)
                {
                    if (userID == DatabaseManager.artInfo[i].getUserID())
                    {
                        userIndexArray.Add(i); 
                    }
                }

                LoadArtList(publicMode);
                
                initialMode = false;
                LoginManager.LoginButtonMode = false;
                LoginRequestPanel.gameObject.SetActive(false);
            }

            // 새로고침 한 경우 
            if (refreshMode == true) 
            {
                LoadArtList(publicMode);
                refreshMode = false;
            }

            // public/private 모드 변경한 경우 
            if (publicModeChange == true)
            {
                LoadArtList(publicMode);
                publicModeChange = false;
            }
        }

        public void DeleteArtList()
        {      
            if (numOfChild > 0)
            {
                for (int i=0;i<numOfChild;i++)
                {
                    Transform temp_button = scrollContent.transform.GetChild(i);
                    Destroy(temp_button.gameObject);
                }
            }
        }

        public void LoadArtList(bool mode)
        {
            int rows = 0;
            List<int> indexArray;

            // public/private mode 확인 
            if (mode == true) 
            {
                rows = DatabaseManager.publicnRows;
                indexArray = DatabaseManager.publicIndexArray;
            }
            else{
                rows = userIndexArray.Count;
                indexArray = userIndexArray;
            }

            DeleteArtList();

            for (int i=0;i<indexArray.Count;i++)
            {
                int childNum = indexArray[i];
                Button child = Instantiate(exampleButton);
                child.name = "Button " + childNum.ToString();

                RawImage artImg = child.transform.GetChild(1).gameObject.GetComponent<RawImage>(); 
                Text artName = child.transform.GetChild(2).gameObject.GetComponent<Text>();

                child.transform.GetChild(1).name = "ArtImage_RawImage " + childNum.ToString(); 
                child.transform.GetChild(2).name = "ArtName_Text (Legacy) " + childNum.ToString(); 

                child.transform.SetParent(scrollContent.transform);
                child.transform.localScale = new Vector3(1, 1, 1); // 자동으로 스케일 변경되는 것 방지

                LoadArtItem(artImg, artName, childNum);
            }
        }

        /// ButtonClick Event

        public void RefreshButtonClick()
        {
            refreshMode = true; 
        }

        public void PublicButtonClick()
        {
            // private -> public
            if (publicMode == false) 
            {
                publicModeChange = true;
                publicMode = true;
            }
            LoginRequestPanel.gameObject.SetActive(false);
        }

        public void PrivateButtonClick()
        {
            // public -> private 
            if (publicMode == true) 
            {
                publicModeChange = true;
                publicMode = false;
            }

            // 로그인 여부 확인
            if (userID != "")
            {
                Debug.Log("User already Login >>> user id = " + userID);
            }
            else{
                Debug.Log("User need to Login");
                LoginRequestPanel.gameObject.SetActive(true);
            }

        }

        public void LoginRequestButtonClick()
        {
            LoginPanel.SetActive(true);
        }

        public void LoadArtItem(RawImage img, Text text, int index)
        {
            string img_url = DatabaseManager.artInfo[index].getArtImgUrl();
            string art_name = DatabaseManager.artInfo[index].getArtName();

            coroutine = StartCoroutine(GetTexture(img, img_url));

            text.text = art_name; 
        }

        public void ArtItemButtonClick() 
        {
            // 방금 클릭한 게임 오브젝트의 번호 (= 작품 번호) 저장
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            string artItemName = clickObject.name; 

            string[] temp_artItemName = artItemName.Split(' ');
            artItemNum = int.Parse(temp_artItemName[temp_artItemName.Length-1]);
        }

        IEnumerator GetTexture(RawImage img, string img_url)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(img_url); 
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }
    }
}