namespace Client
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InvokeCallbackAttribute : Attribute
    {
        public InvokeCallbackAttribute(Type type)
        {
        }
    }
}
