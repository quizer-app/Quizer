using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.Get(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        _questionRepository.Delete(question);
        question.Delete();

        return (QuestionId)question.Id;
    }
}
