using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StencilLayerManager : MonoBehaviour
{
    public int layerToExclude = 8; // Layer à exclure (PortalExclude)
    public Material stencilMaterial; // Matériau utilisant le shader stencil

    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera.cameraType == CameraType.Game)
        {
            var cmd = CommandBufferPool.Get("StencilLayerManager");

            // Désactiver le rendu pour le layer spécifié
            cmd.Clear();
            cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
            cmd.SetGlobalInt("_StencilID", stencilMaterial.GetInt("_StencilID"));

            // Rendre les objets avec le stencil, en excluant le layer spécifié
            cmd.Blit(null, BuiltinRenderTextureType.CameraTarget, stencilMaterial, 1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}