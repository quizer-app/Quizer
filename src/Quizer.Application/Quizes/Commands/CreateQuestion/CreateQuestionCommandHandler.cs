﻿using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;

    public CreateQuestionCommandHandler(IQuizRepository quizRepository, IQuestionRepository questionRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        var result = Question.Create(
            request.UserId,
            (QuizId)quiz.Id,
            request.QuestionText,
            request.Answers
                .ConvertAll(a => Answer.Create(a.Text, a.IsCorrect).Value));

        if (result.IsError) return result.Errors;
        var question = result.Value;

        await _questionRepository.Add(question);

        quiz.AddQuestion(
            request.UserId,
            question);

        return (QuestionId)question.Id;
    }
}
