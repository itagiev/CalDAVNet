using System.Xml.Linq;

namespace CalDAVNet;

public class PropertyUpdateBuilder
{
    private readonly XElement _propsToSet = new XElement(XNames.Prop);
    private readonly XElement _propsToRemove = new XElement(XNames.Prop);

    public XDocument Build()
    {
        var propertyUpdate = new XElement(XNames.PropertyUpdate,
            new XAttribute(XNamespace.Xmlns + Constants.DAV.Prefix, Constants.DAV.Namespace),
            new XAttribute(XNamespace.Xmlns + Constants.Cal.Prefix, Constants.Cal.Namespace),
            new XElement(XNames.Set, _propsToSet),
            new XElement(XNames.Remove, _propsToRemove));

        return new XDocument(new XDeclaration("1.0", "UTF-8", null), propertyUpdate);
    }

    public PropertyUpdateBuilder AddProp(XElement prop)
    {
        _propsToSet.Add(prop);
        return this;
    }

    public PropertyUpdateBuilder RemoveProp(XElement prop)
    {
        _propsToRemove.Add(prop);
        return this;
    }

    public PropertyUpdateBuilder AddDisplayName(string displayName)
    {
        return AddProp(new XElement(XNames.DisplayName, displayName));
    }

    public PropertyUpdateBuilder RemoveDisplayName()
    {
        return RemoveProp(new XElement(XNames.DisplayName));
    }

    public PropertyUpdateBuilder AddDescription(string description)
    {
        return AddProp(new XElement(XNames.CalendarDescription, description));
    }

    public PropertyUpdateBuilder RemoveDescription()
    {
        return RemoveProp(new XElement(XNames.CalendarDescription));
    }

    public PropertyUpdateBuilder AddColor(string color)
    {
        return AddProp(new XElement(XNames.CalendarColor, color));
    }

    public PropertyUpdateBuilder RemoveColor()
    {
        return RemoveProp(new XElement(XNames.CalendarColor));
    }
}
