using Leopotam.Ecs;
using UnityEngine;

namespace EcsCollision.Editor
{
    public sealed class DrawCollidersSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SphereColliderComponent> _sphereColliderFilter = null;
        private readonly EcsFilter<BoxColliderComponent> _boxColliderFilter = null;
        
        public void Run()
        {
            for (var i = 0; i < _sphereColliderFilter.GetEntitiesCount(); i++)
            {
                ref var current = ref _sphereColliderFilter.Get1(i);
                var offset = _sphereColliderFilter.GetEntity(i).Get<PositionComponent>().value;
                Gizmos.DrawWireSphere(current.center + offset, current.radius);
            }

            for (var i = 0; i < _boxColliderFilter.GetEntitiesCount(); i++)
            {
                ref var temp =  ref _boxColliderFilter.Get1(i);
                var position = temp.position + _boxColliderFilter.GetEntity(i).Get<PositionComponent>().value;
                var rotation = Quaternion.Euler(_boxColliderFilter.GetEntity(i).Get<RotationComponent>().value);


                var rotationMatrix = Matrix4x4.TRS(position, rotation.normalized, Vector3.one);
                Gizmos.matrix = rotationMatrix;
                Gizmos.DrawCube(position, temp.size);
            }
        }
    }
}