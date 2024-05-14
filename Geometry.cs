public class Geometry
{
    public string Type { get; set; }
    public List<double> Coordinates { get; set; }
}

public class Properties
{
    public string Created { get; set; }
    public string Observed { get; set; }
    public string ParameterId { get; set; }
    public string StationId { get; set; }
    public double Value { get; set; }
}

public class Feature
{
    public Geometry Geometry { get; set; }
    public string Id { get; set; }
    public string Type { get; set; }
    public Properties Properties { get; set; }
}

public class Link
{
    public string Href { get; set; }
    public string Rel { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
}

public class FeatureCollection
{
    public string Type { get; set; }
    public List<Feature> Features { get; set; }
    public string TimeStamp { get; set; }
    public int NumberReturned { get; set; }
    public List<Link> Links { get; set; }
}