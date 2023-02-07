using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomScripts
{
    public class LoadManager : MonoBehaviour
    {
        public GameObject LoadingPanel;

        public static float loadedTime = 0.0f; 

        void Update()
        {
            if (loadedTime != 0.0f)
            {
                LoadingPanel.gameObject.SetActive(false);
                loadedTime = 0.0f;
            }

            if (ImportOBJManager.artLoadStart == true)
            {
                LoadingPanel.gameObject.SetActive(true);
            }
        }
    }
}
