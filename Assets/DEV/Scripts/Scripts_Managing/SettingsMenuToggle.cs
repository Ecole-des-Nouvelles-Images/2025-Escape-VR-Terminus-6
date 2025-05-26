using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingsMenuToggle : MonoBehaviour
{
    // Nom de la scène où l'action ne doit pas être effectuée
    public string excludedSceneName = "TitleScreen";

    // Référence à l'objet Canvas à activer/désactiver
    public Canvas canvasToToggle;

    // Référence à l'Input Action Asset
    public InputActionAsset inputActionAsset;

    // Nom de l'action d'entrée à vérifier
    public string inputActionName = "PauseButton"; // Assurez-vous que ce nom correspond à votre action

    private InputAction inputAction;

    void Awake()
    {
        // Assure que cet objet et ses enfants ne sont pas détruits lors du chargement d'une nouvelle scène
        DontDestroyOnLoad(gameObject);

        // Trouver l'action d'entrée spécifique
        if (inputActionAsset != null)
        {
            inputAction = inputActionAsset.FindAction(inputActionName);
            if (inputAction != null)
            {
                inputAction.Enable();
            }
        }
    }

    void Update()
    {
        // Vérifie si l'action d'entrée est effectuée
        if (inputAction != null && inputAction.triggered)
        {
            // Vérifie si la scène actuelle n'est pas la scène exclue
            if (SceneManager.GetActiveScene().name != excludedSceneName)
            {
                // Active ou désactive le Canvas
                if (canvasToToggle != null)
                {
                    canvasToToggle.enabled = !canvasToToggle.enabled;
                }
            }
        }
    }

    void OnDestroy()
    {
        // Désactive l'action d'entrée lorsque l'objet est détruit
        if (inputAction != null)
        {
            inputAction.Disable();
        }
    }
}