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
                // Create FC, add data into it to prepare datassource for report 
                FeatureLayer layer = await FeatureClassHelper.CreateFeatureLayer();

                // Create new report using FeatureLayer as datasource
                var report = await ReportHelper.CreateReport(layer);

                // Adding element in section 
                
                await ReportHelper.TestCreateTextElement(report);

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