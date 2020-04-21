using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VGT.Common.Models
{
    public class Team
    {
        [Required, Display(Name = "Team Name")]
        public string NameOfTeam { get; set; }

        [Required]
        public List<Participant> TeamMembers = new List<Participant>(5);
    }

}
