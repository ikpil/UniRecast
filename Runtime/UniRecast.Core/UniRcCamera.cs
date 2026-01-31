using UnityEngine;
using UnityEngine.InputSystem;

namespace UniRecast.Core
{
    public static class KeyModState
    {
        public const int None = 0;
        public const int Shift = 1;
        public const int Control = 2;
    }

    public class UniRcCamera : MonoBehaviour
    {
        private Vector2 _lastMousePosition;
        private GameObject _capsule;

        // input
        private int _modState;

        private float _moveFront;
        private float _moveLeft;
        private float _moveBack;
        private float _moveRight;
        private float _moveUp;
        private float _moveDown;
        private float _moveAccel;

        private float _scrollZoom;

        private void Update()
        {
            UpdateMouse(Time.deltaTime);
            UpdateKeyboard(Time.deltaTime);
            UpdateCamera(Time.deltaTime);
        }

        private void UpdateMouse(float dt)
        {
            if (null == Mouse.current)
                return;

            // left button
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Camera.main != null)
                {
                    var tempPosition = Mouse.current.position.ReadValue();
                    //Debug.Log($"mouse position - {tempPosition}");

                    var ray = Camera.main.ScreenPointToRay(tempPosition);
                    //Debug.Log($"ray - {ray}");

                    var wasHit = RaycastForMeshFilter(ray, out var hit);
                    if (wasHit)
                    {
                        //Debug.Log($"hit position - {hit.point}");
                        if (null == _capsule)
                        {
                            _capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        }

                        var capsuleCollider = _capsule.GetComponent<CapsuleCollider>();
                        _capsule.transform.position =
                            hit.point + (_capsule.transform.up * capsuleCollider.height * 0.5f);
                    }
                }
            }

            // right button
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _lastMousePosition = Mouse.current.position.ReadValue();
            }

            if (Mouse.current.rightButton.isPressed)
            {
                // delta
                var tempMousePosition = Mouse.current.position.ReadValue();
                var delta = tempMousePosition - _lastMousePosition;
                _lastMousePosition = tempMousePosition;

                transform.Rotate(Vector3.up, delta.x * 0.25f, Space.World);
                transform.Rotate(Vector3.left, delta.y * 0.25f, Space.Self);
            }

            // wheel button
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
            }

            // wheel scroll
            var scrollDelta = Mouse.current.scroll.ReadValue();
            if (0 != scrollDelta.y)
            {
                _scrollZoom = Mathf.Clamp(scrollDelta.y, -1, 1);
            }
            else
            {
                _scrollZoom = 0;
            }
        }

        private bool RaycastForMeshFilter(Ray ray, out RaycastHit hit)
        {
            hit = new RaycastHit();

            var meshFilters = FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
            if (meshFilters == null || 0 >= meshFilters.Length)
            {
                Debug.LogError("Mesh not found.");
                return false;
            }

            float closest = float.MaxValue;

            foreach (var meshFilter in meshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                var vertices = mesh.vertices;
                var triangles = mesh.triangles;

                //Debug.Log($"{meshFilter.gameObject.name}-{triangles.Length}");

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    Vector3 v0 = vertices[triangles[i]];
                    Vector3 v1 = vertices[triangles[i + 1]];
                    Vector3 v2 = vertices[triangles[i + 2]];

                    if (RayTriangleIntersection(ray, v0, v1, v2, out var tempHit))
                    {
                        float dist = Vector3.Distance(tempHit.point, ray.origin);
                        if (dist < closest)
                        {
                            closest = dist;
                            hit = tempHit;
                        }
                    }
                }
            }

            if (float.MaxValue > closest)
            {
                return true;
            }

            return false;
        }

        private bool RayTriangleIntersection(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out RaycastHit hit)
        {
            hit = new RaycastHit();

            Vector3 e1 = v1 - v0;
            Vector3 e2 = v2 - v0;
            Vector3 h = Vector3.Cross(ray.direction, e2);
            float a = Vector3.Dot(e1, h);

            if (a > -float.Epsilon && a < float.Epsilon)
                return false;

            float f = 1.0f / a;
            Vector3 s = ray.origin - v0;
            float u = f * Vector3.Dot(s, h);

            if (u < 0.0f || u > 1.0f)
                return false;

            Vector3 q = Vector3.Cross(s, e1);
            float v = f * Vector3.Dot(ray.direction, q);

            if (v < 0.0f || u + v > 1.0f)
                return false;

            float t = f * Vector3.Dot(e2, q);
            if (t > float.Epsilon)
            {
                hit.point = ray.origin + ray.direction * t;
                hit.normal = Vector3.Cross(e1, e2).normalized;
                hit.distance = t;
                return true;
            }

            return false;
        }

        private float GetKeyValue(Key key1, Key key2)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return -1.0f;

            return IsPressed(keyboard, key1) || IsPressed(keyboard, key2)
                ? 1.0f
                : -1.0f;
        }

        private static bool IsPressed(Keyboard keyboard, Key key)
        {
            return keyboard[key].isPressed;
        }


        private void UpdateKeyboard(float dt)
        {
            var tempMoveFront = GetKeyValue(Key.W, Key.UpArrow);
            var tempMoveLeft = GetKeyValue(Key.A, Key.LeftArrow);
            var tempMoveBack = GetKeyValue(Key.S, Key.DownArrow);
            var tempMoveRight = GetKeyValue(Key.D, Key.RightArrow);
            var tempMoveUp = GetKeyValue(Key.Q, Key.PageUp);
            var tempMoveDown = GetKeyValue(Key.E, Key.PageDown);
            var tempMoveAccel = GetKeyValue(Key.LeftShift, Key.RightShift);
            var tempControl = GetKeyValue(Key.LeftCtrl, Key.RightCtrl);

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
            float keySpeed = 22.0f;
            if (0 < _moveAccel)
            {
                keySpeed *= _moveAccel * 2.0f;
            }

            float x = (_moveRight - _moveLeft) * keySpeed * dt;
            float y = (_moveUp - _moveDown) * keySpeed * dt;
            float z = (_moveFront - _moveBack) * keySpeed * dt + _scrollZoom * 2.0f;

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