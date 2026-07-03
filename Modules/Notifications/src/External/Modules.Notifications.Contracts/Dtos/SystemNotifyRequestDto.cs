namespace Modules.Notifications.Contracts.Dtos;

public enum TemplateType
{
    RejectionMessage = 1,
    ApprovalMessage = 2,
    OtpMessage = 3,
    ForgetPasswordMessage = 4,
}

public record SystemNotifyRequestDto(
    bool NotifyEmail,
    bool NotifyPhone,
    bool NotifySystem,
    TemplateType TemplateType,
    ICollection<string> ToEmails,
    ICollection<string> ToPhoneNumbers,
    IDictionary<string, string> KeyValuePairs);

public record NotifyUsersRequestDto(
    string Title,
    string Message,
    Guid[] ToUsersIds);