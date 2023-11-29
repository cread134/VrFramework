namespace Core.DI
{
    public class DependencyConfiguration
    {
        public DependencyConfiguration(ConstructorConfiguration contructorConfiguration, ComponentConfiguration componentConfiguration) 
        {
            ContructorConfiguration = contructorConfiguration;
            ComponentConfiguration = componentConfiguration;
        }

        public ConstructorConfiguration ContructorConfiguration { get; }
        public ComponentConfiguration ComponentConfiguration { get; }
    }

    public class ConstructorConfiguration
    {
        public static ConstructorConfiguration Empty = new ConstructorConfiguration();
    }

    public class ComponentConfiguration
    {
        public static ComponentConfiguration Empty = new ComponentConfiguration();
    }
}