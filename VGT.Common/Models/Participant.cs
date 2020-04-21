using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VGT.Common.Models
{
    public class Participant
    {
        [Required, Display(Name = "Game")]
        public string GameChosen { get; set; }
        [Required, Display(Name = "First Name")]
        public string ParticipantFirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string ParticipantLastName { get; set; }
        [Required, Display(Name = "eMail")]
        public string ParticipantEMail { get; set; }
        [Required, Display(Name = "Phone")]
        public string PhoneOfParticipant { get; set; }
        [Required, Display(Name = "In Game Name")]
        public string InGameName { get; set; }
    }
}
