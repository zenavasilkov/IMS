namespace IMS.BLL.Models;

public class MentorModel : UserModel
{  
    public required List<InternModel> Inters { get; set; } = [];

    public required HumanResouncesManagerModel HRManager { get; set; }

    public required List<InternshipModel> Internships { get; set; } = [];

    public required List<BoardModel> Boards { get; set; } = [];
}
