using Ecommerce.EFCore;
using Ecommerce.Models.DTO;
using Ecommerce.Models.Utility;
using Ecommerce.Repositories.Repositories.Authenticetion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly EcommerceContext _ecommerceContext;
        public AuthenticationController(IAuthenticationRepository authenticationRepository, EcommerceContext ecommerceContext)
        {
            _authenticationRepository = authenticationRepository;  
            _ecommerceContext = ecommerceContext;   
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDTO)
        {
            try
            {
                var response = new UserLoginResponse();
                var userExists = await _ecommerceContext.UserProfiles.Where(x => (x.EmailId.ToLower() == loginDTO.Email.ToLower()) || (x.UserName.ToLower() == loginDTO.Email.ToLower())).FirstOrDefaultAsync();
                if (userExists != null)
                {
                    //var userdecryptpassword = Helper.DecryptString(Helper.SymmetricSecurityKey, userExists.Password);
                    if (userExists.Password != loginDTO.Password)
                    {
                        response.ErrorMessage += "Invalid Password.";
                    }
                    else
                    {
                        //var userRole = await _authenticationRepository.GetBy(x => x.PKRoleId == userExists.FKRoleId);
                        TokenModel tokenrespone = await _authenticationRepository.GenerateJwtToken(userExists.UserName, userExists.PkUserId);
                        //userExists.ModifiedDate = DateTime.Now;
                        //await _authenticationRepository.Update(userExists);
                        //await _loginHistoryRepository.AddLoginHistory(userExists.PKUserId, Request.Headers["User-Agent"], Helper.Admin);
                        response.ErrorMessage += null;
                        response.TokenModel = tokenrespone;
                    }


                }
                else
                {
                    response.ErrorMessage += "User Doesn't Exists.";
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
