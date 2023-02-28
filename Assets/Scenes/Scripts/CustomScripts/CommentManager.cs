using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CustomScripts{
    public class CommentManager : MonoBehaviour
    {
        public InputField commentField;

        public GameObject CommentPanel;
        public GameObject commentScrollPanel;
        public GameObject exampleComment;

        public Button Return_Button;
        public Button Like_Button;
        public Button Comment_Refresh_Button;

        public Text Like_Num;

		public RawImage Like_Image;
		public RawImage Unlike_Image;
        
        private float clickTime; 
        public float minClickTime = 1f;

        public int commentNum = 0;
        public int getItemNum = -1;
        public int currentItemNum = -1;

        private bool isClick; 

        [SerializeField] 
        public string comment_API_url = "[TO-DO 1]";

        public void ButtonDown() 
        { 
            isClick = true; 
        } 
        
        public void ButtonUp() 
        { 
            isClick = false;
        } 

        public void ReturnButtonClick()
        {
            CommentPanel.SetActive(false);
        }

        public void LongButttonClick()
        {
            CommentPanel.SetActive(true);
        
            // 방금 클릭한 게임 오브젝트의 번호 (= 작품 번호) 저장
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            string artItemName = clickObject.name; 

            getItemNum = int.Parse(artItemName[artItemName.Length-1].ToString()); 
        }

        public void LikeButtonClick()
		{
			bool Like_isActive = Like_Image.gameObject.activeSelf;
			bool Unlike_isActive = Unlike_Image.gameObject.activeSelf;

            if (Like_isActive == true) // active -> inactive 
            {
                StartCoroutine(UpdateDB("unlike_update"));
            }
            else
            {
                StartCoroutine(UpdateDB("like_update"));
            }

			Like_Image.gameObject.SetActive(!Like_isActive);
			Unlike_Image.gameObject.SetActive(!Unlike_isActive);
		}

        public void CommentButtonClick() => StartCoroutine(UpdateDB("comment_update"));

        IEnumerator UpdateDB(string command) 
        {
            WWWForm form = new WWWForm();
            form.AddField("command", command);
            form.AddField("artwork_id", currentItemNum);

            if (command == "comment_update")
            {
                form.AddField("comment_val", commentField.text);
                form.AddField("comment_date", DateTime.Now.ToString("yyyy-MM-dd"));
                commentField.text = "";
                LoadCommentList(); // 댓글 업데이트 적용 + 업데이트에 시간이 걸리는 경우도 있음 -> 새로고침 버튼 누르도록 
            }

            UnityWebRequest www = UnityWebRequest.Post(comment_API_url, form);

            yield return www.SendWebRequest();
            www.Dispose();

            if (command == "like_update" || command == "unlike_update")
            {
                StartCoroutine(ReadDB("like_read"));
            }
        }

        IEnumerator ReadDB(string command) 
        {
            WWWForm form = new WWWForm();

            form.AddField("command", command);
            form.AddField("artwork_id", currentItemNum);

            UnityWebRequest www = UnityWebRequest.Post(comment_API_url, form);

            yield return www.SendWebRequest();

            if (command == "like_read")
            {
                Like_Num.text = www.downloadHandler.text;
            }

            if (command == "comment_read")
            {
                // 각 댓글은 |로 구분하고, 댓글에 포함된 각 항목은 >로 구분 
                string[] comment_result = www.downloadHandler.text.Split(new char[] { '|' });
                commentNum = int.Parse(comment_result[0]);
                comment_result = comment_result.Skip(1).Take(comment_result.Length-2).ToArray();

                for (int i = 0; i < comment_result.Length ; i++)
                {
                    string[] comment_row = comment_result[i].Split(new char[] { '>' });

                    int childNum = i;
                    GameObject child = Instantiate(exampleComment);
                    child.name = "CommentItem " + childNum.ToString();

                    Text CommentDate = child.transform.GetChild(0).gameObject.GetComponent<Text>(); 
                    Text CommentVal = child.transform.GetChild(1).gameObject.GetComponent<Text>();
                    CommentDate.text = comment_row[0];
                    CommentVal.text = comment_row[1];

                    child.transform.GetChild(0).name = "CommentDate " + childNum.ToString(); 
                    child.transform.GetChild(1).name = "CommentVal " + childNum.ToString(); 
                    child.transform.SetParent(commentScrollPanel.transform);
                }
            }
            www.Dispose();
        }

        public void LoadCommentList()
        {
            DeleteCommentList();
            StartCoroutine(ReadDB("comment_read"));
        }
        
        public void DeleteCommentList()
        {      
            int numOfChild = commentScrollPanel.transform.childCount;
            if (numOfChild > 0)
            {
                for (int i=0;i<numOfChild;i++)
                {
                    Transform temp_comment = commentScrollPanel.transform.GetChild(i);
                    Destroy(temp_comment.gameObject);
                }
            }
        }

        private void Update() 
        { 
            if (isClick) 
            { 
                clickTime += Time.deltaTime; 

                if(clickTime >= minClickTime) 
                { 
                    LongButttonClick();
                } 
            } 
            else
            { 
                clickTime = 0; 
            } 

            if (getItemNum != currentItemNum)
            {
                currentItemNum = getItemNum;
                LoadCommentList();
                StartCoroutine(ReadDB("like_read")); // 여러 번 실행되지 않도록 
            }
        }
    }
}