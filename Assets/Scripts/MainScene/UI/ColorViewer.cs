using UnityEngine;
using UnityEngine.UI;

public class ColorViewer : MonoBehaviour // 칼라 사진 받아오기
{
    public MultiSourceManager MultiManager;
    RawImage colorViewer;

    private void Start()
    {
        colorViewer = gameObject.GetComponent<RawImage>(); 
    }

    private void Update()
    {
        colorViewer.texture = MultiManager.GetColorTexture();
    }
}
