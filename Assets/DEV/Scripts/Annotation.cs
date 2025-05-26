#if UNITY_EDITOR
using Unity.Tutorials.Core.Editor;
using UnityEngine;

[AddComponentMenu("Dev Tools/Annotation")]
public class Annotation : MonoBehaviour {
    [Header("This component is editor-only and will self-destruct on Awake")]
    [TextArea(2,100)]
    [SerializeField] private string _comment;
    
    private void Awake() {
        _comment = null;
        Destroy(this);
    }
}
#endif