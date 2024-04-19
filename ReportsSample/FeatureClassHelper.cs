using System.Threading.Tasks;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Collections.Generic;
using System;
using ArcGIS.Desktop.Editing;
using ArcGIS.Core.Data;

namespace ReportsSample
{
    internal class FeatureClassHelper
    {

        public static async Task<FeatureLayer> CreateFeatureLayer()
        {
            var layerName = "ReportTestFeatureClass";

            return await QueuedTask.Run(async () =>
            {
                List<object> arguments = new()
                {
                    Project.Current.DefaultGeodatabasePath,
                    layerName,
                    GeometryType.Point.ToString(),
                    "",
                    "ENABLED",
                    "ENABLED",
                    MapView.Active.Map.SpatialReference
                };

                var environment = Geoprocessing.MakeEnvironmentArray(overwriteoutput: null);
                var result = await Geoprocessing.ExecuteToolAsync("management.CreateFeatureClass", Geoprocessing.MakeValueArray(arguments.ToArray()), environment);

                if (!result.IsFailed)
                {
                    var layers = MapView.Active.Map.GetLayersAsFlattenedList();

                    foreach (var layer in layers)
                    {
                        if (layer != null && layer is FeatureLayer featureLayer && layer.Name == layerName)
                        {
                            if (await AddFields(featureLayer))
                            {
                                await AddDataInTempDataLayer(featureLayer);
                                return featureLayer;
                            }
                        }
                    }
                }

                return null;
            });
        }

        public static async Task<bool> AddFields(FeatureLayer layer)
        {
            if (layer == null) return false;

            List<string> fieldDescriptions = new()
            {
                "Id" + " " + "TEXT",
                "GroupField" + " " + "TEXT",
                "Field1" + " " + "TEXT Header1",
                "Field2" + " " + "TEXT Header2",
                "Field3" + " " + "TEXT Header3",
                "Field4" + " " + "TEXT Header4",
                "Field5" + " " + "TEXT Header5",
                "Field6" + " " + "TEXT Header6",
                "Field7" + " " + "TEXT Header7",
                "SummaryField1" + " " + "TEXT Header8",
                "SummaryField2" + " " + "TEXT Header9",
                "SummaryField3" + " " + "TEXT Header10",
                "SummaryField4" + " " + "TEXT Header11",
                "SummaryField5" + " " + "TEXT Header12",
                "SummaryField6" + " " + "TEXT Header13",
                "SummaryField7" + " " + "TEXT Header14",
                "SummaryField8" + " " + "TEXT Header15",
                "SummaryField9" + " " + "TEXT Header16",
            };

            GPExecuteToolFlags gpFlags = GPExecuteToolFlags.None;

            return await QueuedTask.Run(async () =>
            {
                var arguments = new List<object>() { layer, String.Join(";", fieldDescriptions) };
                var valueArray = Geoprocessing.MakeValueArray(arguments.ToArray());

                var result = await Geoprocessing.ExecuteToolAsync("management.AddFields", valueArray, null, null, null, gpFlags);

                return !result.IsFailed;
            });
        }

        private static async Task AddDataInTempDataLayer(FeatureLayer fLayer)
        {
            List<List<string>> data = new()
            {
                new List<string>(){"1", "1","1", "1", "1", "1" , "1", "1", "1" , "1", "Suhas1",
                    "summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
                new List<string>(){"2", "1","2", "2", "2", "2" , "2", "2", "2" , "2","Suhas1",
                "summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
                new List<string>(){"3", "1","3", "3", "3", "3" , "3", "3", "3" , "3", "Suhas1","summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
                new List<string>(){"4", "2","4", "4", "4", "4" , "4", "4", "4" , "4", "Suhas 2","summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
                new List<string>(){"5", "2","5", "5", "5", "5" , "5", "5", "5" , "5", "Suhas 2","summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
                new List<string>(){"6", "3","6", "6", "6", "6" , "6", "6", "6" , "6", "Suhas 3","summary 3",
                    "summary 4",
                    "summary 5",
                    "summary 6",
                    "summary 7",
                    "summary 8",
                    "summary 9"},
            };

            await QueuedTask.Run(() =>
            {
                EditOperation editOperation = new();
                editOperation.Callback(context =>
                {
                    using RowBuffer rowBuffer = fLayer.GetFeatureClass().CreateRowBuffer();
                    for (int i = 0; i < data.Count; i++)
                    {

                        rowBuffer["Id"] = data[i][0];
                        rowBuffer["GroupField"] = data[i][1];
                        rowBuffer["Field1"] = data[i][2];
                        rowBuffer["Field2"] = data[i][3];
                        rowBuffer["Field3"] = data[i][4];
                        rowBuffer["Field4"] = data[i][5];
                        rowBuffer["Field5"] = data[i][6];
                        rowBuffer["Field6"] = data[i][7];
                        rowBuffer["Field7"] = data[i][8];
                        rowBuffer["SummaryField1"] = data[i][9];
                        rowBuffer["SummaryField2"] = data[i][10];
                        rowBuffer["SummaryField3"] = data[i][11];
                        rowBuffer["SummaryField4"] = data[i][12];
                        rowBuffer["SummaryField5"] = data[i][13];
                        rowBuffer["SummaryField6"] = data[i][14];
                        rowBuffer["SummaryField7"] = data[i][15];
                        rowBuffer["SummaryField8"] = data[i][16];
                        rowBuffer["SummaryField9"] = data[i][17];

                        using Row firstFeature = fLayer.GetFeatureClass().CreateRow(rowBuffer);
                        context.Invalidate(firstFeature);
                    }
                }, fLayer.GetFeatureClass());

                var creationResult = editOperation.Execute();
                if (!creationResult)
                {
                    throw new Exception("Could not add row");
                }
            });
        }

    }
}
