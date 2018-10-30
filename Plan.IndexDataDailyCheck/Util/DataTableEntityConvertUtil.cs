namespace Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    /// <summary>
    /// Convert转换
    /// </summary>
    public static class DataTableEntityConvertUtil
    {
        /// <summary>
        /// DataRow转Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableRow"></param>
        /// <returns></returns>
        public static T ConvertToEntity<T>(DataRow tableRow) where T : new()
        {
            if (tableRow == null)
            {
                return default(T);
            }

            Type objType = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in tableRow.Table.Columns)
            {
                PropertyInfo property = objType.GetProperty(
                    column.ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (null == property || !property.CanWrite || null == tableRow[property.Name] || tableRow[property.Name] == DBNull.Value)
                {
                    continue;
                }
                // 类型强转，将table字段类型转为集合字段类型
                try
                {
                    object value = Convert.ChangeType(tableRow[property.Name], property.PropertyType);
                    property.SetValue(obj, value, null);
                }
                catch (Exception ex)
                {
                    Log4NetUtil.WriteErrorLog(ex.ToString());
                    throw;
                }
            }

            return obj;
        }

        /// <summary>
        /// DataTable转List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="isStoreDB"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dt, bool isStoreDB = true) where T : new()
        {
            if (null == dt || dt.Rows.Count == 0)
            {
                return default(List<T>);
            }

            List<T> list = new List<T>();
            Type type = typeof(T);

            // 集合属性数组
            PropertyInfo[] porpertyArray = type.GetProperties();
            foreach (DataRow row in dt.Rows)
            {
                // 新建对象实例
                T entity = Activator.CreateInstance<T>();
                foreach (PropertyInfo p in porpertyArray)
                {
                    if (!dt.Columns.Contains(p.Name) || null == row[p.Name] || row[p.Name] == DBNull.Value)
                    {
                        // DataTable列中不存在集合属性或者字段内容为空则，跳出循环，进行下个循环
                        continue;
                    }

                    if (isStoreDB && p.PropertyType == typeof(DateTime) && Convert.ToDateTime(row[p.Name]) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }

                    try
                    {
                        // 类型强转，将table字段类型转为集合字段类型
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception ex)
                    {
                        Log4NetUtil.WriteErrorLog(ex.ToString());
                        throw;
                    }
                }

                list.Add(entity);
            }

            list.TrimExcess();

            return list;
        }
    }
}