using UnityEngine.UI;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
	public GameObject ArtListSystem;
	public GameObject LoginSystem;
	public Button ArtworkList_Button;
	public Button LoginRequest_Button;
	
	public void ArtworkListButtonClick()
	{
		bool isActive = ArtListSystem.activeSelf;
		Debug.Log("Artwork Popup screen - active : " + (!isActive).ToString());
		ArtListSystem.SetActive(!isActive);
	}

	public void LoginRequestButtonClick()
	{
		bool isActive = LoginSystem.activeSelf;
		Debug.Log("Login Popup screen - active : " + (!isActive).ToString());
		LoginSystem.SetActive(!isActive);
	}
}