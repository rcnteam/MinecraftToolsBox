namespace MinecraftCommandsGenerator.Json
{
    public class JsonText
    {
        public static string transfer(string untransferStr)
        {
            if (untransferStr == "") return "";
            else
            {
                string Step1 = untransferStr.Replace("\\","\\\\");//转换‘\’为“\\”
                string Step2 = Step1.Replace("\"","\\\"");//转换‘"’为“\"”
                string result = Step2.Replace("\n","\\n");
                return result;
            }
        }
    }
}