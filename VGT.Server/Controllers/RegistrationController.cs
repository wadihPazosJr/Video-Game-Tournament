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

namespace VGT.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _Logger;
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _HttpClient;
        public RegistrationController(ILogger<AuthenticationController> logger,  IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
            _HttpClient = new HttpClient();
            //Add default headers used by every Toornament call
            _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", "9xEAHpge9L4ApiyDFIzPtxtCbV81TVHsz9jOFGNfwpk");

        }

        [HttpPost("RegisterSingleParticipantForTournament")]
        public async Task<ActionResult> RegisterSingleParticipantForTournament([FromBody,Required] Participant ParticipantToRegister)
        {
            throw new NotImplementedException();
        }

        [HttpPost("RegisterTeamForTournament")]
        public async Task<ActionResult> RegisterTeamForTournament([FromBody, Required] Team TeamToRegister)
        {
            throw new NotImplementedException();
        }
    }
}
