using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class WishlistService
    {
        private readonly VaporDbContext _context;

        public WishlistService(VaporDbContext context)
        {
            _context = context;
        }

        public List<WishlistReport> GetWishlistReportData()
        {
            return _context.Wishlist
                .Include(w => w.User)
                .Include(w => w.Game)
                .Select(w => new WishlistReport
                {
                    UserName = w.User.UserName,
                    GameName = w.Game.Name
                })
                .ToList();
        }

        public byte[] GenerateWishlistReportExcel(List<WishlistReport> wishlist) 
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Wishlist Report");

                worksheet.Cells[1, 1].Value = "Member Username";
                worksheet.Cells[1, 2].Value = "Game Name";

                for (int i = 0; i < wishlist.Count; i++)
                {
                    var wish = wishlist[i];
                    
                    worksheet.Cells[i + 2, 1].Value = wish.UserName;
                    worksheet.Cells[i + 2, 2].Value = wish.GameName;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }
    }
}
