﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Repositories;

namespace TattooRazorPages.Pages.ArtistPage
{
    public class ScheduleModel : PageModel
    {
        private readonly IScheduleRepository _context;

        public ScheduleModel(IScheduleRepository context)
        {
            _context = context;
        }

        public IList<Schedule> Schedule { get;set; } = default!;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("art_email") == null)
            {
                return RedirectToPage("/Login");
            }
            int artistId = HttpContext.Session.GetInt32("art_email").Value;
            if (_context.GetSchedules() != null)
            {
                Schedule = _context.GetListScheduleByArtistId(artistId);
            }
            return Page();
        }
        public IActionResult OnPost(DateTime searchDate)
        {
            if (HttpContext.Session.GetInt32("art_email") != null)
            {
                int artistId = HttpContext.Session.GetInt32("art_email").Value;
                Schedule = _context.GetScheduleInDayByArtistId(searchDate, artistId)
                    .ToList();

                if (!Schedule.Any())
                {
                    ViewData["Message"] = "No Schedule found for the selected date!";
                }
            }
            return Page();
        }
    }
}
