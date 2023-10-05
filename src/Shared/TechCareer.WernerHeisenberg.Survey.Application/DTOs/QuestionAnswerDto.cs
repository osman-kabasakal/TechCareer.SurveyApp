namespace TechCareer.WernerHeisenberg.Survey.Application.DTOs;

public class QuestionAnswerDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public byte DisplayOrder { get; set; }
    public string Text { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDate { get; set; }
    public int? ModifierUserId { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Deleted { get; set; }

    public QuestionDto QuestionDto { get; set; }
}