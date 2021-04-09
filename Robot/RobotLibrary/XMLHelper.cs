using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RobotLibrary
{
    public class XMLHelper
    {
        public static string Serialize(object o)
        {
            XmlSerializer s = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            try
            {
                s.Serialize(ms, o);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                XmlDocument xml = new XmlDocument();
                // string xmlString = ASCIIEncoding.UTF8.GetString(ms.ToArray());
                xml.Load(ms);
                return xml.OuterXml;
            }
            catch (Exception e)
            {
                //caught = e;
            }
            finally
            {
                //writer.Close();
                //ms.Close();

                //if (caught != null)
                //    throw caught;
            }
            return null;
        }


        public static object Deserialize(XmlDocument xml, Type type)
        {
            XmlSerializer s = new XmlSerializer(type);
            string xmlString = xml.OuterXml.ToString();
            byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(ms);
            Exception caught = null;

            try
            {
                object o = s.Deserialize(reader);
                return o;
            }

            catch (Exception e)
            {
                caught = e;
            }
            finally
            {
                reader.Close();

                if (caught != null)
                    throw caught;
            }
            return null;
        }

        public static object Deserialize(string xml, Type type)
        {
            if(!string.IsNullOrEmpty(xml))
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(xml);
                XmlSerializer s = new XmlSerializer(type);
                string xmlString = dom.OuterXml.ToString();
                byte[] buffer = ASCIIEncoding.UTF8.GetBytes(xmlString);
                MemoryStream ms = new MemoryStream(buffer);
                XmlReader reader = new XmlTextReader(ms);
                Exception caught = null;

                try
                {
                    object o = s.Deserialize(reader);
                    return o;
                }

                catch (Exception e)
                {
                    caught = e;
                }
                finally
                {
                    reader.Close();

                    if (caught != null)
                        throw caught;
                }
            }
            return null;
        }

        public static void Serialize(object o, ref XmlDocument xml )
        {
            XmlSerializer s = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            try
            {
                s.Serialize(ms, o);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                xml = new XmlDocument();
                // string xmlString = ASCIIEncoding.UTF8.GetString(ms.ToArray());
                xml.Load(ms);
            }
            catch (Exception e)
            {
                //caught = e;
            }
            finally
            {
                //writer.Close();
                //ms.Close();

                //if (caught != null)
                //    throw caught;
            }
          
        }
    }
}
