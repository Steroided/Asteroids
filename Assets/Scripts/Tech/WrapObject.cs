using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Tech
{
    public class WrapObject
    {
        public float BoundariesX { get; set; }
        public float BoundariesY { get; set; }
        private Transform objT { get; }

        public WrapObject(float BoundariesX_, float BoundariesY_, Transform objT_)
        {
            BoundariesX = BoundariesX_;
            BoundariesY = BoundariesY_;
            objT = objT_;
        }
        #region MAIN
        public void Boundaries()
        {
            if (objT.position.x > BoundariesX)
            {
                float newPosX = objT.position.x * -1 + 0.5f;
                objT.position = new Vector2(newPosX, objT.position.y);
            }
            if (objT.position.x < -BoundariesX)
            {
                float newPosX = objT.position.x * -1 - 0.5f;
                objT.position = new Vector2(newPosX, objT.position.y);
            }
            if (objT.position.y > BoundariesY)
            {
                float newPosY = objT.position.y * -1 + 0.5f;
                objT.position = new Vector2(objT.position.x, newPosY);
            }
            if (objT.position.y < -BoundariesY)
            {
                float newPosY = objT.position.y * -1 - 0.5f;
                objT.position = new Vector2(objT.position.x, newPosY);
            }
        }
        #endregion
    }
}