using System.Threading.Tasks;
using System.Collections.Generic;

using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Reports;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Windows.Documents;
using System;
using System.Linq;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;

namespace ReportsSample
{
    internal class ReportHelper
    {
        public static async Task<Report> CreateReport(FeatureLayer featureLayer)
        {
            var listOfFields = new List<CIMReportField> {
                new CIMReportField{ Name = "GroupField" ,Group=true },
                new CIMReportField{ Name = "Id" },
                new CIMReportField{ Name = "Field1" },
                new CIMReportField{ Name = "Field2" },
                new CIMReportField{ Name = "Field3" },
                new CIMReportField{ Name = "Field4" },
                new CIMReportField{ Name = "Field5" },
                new CIMReportField{ Name = "Field6" },
                new CIMReportField{ Name = "Field7" },
                new CIMReportField{ Name = "SummaryField1" },
                new CIMReportField{ Name = "SummaryField2" },
                new CIMReportField{ Name = "SummaryField3" },
                new CIMReportField{ Name = "SummaryField4" },
                new CIMReportField{ Name = "SummaryField5" },
                new CIMReportField{ Name = "SummaryField6" },
                new CIMReportField{ Name = "SummaryField7" },
                new CIMReportField{ Name = "SummaryField8" },
                new CIMReportField{ Name = "SummaryField9" },
            };

            List<ReportFieldStatistic> reportFieldStats = new()
            {
                new ReportFieldStatistic{ Field = "SummaryField1", Statistic = FieldStatisticsFlag.Maximum },
                new ReportFieldStatistic{ Field = "SummaryField2", Statistic = FieldStatisticsFlag.Maximum },
                new ReportFieldStatistic{ Field = "SummaryField3", Statistic = FieldStatisticsFlag.Maximum },
                new ReportFieldStatistic{ Field = "SummaryField4", Statistic = FieldStatisticsFlag.Sum },
                new ReportFieldStatistic{ Field = "SummaryField5", Statistic = FieldStatisticsFlag.Sum },
                new ReportFieldStatistic{ Field = "SummaryField6", Statistic = FieldStatisticsFlag.Sum },
                new ReportFieldStatistic{ Field = "SummaryField7", Statistic = FieldStatisticsFlag.Count },
                new ReportFieldStatistic{ Field = "SummaryField8", Statistic = FieldStatisticsFlag.Sum },
                new ReportFieldStatistic{ Field = "SummaryField9", Statistic = FieldStatisticsFlag.Sum },
            };

            ReportDataSource reportDataSource = new(featureLayer, "", false, listOfFields);

            var reportCIMPage = new CIMPage
            {
                Units = LinearUnit.Inches,
                Height = 11,
                Width = 8.5,
                Margin = new CIMMargin { Bottom = 1, Top = 1, Left = .5, Right = .5 },
            };

            return await QueuedTask.Run(() =>
            {
                var reportTemplate = ReportTemplateManager.GetTemplates().FirstOrDefault(rt => rt.Name == "Attribute List with Grouping");

                return ReportFactory.Instance.CreateReport("New Report", reportDataSource, reportCIMPage, reportFieldStats, reportTemplate, "Black and White");
            });
        }

        //public static void ChangeHeader(Report report)
        //{
        //    if (report == null) return;

        //    List<string> displayNames = new()
        //        {
        //            "Group Header",
        //            "Id Header",
        //            "Field 1 Header",
        //            "Field 2 Header",
        //            "Field 3 Header",
        //            "Field 4 Header",
        //            "Field 5 Header",
        //            "Field 6 Header",
        //            "Field 7 Header",
        //            "Summary Field 1 Header",
        //            "Summary Field 2 Header"
        //    };

        //    for (int i = 0; i < displayNames.Count; i++)
        //    {
        //        var cimField = report.DataSource.Fields.ElementAt(i);
        //        cimField.DisplayName = displayNames[i];
        //        if (i == 0) cimField.Group = true;
        //        if (i == 3 || i == 5 || i == 7)
        //        {
        //            cimField.IsVisible = false;
        //        }
        //    }
        //}

        internal static async Task AddFieldsInPageHeaderAsync(Report report)
        {
            await QueuedTask.Run(() =>
            {
                if (report != null)
                {
                    var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();
                    var reportPageHeader = mainReportSection?.Elements.OfType<ReportPageHeader>().FirstOrDefault();
                    reportPageHeader.SetVisible(true);

                    var textElement = "Field 1: Field1 value";
                    textElement += Environment.NewLine + "Field 2: Field2 value";
                    textElement += Environment.NewLine + "Field 3: Field3 value";

                    var oldEnv = mainReportSection.GetBounds();
                    var newMinPoint = new Coordinate2D(oldEnv.XMin + 0.5, oldEnv.YMin + 0.5);
                    var newMaxPoint = new Coordinate2D(oldEnv.XMax - 0.5, oldEnv.YMax - 0.5);
                    Envelope newElementEnvelope = EnvelopeBuilder.CreateEnvelope(newMinPoint, newMaxPoint);
                    // Create the "RectangleParagraphGraphicElement" graphic.
                    var ElementGraphic = ReportElementFactory.Instance.CreateRectangleParagraphGraphicElement(reportPageHeader, newElementEnvelope, textElement);
                }
            });

        }

        private static void UpdateReport()
        {

        }

    }
}
