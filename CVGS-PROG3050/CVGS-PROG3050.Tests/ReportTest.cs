using CVGS_PROG3050.Models;
using CVGS_PROG3050.Services;

namespace CVGS_PROG3050.Tests
{
    public class ReportTest
    {
        [Fact]
        public async Task FormatDateCorrectly()
        {
            // Arrange
            var eventReport = new EventReport
            {
                EventName = "Test",
                EventDate = new DateTime(2024, 11, 9),
                UserCount = 3
            };

            // Act
            var formattedDate = eventReport.EventDate.ToString("yyyy-MM-dd");

            // Assert
            Assert.Equal("2024-11-09", formattedDate);
        }

        [Fact]
        public void ShouldHandleEmptyList()
        {
            // Arrange
            var emptyEventList = new List<EventReport>();
            var reportService = new EventService(null);

            // Act
            var fileContent = reportService.GenerateEventReportExcel(emptyEventList);

            // Assert
            Assert.NotNull(fileContent);
            Assert.True(fileContent.Length > 0);
        }
    }
}
