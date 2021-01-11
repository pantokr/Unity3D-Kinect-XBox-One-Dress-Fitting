using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressFunctions : MonoBehaviour
{
    public Quaternion angleCalculation(Vector3 from, Vector3 to) // 각도 계산
    {
        return Quaternion.Euler(0, 0, Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z);
    }

    public Vector3 middleVector(Vector3 v1, Vector3 v2) // 두 벡터 중간값 계산
    {
        Vector3 mv = (v1 + v2) / 2;
        mv.z = 0f;
        return mv;
    }
}