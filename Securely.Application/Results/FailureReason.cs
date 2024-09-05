namespace Securely.Application.Results;

public enum FailureReason
{
    None,
    NotFound,
    Expired,
    Gone,
    Unauthorized,
    BadRequest,
    InternalError,
    Database,
    Keyvault,
    Partial
}
