using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Calibration()
    {
        SceneManager.LoadScene("Calibration");
    }

    public void ScreenStabilized()
    {
        SceneManager.LoadScene("ScreenStabilized");
    }

    public void WorldStabilized()
    {
        SceneManager.LoadScene("WorldStabilized");
    }

    public void Hallway()
    {
        SceneManager.LoadScene("Hallway");
    }

    public void StarterScene()
    {
        SceneManager.LoadScene("StartingScene");
    }

    public void TestScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void TestScene2()
    {
        SceneManager.LoadScene("TestScene2");
    }
}
