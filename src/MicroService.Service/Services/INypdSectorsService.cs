﻿using System.Collections.Generic;
using MicroService.Service.Models;
using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace MicroService.Service.Services
{
    public interface INypdSectorsService
    {
        List<Feature> GetFeatures();

        NypdSectorShape GetFeatureLookup(double x, double y);

        ShapefileHeader GetShapeProperties();

        DbaseFileHeader GetShapeDatabaseProperties();
    }
}
