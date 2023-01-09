using UnityEngine;
// using CustomScripts;
using System.Collections;
using ArtItem;
using LoadDB;
using AsImpL;

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
            protected string objectName = "User Artwork";

            [SerializeField]
            protected ImportOptions importOptions = new ImportOptions();

            [SerializeField]
            protected PathSettings pathSettings;

            protected ObjectImporter objImporter;

            public Coroutine coroutine = null;

            public int artNum;
            public int getArtNum;

            public GameObject obj = null;
            public string test = "";
            public bool artLoadStart = false;
            public bool artLoadEnd = false;

            
            private void Awake()
            {
                /*
                filePath = pathSettings.RootPath + filePath;
                */

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
                getArtNum = ArtItem.ArtImgManager.artItemNum; // SelectedArtItem.selectedArtItem.getArtItemNum();

                // 버튼 비활성화 상태 (Load 시작 true / Load 끝 false)

                if (GameObject.Find(objectName) != null)
                {
                    if (artLoadStart == true)
                    {
                        Debug.Log("작품 로드 완료 -> 버튼 비활성화 해제");
                        StartCoroutine(ArtImgManager.InteractableList(true));
                        // ArtImgManager.InteractableList(true);
                        artLoadStart = false;
                    }
                    artLoadEnd = true;
                }
                else
                {
                    artLoadEnd = false;

                    if (artLoadStart == true) // 아직 로딩 중
                    {
                        StartCoroutine(ArtImgManager.InteractableList(false));
                        // ArtImgManager.InteractableList(false);
                    }

                }


                if (artNum != getArtNum) // 이전과 다른 작품 요구 
                {
                    filePath = DatabaseManager.artInfo[getArtNum].getArtworkUrl();
                    // Debug.Log("받아온 번호 : " + getArtNum + "현재 artNum " + artNum);

                    // artNum = getArtNum;
                    Debug.Log("Start The Coroutine (check UserID Value O/X).");
                    // 사실 코루틴 필요 없음 Update 만으로도 기다릴 수 있음 coroutine = StartCoroutine(checkUserIDValueCoroutine());

                    // 이미 있는 작품 지우기 & 새로 생성 
                    // object = child.transform.GetChild(0).gameObject.GetComponent<RawImage>(); 
                    // Destroy(object);


                    /*
                    if (obj != null){
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                        obj = GameObject.Find(objectName); //
                        Destroy(obj);
                        Debug.Log("object : " + obj.ToString());
                    }
                    else
                    {
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                        obj = GameObject.Find(objectName);
                        Debug.Log("object : " + obj.ToString());
                    }
                    */

                    Debug.Log("현재 상황입니다. artnum" + artNum.ToString() + " getartnum " + getArtNum.ToString());
                    // obj 삭제되는지 확인 + 삭제할 obj를 잘 찾는지 확인
                    if (artNum == -1) // obj가 로드되는 시간이 다르므로, obj의 null 여부로 if문 확인하는 것은 맞지 않음
                    {
                        Debug.Log("=================처음=====================");
                        Debug.Log("받아온 번호 : " + getArtNum + "현재 artNum " + artNum);
                        Debug.Log("처음 만드는 중입니다.");
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);

                        float delay1 = 0f;
                        artLoadStart = true;
                        // 완성될 때까지 다른 클릭 못 받도록 
                        /*
                        while (GameObject.Find(objectName) == null) // 작품 에셋 찾을 때까지 (작품이 아직 로드되지 않은 경우, 로딩 기다리기)
                        {
                            delay1 += Time.deltaTime;
                            test = delay1.ToString();
                        }
                        */
                        Debug.Log(delay1.ToString() + "초 걸려서 만들었습니다");

                        // obj = ObjectImporter.loadedObj;

                        /*
                        while (obj == null)
                        {
                            obj = GameObject.Find(objectName);
                        }
                        // obj = GameObject.Find(objectName);
                        Debug.Log("처음 만들었습니다. object : " + obj.ToString());
                        */
                    }
                    else// if (artNum >= 0) // 기존에 작품이 있는 경우 
                    {
                        Debug.Log("===================나중===================");
                        float delay = 0f;

                        
                        while (GameObject.Find(objectName) == null) // 작품 에셋 찾을 때까지 (작품이 아직 로드되지 않은 경우, 로딩 기다리기)
                        {
                            delay += Time.deltaTime;
                            test = delay.ToString();
                        }
                        
                        // 앞 작품이 만들어지는 것 기다리기
                        artLoadStart = true;


                        
                        obj = GameObject.Find(objectName);
                        Destroy(obj);  // 삭제
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                        Debug.Log(delay.ToString() + "초 걸려서 삭제하였고 다시 만들었습니다");
                        

                        // Debug.Log("존재하는 모델 이름 : " + GameObject.Find(objectName).ToString());


                        // obj = ObjectImporter.loadedObj; // 이렇게 해도 안 나오는디...
                        // Debug.Log("삭제했습니다. object : " + obj.ToString());
                    }

                    artNum = getArtNum;      

                    /*
                    if (artNum == -1 && obj != null) // 처음 생성
                    {
                        // Debug.Log("처음 만들었습니다. object : " + obj.ToString());
                    }
                    */
                    
                    /*
                    if (obj != null)
                    {
                        artNum = getArtNum;
                        Debug.Log("===================나중===================");
                        // obj = GameObject.Find(objectName);
                        // Debug.Log("obj가 이미 존재합니다. object : " + obj.ToString());
                        Destroy(obj); 
                        objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                        Debug.Log("다시 만들었습니다");
                        // obj = ObjectImporter.loadedObj; // 이렇게 해도 안 나오는디...
                        // Debug.Log("삭제했습니다. object : " + obj.ToString());
                    }
                    */



                    // objImporter.ImportModelAsync(objectName, filePath, null, importOptions);

                    // obj = GameObject.Find(objectName);
                    // Debug.Log("object : " + obj.ToString());

                }

                // 아이디 받아온 경우, 다음 실행
                if (string.IsNullOrWhiteSpace(filePath) == false && coroutine != null)
                {
                    StopAllCoroutines();
                    coroutine = null;
                    Debug.Log("Stop The Coroutine (check Value O/X).");

                    // url 로부터 작품 불러오기
                    // objImporter.ImportModelAsync(objectName, filePath, null, importOptions);
                }
            }

            IEnumerator checkUserIDValueCoroutine()
            {
                Debug.Log("Loading the file path value ...");

                yield return null;                
            }
        }
    }
}
