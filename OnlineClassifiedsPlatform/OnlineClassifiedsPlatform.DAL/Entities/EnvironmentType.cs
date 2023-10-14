using OnlineClassifiedsPlatform.DAL.Interfaces;
using System.Collections.Generic;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class EnvironmentType : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<BearerTokenSetting> BearerTokenSettings { get; private set; }
            = new HashSet<BearerTokenSetting>();
    }
}
