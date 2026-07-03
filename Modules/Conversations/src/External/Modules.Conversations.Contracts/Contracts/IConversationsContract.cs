using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Conversations.Contracts.Dtos;

namespace Modules.Conversations.Contracts.Contracts
{
    public interface IConversationsContract
    {
        public Task CreateConversation(CreateConversationDto request);
    }
}