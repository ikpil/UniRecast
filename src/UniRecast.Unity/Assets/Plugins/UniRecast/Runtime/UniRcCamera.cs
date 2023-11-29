using System;
using UnityEngine;

namespace UniRecast.Runtime
{
    public static class KeyModState
    {
        public const int None = 0;
        public const int Shift = 1;
        public const int Control = 2;
    }

    public class UniRcCamera : MonoBehaviour
    {
        private float camSens = 0.25f;
        private Vector3 lastMouse = new Vector3(255, 255, 255);

        // input
        private int _modState;

        private float _moveFront;
        private float _moveLeft;
        private float _moveBack;
        private float _moveRight;
        private float _moveUp;
        private float _moveDown;
        private float _moveAccel;


        private void Update()
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;

            UpdateKeyboard(Time.deltaTime);
            UpdateCamera(Time.deltaTime);
        }

        private void UpdateKeyboard(float dt)
        {
            var tempMoveFront = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1.0f : -1f;
            var tempMoveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1.0f : -1f;
            var tempMoveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1.0f : -1f;
            var tempMoveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1.0f : -1f;
            var tempMoveUp = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.PageUp) ? 1.0f : -1f;
            var tempMoveDown = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageDown) ? 1.0f : -1f;
            var tempMoveAccel = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1.0f : -1f;
            var tempControl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            _modState = 0;
            _modState |= tempControl ? (int)KeyModState.Control : (int)KeyModState.None;
            _modState |= 0 < tempMoveAccel ? (int)KeyModState.Shift : (int)KeyModState.None;

            //Logger.Information($"{_modState}");
            _moveFront = Mathf.Clamp(_moveFront + tempMoveFront * dt * 4.0f, 0, 1.0f);
            _moveLeft = Mathf.Clamp(_moveLeft + tempMoveLeft * dt * 4.0f, 0, 1.0f);
            _moveBack = Mathf.Clamp(_moveBack + tempMoveBack * dt * 4.0f, 0, 1.0f);
            _moveRight = Mathf.Clamp(_moveRight + tempMoveRight * dt * 4.0f, 0, 1.0f);
            _moveUp = Mathf.Clamp(_moveUp + tempMoveUp * dt * 4.0f, 0, 1.0f);
            _moveDown = Mathf.Clamp(_moveDown + tempMoveDown * dt * 4.0f, 0, 1.0f);
            _moveAccel = Mathf.Clamp(_moveAccel + tempMoveAccel * dt * 4.0f, 0, 1.0f);
        }

        private void UpdateCamera(float dt)
        {
            float scrollZoom = 0;

            float x = (_moveRight - _moveLeft) * 2.0f;
            float y = (_moveFront - _moveBack) + scrollZoom * 4.0f;
            scrollZoom = 0;

            var forward = transform.forward;
            var right = transform.right;

            // forward.y = 0f;
            // right.y = 0;
            Vector3 moveDirection = (forward * y + right * x).normalized;

            float keySpeed = 22.0f;
            if (0 < _moveAccel)
            {
                keySpeed *= _moveAccel * 2.0f;
            }

            transform.Translate(moveDirection * (keySpeed * dt), Space.World);
        }
    }
}