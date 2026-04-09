namespace Users.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ILoginService _loginUser) : ControllerBase
    {

        //Generate JWT token for user authentication and authorization
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _loginUser.HandleAsync(request);
            return result.ToActionResult();
        }
        
    }
}
