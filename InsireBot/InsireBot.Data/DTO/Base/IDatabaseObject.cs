namespace InsireBot.Data
{
    public interface IDatabaseObject
    {
        int Id { get; set; }
        int Sequence { get; set; }
        bool IsDeleted { get; set; }
        bool IsNew { get; }
    }
}
