using PremierHub.Models;
using PremierHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PremierHub.Controllers
{
    public class HomeController : Controller
    {
        public PremiersHubEntities db = new PremiersHubEntities();

        public ActionResult Index(string hometeam, string awayteam, string stadium)
        {
            //setting up drop down lists of all Teams and Stadiums for searching.

            var hometeamList = new List<string>();
            var awayteamList = new List<string>();
            var stadiumList = new List<string>();
            var teamQuery = from g in db.Matches orderby g.HomeTeam.Name select g.HomeTeam.Name;
            var teamQuery2 = from g in db.Matches orderby g.AwayTeam.Name select g.AwayTeam.Name;
            var stadiumQuery = from g in db.Matches orderby g.Stadium.Name select g.Stadium.Name;
            hometeamList.AddRange(teamQuery.Distinct());
            awayteamList.AddRange(teamQuery2.Distinct());
            stadiumList.AddRange(stadiumQuery.Distinct());
            ViewBag.hometeam = new SelectList(hometeamList);
            ViewBag.awayteam = new SelectList(awayteamList);
            ViewBag.stadium = new SelectList(stadiumList);

            //requesting to display all matches that haven't started yet ordered by the earliest starting.

            var upcomingMatches = from m in db.Matches.Where(g => g.DateTime >= DateTime.Now).OrderBy(g => g.DateTime).ToList() select m;

            // setting the search results.

            if (!string.IsNullOrEmpty(hometeam))
            {
                upcomingMatches = upcomingMatches.Where(s => s.HomeTeam.Name == hometeam);
            }

            if (!string.IsNullOrEmpty(awayteam))
            {
                upcomingMatches = upcomingMatches.Where(s => s.AwayTeam.Name == awayteam);
            }

            if (!string.IsNullOrEmpty(stadium))
            {
                upcomingMatches = upcomingMatches.Where(s => s.Stadium.Name == stadium);
            }

            return View(upcomingMatches);
        }

        public ActionResult Create()
        {
            var viewModel = new MatchFormViewModel
            {
                HomeTeams = db.HomeTeams.ToList(),
                AwayTeams = db.AwayTeams.ToList(),
                Stadiums = db.Stadia.ToList(),
                Heading = "Add a Match"

            };
            return View("MatchForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.HomeTeams = db.HomeTeams.ToList();
                viewModel.AwayTeams = db.AwayTeams.ToList();
                viewModel.Stadiums = db.Stadia.ToList();

                return View("MatchForm", viewModel);
            }

            var hometeam = db.HomeTeams.Single(t => t.Id == viewModel.HomeTeam);
            var awayteam = db.AwayTeams.Single(t => t.Id == viewModel.AwayTeam);
            var stadium = db.Stadia.Single(t => t.Id == viewModel.Stadium);

            var match = new Match
            {
                DateTime = viewModel.GetDateTime(),
                HomeTeam = hometeam,
                AwayTeam = awayteam,
                Stadium = stadium,
                LiveScoreLink = viewModel.LiveScoreLink,
                HomeTeamGoals = viewModel.HomeTeamGoals,
                AwayTeamGoals = viewModel.AwayTeamGoals,
                Highlights = viewModel.Highlights

            };

            db.Matches.Add(match);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MatchFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.HomeTeams = db.HomeTeams.ToList();
                viewModel.AwayTeams = db.AwayTeams.ToList();
                viewModel.Stadiums = db.Stadia.ToList();

                return View("MatchForm", viewModel);
            }

            var match = db.Matches.Single(m => m.Id == viewModel.Id);
            match.DateTime = viewModel.GetDateTime();
            match.HomeTeamId = viewModel.HomeTeam;
            match.AwayTeamId = viewModel.AwayTeam;
            match.StadiumId = viewModel.Stadium;
            match.LiveScoreLink = viewModel.LiveScoreLink;
            match.HomeTeamGoals = viewModel.HomeTeamGoals;
            match.AwayTeamGoals = viewModel.AwayTeamGoals;
            match.Highlights = viewModel.Highlights;
            match.HomeTeamGoals = viewModel.HomeTeamGoals;
            match.AwayTeamGoals = viewModel.AwayTeamGoals;
            match.Highlights = viewModel.Highlights;

            db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches.Find(id);

            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        public ActionResult FinishedDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Match match = db.Matches.Find(id);

            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        public ActionResult Finished()
        {

            var finishedMatches = from m in db.Matches.Where(g => g.DateTime < DateTime.Now).OrderByDescending(g => g.DateTime).ToList() select m;

            return View(finishedMatches);
        }

        public ActionResult Edit(int id)
        {
            var match = db.Matches.Single(g => g.Id == id);

            var viewModel = new MatchFormViewModel
            {
                Heading = "Edit Match",
                Id = match.Id,
                HomeTeams = db.HomeTeams.ToList(),
                AwayTeams = db.AwayTeams.ToList(),
                Stadiums = db.Stadia.ToList(),
                Date = match.DateTime.ToString("yyyy-MMM-dd ddd"),
                Time = match.DateTime.ToString("HH:mm"),
                HomeTeam = match.HomeTeamId,
                AwayTeam = match.AwayTeamId,
                Stadium = match.StadiumId,
                LiveScoreLink = match.LiveScoreLink,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                Highlights = match.Highlights


            };
            return View("MatchForm", viewModel);
        }

        public ActionResult Edit2(int id)
        {
            var match = db.Matches.Single(g => g.Id == id);

            var viewModel = new MatchFormViewModel
            {
                Heading = "Edit Completed Match",
                Id = match.Id,
                HomeTeams = db.HomeTeams.ToList(),
                AwayTeams = db.AwayTeams.ToList(),
                Stadiums = db.Stadia.ToList(),
                Date = match.DateTime.ToString("yyyy-MMM-dd ddd"),
                Time = match.DateTime.ToString("HH:mm"),
                HomeTeam = match.HomeTeamId,
                AwayTeam = match.AwayTeamId,
                Stadium = match.StadiumId,
                LiveScoreLink = match.LiveScoreLink,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                Highlights = match.Highlights
            };
            return View("MatchForm2", viewModel);
        }

        public ActionResult Delete(int? id)
        {
            Match match = db.Matches.Find(id);

            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Match match = db.Matches.Find(id);
            // db.Entry(vinyl).State = EntityState.Modified;
            db.Matches.Remove(match);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}