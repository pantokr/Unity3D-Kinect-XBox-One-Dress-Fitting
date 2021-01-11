using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class SkeletonOverlayer : MonoBehaviour // 칼라, 스켈레톤 매칭
{
    public Material JointMaterial;
    public BodySourceManager BodyManager;

    private Kinect.KinectSensor _Sensor;
    private Kinect.CoordinateMapper _Mapper;
    private readonly Vector2Int colorResolution = new Vector2Int(1920, 1080);

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        _Sensor = Kinect.KinectSensor.GetDefault();
        _Mapper = _Sensor.CoordinateMapper;
    }

    private void Update()
    {
        if (BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Joint");
        body.transform.position = new Vector3(colorResolution.x / 2 * (-1), colorResolution.y / 2 * (-1), 10f);
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            jointObj.transform.localScale = new Vector3(30f, 30f, 30f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
            jointObj.GetComponent<MeshRenderer>().material = JointMaterial;
        }

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.ColorSpacePoint colorSpacePoint;
            Kinect.Joint sourceJoint = body.Joints[jt];
            Vector2 point;

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            colorSpacePoint = _Mapper.MapCameraPointToColorSpace(sourceJoint.Position);

            point.x = float.IsInfinity(colorSpacePoint.X) ? 0 : colorSpacePoint.X;
            point.y = float.IsInfinity(colorSpacePoint.Y) ? 0 : colorSpacePoint.Y;
            jointObj.localPosition = GetVector3FromJoint(point);
        }
    }

    private Vector3 GetVector3FromJoint(Vector2 point)
    {
        return new Vector3(point.x, colorResolution.y - point.y, Camera.main.farClipPlane);
    }
}
