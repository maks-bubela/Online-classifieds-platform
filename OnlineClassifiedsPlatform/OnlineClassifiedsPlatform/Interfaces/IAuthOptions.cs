using OnlineClassifiedsPlatform.BLL.DTO;


namespace OnlineClassifiedsPlatform.Interfaces
{
    public interface IAuthOptions
    {
        string GetSymmetricSecurityKey(TokenSettingsDTO settingsDto);
    }
}
