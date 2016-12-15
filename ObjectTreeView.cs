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
            if (item is IEnumerable)
            {
                var res = new List<string>();
                foreach (var i in (IEnumerable) item)
                {
                    res.Add(treeViewObj(i, bindingFlags, treeMembers));
                }
                return "[" + Environment.NewLine +
                       String.Join("," + Environment.NewLine, res) + Environment.NewLine +
                       "]";
            }
            return treeViewObj(item, bindingFlags, treeMembers);
        }

        private static string treeViewObj(object item, BindingFlags bindingFlags, TreeMembers treeMembers)
        {
            var res = new StringBuilder();

            var itemType = item.GetType();
            res.AppendLine(item.ToString());
            res.AppendLine("{");

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
            res.Append("}");
            return res.ToString();
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