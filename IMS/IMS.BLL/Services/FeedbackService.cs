using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class FeedbackService(IFeedbackRepository repository, IMapper mapper)
    : Service<FeedbackModel, Feedback>(repository, mapper), IFeedbackService
{
}
