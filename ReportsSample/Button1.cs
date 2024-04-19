using System;
using System.Threading.Tasks;

using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Mapping;

namespace ReportsSample
{
    internal class Button1 : Button
    {
        protected override void OnClick()
        {
            GenerateReport();
        }


        private static async Task GenerateReport()
        {
            try
            {
                FeatureLayer layer = await FeatureClassHelper.CreateFeatureLayer();
                var report = await ReportHelper.CreateReport(layer);
                await ReportHelper.AddFieldsInPageHeaderAsync(report);

                MessageBox.Show("Report created successfuly.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate the report. Error" + ex.Message);
            }
        }
    }
}