using UnityEngine;

namespace StylizedGrassDemo
{
    public class MoveInCircle : MonoBehaviour
    {
        public float radius = 1f;
        public float speed = 1f;
        public Vector3 offset;
        public bool rotate = false;

        private Vector3 lastPos;
        private void Update()
        {
            Move();
        }

        void Move()
        {
            float x = Mathf.Sin(Time.realtimeSinceStartup * speed) * radius + offset.x;
            float y = this.transform.localPosition.y + offset.y;
            float z = Mathf.Cos(Time.realtimeSinceStartup * speed) * radius + offset.z;

            if (rotate)
            {
                lastPos = this.transform.position;
            }
            
            transform.localPosition = new Vector3(x, y, z);

            if (rotate)
            {
                Vector3 forward = (transform.position - lastPos).normalized;
                this.transform.forward = forward;
            }
        }
    }
}