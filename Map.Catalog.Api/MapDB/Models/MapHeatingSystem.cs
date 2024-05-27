using System;
using System.Collections.Generic;

namespace Map.Catalog.Api.MapDB.Models;

public partial class MapHeatingSystem
{
    public string? SourceMap { get; set; }

    public string? DestinationMap { get; set; }

    public string? SourcePath { get; set; }

    public string? DestinationPath { get; set; }
}
