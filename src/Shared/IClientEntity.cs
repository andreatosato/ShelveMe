namespace SharedModels
{
    public interface IClientEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
