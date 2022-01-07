using System;
using UnityEngine;

namespace EcsCollision
{
    [Serializable]
    public struct SphereColliderComponent : IColliderComponent
    {
        public Vector3 center;
        [Min(0f)] 
        public float radius;
    }
}