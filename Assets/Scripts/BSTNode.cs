public class BSTNode
{
    //variables held for the node prefab
    //value = value of the node (number)
    //bstnode left and right; helps node keep track of what node is left and right of itself
    public int Value;
    public BSTNode Left;
    public BSTNode Right;

    public BSTNode(int value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}
