namespace Users.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IQueryDispatcher _queryDispatcher, ICommandDispatcher _commandDispatcher) : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var result = await _queryDispatcher.QueryAsync<GetAllUsersQuery, IEnumerable<GetUserDto>>(
                new GetAllUsersQuery());

            return result.ToActionResult(HttpContext);
        }

        [HttpGet("GetAllPaginated")]
        [ProducesResponseType(typeof(PagedResult<GetUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaginated(
         [FromQuery] int pageNumber = 1,
         [FromQuery] int pageSize = 50,
         [FromQuery] bool? isActive = null)
        {
            var result = await _queryDispatcher.QueryAsync<GetAllUsersPaginatedQuery,PagedResult<GetUserDto>>(
                new GetAllUsersPaginatedQuery(pageNumber, pageSize, isActive));

            return result.ToActionResult(HttpContext);
        }

        // GET api/<UsersController>/by-id/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _queryDispatcher.QueryAsync<GetUserByIdQuery,GetUserDto>(
                new GetUserByIdQuery(id));

            return result.ToActionResult(HttpContext);
        }

        // GET api/<UsersController>/by-email/{email}
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _queryDispatcher.QueryAsync<GetUserByEmailQuery, GetUserDto>(
                  new GetUserByEmailQuery(email));

            return result.ToActionResult(HttpContext);
        }

        [HttpGet("{id}/roles")]
        [ProducesResponseType(typeof(IEnumerable<GetRoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles(Guid id)
        {
            var result = await _queryDispatcher.QueryAsync<GetRolesByUserIdQuery, IEnumerable<GetRoleDto>>(
                new GetRolesByUserIdQuery(id));

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            return result.ToActionResult(HttpContext);

            //return Ok(roles);
        }

        // POST api/<UsersController>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            var result = await _commandDispatcher.SendAsync<CreateUserCommand, Guid>(command);

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            //return CreatedAtAction(nameof(Get), new { email = command.Email }, result.Value);

            return result.ToActionResult(value => 
            CreatedAtAction(nameof(Get), new { id = value }, value), HttpContext);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserCommand command)
        {
            // Ensure the ID from the route matches the command
            var updateCommand = command with { Id = id };
            
            var result = await _commandDispatcher.SendAsync<UpdateUserCommand, Unit>(updateCommand);

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            //return NoContent();

            return result.ToActionResult(() => NoContent(), HttpContext);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _commandDispatcher.SendAsync<DeleteUserCommand, Unit>(
                new DeleteUserCommand(id));

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            //return NoContent();

            return result.ToActionResult(() => NoContent(), HttpContext);

        }
    }
}
