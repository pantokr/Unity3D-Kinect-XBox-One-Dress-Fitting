using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.Collections;
using UnityEngine.SceneManagement;

public class JointRecognizer : MonoBehaviour
{
    public BodySourceManager BodyManager; //Body Manager

    private Kinect.KinectSensor _Sensor; //Sensor
    private Kinect.CoordinateMapper _Mapper; //Mapper
    private readonly Vector2Int colorResolution = new Vector2Int(1920, 1080); //해상도

    public static Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        GlobalManager.ResetGlobalManager(); // 스태틱 클래스 초기화

        _Sensor = Kinect.KinectSensor.GetDefault(); // 플러그인 다운
        _Mapper = _Sensor.CoordinateMapper;
    }

    private void Update()
    {
        if (BodyManager == null) // 기본
        {
            return;
        }

        Kinect.Body[] data = BodyManager.GetData(); // 기본
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>(); // 기본
        foreach (var body in data)
        {
            if (body == null) // 기본
            {
                continue;
            }

            if (body.IsTracked) //사람들이 인식되고 있으면 실행 리스트에 추가
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys); // 

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds) // 사라진 사람 삭제
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
                if (GlobalManager.hasEntered && !_Bodies.ContainsValue(GameObject.Find(GlobalManager.entrant))) // 사람이 지정되었고, 그 사람이 리스트에서 삭제 되었을 때
                {
                    Debug.Log("Entrant Got Out"); 
                    ScenePhaseManager.SceneReload(); // 씬 재시작
                }
            }
        }

        foreach (var body in data)
        {
            if (body.IsTracked)
            {
                if (!GlobalManager.hasEntered) // 사람이 인식 되었으면
                {
                    if (!_Bodies.ContainsKey(body.TrackingId)) //사람 추가 X
                    {
                        _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    }
                }

                if (_Bodies.ContainsKey(body.TrackingId)) // 사람 위치 초기화
                {
                    RefreshBodyObject(body, _Bodies[body.TrackingId]);
                }
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body : " + id.ToString()); // 사람 생성
        body.transform.position = new Vector3Int(colorResolution.x / 2 * (-1), colorResolution.y / 2 * (-1), 0);
        body.AddComponent<JumpRecognizer>();

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = new GameObject(jt.ToString());
            jointObj.transform.SetParent(body.transform);
        }

        body.transform.SetParent(GameObject.Find("Bodies").transform); // 부모 지정

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject) //사람 초기화
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.ColorSpacePoint colorSpacePoint;
            Kinect.Joint sourceJoint = body.Joints[jt];

            GameObject joint = bodyObject.transform.Find(jt.ToString()).gameObject;
            colorSpacePoint = _Mapper.MapCameraPointToColorSpace(sourceJoint.Position);

            Vector2 point; // 칼라 스켈레톤 매칭
            point.x = float.IsInfinity(colorSpacePoint.X) ? 0 : colorSpacePoint.X;
            point.y = float.IsInfinity(colorSpacePoint.Y) ? 0 : colorSpacePoint.Y;
            joint.transform.localPosition = GetVector3FromJoint(point);
        }
    }

    private Vector3 GetVector3FromJoint(Vector2 point)
    {
        return new Vector3(point.x, colorResolution.y - point.y, 0); // 상하반전
    }
}
