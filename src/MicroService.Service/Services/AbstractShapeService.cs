﻿using System.Collections.Generic;
using MicroService.Service.Helpers;
using MicroService.Service.Models.Base;
using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace MicroService.Service.Services
{
    public delegate IShapefileDataReaderService ShapefileDataReaderResolver(string key);

    public abstract class AbstractShapeService<T>
    {
        public IShapefileDataReaderService ShapeFileDataReader { get; internal set; }

        public ShapefileHeader GetShapeProperties()
        {
            ShapefileHeader shpHeader = ShapeFileDataReader.ShapeHeader;
            return shpHeader;
        }

        public DbaseFileHeader GetShapeDatabaseProperties()
        {
            DbaseFileHeader header = ShapeFileDataReader.DbaseHeader;
            return header;
        }

        public abstract T GetFeatureLookup(double x, double y);

        public ShapeBase GetShapeBaseFeatureLookup(double x, double y)
        {
            var shape = new ShapeBase();
            return shape;
        }

        public IReadOnlyCollection<Feature> GetFeatures() =>
            ShapeFileDataReader.GetFeatures();
    }
}
