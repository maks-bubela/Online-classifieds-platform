using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private OnlineClassifiedsPlatformContext _ctx;
        private readonly IMapper _mapper;
        private const long ID_NOT_FOUND = 0;
        public AzureStorageService(IMapper mapper, OnlineClassifiedsPlatformContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(OnlineClassifiedsPlatformContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
        }

        #region Predicated
        public Expression<Func<AzureBlob, bool>> CreateActiveAzureBlobPredicate(string containerName)
        {
            Expression<Func<AzureBlob, bool>> predicate = x => x.ContainerName == containerName;
            return predicate;
        }

        public Expression<Func<AzureStorageAccount, bool>> CreateActiveAzureSorageAccountPredicate(string accountName)
        {
            Expression<Func<AzureStorageAccount, bool>> predicate = x => x.StorageAccount == accountName;
            return predicate;
        }

        public Expression<Func<AzureBlobType, bool>> CreateActiveBlobTypePredicate(string blobType)
        {
            Expression<Func<AzureBlobType, bool>> predicate = x => x.BlobType == blobType;
            return predicate;
        }
        #endregion

        #region AddEntity Methods
        public async Task<long> AddNewAzureBlobTypeAsync(AzureBlobTypeDTO dto)
        {
            if (dto == null) throw new EntityArgumentNullException(nameof(dto));
            if (dto.BlobType == null) throw new EntityArgumentNullException(nameof(dto.BlobType));

            var azureBlobType = _mapper.Map<AzureBlobType>(dto);
            await _ctx.Set<AzureBlobType>().AddAsync(azureBlobType);
            await _ctx.SaveChangesAsync();
            if (azureBlobType.Id > ID_NOT_FOUND)
                return azureBlobType.Id;
            throw new FailedAddToDatabaseException();
        }

        public async Task<long> AddNewAzureStorageAccountAsync(AzureStorageAccountDTO dto)
        {
            if (dto == null) throw new EntityArgumentNullException(nameof(dto));
            if (dto.StorageAccount == null) throw new EntityArgumentNullException(nameof(dto.StorageAccount));

            var azureStorageAccount = _mapper.Map<AzureStorageAccount>(dto);
            await _ctx.Set<AzureStorageAccount>().AddAsync(azureStorageAccount);
            await _ctx.SaveChangesAsync();
            if (azureStorageAccount.Id > ID_NOT_FOUND)
                return azureStorageAccount.Id;
            throw new FailedAddToDatabaseException();
        }
        public async Task<long> AddNewBlobFileAsync(AzureBlobFileDTO dto)
        {
            if (dto == null) throw new EntityArgumentNullException(nameof(dto));
            if (dto.FileName == null) throw new EntityArgumentNullException(nameof(dto.FileName));
            if (dto.BlobTypeId <= ID_NOT_FOUND) throw new EntityArgumentNullException(nameof(dto.BlobTypeId));
            if (dto.ContainerId <= ID_NOT_FOUND) throw new EntityArgumentNullException(nameof(dto.ContainerId));

            var blobFile = _mapper.Map<AzureBlobFile>(dto);
            await _ctx.Set<AzureBlobFile>().AddAsync(blobFile);
            await _ctx.SaveChangesAsync();
            if (blobFile.Id > ID_NOT_FOUND)
                return blobFile.Id;
            throw new FailedAddToDatabaseException();
        }

        public async Task<long> AddImageMetadataAsync(ImageMetadataDTO dto)
        {
            if (dto == null) throw new EntityArgumentNullException(nameof(dto));
            if (dto.Width <= 0) throw new EntityArgumentNullException(nameof(dto.Width));
            if (dto.Height <= 0) throw new EntityArgumentNullException(nameof(dto.Height));
            if (dto.ImageId <= 0) throw new EntityArgumentNullException(nameof(dto.ImageId));

            var imageMetadata = _mapper.Map<ImageMetadata>(dto);
            await _ctx.Set<ImageMetadata>().AddAsync(imageMetadata);
            await _ctx.SaveChangesAsync();
            if (imageMetadata.Id > ID_NOT_FOUND)
                return imageMetadata.Id;
            throw new FailedAddToDatabaseException();
        }
        #endregion

        #region IsExist Methods
        public async Task<bool> IsContainerNameExistAsync(string containerName)
        {
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var dbAzureBlob = await _ctx.Set<AzureBlob>()
                .Where(CreateActiveAzureBlobPredicate(containerName)).SingleOrDefaultAsync();
            return dbAzureBlob != null;
        }

        public async Task<bool> IsAccountExistAsync(string accountName)
        {
            if (accountName == null) throw new ArgumentNullException(nameof(accountName));

            var dbAzureStorageAccount = await _ctx.Set<AzureStorageAccount>()
                .Where(CreateActiveAzureSorageAccountPredicate(accountName)).SingleOrDefaultAsync();
            return dbAzureStorageAccount != null;
        }

        public async Task<bool> IsBlobTypeExistAsync(string blobType)
        {
            if (blobType == null) throw new ArgumentNullException(nameof(blobType));

            var dbAzureBlobType = await _ctx.Set<AzureBlobType>()
                .Where(CreateActiveBlobTypePredicate(blobType)).SingleOrDefaultAsync();
            return dbAzureBlobType != null;
        }
        #endregion

        #region GetEntity Methods
        public async Task<AzureBlobDTO> GetAzureBlobAsync(string ContainerName)
        {
            if (ContainerName == null) throw new ArgumentNullException(nameof(ContainerName));

            var azureBlob = await _ctx.Set<AzureBlob>()
                .Where(o => o.ContainerName == ContainerName).SingleOrDefaultAsync();
            return _mapper.Map<AzureBlobDTO>(azureBlob);
        }

        public async Task<AzureBlobTypeDTO> GetAzureBlobTypeAsync(string BlobType)
        {
            if (BlobType == null) throw new ArgumentNullException(nameof(BlobType));

            var azureBlobType = await _ctx.Set<AzureBlobType>()
                .Where(CreateActiveBlobTypePredicate(BlobType)).SingleOrDefaultAsync();
            return _mapper.Map<AzureBlobTypeDTO>(azureBlobType);
        }

        public async Task<AzureStorageAccountDTO> GetAzureStorageAccountAsync(string StorageAccount)
        {
            if (StorageAccount == null) throw new ArgumentNullException(nameof(StorageAccount));

            var azureStorageAccount = await _ctx.Set<AzureStorageAccount>()
                .Where(CreateActiveAzureSorageAccountPredicate(StorageAccount)).SingleOrDefaultAsync();
            return _mapper.Map<AzureStorageAccountDTO>(azureStorageAccount);
        }

        public async Task<AzureBlobFileDTO> GetAzureBlobFileByNameAsync(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var azureBlobFile = await _ctx.Set<AzureBlobFile>()
                .Where(o => o.FileName == fileName).Include(x => x.ImageMetadata).SingleOrDefaultAsync();
            return _mapper.Map<AzureBlobFileDTO>(azureBlobFile);
        }
        public async Task<ImageMetadataDTO> GetImageMetadataAsync(long Id)
        {
            if (Id <= ID_NOT_FOUND) throw new ArgumentNullException(nameof(Id));

            var imageMetadata = await _ctx.Set<ImageMetadata>()
                .Where(x => x.Id == Id).SingleOrDefaultAsync();
            return _mapper.Map<ImageMetadataDTO>(imageMetadata);
        }
        #endregion

        #region GetEntityId
        public async Task<long> GetAzureBlobIdAsync(string ContainerName)
        {
            if (ContainerName == null) throw new ArgumentNullException(nameof(ContainerName));

            var azureBlob = await _ctx.Set<AzureBlob>()
                .Where(CreateActiveAzureBlobPredicate(ContainerName)).SingleOrDefaultAsync();
            return azureBlob.Id;
        }

        public async Task<long> GetAzureBlobTypeIdAsync(string BlobType)
        {
            if (BlobType == null) throw new ArgumentNullException(nameof(BlobType));

            var azureBlobType = await _ctx.Set<AzureBlobType>()
                .Where(CreateActiveBlobTypePredicate(BlobType)).SingleOrDefaultAsync();
            return azureBlobType.Id;
        }
        #endregion

        public async Task<bool> IsAllowedFileTypeAsync(string containerName, string fileType)
        {
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));
            if (containerName == null) throw new ArgumentNullException(nameof(fileType));

            var azureBlobType = await _ctx.Set<AzureBlobType>()
                .Where(CreateActiveBlobTypePredicate(fileType)).SingleOrDefaultAsync();
            return azureBlobType != null;
        }
    }
}
