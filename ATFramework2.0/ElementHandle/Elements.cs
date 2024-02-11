namespace ATFramework2._0.ElementHandle;

public class Elements
{
    private IEnumerable<IWebElement> _elements;

    public Elements(IEnumerable<IWebElement> elements)
    {
        _elements = elements;
    }

    public int Count()
    {
        return _elements.Count();
    }
}