using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
public class LolliPop : MonoBehaviour
{

    // Update is called once per frame
    private void Start() // 롤리팝 애니메이션
    {
    }

    private void Update()
    {
        Vector3 b = new Vector3(-250f, -100f, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, b, Time.deltaTime * 5f);
    }
}
