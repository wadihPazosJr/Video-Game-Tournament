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
using System.Web;
using System.Net.Mime;
using System.Net;

namespace VGT.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _Logger;
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _HttpClient;
        public RegistrationController(ILogger<AuthenticationController> logger, IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
            _HttpClient = new HttpClient();
            //Add default headers used by every Toornament call
            _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", "9xEAHpge9L4ApiyDFIzPtxtCbV81TVHsz9jOFGNfwpk");

        }

        [HttpPost("RegisterForTournament")]
        public async Task<ActionResult<string>> RegisterForTournament([FromBody, Required] RegistrationSubmission RegistrationInfo)
        {
            StringContent callParams = null;
            _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + RegistrationInfo.TokenToUse.AccessToken);
            if (RegistrationInfo.SubmissionType == "Single Participant") {
                Participant participantToRegister = RegistrationInfo.ParticipantToRegister;
                
                dynamic objectToPass = new
                {
                    name = participantToRegister.ParticipantFirstName + ' ' + participantToRegister.ParticipantLastName,
                    tournament_id = participantToRegister.GameChosen,
                    custom_fields = new {
                        full_name = new {
                            first_name = participantToRegister.ParticipantFirstName,
                            last_name = participantToRegister.ParticipantLastName
                        },
                        email = participantToRegister.ParticipantEMail,
                        phone_number = participantToRegister.PhoneOfParticipant,
                        in_game_name = participantToRegister.InGameName
                    },
                    type = "player"
                };
                    callParams = new StringContent(
                    JsonSerializer.Serialize(objectToPass),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);
            }
                HttpResponseMessage response = await _HttpClient.PostAsync("https://api.toornament.com/participant/v2/me/registrations", callParams);
                HttpContent responseBody = response.Content;
                string responseBodyAsString = await responseBody.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return Ok("Success!");
                else
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized("Registrations are closed");
                else
                    return Problem("Something bad happened");
            }
        }
}
