using UnityEngine;

public class BloodyWalls : MonoBehaviour
{
    private MeshRenderer thisRenderer;
    private int bloodLevel;
    public Material[] bloodyWallMats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bloodLevel = 0;
        thisRenderer = GetComponent<MeshRenderer>();
    }

    //Detectar salpicaduras de sangre
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Blood")
        {
            BloodStained();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blood")
        {
            BloodStained();
        }
    }

    //Aplicar los distintos niveles de texturas sangrientas
    void BloodStained()
    {
        bloodLevel++;
        if (bloodLevel > 20)
        {
            thisRenderer.material = bloodyWallMats[3];
        }
        else if (bloodLevel > 14)
        {
            thisRenderer.material = bloodyWallMats[2];
        }
        else if (bloodLevel > 8)
        {
            thisRenderer.material = bloodyWallMats[1];
        }
        else if (bloodLevel > 2)
        {
            thisRenderer.material = bloodyWallMats[0];
        }
    }
}
