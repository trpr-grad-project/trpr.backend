namespace Modules.Conversations.Application.Dtos.Responses;

public class CursorPageDto<TItem, TCursor>
{
    public ICollection<TItem> Items { get; set; } = [];
    public TCursor? NextCursor { get; set; }
    public bool HasNextPage => NextCursor != null;
}

public class ConversationCursorPageDto : CursorPageDto<ConversationSummaryResponseDto, Guid?> { }

public class MessageCursorPageDto : CursorPageDto<MessageListItemDto, int?> { }
