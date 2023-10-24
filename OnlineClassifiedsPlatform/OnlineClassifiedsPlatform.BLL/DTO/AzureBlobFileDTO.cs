namespace OnlineClassifiedsPlatform.BLL.DTO
{
    public class AzureBlobFileDTO
    {
        public string FileName { get; set; }
        public long BlobTypeId { get; set; }
        public long ContainerId { get; set; }
        public long? JobId { get; set; }
        public long ImageMetadataId { get; set; }
    }
}
