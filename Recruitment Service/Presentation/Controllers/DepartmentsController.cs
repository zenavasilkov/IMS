using Application.Departments.Commands.AddDepartment;
using Application.Departments.Commands.RenameDepartment;
using Application.Departments.Commands.UpdateDescription;
using Application.Departments.Queries.GetAll;
using Application.Departments.Queries.GetDepartmentById;
using Application.Departments.Queries.GetDepartmentByName;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiRoutes.ApiRoutes.Departments;

namespace Presentation.Controllers;

[Route(Base)]
public class DepartmentsController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] AddDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpPut(Departments.Rename)]
    public async Task<ActionResult> Rename([FromBody] RenameDepartmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpPut(Departments.UpdateDescription)]
    public async Task<ActionResult> UpdateDescription([FromBody] UpdateDescriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpGet(Id)]
    public async Task<ActionResult<GetDepartmentByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error.ToString());
    }

    [HttpGet(ByName)]
    public async Task<ActionResult<GetDepartmentByNameResponse>> GetByName([FromRoute] string name, CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByNameQuery(name);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error.ToString());
    }

    [HttpGet(Departments.GetAll)]
    public async Task<ActionResult<GetAllDepartmentsQueryResponse>> GetAll([FromQuery] GetAllDepartmentsQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }
}
