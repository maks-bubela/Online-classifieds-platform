using AutoMapper;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.BLL.MappingProfiles
{
    public class AzureStorageProfileBLL : Profile
    {
        public AzureStorageProfileBLL()
        {
            #region To DTO
            CreateMap<AzureBlob, AzureBlobDTO>();
            CreateMap<AzureBlobType, AzureBlobTypeDTO>();
            CreateMap<AzureStorageAccount, AzureStorageAccountDTO>();
            CreateMap<AzureBlobFile, AzureBlobFileDTO>();
            CreateMap<ImageMetadata, ImageMetadataDTO>();
            #endregion

            #region To Entity
            CreateMap<AzureBlobDTO, AzureBlob>();
            CreateMap<AzureBlobTypeDTO, AzureBlobType>();
            CreateMap<AzureStorageAccountDTO, AzureStorageAccount>();
            CreateMap<AzureBlobFileDTO, AzureBlobFile>();
            CreateMap<ImageMetadataDTO, ImageMetadata>();
            #endregion
        }
    }
}
