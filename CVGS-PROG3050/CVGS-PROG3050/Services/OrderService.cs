using CVGS_PROG3050.DataAccess;
using Microsoft.EntityFrameworkCore;
using CVGS_PROG3050.Models;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class OrderService
    {
        private readonly VaporDbContext _context;

        public OrderService (VaporDbContext context)
        {
            _context = context;
        }

        public List<OrderReport> GetOrderReportData()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Game)
                .Select(o => new OrderReport
                {
                    OrderDate = o.OrderDate,
                    UserName = o.User.UserName,
                    GameName = o.Game.Name,
                    SubTotal = o.Subtotal,
                    Tax = o.Tax,
                    GrandTotal = o.GrandTotal,
                    Status = o.Status,
                    ShipPhysicalCopy = o.ShipPhysicalCopy ? "Yes" : "No",
                    ShippingCost = o.ShippingCost
                })
                .ToList();
        }

        public byte[] GenerateOrderReportExcel(List<OrderReport> orders)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Order Report");

                worksheet.Cells[1, 1].Value = "Order Date";
                worksheet.Cells[1, 2].Value = "Member Username";
                worksheet.Cells[1, 3].Value = "Game Purchased";
                worksheet.Cells[1, 4].Value = "Subtotal";
                worksheet.Cells[1, 5].Value = "Tax";
                worksheet.Cells[1, 6].Value = "Grand Total";
                worksheet.Cells[1, 7].Value = "Status";
                worksheet.Cells[1, 8].Value = "Shipped Physical Copy";
                worksheet.Cells[1, 9].Value = "Shipping Cost";

                for (int i = 0; i < orders.Count; i++)
                {
                    var order = orders[i];
                    worksheet.Cells[i + 2, 1].Value = order.OrderDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 2].Value = order.UserName;
                    worksheet.Cells[i + 2, 3].Value = order.GameName;
                    worksheet.Cells[i + 2, 4].Value = order.SubTotal;
                    worksheet.Cells[i + 2, 5].Value = order.Tax;
                    worksheet.Cells[i + 2, 6].Value = order.GrandTotal;
                    worksheet.Cells[i + 2, 7].Value = order.Status;
                    worksheet.Cells[i + 2, 8].Value = order.ShipPhysicalCopy;
                    worksheet.Cells[i + 2, 9].Value = order.ShippingCost;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }

        }
    }
}
