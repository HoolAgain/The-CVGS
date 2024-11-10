using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class GameService
    {
        private readonly VaporDbContext _context;

        public GameService(VaporDbContext context)
        {
            _context = context;
        }

        // retrieves all games from the database
        public List<Game> GetAllGames()
        {
            return _context.Games.ToList();
        }

        // Method to generate the Excel report from the list of games
        public byte[] GenerateGameReportExcel(List<Game> games)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Game Report");

                // Defining headers
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Genre";
                worksheet.Cells[1, 3].Value = "Release Date";
                worksheet.Cells[1, 4].Value = "Developer";
                worksheet.Cells[1, 5].Value = "Publisher";
                worksheet.Cells[1, 6].Value = "Description";
                worksheet.Cells[1, 7].Value = "Price";

                // Populating rows with game data
                for (int i = 0; i < games.Count; i++)
                {
                    var game = games[i];
                    worksheet.Cells[i + 2, 1].Value = game.Name;
                    worksheet.Cells[i + 2, 2].Value = game.Genre;
                    worksheet.Cells[i + 2, 3].Value = game.ReleaseDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 4].Value = game.Developer;
                    worksheet.Cells[i + 2, 5].Value = game.Publisher;
                    worksheet.Cells[i + 2, 6].Value = game.Description;
                    worksheet.Cells[i + 2, 7].Value = game.Price;
                }

                // Adjust column widths
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Returning the Excel file as a byte array
                return package.GetAsByteArray();
            }
        }
    }
}
