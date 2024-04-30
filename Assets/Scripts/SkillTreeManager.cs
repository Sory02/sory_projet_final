using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillTreeManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
