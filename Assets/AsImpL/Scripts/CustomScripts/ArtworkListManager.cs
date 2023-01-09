using UnityEngine.UI;
using UnityEngine;

public class ArtworkListManager : MonoBehaviour
{
	public GameObject ArtListSystem;
	public Button ArtworkList_Button;
	
	public void ArtworkListButtonClick()
	{
		bool isActive = ArtListSystem.activeSelf;
		Debug.Log("Artwork Popup screen - active : " + (!isActive).ToString());
		ArtListSystem.SetActive(!isActive);
	}
}