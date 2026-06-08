using Quizer.Application.Authentication.Commands.Register;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Quizes.Events;
using Quizer.Application.Quizes.Queries.GetQuizes;
using Xunit;

namespace Quizer.Application.UnitTests;

public class ApplicationBehaviorTests
{
    [Fact]
    public void Register_validator_rejects_short_password()
    {
        var validator = new RegisterCommandValidator();
        var command = new RegisterCommand("validuser", "user@example.com", "short");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Password");
    }

    [Fact]
    public void Get_quizes_validator_rejects_invalid_paging_values()
    {
        var validator = new GetQuizesValidator();
        var query = new GetQuizesQuery(null, null, null, 0, 101, null);

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "PageNumber");
        Assert.Contains(result.Errors, error => error.PropertyName == "PageSize");
    }

    [Fact]
    public void Quiz_deleted_handler_handles_quiz_deleted_domain_event()
    {
        var handlerType = typeof(QuizDeletedHandler);
        var domainEventHandlerType = typeof(IDomainEventHandler<Quizer.Domain.QuizAggregate.Events.QuizDeleted>);

        Assert.True(domainEventHandlerType.IsAssignableFrom(handlerType));
    }
}