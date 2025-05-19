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
                        col.Item().AlignCenter().Text("Workout Diary Report").FontSize(20).Bold();
                        col.Item().AlignRight().Text($"Generated on: {DateTime.Now}").FontSize(10);
                        col.Item().PaddingVertical(10);

                        foreach (var workout in workouts)
                        {
                            col.Item().Text($"{workout.Date.ToShortDateString()} - {workout.Name}")
                                .FontSize(12).Bold();
                            col.Item().Text(workout.Notes).FontSize(10);
                            col.Item().PaddingBottom(5);
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
