using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Behaviors;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Models;
using Xunit;

namespace Quizer.Application.UnitTests;

public class CqrsBehaviorTests
{
    [Fact]
    public async Task Dispatcher_short_circuits_invalid_requests_before_handler_and_unit_of_work()
    {
        using var provider = CreateServices(services =>
        {
            services.AddScoped<TestCommandHandler>();
            services.AddScoped<IRequestHandler<TestCommand, ErrorOr<int>>>(provider => provider.GetRequiredService<TestCommandHandler>());
            services.AddScoped<IValidator<TestCommand>, TestCommandValidator>();
        });

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        var handler = provider.GetRequiredService<TestCommandHandler>();
        var unitOfWork = provider.GetRequiredService<TrackingUnitOfWork>();

        var result = await dispatcher.Send(new TestCommand(-1));

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == nameof(TestCommand.Value));
        Assert.Equal(0, handler.HandleCount);
        Assert.Equal(0, unitOfWork.SaveCount);
    }

    [Fact]
    public async Task Dispatcher_saves_unit_of_work_for_successful_commands()
    {
        using var provider = CreateServices(services =>
        {
            services.AddScoped<TestCommandHandler>();
            services.AddScoped<IRequestHandler<TestCommand, ErrorOr<int>>>(provider => provider.GetRequiredService<TestCommandHandler>());
            services.AddScoped<IValidator<TestCommand>, TestCommandValidator>();
        });

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        var handler = provider.GetRequiredService<TestCommandHandler>();
        var unitOfWork = provider.GetRequiredService<TrackingUnitOfWork>();

        var result = await dispatcher.Send(new TestCommand(42));

        Assert.False(result.IsError);
        Assert.Equal(42, result.Value);
        Assert.Equal(1, handler.HandleCount);
        Assert.Equal(1, unitOfWork.SaveCount);
    }

    [Fact]
    public async Task Dispatcher_does_not_save_unit_of_work_for_queries()
    {
        using var provider = CreateServices(services =>
        {
            services.AddScoped<TestQueryHandler>();
            services.AddScoped<IRequestHandler<TestQuery, ErrorOr<int>>>(provider => provider.GetRequiredService<TestQueryHandler>());
        });

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        var handler = provider.GetRequiredService<TestQueryHandler>();
        var unitOfWork = provider.GetRequiredService<TrackingUnitOfWork>();

        var result = await dispatcher.Send(new TestQuery(7));

        Assert.False(result.IsError);
        Assert.Equal(7, result.Value);
        Assert.Equal(1, handler.HandleCount);
        Assert.Equal(0, unitOfWork.SaveCount);
    }

    [Fact]
    public async Task Domain_event_publisher_resolves_registered_handlers()
    {
        using var provider = CreateServices(services =>
        {
            services.AddScoped<TestDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<TestDomainEvent>>(provider => provider.GetRequiredService<TestDomainEventHandler>());
        });

        var publisher = provider.GetRequiredService<IDomainEventPublisher>();
        var handler = provider.GetRequiredService<TestDomainEventHandler>();
        var domainEvent = new TestDomainEvent(123);

        await publisher.Publish(domainEvent);

        Assert.Equal(domainEvent, handler.HandledEvent);
    }

    [Fact]
    public async Task Dispatcher_reports_missing_handlers_with_request_and_response_types()
    {
        using var provider = CreateServices(_ => { });

        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.Send(new TestQuery(7)));

        Assert.Contains(typeof(TestQuery).FullName!, exception.Message);
        Assert.Contains(typeof(ErrorOr<int>).FullName!, exception.Message);
    }

    private static ServiceProvider CreateServices(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<TrackingUnitOfWork>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TrackingUnitOfWork>());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        configure(services);
        return services.BuildServiceProvider();
    }

    private sealed record TestCommand(int Value) : ICommand<ErrorOr<int>>;

    private sealed record TestQuery(int Value) : IQuery<ErrorOr<int>>;

    private sealed record TestDomainEvent(int Value) : IDomainEvent;

    private sealed class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(command => command.Value).GreaterThan(0);
        }
    }

    private sealed class TestCommandHandler : ICommandHandler<TestCommand, ErrorOr<int>>
    {
        public int HandleCount { get; private set; }

        public Task<ErrorOr<int>> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            HandleCount++;
            return Task.FromResult<ErrorOr<int>>(request.Value);
        }
    }

    private sealed class TestQueryHandler : IQueryHandler<TestQuery, ErrorOr<int>>
    {
        public int HandleCount { get; private set; }

        public Task<ErrorOr<int>> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            HandleCount++;
            return Task.FromResult<ErrorOr<int>>(request.Value);
        }
    }

    private sealed class TestDomainEventHandler : IDomainEventHandler<TestDomainEvent>
    {
        public TestDomainEvent? HandledEvent { get; private set; }

        public Task Handle(TestDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            HandledEvent = domainEvent;
            return Task.CompletedTask;
        }
    }

    private sealed class TrackingUnitOfWork : IUnitOfWork
    {
        public int SaveCount { get; private set; }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCount++;
            return Task.CompletedTask;
        }
    }
}
