using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkoutDiaryMVC.Data;
using System.IO;
using System.Linq;
using System;

namespace WorkoutDiaryMVC.Controllers
{
    public class ExportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Pdf()
        {
            var workouts = _context.Workouts.ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Workout Report").FontSize(20).Bold();
                        col.Item().AlignRight().Text($"{DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(10).Italic();
                        col.Item().PaddingVertical(10);

                        int i = 1;
                        foreach (var workout in workouts)
                        {
                            col.Item().Text($"{i++}. {workout.Name} ({workout.WorkoutType})").FontSize(12).Bold();
                            col.Item().Text($"Date: {workout.Date:dd.MM.yyyy}").FontSize(10).Italic();
                            col.Item().PaddingBottom(5);
                            col.Item().Text($"Duration: {workout.DurationInMinutes} min").FontSize(10);
                            col.Item().PaddingBottom(5);

                            if (!string.IsNullOrEmpty(workout.Notes))
                            {
                                var lines = workout.Notes
                                    .Replace("è", "c").Replace("æ", "c")
                                    .Replace("ž", "z").Replace("š", "s")
                                    .Replace("ð", "d")
                                    .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

                                foreach (var line in lines)
                                {
                                    if (string.IsNullOrWhiteSpace(line))
                                        col.Item().PaddingVertical(5);
                                    else
                                        col.Item().Text(line.Trim()).FontSize(10);
                                }
                            }

                            col.Item().PaddingBottom(8);
                        }
                    });
                });
            });

            var pdfStream = new MemoryStream();
            document.GeneratePdf(pdfStream);
            pdfStream.Position = 0;

            return File(pdfStream, "application/pdf", "WorkoutDiary.pdf");
        }

        public IActionResult PdfSingle(int id)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id);
            if (workout == null)
                return NotFound();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().Text($"{workout.Name}").FontSize(20).Bold();
                        col.Item().AlignRight().Text($"{DateTime.Now:dd.MM.yyyy}").FontSize(10).Italic();
                        col.Item().PaddingVertical(10);
                        col.Item().Text($"{workout.Name} ({workout.WorkoutType})").FontSize(12).Bold();
                        col.Item().Text($"Date: {workout.Date:dd.MM.yyyy}").FontSize(10).Italic();
                        col.Item().PaddingBottom(5);
                        col.Item().Text($"Duration: {workout.DurationInMinutes} min").FontSize(10);
                        col.Item().PaddingBottom(5);

                        if (!string.IsNullOrEmpty(workout.Notes))
                        {
                            var lines = workout.Notes
                                .Replace("è", "c").Replace("æ", "c")
                                .Replace("ž", "z").Replace("š", "s")
                                .Replace("ð", "d")
                                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

                            foreach (var line in lines)
                            {
                                if (string.IsNullOrWhiteSpace(line))
                                    col.Item().PaddingVertical(5);
                                else
                                    col.Item().Text(line.Trim()).FontSize(10);
                            }
                        }

                        col.Item().PaddingBottom(8);
                    });
                });
            });

            var pdfStream = new MemoryStream();
            document.GeneratePdf(pdfStream);
            pdfStream.Position = 0;

            return File(pdfStream, "application/pdf", $"Workout_{workout.Id}.pdf");
        }
    }
}
