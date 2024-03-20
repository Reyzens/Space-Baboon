
using UnityEngine;

namespace SpaceBaboon
{
    public interface IPoolable
    {
        public bool IsActive { get; } // private set ?
        public void Activate(Vector2 pos, ObjectPool pool);
        public void Deactivate();
    }
}
