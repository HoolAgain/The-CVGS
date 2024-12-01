using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class ReviewService
    {
        private readonly VaporDbContext _context;

        public ReviewService (VaporDbContext context)
        {
            _context = context;
        }

        public List<ReviewReport> GetReviewReportData()
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .Select(r => new ReviewReport
                {
                    UserName = r.User.UserName,
                    GameName = r.Game.Name,
                    ReviewDescription = r.ReviewText
                })
                .ToList();
        }

        public byte[] GenerateReviewReportExcel(List<ReviewReport> reviews)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Review Report");

                worksheet.Cells[1, 1].Value = "Member Username";
                worksheet.Cells[1, 2].Value = "Game Name";
                worksheet.Cells[1, 3].Value = "Review Description";

                for (int i = 0; i < reviews.Count; i++)
                {
                    var review = reviews[i];
                    worksheet.Cells[i + 2, 1].Value = review.UserName;
                    worksheet.Cells[i + 2, 2].Value = review.GameName;
                    worksheet.Cells[i + 2, 3].Value = review.ReviewDescription;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }
}
