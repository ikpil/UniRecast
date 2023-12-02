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
        private Vector3 lastMousePosition;

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
            UpdateMouse(Time.deltaTime);
            UpdateKeyboard(Time.deltaTime);
            UpdateCamera(Time.deltaTime);
        }

        private void UpdateMouse(float dt)
        {
            // left button
            if (Input.GetMouseButton(0))
            {
            }

            // right button
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                // delta
                var tempMousePosition = Input.mousePosition;
                var delta = tempMousePosition - lastMousePosition;
                lastMousePosition = tempMousePosition;

                transform.Rotate(Vector3.up, delta.x * 0.25f, Space.World);
                transform.Rotate(Vector3.left, delta.y * 0.25f, Space.Self);
            }

            // wheel button
            if (Input.GetMouseButton(2))
            {
            }
        }

        private float GetKeyValue(KeyCode key1, KeyCode key2)
        {
            return Input.GetKey(key1) || Input.GetKey(key2) ? 1.0f : -1.0f;
        }

        private void UpdateKeyboard(float dt)
        {
            var tempMoveFront = GetKeyValue(KeyCode.W, KeyCode.UpArrow);
            var tempMoveLeft = GetKeyValue(KeyCode.A, KeyCode.LeftArrow);
            var tempMoveBack = GetKeyValue(KeyCode.S, KeyCode.DownArrow);
            var tempMoveRight = GetKeyValue(KeyCode.D, KeyCode.RightArrow);
            var tempMoveUp = GetKeyValue(KeyCode.Q, KeyCode.PageUp);
            var tempMoveDown = GetKeyValue(KeyCode.E, KeyCode.PageDown);
            var tempMoveAccel = GetKeyValue(KeyCode.LeftShift, KeyCode.RightShift);
            var tempControl = GetKeyValue(KeyCode.LeftControl, KeyCode.RightControl);

            _modState = 0;
            _modState |= 0 < tempControl ? (int)KeyModState.Control : (int)KeyModState.None;
            _modState |= 0 < tempMoveAccel ? (int)KeyModState.Shift : (int)KeyModState.None;

            //Logger.Information($"{_modState}");
            _moveFront = Mathf.Clamp(_moveFront + tempMoveFront * dt * 4.0f, 0, 2.0f);
            _moveLeft = Mathf.Clamp(_moveLeft + tempMoveLeft * dt * 4.0f, 0, 2.0f);
            _moveBack = Mathf.Clamp(_moveBack + tempMoveBack * dt * 4.0f, 0, 2.0f);
            _moveRight = Mathf.Clamp(_moveRight + tempMoveRight * dt * 4.0f, 0, 2.0f);
            _moveUp = Mathf.Clamp(_moveUp + tempMoveUp * dt * 4.0f, 0, 2.0f);
            _moveDown = Mathf.Clamp(_moveDown + tempMoveDown * dt * 4.0f, 0, 2.0f);
            _moveAccel = Mathf.Clamp(_moveAccel + tempMoveAccel * dt * 4.0f, 0, 2.0f);
        }

        private void UpdateCamera(float dt)
        {
            float scrollZoom = 0;

            float keySpeed = 22.0f;
            if (0 < _moveAccel)
            {
                keySpeed *= _moveAccel * 2.0f;
            }

            float x = (_moveRight - _moveLeft) * keySpeed * dt;
            float y = (_moveUp - _moveDown) * keySpeed * dt;
            float z = (_moveFront - _moveBack) * keySpeed * dt + scrollZoom * 2.0f;
            scrollZoom = 0;

            var forward = transform.forward;
            var right = transform.right;

            // forward.y = 0f;
            // right.y = 0;
            var translation = (forward * z + right * x);
            translation.y += y;


            transform.Translate(translation, Space.World);
        }
    }
}