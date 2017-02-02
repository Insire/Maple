namespace Maple.Data
{
    public abstract class DatabaseObject: IDatabaseObject
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNew
        {
            get { return Id == default(int); }
        }
    }
}
