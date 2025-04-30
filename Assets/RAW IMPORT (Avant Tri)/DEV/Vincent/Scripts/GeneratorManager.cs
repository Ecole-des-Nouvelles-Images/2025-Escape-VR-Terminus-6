using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public GeneratorLever LocalGeneratorLever;
    public Fusible LocalFusible;
    public CableAnchor LocalCableHead1;
    public CableAnchor LocalCableHead2;
    public CableAnchor LocalCableHead3;

    public MeshRenderer LampMeshRenderer;
    public Material LampMatOn;
    public Material LampMatOff;
    public bool GeneratorOk { get; private set; }

    private void Update()
    {
        UpdateGeneratorOk();
        VerifyLamp();
    }

    private void UpdateGeneratorOk()
    {
        // Vérifie les booléens dans les objets associés
        bool leverOk = LocalGeneratorLever != null && LocalGeneratorLever.LeverActivated;
        bool fusibleOk = LocalFusible != null && LocalFusible.FusibleOk;
        bool cableHead1Ok = LocalCableHead1 != null && LocalCableHead1.CableOk;
        bool cableHead2Ok = LocalCableHead2 != null && LocalCableHead2.CableOk;
        bool cableHead3Ok = LocalCableHead3 != null && LocalCableHead3.CableOk;

        // Met à jour GeneratorOk en fonction des booléens
        GeneratorOk = leverOk && fusibleOk && cableHead1Ok && cableHead2Ok && cableHead3Ok;
    }

    private void VerifyLamp() {
        if (GeneratorOk) {
            LampOn();
        }
        else {
            LampOff();
        }
    }

    private void LampOn() {
        if (LampMeshRenderer.material != LampMatOn) {
            LampMeshRenderer.material = LampMatOn;
        }
    }

    private void LampOff() {
        if (LampMeshRenderer.material != LampMatOff) {
            LampMeshRenderer.material = LampMatOff;
        }
    }
    
}