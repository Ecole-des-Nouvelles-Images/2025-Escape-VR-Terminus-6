using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchBakedLights : MonoBehaviour
{
    public Texture2D[] darkLightmapDir, darkLightmapColor;
    public Texture2D[] brightLightmapDir, brightLightmapColor;

    private LightmapData[] darkLightmap, brightLightmap;

    void Start()
    {
        List<LightmapData> dlightmap = new List<LightmapData>();

        for(int i = 0; i < darkLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];

            dlightmap.Add(lmdata);
        }

        darkLightmap = dlightmap.ToArray();

        List<LightmapData> blightmap = new List<LightmapData>();

        for(int i = 0; i < brightLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];

            blightmap.Add(lmdata);
        }

        brightLightmap = blightmap.ToArray();
    }

    public List<GameObject> lightsObjects = new List<GameObject>();
    public List<MeshRenderer> lightsObjectMeshRenderers = new List<MeshRenderer>();
    public Material lampOnMat;
    public Material lampOffMat;
    
    [ContextMenu("Switch to Dark Lightmap")]
    private void SwitchDark()
    {
        LightmapSettings.lightmaps = darkLightmap;
        foreach (GameObject lightRay in lightsObjects) {
            lightRay.SetActive(false);
        }
        /*foreach (MeshRenderer lampRenderer in lightsObjectMeshRenderers) {
            lampRenderer.material = lampOffMat;
        }*/
    }

    [ContextMenu("Switch to Bright Lightmap")]
    private void SwitchBright()
    {
        LightmapSettings.lightmaps = brightLightmap;
        foreach (GameObject lightRay in lightsObjects) {
            lightRay.SetActive(transform);
        }
        /*foreach (MeshRenderer lampRenderer in lightsObjectMeshRenderers) {
            lampRenderer.material = lampOnMat;
        }*/
    }

    
}