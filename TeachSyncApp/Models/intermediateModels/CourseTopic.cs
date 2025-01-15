namespace TeachSyncApp.Models.intermediateModels;

public class CourseTopic
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Courses Course { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
}