using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTwebAPI.Controllers
{
    [Route("api/{controller}")]
    public class AuthController : Controller
    {

        [HttpPost("token")]
        public IActionResult Token()
        {
            //string tokenString = "test";
            var header = Request.Headers["Authorization"];
            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim();
                var usernameAndPassword = Encoding.UTF8.GetString(Convert.FromBase64String(credValue)); //admin: pass
                var usernaneAndPass = usernameAndPassword.Split(":");

                //checked in DB if username and password exist;
                if (usernaneAndPass[0] == "Admin" && usernaneAndPass[1] == "pass")
                {            

                var claimsdata = new[] { new Claim(ClaimTypes.Name, usernaneAndPass[0]) };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hbfinjafnfjffrjifnkfnoorpoengelelnvnvinio934hhjsjskddddseee"));
                var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var token = new JwtSecurityToken(
                    issuer: "mysite.com",
                    audience: "mysite.com",
                    expires: DateTime.Now.AddMinutes(1),
                    claims: claimsdata,
                    signingCredentials: signInCred
                    );

                //this line of code undernet give us the actual token.
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenString);
                }
            }
            return BadRequest("Something is Wrong");

        }
    }
}