using System;
using System.Collections.Generic;
using System.Text;

namespace VGT.Common.Models
{
    public class RegistrationSubmission
    {
        public string SubmissionType { get; set; }
        public Team TeamToRegister { get; set; }
        public Participant ParticipantToRegister { get; set; }
        public ToornamentToken TokenToUse { get; set; }
    }
}
