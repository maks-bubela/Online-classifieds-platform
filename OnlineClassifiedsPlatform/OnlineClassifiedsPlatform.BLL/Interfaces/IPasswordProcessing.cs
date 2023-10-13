using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IPasswordProcessing
    {
        string GenerateSalt();
        string GetHashCode(string pass, string salt);
    }
}
