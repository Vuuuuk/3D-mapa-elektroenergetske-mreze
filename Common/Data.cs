using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PZ2.Common
{
    public class Data
    {

        //LIMITS PER SPECIFICATION

        private double minLon = 19.793909;
        private double minLat = 45.2325;
        private double maxLon = 19.894459;
        private double maxLat = 45.277031;


        //GRUPA 2 DODATNI ZADATAK

        private List<GeometryModel3D> switchGeometryModel = new List<GeometryModel3D>();
        private List<GeometryModel3D> nodeGeometryModel = new List<GeometryModel3D>();
        private List<GeometryModel3D> substationsGeometryModel = new List<GeometryModel3D>();

        //GRUPA 2 DODATNI ZADATAK


        private List<LineEntity> linesList = new List<LineEntity>();
        private List<NodeEntity> nodeList = new List<NodeEntity>();
        private List<SubstationEntity> substationList = new List<SubstationEntity>();
        private List<SwitchEntity> switchList = new List<SwitchEntity>();

        private Dictionary<long, PowerEntity> entityCollection = new Dictionary<long, PowerEntity>();

        Dictionary<GeometryModel3D, PowerEntity> entityGeo = new Dictionary<GeometryModel3D, PowerEntity>();
        Dictionary<GeometryModel3D, LineEntity> lineGeo = new Dictionary<GeometryModel3D, LineEntity>();

        Dictionary<GeometryModel3D, long> allEntities = new Dictionary<GeometryModel3D, long>();
        Dictionary<GeometryModel3D, long> allLines = new Dictionary<GeometryModel3D, long>();

        //USED FOR COUNTING HOW MUCH ENTITIES ARE IN THE SAME POINT
        private Dictionary<System.Windows.Point, int> numberOfEntityOnPoint = new Dictionary<System.Windows.Point, int>();

        private List<GeometryModel3D> lineGeometryModel = new List<GeometryModel3D>();

        public Data()
        {
            XmlHelper.LoadXml(EntityCollection, LinesList, SubstationList, SwitchList, NodeList, minLon, minLat, maxLon, maxLat);
        }

        public List<LineEntity> LinesList { get => linesList; set => linesList = value; }
        public List<NodeEntity> NodeList { get => nodeList; set => nodeList = value; }
        public List<SubstationEntity> SubstationList { get => substationList; set => substationList = value; }
        public List<SwitchEntity> SwitchList { get => switchList; set => switchList = value; }
        public Dictionary<long, PowerEntity> EntityCollection { get => entityCollection; set => entityCollection = value; }
        public Dictionary<GeometryModel3D, PowerEntity> EntityGeo { get => entityGeo; set => entityGeo = value; }
        public Dictionary<GeometryModel3D, LineEntity> LineGeo { get => lineGeo; set => lineGeo = value; }
        public Dictionary<GeometryModel3D, long> AllEntities { get => allEntities; set => allEntities = value; }
        public Dictionary<GeometryModel3D, long> AllLines { get => allLines; set => allLines = value; }
        public Dictionary<System.Windows.Point, int> NumberOfEntityOnPoint { get => numberOfEntityOnPoint; set => numberOfEntityOnPoint = value; }
        public double MinLon { get => minLon; set => minLon = value; }
        public double MinLat { get => minLat; set => minLat = value; }
        public double MaxLon { get => maxLon; set => maxLon = value; }
        public double MaxLat { get => maxLat; set => maxLat = value; }
        public List<GeometryModel3D> LineGeometryModel { get => lineGeometryModel; set => lineGeometryModel = value; }
        public List<GeometryModel3D> SwitchGeometryModel { get => switchGeometryModel; set => switchGeometryModel = value; }
        public List<GeometryModel3D> NodeGeometryModel { get => nodeGeometryModel; set => nodeGeometryModel = value; }
        public List<GeometryModel3D> SubstationsGeometryModel { get => substationsGeometryModel; set => substationsGeometryModel = value; }
    }
}
