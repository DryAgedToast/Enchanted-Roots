using UnityEngine;

namespace EnchantedRoots
{
    public class BSTNode
    {
        public int Value;
        public BSTNode Left;
        public BSTNode Right;
        public BSTNodeBehavior Behavior;  // Reference to the visual component
        public bool isInvasive;

        public BSTNode(int value, bool isInvasive = false)
        {
            Value = value;
            this.isInvasive = isInvasive;
            Left = null;
            Right = null;
        }
    }
}
