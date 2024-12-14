namespace ATFramework2._0.ElementHandle;

public class Element
{
    public IWebElement webElementCore;
    public string Text { get; private set; }
    public Element(IWebElement element)
    {
        webElementCore = element;
        Text = webElementCore.Text;
    }

    public void Click()
    {
        webElementCore.ScrollToElement();
        webElementCore.PerformActionWithHighlighting(() => webElementCore.Click());
    }

    public void Clear()
    {
        webElementCore.ScrollToElement();
        webElementCore.PerformActionWithHighlighting(() => webElementCore.Clear());
    }

    public void SendKeys(string text)
    {
        webElementCore.ScrollToElement();
       webElementCore.PerformActionWithHighlighting(() => webElementCore.SendKeys(text));
    }
}