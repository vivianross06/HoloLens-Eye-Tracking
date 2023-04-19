using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject screenTrackingSphere;
    public GameObject worldTrackingSphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            StartCalibrationTask();
        }
        if (Input.GetKeyDown("2"))
        {
            StartScreenStabilizedHeadConstrainedTask();
        }
        if (Input.GetKeyDown("3"))
        {
            StartWorldStabilizedBodyConstrainedTask();
        }
        if (Input.GetKeyDown("4"))
        {
            StartScreenStabilizedWalkingTask();
        }
        if (Input.GetKeyDown("5"))
        {
            StartWorldStabilizedWalkingTask();
        }
        
    }

    public void StartCalibrationTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<Calibration>().enabled = true;
    }

    public void StartScreenStabilizedHeadConstrainedTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssHeadConstrained";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
    }

    public void StartWorldStabilizedBodyConstrainedTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsBodyConstrained";
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
    }

    public void StartScreenStabilizedWalkingTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssWalking";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
    }

    public void StartWorldStabilizedWalkingTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsWalking";
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
    }

        public void DisableEverything()
    {
        screenTrackingSphere.GetComponent<Calibration>().enabled = false;
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = false;
        screenTrackingSphere.SetActive(false);
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = false;
        worldTrackingSphere.SetActive(false);
    }
}
