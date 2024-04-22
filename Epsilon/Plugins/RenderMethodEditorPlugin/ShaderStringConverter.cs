namespace RenderMethodEditorPlugin
{
    static class ShaderStringConverter
    {
        public static string ToPrettyFormat(string input)
        {
            string result = input.Replace("_", " ");
            result = result.ToLower();
            return result;
        }

        public static string FromPrettyFormat(string input)
        {
            string result = input.Replace(" ", "_");
            result = result.ToLower();
            return result;
        }
    }
}
