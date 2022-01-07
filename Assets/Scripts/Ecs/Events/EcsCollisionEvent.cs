namespace EcsCollision
{
    public struct EcsCollisionEvent<T> where T: IColliderComponent
    {
        public T a;
        public T b;
    }
}