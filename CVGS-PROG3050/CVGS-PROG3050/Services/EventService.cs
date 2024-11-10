using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using OfficeOpenXml;

namespace CVGS_PROG3050.Services
{
    public class EventService
    {
        private readonly VaporDbContext _context;

        public EventService(VaporDbContext context)
        {
            _context = context;
        }

        public List<EventReport> GetEventReportData()
        {
            return _context.Events
                .Select(e => new EventReport
                {
                    EventName = e.EventName,
                    EventDate = e.EventDate,
                    Location = e.Location,
                    LocationType = e.LocationType,
                    Description = e.Description,
                    EventPrice = e.EventPrice,
                    UserCount = e.UserEvents.Count
                })
                .ToList();
        }

        public byte[] GenerateEventReportExcel(List<EventReport> events) 
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Event Report");

                worksheet.Cells[1, 1].Value = "Event Name";
                worksheet.Cells[1, 2].Value = "Event Date";
                worksheet.Cells[1, 3].Value = "Location";
                worksheet.Cells[1, 4].Value = "Location Type";
                worksheet.Cells[1, 5].Value = "Description";
                worksheet.Cells[1, 6].Value = "Event Price";
                worksheet.Cells[1, 7].Value = "User Count";

                for (int i = 0; i < events.Count; i++)
                {
                    var ev = events[i];
                    worksheet.Cells[i + 2, 1].Value = ev.EventName;
                    worksheet.Cells[i + 2, 2].Value = ev.EventDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 3].Value = ev.Location;
                    worksheet.Cells[i + 2, 4].Value = ev.LocationType;
                    worksheet.Cells[i + 2, 5].Value = ev.Description;
                    worksheet.Cells[i + 2, 6].Value = ev.EventPrice;
                    worksheet.Cells[i + 2, 7].Value = ev.UserCount;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }  
        }
    }
}
