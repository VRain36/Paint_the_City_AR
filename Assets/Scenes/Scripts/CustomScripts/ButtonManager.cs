using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts
{
	public class ButtonManager : MonoBehaviour
	{
		public GameObject ArtListSystem;
		public GameObject LoginSystem;

		public Button ArtworkList_Button;
		public Button LoginRequest_Button;
		
		public void ArtworkListButtonClick()
		{
			bool isActive = ArtListSystem.activeSelf;
			ArtListSystem.SetActive(!isActive);
		}

		public void LoginRequestButtonClick()
		{
			bool isActive = LoginSystem.activeSelf;
			LoginSystem.SetActive(!isActive);
		}
	}
}