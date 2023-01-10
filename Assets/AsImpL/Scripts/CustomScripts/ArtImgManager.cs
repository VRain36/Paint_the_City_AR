using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LoadDB;
using CustomScripts;


namespace ArtItem
{
    public class ArtImgManager : MonoBehaviour
    {
        public GameObject LoginRequestPanel;
        public GameObject LoginPanel;

        public int nRows = 0;

        public static int artItemNum = -1;

        public Coroutine coroutine = null;

        public GameObject scrollContent; // Content 아래에 button 만들려고 해서, Content 클래스 불러오려고 했는데 없다고 함 -> GameObject
        public Button exampleButton;
        public Button refreshButton;
        public Button publicButton;
        public Button privateButton;
        // public RawImage LoadingPanel;


        public int numOfChild = 0;
        public int childItemNum = 0;

        public bool refreshMode = false;
        public bool publicMode = true;
        public bool publicModeChange = false;
        public bool initialMode = true;
        public string test;
        public string userID;

        public static List<int> userIndexArray = new List<int>(); 

        void Start()
        {
        }

        void Update()
        {
            numOfChild = scrollContent.transform.childCount;
            userID = UserInfo.userInfo.getUserID();

            
            if (initialMode == true) // 초기상태 -> 의 조건을 (numOfChild 개수 == 0) 이렇게 하다보니, 만약 개수 0이면 mode 변경되면 중복으로 생성되는 문제
            {
                LoadArtList(publicMode); // public            
                initialMode = false; // 작품 리스트 창 밖에서 로그인 한 경우, 초기모드로 인식되지 않아서 중복으로 보일 수도 
            }


            // 로그인 유무 확인
            if (LoginManager.LoginButtonMode == true) // && userID != "" 여부는 LoginButtonMode == true 에서 확인함
            {
                userIndexArray = new List<int>(); // 사용자 변경될 수도 있으므로, 초기화
                for (int i=0;i<DatabaseManager.artInfo.Length;i++)
                {
                    if (userID == DatabaseManager.artInfo[i].getUserID())
                    {
                        userIndexArray.Add(i); // for문으로 계속 add() 하므로, 한 번만 확인하도록 해야 
                    }
                }

                LoginRequestPanel.gameObject.SetActive(false);
                LoadArtList(publicMode);
                LoginManager.LoginButtonMode = false;
                initialMode = false;
            }

            if (refreshMode == true) // 새로고침 한 경우 // 따로 해야 하는 이유는 Destroy 있어서 
            {
                LoadArtList(publicMode);
                refreshMode = false;
            }

            if (publicModeChange == true) // 모드 변경한 경우 // 따로 해야 하는 이유는 Destroy 있어서 
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
                    // if (temp_button.name.Contains("Button"))
                        // GameObject temp_button = GameObject.Find("Button " + i);
                    Destroy(temp_button.gameObject);
                }
            }
        }

        public void LoadArtList(bool mode)
        {
            int rows = 0;
            List<int> indexArray;

            if (mode == true) // public mode
            {
                rows = DatabaseManager.publicnRows;
                indexArray = DatabaseManager.publicIndexArray;
            }
            else{
                rows = userIndexArray.Count;
                indexArray = userIndexArray;
            }

            // if 문으로 현재 만들어진 자식 obj 개수 확인하고 싶었으나, Update() 하기 전에는 그대로 유지되어서 의미 X
            // 만들기 전에, 지우고 시작하면?
            DeleteArtList();

            childItemNum = 0; // 초기화 후 새로 시작
            for (int i=0;i<indexArray.Count;i++)
            {
                Button child = Instantiate(exampleButton);
                child.name = "Button " + childItemNum.ToString();

                RawImage artImg = child.transform.GetChild(0).gameObject.GetComponent<RawImage>(); // GetComponent<>() 가 GameObject -> RawImage로 형변환
                Text artName = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                child.transform.GetChild(0).name = "ArtImage_RawImage " + childItemNum.ToString(); // 버튼 아래에 작품 이미지 생성
                child.transform.GetChild(1).name = "ArtName_Text (Legacy) " + childItemNum.ToString(); // 버튼 아래에 작품 이름 생성
                
                child.transform.SetParent(scrollContent.transform);
                LoadArtItem(artImg, artName, indexArray[i]);
                childItemNum += 1;
            }
        }

        ////

        public void RefreshButtonClick()
        {
            refreshMode = true; // 처음부터 다시 받아오기 
        }

        public void PublicButtonClick()
        {
            if (publicMode == false) // private -> public
            {
                publicModeChange = true;
                publicMode = true;
            }
            LoginRequestPanel.gameObject.SetActive(false);
        }

        public void PrivateButtonClick()
        {
            if (publicMode == true) // public -> private 
            {
                publicModeChange = true;
                publicMode = false;
            }

            // 로그인 여부 확인 + 아이디 값 저장
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

        ///

        public static IEnumerator InteractableList(bool InteractableMode)
        {
            GameObject scrollPanel = GameObject.Find("ScrollContent_Content");
            Transform[] allChildren = scrollPanel.GetComponentsInChildren<Transform>(); // RawImage, 자기 자신 등 모든 객체 가져옴 -> 구분 필요 
            
            GameObject.Find("ArtListSystem").transform.Find("LoadingPanel_RawImage").gameObject.SetActive(!InteractableMode);
            // RawImage loadingPanel = LoadingPanel.gameObject.GetComponent<RawImage>();
            // loadingPanel.gameObject.SetActive(!InteractableMode);

            /* 작품 기다리는 동안 로딩 창 띄우려고 했는데 안 됨ㅠㅠ.. 코루틴이 아니라 그냥 함수처럼 행동함
            // 심지어 로딩 창 안 뜸... setActive(false) 인데도 띄우고 싶으면 어떡갛ㅁ...?
            float delay = 0f;
            if (InteractableMode == false){
                while (delay <= 10f){
                    delay += Time.deltaTime;
                }
                Debug.Log("delay = " + delay.ToString());
            }
            
            foreach(Transform child in allChildren)
            {
                if (child.name.Contains("Button"))
                {
                    Button childButton = child.gameObject.GetComponent<Button>();
                    childButton.interactable = InteractableMode;
                }
            }
            */

            yield break;
            
        }

        public void LoadArtItem(RawImage img, Text text, int index)
        {
            // 작품 이미지 로드 
            string img_url = "";
            string art_name = "";

            if (img_url != DatabaseManager.artInfo[index].getArtImgUrl())
            {
                img_url = DatabaseManager.artInfo[index].getArtImgUrl();
                // Debug.Log("Start The Coroutine (Load Image... O/X).");
                coroutine = StartCoroutine(GetTexture(img, img_url));
            }

            // 작품 이름 로드 
            if (art_name != DatabaseManager.artInfo[index].getArtName())
            {
                art_name = DatabaseManager.artInfo[index].getArtName();
                text.text = art_name;
            }

            // 선택된 작품 번호
            SelectedArtItem.selectedArtItem.setArtItemNum(artItemNum);
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

        public void ArtItemButtonClick() //예시 버튼에 onClick 해두면, 프리팹 생성될 때 이 함수도 알아서 컴포넌트에 붙음
        {
            // 방금 클릭한 게임 오브젝트를 가져와서 저장
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            string artItemName = clickObject.name; 
            artItemNum = int.Parse(artItemName[artItemName.Length-1].ToString()); // 이름 문자열의 마지막에 작품 번호 붙이기
            test = artItemNum.ToString();
        }
    }

    public class SelectedArtItem
    {
        public static SelectedArtItem selectedArtItem = new SelectedArtItem();

        private int selectedArtNum;

        public SelectedArtItem()
        {
            selectedArtNum = -1;
        }

        public void setArtItemNum(int num)
        {
            this.selectedArtNum = num;
        }

        public int getArtItemNum()
        {
            return this.selectedArtNum;
        }
    }
}