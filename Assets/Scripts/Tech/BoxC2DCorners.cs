using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Tech
{
    public class BoxC2DCorners
    {
        public Vector3 _bottomLeftCorner { get;private set; }
        public Vector3 _topRightCorner { get; private set; }
        public Transform _transform { get; private set; }
        private BoxCollider2D _boxCollider { get; }
        public BoxC2DCorners(Transform transform,BoxCollider2D boxCollider)
        {
            _transform= transform;
            _boxCollider= boxCollider;
            GetCC();
        }
        public void GetCC()
        {
            float top = _boxCollider.offset.y + _boxCollider.size.y / 2f;
            float btm = _boxCollider.offset.y - _boxCollider.size.y / 2f;
            float left = _boxCollider.offset.x - _boxCollider.size.x / 2f;
            float right = _boxCollider.offset.x + _boxCollider.size.x / 2f;

            _bottomLeftCorner = _transform.TransformVector(new Vector3(right, top, 0f));
            _topRightCorner = _transform.TransformVector(new Vector3(left, btm, 0f));

           
        }
    }
}