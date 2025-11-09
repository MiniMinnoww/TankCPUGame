using UnityEngine;

namespace Projectiles.Data
{
    [CreateAssetMenu(menuName = "Game/Basic Shot Projectile Data")]
    public class ShotProjectileData : ScriptableObject
    {
        public float damage;
        public bool destroyOnHit;
        public float speed;
    }
}

