using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LoadDB;

namespace ArtItem
{
    public class ArtImgManager : MonoBehaviour
    {
        public RawImage artImg;
        public Text artName;

        public int nRows = 0;

        public static int artItemNum = -1;

        public Coroutine coroutine = null;

        public GameObject scrollContent; // Content 아래에 button 만들려고 해서, Content 클래스 불러오려고 했는데 없다고 함 -> GameObject
        public Button exampleButton;
        public Button refreshButton;
        // public RawImage LoadingPanel;


        public int numOfChild = 0;

        public bool refreshMode = false;
        public string test;

        void Start()
        {
        }

        void Update()
        {
            nRows = DatabaseManager.nRows;
            numOfChild = scrollContent.transform.childCount;

            if (numOfChild == 0) // 처음 상태
            {
                while (numOfChild < nRows)
                {
                    Button child = Instantiate(exampleButton);
                    child.name = "Button " + numOfChild.ToString();

                    artImg = child.transform.GetChild(0).gameObject.GetComponent<RawImage>(); // GetComponent<>() 가 GameObject -> RawImage로 형변환
                    artName = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                    child.transform.GetChild(0).name = "ArtImage_RawImage " + numOfChild.ToString(); // 버튼 아래에 작품 이미지 생성
                    child.transform.GetChild(1).name = "ArtName_Text (Legacy) " + numOfChild.ToString(); // 버튼 아래에 작품 이름 생성
                    
                    child.transform.SetParent(scrollContent.transform);
                    LoadArtItem(artImg, artName, numOfChild);
                    numOfChild += 1;
                }
            }

            if (refreshMode == true) // 새로고침 한 경우 // 따로 해야 하는 이유는 Destroy 있어서 
            {
                for (int i=0;i<scrollContent.transform.childCount;i++)
                {
                    GameObject temp_button = GameObject.Find("Button " + i);
                    Destroy(temp_button);
                }

                numOfChild = 0;
                while (numOfChild < nRows)
                {
                    Button child = Instantiate(exampleButton);
                    child.name = "Button " + numOfChild.ToString();

                    artImg = child.transform.GetChild(0).gameObject.GetComponent<RawImage>(); // GetComponent<>() 가 GameObject -> RawImage로 형변환
                    artName = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                    child.transform.GetChild(0).name = "ArtImage_RawImage " + numOfChild.ToString(); // 버튼 아래에 작품 이미지 생성
                    child.transform.GetChild(1).name = "ArtName_Text (Legacy) " + numOfChild.ToString(); // 버튼 아래에 작품 이름 생성
                    
                    child.transform.SetParent(scrollContent.transform);
                    LoadArtItem(artImg, artName, numOfChild);
                    numOfChild += 1;
                }

                refreshMode = false;
            }
        }

        public void RefreshButtonClick()
        {
            refreshMode = true; // 처음부터 다시 받아오기 
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

        /*
            이미지 로드되기 전에 coroutine 종료되는 문제 있어서 일단 주석 처리 
            if (img_url == DatabaseManager.artInfo[0].getArtImgUrl())
            {
                StopAllCoroutines();
                coroutine = null;
                // Debug.Log("Stop The Coroutine (Load Image... O/X).");
            }
            // imgList = new RawImage();
            
        
            SelectedArtItem.selectedArtItem.setArtItemNum(artItemNum);

            if (art_name != DatabaseManager.artInfo[0].getArtName())
            {
                art_name = DatabaseManager.artInfo[0].getArtName();
                text.text = art_name;
            }
        */

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