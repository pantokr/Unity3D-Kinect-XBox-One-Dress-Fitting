using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrintButton : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            System.Diagnostics.Process.Start("mspaint.exe", "/pt " + Application.dataPath + "\\Screenshots\\" + DecisionButton.nowString + ".png");
            ScenePhaseManager.SceneReload();
        }
    }
}