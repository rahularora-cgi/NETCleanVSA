
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounts.Presentation.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(ICommandDispatcher _commandDispatcher, IQueryDispatcher _queryDispatcher) : ControllerBase
    {
        // GET: api/<AccountsController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllAccountsQuery();
            var result = await _queryDispatcher.QueryAsync<GetAllAccountsQuery, GetAccountListDto>(query);
            
            return result.ToActionResult(HttpContext);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAccountByIdQuery(id);
            var result = await _queryDispatcher.QueryAsync<GetAccountByIdQuery, GetAccountDto>(query);

            return result.ToActionResult(HttpContext);
        }

        // POST api/<AccountsController>
        [HttpPost]
        [ValidateWithFluent<CreateAccountCommand>]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateAccountCommand createAccountCommand)
        {
            var result = await _commandDispatcher.SendAsync<CreateAccountCommand, int>(createAccountCommand);

            return result.ToActionResult(
              value => CreatedAtAction(nameof(Get), new { id = value }, value),
              HttpContext);

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            //return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
        }

        // PUT api/<AccountsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateAccountCommand command)
        {
            // Ensure the ID from the route matches the command
            var updateCommand = command with { Id = id };
            
            var result = await _commandDispatcher.SendAsync<UpdateAccountCommand, Unit>(updateCommand);

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            return result.ToActionResult(() => NoContent(), HttpContext);

            //return NoContent();
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _commandDispatcher.SendAsync<DeleteAccountCommand, Unit>(
                new DeleteAccountCommand(id));

            //if (result.IsFailure)
            //    return result.ToProblemDetails(HttpContext);

            //return NoContent();

            return result.ToActionResult(() => NoContent(), HttpContext);

        }
    }
}
