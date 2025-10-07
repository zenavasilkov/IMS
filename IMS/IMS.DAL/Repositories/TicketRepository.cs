using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Repositories
{
    public class TicketRepository(IMSDbContext context) : Repository<Ticket>(context), ITicketRepository
    {
    }
}
