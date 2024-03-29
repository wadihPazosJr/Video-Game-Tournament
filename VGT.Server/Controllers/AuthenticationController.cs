﻿using System;
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
using System.Web;

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

        }

        [HttpGet("ToornamentAuthCallback")]
        public async Task<ActionResult> ToornamentAuthCallback()
        {
            //Update the tokens with valid values by calling the Toornament API client and getting them by trading the temporary code passed to this method.
            if (Request.Query.ContainsKey("code"))
            {
                //Get the temporary code given by Toornament from the query string
                string code = Request.Query["code"];
                if (Request.Query.ContainsKey("state"))
                {
                    string redirectURI = Request.Query["state"];
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("redirect_uri", "https://localhost:44366/Authentication/ToornamentAuthCallback"),
                        new KeyValuePair<string, string>("client_id", "79c3270e1d8eee3741075a46152tddg45yv44g0csoss84k4s8g0o0g4wkwws0wo88gccss4wo"),
                        new KeyValuePair<string, string>("client_secret", "1y7kqy74a7wgsoso0cg8084gw84c8cogwgcoco4w40c4co88wg"),
                    });
                    HttpResponseMessage response = await _HttpClient.PostAsync(
                        "https://api.toornament.com/oauth/v2/token",
                        content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        InternalToornamentToken toornamentToken = JsonSerializer.Deserialize<InternalToornamentToken>(responseContent);
                        return Redirect(redirectURI + "&AccessToken=" + toornamentToken.access_token + "&RefreshToken=" + toornamentToken.refresh_token + "&ExpiresIn=" + toornamentToken.expires_in.ToString());
                    }
                    else
                        return Ok("[" + responseContent + ",\n" + JsonSerializer.Serialize(response.RequestMessage.Headers) + ",\n" + await response.RequestMessage.Content.ReadAsStringAsync() + "]");

                }
                else
                    throw new ArgumentException("The CallbackAsync method expects to be called by Toornament and expects to have the login session id (State) passed back in the state request param");
            }else
                throw new ArgumentException("The CallbackAsync method expects to be called by Toornament and expects to have a temporary access code in the query string");
            
        }
    }
}
