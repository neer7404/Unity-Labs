using UnityEngine;
using UnityEngine.SceneManagement; // Essential

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // This replaces the Menu with your Game Level
        SceneManager.LoadScene("SampleScene"); 
    }
}