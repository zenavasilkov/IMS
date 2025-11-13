using Application.Employees.Commands.ChangeRole;
using Application.Employees.Commands.MoveToDepartment;
using Application.Employees.Commands.Register;
using Application.Employees.Queries.GetAll;
using Application.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiRoutes.ApiRoutes.Employees;

namespace Presentation.Controllers;

[Route(Base)]
public class EmployeesController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Regester(RegisterEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result =  await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpPut(Employees.ChangeRole)]
    public async Task<ActionResult> ChangeRole(ChangeRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpPut(Employees.MoveToDepartment)]
    public async Task<ActionResult> MoveToDepartment([FromBody] MoveToDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpGet(Id)]
    public async Task<ActionResult<GetEmployeeByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpGet(Employees.GetAll)]
    public async Task<ActionResult<GetAllEmployeesQueryResponse>> GetAll([FromQuery] GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result?.Error.ToString());
    }
}
