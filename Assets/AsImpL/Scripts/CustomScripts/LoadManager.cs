using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtItem;
using AsImpL.Scripts;

public class LoadManager : MonoBehaviour
{
    public GameObject LoadingPanel;
    public string test = "";
    public static float loadedTime = 0.0f; // 작품 로드 완료된 시간을 받아옴

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("ImportOBJManager.artLoadStart = " + ImportOBJManager.artLoadStart.ToString());
        // 작품 클릭된 경우, 로딩 페이지 띄우기 // 같은 작품이 클릭될 수도 있으므로, 버튼 클릭 유무가 아니라 importobj에서 변수값 확인
        /*
        if (ImportOBJManager.artLoadStart == true)
        {
            LoadingPanel.gameObject.SetActive(true);
        }
        */


        test = loadedTime.ToString();

        if (loadedTime != 0.0f) // 로드 완료
        {
            LoadingPanel.gameObject.SetActive(false);
            loadedTime = 0.0f; // 초기화
        }

        if (ImportOBJManager.artLoadStart == true)
        {
            LoadingPanel.gameObject.SetActive(true);
        }

        // 평소에는 (false) 니까 굳이 조건문 추가 하지 않아도 될 듯?



        /*
        if (ImportOBJManager.artLoadStart == true)
        {
            LoadingPanel.gameObject.SetActive(true);
        }
        else{
            LoadingPanel.gameObject.SetActive(false);
        }
        */

        // Debug.Log("Set Active = " + test.ToString());

        // 작품 로드 시작 + 작품 이름을 발견하면 false + (작품 로드 시작 && 작품 이름을 발견 못 함 -> true)
        


        /*
        // 다른 작품 선택하려고 할 때, Destroy() 하고 불러오니까 (Find("") 가 너무 짧게 지나감 -> 조건 바꿔야...)
        // 작품 로드를 시작하고 + 아직 작품을 불러오지 않았음 
        if (GameObject.Find("User Artwork") == false && ArtImgManager.clickedMode) // 작품을 선택하지 않아도 false 이므로, 이 부분 동작함 -> 작품 선택되었을 때의 조건 추가하기
        {
            LoadingPanel.gameObject.SetActive(true);
        }
        else  // 작품 로드 완료 
        {
            if (GameObject.Find("User Artwork") == true)
            {
                ArtImgManager.clickedMode = false;
            }
            LoadingPanel.gameObject.SetActive(false);
        }
        */


        /*
        else // if (ImportOBJManager.artLoadStart == false) // 작품이 로드된 경우, 로딩 페이지 닫기
        {
            LoadingPanel.SetActive(false);
        }
        */
    }
}
