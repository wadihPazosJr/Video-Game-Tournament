using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VGT.Common;
using VGT.Server.CustomExceptions;
using VGT.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace VGT.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _Logger;
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _HttpClient;
        public AuthenticationController(ILogger<AuthenticationController> logger,  IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
            _HttpClient = new HttpClient();
            //Add default headers used by every Toornament call
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", "9xEAHpge9L4ApiyDFIzPtxtCbV81TVHsz9jOFGNfwpk");

        }

        [HttpPost("ToornamentAuthCallback")]
        public async Task<ActionResult> ToornamentAuthCallback()
        {
            //Update the tokens with valid values by calling the Toornament API client and getting them by trading the temporary code passed to this method.
            if (Request.Query.ContainsKey("code"))
            {
                //Get the temporary code given by Toornament from the query string
                string code = Request.Query["code"];
                if (Request.Query.ContainsKey("state"))
                {
                    string loginSessionID = Request.Query["state"];
                    HttpResponseMessage response = await _HttpClient.PostAsync(
                        "https://api.toornament.com/oauth/v2/token",
                        new StringContent(
                            string.Format(
                                "grant_type=authorization_code&client_id={0}&client_secret={1}&redirect_uri={2}&code={3}",
                                "79c3270e1d8eee3741075a46152tddg45yv44g0csoss84k4s8g0o0g4wkwws0wo88gccss4wo",
                                "1y7kqy74a7wgsoso0cg8084gw84c8cogwgcoco4w40c4co88wg",
                                "https://localhost:44392?loginsession=" +loginSessionID,
                                code
                             ),
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"));
                }
                else
                    throw new ArgumentException("The CallbackAsync method expects to be called by Toornament and expects to have the login session id (State) passed back in the state request param");
            }
            throw new ArgumentException("The CallbackAsync method expects to be called by Toornament and expects to have a temporary access code in the query string");

        }

        [HttpPost("LoginToToornament")]
        public async Task<ActionResult> LoginToToornament([FromQuery,Required]string LoginSessionID)
        {
            HttpResponseMessage response = await _HttpClient.PostAsync(
                "https://account.toornament.com/oauth2/authorize",
                new StringContent(
                    string.Format(
                        "response_type=code&client_id={0}&redirect_uri={1}&scope=participant:manage_registrations&state={2}",
                        "79c3270e1d8eee3741075a46152tddg45yv44g0csoss84k4s8g0o0g4wkwws0wo88gccss4wo",
                        "https://localhost:44366/Authentication/ToornamentAuthCallback",
                        LoginSessionID
                     ),
                Encoding.UTF8,
                "application/x-www-form-urlencoded"));
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode, response.Content.ReadAsStringAsync());
            }
        }
    }
}
