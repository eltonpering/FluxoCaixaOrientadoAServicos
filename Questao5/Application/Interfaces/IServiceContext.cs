using FluentValidation.Results;
using Questao5.Application.Services.Base;

namespace Questao5.Application.Interfaces
{
    public interface IServiceContext : IScopedService
    {
        ServiceContext AddNotification(string message);
        ServiceContext AddError(string message);
        ServiceContext AddEntityError(IReadOnlyCollection<ValidationFailure> errors);
        ServiceContext AddEntityError(string property, string error);
        ServiceContext ClearNotifications();
        ServiceContext ClearErrors();
        ServiceContext ClearEntityErrors();
        bool HasNotification();
        bool HasError();
        bool HasEntityError();
        IReadOnlyCollection<string> Notifications { get; }
        IReadOnlyCollection<string> Errors { get; }
        IReadOnlyDictionary<string, List<string>> EntityErrors { get; }
        bool Ok { get; }
    }
}
