using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandupWatcher.DataAccess.Models
{
    public record SubscribedAuthors : Entity
    {
        public long ChatId { get; set; }
        public string StanduperName { get; set; }
    }
}
