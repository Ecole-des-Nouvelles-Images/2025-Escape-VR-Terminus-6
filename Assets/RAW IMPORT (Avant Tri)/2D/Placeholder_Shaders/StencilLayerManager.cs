using UnityEngine;

public class StencilLayerManager : MonoBehaviour
{
    public int layerToExclude = 8;

    void OnPreRender()
    {
        Camera.main.cullingMask &= ~(1 << layerToExclude);
    }

    void OnPostRender()
    {
        Camera.main.cullingMask |= (1 << layerToExclude);
    }
}