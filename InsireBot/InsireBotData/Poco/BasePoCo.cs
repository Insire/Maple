namespace InsireBot.Data
{
    public abstract class BasePoCo
    {
        public int Id { get; set; }

        public bool IsNew
        {
            get
            {
                return Id == default(int);
            }
        }

        public bool IsDeleted { get; set; }
    }
}
