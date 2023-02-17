using Asteroids.Tech;
using UnityEngine;

namespace Asteroids.Core
{
    public interface ISpaceObject
    {
        float _flySpeed { get; }
        float _lifeTime { get; }
        float _size { get; }
        Vector3 _flyDir { get; }
        WrapObject _boundariesW { get; }
        GameObject _object { get; }
        Transform _transform { get; }
        SpriteRenderer _spriteRenderer { get; }

    }
}