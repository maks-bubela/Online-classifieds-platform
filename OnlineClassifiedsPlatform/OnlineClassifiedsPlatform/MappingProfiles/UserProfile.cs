using AutoMapper;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.Models;

namespace OnlineClassifiedsPlatform.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region To Model
            CreateMap<UserDTO, UserLoginModel>();
            CreateMap<UserDTO, UserRegistrationModel>();
            CreateMap<UserDTO, UserInfoModel>()
                .ForMember(x => x.Role, s => s.MapFrom(x => x.RoleName));

            #endregion

            #region To DTO
            CreateMap<UserLoginModel, UserDTO>();
            CreateMap<UserRegistrationModel, UserDTO>();
            #endregion
        }
    }
}
