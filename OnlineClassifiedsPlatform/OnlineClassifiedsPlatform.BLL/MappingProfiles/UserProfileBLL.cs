using AutoMapper;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Entities;

namespace OnlineClassifiedsPlatform.BLL.MappingProfiles
{
    public class UserProfileBLL : Profile
    {
        public UserProfileBLL()
        {
            #region To DTO
            CreateMap<User, UserDTO>();
            #endregion

            #region from DTO
            CreateMap<UserDTO, User>();
            #endregion
        }
    }
}
