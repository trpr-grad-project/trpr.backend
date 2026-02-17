using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Conversations.Application.Interfaces
{
    public interface IAiChatService
    {
        public Task<ICollection<KeyValuePair<string, object?>>> SendMessageAsync(string message);
    }
}