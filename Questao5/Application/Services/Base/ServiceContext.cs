using FluentValidation.Results;
using MediatR;
using Questao5.Application.Interfaces;

namespace Questao5.Application.Services.Base
{
    public class ServiceContext : IServiceContext
    {
        public ServiceContext()
        {
            _notifications = new List<string>();
            _errors = new List<string>();
            _entityErrors = new Dictionary<string, List<string>>();
        }

        private readonly List<string> _notifications;
        public IReadOnlyCollection<string> Notifications => _notifications.AsReadOnly();

        private readonly List<string> _errors;
        public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

        private readonly Dictionary<string, List<string>> _entityErrors;
        public IReadOnlyDictionary<string, List<string>> EntityErrors => _entityErrors;

        public bool Ok => !HasEntityError() && !HasError();

        public ServiceContext AddError(string message)
        {
            _errors.Add(message);
            return this;
        }

        public ServiceContext AddEntityError(string property, string error)
        {
            if (!_entityErrors.TryGetValue(property, out var errorsList))
            {
                errorsList = new List<string>();
                _entityErrors[property] = errorsList;
            }

            if (errorsList.Contains(error)) return this;
            errorsList.Add(error);

            return this;
        }

        public ServiceContext AddEntityError(IReadOnlyCollection<ValidationFailure> errors)
        {
            foreach (var error in errors)
            {
                AddEntityError(error.PropertyName, error.ErrorMessage);
            }

            return this;
        }

        public ServiceContext AddNotification(string message)
        {
            _notifications.Add(message);
            return this;
        }

        public ServiceContext ClearErrors()
        {
            _errors.Clear();
            return this;
        }

        public ServiceContext ClearEntityErrors()
        {
            _entityErrors.Clear();
            return this;
        }

        public ServiceContext ClearNotifications()
        {
            _notifications.Clear();
            return this;
        }

        public bool HasError() => _errors.Any();

        public bool HasNotification() => _notifications.Any();

        public bool HasEntityError() => _entityErrors.Any();
    }
}
