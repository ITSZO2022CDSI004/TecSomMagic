using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturas : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X))
        {
            string screenshotTime = System.DateTime.Now.ToString("HH-mm-ss");
            ScreenCapture.CaptureScreenshot("SomMagicCaptura" + screenshotTime + ".png");
        }
#endif

    }
}