using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TheObserver : MonoBehaviour
{
    public GameObject player;
    public Transform bodyTransform, eyeTransform;
    public Vector3 targetEuler;
    public float minAngleX, maxAngleX;
    private Vector3 targetVector;    
    private Quaternion targetQuaternion;
    private float heightDifference;
    private Transform targetTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetLookAt();
        bodyTransform.localEulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Coger el vector que apunta del observador al jugador, y la rotacion para apuntar a ese vector
        targetVector = (new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z) - bodyTransform.position).normalized;
        targetQuaternion = Quaternion.LookRotation(targetVector);        
        targetEuler = targetQuaternion.eulerAngles;

        //Giro del cuerpo
        if ((targetEuler.x > minAngleX && targetEuler.x < 360) || (targetEuler.x < maxAngleX && targetEuler.x > 0))
        {
            bodyTransform.eulerAngles = new Vector3(targetEuler.x, targetEuler.y, targetEuler.z);
        }
        else if (targetEuler.x < minAngleX && targetEuler.x> 180)
        {
            bodyTransform.eulerAngles = new Vector3(minAngleX, targetEuler.y, targetEuler.z);
        }
        else if(targetEuler.x > maxAngleX && targetEuler.x < 180)
        {
            bodyTransform.eulerAngles = new Vector3(maxAngleX, targetEuler.y, targetEuler.z);
        }

        //Movimiento del ojo
        heightDifference=bodyTransform.position.y - (player.transform.position.y+1.6f);
        if (heightDifference > 0)
        {
            eyeTransform.localPosition = new Vector3(eyeTransform.localPosition.x, -0.0015f, eyeTransform.localPosition.z);
        }
        else
        {
            eyeTransform.localPosition = new Vector3(eyeTransform.localPosition.x, -0.0005f, eyeTransform.localPosition.z);
        }
         
    }
    //Para apuntar las camaras, coger el transform del hijo vacio que esta en la cabeza del player
    public void GetLookAt()
    {
        foreach (Transform t in player.GetComponentsInChildren<Transform>())
        {
            if (t.name == "HeadLook")
            {
                targetTransform = t;
            }
        }
    }
}
