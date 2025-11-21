using UnityEngine;

    public enum ResourceType
    {
        Wood,
        Stone,
        Metal,
        Coal,
        Water,
        Diesel,
        RawMeat,
        Food,
    }

    [System.Serializable]
    public class ResourceStack
    {
        public ResourceType type;
        public int amount;

        public ResourceStack(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
