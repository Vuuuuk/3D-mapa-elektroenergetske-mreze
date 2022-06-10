using PZ2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PZ2.Common
{
    public class XmlHelper
    {
        public static void LoadXml(Dictionary<long, PowerEntity> entityCollection, List<LineEntity> linesList, 
                                   List<SubstationEntity> substationList, List<SwitchEntity> switchList, 
                                   List<NodeEntity> nodeeList, double minLon, double minLat, double maxLon, double maxLat)
        {
            double longit = 0;
            double latid = 0;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;
            List<SubstationEntity> subE = new List<SubstationEntity>();
            List<NodeEntity> nodeE = new List<NodeEntity>();
            List<SwitchEntity> switchE = new List<SwitchEntity>();
            List<LineEntity> lineE = new List<LineEntity>();

            var filename = "Geographic.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = System.IO.Path.Combine(currentDirectory, filename);
            StringBuilder result = new StringBuilder();
            XDocument xdoc = XDocument.Load(filename);


            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {

                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                subE.Add(sub); // dodat odmah iz xml 
            }

            for (int i = 0; i < subE.Count(); i++)
            {
                var item = subE[i];
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                if (latid >= minLat && latid <= maxLat && longit >= minLon && longit <= maxLon)
                {
                    subE[i].Latitude = latid;
                    subE[i].Longitude = longit;
                    substationList.Add(subE[i]);
                    entityCollection.Add(subE[i].Id, subE[i]);
                }

            }

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {

                NodeEntity nodeobj = new NodeEntity();
                nodeobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                nodeobj.Name = node.SelectSingleNode("Name").InnerText;
                nodeobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                nodeobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                nodeE.Add(nodeobj);

            }
            for (int i = 0; i < nodeE.Count(); i++)
            {
                var item = nodeE[i];
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                if (latid >= minLat && latid <= maxLat && longit >= minLon && longit <= maxLon)
                {
                    nodeE[i].Latitude = latid;
                    nodeE[i].Longitude = longit;
                    nodeeList.Add(nodeE[i]);
                    entityCollection.Add(nodeE[i].Id, nodeE[i]);
                }

            }

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity switchobj = new SwitchEntity();
                switchobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                switchobj.Name = node.SelectSingleNode("Name").InnerText;
                switchobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                switchobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                switchobj.Status = node.SelectSingleNode("Status").InnerText;

                switchE.Add(switchobj);

            }

            for (int i = 0; i < switchE.Count(); i++)
            {
                var item = switchE[i];
                ToLatLon(item.X, item.Y, 34, out latid, out longit);
                if (latid >= minLat && latid <= maxLat && longit >= minLon && longit <= maxLon)
                {
                    switchE[i].Latitude = latid;
                    switchE[i].Longitude = longit;
                    switchList.Add(switchE[i]);
                    entityCollection.Add(switchE[i].Id, switchE[i]);
                }


            }

            var lines = xdoc.Descendants("LineEntity")
                     .Select(line => new LineEntity
                     {
                         Id = (long)line.Element("Id"),
                         Name = (string)line.Element("Name"),
                         ConductorMaterial = (string)line.Element("ConductorMaterial"),
                         IsUnderground = (bool)line.Element("IsUnderground"),
                         R = (float)line.Element("R"),
                         FirstEnd = (long)line.Element("FirstEnd"),
                         SecondEnd = (long)line.Element("SecondEnd"),
                         LineType = (string)line.Element("LineType"),
                         ThermalConstantHeat = (long)line.Element("ThermalConstantHeat"),
                         Vertices = line.Element("Vertices").Descendants("Point").Select(p => new Point
                         {
                             X = (double)p.Element("X"),
                             Y = (double)p.Element("Y"),
                         }).ToList()
                     }).ToList();

            for (int i = 0; i < lines.Count(); i++)
            {
                if (entityCollection.ContainsKey(lines[i].SecondEnd) && entityCollection.ContainsKey(lines[i].FirstEnd))
                {
                    var line = lines[i];
                    foreach (var point in line.Vertices)
                    {

                        ToLatLon(point.X, point.Y, 34, out latid, out longit);
                        point.Latitude = latid;
                        point.Longitude = longit;

                    }
                    linesList.Add(line);
                }
            }
        }

        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }
}
