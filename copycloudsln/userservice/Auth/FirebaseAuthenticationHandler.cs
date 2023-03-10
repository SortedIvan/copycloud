using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace userservice.Auth
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseApp firebaseApp;
        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, FirebaseApp _firebaseApp)
            : base(options, logger, encoder, clock) { firebaseApp = _firebaseApp; }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = "";
            bool cookieAuth = false;
            if (Context.Request.Cookies.ContainsKey("token"))
            {
                token = Context.Request.Cookies["token"];
                cookieAuth = true;
            }
            else
            {
                if (!Context.Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.NoResult();
                }
                string bearerToken = Context.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(bearerToken) && !bearerToken.StartsWith("Bearer "))
                {
                    return AuthenticateResult.Fail("Invalid token.");
                }

                token = bearerToken.Substring("Bearer ".Length);
            }

            try
            {
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(firebaseApp).VerifyIdTokenAsync(token);
                return AuthenticateResult.Success(new AuthenticationTicket(
                    new ClaimsPrincipal(new List<ClaimsIdentity>()
                    {
                    new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler))
                    }
                    ), JwtBearerDefaults.AuthenticationScheme));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }


        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {
                new Claim("id", claims["user_id"].ToString()),
                new Claim("email", claims["email"].ToString()),
                new Claim(ClaimTypes.Role, "User")
            };
        }

    }


}
