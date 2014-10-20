namespace MeshkatEnterprise.Booklet.Entity
{
    public class TreeNode
    {
        public long? ParentKey {get;set;}
        public string Title {get;set;}
        public long Key{get;set;}
        public bool IsLazy{get;set;}
        public bool HasChild{get;set;}
        public string Path { get; set; }
    }
}
