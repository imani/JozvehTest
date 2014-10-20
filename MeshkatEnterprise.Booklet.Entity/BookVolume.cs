namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookVolume
    {
        public long StartParagraphId{ get; set; }
        public long EndParagraphId{ get; set; }
        public long VolumeId{ get; set; }
        public long VolumeNumber{ get; set; }
        public Book Book { get; set; }
        public int Pages { get; set; }
    }
}
