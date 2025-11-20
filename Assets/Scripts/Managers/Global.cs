using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class Global : MonoBehaviour
    {
        [SerializeField] private List<ColouredSprites> sprites;
        public static Dictionary<int, ColouredSprites> Sprites { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Sprites = new Dictionary<int, ColouredSprites>();

            foreach (ColouredSprites spritesData in sprites)
            {
                Sprites.Add(sprites.IndexOf(spritesData), spritesData);
            }
        }
    }
    
    [Serializable]
    public struct ColouredSprites : IEquatable<ColouredSprites>
    {
        public Color colour;
        public Sprite uiImage;
        public Sprite bodySprite;
        public Sprite turretSprite;
        public Sprite bulletSprite;

        public bool Equals(ColouredSprites other)
        {
            return Equals(uiImage, other.uiImage) && Equals(colour, other.colour) && Equals(bodySprite, other.bodySprite) && Equals(turretSprite, other.turretSprite) && Equals(bulletSprite, other.bulletSprite);
        }

        public override bool Equals(object obj)
        {
            return obj is ColouredSprites other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(uiImage, bodySprite, turretSprite, bulletSprite, colour);
        }
    }
}