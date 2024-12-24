using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public void LoadReactScene()
    {
        SceneManager.LoadScene("ReactScene");
    }

    public void LoadPuzzleScene()
    {
        SceneManager.LoadScene("PuzzleScene");
    }

    public void LoadMozaicScene()
    {
        SceneManager.LoadScene("MozaicScene");
    }

    public void LoadMemoryScene()
    {
        SceneManager.LoadScene("MemoryScene");
    }

}
