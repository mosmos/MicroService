﻿using System.ComponentModel;

namespace MicroService.Service.Models.Enum
{
    public enum ShapeProperties
    {
        [Description("Borough Boundaries")]
        [ShapeAttributes("Borough_Boundaries", "nybb")]
        BoroughBoundaries,

        [Description("Historic Districts")]
        [ShapeAttributes("Historic_Districts", "Historic_Districts")]
        HistoricDistricts,

        [Description("NYPD Police Precincts")]
        [ShapeAttributes("NYPD_Police_Precincts", "nypp")]
        NypdPolicePrecincts,

        [Description("NYPD Sectors")]
        [ShapeAttributes("NYPD_Sectors", "NYPD_Sectors")]
        NypdSectors,

    }
}