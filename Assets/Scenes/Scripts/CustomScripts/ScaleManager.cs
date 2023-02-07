using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace CustomScripts
{
    public class ScaleManager : MonoBehaviour
    {
        public Text objScalingText;
        
        public void SetScaling(float scl)
        {
            scl = Mathf.Pow(2.0f, scl);
            objScalingText.text = "Scaling: " + scl;
            transform.localScale = new Vector3(scl, scl, scl);
        }
    }
}