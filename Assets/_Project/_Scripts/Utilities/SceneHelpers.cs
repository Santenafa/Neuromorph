using UnityEngine;
using UnityEngine.SceneManagement;

namespace Neuromorph
{
public class SceneHelpers : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.C)) Application.Quit();
    }
}
}