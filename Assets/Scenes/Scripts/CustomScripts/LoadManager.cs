using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomScripts
{
    public class LoadManager : MonoBehaviour
    {
        public GameObject LoadingPanel;
        public GameObject DisplayPanel;

        public static float loadedTime = 0.0f; 

        void Update()
        {
            if (loadedTime != 0.0f)
            {
                LoadingPanel.SetActive(false);
                loadedTime = 0.0f;
                DisplayPanel.SetActive(false); // AR 기능 미리 실행되는 것 방지
            }

            if (ImportOBJManager.artLoadStart == true)
            {
                LoadingPanel.gameObject.SetActive(true);
            }
        }
    }
}
