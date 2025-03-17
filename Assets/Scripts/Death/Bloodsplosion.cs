using UnityEngine;

public class Bloodsplosion : MonoBehaviour
{
    public Rigidbody[] bloodDropsRB;
    public float expForce, expRad, killTime;
    private Vector3 expPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        expPos = transform.position;
        for (int i = 0; i < bloodDropsRB.Length; i++)
        {
            bloodDropsRB[i].AddExplosionForce(expForce, expPos, expRad);
        }
    }

    // Update is called once per frame
    void Update()
    {
        killTime = killTime - Time.deltaTime;
        if (killTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
