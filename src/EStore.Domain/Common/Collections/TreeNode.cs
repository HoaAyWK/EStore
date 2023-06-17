using System.Runtime.Serialization;

namespace EStore.Domain.Common.Collections;

public class TreeNode<T>
{
    private int _level = 0;
    public T Data { get; private set; }

    public List<TreeNode<T>> Children { get; set; } = new();

    [IgnoreDataMember]
    public TreeNode<T>? Parent { get; set; }

    public int Level
    {
        get
        {
            var node = this;

            while (node is not null && !node.IsRoot)
            {
                _level++;
                node = node.Parent;
            }

            return _level;
        }
    }

    [IgnoreDataMember]
    public TreeNode<T> Root
    {
        get
        {
            var root = this;

            while (root.Parent is not null)
            {
                root = root.Parent;
            }

            return root;
        }
    }

    [IgnoreDataMember]
    public bool IsRoot => Parent is null;

    public TreeNode(T data)
    {
        Data = data;
    }

    public void AddChild(TreeNode<T> child)
    {
        Children.Insert(0, child);
        child.Parent = this;
    }

    public IEnumerable<T> Flatten(bool includeSelf = true)
    {
        var list = includeSelf ? new[] { Data } : Enumerable.Empty<T>();

        if (Children.Count is not > 0)
        {
            return list;
        }

        return list.Union(Children.SelectMany(x => x.Flatten()));
    }
}
