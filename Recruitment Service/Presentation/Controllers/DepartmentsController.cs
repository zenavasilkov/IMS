using Application.Departments.Commands.AddDepartment;
using Application.Departments.Commands.RenameDepartment;
using Application.Departments.Commands.UpdateDescription;
using Application.Departments.Queries.GetAll;
using Application.Departments.Queries.GetDepartmentById;
using Application.Departments.Queries.GetDepartmentByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiConstants.Permissions;

namespace Presentation.Controllers;

[Authorize]
[Route(Departments.Base)]
public class DepartmentsController(ISender sender) : ApiController(sender)
{
    [Authorize(DepartmentsPermissions.ManageDepartments)]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] AddDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(DepartmentsPermissions.ManageDepartments)]
    [HttpPut(Departments.Rename)]
    public async Task<ActionResult> Rename([FromBody] RenameDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(DepartmentsPermissions.ManageDepartments)]
    [HttpPut(Departments.UpdateDescription)]
    public async Task<ActionResult> UpdateDescription([FromBody] UpdateDescriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(DepartmentsPermissions.Read)]
    [HttpGet(Id)]
    public async Task<ActionResult<GetDepartmentByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error.ToString());
    }

    [Authorize(DepartmentsPermissions.Read)]
    [HttpGet(Departments.ByName)]
    public async Task<ActionResult<GetDepartmentByNameResponse>> GetByName([FromRoute] string name, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByNameQuery(name);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error.ToString());
    }

    [Authorize(DepartmentsPermissions.Read)]
    [HttpGet(Departments.GetAll)]
    public async Task<ActionResult<GetAllDepartmentsQueryResponse>> GetAll([FromQuery] GetAllDepartmentsQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }
}
