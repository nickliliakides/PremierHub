using PremierHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PremierHub.ViewModels
{
    public class MatchFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public int HomeTeam { get; set; }

        [Required]
        public int AwayTeam { get; set; }

        [Required]
        public int Stadium { get; set; }

        [Required]
        [ValidDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public string LiveScoreLink { get; set; }

        
        public string HomeTeamGoals { get; set; }

        
        public string AwayTeamGoals { get; set; }

        
        public string Highlights { get; set; }

        public IEnumerable<HomeTeam> HomeTeams { get; set; }
        public IEnumerable<AwayTeam> AwayTeams { get; set; }
        public IEnumerable<Stadium> Stadiums { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                return (Id != 0) ? "Update" : "Create";
            }

        }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(String.Format("{0} {1}", Date, Time));
        }
    }
}