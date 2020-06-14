using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float speed = 10f;
    private Vector2 prevMousePosition;
    private Vector2 rotation;
    private float maxRotation = 90f;

    public void Start() {
        rotation = new Vector2();
        Cursor.visible = true;
    }

    public void FixedUpdate() {
        HandleKeyPresses();
        HandleMouse();
    }

    private void HandleKeyPresses() {
        Vector3 delta = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            delta.z = speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            delta.x = -speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.RightArrow)) {
            delta.z = -speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow)) {
            delta.x = speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space)) {
            delta.y = speed * Time.deltaTime;
        }

        transform.Translate(delta);
    }

    private void HandleMouse() {
        Vector2 mousePosition = new Vector2(
            Input.mousePosition.x - Screen.width / 2f,
            Input.mousePosition.y - Screen.height / 2f
        );

        Vector2 mouseChange = prevMousePosition - mousePosition;

        mouseChange *= speed * Time.deltaTime;
        rotation += mouseChange;

        // if (rotation.x < -maxRotation) rotation.x = -maxRotation;
        // else if (rotation.y < -maxRotation) rotation.y = -maxRotation;
        // if (rotation.x > maxRotation) rotation.x = maxRotation;
        // else if (rotation.y > maxRotation) rotation.y = maxRotation;

        transform.localRotation = Quaternion.Euler(
            rotation.y,
            -rotation.x,
            0f
        );

        prevMousePosition = mousePosition;
    }
}
