using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectTreeView
{
    public static class ObjectTreeView
    {
        public static string TreeView(this object item, BindingFlags bindingFlags = BindingFlags.Default, TreeMembers treeMembers = TreeMembers.Default )
        {
            var res = new StringBuilder();
            if (item is IEnumerable)
            {
                return "[]";
            }
            else
            {
                var itemType = item.GetType();

                if ((treeMembers | TreeMembers.Fields) == TreeMembers.Fields)
                {
                    var fields = itemType.GetFields().ToList();
                    fields.Sort((x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    res.AppendLine("Поля:");
                    foreach (var field in fields)
                    {
                        res.AppendLine(String.Format("\t{0}:\t{1}", field.Name, field.GetValue(item) ?? "<null>"));
                    }
                }

                if ((treeMembers | TreeMembers.Properties) == TreeMembers.Properties)
                {
                    var properties = itemType.GetProperties().ToList();
                    properties.Sort((x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    res.AppendLine("Свойства:");
                    foreach (var property in properties)
                    {
                        res.AppendLine(String.Format("\t{0}:\t{1}", property.Name, property.GetValue(item) ?? "<null>"));
                    }
                }

                if ((treeMembers | TreeMembers.Methods) == TreeMembers.Methods)
                {
                    var methods = itemType.GetMethods().ToList();
                    methods.Sort((x, y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    res.AppendLine("Методы:");
                    foreach (var method in methods)
                    {
                        res.AppendLine(String.Format("\t{0}\t[{1}]", method.Name, method.ReturnType.FullName));
                    }
                }

                return res.ToString();
            }
        }

        [Flags]
        public enum TreeMembers
        {
            Fields,
            Properties,
            Methods,
            Events,
            Operators,
            Indexers,
            Constructors,
            Destructors,
            Default = Fields | Properties
        }
    }
}