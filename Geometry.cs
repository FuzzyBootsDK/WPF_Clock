namespace WPF_CLock;

public class Geometry
{
    public IList<double> coordinates { get; set; }
    public string type { get; set; }
}

public class Properties
{
    public string created { get; set; }
    public string observed { get; set; }
    public string parameterId { get; set; }
    public string stationId { get; set; }
    public double value { get; set; }
}

public class Feature
{
    public Geometry geometry { get; set; }
    public string id { get; set; }
    public string type { get; set; }
    public Properties properties { get; set; }
}

public class Link
{
    public string href { get; set; }
    public string rel { get; set; }
    public string type { get; set; }
    public string title { get; set; }
}

public class Root
{
    public string type { get; set; }
    public IList<Feature> features { get; set; }
    public string timeStamp { get; set; }
    public int numberReturned { get; set; }
    public IList<Link> links { get; set; }
}