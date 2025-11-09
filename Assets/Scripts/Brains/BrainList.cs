using System;
using System.Collections.Generic;
using UnityEngine;

namespace Brains
{
    [CreateAssetMenu(menuName = "Game/BrainList")]
    public class BrainList : ScriptableObject
    {
        public BrainPair[] brains;

        private readonly Dictionary<string, Brain> cachedBrains = new();

        public void ReloadCache() 
        {
            cachedBrains.Clear();

            foreach (BrainPair pair in brains) 
                cachedBrains.Add(pair.brainName, pair.brain);
        }

        public string[] GetBrainNames() => new List<string>(cachedBrains.Keys).ToArray();
    
        public Brain GetBrainFromName(string brainName)
        {
            if (cachedBrains.TryGetValue(brainName, out Brain brain)) return brain;
            Debug.LogError($"Tried to get brain {brainName} but it wasn't in cache!");
            return null;

        }
    }

    [Serializable]
    public struct BrainPair
    {
        public string brainName;
        [SerializeReference, SubclassSelector] public Brain brain;
    }
}