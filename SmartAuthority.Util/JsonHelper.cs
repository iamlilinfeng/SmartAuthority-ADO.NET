/*技术支持QQ群：226090960*/
using System.Web.Script.Serialization;

namespace SmartAuthority.Util
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static T ToObject<T>(this string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(json);
        }
    }
}
