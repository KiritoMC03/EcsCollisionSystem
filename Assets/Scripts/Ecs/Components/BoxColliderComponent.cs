using System;
using UnityEngine;

namespace EcsCollision
{
    [Serializable]
    public struct BoxColliderComponent : IColliderComponent
    {
        public Vector3 position;
        public Vector3 size;
    }
}