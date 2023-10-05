using Bogus;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;
using TechCareer.WernerHeisenberg.Survey.Domain.Enums;

namespace TechCareer.WernerHeisenberg.Survey.Tests.UnitTests.Core.Persistence.TheoryDatas;

public class QuestionDatas: TheoryData<Question>
{
    public QuestionDatas()
    {
        var faker= new Faker("tr");

        for (int i = 0; i < 5; i++)
        {
            Add(new Question
            {
                Text = faker.Lorem.Sentence(),
                QuestionType = faker.PickRandom<QuestionTypes>(),
                AnswerType = faker.PickRandom<AnswerTypes>(),
                IsPublic = faker.PickRandom<bool>(true, false),
                Deleted = faker.PickRandom<bool>(true, false),
                CreatorUserId = 1,
                CreateDate = DateTime.Now,
                ModifierUserId = 1,
                ModifyDate = DateTime.Now,
            });
        }   
    }
}