using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class DataLogger : MonoBehaviour
{
    private List<string> log = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFrame(int frameNumber, string movement)
    {
        log.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}",
                System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffff"),
                frameNumber,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingEnabled,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeCalibrationValid,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingEnabledAndValid,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingDataValid,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.x,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.y,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.z,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.x,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.y,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.z,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.x,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.y,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.z,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.x,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.y,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.z,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.x,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.y,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.z,
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z,
                Camera.main.transform.rotation.x,
                Camera.main.transform.rotation.y,
                Camera.main.transform.rotation.z,
                Camera.main.transform.rotation.w,
                Camera.main.transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y,
                Camera.main.transform.eulerAngles.z,
                transform.position.x,
                transform.position.y,
                transform.position.z,
                Camera.main.transform.InverseTransformPoint(transform.position).x,
                Camera.main.transform.InverseTransformPoint(transform.position).y,
                Camera.main.transform.InverseTransformPoint(transform.position).z,
                Camera.main.transform.TransformVector(Vector3.right).x,
                Camera.main.transform.TransformVector(Vector3.right).y,
                Camera.main.transform.TransformVector(Vector3.right).z,
                Camera.main.transform.TransformVector(Vector3.up).x,
                Camera.main.transform.TransformVector(Vector3.up).y,
                Camera.main.transform.TransformVector(Vector3.up).z,
                Camera.main.transform.TransformVector(Vector3.forward).x,
                Camera.main.transform.TransformVector(Vector3.forward).y,
                Camera.main.transform.TransformVector(Vector3.forward).z,
                movement,
                Mathf.Rad2Deg * Mathf.Atan(CoreServices.InputSystem.EyeGazeProvider.GazeDirection.x / CoreServices.InputSystem.EyeGazeProvider.GazeDirection.z),
                Mathf.Rad2Deg * Mathf.Atan(CoreServices.InputSystem.EyeGazeProvider.GazeDirection.y / CoreServices.InputSystem.EyeGazeProvider.GazeDirection.z),
                Mathf.Rad2Deg * Mathf.Atan(Camera.main.transform.InverseTransformPoint(transform.position).x / Camera.main.transform.InverseTransformPoint(transform.position).z),
                Mathf.Rad2Deg * Mathf.Atan(Camera.main.transform.InverseTransformPoint(transform.position).y / Camera.main.transform.InverseTransformPoint(transform.position).z)
                ));
    }

    public void AddHeader()
    {
        log.Clear();
        log.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}",
                "Time",
                "Frame",
                "EyeTrackingEnabled",
                "EyeCalibrationValid",
                "EyeTrackingEnabledAndValid",
                "EyeTrackingDataValid",
                "EyeGazeOrigin.x",
                "EyeGazeOrigin.y",
                "EyeGazeOrigin.z",
                "EyeGazeDirection.x",
                "EyeGazeDirection.y",
                "EyeGazeDirection.z",
                "EyeHitInfo.point.x",
                "EyeHitInfo.point.y",
                "EyeHitInfo.point.z",
                "EyeHitPosition.x",
                "EyeHitPosition.y",
                "EyeHitPosition.z",
                "EyeHitNormal.x",
                "EyeHitNormal.y",
                "EyeHitNormal.z",
                "HeadGazeOrigin.x",
                "HeadGazeOrigin.y",
                "HeadGazeOrigin.z",
                "HeadRotation.x",
                "HeadRotation.y",
                "HeadRotation.z",
                "HeadRotation.w",
                "HeadEulerAngles.x",
                "HeadEulerAngles.y",
                "HeadEulerAngles.z",
                "transform.position.x",
                "transform.position.y",
                "transform.position.z",
                "localTransform.position.x",
                "localTransform.position.y",
                "localTransform.position.z",
                "localXAxis.x",
                "localXAxis.y",
                "localXAxis.z",
                "localYAxis.x",
                "localYAxis.y",
                "localYAxis.z",
                "localZAxis.x",
                "localZAxis.y",
                "localZAxis.z",
                "Movement",
                "GazeAngleX",
                "GazeAngleY",
                "TargetAngleX",
                "TargetAngleY"
                ));
    }

    public void SaveFile(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
    }
}
