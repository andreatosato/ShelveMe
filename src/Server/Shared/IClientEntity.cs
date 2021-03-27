namespace SharedModels
{
    public interface IClientEntity
    {
        public string PartitionKey { get; init; }
        public string RowKey { get; init; }
    }
}
