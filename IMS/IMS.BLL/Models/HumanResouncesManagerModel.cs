namespace IMS.BLL.Models;

public class HumanResouncesManagerModel : UserModel
{  
    public required List<InternshipModel> Interships { get; set; } = [];
}
