using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControls : MonoBehaviour
{
    [Header("Keybinds")]
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode sprintKey = KeyCode.LeftShift;
    public static KeyCode crouchKey = KeyCode.C;
    
    public static KeyCode sneakKey = KeyCode.C;
    public static KeyCode throwKey = KeyCode.E;

    public static KeyCode pauseKey = KeyCode.P;

    public static bool getJump() {
        if (Input.GetKey(jumpKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.buttonSouth.wasPressedThisFrame;
        } else {
            return false;
        }
    }

    public static bool getSprint() {
        if (Input.GetKey(sprintKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.leftStickButton.isPressed;
        } else {
            return false;
        }
    }

    public static bool getCrouch() {
        if (Input.GetKey(crouchKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.buttonEast.isPressed;
        } else {
            return false;
        }
    }

    public static bool getSneak() {
        if (Input.GetKey(sneakKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.rightStickButton.isPressed;
        } else {
            return false;
        }
    }

    public static bool getThrow() {
        if (Input.GetKey(throwKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.rightTrigger.isPressed;
        } else {
            return false;
        }
    }

    public static bool getPause() {
        if (Input.GetKeyDown(pauseKey)) {
            return true;
        } else if (Gamepad.current != null) {
            return Gamepad.current.startButton.isPressed;
        } else {
            return false;
        }
    }
}
