using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkoutDiaryMVC.Data;

namespace WorkoutDiaryMVC.Controllers
{
    public class ExportController : Controller
    {
        private readonly WorkoutRepository _repo;

        public ExportController(WorkoutRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Pdf()
        {
            var workouts = _repo.GetAll();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Content().Column(col =>
                    {
                        // Naslov
                        col.Item().AlignCenter().Text("Workout Report")
                            .FontSize(20).Bold();

                        // Datum generiranja
                        col.Item().AlignRight().Text($"Generated on: {DateTime.Now:dd.MM.yyyy HH:mm}")
                            .FontSize(10).Italic();

                        col.Item().PaddingVertical(10);
                        int i = 1;
                        foreach (var workout in workouts)
                        {
                            // Naslov + tip
                            col.Item().Text($"{i++}. {workout.Name} ({workout.WorkoutType})")
                                .FontSize(12).Bold();

                            // Datum
                            col.Item().Text($"Date: {workout.Date:dd.MM.yyyy}")
                                .FontSize(10).Italic();

                            // Trajanje
                            col.Item().Text($"Duration: {workout.DurationInMinutes} min")
                                .FontSize(10);

                            // Bilješke ako ih ima
                            if (!string.IsNullOrWhiteSpace(workout.Notes))
                            {
                                col.Item().Text(workout.Notes).FontSize(10);
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
    }
}
