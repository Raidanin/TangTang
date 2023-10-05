using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchJoystickController : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public Image joystickBackground;
    public Image joystickHandle;

    private Vector2 clickStartPosition;
    private Vector2 clickDirection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickStartPosition = Input.mousePosition;
            joystickBackground.gameObject.SetActive(true);
            joystickBackground.transform.position = clickStartPosition;
            joystickHandle.transform.position = clickStartPosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 clickPosition = Input.mousePosition;
            clickDirection = clickPosition - clickStartPosition;
            clickDirection.Normalize();

            Vector3 movement = new Vector3(clickDirection.x * moveSpeed * Time.deltaTime, 0, clickDirection.y * moveSpeed * Time.deltaTime);
            player.transform.position += movement;

            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            joystickHandle.transform.position = clickStartPosition + clickDirection * 50; // 조절 가능한 값
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystickBackground.gameObject.SetActive(false);
            joystickHandle.transform.position = clickStartPosition;
        }
    }
}