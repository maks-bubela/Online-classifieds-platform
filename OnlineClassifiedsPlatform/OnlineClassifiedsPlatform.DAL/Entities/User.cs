using OnlineClassifiedsPlatform.DAL.Interfaces;
using System.Collections.Generic;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Password { get; set; }

        public long RoleId { get; set; }
        public virtual Role Role { get; private set; }
        public bool IsDelete { get; set; }

        public string Salt { get; set; }
        public virtual ICollection<Goods> Goods { get; private set; }
            = new HashSet<Goods>();

    }
}
