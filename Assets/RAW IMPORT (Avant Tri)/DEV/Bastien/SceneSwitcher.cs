using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void StartGame(){
	SceneManager.LoadScene("GameScene_B", LoadSceneMode.Single);
    }

    public void StartTransitionAnimation()
    {
        GetComponent<Animator>().SetTrigger("StartTransition");
    }
}
