using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControl : MonoBehaviour
{
    public UJoystickButton Joystick;
    public Transform Cube;

    private Vector2 moveVect = Vector2.zero;

	private void Start ()
    {
        Joystick.OnDragEvent += OnDragEvent;
        Joystick.OnEndDragEvent += OnEndDragEvent;
    }

    private void FixedUpdate()
    {
        Cube.transform.position = new Vector3(Cube.transform.position.x + moveVect.x, Cube.transform.position.y, Cube.transform.position.z + moveVect.y);
    }

    private void OnDragEvent(Vector2 vect)  { moveVect = vect;}
    private void OnEndDragEvent() { moveVect = Vector2.zero; }
}
