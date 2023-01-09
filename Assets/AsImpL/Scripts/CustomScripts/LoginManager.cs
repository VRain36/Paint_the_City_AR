using UnityEngine.UI;
using UnityEngine;

namespace CustomScripts
{
	public class LoginManager : MonoBehaviour
	{
		//로그인 화면 Root
		public GameObject LoginView;
		public InputField ID_inputField;
		public InputField PW_inputField;
		public Button Login_Button;

		// 다른 모듈에서 참조할 수 있도록 변수 추가  
		public string testuserID = "";

		/// <summary>
		/// 로그인 버튼 클릭시 실행
		/// </summary>

		public void LoginButtonClick()
		{
			if (string.IsNullOrWhiteSpace(ID_inputField.text) == false)
			{
				Debug.Log("아이디가 입력 되었습니다.");

				// User ID 변수에 값 저장
				UserInfo.userInfo.setUserID(ID_inputField.text);
				testuserID = UserInfo.userInfo.getUserID();

				// 로그인 창 닫음
				LoginView.SetActive(false);
			}
			else
			{
				Debug.Log("아이디가 입력되지 않았습니다.");
			}
		}
	}

	public class UserInfo
	{
		public static UserInfo userInfo =  new UserInfo();

		private string userID;
		private string userPW;

		public UserInfo()
		{
			userID = "";
			userPW = "";
		}

		public void setUserID(string ID)
		{
			this.userID = ID;
		}

		public void setUserPW(string PW)
		{
			this.userPW = PW;
		}

		public string getUserID()
		{
			return this.userID;
		}

		public string getUserPW()
		{
			return this.userPW;
		}
	}
}