namespace MeshkatEnterprise.Booklet.Entity
{
    public class Person
    {
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonNationalCode { get; set; }
        public string PersonIdentity { get; set; }
        public string PersonAddress { get; set; }

        public string PersonImagePath
        {
            get { return @"/Areas/Files/Images/" + PersonIdentity + ".png"; }
        }
    }
}