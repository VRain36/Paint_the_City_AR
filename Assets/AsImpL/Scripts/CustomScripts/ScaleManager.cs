using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleManager : MonoBehaviour
{
    public Text objScalingText;
    
    public void SetScaling(float scl)
    {
        scl = Mathf.Pow(2.0f, scl); // Mathf.Pow(10.0f, scl); // 10배씩은 크므로, 2배씩 늘리거나 1.5배씩 늘리기?
        objScalingText.text = "Scaling: " + scl;
        transform.localScale = new Vector3(scl, scl, scl);
    }
}
