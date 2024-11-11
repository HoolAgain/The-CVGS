using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class UserService
    {
        private readonly VaporDbContext _context;

        public UserService(VaporDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllMembers()
        {
            return _context.Users.ToList();
        }

        public byte[] GenerateUserReportExcel(List<User> users)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage()) 
            {
                var worksheet = package.Workbook.Worksheets.Add("User Report");

                worksheet.Cells[1, 1].Value = "Username";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "First Name";
                worksheet.Cells[1, 4].Value = "Last Name";
                worksheet.Cells[1, 5].Value = "Gender";
                worksheet.Cells[1, 6].Value = "Date of Birth";
                worksheet.Cells[1, 7].Value = "Favourite Platform";
                worksheet.Cells[1, 8].Value = "Favourite Category";
                worksheet.Cells[1, 9].Value = "Language Preference";

                for (int i = 0; i < users.Count; i++) 
                {
                    var user = users[i];
                    worksheet.Cells[i + 2, 1].Value = user.UserName;
                    worksheet.Cells[i + 2, 2].Value = user.Email;
                    worksheet.Cells[i + 2, 3].Value = user.FirstName;
                    worksheet.Cells[i + 2, 4].Value = user.LastName;
                    worksheet.Cells[i + 2, 5].Value = user.Gender;
                    worksheet.Cells[i + 2, 6].Value = user.BirthDate?.ToString("yyyy-MM-dd") ?? "";
                    worksheet.Cells[i + 2, 7].Value = user.FavoritePlatform;
                    worksheet.Cells[i + 2, 8].Value = user.FavoriteCategory;
                    worksheet.Cells[i + 2, 9].Value = user.LanguagePreference;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }
}
