using UnityEngine;
using System.Collections;

public static class ScenePhaseManager // 어느 씬에서나 사용 가능한 함수들
{
    public static void SceneReload() //씬 재시작
    {
        LoadingSceneManager.LoadScene("MainScene");
        Debug.Log("Scene Reload");
    }

    public static void MasterHome() // 씬 재시작
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneReload();
        }
    }
    public static void MasterPass() // 씬 재시작
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Scene Reload");
            SceneReload();
        }
    }

    public static void Quit() //나가기
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
