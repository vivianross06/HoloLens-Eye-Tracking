using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject screenTrackingSphere;
    public GameObject worldTrackingSphere;

    public AudioClip enterPitch;
    public AudioClip calibrationPitch;
    public AudioClip headConstrainedPitch;
    public AudioClip bodyConstrainedPitch;
    public AudioClip ssWalkingPitch;
    public AudioClip wsWalkingPitch;
    public AudioClip hallwayPitch;

    private static int taskIndex = 0;
    private static List<int> tasks;
    
    private List<int> order1 = new List<int>() { 5, 6, 2, 4, 1, 3 };
    private List<int> order2 = new List<int>() { 1, 2, 4, 6, 3, 5 };
    private List<int> order3 = new List<int>() { 2, 3, 5, 1, 4, 6 };
    private List<int> order4 = new List<int>() { 4, 5, 1, 3, 6, 2 };
    private List<int> order5 = new List<int>() { 3, 4, 6, 2, 5, 1 };
    private List<int> order6 = new List<int>() { 6, 1, 3, 5, 2, 4 };

    private bool automaticMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            DisableEverything();
            automaticMode = !automaticMode;
            taskIndex = 0;
        }
        if (Input.GetKeyDown("1"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order1;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartCalibrationTask();
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order2;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartScreenStabilizedHeadConstrainedTask();
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order3;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartWorldStabilizedBodyConstrainedTask();
            }
        }
        if (Input.GetKeyDown("4"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order4;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartScreenStabilizedWalkingTask();
            }
        }
        if (Input.GetKeyDown("5"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order5;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartWorldStabilizedWalkingTask();
            }
        }
        if (Input.GetKeyDown("6"))
        {
            if (automaticMode && ((taskIndex == 0) || (taskIndex >= tasks.Count)))
            {
                tasks = order6;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartHallwayTask();
            }
        }
        
    }

    public void StartNextTask()
    {
        DisableEverything();
        if (!automaticMode || (taskIndex >= tasks.Count))
        {
            return;
        }
        if (tasks[taskIndex] == 1)
        {
            StartCalibrationTask();
        }
        if (tasks[taskIndex] == 2) {
            StartScreenStabilizedHeadConstrainedTask();
        }
        if (tasks[taskIndex] == 3)
        {
            StartWorldStabilizedBodyConstrainedTask();
        }
        if (tasks[taskIndex] == 4)
        {
            StartScreenStabilizedWalkingTask();
        }
        if (tasks[taskIndex] == 5)
        {
            StartWorldStabilizedWalkingTask();
        }
        if (tasks[taskIndex] == 6)
        {
            StartHallwayTask();
        }
        taskIndex++;
    }

    public void StartCalibrationTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<Calibration>().enabled = true;
        PlayAudio(1);
    }

    public void StartScreenStabilizedHeadConstrainedTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssHeadConstrained";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
        PlayAudio(2);
    }

    public void StartWorldStabilizedBodyConstrainedTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsBodyConstrained";
        worldTrackingSphere.GetComponent<WorldStabilized>().isBodyConstrained = true;
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
        PlayAudio(3);
    }

    public void StartScreenStabilizedWalkingTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssWalking";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
        PlayAudio(4);
    }

    public void StartWorldStabilizedWalkingTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsWalking";
        worldTrackingSphere.GetComponent<WorldStabilized>().isBodyConstrained = false;
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
        PlayAudio(5);
    }

    public void StartHallwayTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<Hallway>().enabled = true;
        PlayAudio(6);
    }

    public void DisableEverything()
    {
        screenTrackingSphere.GetComponent<Calibration>().enabled = false;
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = false;
        screenTrackingSphere.SetActive(false);
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = false;
        worldTrackingSphere.GetComponent<Hallway>().enabled = false;
        worldTrackingSphere.SetActive(false);
    }

    private void PlayAudio(int taskIndex)
    {
        if (taskIndex == 1 || taskIndex == 2 || taskIndex == 4)
        {
            StartCoroutine(PlayScreenStabilizedAudio(taskIndex));
        }
        else
        {
            StartCoroutine(PlayWorldStabilizedAudio(taskIndex));
        }
    }

    private IEnumerator PlayScreenStabilizedAudio(int taskIndex)
    {
        AudioSource taskAudio = screenTrackingSphere.GetComponent<AudioSource>();
        if (taskIndex == 1)
        {
            taskAudio.clip = calibrationPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
        if (taskIndex == 2)
        {
            taskAudio.clip = headConstrainedPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
        if (taskIndex == 4)
        {
            taskAudio.clip = ssWalkingPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
    }

    private IEnumerator PlayWorldStabilizedAudio(int taskIndex)
    {
        AudioSource taskAudio = worldTrackingSphere.GetComponent<AudioSource>();
        if (taskIndex == 3)
        {
            taskAudio.clip = bodyConstrainedPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
        if (taskIndex == 5)
        {
            taskAudio.clip = wsWalkingPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
        if (taskIndex == 6)
        {
            taskAudio.clip = hallwayPitch;
            taskAudio.Play();
            yield return new WaitForSeconds(0.5f);
            taskAudio.Stop();
            taskAudio.clip = enterPitch;
        }
    }
}
