using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void StartGame(){
	SceneManager.LoadScene("GameScene_C", LoadSceneMode.Single);
    }

    public void StartTransitionAnimation()
    {
        GetComponent<Animator>().SetTrigger("StartTransition");
    }
}
