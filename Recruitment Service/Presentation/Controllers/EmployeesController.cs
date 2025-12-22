using Application.Employees.Commands.ChangeRole;
using Application.Employees.Commands.MoveToDepartment;
using Application.Employees.Commands.Register;
using Application.Employees.Queries.GetAll;
using Application.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiConstants.Permissions;

namespace Presentation.Controllers;

[Authorize]
[Route(Employees.Base)]
public class EmployeesController(ISender sender) : ApiController(sender)
{
    [Authorize(EmployeesPermissions.ManageEmployees)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Register(RegisterEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result =  await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(EmployeesPermissions.ManageEmployees)]
    [HttpPut(Employees.ChangeRole)]
    public async Task<ActionResult> ChangeRole(ChangeRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(EmployeesPermissions.ManageEmployees)]
    [HttpPut(Employees.MoveToDepartment)]
    public async Task<ActionResult> MoveToDepartment([FromBody] MoveToDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(EmployeesPermissions.Read)]
    [HttpGet(Id)]
    public async Task<ActionResult<GetEmployeeByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(EmployeesPermissions.Read)]
    [HttpGet(Employees.GetAll)]
    public async Task<ActionResult<GetAllEmployeesQueryResponse>> GetAll([FromQuery] GetAllEmployeesQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }
}
