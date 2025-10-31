using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ChangeControls(int controlNum)
    {
        if (controlNum == 0)
        {
            //Make 0 be turntable rotation = movement forward/back
            //PlayerController.isTurntableRotationMoveSideways = false;
        }
        else if (controlNum == 1)
        {
            // Make 1 be turntable rotation = boat rotation
            //PlayerController.isTurntableRotationMoveSideways = true;
        }
    }
}
