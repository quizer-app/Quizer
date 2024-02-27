using Quizer.Contracts.Authentication;
using Quizer.Contracts.Quiz;
using Quizer.Contracts.Question;

namespace Quizer.Seed;

internal class Program
{
    private static readonly QuizerApi _api = new();

    private static async Task Main(string[] args)
    {
        var login = await _api.Login(new LoginRequest("admin@quizer.com", "Test123#", true));

        if(login is null)
        {
            Console.WriteLine("Login failed");
            return;
        }

        var imageId = new Guid("91b6de42-f148-4d9e-5062-5bd4d9149600");

        var quizes = new List<CreateQuizRequest>
        {
            new CreateQuizRequest("Introduction to Science", "Test your basic knowledge of scientific concepts and principles in this introductory quiz.", imageId),
            new CreateQuizRequest("World History Trivia", "Explore the depths of history with questions spanning civilizations, events, and influential figures.", imageId),
            new CreateQuizRequest("Math Challenge", "Put your math skills to the test with challenging problems and brain-teasers covering various mathematical topics.", imageId),
            new CreateQuizRequest("Literature Classics", "Delve into the world of literature with questions about famous authors, novels, and literary movements.", imageId),
            new CreateQuizRequest("Geography Explorer", "Embark on a journey around the globe as you answer questions about countries, capitals, and geographic features.", imageId),
            new CreateQuizRequest("Music Trivia Madness", "From classical compositions to modern hits, this quiz covers a wide range of musical genres and artists.", imageId),
            new CreateQuizRequest("Movie Buff Challenge", "Test your knowledge of cinema with questions about iconic films, directors, actors, and memorable quotes.", imageId),
            new CreateQuizRequest("Sports Fanatic Showdown", "Put on your sports cap and tackle questions about your favorite teams, athletes, and sporting events.", imageId),
        };

        foreach(var quiz in quizes)
        {
            var quizIdResponse = await _api.AddQuiz(quiz);

            if(quizIdResponse is null)
            {
                continue;
            }

            var quizId = quizIdResponse.Id;

            for (int i = 1; i <= 10; i++)
            {
                var answer = new AnswerRequest($"Answer to question number {i}", true);
                var question = new CreateQuestionRequest($"Question number {i}", new List<AnswerRequest> { answer });
                await _api.AddQuestion(question, quizId);
            }
        }
    }
}
