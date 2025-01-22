using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 10;
    [SerializeField] private GameObject m_Weapon;



    private void Start()
    {
        if (m_Weapon == null) Debug.LogError("WEAPON ISN'T SET FOR CAMERA MOVEMENT");
    }

    private void Update()
    {
        SetPosition();
    }


    private void SetPosition()
    {
        Vector2 pos = m_Weapon.transform.position;
        Vector2 posn = new Vector3(pos.x, pos.y, 0);

        Vector2 posint = Vector2.Lerp(transform.position, posn, m_MovementSpeed * Time.deltaTime);

        transform.position = posint;
        //transform.position = Vector3.MoveTowards(transform.position, posint, m_MovementSpeed * Time.deltaTime);




    }




}
