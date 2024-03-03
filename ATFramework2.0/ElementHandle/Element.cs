using ATFramework2._0.Extensions;

namespace ATFramework2._0.ElementHandle;

public class Element
{
    private IWebElement _element;
    public string Text { get; private set; }
    public Element(IWebElement element)
    {
        _element = element;
        Text = _element.Text;
    }

    public void Click()
    {
        _element.Click();
       //_element.PerformActionWithHighlighting(() => _element.Click());
    }
    public void SendKeys(string text)
    {
        _element.SendKeys(text);
       // _element.PerformActionWithHighlighting(() => _element.SendKeys(text));
    }
}