using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
	public class LoginManager : MonoBehaviour
	{
		public GameObject LoginView;

		public Button LoginButton;
		public Button CancelButton;

		public InputField ID_inputField;
		public InputField PW_inputField;

		public static bool LoginButtonMode = false;

		public void LoginButtonClick()
		{
			if (string.IsNullOrWhiteSpace(ID_inputField.text) == false)
			{
				UserInfo.userInfo.setUserID(ID_inputField.text);
				LoginView.SetActive(false);
				LoginButtonMode = true;
			}
		}

		public void CancelButtonClick()
		{
			LoginView.SetActive(false);
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