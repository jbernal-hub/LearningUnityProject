using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeGame : MonoBehaviour
{
    public void InitializePlatform()
    {
        VirtualMountPoint.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
